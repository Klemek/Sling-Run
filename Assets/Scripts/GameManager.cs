using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace SlingRun
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public int level;
        public int life;

        private LevelManager levelManager;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            levelManager = GetComponent<LevelManager>();
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
            if (scene.buildIndex == Constants.GAME_SCENE)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            level = 0;
            life = Constants.START_LIFE;
            levelManager.LoadLevel(true, level);
            UIController.Instance.SetLevel(level);
            UIController.Instance.SetLife(life);
        }

        internal void NextLevel()
        {
            level++;
            UIController.Instance.SetLevel(level);
            levelManager.LoadLevel(false, level);
        }

        internal void FinishGame()
        {
            if (level > PlayerData.HighScore)
                PlayerData.HighScore = level;
            if(life == 0)
                UIController.Instance.ShowEndPopup();
        }

        internal void Respawn()
        {
            levelManager.ball.Respawn();
        }
        
        internal void LooseLife()
        {
            life--;
            UIController.Instance.SetLife(life);
            if (life == 0)
                FinishGame();
        }
    }
}