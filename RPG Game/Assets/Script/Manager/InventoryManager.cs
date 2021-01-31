using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    //属性可以从任何地方获取,但是只能在这个脚本设置
    public ManagerStatus status { get; private set; }

    public string equippedItem { get; private set; }

    private Dictionary<string, int> items;

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        //任何长时间运行的任务都放在这里
        Debug.Log("Inventory manager starting...");
        network = service;
        //初始化一个空列表
        UpdateData(new Dictionary<string, int>());
        //如果是长时间运行的任务,状态变成Initialzing
        status = ManagerStatus.Started;
    }

    public void UpdateData(Dictionary<string, int> items)
    {
        this.items = items;
    }

    //为了保存游戏数据,需要getter方法
    public Dictionary<string, int> GetData(){
        return items;
    }


    //增加物品
    public void AddItem(string name)
    {
        if (items.ContainsKey(name))
        {
            items[name] += 1;
        }
        else
        {
            items[name] = 1;
        }

        DisplayItems();
    }

    //使用物品
    public bool ConsumeItem(string name)
    {
        //检查物品是否在仓库中
        if (items.ContainsKey(name))
        {
            items[name]--;
            if (items[name] == 0)
            {
                if (equippedItem == name)
                {
                    equippedItem = null;
                }
                items.Remove(name);
            }
        }
        else
        {
            Debug.Log("can't consume " + name);
            return false;
        }
        DisplayItems();
        return true;
    }

    public List<string> GetItemList()
    {
        List<string> list = new List<string>(items.Keys);
        return list;
    }

    public int GetItemCount(string name)
    {
        if (items.ContainsKey(name))
        {
            return items[name];
        }
        return 0;
    }
    //装备物品
    public bool EquipItem(string name)
    {
        //检查仓库中有该物品,但还没有被装备
        if (items.ContainsKey(name) && equippedItem != name)
        {
            equippedItem = name;
            Debug.Log("Equipped " + name);
            return true;
        }

        equippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }

    private void DisplayItems()
    {
        //打印当前仓库的控制台信息
        string itemDisplay = "Items: ";

        foreach (KeyValuePair<string, int> item in items)
        {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(itemDisplay);
    }
}
