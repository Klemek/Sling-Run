using UnityEngine;

namespace SlingRun
{
    public static class Constants
    {
        #region Global Constants

        public const string VERSION = "alpha 1.0.8";

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
        public const float UI_ANIMATION_TIME = 0.25f;


        public const string RESPAWN_TAG = "Respawn";
        public const string FINISH_TAG = "Finish";

        public const int START_LIFE = 5;

        #region Level Manager

        public const float LEVEL_MOVE_TIME = 0.1f;
        public const float LEVEL_HEIGHT = 9f;
        public const float BALL_MAX_X = 1.1f;

        public const float LEVEL_MIN_Y = -1.5f;
        public const float LEVEL_MAX_Y = 4f;
        public const float LEVEL_MAX_X = 3f;
        public const float LEVEL_FRAGMENT_MIN_MARGIN = 1f;
        public const float LEVEL_FRAGMENT_MAX_MARGIN = 1.5f;

        public const int MAX_FRAGMENT_NUMBER = 3;
        public const int MAX_MOV_SPEED = 6;
        public const int MAX_ROT_SPEED = 8;

        public const float MIN_PATH_WIDTH = 1f;
        private const float MAX_PATH_WIDTH = 5f;
        public const float MAX_AREA_DIST_BALL = 0.5f;
        
        public const int MOV_SPEED_DIFFICULTY = 30;
        public const int ROT_SPEED_DIFFICULTY = 25;
        
        public const float AREA_WIDTH_FACTOR = -0.5f;
        
        public static readonly float MIN_DIFFICULTY = (MOV_SPEED_DIFFICULTY + ROT_SPEED_DIFFICULTY) * Mathf.Pow(MAX_PATH_WIDTH, AREA_WIDTH_FACTOR);
        public static readonly float MAX_DIFFICULTY = MAX_FRAGMENT_NUMBER *
                                             ((MAX_MOV_SPEED + 1) * MOV_SPEED_DIFFICULTY +
                                              (MAX_ROT_SPEED + 1) * ROT_SPEED_DIFFICULTY) * Mathf.Pow(MIN_PATH_WIDTH, AREA_WIDTH_FACTOR);

        
        public const float DIFFICULTY_FACTOR = 0.002f;
        public const float DIFFICULTY_MAX_MARGIN = 0.2f;

        #endregion
        
        #region Wall Manager
        
        public const float WALL_MIN_M_SPEED = 0.01f;
        public const float WALL_M_SPEED_FACTOR = 0.015f;
        public const float WALL_MIN_R_SPEED = 0.3f;
        public const float WALL_R_SPEED_FACTOR = 1f;

        public const int WALL_PRECISION = 12;
        
        #endregion

        #region Player Controller

        public const float BALL_MAX_MAGNITUDE = 1.5f;
        public const float BALL_MIN_SPEED = 1f;
        public const float BALL_RESET_SPEED = 0.5f;
        public const float BALL_TOUCH_SIZE = 0.6f;
        public static readonly Color PREDICT_LINE_COLOR = Color.white;
        public const float PREDICT_LINE_THICKNESS = 0.05f;
        public const float PREDICT_LINE_DASH_LENGTH = 0.1f;
        public const float BALL_RESPAWN_TIME = 0.05f;

        public static readonly Vector3 MOUSE_POS_DEPTH = new Vector3(0, 0, -5);
        public static readonly Vector3 PREDICT_LINE_DEPTH = new Vector3(0, 0, 1);

        #endregion

        #endregion
    }
}