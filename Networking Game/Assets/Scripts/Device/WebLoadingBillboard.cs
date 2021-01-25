using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLoadingBillboard : MonoBehaviour
{
    public void Operate(){
        Managers.Images.GetWebImage(OnWebImage);
    }


    private void OnWebImage(Texture2D image){
        //在回调中将已经下载的图像应用的材质
        GetComponent<Renderer>().material.mainTexture = image;
    }
}
