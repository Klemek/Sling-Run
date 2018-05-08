using UnityEngine;
using Random = System.Random;

namespace SlingRun
{
    public static class Utils
    {
        internal static float NextFloat(float min, float max)
        {
            var r = new Random();
            return (float) (r.NextDouble() * (max - min) + min);
        }

        internal static Rect ObjectBounds(GameObject go)
        {
            Vector2? min = null;
            Vector2? max = null;

            var renderer = go.GetComponent<Renderer>();
            
            if (renderer == null)
            {
                for (var i = 0; i < go.transform.childCount; i++)
                {
                    var childBounds = ObjectBounds(go.transform.GetChild(i).gameObject);
                    var newMin = new Vector2(childBounds.xMin, childBounds.yMin);
                    var newMax = new Vector2(childBounds.xMax, childBounds.yMax);
                    min = min == null ? newMin : Vector2.Min(min.Value, newMin);
                    max = max == null ? newMax : Vector2.Max(max.Value, newMax);
                }
            }
            else
            {
                var cen = renderer.bounds.center;
                var ext = renderer.bounds.extents;
                var extentPoints = new[]
                {
                    new Vector2(cen.x - ext.x, cen.y - ext.y),
                    new Vector2(cen.x + ext.x, cen.y - ext.y),
                    new Vector2(cen.x - ext.x, cen.y + ext.y),
                    new Vector2(cen.x + ext.x, cen.y + ext.y)
                };
                min = extentPoints[0];
                max = extentPoints[0];
                foreach (var v in extentPoints)
                {
                    min = Vector2.Min(min.Value, v);
                    max = Vector2.Max(max.Value, v);
                }
            }

            return min.HasValue
                ? new Rect(min.Value.x, min.Value.y, max.Value.x - min.Value.x, max.Value.y - min.Value.y)
                : new Rect();
        }

        internal static Vector4 ColorToVector4(Color c1)
        {
            return new Vector4(c1.r, c1.g, c1.b, c1.a);
        }

        internal static Color Vector4ToColor(Vector4 v1)
        {
            return new Color(v1.x, v1.y, v1.z, v1.w);
        }
    }
}