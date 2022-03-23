using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovingWall;
using manageGame;

public class WallSpawner : MonoBehaviour
{
    public float defaultVelocity = 1;
    //public float songBPM;
    public float defaultTime = 5;
    public GameObject wallPrefab;
    public GameObject alarmAudio;
    GameObject currentWall;
    public bool spawnWait = false;
    Transform spawnLocation;
    int wallLayer = 6;

    AudioSource alarmSource;

    private int seed;

    gameManager m_gameManager;

    //Wall list containing type and vec2 with position offset and rotation along the forward axis;
    KeyValueList<string, Vector2> wallType = new KeyValueList<string, Vector2>
        {
            { "Front", new Vector2(0f, 0f)},
            { "Right", new Vector2(90f, 0f)},
            { "Left", new Vector2(-90f, 0f)},
            {"FrontRight", new Vector2(45f, 0f)},
            {"FrontLeft", new Vector2(-45f, 0f)},
            {"BackRight", new Vector2(135f, 0f)},
            {"BackLeft", new Vector2(-135f, 0f)},
            { "Back", new Vector2(180f, 0f)}

        };

    void Start()
    {
        m_gameManager = GameObject.Find("gameManager").GetComponent<gameManager>();

        spawnLocation = transform.Find("SpawnLocation");
        alarmSource = alarmAudio.GetComponent<AudioSource>();
        //Debug.Log(wallType[seed].Key);
    }
    void Update()
    {
        if(m_gameManager.isRunning)
        {
            if (!spawnWait)
            {
                seed = Random.Range(0, wallType.Count);
                StartCoroutine(SpawnWall(m_gameManager.difficulty, defaultVelocity, defaultTime, seed));
            }
        }
    }

    public IEnumerator SpawnWall(float difficulty, float defaultVelocity, float defaultTime, int seed)
    {
        //flag
        spawnWait = true;

        float spawnSpeed = defaultVelocity += difficulty;//Mathf.Pow(1.1f*difficulty,2);
        float spawnTime = (1 / (difficulty)) * defaultTime;

        spawnAtRotation(seed, spawnLocation);

        currentWall.transform.parent = transform;
        //Set Layers
        currentWall.layer = wallLayer;
        foreach (Transform child in currentWall.transform)
            child.gameObject.layer = wallLayer;

        //Set Velocity
        currentWall.GetComponent<WallScript>().velocity = spawnSpeed;

        yield return new WaitForSeconds(spawnTime);
        //flag
        spawnWait = false;
    }


    void spawnAtRotation(int seed, Transform spawnLocation)
    {
        Vector3 oldPos = spawnLocation.position;
        Quaternion oldRot = spawnLocation.rotation;

        spawnLocation.RotateAround(transform.position, transform.up, wallType[seed].Value[0]);

        currentWall = Instantiate(wallPrefab, spawnLocation.position, spawnLocation.rotation);
        Destroy(Instantiate(alarmAudio, spawnLocation.position, spawnLocation.rotation),3);

        spawnLocation.rotation = oldRot;
        spawnLocation.position = oldPos;
    }

    private void OnDrawGizmos()
    {
        spawnLocation = transform.Find("SpawnLocation");
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawWireSphere(spawnLocation.position, 0.3f);
    }
}

public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
{
    public void Add(TKey key, TValue value)
    {
        Add(new KeyValuePair<TKey, TValue>(key, value));
    }
}
