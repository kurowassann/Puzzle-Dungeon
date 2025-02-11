using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BGMManager : AudioManagerBase
{

    public BGMManager(AudioSource source) : base(source) { }



    /// <summary>
    /// 現在のBGMの音量を取得
    /// </summary>
    /// <returns>現在のBGMの音量</returns>
    public float GetAudioVolume()
    {
        return audioSource.volume;
    }

    /// <summary>
    /// 指定音量小さくする
    /// </summary>
    /// <param name="decVol">減少する音量</param>
    public void FadeOutAudio(float decVol)
    {
        audioSource.volume -= decVol;
    }

    /// <summary>
    /// 音量の自動調節機能を追加する
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="isLoop"></param>
    public override void PlayAudio(AudioClip clip, bool isLoop)
    {
        var key = Sounds.BGM_PATHS.FirstOrDefault(kvp => kvp.Value == clip.name).Key;
        volumeDefault = Sounds.BGM_VOLUMES[key];

        audioSource.volume = volumeDefault * volumeRate;

        base.PlayAudio(clip, isLoop);
    }
}
