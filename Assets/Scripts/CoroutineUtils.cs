using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace SlingRun
{
    public static class CoroutineUtils
    {
        public static IEnumerator SmoothScale(Vector3 endScale, float time, Action finishedAction, params GameObject[] objects)
        {
            objects = objects.Where(o => o != null).ToArray();
            
            var inverseTime = 1f / time;
            
            var sqrRemainingScale = objects.Max(o => (o.transform.localScale - endScale).magnitude);
            
            while (sqrRemainingScale > float.Epsilon)
            {
                foreach (var obj in objects)
                {
                    var newScale = Vector3.MoveTowards(obj.transform.localScale, endScale,
                        inverseTime * Time.deltaTime);
                    obj.transform.localScale = newScale;
                }
                sqrRemainingScale = objects.Max(o => (o.transform.localScale - endScale).magnitude);
                yield return null;
            }
            finishedAction();
        }
        
        public static IEnumerator SmoothMove(float time, Action finishedAction, GameObject[] objects, Vector3[] endPos)
        {
            var inverseTime = 1f / time;
            var sqrRemainingScale = objects.Select((o, i) => (o.transform.position - endPos[i]).magnitude).Max();

            while (sqrRemainingScale > float.Epsilon)
            {
                for(var i = 0; i < objects.Length; i++)
                {
                    var obj = objects[i];
                    var newPos = Vector3.MoveTowards(obj.transform.position, endPos[i],
                        inverseTime * Time.deltaTime);
                    obj.transform.position = newPos;
                }
                sqrRemainingScale = objects.Select((o, i) => (o.transform.position - endPos[i]).magnitude).Max();
                yield return null;
            }
            finishedAction();
        }

        public static IEnumerator Timer(float time, Action finishedAction)
        {
            yield return new WaitForSeconds(time);
            finishedAction();
        }
    }
}