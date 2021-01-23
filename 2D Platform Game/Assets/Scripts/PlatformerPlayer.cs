using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    public float speed = 4.5f;
    //跳跃的力
    public float jumpForce = 12.0f;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, rigidbody.velocity.y);
        rigidbody.velocity = movement;
        //collider.bounds 碰撞体的世界空间包围体积
        Vector3 max = collider.bounds.max;
        Vector3 min = collider.bounds.min;
        //检查碰撞器的最小Y值
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(max.x, min.y - 0.2f);
        //Physics2D.OverlapArea 检测区域内是否有碰撞体
        Collider2D hitCollider = Physics2D.OverlapArea(corner1, corner2);

        //检查玩家站在地上且没有移动
        rigidbody.gravityScale = hitCollider != null && Mathf.Approximately(deltaX, 0) ? 0 : 1;

        if (hitCollider != null && Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        MovingPlatform platform = null;
        if (hitCollider != null)
        {
            platform = hitCollider.GetComponent<MovingPlatform>();
        }
        if (platform != null)
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }


        animator.SetFloat("speed", Mathf.Abs(deltaX));
        //获取平台缩放, 没有平台默认1
        Vector3 pScale = Vector3.one;
        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }

        if (!Mathf.Approximately(deltaX, 0))
        {
            //按照正负速度,给角色变换面向
            //transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
        }
    }
}
