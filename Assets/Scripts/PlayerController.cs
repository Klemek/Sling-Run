using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Unity Attributes
    
    public GameObject DefaultSprite;
    public float SpeedFactor;
    
    #endregion
    
    #region Attributes
    
    private GameObject _line;
    private bool _mouseLastClicked;
    private Vector3 _mouseStartPos;

    private Rigidbody2D _rb2D;
    private Vector3 _scale;
    private bool _sliding;
    private Vector3 _startPos;

    private bool _locked;
    private bool _moving;
    
    #endregion
    
    #region Members
    
    internal bool CanRespawn
    {
        get { return _moving && !_locked; }
    }
    
    #endregion

    #region Unity Methods
    
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _startPos = _rb2D.position;
        _moving = false;
        _locked = false;
    }

    private void Update()
    {
        if (UiController.Paused || _locked) return;
        if (_moving)
        {
            if (_rb2D.velocity.magnitude <= Constants.BallResetSpeed)
                StartCoroutine(CoroutineUtils.Timer(1, Respawn));
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (!_mouseLastClicked)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if ((mousePos - Constants.MousePosDepth - transform.position).magnitude <
                    Constants.BallTouchSize)
                {
                    _mouseStartPos = mousePos;
                    _sliding = true;
                }
            }

            if (_sliding)
            {
                var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _mouseStartPos;

                if (delta.magnitude > Constants.BallMaxMagnitude)
                    delta = delta.normalized * Constants.BallMaxMagnitude;

                if (delta.y > 0)
                    delta.y = 0;

                _rb2D.position = _startPos + delta;

                if (_line != null)
                    Destroy(_line);
                _line = LineFactory.CreateDashedLine(DefaultSprite,
                    _startPos + delta + Constants.PredictLineDepth,
                    _startPos - delta + Constants.PredictLineDepth,
                    Constants.PredictLineColor, Constants.PredictLineThickness,
                    Constants.PredictLineDashLength);
            }

            _mouseLastClicked = true;
        }
        else
        {
            if (_mouseLastClicked)
            {
                var force = (_startPos - (Vector3) _rb2D.position) * SpeedFactor;

                Destroy(_line);

                if (force.magnitude >= Constants.BallMinSpeed)
                {
                    _rb2D.velocity = force;
                    _moving = true;
                }
                else
                {
                    _rb2D.position = _startPos;
                }

                _sliding = false;
            }

            _mouseLastClicked = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_locked || UiController.Paused) return;
        if (collision.gameObject.CompareTag(Constants.WallTags[WallType.Breakable]))
        {
            collision.gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_locked || UiController.Paused) return;
        if (collision.gameObject.CompareTag(Constants.RespawnTag))
        {
            Respawn();
        }
        else if (collision.gameObject.CompareTag(Constants.FinishTag))
        {
            GameManager.Instance.NextLevel();
            _rb2D.velocity = Vector2.zero;
            _moving = false;
            _locked = true;
        }
    }
    
    #endregion

    #region Methods
    
    internal void Release()
    {
        if (_rb2D == null) return;
        _rb2D.velocity = Vector2.zero;
        _moving = false;
        _locked = false;
        _startPos = _rb2D.position;
    }

    internal void Respawn()
    {
        if (!CanRespawn) return;
        _locked = true;
        _scale = transform.localScale;
        StartCoroutine(CoroutineUtils.SmoothScale(Vector3.zero, Constants.BallRespawnTime, RespawnMiddle,
            gameObject));
    }

    private void RespawnMiddle()
    {
        transform.position = _startPos;
        _rb2D.velocity = Vector2.zero;
        _moving = false;
        GameManager.Instance.LooseLife();
        if (GameManager.Instance.Life > 0)
            StartCoroutine(CoroutineUtils.SmoothScale(_scale, Constants.BallRespawnTime, RespawnEnd, gameObject));
    }

    private void RespawnEnd()
    {
        _locked = false;
    }
    
    #endregion
}