using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using MiniJSON;

public class WeadtherManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    //多云值对外只读, 内部可以修改
    public float cloudValue { get; private set; }

    //add cloud value here 
    private NetworkService service;

    public void Startup(NetworkService service)
    {
        Debug.Log("Weather manager starting...");

        this.service = service;
        //开始从互联网加载数据
        StartCoroutine(this.service.GetWeatherJson(OnJsonDataLoadded));

        //将状态改为Initializing 而不是started
        status = ManagerStatus.Initializing;
    }

    public void OnJsonDataLoadded(string data)
    {
        Dictionary<string, object> dict = Json.Deserialize(data) as Dictionary<string, object>;
        Dictionary<string, object> clouds = dict["clouds"] as Dictionary<string, object>;

        cloudValue = (long)clouds["all"] / 100f;
        Debug.Log("Value: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);


        status = ManagerStatus.Started;
    }

    public void LogWeather(string name)
    {
        StartCoroutine(service.LogWeather(name, cloudValue, OnLogged));
    }

    private void OnLogged(string response)
    {
        Debug.Log(response);
    }

    public void OnXMLDataLoadded(string data)
    {
        // Debug.Log(data);
        XmlDocument doc = new XmlDocument();
        doc.Load(data);
        XmlElement root = doc.DocumentElement;

        //从数据中拉取一个节点
        XmlNode clouds = root.SelectSingleNode("clouds");
        string value = clouds.Attributes["value"].Value;
        //将数值转换为0-1的浮点数
        cloudValue = Convert.ToInt32(value) / 100f;

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }

}
