using UnityEngine;

public class Loader : MonoBehaviour
{
    #region Unity Attributes
    
    public GameManager GameManager;
    
    #endregion

    #region Unity Methods
    
    private void Awake()
    {
        if (GameManager.Instance == null)
            Instantiate(GameManager);
    }
    
    #endregion
}