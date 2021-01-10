using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject m_enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_enemy == null)
        {
            m_enemy = Instantiate(enemyPrefab);
            m_enemy.transform.position = new Vector3(0, 1, 0);
            float angle = Random.Range(0, 360);
            m_enemy.transform.Rotate(0, angle, 0);
        }
    }
}
