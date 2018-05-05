using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlingRun
{
    public class LevelManager : MonoBehaviour
    {
        public Level[] levels;
        public PlayerController ballTemplate;
        public GameObject borders;
        public float moveTime;

        public float levelHeight;
        public float levelMaxX;

        private GameObject currentLevel;
        private GameObject nextLevel;
        internal PlayerController ball;
        private Vector3 delta;
        private float inverseMoveTime;
        
        private void Awake()
        {
            delta = new Vector3(0, levelHeight, 0);
            inverseMoveTime = 1f / moveTime;
        }
        
        internal void LoadLevel(bool start, int level)
        {
            if (start)
            {
                currentLevel = Instantiate(levels[0].gameObject, Vector3.zero, Quaternion.identity);
                ball = Instantiate(ballTemplate, ballTemplate.transform.position, Quaternion.identity);
                Instantiate(borders, borders.transform.position, Quaternion.identity);
            }
            else
            {
                //TODO choose level from difficulty
                var r = new System.Random();
                nextLevel = Instantiate(levels[r.Next(levels.Length)].gameObject, delta, Quaternion.identity);
                StartCoroutine(SmoothMove());
            }
        }

        private Vector3 getBallNextPosition()
        {
            var pos = ball.gameObject.transform.position - delta;

            if (pos.x > levelMaxX)
                pos.x = levelMaxX;

            if (pos.x < -levelMaxX)
                pos.x = -levelMaxX;

            return pos;
        }
        
        private IEnumerator SmoothMove()
        {
            var posBall = getBallNextPosition();
            
            var sqrRemainingDistance = nextLevel.transform.position.sqrMagnitude;
            while (sqrRemainingDistance > float.Epsilon)
            {
                var newPosition = Vector3.MoveTowards(ball.transform.position, posBall, inverseMoveTime * Time.deltaTime);
                ball.transform.position = newPosition;
                
                newPosition = Vector3.MoveTowards(currentLevel.transform.position, -delta, inverseMoveTime * Time.deltaTime);
                currentLevel.transform.position = newPosition;
                
                newPosition = Vector3.MoveTowards(nextLevel.transform.position, Vector3.zero, inverseMoveTime * Time.deltaTime);
                nextLevel.transform.position = newPosition;
                
                sqrRemainingDistance = nextLevel.transform.position.sqrMagnitude;
                yield return null;
            }

            ball.Release();
            Destroy(currentLevel);
            currentLevel = nextLevel;

        }
    }
}