using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDevice : MonoBehaviour
{
    public float radius = 3.5f;

    //单击时运行的函数
    private void OnMouseDown()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        if (Vector3.Distance(player.position, transform.position) < radius)
        {
            Vector3 direction = transform.position - player.position;
            if (Vector3.Dot(player.forward, direction) > 0.5f)
            {
                Operate();
            }
        }
    }

    //virtual标记可以在继承时重写的方法
    public virtual void Operate()
    {
        // behavior of the specific device
        //特定设备的行为
    }

}
