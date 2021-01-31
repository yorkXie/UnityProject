using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//确保存在不同的管理器
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
[RequireComponent(typeof(MusicManager))]
public class Managers : MonoBehaviour
{
    //其他代码用来访问管理器的静态属性
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static MissionManager Mission { get; private set; }
    public static DataManager Data { get; private set; }
    public static MusicManager music { get; private set; }

    //启动时要遍历的管理器列表
    private List<IGameManager> startSequence;

    private void Awake()
    {
        //用于让对象在场景之间持久化
        DontDestroyOnLoad(gameObject);

        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Mission = GetComponent<MissionManager>();
        music = GetComponent<MusicManager>();
        Data = GetComponent<DataManager>();

        //因为DataManager使用其他管理器, 所以确保其他管理器启动序列在DataManager之前
        startSequence = new List<IGameManager>();
        startSequence.Add(Player);
        startSequence.Add(Inventory);
        startSequence.Add(Mission);
        startSequence.Add(music);
        startSequence.Add(Data);

        //异步启动序列
        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        NetworkService network = new NetworkService();
        ;
        foreach (IGameManager manager in startSequence)
        {
            manager.Startup(network);
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
                {
                    Debug.Log("Progress: " + numReady + "/" + numModules);
                    //Startup时间广播与时间相关的数据
                    Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
                }
                //再次检查之前停顿一帧
                yield return null;
            }

        }
        Debug.Log("All managers started up");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
    }

}
