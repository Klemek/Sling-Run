using UnityEngine;

public class PickupController : MonoBehaviour
{
    #region Attributes

    private Vector3 _scale;
    private bool _locked;

    #endregion
    
    #region Unity Methods

    private void Update()
    {
        if (_locked || Utils.RandInt(Constants.WobblingChance) > 0) return;
        _locked = true;
        _scale = transform.localScale;
        StartCoroutine(CoroutineUtils.SmoothScale(_scale * Constants.WobblingFactor, Constants.WobblingTime,
            MiddleWobbling, gameObject));
    }
    
    #endregion
    
    #region Methods

    private void MiddleWobbling()
    {
        StartCoroutine(CoroutineUtils.SmoothScale(_scale, Constants.WobblingTime,
            EndWobbling, gameObject));
    }
    
    private void EndWobbling()
    {
        _locked = false;
    }
    
    #endregion
}