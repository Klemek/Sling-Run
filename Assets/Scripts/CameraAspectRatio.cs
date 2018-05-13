using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    #region Unity Attributes
    
    public Vector2 Ratio;
    
    #endregion

    #region Attributes

    private Vector2Int _lastResolution;

    #endregion
    
    #region Unity Methods
    
    private void Start()
    {
        CameraUpdate();
        _lastResolution = new Vector2Int(Screen.width,Screen.height);
    }

    private void Update()
    {
        if (Screen.width == _lastResolution.x && Screen.height == _lastResolution.y) return;
        CameraUpdate();
        _lastResolution = new Vector2Int(Screen.width,Screen.height);
    }

    #endregion
    
    #region Methods

    private void CameraUpdate()
    {
        var expectedRatio = Ratio.x / Ratio.y;
        var mainCamera = GetComponent<Camera>();
        var currentRatio = Screen.width / (float) Screen.height;
        var rect = mainCamera.rect;
        
        if (currentRatio - expectedRatio < -float.Epsilon) // <
        {
            rect.height = currentRatio / expectedRatio;
            rect.position = new Vector2(0, (1f - rect.height) / 2f);
        }
        else if (currentRatio - expectedRatio > float.Epsilon) // >
        {
            rect.width = expectedRatio / currentRatio;
            rect.position = new Vector2((1f - rect.width) / 2f, 0);
        }
        
        mainCamera.rect = rect;
    }
    
    #endregion
}