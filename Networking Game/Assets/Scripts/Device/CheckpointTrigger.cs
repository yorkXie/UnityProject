using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{

    public string identifier;
    //如果检查点已被触发,则跟踪它
    private bool triggered;
    
    
    private void OnTriggerEnter(Collider other) {
        if (triggered)
        {
            return;
        }

        Managers.Weather.LogWeather(identifier);
        triggered = true;
    }
}
