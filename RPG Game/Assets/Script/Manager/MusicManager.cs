using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour, IGameManager
{
    //音效
    [SerializeField] private AudioSource source;
    //音乐
    [SerializeField] private AudioSource music;
    //淡入淡出速度
    public float crossFadeRate = 0.2f;
    public ManagerStatus status { get; private set; }

    public float musicNowVolume { get { return music.volume; } }
    public string clipName { get { return music.clip.name; } }

    //音效音量
    private float sourceVolume;
    //音乐音效
    private float musicVolume;
    //是否正在淡入淡出
    private bool crossFading;


    public void Startup(NetworkService service)
    {
        music.ignoreListenerVolume = true;

        updateData(1f, 0.5f, 0f, null);


        status = ManagerStatus.Started;
    }

    //播放音效
    public void playSource(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void playMusic(AudioClip clip)
    {
        Debug.Log(clip.name);
        if (crossFading)
            return;
        StartCoroutine(fadeMusic(clip));
    }

    private IEnumerator fadeMusic(AudioClip clip)
    {
        crossFading = true;
        music.clip = clip;
        bool isMute = music.volume == 0;
        if (!music.isPlaying)
        {
            music.Play();
        }
        bool isOk = false;
        float scaledRate = crossFadeRate * musicVolume;
        Debug.Log("isMute" + isMute);
        while (!isOk)
        {
            if (isMute)
            {
                music.volume += scaledRate * Time.deltaTime;
                // Debug.Log("volume" + music.volume);
            }
            else
            {
                music.volume -= scaledRate * Time.deltaTime;
            }
            //暂停一帧
            yield return null;

            if (music.volume <= 0 || music.volume >= musicVolume)
            {
                isOk = true;
            }
        }

        if (isMute)
        {
            //静音转成非静音
            music.volume = musicVolume;
        }
        else
        {
            music.volume = 0f;
        }
        crossFading = false;
    }



    //用于读取游戏时, 设置音量配置
    public void updateData(float sourceVolume, float musicVolume, float musicNowVolume, AudioClip clip)
    {
        setSourceVolume(sourceVolume);
        setMusicVolume(musicVolume);
        music.volume = musicNowVolume;
        Debug.Log(clip);
        if (musicNowVolume != 0f && clip != null)
        {
            music.clip = clip;
            music.Play();
        }
    }


    public float getSourceVolume()
    {
        return sourceVolume;
    }

    public void setSourceVolume(float sourceVolume)
    {
        this.sourceVolume = sourceVolume;
        AudioListener.volume = sourceVolume;
    }

    public float getMusicVolume()
    {
        return musicVolume;
    }

    public void setMusicVolume(float musicVolume)
    {
        this.musicVolume = musicVolume;
        music.volume = musicVolume;
    }
}
