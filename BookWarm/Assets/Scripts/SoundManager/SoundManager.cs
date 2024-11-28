
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


/// <summary>
/// BGMとSEを管理するクラス
/// シングルトンでインスタンスを持つ
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Tooltip("音管理クラスのインスタンス")]
    public static SoundManager Instance { get; private set; }


    [Tooltip("BGMManagerのインスタンス")]
    BGMManager bgmManager;
    [Tooltip("SEManagerのインスタンス")]
    SEManager seManager;


    private void Awake()
    {
        // シングルトンにする
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // コンポーネント追加
        var bgmSource = gameObject.AddComponent<AudioSource>();
        var seSource = gameObject.AddComponent<AudioSource>();
        // コンストラクタ呼び出し
        bgmManager = new BGMManager(bgmSource);
        seManager = new SEManager(seSource);
        // シーン切り替えイベントに関数を追加
        SceneManager.sceneLoaded += OnSceneLoded;
    }

    /// <summary>
    /// オブジェクト破棄時の処理
    /// </summary>
    private void OnDestroy()
    {
        // イベントに追加した処理を削除する
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    /// <summary>
    /// <para>シーン切り替え時にシーンに対応したBGMを読み込んで再生する処理</para>
    /// <see cref="BGMManager.IsPlayingBGM"/>現在再生しているかを確認して、再生されていたら
    /// <para><see cref="FadeOutAndStopBGM(Sounds.BGMType)"/>フェードアウトさせる</para>
    /// <para><see cref="PlayBGM(Sounds.BGMType)"/>そうでないならそのまま再生</para>
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        if (Enum.TryParse(scene.name, out Sounds.BGMType bgmType))
        {
            
            // 今BGMが流れているかどうか
            if (bgmManager.IsPlayingAudio())
            {
                // まず、今流れてるモノを止める
                // 音量を小さくしてフェードアウトさせる
                StartCoroutine(FadeOutAndStopBGM(bgmType));
            }
            else
            {
                // つぎのBGMを流す処理に入る
                PlayBGM(bgmType);
            }
        }
        else if(scene.name == "GameScene")
        {
            var stageId = PlayerPrefs.GetInt(Common.Common.KEY_SELECTED_STAGE_ID);
            var idName = LoadData.GetGameData(stageId).stage_data.stage_id_name;
            
            if(Enum.TryParse(idName, out bgmType))
            {
                // 今BGMが流れているかどうか
                if (bgmManager.IsPlayingAudio())
                {
                    // まず、今流れてるモノを止める
                    // 音量を小さくしてフェードアウトさせる
                    StartCoroutine(FadeOutAndStopBGM(bgmType));
                }
                else
                {
                    // つぎのBGMを流す処理に入る
                    PlayBGM(bgmType);
                }
            }
        }
    }

    /// <summary>
    /// フェードアウトさせるための非同期処理
    /// </summary>
    /// <returns>フェードアウトのためのコルーチン</returns>
    IEnumerator FadeOutAndStopBGM(Sounds.BGMType bgmType)
    {
        // 最初の音量を記憶
        var startVol = bgmManager.GetAudioVolume();

        while (bgmManager.GetAudioVolume() > 0)
        {
            // 指定秒数かけて
            bgmManager.FadeOutAudio(startVol * Time.deltaTime / Sounds.DEFAULT_FADE_DURATION);
            yield return null;
        }
        bgmManager.StopAudio();
        PlayBGM(bgmType);
    }


    /// <summary>
    /// <see cref="SEManager.PlayAudio(AudioClip, bool)"/>と<see cref="BGMManager.PlayAudio(AudioClip, bool)"/>
    /// で呼び出す共通の音を再生する処理
    /// </summary>
    /// <param name="pathPrefix">音声ファイルの前半パス</param>
    /// <param name="audioPaths"><see cref="Sounds"/>に登録されているファイル名の連想配列</param>
    /// <param name="audioType">再生する音の種類<see cref="Sounds.BGMType"/>か<see cref="Sounds.SEType"/></param>
    /// <param name="audioManager"><see cref="BGMManager"/>か<see cref="SEManager"/>のインスタンス</param>
    /// <param name="isLoop">ループフラグ</param>
    void PlayAudio<T>(string pathPrefix, Dictionary<T, string> audioPaths, T audioType, AudioManagerBase audioManager, bool isLoop)where T:Enum
    {
        // キーからファイル名を取得する
        if (audioPaths.TryGetValue(audioType, out string fileName))
        {
            // 取得したファイル名からファイルのパスを作成
            string clipPath = pathPrefix + fileName;

            // 音声ファイルの読み込み
            var clip = Resources.Load<AudioClip>(clipPath);
            // 読み込めたら実行
            if (clip != null)
            {
                //print($"{clip}を再生します");
                audioManager.PlayAudio(clip, isLoop);
            }
            // 読み込めなかった
            else
            {
                Debug.LogError($"該当音声ファイルなし: {clipPath}");
            }
        }
        // キーからファイル名を取得できなかった
        else
        {
            Debug.LogError($"指定音声タイプの定義なし: {audioType}");
        }
    }


    /// <summary>
    /// BGMの再生処理
    /// Soundsクラスの定義されているBGMを鳴らす(Loop)
    /// </summary>
    public void PlayBGM(Sounds.BGMType bgmType, bool isLoop = true)
    {
        PlayAudio(Sounds.BGM_PATH_PREFIX, Sounds.BGM_PATHS, bgmType, bgmManager, isLoop);
    }

    /// <summary>
    /// BGMの停止処理
    /// </summary>
    public void StopBGM()
    {
        bgmManager.StopAudio();
    }

    /// <summary>
    /// SEの再生処理
    /// Soundsクラスの定義されているSEを鳴らす(非Loop)
    /// </summary>
    public void PlaySE(Sounds.SEType seType, bool isLoop = false)
    {
        PlayAudio(Sounds.SE_PATH_PREFIX, Sounds.SE_PATHS, seType, seManager, isLoop);
    }

    /// <summary>
    /// SEを停止させる処理
    /// </summary>
    public void StopSE()
    {
        seManager.StopAudio();
    }

    /// <summary>
    /// ゲームの音量を調整する処理
    /// </summary>
    /// <param name="normalizedVol">0~1で正規化された値</param>
    public void SetVolumeRate(float normalizedVol)
    {
        bgmManager.SetVolumeRate(normalizedVol);
        seManager.SetVolumeRate(normalizedVol);
    }

    /// <summary>
    /// BGMの音量のみを調整する処理
    /// </summary>
    /// <param name="normalizedVol">0~1で正規化された値</param>
    public void SetBGMVolumeRate(float normalizedVol)
    {
        bgmManager.SetVolumeRate(normalizedVol);
    }

    /// <summary>
    /// SEの音量のみを調整する処理
    /// </summary>
    /// <param name="normalizedVol">0~1で正規化された値</param>
    public void SetSEVolumeRate(float normalizedVol)
    {
        bgmManager.SetVolumeRate(normalizedVol);
    }
}
