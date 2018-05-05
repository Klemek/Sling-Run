using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace SlingRun
{
    public static class Constants
    {
        #region Global Constants

        public const int MAIN_MENU_SCENE = 0;
        public const int GAME_SCENE = 1;

        public const string HIGHSCORE_KEY = "highscore";
        
        public const string HIGHSCORE_TEXT = "HighScore : {0:d}";

        #endregion

        #region Menu Constants

        #endregion

        #region Game Constants

        public const string UICONTROLLER_OBJECT_NAME = "Canvas";
        
        public const string NEW_HIGHSCORE_TEXT = "New HighScore !";

        public const string RESPAWN_TAG = "Respawn";
        public const string FINISH_TAG = "Finish";

        public const int START_LIFE = 3;
        
        #region Player Controller
        
        public const float BALL_MAX_MAGNITUDE = 1.5f;
        public const float BALL_MIN_SPEED = 0.3f;
        public const float BALL_TOUCH_SIZE = 0.3f;
        public static readonly Color PREDICT_LINE_COLOR = Color.white;
        public const float PREDICT_LINE_THICKNESS = 0.05f;
        public const float PREDICT_LINE_DASH_LENGTH = 0.1f;
        
        public static readonly Vector3 MOUSE_POS_DEPTH = new Vector3(0,0,-5);
        public static readonly Vector3 PREDICT_LINE_DEPTH = new Vector3(0,0,1);
        
        #endregion

        #endregion
    }
}