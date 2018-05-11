using UnityEngine;

public class WallManagerMaster : WallManager
{
    #region Unity Attributes
    
    public WallManager[] SyncedWalls;

    public bool AllowMovement;

    public int MovementSpeed;

    public Vector3 MinPosition;
    public Vector3 MaxPosition;

    public bool AllowRotation;
    public int RotationSpeed;

    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        CanMove = AllowMovement;
        CanRotate = AllowRotation;

        SetMovementSpeed(MovementSpeed);
        SetRotationSpeed(RotationSpeed);
    }
    
    #endregion
    
    #region Methods

    internal void SetMovementSpeed(int speed)
    {
        MovementSpeed = speed;
        if (!AllowMovement) return;

        ResetMovement();

        if (MovementSpeed == 0)
        {
            MovSpeed = 0;
            MaxSteps = (int) ((MaxPosition - MinPosition).magnitude / Constants.WallMinMSpeed);
            Delta = Vector3.MoveTowards(Vector3.zero, MaxPosition - MinPosition, Constants.WallMinMSpeed);
        }
        else
        {
            MovSpeed = Random.Range(
                Mathf.Max((MovementSpeed - 1) * Constants.WallMSpeedFactor, Constants.WallMinMSpeed),
                MovementSpeed * Constants.WallMSpeedFactor);
            MaxSteps = (int) ((MaxPosition - MinPosition).magnitude / MovSpeed);
            Delta = Vector3.MoveTowards(Vector3.zero, MaxPosition - MinPosition, MovSpeed);
        }

        NSteps = Random.Range(0, MaxSteps + 1);
        Forward = NSteps == 0 || NSteps != MaxSteps && Random.Range(0, 2) == 0;

        StartPos = Inverted ? -MinPosition : MinPosition;

        transform.position += StartPos +
                              NSteps * (Inverted ? -Delta : Delta);
        InvalidateBounds();

        foreach (var syncedWall in SyncedWalls)
        {
            syncedWall.ResetMovement();

            syncedWall.CanMove = AllowMovement;
            syncedWall.MovSpeed = MovSpeed;
            syncedWall.MaxSteps = MaxSteps;
            syncedWall.NSteps = NSteps;
            syncedWall.Forward = Forward;
            syncedWall.Delta = Delta;

            syncedWall.StartPos = syncedWall.Inverted ? -MinPosition : MinPosition;

            syncedWall.transform.position += syncedWall.StartPos +
                                             NSteps * (syncedWall.Inverted ? -Delta : Delta);
            syncedWall.InvalidateBounds();
        }
    }

    internal void SetRotationSpeed(int speed)
    {
        RotationSpeed = speed;
        if (!AllowRotation) return;
        if (RotationSpeed == 0)
        {
            RotSpeed = 0;
            Rotation = Random.Range(0, (int) (360 / Constants.WallMinRSpeed)) * Constants.WallMinRSpeed;
        }
        else
        {
            RotSpeed = Random.Range(
                Mathf.Max((RotationSpeed - 1) * Constants.WallRSpeedFactor, Constants.WallMinMSpeed),
                RotationSpeed * Constants.WallRSpeedFactor);
            if (Random.Range(0, 2) == 0)
                RotSpeed = -RotSpeed;
            Rotation = Random.Range(0, (int) (360 / Mathf.Abs(RotSpeed))) * RotSpeed;
        }

        InvalidateBounds();

        transform.rotation = Quaternion.AngleAxis(Rotation, Vector3.forward);

        foreach (var syncedWall in SyncedWalls)
        {
            syncedWall.CanRotate = AllowRotation;
            syncedWall.RotSpeed = RotSpeed;
            syncedWall.Rotation = syncedWall.Inverted ? -Rotation : Rotation;

            syncedWall.transform.rotation =
                Quaternion.AngleAxis(syncedWall.Inverted ? -Rotation : Rotation, Vector3.forward);

            syncedWall.InvalidateBounds();
        }
    }
    
    #endregion
}