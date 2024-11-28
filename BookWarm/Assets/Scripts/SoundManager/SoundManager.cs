
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


/// <summary>
/// BGM��SE���Ǘ�����N���X
/// �V���O���g���ŃC���X�^���X������
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Tooltip("���Ǘ��N���X�̃C���X�^���X")]
    public static SoundManager Instance { get; private set; }


    [Tooltip("BGMManager�̃C���X�^���X")]
    BGMManager bgmManager;
    [Tooltip("SEManager�̃C���X�^���X")]
    SEManager seManager;


    private void Awake()
    {
        // �V���O���g���ɂ���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // �R���|�[�l���g�ǉ�
        var bgmSource = gameObject.AddComponent<AudioSource>();
        var seSource = gameObject.AddComponent<AudioSource>();
        // �R���X�g���N�^�Ăяo��
        bgmManager = new BGMManager(bgmSource);
        seManager = new SEManager(seSource);
        // �V�[���؂�ւ��C�x���g�Ɋ֐���ǉ�
        SceneManager.sceneLoaded += OnSceneLoded;
    }

    /// <summary>
    /// �I�u�W�F�N�g�j�����̏���
    /// </summary>
    private void OnDestroy()
    {
        // �C�x���g�ɒǉ������������폜����
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    /// <summary>
    /// <para>�V�[���؂�ւ����ɃV�[���ɑΉ�����BGM��ǂݍ���ōĐ����鏈��</para>
    /// <see cref="BGMManager.IsPlayingBGM"/>���ݍĐ����Ă��邩���m�F���āA�Đ�����Ă�����
    /// <para><see cref="FadeOutAndStopBGM(Sounds.BGMType)"/>�t�F�[�h�A�E�g������</para>
    /// <para><see cref="PlayBGM(Sounds.BGMType)"/>�����łȂ��Ȃ炻�̂܂܍Đ�</para>
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        if (Enum.TryParse(scene.name, out Sounds.BGMType bgmType))
        {
            
            // ��BGM������Ă��邩�ǂ���
            if (bgmManager.IsPlayingAudio())
            {
                // �܂��A������Ă郂�m���~�߂�
                // ���ʂ����������ăt�F�[�h�A�E�g������
                StartCoroutine(FadeOutAndStopBGM(bgmType));
            }
            else
            {
                // ����BGM�𗬂������ɓ���
                PlayBGM(bgmType);
            }
        }
        else if(scene.name == "GameScene")
        {
            var stageId = PlayerPrefs.GetInt(Common.Common.KEY_SELECTED_STAGE_ID);
            var idName = LoadData.GetGameData(stageId).stage_data.stage_id_name;
            
            if(Enum.TryParse(idName, out bgmType))
            {
                // ��BGM������Ă��邩�ǂ���
                if (bgmManager.IsPlayingAudio())
                {
                    // �܂��A������Ă郂�m���~�߂�
                    // ���ʂ����������ăt�F�[�h�A�E�g������
                    StartCoroutine(FadeOutAndStopBGM(bgmType));
                }
                else
                {
                    // ����BGM�𗬂������ɓ���
                    PlayBGM(bgmType);
                }
            }
        }
    }

    /// <summary>
    /// �t�F�[�h�A�E�g�����邽�߂̔񓯊�����
    /// </summary>
    /// <returns>�t�F�[�h�A�E�g�̂��߂̃R���[�`��</returns>
    IEnumerator FadeOutAndStopBGM(Sounds.BGMType bgmType)
    {
        // �ŏ��̉��ʂ��L��
        var startVol = bgmManager.GetAudioVolume();

        while (bgmManager.GetAudioVolume() > 0)
        {
            // �w��b��������
            bgmManager.FadeOutAudio(startVol * Time.deltaTime / Sounds.DEFAULT_FADE_DURATION);
            yield return null;
        }
        bgmManager.StopAudio();
        PlayBGM(bgmType);
    }


    /// <summary>
    /// <see cref="SEManager.PlayAudio(AudioClip, bool)"/>��<see cref="BGMManager.PlayAudio(AudioClip, bool)"/>
    /// �ŌĂяo�����ʂ̉����Đ����鏈��
    /// </summary>
    /// <param name="pathPrefix">�����t�@�C���̑O���p�X</param>
    /// <param name="audioPaths"><see cref="Sounds"/>�ɓo�^����Ă���t�@�C�����̘A�z�z��</param>
    /// <param name="audioType">�Đ����鉹�̎��<see cref="Sounds.BGMType"/>��<see cref="Sounds.SEType"/></param>
    /// <param name="audioManager"><see cref="BGMManager"/>��<see cref="SEManager"/>�̃C���X�^���X</param>
    /// <param name="isLoop">���[�v�t���O</param>
    void PlayAudio<T>(string pathPrefix, Dictionary<T, string> audioPaths, T audioType, AudioManagerBase audioManager, bool isLoop)where T:Enum
    {
        // �L�[����t�@�C�������擾����
        if (audioPaths.TryGetValue(audioType, out string fileName))
        {
            // �擾�����t�@�C��������t�@�C���̃p�X���쐬
            string clipPath = pathPrefix + fileName;

            // �����t�@�C���̓ǂݍ���
            var clip = Resources.Load<AudioClip>(clipPath);
            // �ǂݍ��߂�����s
            if (clip != null)
            {
                //print($"{clip}���Đ����܂�");
                audioManager.PlayAudio(clip, isLoop);
            }
            // �ǂݍ��߂Ȃ�����
            else
            {
                Debug.LogError($"�Y�������t�@�C���Ȃ�: {clipPath}");
            }
        }
        // �L�[����t�@�C�������擾�ł��Ȃ�����
        else
        {
            Debug.LogError($"�w�艹���^�C�v�̒�`�Ȃ�: {audioType}");
        }
    }


    /// <summary>
    /// BGM�̍Đ�����
    /// Sounds�N���X�̒�`����Ă���BGM��炷(Loop)
    /// </summary>
    public void PlayBGM(Sounds.BGMType bgmType, bool isLoop = true)
    {
        PlayAudio(Sounds.BGM_PATH_PREFIX, Sounds.BGM_PATHS, bgmType, bgmManager, isLoop);
    }

    /// <summary>
    /// BGM�̒�~����
    /// </summary>
    public void StopBGM()
    {
        bgmManager.StopAudio();
    }

    /// <summary>
    /// SE�̍Đ�����
    /// Sounds�N���X�̒�`����Ă���SE��炷(��Loop)
    /// </summary>
    public void PlaySE(Sounds.SEType seType, bool isLoop = false)
    {
        PlayAudio(Sounds.SE_PATH_PREFIX, Sounds.SE_PATHS, seType, seManager, isLoop);
    }

    /// <summary>
    /// SE���~�����鏈��
    /// </summary>
    public void StopSE()
    {
        seManager.StopAudio();
    }

    /// <summary>
    /// �Q�[���̉��ʂ𒲐����鏈��
    /// </summary>
    /// <param name="normalizedVol">0~1�Ő��K�����ꂽ�l</param>
    public void SetVolumeRate(float normalizedVol)
    {
        bgmManager.SetVolumeRate(normalizedVol);
        seManager.SetVolumeRate(normalizedVol);
    }

    /// <summary>
    /// BGM�̉��ʂ݂̂𒲐����鏈��
    /// </summary>
    /// <param name="normalizedVol">0~1�Ő��K�����ꂽ�l</param>
    public void SetBGMVolumeRate(float normalizedVol)
    {
        bgmManager.SetVolumeRate(normalizedVol);
    }

    /// <summary>
    /// SE�̉��ʂ݂̂𒲐����鏈��
    /// </summary>
    /// <param name="normalizedVol">0~1�Ő��K�����ꂽ�l</param>
    public void SetSEVolumeRate(float normalizedVol)
    {
        bgmManager.SetVolumeRate(normalizedVol);
    }
}
