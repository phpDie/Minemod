using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invData : MonoBehaviour
{

    public List<itemElement> items = new List<itemElement>();

    public inv myViewInvUi;

    public invItem blankItem;

    public void itemRemoveIndex(int J)
    { 
        Destroy(items[J].e.gameObject);

        items.Remove(items[J]);


    }

    public int getIdFromElement(itemElement e)
    {
        for (int j = 0; j < items.Count; j++)
        {
            
            itemElement it = items[j];
            if (it == e)
            {
                return j;
            }
        }

            return -1;
    }
    public void clickItemElemet(itemElement e)
    {
        //print("ClickInv");

        if (Global.Links.getSui().invOther.gameObject.active)
        {
            //print("Move mode");


            if (myViewInvUi.isPlayer)
            {
                Global.Links.getOtherInv().myData.items.Add(e);
                items.Remove(e);
                Global.Links.getOtherInv().myData.ReRender(true);
                ReRender(true);
            }
            else
            {
                Global.Links.getPlayerInv().myData.items.Add(e);
                items.Remove(e);

                Global.Links.getPlayerInv().myData.ReRender(true);
                ReRender(true);

            }


            return;
        }
        

        if (myViewInvUi.isPlayer)
        {            
            myViewInvUi.selectNewActiveInde(getIdFromElement(e));
        }
    }

    public bool giveFromInd(string ind, int count)
    {
        int j = searchIsset(ind, count);
        if (j < 0) return false;

        items[j].count -= count;
        if (items[j].count <= 0)
        {
            itemRemoveIndex(j);

        }
        ReRender();
        return true;
    }


    public int searchIsset(string ind, int count)
    {
        for (int j = 0; j < items.Count; j++)
        {
            itemElement it = items[j];
            if (it.ind == ind)
            {
                if (it.count >= count)
                {
                    return j;
                }
            }
        }

        return -1;

    }


    public void itemAdd(string ind)
    {


        itemSave itemData = Global.Links.getModLoader().itemBaseGetFromInd(ind);


        invItem nItem = Instantiate(blankItem, transform);

        nItem.myData = this;
        nItem.transform.GetChild(2).GetComponent<Text>().text = itemData.nameView.ToString();



        nItem.transform.GetChild(3).gameObject.SetActive(false);

        // nItem.transform.GetChild(1).GetComponent<Text>().text = itemData.nameView.ToString();
        Texture2D blurredTexture = itemData.icon;
        Sprite newSprite = Sprite.Create(blurredTexture, new Rect(0, 0, blurredTexture.width, blurredTexture.height), new Vector2(32f, 32f));
        nItem.transform.GetChild(0).GetComponent<Image>().sprite = newSprite;

        itemElement newElement = new itemElement();
        newElement.ind = ind;
        newElement.infoItemSave = itemData;
        newElement.e = nItem;
        newElement.count = itemData.stackSize;

        nItem.myElement= newElement;
        items.Add(newElement);

        ReRender();
    }

    public void ReRender(bool fullUpd =false)
    {
        if (myViewInvUi == null) {

            foreach (itemElement it in items)
            {
                it.e.transform.SetParent(transform);
            }

            return;
        }


        foreach (itemElement it in items)
        {
            it.e.transform.SetParent(myViewInvUi.transform);
        }

            myViewInvUi.ReRender(fullUpd);
        

    }

    // Start is called before the first frame update
    public void init()
    {
        blankItem = Resources.Load<invItem>("invItem");
    }
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool dataSet( string inputTextData)
    {
        
        if (myViewInvUi != null)
        {

            myViewInvUi.closeCargo();
            print("CLOSE CARGO");
        }


        for (int j = 0; j < items.Count; j++)
        {
            itemRemoveIndex(j);/*
            itemElement it = items[j];

            //it.e.transform.SetParent(GameObject)
            if (it.e != null)
            {
                it.e.transform.SetParent(transform);
                Destroy(it.e.gameObject);
                items.Remove(it);
            }
            */
            
            
        }
        items.Clear();


        string[] lines = inputTextData.Split('\n');

        if (lines.Length < 1)
        {
            print("no valid data inv loader");
            return false;
        }


        for (int i = 0; i < lines.Length; i++)
        {

            string[] lineOne = lines[i].Split(' ');
            itemAdd(lineOne[0]);
        }

        //print("INV LOADED");

        ReRender(true);
        return true;
    }

    public string dataGet()
    {
        string Out= "";


        foreach (itemElement it in items)
        {
            string line = "\n";
            line += it.ind + " " + it.count.ToString();
            Out += line;
        }

        Out = Out.Trim();

        return Out;

    }
}
