using System;
using UnityEngine;

namespace SlingRun
{
    public class LevelFragment : MonoBehaviour
    {
        private float _height;
        public int Difficulty;

        public float Height
        {
            get
            {
                if (Mathf.Abs(_height) < float.Epsilon)
                    ComputeBounds();
                return _height;
            }
        }

        private void ComputeBounds()
        {
            var bounds = Utils.ObjectBounds(gameObject);
            _height = bounds.height;
        }
    }
}