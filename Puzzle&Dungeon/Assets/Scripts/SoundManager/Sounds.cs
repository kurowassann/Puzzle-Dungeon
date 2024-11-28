using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 音用の設定ファイル
/// BGM,SEの一覧の列挙体.
/// 列挙をキーにしたファイル名の連想配列を定義
/// </summary>
public class Sounds
{
    /// <summary>
    /// BGMの一覧
    ///　ゲームシーンのシーン名の方がなりやすい
    /// </summary>
    public enum BGMType
    {
        Title,
        StageSelect,
        Grassland,
        Desert,
        Cave,

        // 追加する
    }

    /// <summary>
    /// SEの一覧
    /// </summary>
    public enum SEType
    {
        CursorMove,
        Attack,
        PlayerMove,
        Submit,
        

        // 追加する
    }


    [Tooltip("デフォルトのBGMのフェードアウト時間")]
    public const float DEFAULT_FADE_DURATION = 0.3f;



    [Tooltip("BGMの共通パス")]
    public const string BGM_PATH_PREFIX = "Sounds/BGM/";
    [Tooltip("SEの共通パス")]
    public const string SE_PATH_PREFIX = "Sounds/SE/";


    /// <summary>
    /// <see cref="BGMType"/>をキーとしたBGMのファイル名の連想配列
    /// </summary>
    public static Dictionary<BGMType, string> BGM_PATHS { get; } = new Dictionary<BGMType, string>
    {
        {BGMType.Title,         "Title" },
        {BGMType.StageSelect,   "StageSelect" },
        {BGMType.Grassland,     "Grassland" },
        {BGMType.Desert,        "Desert" },
        {BGMType.Cave,          "Cave" },

    };

    [Tooltip("BGMのボリューム")]
    public static Dictionary<BGMType, float> BGM_VOLUMES { get; } = new Dictionary<BGMType, float>
    {
        {BGMType.Title,         0.6f },
        {BGMType.StageSelect,   0.8f },
        {BGMType.Grassland,     0.8f },
        {BGMType.Desert,        0.8f },
        {BGMType.Cave,          0.8f },

    };

    [Tooltip("SEのボリューム")]
    public static Dictionary<SEType, float> SE_VOLUMES { get; } = new Dictionary<SEType, float>
    {
        {SEType.CursorMove,     1.0f },
        {SEType.Attack,         1.0f },
        {SEType.Submit,         1.0f },
    };

    /// <summary>
    /// <see cref="SEType"/>をキーとしたSEのファイル名の連想配列
    /// </summary>
    public static Dictionary<SEType, string> SE_PATHS { get; } = new Dictionary<SEType, string>
    {
        {SEType.CursorMove,     "CursorMove" },

        {SEType.PlayerMove,     "PlayerMove" },
        {SEType.Submit,             "Button" },
    };
}
