using System;
using UnityEngine;

namespace SlingRun
{
    public class WallManager : MonoBehaviour
    {
        public bool Inverted;

        private Rect? _bounds;

        public Rect Bounds
        {
            get
            {
                if(_bounds == null)
                    ComputeBounds();
                return _bounds ?? new Rect();
            }
        }

        protected internal bool CanMove;

        protected internal float MovSpeed;
        protected internal int MaxSteps;
        protected internal int NSteps;
        protected internal bool Forward;
        protected internal Vector3 Delta;

        protected internal Vector3 StartPos;
        
        protected internal bool CanRotate;
        protected internal float RotSpeed;
        protected internal float Rotation;

        protected internal bool Synced;

        private bool Moving
        {
            get { return CanMove && Mathf.Abs(MovSpeed) > float.Epsilon; }
        }

        private bool Rotating
        {
            get { return CanRotate && Mathf.Abs(RotSpeed) > float.Epsilon; }
        }
        
        internal bool Master
        {
            get { return !Synced && (CanMove || CanRotate); }
        }

        private void Update()
        {
            if (Moving)
            {
                if (Forward)
                {
                    NSteps++;
                    if (NSteps == MaxSteps)
                        Forward = false;
                    transform.position += Inverted ? -Delta : Delta;
                }
                else
                {
                    NSteps--;
                    if (NSteps == 0)
                        Forward = true;
                    transform.position -= Inverted ? -Delta : Delta;
                }
            }

            if (Rotating)
            {
                transform.rotation = Quaternion.AngleAxis(Rotation += Inverted ? -RotSpeed : RotSpeed, Vector3.forward);
            }
        }

        protected internal void InvalidateBounds()
        {
            _bounds = null;
        }

        private void ComputeBounds()
        {
            var tmpPos = transform.position;
            var tmpRot = transform.rotation;

            const float rotationDelta = 360 / (float) Constants.WALL_PRECISION;

            var delta = Delta * (MaxSteps / (float) Constants.WALL_PRECISION);

            float? xMinMax = null;
            float? xMaxMin = null;
            float? yMin = null;
            float? yMax = null;
            float? xMin = null;
            float? xMax = null;
           

            if (Moving)
            {
                ResetMovement();
                transform.position += StartPos;
            }

            var times = 0;
            
            for (var n = 0; n < Constants.WALL_PRECISION; n++)
            {
                for (var a = 0f; a < 360; a += rotationDelta)
                {
                    if(Rotating)
                        transform.rotation = Quaternion.AngleAxis(a, Vector3.forward);
                    
                    var bounds = Utils.ObjectBounds(gameObject);
                    yMin = yMin.HasValue ? Mathf.Min(yMin.Value, bounds.yMin) : bounds.yMin;
                    yMax = yMax.HasValue ? Mathf.Max(yMax.Value, bounds.yMax) : bounds.yMax;
                    xMin = xMin.HasValue ? Mathf.Min(xMin.Value, bounds.xMin) : bounds.xMin;
                    xMax = xMax.HasValue ? Mathf.Max(xMax.Value, bounds.xMax) : bounds.xMax;
                    xMinMax = xMinMax.HasValue ? Mathf.Max(xMinMax.Value, bounds.xMin) : bounds.xMin;
                    xMaxMin = xMaxMin.HasValue ? Mathf.Min(xMaxMin.Value, bounds.xMax) : bounds.xMax;

                    times++;
                    
                    if (!Rotating)
                        break;
                }

                if (!Moving)
                    break;
                
                transform.position += Inverted ? -delta : delta;
            }

            if (yMin.HasValue)
            {
                if (xMinMax < xMaxMin)
                {
                    _bounds = new Rect(xMinMax.Value, yMin.Value,
                        xMaxMin.Value - xMinMax.Value,
                        yMax.Value - yMin.Value);
                }
                else
                {
                    _bounds = new Rect(0, yMin.Value,
                        0, yMax.Value - yMin.Value);
                }
            }

            transform.position = tmpPos;
            transform.rotation = tmpRot;
        }

        protected internal void ResetMovement()
        {
            transform.position -= StartPos + NSteps * (Inverted ? -Delta : Delta);
        }
    }
}