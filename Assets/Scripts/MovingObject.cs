using System;
using UnityEngine;
using Random = System.Random;

namespace SlingRun
{
    public class MovingObject : MonoBehaviour
    {
        private Vector3 _delta;
        private bool _forward;

        private int _maxSteps;

        private float _movementSpeed;
        private int _nSteps;

        private float _rotation;
        private float _rotationSpeed;
        public float MaxMovementSpeed;
        public float MaxRotationSpeed;

        public float MinMovementSpeed;

        public float MinRotationSpeed;
        public Vector3 PositionDelta;

        private void Start()
        {
            var r = new Random();
            _movementSpeed = (float) (r.NextDouble() * (MaxMovementSpeed - MinMovementSpeed) + MinMovementSpeed);
            _rotationSpeed = (float) (r.NextDouble() * (MaxRotationSpeed - MinRotationSpeed) + MinRotationSpeed);

            if (!(Mathf.Abs(_movementSpeed) > float.MinValue)) return;
            _delta = Vector3.MoveTowards(_delta, PositionDelta, _movementSpeed);
            _maxSteps = (int) (PositionDelta.magnitude / _movementSpeed);

            _rotation = 0;
            _nSteps = 0;
            _forward = true;
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(_rotationSpeed) > float.MinValue)
                transform.rotation = Quaternion.AngleAxis(_rotation += _rotationSpeed, new Vector3(0, 0, 1));
            if (!(Mathf.Abs(_movementSpeed) > float.MinValue)) return;
            if (_forward)
            {
                transform.position += _delta;
                _nSteps++;
                if (_nSteps == _maxSteps)
                    _forward = false;
            }
            else
            {
                transform.position -= _delta;
                _nSteps--;
                if (_nSteps == 0)
                    _forward = true;
            }
        }
    }
}