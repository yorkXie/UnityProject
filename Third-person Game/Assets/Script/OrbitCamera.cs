using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float rotSpeed = 1.5f;

    private float rotY;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        rotY = transform.eulerAngles.y;
        //存储摄像机和目标紫健的起始位置偏移
        offset = target.position - transform.position;
    }

    private void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal");
        // if (horInput != 0)
        // {
        //     //Debug.Log("use keboard");
        //     //使用方向键缓慢旋转摄像机
        //     rotY += horInput * rotSpeed * 0.1f;
        // }
        // else
        // {
            // Debug.Log("use mouse");
            //或者使用鼠标快速旋转摄像机
            rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
            //限制旋转角度，自己加的
            // rotY = Mathf.Clamp(rotY, -60, 60);
        // }

        Quaternion rotation = Quaternion.Euler(0, rotY, 0);
        //维持起始偏移,根据摄像机旋转进行位置偏移
        transform.position = target.position - (rotation * offset);
        //不管摄像机在目标的什么地方,摄像机总是面向目标
        transform.LookAt(target);

    }
}
