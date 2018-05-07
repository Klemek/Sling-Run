using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SlingRun
{
    public static class LineFactory
    {
        private static GameObject CreateSimpleLine(GameObject defaultSprite, Vector3 p1, Vector3 p2, Color color,
            float thickness)
        {
            var center = (p1 + p2) / 2f;
            var tmp = p1 - p2;
            var length = tmp.magnitude;
            var angle = (float) (Math.Atan2(tmp.y, tmp.x) * (180 / Math.PI));

            var obj = Object.Instantiate(defaultSprite, center, Quaternion.identity);
            var renderer = obj.GetComponent<SpriteRenderer>();
            renderer.color = color;

            obj.transform.localScale = new Vector3(length, thickness, 1f);
            obj.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

            return obj;
        }

        internal static GameObject CreateDashedLine(GameObject defaultSprite, Vector3 p1, Vector3 p2, Color color,
            float thickness, float size)
        {
            var dist = p2 - p1;
            var length = dist.magnitude;
            var n = length / size;
            var tmpDist = dist / n;

            var obj = new GameObject("Dashed line");

            for (var i = 0; i < n; i += 2)
            {
                var color2 = new Color(color.r, color.g, color.b, 1f - i / n);
                var line = CreateSimpleLine(defaultSprite, p1 + tmpDist * i, p1 + tmpDist * (i + 1), color2,
                    thickness);
                line.transform.SetParent(obj.transform);
            }

            return obj;
        }
    }
}