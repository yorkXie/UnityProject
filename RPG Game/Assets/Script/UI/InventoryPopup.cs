using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private Text[] ItemLabels;

    [SerializeField] private Text curItemLabel;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button useButton;
    //音效
    [SerializeField] private AudioClip source;
    //音乐名字
    [SerializeField] private string musicName;

    private string curItem;
    private void Awake()
    {
        Messenger.AddListener(GameEvent.REFRESH_REPOSITORY, Refresh);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.REFRESH_REPOSITORY, Refresh);

    }

    public void Refresh()
    {
        List<string> itemList = Managers.Inventory.GetItemList();

        int len = itemIcons.Length;
        for (int i = 0; i < len; i++)
        {
            //循环所有UI图像时检查仓库列表
            if (i < itemList.Count)
            {
                itemIcons[i].gameObject.SetActive(true);
                ItemLabels[i].gameObject.SetActive(true);

                string item = itemList[i];
                //从Resources中加载精灵
                Sprite sprite = Resources.Load<Sprite>("Icons/" + item);
                itemIcons[i].sprite = sprite;
                //将图像的大小重新设置为精灵的原始大小
                itemIcons[i].SetNativeSize();

                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;
                if (item == Managers.Inventory.equippedItem)
                {
                    //标签除了显示物品数量外, 还可能显示Equipped
                    message = "Equipped\n" + message;
                }

                ItemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                //允许单击图标
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) =>
                {
                    //为每个物品触发不同的lambda函数
                    OnItem(item);
                });

                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                //清空侦听器, 以便从空白的状态刷新
                trigger.triggers.Clear();
                //将侦听器添加到EventTrigger
                trigger.triggers.Add(entry);
            }
            else
            {
                //如果没有文本需要显示,则隐藏这个图像/文本
                itemIcons[i].gameObject.SetActive(false);
                ItemLabels[i].gameObject.SetActive(false);
            }
        }

        if (!itemList.Contains(curItem))
        {
            curItem = null;
        }
        //如果没有选择物品, 则隐藏按钮
        if (curItem == null)
        {
            curItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            curItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if (curItem == "health")
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }

            curItemLabel.text = curItem + ": ";
        }
    }

    public void changeSourceVolume(float volume){
        Managers.music.setSourceVolume(volume);
        Managers.music.playSource(source);
    }

    public void playeMusic(){
        Managers.music.playMusic(Resources.Load<AudioClip>("Music/" + musicName));
    }

    public void changeMusicVolume(float volume){
        Managers.music.setMusicVolume(volume);
    }

    ///由鼠标单击侦听器调用的函数
    public void OnItem(string item)
    {
        curItem = item;
        //改变物品后刷新仓库显示
        Refresh();
    }

    public void OnEuquip()
    {
        Managers.Inventory.EquipItem(curItem);
        Refresh();
    }

    public void OnUse()
    {
        Managers.Inventory.ConsumeItem(curItem);
        if (curItem == "health")
        {
            Managers.Player.ChangeHealth(25);
        }
        Refresh();
    }
}
