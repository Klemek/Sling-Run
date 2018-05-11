using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class CoroutineUtils
{
    public static IEnumerator SmoothScale(Vector3 endScale, float time, Action finishedAction,
        params GameObject[] objects)
    {
        objects = objects.Where(o => o != null).ToArray();

        var inverseTime = 1f / time;

        var sqrRemainingScale = objects.Max(o => (o.transform.localScale - endScale).magnitude);

        while (sqrRemainingScale > float.Epsilon)
        {
            sqrRemainingScale = 0f;
            foreach (var obj in objects)
            {
                if (obj == null) continue;
                var newScale = Vector3.MoveTowards(obj.transform.localScale, endScale,
                    inverseTime * Time.deltaTime);
                obj.transform.localScale = newScale;
                sqrRemainingScale = Mathf.Max(sqrRemainingScale, (newScale - endScale).magnitude);
            }

            yield return null;
        }

        finishedAction();
    }

    public static IEnumerator SmoothMove(float time, Action finishedAction, GameObject[] objects, Vector3[] endPos)
    {
        var inverseTime = 1f / time;
        var sqrRemainingDist = objects.Select((o, i) => (o.transform.position - endPos[i]).magnitude).Max();

        while (sqrRemainingDist > float.Epsilon)
        {
            sqrRemainingDist = 0f;
            for (var i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];
                if (obj == null) continue;
                var newPos = Vector3.MoveTowards(obj.transform.position, endPos[i],
                    inverseTime * Time.deltaTime);
                obj.transform.position = newPos;
                sqrRemainingDist = Mathf.Max(sqrRemainingDist, (newPos - endPos[i]).magnitude);
            }

            yield return null;
        }

        finishedAction();
    }

    public static IEnumerator SmoothColor(Color color, float time, Action finishedAction, Image image)
    {
        var inverseTime = 1f / time;
        var pos = image.color.ToVector4();
        var endPos = color.ToVector4();

        var sqrRemainingColor = (pos - endPos).magnitude;

        while (sqrRemainingColor > float.Epsilon)
        {
            pos = Vector4.MoveTowards(pos, endPos,
                inverseTime * Time.deltaTime);
            image.color = pos.ToColor();

            sqrRemainingColor = (pos - endPos).magnitude;
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