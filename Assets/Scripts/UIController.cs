using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlingRun
{
    public class UIController : MonoBehaviour
    {
        public static bool paused;

        public GameObject pauseMenu;
        public GameObject endPopup;
        public GameObject buttons;

        public TextMeshProUGUI highScoreText;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI lifeText;

        private static UIController instance;

        public static UIController Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.Find(Constants.UICONTROLLER_OBJECT_NAME).GetComponent<UIController>();
                return instance;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            pauseMenu.SetActive(true);
            buttons.SetActive(false);
            PauseGame(true);
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            buttons.SetActive(true);
            PauseGame(false);
        }

        public void Quit()
        {
            PauseGame(false);
            GameManager.instance.FinishGame();
            SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
        }

        public void Respawn()
        {
            GameManager.instance.Respawn();
        }

        public void Restart()
        {
            PauseGame(false);
            SceneManager.LoadScene(Constants.GAME_SCENE);
        }

        private void PauseGame(bool pause)
        {
            paused = pause;
            Time.timeScale = pause ? 0f : 1f;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                Pause();
        }

        internal void SetLevel(int level)
        {
            levelText.text = level.ToString();
            if (level <= PlayerData.HighScore)
                highScoreText.text = string.Format(Constants.HIGHSCORE_TEXT, PlayerData.HighScore);
            else
                highScoreText.text = Constants.NEW_HIGHSCORE_TEXT;
        }

        internal void SetLife(int life)
        {
            lifeText.text = life.ToString();
        }

        internal void ShowEndPopup()
        {
            buttons.SetActive(false);
            endPopup.SetActive(true);
            PauseGame(true);
        }
    }
}