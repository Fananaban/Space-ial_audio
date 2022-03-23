using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovingWall;
using manageGame;

public class WallDestroyer : MonoBehaviour
{
    public float capsuleRadius;
    public float capsuleHeight;
    LayerMask m_layermask;
    Vector3 offsetVector;
    public float destructionTime = 5f;

    public GameObject failAudio;
    gameManager m_gameManager;

    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        offsetVector = new Vector3(0, capsuleHeight, 0);
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offsetVector, capsuleRadius);
        Gizmos.DrawWireSphere(transform.position - offsetVector, capsuleRadius);

    }
    void Start()
    {
        m_gameManager = GameObject.Find("gameManager").GetComponent<gameManager>();
        m_layermask = LayerMask.GetMask("Walls");
        offsetVector = new Vector3(0, capsuleHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        checkWall();
    }

    void checkWall()
    {
        Collider[] wallColliders = Physics.OverlapCapsule(transform.position + offsetVector, transform.position - offsetVector, capsuleRadius, m_layermask);
        if(wallColliders.Length != 0)
        {
            //Debug.Log("FOUND WALL");
            foreach (Collider collider in wallColliders)
            {
                if (collider.gameObject.GetComponent<WallScript>().isDestroy == false)
                {
                    m_gameManager.missilesReceived++;
                    collider.gameObject.GetComponent<WallScript>().isDestroy = true;
                    //Debug.Log(collider);
                    Destroy(collider.gameObject, destructionTime);
                    Destroy(Instantiate(failAudio, transform), 3);
                }
            }
        }
    }
}
