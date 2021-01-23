using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //要移动到的位置
    public Vector3 finishPos = Vector3.zero;
    public float speed = 0.5f;

    private Vector3 startPos;
    //在start和finish之间"跟踪"有多远
    private float trackPercent = 0;    
    //当前移动的方向
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        trackPercent += direction * speed * Time.deltaTime;
        //Debug.Log(trackPercent);
        float x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
        float y = (finishPos.y - startPos.y) * trackPercent + startPos.y;
        transform.position = new Vector3(x, y, startPos.z);
        //Debug.Log(transform.position);
        if ((direction == 1 && trackPercent > .9f) || (direction == -1 && trackPercent < .1f))
        {
            direction *= -1;
        }

    }
}
