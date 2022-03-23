using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using manageGame;
using MovingWall;

public class DestroyMissile : MonoBehaviour
{
    public Vector3 shieldSize = new Vector3(5f, 5f, 1f);
    LayerMask m_layermask;

    public GameObject successAudio;

    public gameManager m_gameManager;

    public Material shieldShader;
    public float[] shieldHitFactor = { 0.0f, 2.0f };
    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.Find("gameManager").GetComponent<gameManager>();
        m_layermask = LayerMask.GetMask("Walls");
    }

    // Update is called once per frame
    void Update()
    {
        checkMissile();
    }

    void checkMissile()
    {
        Collider[] wallColliders = Physics.OverlapBox(transform.position, shieldSize / 2, transform.rotation, m_layermask);
        if (wallColliders.Length != 0)
        {
            //Debug.Log("FOUND WALL");
            foreach (Collider collider in wallColliders)
            {
                if(collider.gameObject.GetComponent<WallScript>().isDestroy == false)
                {
                    GameObject hitObject = collider.gameObject;

                    RaycastHit hit;
                    Physics.Raycast(hitObject.transform.position, hitObject.transform.forward, out hit, Mathf.Infinity);
                    StartCoroutine(shieldCollision(shieldHitFactor[0], shieldHitFactor[1], hit.point, shieldShader));
                    m_gameManager.missilesBlocked++;
                    //Debug.Log(collider);
                    Destroy(collider.gameObject);
                    Destroy(Instantiate(successAudio, transform), 3);
                }
            }
        }
    }

    IEnumerator shieldCollision(float start, float end, Vector3 collisionPoint, Material shieldShader) 
    {
        shieldShader.SetVector("hitPoint", collisionPoint);
        float factor = start;
        while(factor <= end)
        {
            shieldShader.SetFloat("hitFactor", factor);
            factor += 0.07f;
            yield return new WaitForSeconds(0.02f);
        }
        factor = start;
    }

    void OnDrawGizmos()
    {
        //Gizmos.matrix = transform.worldToLocalMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, shieldSize);

    }
}
