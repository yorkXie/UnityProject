using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    //用于引用新的音源
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;

    //在这些字符串中填写音乐名
    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;

    public float crossFadeRate = 1.5f;
    //正在淡入淡出时用于避免bug的开关
    private bool m_crossFading;

    //记录哪个音源是激活的, 哪个是非激活的
    private AudioSource m_activeMusic;
    private AudioSource m_inactiveMusic;

    private NetworkService network;
    // ------ 带get set的属性

    //带有getter和setter的音量属性
    public float soundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    //为静音添加一个类似的属性
    public bool soundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    //私有变量,不能直接访问,只能通过书信的getter访问
    private float m_musicVolume;
    public float musicVolume
    {
        get { return m_musicVolume; }
        set
        {
            m_musicVolume = value;
            if (music1Source != null && !m_crossFading)
            {
                //直接调整AudioSource的音量
                music1Source.volume = m_musicVolume;
                //调整两个音源的音效
                music2Source.volume = m_musicVolume;
            }
        }
    }

    public bool musicMute
    {
        get
        {
            if (music1Source != null)
            {
                return music1Source.mute;
            }
            //当audioSource 不存在时返回默认值
            return false;
        }
        set
        {
            if (music1Source != null)
            {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }
    //带get set的属性 ------ 



    public void PLayIntroMusic()
    {
        PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
    }

    public void PlayLevelMusic()
    {
        PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
    }



    //播放没有其他音源的声音
    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void Startup(NetworkService service)
    {
        Debug.Log("Audio manager starting.....");
        status = ManagerStatus.Initializing;
        network = service;
        //初始化值0-1, 1是满音量
        soundVolume = 1f;

        //ignoreListenerVolume/ignoreListenerPause 通知AudioSource忽略AudioListener的音量/暂停
        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        musicVolume = 1f;

        //初始化1为激活的AudioSource
        m_activeMusic = music1Source;
        m_inactiveMusic = music2Source;

        status = ManagerStatus.Started;
    }

    private void PlayMusic(AudioClip clip)
    {
        // music1Source.clip = clip;
        // music1Source.Play();
        if (m_crossFading)
            return;
        //当切换音乐时调用协程
        StartCoroutine(CrossFadeMusic(clip));
    }

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        m_crossFading = true;

        m_inactiveMusic.clip = clip;
        m_inactiveMusic.volume = 0;
        m_inactiveMusic.Play();

        float scaledRate = crossFadeRate * m_musicVolume;
        while (m_activeMusic.volume > 0)
        {
            m_activeMusic.volume -= scaledRate * Time.deltaTime;
            m_inactiveMusic.volume += scaledRate * Time.deltaTime;

            //暂停一帧
            yield return null;
        }

        //交换激活声源
        AudioSource temp = m_activeMusic;
        m_activeMusic = m_inactiveMusic;
        m_activeMusic.volume = m_musicVolume;

        m_inactiveMusic = temp;
        m_inactiveMusic.Stop();

        m_crossFading = false;


    }

    public void StopMusic()
    {
        music1Source.Stop();
        music2Source.Stop();
    }

}
