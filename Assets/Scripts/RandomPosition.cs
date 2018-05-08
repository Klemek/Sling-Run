using UnityEngine;

namespace SlingRun
{
    public class RandomPosition : MonoBehaviour
    {
        public Vector3 MaxPosition;
        public float MaxRotation;
        public Vector3 MinPosition;

        public float MinRotation;

        private void Start()
        {
            if(MinPosition.magnitude > float.Epsilon || MaxPosition.magnitude > float.Epsilon)
                transform.position += new Vector3(Utils.NextFloat(MinPosition.x, MaxPosition.x),
                    Utils.NextFloat(MinPosition.y, MaxPosition.y),
                    Utils.NextFloat(MinPosition.z, MaxPosition.z));
            if(Mathf.Abs(MinRotation) > float.Epsilon || Mathf.Abs(MaxRotation) > float.Epsilon)
                transform.rotation = Quaternion.AngleAxis(Utils.NextFloat(MinRotation, MaxRotation), Vector3.forward);
        }
    }
}