using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    //淡入时间
    public float fadeDuration = 1f;
    //淡入后显示时间
    public float displayImageDuration = 1f;
    //玩家
    public GameObject player;
    //淡入画布的CanvasGroup
    public CanvasGroup exitBackgroundImageCanvasGroup;
    //失败的画布
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    //结束音乐
    public AudioSource exitAudio;
    //被发现音乐
    public AudioSource caughtAudio;

    //玩家是否已经进入检测体
    bool m_IsPlayerAtExit;
    //开始淡入经过的时间
    float m_Timer;
    //玩家是否被发现
    bool m_IsPlayerCaught;
    //确保音频仅播放一遍
    bool m_HasAudioPlayed;





    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
            //Debug.Log("restart");
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;
        //淡入淡出完毕,游戏退出
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                //游戏退出,只能在完成构建的项目中生效
                Application.Quit();
            }
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

}
