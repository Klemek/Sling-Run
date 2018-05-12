using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Constants
{
    #region Global Constants

    public const string Version = "alpha 1.0.11";

    public const int MainMenuScene = 0;
    public const int GameScene = 1;

    public const string HighscoreKey = "highscore";
    public const string MoneyKey = "money";
    
    public const string HighscoreText = "HighScore : {0:d}";

    #endregion

    #region Menu Constants

    #endregion

    #region Game Constants

    public const string UicontrollerObjectName = "Canvas";

    public const string NewHighscoreText = "New HighScore !";
    public const float UiAnimationTime = 0.25f;


    #region Tags

    public const string DefaultTag = "Untagged";
    public const string RespawnTag = "Respawn";
    public const string FinishTag = "Finish";

    public const string HeartTag = "Heart";
    public const string CooperCoinTag = "CooperCoin";
    public const string SilverCoinTag = "SilverCoin";
    public const string GoldCoinTag = "GoldCoin";

    public const string DefaultWallTag = "DefaultWall";
    public const string BreakableWallTag = "BreakableWall";
    public const string BouncingWallTag = "BouncingWall";
    public const string StickyWallTag = "StickyWall";
    public const string DeadlyWallTag = "DeadlyWall";

    #endregion

    public const int CooperCoinValue = 1;
    public const int SilverCoinValue = 5;
    public const int GoldCoinValue = 20;

    public const int StartLife = 5;

    #region Wall Manager

    public const float WallMinMSpeed = 0.01f;
    public const float WallMSpeedFactor = 0.015f;
    public const float WallMinRSpeed = 0.3f;
    public const float WallRSpeedFactor = 1f;

    public const int WallPrecision = 12;

    public static readonly Dictionary<WallType, string> WallTags = new Dictionary<WallType, string>
    {
        {WallType.Default, DefaultWallTag},
        {WallType.Breakable, BreakableWallTag},
        {WallType.Bouncing, BouncingWallTag},
        {WallType.Sticky, StickyWallTag},
        {WallType.Deadly, DeadlyWallTag}
    };

    public static readonly Dictionary<WallType, Color> WallColors = new Dictionary<WallType, Color>
    {
        {WallType.Default, Color.white},
        {WallType.Breakable, "#A1887F".ToColor()},
        {WallType.Bouncing, "#4DB6AC".ToColor()},
        {WallType.Sticky, "#FFB74D".ToColor()},
        {WallType.Deadly, "#E57373".ToColor()}
    };

    public static readonly Dictionary<WallType, float> WallDifficulties = new Dictionary<WallType, float>
    {
        {WallType.Default, 1f},
        {WallType.Breakable, 1.1f},
        {WallType.Bouncing, 1.3f},
        {WallType.Sticky, 1.5f},
        {WallType.Deadly, 2.5f}
    };

    #endregion

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

    public static readonly float MinDifficulty =  WallDifficulties.Min(o => o.Value) *
                                                 (MovSpeedDifficulty + RotSpeedDifficulty) *
                                                 Mathf.Pow(MaxPathWidth, AreaWidthFactor);

    public static readonly float MaxDifficulty = WallDifficulties.Max(o => o.Value) * MaxFragmentNumber *
                                                 ((MaxMovSpeed + 1) * MovSpeedDifficulty +
                                                  (MaxRotSpeed + 1) * RotSpeedDifficulty) *
                                                 Mathf.Pow(MinPathWidth, AreaWidthFactor);


    public const float DifficultyFactor = 0.001f;
    public const float DifficultyMaxMargin = 0.2f;

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