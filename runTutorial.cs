using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using manageGame;


public class runTutorial : MonoBehaviour
{
    public AudioClip[] voiceLines;
    public AudioClip[] voiceLeftRight;

    public Transform voice;
    AudioSource voiceSource;

    gameManager m_gameManager;
    Transform player;
    WallSpawner wallSpawner;

    bool isTraining;



    IEnumerator tutorial()
    {
        yield return new WaitForSeconds(2f);
        voiceSource.clip = voiceLines[0];
        StartCoroutine(lerpPosition(voice.position, new Vector3(voice.position.x,voice.position.y,transform.position.z), voice, voiceLines[0].length));
        voiceSource.Play();
        //Good morning captain
        //A fleet of enemy ships is closing in, and we will be surrounded imminently.
        yield return new WaitForSeconds(voiceLines[0].length + 2f);

        voiceSource.clip = voiceLines[1];
        StartCoroutine(lerpRotation(-360f,transform, voice, voiceLines[1].length, 1000));
        //They think that because they’re invisible, they can fool us.
        //It’s funny how they underestimate our blue shield and you, captain.
        voiceSource.Play();
        yield return new WaitForSeconds(voiceLines[1].length + 1f);
        
        voiceSource.clip=voiceLines[2];
        //We had better get started. I will help you with this one.
        voiceSource.Play();
        yield return new WaitForSeconds(voiceLines[2].length + 1f);


        //Rotate to Spawn Angle
        voice.RotateAround(transform.position, transform.up, -90f - 135f);
        voiceSource.clip = voiceLines[3];
        //Can you face where my voice is coming from?
        voiceSource.Play();
        yield return new WaitForSeconds(voiceLines[3].length);

        isTraining = true;
        StartCoroutine(leftRight(player,voice.right));
        yield return new WaitWhile(() => isTraining);

        voiceSource.clip = voiceLines[4];
        voiceSource.Play();
        //Brace for impact!
        yield return new WaitForSeconds(voiceLines[4].length + 0.1f);
        
        StartCoroutine(wallSpawner.SpawnWall(1, wallSpawner.defaultVelocity, wallSpawner.defaultTime, 6));
        yield return new WaitForSeconds(6f);

        voiceSource.clip = voiceLines[5];
        //Block the missiles by facing the direction they’re coming from.
        //You must survive for as long as possible to give as much of your crew time to evacuate.
        //Are you Ready? Good Luck
        voiceSource.Play();
        yield return new WaitForSeconds(voiceLines[5].length+0.5f);

        endTutorial();
        yield return null;
    }
    public void run()
    {
        wallSpawner = GameObject.Find("WallSpawner").GetComponent<WallSpawner>();
        voiceSource = voice.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        StartCoroutine(tutorial());
    }

    public void endTutorial()
    {
        m_gameManager = transform.parent.gameObject.GetComponent<gameManager>();
        m_gameManager.isRunning = true;
        Destroy(gameObject);
    }

    IEnumerator leftRight(Transform camera, Vector3 target)
    {
        bool lastInstruction = false;
        float waitTime = 2f;
        while(Vector3.Angle(Vector3.ProjectOnPlane(camera.forward, transform.up), target) > 5f)
        {
            Vector3 forward = Vector3.ProjectOnPlane(camera.forward, transform.up);
            if (lastInstruction)
            {
                yield return new WaitForSeconds(waitTime);
                //A little More
                voiceSource.clip = voiceLeftRight[0];
                voiceSource.Play();
                lastInstruction = false;
            }
            else
            {
                lastInstruction = true;
                if(Vector3.SignedAngle(forward,target,Vector3.up) > 0)
                {
                    yield return new WaitForSeconds(waitTime);
                    //A little Right
                    voiceSource.clip = voiceLeftRight[1];
                    voiceSource.Play();
                }
                else
                {
                    yield return new WaitForSeconds(waitTime);
                    //A little Left
                    voiceSource.clip = voiceLeftRight[2];
                    voiceSource.Play();
                }
            }
        }
        //That's Perfect;
        voiceSource.clip = voiceLeftRight[3];
        voiceSource.Play();
        yield return new WaitForSeconds(voiceLeftRight[3].length + 1f);
        isTraining = false;
    }

    IEnumerator lerpPosition(Vector3 start, Vector3 end, Transform target, float time, int resolution = 100)
    {
        for (int i = 0; i < resolution; i++)
        {
            float lerpVal = i / (float)resolution;
            target.position = Vector3.Lerp(start,end,lerpVal);
            yield return new WaitForSeconds(time / resolution);
        }
    }

    IEnumerator lerpRotation(float angle, Transform pivot, Transform target, float time, int resolution = 100)
    {

        float angleStep = angle/resolution;

        for (int i = 0; i < resolution; i++)
        {
            target.RotateAround(pivot.position, pivot.up, angleStep);
            yield return new WaitForSeconds(time/(float)resolution);
        }
    }

}
