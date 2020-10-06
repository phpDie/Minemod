using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class craftItem : MonoBehaviour
{

    public craftUi myCallBack;
    public receptCraft myRecept;


    public void RenderThis()
    {
        Texture2D blurredTexture = myRecept.icon;
        Sprite newSprite = Sprite.Create(blurredTexture, new Rect(0, 0, blurredTexture.width, blurredTexture.height), new Vector2(32f, 32f));
        transform.GetChild(0).GetComponent<Image>().sprite = newSprite;

        transform.GetChild(1).GetComponent<Text>().text = myRecept.cout.ToString();
        transform.GetChild(2).GetComponent<Text>().text = myRecept.name;
    }


    public void btnSelectMe()
    {
        Global.Links.getCraftUi().selectRecept(myRecept);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
