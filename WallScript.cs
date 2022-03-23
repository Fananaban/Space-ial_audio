 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extraMath;

namespace MovingWall
{
    public class WallScript : MonoBehaviour
    {
        public bool isDestroy;
        public float velocity = 0;

        public float minLowpass = 60f;
        public float maxLowpass = 5000f;
        public enum Rolloff
        {
            linear,
            logarithmic,
            inverseLog
        }

        public Rolloff m_rolloff;

        Vector3 moveDirection;
        Transform spawnLocation;
        Transform player;

        Transform m_audioSource;
        AudioLowPassFilter lowPassFilter;

        float playerDist;
        float spawnerDist;
        float distanceFactor;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("[CameraRig]").transform;
            spawnLocation = GameObject.Find("SpawnLocation").transform;

            spawnerDist = Vector3.Distance(player.position, spawnLocation.position);

            moveDirection = transform.forward;

            m_audioSource = transform.Find("Audio Source");

            lowPassFilter = m_audioSource.GetComponent<AudioLowPassFilter>();
        }

        // Update is called once per frame
        void Update()
        {
            playerDist = Vector3.Distance(transform.position, player.position);

            distanceFactor = calculateDistanceFactor(playerDist, spawnerDist, m_rolloff);

            StartCoroutine(AdjustLowpass(lowPassFilter, distanceFactor));
            transform.Translate(moveDirection*velocity*Time.deltaTime, Space.World);
        }

        IEnumerator AdjustLowpass(AudioLowPassFilter filter, float distanceFactor)
        {
            filter.cutoffFrequency = mathExtras.Map(distanceFactor, 0, 1, minLowpass, maxLowpass);
            yield return new WaitForSeconds(0.03f);
        }

        float calculateDistanceFactor(float playerDist, float maxDist, Rolloff rolloff, float minFactor = 0f, float maxFactor = 1f)
        {
            float distanceFactor = 0f;
            float baseFactor = (playerDist / maxDist);
            switch (rolloff)
            {
                case Rolloff.linear:
                    distanceFactor = mathExtras.Map(baseFactor, 0,1,1,0);
                    break;

                case Rolloff.logarithmic:
                    distanceFactor = Mathf.Log10(mathExtras.Map(baseFactor, 0, 1, 10, 1));
                    break;

                case Rolloff.inverseLog:
                    distanceFactor = 1- Mathf.Log10(mathExtras.Map(baseFactor, 0, 1, 1, 10));
                    break;
            }
            return mathExtras.Map(distanceFactor, 0, 1, minFactor, maxFactor);
        }
    }
}




