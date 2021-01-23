using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    //打开门时要偏移到的位置(偏移量)
    [SerializeField] private Vector3 dPos;

    private Vector3 originalPos;

    private bool isOpen;

    private void Start()
    {
        originalPos = transform.position;
    }

    public void Operate()
    {
        //根据们的状态决定打开还是关闭门
        if (isOpen)
        {
            // Vector3 pos = transform.position - dPos;
            // transform.position = pos;

            // 自己改进一下， 关门时直接位移到原位置
            iTween.MoveTo(gameObject, originalPos, 1);
        }
        else
        {
            // Vector3 pos = transform.position + dPos;

            //自己改进的，开门时用原位置 加上偏移位置，得出需要移动到的位置
            Vector3 pos = originalPos + dPos;
            // transform.position = pos;
            iTween.MoveTo(gameObject, pos, 1);
        }
        isOpen = !isOpen;
    }

    public void Activate(){
        //当门没有打开时才打开
        if (!isOpen)
        {
            Vector3 pos = originalPos + dPos;
            // transform.position = pos;
            iTween.MoveTo(gameObject, pos, 1);
            isOpen = true;
        }
    }

    public void Deactivate(){
        //当门没有关闭时才关闭
        if (isOpen)
        {
            // 自己改进一下， 关门时直接位移到原位置
            iTween.MoveTo(gameObject, originalPos, 1);
            isOpen = false;
        }
    }

}
