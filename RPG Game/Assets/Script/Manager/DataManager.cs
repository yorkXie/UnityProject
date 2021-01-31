using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private string filename;

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Data manager starting...");

        network = service;

        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        status = ManagerStatus.Started;
    }

    public void SaveGameState()
    {
        //要被序列化的map
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("inventory", Managers.Inventory.GetData());
        gamestate.Add("health", Managers.Player.health);
        gamestate.Add("maxHealth", Managers.Player.maxHealth);
        gamestate.Add("curLevel", Managers.Mission.curLevel);
        gamestate.Add("maxLevel", Managers.Mission.maxLevel);
        gamestate.Add("sourceVolume", Managers.music.getSourceVolume());
        gamestate.Add("musicVolume", Managers.music.getMusicVolume());
        gamestate.Add("musicNowVolume", Managers.music.musicNowVolume);
        gamestate.Add("musicName", Managers.music.clipName);

        FileStream fileStream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        //序列化字典作为所建文件的内容
        formatter.Serialize(fileStream, gamestate);
        fileStream.Close();
    }

    public void LoadGameState()
    {
        if (!File.Exists(filename))
        {
            Debug.Log("No saved game");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        Dictionary<string, object> gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        Managers.Inventory.UpdateData((Dictionary<string, int>)gamestate["inventory"]);
        Managers.Player.UpdateData((int)gamestate["health"], (int)gamestate["maxHealth"]);
        Managers.Mission.UpdateData((int)gamestate["curLevel"], (int)gamestate["maxLevel"]);
        Managers.Mission.RestartCurrent();
        Managers.music.updateData((float)gamestate["sourceVolume"], (float)gamestate["musicVolume"],
            (float)gamestate["musicNowVolume"], Resources.Load<AudioClip>("Music/" + gamestate["musicName"]));
    }

}
