using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SlingRun
{
    public class LevelManager : MonoBehaviour
    {
        private GameObject _currentLevel;

        private Vector3 _delta;
        private float _inverseMoveTime;
        private GameObject _nextLevel;

        private int _nextLevelLevel;
        internal PlayerController Ball;
        public PlayerController BallTemplate;
        public GameObject Borders;
        public LevelFragment[] LevelFragments;

        private void Awake()
        {
            _delta = new Vector3(0, Constants.LEVEL_HEIGHT, 0);
            _inverseMoveTime = 1f / Constants.LEVEL_MOVE_TIME;
        }

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

                StartCoroutine(CoroutineUtils.SmoothMove(Constants.LEVEL_MOVE_TIME, FinishedMoving,
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
            float areaWidth;
            while(true){
                if (reload)
                {
                    foreach (var f in frags)
                        Destroy(f.gameObject);

                    nFrag = Random.Range(1, Constants.MAX_FRAGMENT_NUMBER + 1);
                    frags = new LevelFragment[nFrag];
                }

                var area = new List<Tuple<float, float>>();
                h = 0f;
                
                for (var i = 0; i < nFrag; i++)
                {
                    if (reload)
                    {
                        var n = Random.Range(0, LevelFragments.Length);
                        frags[i] = Instantiate(LevelFragments[n], newLevel.transform);
                        frags[i].gameObject.SetActive(false);
                    }

                    h += frags[i].Height;
                    area = Utils.AreaOr(area, frags[i].Area);
                }

                reload = true;
                
                if (nFrag > 1)
                    h += (nFrag - 1) * Constants.LEVEL_FRAGMENT_MAX_MARGIN;

                if(h > Constants.LEVEL_MAX_Y - Constants.LEVEL_MIN_Y)
                    continue;
                
                area = Utils.AreaNot(-Constants.LEVEL_MAX_X, Constants.LEVEL_MAX_X, area);
                areaWidth = Utils.AreaWidth(area);

                if (areaWidth < Constants.MIN_PATH_WIDTH)
                    continue;

                if (Utils.AreaDistance(Ball.transform.position.x, area) > Constants.MAX_AREA_DIST_BALL)
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

            var endH = Constants.LEVEL_MAX_Y - h;
            var startH = Constants.LEVEL_MIN_Y;

            foreach (var f in frags)
            {
                var fh = f.Height;

                endH += fh;

                var py = Utils.NextFloat(startH + fh / 2f, endH - fh / 2f);

                var frag = f.gameObject;
                frag.transform.position = new Vector3(0, py, 0);
                frag.SetActive(true);

                startH = py + fh / 2 + Utils.NextFloat(Constants.LEVEL_FRAGMENT_MIN_MARGIN,
                             Constants.LEVEL_FRAGMENT_MAX_MARGIN);
                endH += Constants.LEVEL_FRAGMENT_MAX_MARGIN;
            }

            var t1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                Debug.Log("Finished generating level " + level + "(diff:"+diff+") in " + (t1 - t0) + " ms");

            return newLevel;
        }

        private static float GetDifficulty(IEnumerable<LevelFragment> frags, float areaWidth)
        {
            var diff = frags.Sum(frag =>
                (frag.MovementSpeed + 1) * Constants.MOV_SPEED_DIFFICULTY +
                (frag.RotationSpeed + 1) * Constants.ROT_SPEED_DIFFICULTY);
            Debug.Log("areaWidth:"+areaWidth+" diff:"+diff);
            return Mathf.Pow(areaWidth, Constants.AREA_WIDTH_FACTOR) * diff;
        }

        private static bool IncreaseDifficulty(IList<LevelFragment> frags)
        {
            for (var i = 0; i < 10; i++)
            {
                var frag = frags[Random.Range(0, frags.Count)];
                if (Random.Range(0, 2) == 0)
                {
                    if (!frag.CanMove || frag.MovementSpeed >= Constants.MAX_MOV_SPEED) continue;
                    frag.MovementSpeed++;
                    return true;
                }

                if (!frag.CanRotate || frag.RotationSpeed >= Constants.MAX_ROT_SPEED) continue;
                frag.RotationSpeed++;
                return true;
            }

            return false;
        }

        private static float GetMaxLevelDifficulty(int level)
        {
            return Constants.MIN_DIFFICULTY + Mathf.Round((Constants.MAX_DIFFICULTY - Constants.MIN_DIFFICULTY) *
                                                          (1f - Mathf.Exp(-Constants.DIFFICULTY_FACTOR * level)));
        }

        private static float GetMinLevelDifficulty(int level)
        {
            return Mathf.Max(Constants.MIN_DIFFICULTY,GetMaxLevelDifficulty(level) * (1f - Constants.DIFFICULTY_MAX_MARGIN));
        }

        private Vector3 GetBallNextPosition()
        {
            var pos = Ball.gameObject.transform.localPosition - _delta;

            if (pos.x > Constants.BALL_MAX_X)
                pos.x = Constants.BALL_MAX_X;

            if (pos.x < -Constants.BALL_MAX_X)
                pos.x = -Constants.BALL_MAX_X;

            return pos;
        }

        private IEnumerator SmoothMove()
        {
            var posBall = GetBallNextPosition();

            var sqrRemainingDistance = _nextLevel.transform.position.sqrMagnitude;
            while (sqrRemainingDistance > float.Epsilon)
            {
                var newPosition =
                    Vector3.MoveTowards(Ball.transform.position, posBall, _inverseMoveTime * Time.deltaTime);
                Ball.transform.position = newPosition;

                newPosition = Vector3.MoveTowards(_currentLevel.transform.position, -_delta,
                    _inverseMoveTime * Time.deltaTime);
                _currentLevel.transform.position = newPosition;

                newPosition = Vector3.MoveTowards(_nextLevel.transform.position, Vector3.zero,
                    _inverseMoveTime * Time.deltaTime);
                _nextLevel.transform.position = newPosition;

                sqrRemainingDistance = _nextLevel.transform.position.sqrMagnitude;
                yield return null;
            }

            Ball.Release();
            Destroy(_currentLevel);
            _currentLevel = _nextLevel;
        }
    }
}