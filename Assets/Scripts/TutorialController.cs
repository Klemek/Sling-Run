using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    #region Unity Attributes

    public WallManager[] Walls;

    #endregion
    
    #region Unity Methods
    
    private void Start()
    {
        PlayerData.HasSeenTutorial = true;
        foreach (WallType wallType in Enum.GetValues(typeof(WallType)))
            Walls[(int) wallType].Type = wallType;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) QuitTutorial();
    }
    
    #endregion
    
    #region Methods
    
    public void PlayGame()
    {
        SceneManager.LoadScene(Constants.GameScene);
    }
    
    public void QuitTutorial()
    {
        SceneManager.LoadScene(Constants.MainMenuScene);
    }
    
    #endregion
}