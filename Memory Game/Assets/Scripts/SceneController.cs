using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //卡牌网格
    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2;
    public const float offsetY = 2.5f;

    //原始卡牌
    [SerializeField] private MemoryCard originalCard;
    //精灵数组
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;

    private MemoryCard m_firstRevealed;
    private MemoryCard m_secondRevealed;

    private int m_score = 0;

    void Start()
    {
        Vector3 startPos = originalCard.transform.position;
        //声明4种卡片数组
        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        //打乱数组
        numbers = ShuffleArray(numbers);

        //Debug.Log(startPos);
        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }
                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
                //Debug.Log(card.transform.position);
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CardRevealed(MemoryCard card)
    {
        if (m_firstRevealed == null)
        {
            m_firstRevealed = card;
        }
        else
        {
            m_secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (m_firstRevealed.id == m_secondRevealed.id)
        {
            m_score++;
            scoreLabel.text = "Score: " + m_score;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            m_firstRevealed.Unreveal();
            m_secondRevealed.Unreveal();
        }

        m_firstRevealed = null;
        m_secondRevealed = null;
    }

    // 实现Knuth重排算法
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }

    //当已经存在第二张翻开的卡片时,返回false
    public bool canReveal
    {
        get { return m_secondRevealed == null; }
    }



}
