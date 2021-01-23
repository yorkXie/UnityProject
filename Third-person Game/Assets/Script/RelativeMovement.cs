using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10f;
    public float minFall = -1.5f;
    //要应用的力量值
    public float pushForce = 3.0f;

    //相对移动的对象(相机)
    [SerializeField] private Transform target;
    public float rotSpeed = 15.0f;

    private CharacterController charController;
    private float vertSpeed;
    //需要在函数之间存储碰撞数据
    private ControllerColliderHit contactHit;

    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        vertSpeed = minFall;
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //从向量(0,0,0)开始并逐步添加移动组件
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        //当按下方向键时只处理移动
        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            //限制对角线移动速度,使它和沿着轴移动速度一样
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            //保存初始旋转,一遍处理完目标对象后还原
            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            //把movement的方向从本地坐标变成世界坐标
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            //LookRotation()计算了movement面向该方向的四元素
            //transform.rotation = Quaternion.LookRotation(movement);

            //使用插值平滑旋转
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }
        m_animator.SetFloat("Speed", movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;
        if (vertSpeed < 0 && // 检查玩家是否在掉落
            Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            //检查碰撞的距离, 稍微超过胶囊体的底部
            // 因为光线从角色中心投射， 所以将角色控制器的高度加上球面端，把这个值除以2。
            // 但真正要检查的距离则要超出角色底部一点点，考虑到光线投射微小的不确定性，因此除以1.9而不是除以2
            float check = (charController.height + charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        //isGrounded判断是否在地面上
        // if (charController.isGrounded)
        if (hitGround)
        {
            //当在地面时响应jump按钮
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                //如果不在地面上,那么应用重力,直到垂直速度达到终止速度
                vertSpeed = minFall;
                m_animator.SetBool("Jumping", false);
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
            //不要在关卡的开始处触发这个值
            if (contactHit != null)
            {
                m_animator.SetBool("Jumping", true);
            }

            //光线没有检测到地面,但是胶囊体接触到了地面
            if (charController.isGrounded)
            {
                //根据角色是否面向接触点,响应稍微不同
                if (Vector3.Dot(movement, contactHit.normal) < 0)
                {
                    movement = contactHit.normal * moveSpeed;
                }
                else
                {
                    movement += contactHit.normal * moveSpeed;
                }
            }
        }
        movement.y = vertSpeed;

        movement *= Time.deltaTime;
        charController.Move(movement);
    }
    // 当检测碰撞时将碰撞数据保存在回调中
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        contactHit = hit;

        //检查碰撞对象是否有rigidbody, 以便接受物理上的外力
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            //将速度应用到物理对象上
            body.velocity = hit.moveDirection * pushForce;
        }
    }

}
 