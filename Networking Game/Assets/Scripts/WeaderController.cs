using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaderController : MonoBehaviour
{
    //引用Project视图中的材质,而不仅仅是场景中的对象
    [SerializeField] private Material sky;
    [SerializeField] private Light sun;

    private float fullIntensity;

    private float cloudValue = 0f;

    public void OnWeatherUpdated(){
        SetOvercast(Managers.Weather.cloudValue);
    }

    private void Awake() {
        Messenger.AddListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始灯光最开始为满强度
        fullIntensity = sun.intensity;
    }


    //同时调整材质的Blend值和灯光强度
    private void SetOvercast(float value)
    {
        sky.SetFloat("_Blend", value);
        sun.intensity = fullIntensity - (fullIntensity * value);
    }
}
