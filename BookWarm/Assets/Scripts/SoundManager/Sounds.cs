using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ���p�̐ݒ�t�@�C��
/// BGM,SE�̈ꗗ�̗񋓑�.
/// �񋓂��L�[�ɂ����t�@�C�����̘A�z�z����`
/// </summary>
public class Sounds
{
    /// <summary>
    /// BGM�̈ꗗ
    ///�@�Q�[���V�[���̃V�[�����̕����Ȃ�₷��
    /// </summary>
    public enum BGMType
    {
        Title,
        StageSelect,
        Grassland,
        Desert,
        Cave,

        // �ǉ�����
    }

    /// <summary>
    /// SE�̈ꗗ
    /// </summary>
    public enum SEType
    {
        CursorMove,
        Attack,
        PlayerMove,
        Submit,
        

        // �ǉ�����
    }


    [Tooltip("�f�t�H���g��BGM�̃t�F�[�h�A�E�g����")]
    public const float DEFAULT_FADE_DURATION = 0.3f;



    [Tooltip("BGM�̋��ʃp�X")]
    public const string BGM_PATH_PREFIX = "Sounds/BGM/";
    [Tooltip("SE�̋��ʃp�X")]
    public const string SE_PATH_PREFIX = "Sounds/SE/";


    /// <summary>
    /// <see cref="BGMType"/>���L�[�Ƃ���BGM�̃t�@�C�����̘A�z�z��
    /// </summary>
    public static Dictionary<BGMType, string> BGM_PATHS { get; } = new Dictionary<BGMType, string>
    {
        {BGMType.Title,         "Title" },
        {BGMType.StageSelect,   "StageSelect" },
        {BGMType.Grassland,     "Grassland" },
        {BGMType.Desert,        "Desert" },
        {BGMType.Cave,          "Cave" },

    };

    [Tooltip("BGM�̃{�����[��")]
    public static Dictionary<BGMType, float> BGM_VOLUMES { get; } = new Dictionary<BGMType, float>
    {
        {BGMType.Title,         0.6f },
        {BGMType.StageSelect,   0.8f },
        {BGMType.Grassland,     0.8f },
        {BGMType.Desert,        0.8f },
        {BGMType.Cave,          0.8f },

    };

    [Tooltip("SE�̃{�����[��")]
    public static Dictionary<SEType, float> SE_VOLUMES { get; } = new Dictionary<SEType, float>
    {
        {SEType.CursorMove,     1.0f },
        {SEType.Attack,         1.0f },
        {SEType.Submit,         1.0f },
    };

    /// <summary>
    /// <see cref="SEType"/>���L�[�Ƃ���SE�̃t�@�C�����̘A�z�z��
    /// </summary>
    public static Dictionary<SEType, string> SE_PATHS { get; } = new Dictionary<SEType, string>
    {
        {SEType.CursorMove,     "CursorMove" },

        {SEType.PlayerMove,     "PlayerMove" },
        {SEType.Submit,             "Button" },
    };
}
