using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace SlingRun
{
    public class LevelManager : MonoBehaviour
    {
        
        private Vector3 _delta;
        private float _inverseMoveTime;
        private GameObject _currentLevel;
        private GameObject _nextLevel;
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
            }
            else
            {
                _nextLevel = GenerateLevel(level);
                _nextLevel.transform.position = _delta;

                StartCoroutine(CoroutineUtils.SmoothMove(Constants.LEVEL_MOVE_TIME, FinishedMoving,
                    new[] {Ball.gameObject, _currentLevel, _nextLevel},
                    new [] {GetBallNextPosition(), -_delta, Vector3.zero}));
            }
        }

        private void FinishedMoving()
        {
            Ball.Release();
            Destroy(_currentLevel);
            _currentLevel = _nextLevel;
        }

        private GameObject GenerateLevel(int level)
        {
            var newLevel = new GameObject("Level " + level);
            var r = new Random();

            bool valid;
            int[] frags;
            float h;
            do
            {
                var nFrag = r.Next(1, Constants.MAX_FRAGMENT_NUMBER + 1);
                var diff = 0;
                frags = new int[nFrag];
                h = 0f;
                for (var i = 0; i < nFrag; i++)
                {
                    frags[i] = r.Next(LevelFragments.Length);
                    h += LevelFragments[frags[i]].Height;
                    diff += LevelFragments[frags[i]].Difficulty;
                }

                if (nFrag > 1)
                    h += (nFrag - 1) * Constants.LEVEL_FRAGMENT_MAX_MARGIN;

                valid = h <= Constants.LEVEL_MAX_Y - Constants.LEVEL_MIN_Y && IsDifficultyCorrect(diff, level);
            } while (!valid);

            var endH = Constants.LEVEL_MAX_Y - h;
            var startH = Constants.LEVEL_MIN_Y;

            foreach (var f in frags)
            {
                var fh = LevelFragments[f].Height;

                endH += fh;

                var py = NextFloat(r, startH + fh / 2f, endH - fh / 2f);

                var pos = new Vector3(0, py, 0);

                var rotation = Quaternion.identity;
                if (LevelFragments[f].CanFlip && r.Next(2) == 0) //flip horizontaly
                    rotation = Quaternion.AngleAxis(180f, Vector3.up);

                var frag = Instantiate(LevelFragments[f].gameObject, pos, rotation);
                frag.transform.parent = newLevel.transform;

                startH = py + fh / 2 + NextFloat(r, Constants.LEVEL_FRAGMENT_MIN_MARGIN,
                             Constants.LEVEL_FRAGMENT_MAX_MARGIN);
                endH += Constants.LEVEL_FRAGMENT_MAX_MARGIN;
            }

            return newLevel;
        }

        private static bool IsDifficultyCorrect(int diff, int level)
        {
            var maxDiff = GetLevelDifficulty(level);
            var minDiff = Math.Round(maxDiff * (1f - Constants.DIFFICULTY_MAX_MARGIN));
            return minDiff <= diff && diff <= maxDiff;
        }

        private static int GetLevelDifficulty(int level)
        {
            return (int) Math.Round(Constants.MAX_DIFFICULTY * (1f - Math.Exp(-Constants.DIFFICULTY_FACTOR * level)));
        }

        private static float NextFloat(Random r, float min, float max)
        {
            return (float) (r.NextDouble() * (max - min) + min);
        }

        private Vector3 GetBallNextPosition()
        {
            var pos = Ball.gameObject.transform.position - _delta;

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