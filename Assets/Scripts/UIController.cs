using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    #region Unity Attributes
    
    public GameObject Buttons;
    public GameObject EndPopup;
    
    public Button RespawnButton;
    public Color EnabledButtonColor;
    public Color DisabledButtonColor;
    
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI LifeText;
    public TextMeshProUGUI MoneyText;
    
    public GameObject PauseMenu;
    
    #endregion
    
    #region Attributes
    
    internal static bool Paused;

    private static UiController _instance;
    private bool _lastCanRespawn = true;
    
    #endregion

    #region Members

    public static UiController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.Find(Constants.UicontrollerObjectName).GetComponent<UiController>();
                return _instance;
            }
        }

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (GameManager.Instance.CanRespawn != _lastCanRespawn)
        {
            _lastCanRespawn = GameManager.Instance.CanRespawn;
            RespawnButton.interactable = _lastCanRespawn;

            StartCoroutine(CoroutineUtils.SmoothColor(_lastCanRespawn ? EnabledButtonColor : DisabledButtonColor,
                Constants.UiAnimationTime, () => { }, RespawnButton.transform.GetChild(0).GetComponent<Image>()));
        }


        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (Paused)
            Resume();
        else
            Pause();
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && !Paused)
            Pause();
    }
    
    #endregion

    #region Methods
    
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
        SceneManager.LoadScene(Constants.MainMenuScene);
    }

    public void Respawn()
    {
        GameManager.Instance.Respawn();
    }

    public void Restart()
    {
        PauseGame(false);
        SceneManager.LoadScene(Constants.GameScene);
    }

    public void PauseGame(bool pause)
    {
        Paused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }

    internal void SetLevel(int level)
    {
        LevelText.text = level.ToString();
        HighScoreText.text = level <= PlayerData.HighScore
            ? string.Format(Constants.HighscoreText, PlayerData.HighScore)
            : Constants.NewHighscoreText;
    }

    internal void SetLife(int life)
    {
        LifeText.text = life.ToString();
    }
    
    internal void SetMoney(int money)
    {
        MoneyText.text = money.ToString();
    }

    internal void ShowEndPopup()
    {
        Buttons.SetActive(false);
        EndPopup.SetActive(true);
        PauseGame(true);
    }
    
    #endregion
}