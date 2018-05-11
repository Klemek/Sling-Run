using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelFragment : MonoBehaviour
{
    #region Attribute
    
    private WallManagerMaster[] _masters;
    private WallManager[] _walls;
    private float? _height;
    private List<Tuple<float, float>> _area;
    
    #endregion
    
    #region Members

    internal bool CanMove
    {
        get { return Masters.Any(m => m.AllowMovement); }
    }

    internal int MovementSpeed
    {
        get { return Masters.Max(m => m.MovementSpeed); }
        set
        {
            foreach (var wallManagerMaster in Masters)
                wallManagerMaster.SetMovementSpeed(value);
            InvalidateBounds();
        }
    }

    internal bool CanRotate
    {
        get { return Masters.Any(m => m.AllowRotation); }
    }

    internal int RotationSpeed
    {
        get { return Masters.Max(m => m.RotationSpeed); }
        set
        {
            foreach (var wallManagerMaster in Masters)
                wallManagerMaster.SetRotationSpeed(value);
            InvalidateBounds();
        }
    }

    internal float Height
    {
        get
        {
            if (!_height.HasValue)
                ComputeBounds();
            return _height ?? 0;
        }
    }

    internal WallType WallType
    {
        get { return Walls.First().Type; }
        set
        {
            foreach (var wall in Walls)
                wall.Type = value;
        }
    }
    
    internal IEnumerable<Tuple<float, float>> Area
    {
        get
        {
            if (_area == null)
                ComputeBounds();
            return _area;
        }
    }
    
    private IEnumerable<WallManagerMaster> Masters
    {
        get { return _masters ?? (_masters = GetComponentsInChildren<WallManagerMaster>()); }
    }

    private IEnumerable<WallManager> Walls
    {
        get { return _walls ?? (_walls = GetComponentsInChildren<WallManager>()); }
    }
    
    #endregion

    #region Methods
    
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
    
    #endregion
}