using System;
using System.Collections;
using UnityEngine;

namespace SlingRun
{
    public class PlayerController : MonoBehaviour
    {
        private GameObject _line;
        private bool _mouseLastClicked;
        private Vector3 _mouseStartPos;

        private Rigidbody2D _rb2D;
        private bool _sliding;
        private Vector3 _startPos;
        private Vector3 _scale;
        public bool Locked;


        public bool Moving;
        public float SpeedFactor;

        public GameObject DefaultSprite;

        // Use this for initialization
        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _startPos = _rb2D.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if (UiController.Paused || Locked) return;
            if (Moving)
            {
                if (_rb2D.velocity.magnitude <= Constants.BALL_RESET_SPEED)
                    StartCoroutine(CoroutineUtils.Timer(1, Respawn));
                return;
            }
            if (Input.GetMouseButton(0))
            {
                if (!_mouseLastClicked)
                {
                    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if ((mousePos - Constants.MOUSE_POS_DEPTH - transform.position).magnitude <
                        Constants.BALL_TOUCH_SIZE)
                    {
                        _mouseStartPos = mousePos;
                        _sliding = true;
                    }
                }

                if (_sliding)
                {
                    var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _mouseStartPos;

                    if (delta.magnitude > Constants.BALL_MAX_MAGNITUDE)
                        delta = delta.normalized * Constants.BALL_MAX_MAGNITUDE;

                    if (delta.y > 0)
                        delta.y = 0;

                    _rb2D.position = _startPos + delta;

                    if (_line != null)
                        Destroy(_line);
                    _line = LineFactory.CreateDashedLine(DefaultSprite,
                        _startPos + delta + Constants.PREDICT_LINE_DEPTH,
                        _startPos - delta + Constants.PREDICT_LINE_DEPTH,
                        Constants.PREDICT_LINE_COLOR, Constants.PREDICT_LINE_THICKNESS,
                        Constants.PREDICT_LINE_DASH_LENGTH);
                }

                _mouseLastClicked = true;
            }
            else
            {
                if (_mouseLastClicked)
                {
                    var force = (_startPos - (Vector3) _rb2D.position) * SpeedFactor;

                    Destroy(_line);

                    if (force.magnitude >= Constants.BALL_MIN_SPEED)
                    {
                        _rb2D.velocity = force;
                        Moving = true;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Locked || UiController.Paused) return;
            if (collision.gameObject.CompareTag(Constants.RESPAWN_TAG))
            {
                Respawn();
            }
            else if (collision.gameObject.CompareTag(Constants.FINISH_TAG))
            {
                GameManager.Instance.NextLevel();
                _rb2D.velocity = Vector2.zero;
                Moving = false;
                Locked = true;
            }
        }

        internal void Release()
        {
            _rb2D.velocity = Vector2.zero;
            Moving = false;
            Locked = false;
            _startPos = _rb2D.position;
        }

        internal void Respawn()
        {
            if (!Moving || Locked) return;
            Locked = true;
            _scale = transform.localScale;
            StartCoroutine(CoroutineUtils.SmoothScale(Vector3.zero, Constants.BALL_RESPAWN_TIME, RespawnMiddle, gameObject));
        }

        private void RespawnMiddle()
        {
            transform.position = _startPos;
            _rb2D.velocity = Vector2.zero;
            Moving = false;
            GameManager.Instance.LooseLife();
            if (GameManager.Instance.Life > 0)
                StartCoroutine(CoroutineUtils.SmoothScale(_scale, Constants.BALL_RESPAWN_TIME, RespawnEnd, gameObject));
        }

        private void RespawnEnd()
        {
            Locked = false;
        }
        
    }
}