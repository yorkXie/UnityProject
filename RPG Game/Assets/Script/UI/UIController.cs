using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthLable;
    [SerializeField] private InventoryPopup popup;
    [SerializeField] private Text levelEnding;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevenFailed);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevenFailed);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    void Start()
    {
        //启动时手动调用函数
        OnHealthUpdated();

        levelEnding.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }
    }

    public void SaveGame()
    {
        Managers.Data.SaveGameState();
    }

    public void LoadGame()
    {
        Managers.Data.LoadGameState();
    }

    public void OnLevelComplete()
    {
        StartCoroutine(CompleteLevel());
    }

    public void OnLevenFailed()
    {
        StartCoroutine(FailLevel());
    }

    private void OnGameComplete()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "You Finished the Game!";
    }

    private IEnumerator CompleteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";

        yield return new WaitForSeconds(2);

        Managers.Mission.GoToNext();
    }

    private IEnumerator FailLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed";

        yield return new WaitForSeconds(2);

        //暂停两秒后重庆开始当前关卡
        Managers.Player.Respawn();
        Managers.Mission.RestartCurrent();
    }

    private void OnHealthUpdated()
    {
        string message = "Health: " + Managers.Player.health + "/" + Managers.Player.maxHealth;
        healthLable.text = message;

    }

}
