using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagesManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private NetworkService network;

    private Texture2D webImage;

    public void Startup(NetworkService service)
    {
        Debug.Log("Image manager starting...");

        this.network = service;

        status = ManagerStatus.Started;
    }

    public void GetWebImage(Action<Texture2D> callback)
    {
        if (webImage == null)
        {
            StartCoroutine(network.DownloadImage((Texture2D image) => {
                //储存已经下载的图像
                webImage = image;
                //回调在lambda函数中使用, 而不是直接发送到NetworkService
                callback(webImage);
            }));
        }
        else
        {
            //如果图像已经存储,立即调用
            callback(webImage);
        }
    }


}
