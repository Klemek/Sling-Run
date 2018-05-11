using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    #region Global Constants

    public const string Version = "alpha 1.0.9";

    public const int MainMenuScene = 0;
    public const int GameScene = 1;

    public const string HighscoreKey = "highscore";

    public const string HighscoreText = "HighScore : {0:d}";

    #endregion

    #region Menu Constants

    #endregion

    #region Game Constants

    public const string UicontrollerObjectName = "Canvas";

    public const string NewHighscoreText = "New HighScore !";
    public const float UiAnimationTime = 0.25f;


    #region Tags

    public const string RespawnTag = "Respawn";
    public const string FinishTag = "Finish";

    #endregion

    public const int StartLife = 5;

    #region Level Manager

    public const float LevelMoveTime = 0.1f;
    public const float LevelHeight = 9f;
    public const float BallMaxX = 1.1f;

    public const float LevelMinY = -1.5f;
    public const float LevelMaxY = 4f;
    public const float LevelMaxX = 3f;
    public const float LevelFragmentMinMargin = 1f;
    public const float LevelFragmentMaxMargin = 1.5f;

    public const int MaxFragmentNumber = 3;
    public const int MaxMovSpeed = 6;
    public const int MaxRotSpeed = 8;

    public const float MinPathWidth = 1f;
    private const float MaxPathWidth = 5f;
    public const float MaxAreaDistBall = 0.5f;

    public const int MovSpeedDifficulty = 30;
    public const int RotSpeedDifficulty = 25;

    public const float AreaWidthFactor = -0.5f;

    public static readonly float MinDifficulty =
        (MovSpeedDifficulty + RotSpeedDifficulty) * Mathf.Pow(MaxPathWidth, AreaWidthFactor);

    public static readonly float MaxDifficulty = MaxFragmentNumber *
                                                 ((MaxMovSpeed + 1) * MovSpeedDifficulty +
                                                  (MaxRotSpeed + 1) * RotSpeedDifficulty) *
                                                 Mathf.Pow(MinPathWidth, AreaWidthFactor);


    public const float DifficultyFactor = 0.002f;
    public const float DifficultyMaxMargin = 0.2f;

    #endregion

    #region Wall Manager

    public const float WallMinMSpeed = 0.01f;
    public const float WallMSpeedFactor = 0.015f;
    public const float WallMinRSpeed = 0.3f;
    public const float WallRSpeedFactor = 1f;

    public const int WallPrecision = 12;

    public enum WallType
    {
        Default = 0,
        Breakable = 1,
        Bouncing = 2,
        Sticky = 3
    }

    public static readonly Dictionary<WallType, string> WallTags = new Dictionary<WallType, string>
    {
        {WallType.Default, "DefaultWall"},
        {WallType.Breakable, "BreakableWall"},
        {WallType.Bouncing, "BouncingWall"},
        {WallType.Sticky, "StickyWall"}
    };

    public static readonly Dictionary<WallType, Color> WallColors = new Dictionary<WallType, Color>
    {
        {WallType.Default, Color.white},
        {WallType.Breakable, 0xFFAB91FF.ToColor()},
        {WallType.Bouncing, 0x81D4FAFF.ToColor()},
        {WallType.Sticky, 0xC5E1A5FF.ToColor()}
    };

    #endregion

    #region Player Controller

    public const float BallMaxMagnitude = 1.5f;
    public const float BallMinSpeed = 1f;
    public const float BallResetSpeed = 0.5f;
    public const float BallTouchSize = 0.6f;
    public static readonly Color PredictLineColor = Color.white;
    public const float PredictLineThickness = 0.05f;
    public const float PredictLineDashLength = 0.1f;
    public const float BallRespawnTime = 0.05f;

    public static readonly Vector3 MousePosDepth = new Vector3(0, 0, -5);
    public static readonly Vector3 PredictLineDepth = new Vector3(0, 0, 1);

    #endregion

    #endregion
}