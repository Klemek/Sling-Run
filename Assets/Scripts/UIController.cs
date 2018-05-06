using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlingRun
{
    public class UiController : MonoBehaviour
    {
        public static bool Paused;

        private static UiController _instance;
        public GameObject Buttons;
        public GameObject EndPopup;

        public TextMeshProUGUI HighScoreText;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI LifeText;

        public GameObject PauseMenu;

        public static UiController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.Find(Constants.UICONTROLLER_OBJECT_NAME).GetComponent<UiController>();
                return _instance;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (Paused)
                Resume();
            else
                Pause();
        }

        public void Pause()
        {
            PauseMenu.SetActive(true);
            Buttons.SetActive(false);
            PauseGame(true);
        }

        public void Resume()
        {
            PauseMenu.SetActive(false);
            Buttons.SetActive(true);
            PauseGame(false);
        }

        public void Quit()
        {
            PauseGame(false);
            GameManager.Instance.FinishGame();
            SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
        }

        public void Respawn()
        {
            GameManager.Instance.Respawn();
        }

        public void Restart()
        {
            PauseGame(false);
            SceneManager.LoadScene(Constants.GAME_SCENE);
        }

        private void PauseGame(bool pause)
        {
            Paused = pause;
            Time.timeScale = pause ? 0f : 1f;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                Pause();
        }

        internal void SetLevel(int level)
        {
            LevelText.text = level.ToString();
            if (level <= PlayerData.HighScore)
                HighScoreText.text = string.Format(Constants.HIGHSCORE_TEXT, PlayerData.HighScore);
            else
                HighScoreText.text = Constants.NEW_HIGHSCORE_TEXT;
        }

        internal void SetLife(int life)
        {
            LifeText.text = life.ToString();
        }

        internal void ShowEndPopup()
        {
            Buttons.SetActive(false);
            EndPopup.SetActive(true);
            PauseGame(true);
        }
    }
}