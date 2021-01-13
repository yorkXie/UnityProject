using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private SceneController controller;

    private int m_id;

    public int id
    {
        get { return m_id; }
    }

    public void SetCard(int id, Sprite image)
    {
        this.m_id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()
    {
        if (cardBack.activeSelf)
        {
            Debug.Log("id : " + this.m_id);
            cardBack.SetActive(false);
            //当卡牌翻开时通知控制器
            controller.CardRevealed(this);
        }
    }

    public void Unreveal()
    {
        this.cardBack.SetActive(true);
    }
}
