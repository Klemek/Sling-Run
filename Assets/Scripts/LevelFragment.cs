using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace SlingRun
{
    public class LevelFragment : MonoBehaviour
    {

        private WallManagerMaster[] _masters;

        private IEnumerable<WallManagerMaster> Masters
        {
            get { return _masters ?? (_masters = GetComponentsInChildren<WallManagerMaster>()); }
        }

        private WallManager[] _walls;

        private IEnumerable<WallManager> Walls
        {
            get { return _walls ?? (_walls = GetComponentsInChildren<WallManager>()); }
        }
        
        public bool CanMove
        {
            get { return Masters.Any(m => m.AllowMovement); }
        }

        public int MovementSpeed
        {
            get { return Masters.Max(m => m.MovementSpeed); }
            set
            {
                foreach (var wallManagerMaster in Masters)
                    wallManagerMaster.SetMovementSpeed(value);
                InvalidateBounds();
            }
        }

        public bool CanRotate
        {
            get { return Masters.Any(m => m.AllowRotation); }
        }
        
        public int RotationSpeed
        {
            get { return Masters.Max(m => m.RotationSpeed); }
            set
            {
                foreach (var wallManagerMaster in Masters)
                    wallManagerMaster.SetRotationSpeed(value);
                InvalidateBounds();
            }
        }
        
        private float? _height;
        
        public float Height
        {
            get
            {
                if (!_height.HasValue)
                    ComputeBounds();
                return _height ?? 0;
            }
        }

        private List<Tuple<float, float>> _area;

        public List<Tuple<float, float>> Area
        {
            get
            {
                if(_area == null)
                    ComputeBounds();
                return _area;
            }
        }

        private void InvalidateBounds()
        {
            _height = null;
        }
        
        private void ComputeBounds()
        {
            float? yMin = null;
            float? yMax = null;

            _area = new List<Tuple<float, float>>();
            
            foreach (var wall in Walls)
            {
                yMin = yMin.HasValue ? Mathf.Min(yMin.Value, wall.Bounds.yMin) : wall.Bounds.yMin;
                yMax = yMax.HasValue ? Mathf.Max(yMax.Value, wall.Bounds.yMax) : wall.Bounds.yMax;

                if (!(wall.Bounds.width > 0)) continue;
                var width = new Tuple<float, float>(wall.Bounds.xMin, wall.Bounds.xMax);
                _area = Utils.AreaOr(_area, new List<Tuple<float, float>> {width});
            }

            _height = yMin.HasValue ? yMax.Value - yMin.Value : 0;
        }
    }
}