using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class Overlays : MonoBehaviour
    {
        public GameObject gameOverUI;
        public GameObject LevelFinishedUI;
        public TMP_Text currentTimeText;
        public TMP_Text bestTimeText;

        public void gameOver()
        {
            gameOverUI.SetActive(true);
        }

        public void LevelFinished()
        {
            LevelFinishedUI.SetActive(true);

            // Show current time
            float currentTime = FindObjectOfType<Timer>().GetElapsedTime();
            int currMin = Mathf.FloorToInt(currentTime / 60);
            int currSec = Mathf.FloorToInt(currentTime % 60);
            currentTimeText.text = string.Format("{0:00}:{1:00}", currMin, currSec);

            // Show best time pulled from database on login
            int bestTime = NetworkManager.HighScore;
            int bestMin = Mathf.FloorToInt(bestTime / 60);
            int bestSec = Mathf.FloorToInt(bestTime % 60);
            bestTimeText.text = string.Format("{0:00}:{1:00}", bestMin, bestSec);
        }
    }
}
