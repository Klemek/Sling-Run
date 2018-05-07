using System;
using UnityEngine;

namespace SlingRun
{
    public class MovingObject : MonoBehaviour
    {
        public Vector3 PositionDelta;
        
        public float MinMovementSpeed;
        public float MaxMovementSpeed;

        public float MinRotationSpeed;
        public float MaxRotationSpeed;

        private float _movementSpeed;
        private float _rotationSpeed;

        private int _maxSteps;
        private int _nSteps;
        
        private float _rotation;
        private Vector3 _delta;
        private bool _forward;
        
        private Rigidbody2D _rb2D;
        
        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            
            var r = new System.Random();
            _movementSpeed = (float) (r.NextDouble() * (MaxMovementSpeed - MinMovementSpeed) + MinMovementSpeed);
            _rotationSpeed = (float) (r.NextDouble() * (MaxRotationSpeed - MinRotationSpeed) + MinRotationSpeed);

            if (!(Math.Abs(_movementSpeed) > float.MinValue)) return;
            _delta = Vector3.MoveTowards(_delta, PositionDelta, _movementSpeed);
            _maxSteps = (int)(PositionDelta.magnitude / _movementSpeed);
            
            _rotation = 0;
            _nSteps = 0;
            _forward = true;

        }

        private void FixedUpdate()
        {
            if (Math.Abs(_rotationSpeed) > float.MinValue)
                _rb2D.transform.rotation = Quaternion.AngleAxis(_rotation += _rotationSpeed, new Vector3(0, 0, 1));
            if (!(Math.Abs(_movementSpeed) > float.MinValue)) return;
            if (_forward)
            {
                _rb2D.transform.position += _delta;
                _nSteps++;
                if (_nSteps == _maxSteps)
                    _forward = false;
            }
            else
            {
                _rb2D.transform.position -= _delta;
                _nSteps--;
                if (_nSteps == 0)
                    _forward = true;
            }
        }
    }
}