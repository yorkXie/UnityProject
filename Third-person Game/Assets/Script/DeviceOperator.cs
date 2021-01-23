using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour
{
    //玩家激活设施的距离
    public float radius = 1.5f;

    // Update is called once per frame
    void Update()
    {
        //响应unity的输入设置中定义的输入按钮
        if (Input.GetButtonDown("Fire3"))
        {
            //OverlapSphere() 返回附近对象数组
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hitCollider in hitColliders)
            {
                Vector3 direction = hitCollider.transform.position - transform.position;
                //当面向正确的方向时才发送消息
                if (Vector3.Dot(transform.forward, direction) > 0.5f)
                {
                    //SendMessage()尝试调用指定的函数,不管目标对象的类型
                    //public void SendMessage (string methodName, SendMessageOptions options);
                    // options	如果目标对象没有为消息实现该方法，是否应报错？
                    hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
