using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//确保存在不同的管理器
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
//[RequireComponent(typeof(IGameManager))]
public class Managers : MonoBehaviour
{
    //其他代码用来访问管理器的静态属性
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }

    //启动时要遍历的管理器列表
    private List<IGameManager> startSequence;

    private void Awake()
    {
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();

        startSequence = new List<IGameManager>();
        startSequence.Add(Player);
        startSequence.Add(Inventory);

        //异步启动序列
        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModules = startSequence.Count;
        int numReady = 0;
        //循环至所有管理器都启动为止
        while (numReady < numModules)
        {
            int lasReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }

                if (numReady > lasReady)
                    Debug.Log("Progress: " + numReady + "/" + numModules);
                //再次检查之前停顿一帧
                yield return null;
            }

        }
            Debug.Log("All managers started up");

    }

}
