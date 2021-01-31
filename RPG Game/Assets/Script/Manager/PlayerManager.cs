using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public int health { get; private set; }
    public int maxHealth { get; private set; }

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager starting...");
        network = service;
        //可以使用保存的数据初始化这些值
        // health = 50;
        // maxHealth = 100;

        //调用更新方法而不是直接设置变量
        UpdateData(50, 100);

        status = ManagerStatus.Started;
    }

    public void UpdateData(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public void ChangeHealth(int value)
    {
        health += value;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0)
        {
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }
        Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
        // Debug.Log("Health: " + health + "/" + maxHealth);
    }

    public void Respawn()
    {
        //将玩家重置为初始状态
        UpdateData(50, 100);
    }
}
