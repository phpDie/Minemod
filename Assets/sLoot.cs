using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sLoot : MonoBehaviour
{
    public string ind;
    public int count = 1;
    itemSave it;
    Transform mySpr;


    public void init(string _ind = "Core:dirt", int _count = 1)
    {

        mySpr = transform.Find("spr").transform;
        ind = _ind;
        count = _count;


        if (!Global.Links.getModLoader().itemBase.ContainsKey(ind)) { 
 
            print("NULL ITEM SPAWN: "+ind);
            Destroy(gameObject);
            return;
        }

        it = Global.Links.getModLoader().itemBaseGetFromInd(ind);

        SpriteRenderer sr = mySpr.GetComponent<SpriteRenderer>();


        Texture2D blurredTexture = it.icon;
        float w = blurredTexture.width;
        float h = blurredTexture.height;


        sr.sprite = Sprite.Create(blurredTexture, new Rect(0, 0, w, h), new Vector2(0.5f, 0f));



    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (mySpr != null)
        {
            mySpr.transform.Rotate(Vector3.up, 0.2f);
        }
    }
}