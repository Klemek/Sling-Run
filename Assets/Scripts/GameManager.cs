using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Static Attributes
    
    internal static GameManager Instance;
    
    #endregion
    
    #region Attributes

    private int _life;

    private int _level;
    private LevelManager _levelManager;
    
    #endregion
    
    #region Members

    internal int Life
    {
        get { return _life; }
        set
        {
            _life = value;
            UiController.Instance.SetLife(_life);
            if (_life == 0)
                UiController.Instance.ShowEndPopup();
        }
    }

    internal int Money
    {
        get { return PlayerData.Money; }
        set
        {
            PlayerData.Money = value;
            UiController.Instance.SetMoney(value);
        }
    }
    
    internal bool CanRespawn
    {
        get
        {
            if (_levelManager == null || _levelManager.Ball == null)
                return false;
            return _levelManager.Ball.CanRespawn;
        }
    }
    
    #endregion
    
    #region Unity Methods

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
        if (scene.buildIndex == Constants.GameScene) StartGame();
    }
    
    #endregion

    #region Method

    internal void NextLevel()
    {
        _level++;
        UiController.Instance.SetLevel(_level);
        _levelManager.LoadLevel(false, _level);
    }

    internal void FinishGame()
    {
        if (_level > PlayerData.HighScore)
            PlayerData.HighScore = _level;
    }

    internal void Respawn()
    {
        _levelManager.Ball.Respawn();
    }
    
    private void StartGame()
    {
        _level = 0;
        Life = Constants.StartLife;
        _levelManager.LoadLevel(true, _level);
        UiController.Instance.SetLevel(_level);
        UiController.Instance.SetMoney(PlayerData.Money);
    }
    
    #endregion
}