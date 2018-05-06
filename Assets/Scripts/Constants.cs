using UnityEngine;

namespace SlingRun
{
    public static class Constants
    {
        #region Global Constants

        public const string VERSION = "alpha 1.0.3";

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

        #region Level Manager

        public const float LEVEL_MOVE_TIME = 0.1f;
        public const float LEVEL_HEIGHT = 9f;
        public const float BALL_MAX_X = 1.1f;

        public const float LEVEL_MIN_Y = -1.5f;
        public const float LEVEL_MAX_Y = 4.5f;
        public const float LEVEL_FRAGMENT_MIN_MARGIN = 1f;
        public const float LEVEL_FRAGMENT_MAX_MARGIN = 1.5f;

        public const int MAX_FRAGMENT_NUMBER = 3;
        private const int MAX_FRAGMENT_DIFFICULTY = 3;
        public const int MAX_DIFFICULTY = MAX_FRAGMENT_NUMBER * MAX_FRAGMENT_DIFFICULTY;
        public const float DIFFICULTY_FACTOR = 0.06f;
        public const float DIFFICULTY_MAX_MARGIN = 0.2f;

        #endregion

        #region Player Controller

        public const float BALL_MAX_MAGNITUDE = 1.5f;
        public const float BALL_MIN_SPEED = 1f;
        public const float BALL_RESET_SPEED = 0.5f;
        public const float BALL_TOUCH_SIZE = 0.3f;
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