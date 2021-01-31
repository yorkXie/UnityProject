using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    //火球预设
    [SerializeField] private GameObject fireBallPrefab;
    //火球实例
    private GameObject m_fireball;
    //移动速度
    public float speed = 3.0f;
    //转向距离
    public float obstacleRange = 5.0f;
    //死了没
    private bool isAlive;


    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetComponent<PlayerCharacter>())
            {
                if (m_fireball == null)
                {
                    m_fireball = Instantiate(fireBallPrefab) as GameObject;
                    m_fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                    m_fireball.transform.rotation = transform.rotation;
                }
            }
            else if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }

    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }
}
