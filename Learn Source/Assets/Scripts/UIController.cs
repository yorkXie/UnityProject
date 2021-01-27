using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField] private SettingsPopup popup;

    private void Start()
    {
        //引用场景中的弹出窗口对象
        popup.gameObject.SetActive(false);
    }


    private void Update()
    {
        //使用M键切换弹出窗口
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isSowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isSowing);

            //随弹出窗口一起切换光标
            if (isSowing)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
