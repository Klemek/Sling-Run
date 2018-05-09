using UnityEngine;

namespace SlingRun
{
    public class WallManagerMaster : WallManager
    {
        public WallManager[] SyncedWalls;

        public bool AllowMovement;

        public int MovementSpeed;

        public Vector3 MinPosition;
        public Vector3 MaxPosition;

        public bool AllowRotation;
        public int RotationSpeed;

        private void Awake()
        {
            CanMove = AllowMovement;
            CanRotate = AllowRotation;

            SetMovementSpeed(MovementSpeed);
            SetRotationSpeed(RotationSpeed);
        }

        internal void SetMovementSpeed(int speed)
        {
            MovementSpeed = speed;
            if (!AllowMovement) return;

            ResetMovement();

            if (MovementSpeed == 0)
            {
                MovSpeed = 0;
                MaxSteps = (int) ((MaxPosition - MinPosition).magnitude / Constants.WALL_MIN_M_SPEED);
                Delta = Vector3.MoveTowards(Vector3.zero, MaxPosition - MinPosition, Constants.WALL_MIN_M_SPEED);
            }
            else
            {
                MovSpeed = Random.Range(
                    Mathf.Max((MovementSpeed - 1) * Constants.WALL_M_SPEED_FACTOR, Constants.WALL_MIN_M_SPEED),
                    MovementSpeed * Constants.WALL_M_SPEED_FACTOR);
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

                syncedWall.Synced = true;
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
                Rotation = Random.Range(0, (int) (360 / Constants.WALL_MIN_R_SPEED)) * Constants.WALL_MIN_R_SPEED;
            }
            else
            {
                RotSpeed = Random.Range(
                    Mathf.Max((RotationSpeed - 1) * Constants.WALL_R_SPEED_FACTOR, Constants.WALL_MIN_M_SPEED),
                    RotationSpeed * Constants.WALL_R_SPEED_FACTOR);
                if (Random.Range(0, 2) == 0)
                    RotSpeed = -RotSpeed;
                Rotation = Random.Range(0, (int) (360 / Mathf.Abs(RotSpeed))) * RotSpeed;
            }

            InvalidateBounds();

            transform.rotation = Quaternion.AngleAxis(Rotation, Vector3.forward);

            foreach (var syncedWall in SyncedWalls)
            {
                syncedWall.Synced = true;
                syncedWall.CanRotate = AllowRotation;
                syncedWall.RotSpeed = RotSpeed;
                syncedWall.Rotation = syncedWall.Inverted ? -Rotation : Rotation;

                syncedWall.transform.rotation =
                    Quaternion.AngleAxis(syncedWall.Inverted ? -Rotation : Rotation, Vector3.forward);

                syncedWall.InvalidateBounds();
            }
        }
    }
}