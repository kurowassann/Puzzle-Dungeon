using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BGMManager : AudioManagerBase
{

    public BGMManager(AudioSource source) : base(source) { }



    /// <summary>
    /// Œ»İ‚ÌBGM‚Ì‰¹—Ê‚ğæ“¾
    /// </summary>
    /// <returns>Œ»İ‚ÌBGM‚Ì‰¹—Ê</returns>
    public float GetAudioVolume()
    {
        return audioSource.volume;
    }

    /// <summary>
    /// w’è‰¹—Ê¬‚³‚­‚·‚é
    /// </summary>
    /// <param name="decVol">Œ¸­‚·‚é‰¹—Ê</param>
    public void FadeOutAudio(float decVol)
    {
        audioSource.volume -= decVol;
    }

    /// <summary>
    /// ‰¹—Ê‚Ì©“®’²ß‹@”\‚ğ’Ç‰Á‚·‚é
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
