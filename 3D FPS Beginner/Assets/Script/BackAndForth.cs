using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public float speed = 3.0f;
    public float maxZ = 16.0f;
    public float minZ = -16.0f;

    private int m_direction = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, m_direction * speed * Time.deltaTime);

        bool bounced = false;
        if (transform.position.z > maxZ || transform.position.z < minZ)
        {
            m_direction = -m_direction;
            bounced = true;
        }
        if (bounced)
        {
            transform.Translate(0, 0, m_direction * speed * Time.deltaTime);
        }
    }
}
