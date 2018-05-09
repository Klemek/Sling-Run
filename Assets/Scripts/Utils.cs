using System;
using System.Collections.Generic;
using System.Linq;
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

        internal static List<Tuple<float, float>> AreaOr(params IEnumerable<Tuple<float, float>>[] areas)
        {
            var tmp = new List<Tuple<float, float>>();
            foreach (var area in areas)
                if (area != null)
                    tmp.AddRange(area);
            tmp.Sort((o1, o2) => o1.Item1.CompareTo(o2.Item1));

            if (tmp.Count < 2) return tmp;

            var output = new List<Tuple<float, float>>();

            float? startx = null;

            while (true)
            {
                // get area start
                float? x0 = null;
                var x1 = 0f;
                foreach (var o in tmp)
                {
                    if (startx.HasValue && o.Item1 <= startx.Value) continue;
                    if (x0.HasValue && o.Item1 >= x0.Value) continue;
                    x0 = o.Item1;
                    x1 = o.Item2;
                }

                if (x0 == null)
                    break;

                //extend area
                var cont = true;
                while (cont)
                {
                    cont = false;
                    foreach (var o in tmp)
                    {
                        if (o.Item1 > x1 || o.Item2 <= x1) continue;
                        cont = true;
                        x1 = o.Item2;
                    }
                }

                output.Add(new Tuple<float, float>(x0.Value, x1));
                startx = x1;
            }


            return output;
        }

        internal static float AreaWidth(IEnumerable<Tuple<float, float>> area)
        {
            return area.Sum(o => o.Item2 - o.Item1);
        }

        internal static List<Tuple<float, float>> AreaNot(float startx, float endx,
            List<Tuple<float, float>> area)
        {
            var output = new List<Tuple<float, float>>();
            output.Sort((o1, o2) => o1.Item1.CompareTo(o2.Item1));

            if (area.Count == 0)
            {
                output.Add(new Tuple<float, float>(startx, endx));
                return output;
            }

            if (area.First().Item1 > startx)
                output.Add(new Tuple<float, float>(startx, area.First().Item1));

            for (var i = 0; i < area.Count - 1; i++)
                if (area[i].Item2 < endx && area[i + 1].Item1 > startx)
                    output.Add(new Tuple<float, float>(
                        Mathf.Max(startx, area[i].Item2),
                        Mathf.Min(endx, area[i + 1].Item1)));

            if (area.Last().Item2 < endx)
                output.Add(new Tuple<float, float>(area.Last().Item2, endx));

            return output;
        }

        internal static float AreaDistance(float pos, IEnumerable<Tuple<float, float>> area)
        {
            var min = float.MaxValue;
            foreach (var o in area)
            {
                if (pos >= o.Item1 && pos <= o.Item2)
                    return 0;
                min = Mathf.Min(min, Mathf.Abs(o.Item1 - pos), Mathf.Abs(o.Item2 - pos));
            }

            return min;
        }
    }
}