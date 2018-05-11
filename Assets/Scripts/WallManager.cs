using UnityEngine;

public class WallManager : MonoBehaviour
{
    #region Unity Attributes
    
    public bool Inverted;

    #endregion
    
    #region Static Attributes
    
    internal static PhysicsMaterial2D[] WallMaterials;
    
    #endregion
    
    #region Attributes

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
    
    private Rect? _bounds;
    private WallType _type;

    #endregion

    #region Members

    internal Rect Bounds
    {
        get
        {
            if (_bounds == null)
                ComputeBounds();
            return _bounds ?? new Rect();
        }
    }

    internal WallType Type
    {
        get { return _type; }
        set
        {
            _type = value;
            var rb2D = GetComponent<Rigidbody2D>();
            var sprite = GetComponent<SpriteRenderer>();
            if (rb2D == null || sprite == null) return;
            rb2D.sharedMaterial = WallMaterials[(int)_type];
            sprite.color = Constants.WallColors[_type];
            tag = Constants.WallTags[_type];
        }
    }
    
    private bool Moving
    {
        get { return CanMove && Mathf.Abs(MovSpeed) > float.Epsilon; }
    }

    private bool Rotating
    {
        get { return CanRotate && Mathf.Abs(RotSpeed) > float.Epsilon; }
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (CompareTag(Constants.DefaultTag))
            Type = WallType.Default;
    }
    
    private void Update()
    {
        if (Moving)
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

        if (Rotating)
            transform.rotation = Quaternion.AngleAxis(Rotation += Inverted ? -RotSpeed : RotSpeed, Vector3.forward);
    }

    #endregion

    #region Methods

    protected internal void InvalidateBounds()
    {
        _bounds = null;
    }

    protected internal void ResetMovement()
    {
        transform.position -= StartPos + NSteps * (Inverted ? -Delta : Delta);
    }

    private void ComputeBounds()
    {
        var tmpPos = transform.position;
        var tmpRot = transform.rotation;

        const float rotationDelta = 360 / (float) Constants.WallPrecision;

        var delta = Delta * (MaxSteps / (float) Constants.WallPrecision);

        float? xMinMax = null;
        float? xMaxMin = null;
        float? yMin = null;
        float? yMax = null;


        if (Moving)
        {
            ResetMovement();
            transform.position += StartPos;
        }

        for (var n = 0; n < Constants.WallPrecision; n++)
        {
            for (var a = 0f; a < 360; a += rotationDelta)
            {
                if (Rotating)
                    transform.rotation = Quaternion.AngleAxis(a, Vector3.forward);

                var bounds = gameObject.Bounds();
                yMin = yMin.HasValue ? Mathf.Min(yMin.Value, bounds.yMin) : bounds.yMin;
                yMax = yMax.HasValue ? Mathf.Max(yMax.Value, bounds.yMax) : bounds.yMax;
                xMinMax = xMinMax.HasValue ? Mathf.Max(xMinMax.Value, bounds.xMin) : bounds.xMin;
                xMaxMin = xMaxMin.HasValue ? Mathf.Min(xMaxMin.Value, bounds.xMax) : bounds.xMax;

                if (!Rotating)
                    break;
            }

            if (!Moving)
                break;

            transform.position += Inverted ? -delta : delta;
        }

        if (yMin.HasValue)
            if (xMinMax < xMaxMin)
                _bounds = new Rect(xMinMax.Value, yMin.Value,
                    xMaxMin.Value - xMinMax.Value,
                    yMax.Value - yMin.Value);
            else
                _bounds = new Rect(0, yMin.Value,
                    0, yMax.Value - yMin.Value);

        transform.position = tmpPos;
        transform.rotation = tmpRot;
    }

    #endregion
}