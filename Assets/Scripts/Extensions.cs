using UnityEngine;

public static class Extensions
{
    #region String

    public static Color ToColor(this string hex)
    {
        Color output;
        ColorUtility.TryParseHtmlString(hex, out output);
        return output;
    }

    #endregion
    
    #region Color

    internal static Vector4 ToVector4(this Color c1)
    {
        return new Vector4(c1.r, c1.g, c1.b, c1.a);
    }

    #endregion

    #region Vector4

    internal static Color ToColor(this Vector4 v1)
    {
        return new Color(v1.x, v1.y, v1.z, v1.w);
    }

    #endregion

    #region GameObject

    internal static Rect Bounds(this GameObject go)
    {
        Vector2? min = null;
        Vector2? max = null;

        var renderer = go.GetComponent<Renderer>();

        if (renderer == null)
        {
            for (var i = 0; i < go.transform.childCount; i++)
            {
                var childBounds = go.transform.GetChild(i).gameObject.Bounds();
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

    #endregion
}