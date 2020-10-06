using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invData : MonoBehaviour
{
    [Header("Параметры")]
    public bool isActionBar = false;
    public bool isPlayerMainInv= false;


    [HideInInspector]
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
 
    public bool moveInOtherInv(inv otherWinInv, itemElement myItemE)
    {
        otherWinInv.myData.items.Add(myItemE);
        items.Remove(myItemE);
        otherWinInv.myData.ReRender(true);
        ReRender(true);

        return true;
    }


    public void clickItemElemet(itemElement e)
    {

  

        //Это главный инв
        if (isPlayerMainInv)
        {

            print("isPlayerMainInv");
            //Переброс в сундук
            if (Global.Links.getSui().winInvOther.gameObject.active)
            {
                Global.Links.getOtherInvUi().myData.items.Add(e);
                items.Remove(e);
                Global.Links.getOtherInvUi().myData.ReRender(true);
                ReRender(true);
                return;
            }
            else
            {

                //Переброс с глав инв в экшен бар

                moveInOtherInv(Global.Links.getSui().winInvAction, e);
                /*
                Global.Links.getPlayerInvUi().myData.items.Add(e);
                items.Remove(e);
                Global.Links.getPlayerInvUi().myData.ReRender(true);
                ReRender(true);
                */
                return;
            }
        }



        //Я сундук, значит из меня копируют в другое место
        if (!isPlayerMainInv && !isActionBar)
        {
            Global.Links.getPlayerMainInvUi().myData.items.Add(e);
            items.Remove(e);

            Global.Links.getPlayerMainInvUi().myData.ReRender(true);
            ReRender(true);
            return;
        }

        /*
        if (Global.Links.getSui().winInvOther.gameObject.active)
        {
            //print("Move mode");


            if (myViewInvUi.isPlayer)
            {
                Global.Links.getOtherInvUi().myData.items.Add(e);
                items.Remove(e);
                Global.Links.getOtherInvUi().myData.ReRender(true);
                ReRender(true);
            }
            else
            {
                Global.Links.getPlayerInvUi().myData.items.Add(e);
                items.Remove(e);

                Global.Links.getPlayerInvUi().myData.ReRender(true);
                ReRender(true);

            }


            return;
        }

        */
        if (isActionBar)
        {

            //Переброс с экшен бара в главный инв
            if (Global.Links.getSui().winInvMain.gameObject.active)
            {
                moveInOtherInv(Global.Links.getSui().winInvMain, e);
            }
            else
            {
                //Всё закрыто, знчит мы активируем предмет
                // if (myViewInvUi.isPlayer) myViewInvUi.selectNewActiveInde(getIdFromElement(e));           
                myViewInvUi.selectNewActiveInde(getIdFromElement(e));
            }
            return;
        }

        print("НЕ ПОНЯТНАЯ СИТУАЦИЯ И ИНВ");
    }

    public bool giveFromInd(string ind, int count)
    {
        int j = searchIsset(ind, count);
        if (j < 0) return false;

        items[j].count -= count;
        if (items[j].count <= 0)
        {

            if (!items[j].infoItemSave.emptyIsset)
            {
                itemRemoveIndex(j);
            }

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


    public void itemAdd(string ind,int count = 0)
    {
        int issetIndex = searchIsset(ind, 1);

        if (issetIndex > -1)
        {
            //print("STACKING");
            items[issetIndex].count += count;
            ReRender();
            return;
        }



        itemSave itemData = Global.Links.getModLoader().itemBaseGetFromInd(ind);
        
        if (itemData == null)
        {
            print("itemAdd not exist element: " + ind);
            return;
        }

        if (blankItem == null)
        {
            print("Принудительная инициализация инв");
            init();
        }

        Transform _parentNew = transform;
        if (myViewInvUi != null)
        {
            _parentNew = myViewInvUi.contentBox;
        }

        invItem nItem = Instantiate(blankItem, _parentNew);

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

        if (count == 0)
        {
            newElement.count = itemData.stackSize;
        }
        else
        {
            newElement.count = count;
        }

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
                it.e.myData = this;
            }

            return;
        }


        foreach (itemElement it in items)
        {
            it.e.transform.SetParent(myViewInvUi.contentBox);
            it.e.myData = this;
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
