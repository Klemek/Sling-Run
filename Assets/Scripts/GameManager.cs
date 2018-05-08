using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlingRun
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private LevelManager _levelManager;

        public int Level;
        public int Life;

        internal bool CanRespawn
        {
            get
            {
                if (_levelManager == null || _levelManager.Ball == null)
                    return false;
                return _levelManager.Ball.CanRespawn;
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            _levelManager = GetComponent<LevelManager>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == Constants.GAME_SCENE) StartGame();
        }

        private void StartGame()
        {
            Level = 0;
            Life = Constants.START_LIFE;
            _levelManager.LoadLevel(true, Level);
            UiController.Instance.SetLevel(Level);
            UiController.Instance.SetLife(Life);
        }

        internal void NextLevel()
        {
            Level++;
            UiController.Instance.SetLevel(Level);
            _levelManager.LoadLevel(false, Level);
        }

        internal void FinishGame()
        {
            if (Level > PlayerData.HighScore)
                PlayerData.HighScore = Level;
        }

        internal void Respawn()
        {
            _levelManager.Ball.Respawn();
        }

        internal void LooseLife()
        {
            Life--;
            UiController.Instance.SetLife(Life);
            if (Life == 0)
                UiController.Instance.ShowEndPopup();
        }
    }
}