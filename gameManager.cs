using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace manageGame
{
    public class gameManager : MonoBehaviour
    {
        public int maxHealth = 3;
        int shipHealth;

        public int missilesBlocked = 0;
        int oldBlock = 0;

        public int missilesReceived = 0;
        int oldReceived = 0;

        public float difficulty = 1f;
        public int combo = 0;
        public bool isRunning = false;
        bool lastBlock = false;

        public float score = 0;



        GameObject gameUI;
        GameObject gameOverUI;
        GameObject[] UITexts;
        Slider healthBar;
        TMPro.TextMeshProUGUI comboText;
        TMPro.TextMeshProUGUI scoreText;
        TMPro.TextMeshProUGUI finalScoreText;

        public GameObject tutorial;
        public GameObject[] finalExplosions;

        // Start is called before the first frame update
        void Start()
        {
            gameUI = GameObject.FindGameObjectWithTag("UI");
            UITexts = GameObject.FindGameObjectsWithTag("UIText");
            comboText = Array.Find(UITexts, element => element.name == "comboText").GetComponent<TMPro.TextMeshProUGUI>();
            scoreText = Array.Find(UITexts, element => element.name == "scoreText").GetComponent<TMPro.TextMeshProUGUI>();
            healthBar = Array.Find(GameObject.FindGameObjectsWithTag("UISlider"), element => element.name == "Bar").GetComponent<Slider>();
            shipHealth = maxHealth;
            healthBar.value = shipHealth/(float)maxHealth;

            Instantiate(tutorial, transform).GetComponent<runTutorial>().run();
        }

        // Update is called once per frame
        void Update()
        {
            if ( missilesReceived != oldReceived)
            {
                combo = 0;
                comboText.text = combo.ToString();
                shipHealth--;
                healthBar.value = shipHealth / (float)maxHealth;
                lastBlock = false;
                if (shipHealth <= 0)
                {
                    gameOver();
                }
            }


            if (missilesBlocked != oldBlock)
            {

                combo++;
                comboText.text = combo.ToString();

                score += combo * difficulty;
                scoreText.text = Mathf.Round(score).ToString();

                difficulty += 0.1f;
                lastBlock = true;
            }

            oldReceived = missilesReceived;
            oldBlock = missilesBlocked;
        }
        void gameOver()
        {
            isRunning = false;
            Destroy(GameObject.Find("WallSpawner"));
            Destroy(Instantiate(finalExplosions[1], transform), 10);
            Destroy(Instantiate(finalExplosions[0], transform),14);
            gameUI.transform.Find("inGame").gameObject.SetActive(false);
            gameOverUI = gameUI.transform.Find("afterGame").gameObject;
            gameOverUI.gameObject.SetActive(true);
            finalScoreText = gameOverUI.transform.Find("finalScoreText").GetComponent<TMPro.TextMeshProUGUI>();
            //finalScoreText = Array.Find(UITexts, element => element.name == "finalScoreText").GetComponent<TMPro.TextMeshProUGUI>();
            finalScoreText.text = Mathf.Round(score).ToString();
        }
    }
}

