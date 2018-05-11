using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    #region Unity Attributes

    public PlayerController BallTemplate;
    public GameObject Borders;
    public LevelFragment[] LevelFragments;
    public PhysicsMaterial2D[] WallMaterials;

    #endregion

    #region Attributes

    internal PlayerController Ball;

    private GameObject _currentLevel;
    private int _nextLevelLevel;
    private GameObject _nextLevel;

    private Vector3 _delta;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _delta = new Vector3(0, Constants.LevelHeight, 0);
        WallManager.WallMaterials = WallMaterials;
    }

    #endregion

    #region Methods

    internal void LoadLevel(bool start, int level)
    {
        if (start)
        {
            _currentLevel = new GameObject("Level 0");
            Ball = Instantiate(BallTemplate, BallTemplate.transform.position, Quaternion.identity);
            Instantiate(Borders, Borders.transform.position, Quaternion.identity);

            StartCoroutine(GenerateNextLevel(level + 1));
        }
        else
        {
            //_nextLevel = GenerateLevel(level);
            //_nextLevel.transform.position = _delta;

            _nextLevelLevel = level + 1;

            StartCoroutine(CoroutineUtils.SmoothMove(Constants.LevelMoveTime, FinishedMoving,
                new[] {Ball.gameObject, _currentLevel, _nextLevel},
                new[] {GetBallNextPosition(), -_delta, Vector3.zero}));
        }
    }

    private void FinishedMoving()
    {
        Ball.Release();
        Destroy(_currentLevel);
        _currentLevel = _nextLevel;

        StartCoroutine(GenerateNextLevel(_nextLevelLevel));
    }

    private IEnumerator GenerateNextLevel(int level)
    {
        _nextLevel = GenerateLevel(level);
        _nextLevel.transform.position = _delta;
        yield return null;
    }

    private GameObject GenerateLevel(int level)
    {
        var newLevel = new GameObject("Level " + level);

        var t0 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        var reload = true;
        var frags = new LevelFragment[0];
        float h;
        var nFrag = 0;
        float diff;
        while (true)
        {
            if (reload)
            {
                foreach (var f in frags)
                    Destroy(f.gameObject);

                nFrag = 1 + Utils.RandInt(Constants.MaxFragmentNumber);
                frags = new LevelFragment[nFrag];
            }

            var area = new List<Tuple<float, float>>();
            h = 0f;

            for (var i = 0; i < nFrag; i++)
            {
                if (reload)
                {
                    var n = Utils.RandInt(LevelFragments.Length);
                    frags[i] = Instantiate(LevelFragments[n], newLevel.transform);
                    frags[i].gameObject.SetActive(false);
                }

                h += frags[i].Height;
                area = Utils.AreaOr(area, frags[i].Area);
            }

            reload = true;

            if (nFrag > 1)
                h += (nFrag - 1) * Constants.LevelFragmentMaxMargin;

            if (h > Constants.LevelMaxY - Constants.LevelMinY)
                continue;

            area = Utils.AreaNot(-Constants.LevelMaxX, Constants.LevelMaxX, area);
            var areaWidth = Utils.AreaWidth(area);

            if (areaWidth < Constants.MinPathWidth)
                continue;

            if (Utils.AreaDistance(Ball.transform.position.x, area) > Constants.MaxAreaDistBall)
                continue;

            diff = GetDifficulty(frags, areaWidth);

            var maxDiff = GetMaxLevelDifficulty(level);
            var minDiff = GetMinLevelDifficulty(level);
            if (diff > maxDiff)
                continue;

            reload = false;

            if (diff >= minDiff)
                break;

            if (IncreaseDifficulty(frags)) continue;

            reload = true;
        }

        var endH = Constants.LevelMaxY - h;
        var startH = Constants.LevelMinY;

        foreach (var f in frags)
        {
            var fh = f.Height;

            endH += fh;

            var py = Random.Range(startH + fh / 2f, endH - fh / 2f);

            var frag = f.gameObject;
            frag.transform.position = new Vector3(0, py, 0);
            frag.SetActive(true);

            startH = py + fh / 2 + Random.Range(Constants.LevelFragmentMinMargin,
                         Constants.LevelFragmentMaxMargin);
            endH += Constants.LevelFragmentMaxMargin;
        }

        var t1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        Debug.Log("Finished generating level " + level + "(diff:" + diff + ") in " + (t1 - t0) + " ms");

        return newLevel;
    }

    private Vector3 GetBallNextPosition()
    {
        var pos = Ball.gameObject.transform.localPosition - _delta;

        if (pos.x > Constants.BallMaxX)
            pos.x = Constants.BallMaxX;

        if (pos.x < -Constants.BallMaxX)
            pos.x = -Constants.BallMaxX;

        return pos;
    }

    #endregion

    #region Static Methods

    private static float GetDifficulty(IEnumerable<LevelFragment> frags, float areaWidth)
    {
        var diff = frags.Sum(frag => Constants.WallDifficulties[frag.WallType] *
                                     (frag.MovementSpeed + 1) * Constants.MovSpeedDifficulty +
                                     (frag.RotationSpeed + 1) * Constants.RotSpeedDifficulty);
        return Mathf.Pow(areaWidth, Constants.AreaWidthFactor) * diff;
    }

    private static bool IncreaseDifficulty(IList<LevelFragment> frags)
    {
        for (var i = 0; i < 10; i++)
        {
            var frag = frags[Utils.RandInt(frags.Count)];
            switch (Utils.RandInt(3))
            {
                default:
                    if (!frag.CanMove || frag.MovementSpeed >= Constants.MaxMovSpeed) continue;
                    frag.MovementSpeed++;
                    return true;
                case 1:
                    if (!frag.CanRotate || frag.RotationSpeed >= Constants.MaxRotSpeed) continue;
                    frag.RotationSpeed++;
                    return true;
                case 2:
                    if ((int) frag.WallType == Enum.GetValues(typeof(WallType)).Length - 1) continue;
                    frag.WallType++;
                    return true;
            }
        }

        return false;
    }

    private static float GetMaxLevelDifficulty(int level)
    {
        return Constants.MinDifficulty + Mathf.Round((Constants.MaxDifficulty - Constants.MinDifficulty) *
                                                     (1f - Mathf.Exp(-Constants.DifficultyFactor * level)));
    }

    private static float GetMinLevelDifficulty(int level)
    {
        return Mathf.Max(Constants.MinDifficulty,
            GetMaxLevelDifficulty(level) * (1f - Constants.DifficultyMaxMargin));
    }

    #endregion
}