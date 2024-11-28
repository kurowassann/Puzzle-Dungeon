using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="BGMManager"/>と<see cref="SEManager"/>の共通基底クラス
/// </summary>
public abstract class AudioManagerBase
{
    protected AudioSource audioSource;

    protected float volumeRate;
    protected float volumeDefault;

    public AudioManagerBase(AudioSource source)
    {
        audioSource = source;
        volumeRate = 1.0f;
        volumeDefault = 1.0f;
    }

    /// <summary>
    /// 音声の再生
    /// </summary>
    /// <param name="clip">音声ファイル</param>
    /// <param name="isLoop">ループ再生するか</param>
    public virtual void PlayAudio(AudioClip clip, bool isLoop)
    {
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }

    /// <summary>
    /// BGMの停止
    /// </summary>
    public void StopAudio()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// BGMが再生中かどうか
    /// </summary>
    /// <returns>再生中ならtrue</returns>
    public bool IsPlayingAudio()
    {
        return audioSource.isPlaying;
    }

    /// <summary>
    /// 音量の倍率を設定
    /// </summary>
    public void SetVolumeRate(float _rate)
    {
        volumeRate = _rate;
        //Debug.Log($"音量 def:{volumeDefault}, rate:{volumeRate}");
        audioSource.volume = volumeDefault * volumeRate;
    }
}

