using UnityEngine;
using System.Collections;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private AudioClip sound;

    public void OnPlayMusic(int selector)
    {
        switch (selector)
        {
            case 1:
                Managers.Audio.PLayIntroMusic();
                break;
            case 2:
                Managers.Audio.PlayLevelMusic();
                break;
            default:
                Managers.Audio.StopMusic();
                break;
        }
    }

    public void OnMusicToggle()
    {
        Managers.Audio.musicMute = !Managers.Audio.musicMute;
        Managers.Audio.PlaySound(sound);
    }

    public void OnMusicValue(float volume)
    {
        Managers.Audio.musicVolume = volume;
    }

    public void OnSoundToggle()
    {
        //这个按钮将切换静音属性
        Managers.Audio.soundMute = !Managers.Audio.soundMute;
        //当按下按钮时播放音效
        Managers.Audio.PlaySound(sound);
    }

    public void OnSoundValue(float volume)
    {
        //这个滑动条将调整AudioManager中的音量属性
        Managers.Audio.soundVolume = volume;
    }

}
