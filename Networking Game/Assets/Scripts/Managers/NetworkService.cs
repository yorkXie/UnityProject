using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;


public class NetworkService
{
    //小米天气的API
    // private const string weatherApi =
    //     "https://weatherapi.market.xiaomi.com/wtr-v3/weather/all?latitude=0&longitude=0&" +
    //     "days=1&appKey=weather20151024&sign=zUFJoAR2ZVrDy1vF3D07&isGlobal=false&locale=zh_cn&locationKey=weathercn%3A101280101";

    //XML的URL
    // private const string weatherApi = 
    //     "http://api.openweathermap.org/data/2.5/weather?q=London,uk&APPID=609c32e91a4b138854046080925dbd42&mode=xml";

    //json的url
    private const string weatherApi =
        "http://api.openweathermap.org/data/2.5/weather?q=London,uk&APPID=609c32e91a4b138854046080925dbd42";

    private const string webImage = "http://n.sinaimg.cn/sinacn13/267/w640h427/20180512/74aa-hamfahw9624877.jpg";

    //服务器端脚本的地址
    private const string localApi = "http://localhost/ch9/api";

    private IEnumerator CallAPI(string url, WWWForm form, Action<string> callback)
    {
        //在get模式下创建UnityWebRequest对象
        using (UnityWebRequest requent = (form == null) ?
            //使用WWWForm执行Post, 或者直接执行get
            UnityWebRequest.Get(url) : UnityWebRequest.Post(url, form))
        {
            //下载时暂停
            yield return requent.SendWebRequest();

            //在响应中检查错误
            if (requent.error != null)
            {
                Debug.LogError("network problem: " + requent.error);
            }
            else if (requent.responseCode != (long)System.Net.HttpStatusCode.OK)
            {
                Debug.LogError("request error, response code: " + requent.responseCode);
            }
            else
            {
                //可以像原始函数一样调用委托
                callback(requent.downloadHandler.text);
            }
        }
    }

    public IEnumerator LogWeather(string name, float cloudValue, Action<string> callback)
    {
        //定义了一个带有发送值的表单
        WWWForm form = new WWWForm();
        form.AddField("message", name);
        form.AddField("cloud_value", cloudValue.ToString());
        //和多云值一起发送时间戳
        form.AddField("timestamp", DateTime.UtcNow.Ticks.ToString());

        return CallAPI(localApi, form, callback);
    }

    public IEnumerator GetWeatherJson(Action<string> callback)
    {
        //通过相互调用的协程方法调用产生级联
        return CallAPI(weatherApi, null, callback);
    }

    //这个回调使用Texture2D 而不是使用字符串
    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);

        yield return request.SendWebRequest();

        //使用DownloadHandler工具获得下载的图像
        callback(DownloadHandlerTexture.GetContent(request));
    }

}