using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class itemElement
{
    public int i = -1; // индекс
    public int count = 1;
    public string ind;
    public itemSave infoItemSave;
    public invItem e;
    public bool active = false;
    public bool isset = false;

   
}


public class invData : MonoBehaviour
{
    [Header("Параметры")]
    public bool isActionBar = false;
    public bool isPlayerMainInv= false;
    public int size = 16;


    public ghostItem ghost;


    [HideInInspector]
    public List<itemElement> items = new List<itemElement>();
    public inv myViewInvUi;
    public invItem blankItem;



    bool isInit =false;
    public void init(int mySize =-1)
    {

        if (isInit) return;
        isInit = true;

        if (mySize > -1)
        {
            size = mySize;
        }

        blankItem = Resources.Load<invItem>("invItem");


        ghost = Instantiate(Resources.Load<ghostItem>("ghostItem"), Global.Links.getSui().transform);
        ghost.gameObject.SetActive(false);

        for (int i = 0; i < size; i++)
        {

            invItem nItem = Instantiate(blankItem, transform);
            nItem.myIndex = i;

            nItem.myData = this;

            //nItem.transform.GetChild(2).GetComponent<Text>().text = "empty";
            nItem.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);
          //  nItem.transform.GetChild(3).gameObject.SetActive(false);

            nItem.setNull();


            itemElement newElement = new itemElement();
            newElement.isset = false;
            newElement.e = nItem;
            newElement.i = i;
            items.Add(newElement);

        }
    }



    public bool itemAdd(string ind, int count = 0, bool isAutoRender =false, int inSelectIndex =-1)
    {
        

        if(ind == "not" || ind == "")
        {
            return false;
        }

        itemSave itemData = Global.Links.getModLoader().itemBaseGetFromInd(ind);


        if (itemData == null)
        {
            print("itemAdd not exist element: " + ind);
            return false;
        }

        if (blankItem == null)
        {
            print("Принудительная инициализация инв");
            init();
        }

        

        if (!itemData.stackHpMode)
        {

            //стак в место
            if (inSelectIndex > -1)
            {
                
                if (items[inSelectIndex].isset)
                {
                    
                    if (items[inSelectIndex].ind != itemData.ind) return false;
                    if (items[inSelectIndex].count + count > itemData.stackSize) return false;

                    items[inSelectIndex].count += count;
                    if (isAutoRender) ReRender();

                    return true;
                }
            }

            //Автостак
            if (inSelectIndex <= -1)
            { 
                int issetIndex = searchIsset(ind, 1);
                
                if (issetIndex > -1)
                {


                    if (items[issetIndex].count - count <= itemData.stackSize)
                    {
                        items[issetIndex].count += count;
                        if (isAutoRender) ReRender();
                        return true;
                    }
                }
            }

            
        }

        


        
        /*

        Transform _parentNew = transform;
        if (myViewInvUi != null)
        {
            _parentNew = myViewInvUi.contentBox;
        }
        */


        int J = searchEmpty();
        if (J <= -1) return false;


        if (inSelectIndex > -1) {
            if (!items[inSelectIndex].isset)
            {
                J = inSelectIndex;
            }
        }


        invItem nItem = items[J].e;
         

        nItem.myData = this;
        nItem.transform.GetChild(2).GetComponent<Text>().text = itemData.nameView.ToString();
        nItem.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);
        nItem.transform.GetChild(3).gameObject.SetActive(false);

        nItem.transform.GetChild(0).gameObject.SetActive(true);
        Texture2D blurredTexture = itemData.icon;
        Sprite newSprite = Sprite.Create(blurredTexture, new Rect(0, 0, blurredTexture.width, blurredTexture.height), new Vector2(32f, 32f));
        nItem.transform.GetChild(0).GetComponent<Image>().sprite = newSprite;


        itemElement newElement = items[J];
        
        newElement.isset = true;
        newElement.ind = ind;
        newElement.infoItemSave = itemData;

        if (itemData == null)
        {
            print("GAVNo");
        }


        newElement.e = nItem;

        if (count == 0)
        {
            newElement.count = itemData.stackSize;
        }
        else
        {
            newElement.count = count;
        }

        nItem.myElement = newElement;

        //   items.Add(newElement);

        if (isAutoRender)ReRender();
        return true;
    }


    public void itemRemoveIndex(int J)
    {  

        items[J].e.setNull();
         
        items[J].isset = false;


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
 
    
    public bool moveLocal(int I, int TO)
    {
        if (!items[I].isset) return false;
        
        if (items[TO].isset)
        {
            if (items[I].ind != items[TO].ind) return false;
        }
        

        if (itemAdd(items[I].ind, items[I].count, true, TO))
        {
            //items[I].isset = false;
            itemRemoveIndex(I);
            ReRender(true);
            return true;
        }

        return false;
    }


    public bool moveInOtherInv(inv otherWinInv, itemElement myItemE, int J=-1)
    {

        if (myItemE == null) return false;
        if (!myItemE.isset) return false;


        if (otherWinInv.myData == this)
        {
            print("юзай moveLocal ");
            return false;
        }


        if (myItemE == null)
        {
           // print("EMPTY ITEM");
            return false;
        }


        int oldIndex = myItemE.i;

       // otherWinInv.myData.items.Add(myItemE);
        bool isMoved =  otherWinInv.myData.itemAdd(myItemE.ind, myItemE.count,false, J);

        if (!isMoved) return false;

        /*
        if (otherWinInv.myData == this)
        {

            ReRender(true);
            return true;
        }
        */

        itemRemoveIndex(oldIndex);


        otherWinInv.myData.ReRender(true);
        ReRender(true);

        return true;
    }


    public void clickItemElemet(itemElement e)
    {

  

        //Это главный инв
        if (isPlayerMainInv)
        {
             
            //Переброс в сундук
            if (Global.Links.getSui().winInvOther.gameObject.active)
            {
                moveInOtherInv(Global.Links.getSui().winInvOther, e);

                /*
                Global.Links.getOtherInvUi().myData.items.Add(e);
                items.Remove(e);
                Global.Links.getOtherInvUi().myData.ReRender(true);
                ReRender(true);
                */
                return;
            }
            else
            {

                //Переброс с глав инв в экшен бар

                moveInOtherInv(Global.Links.getSui().winInvAction, e);
          
                return;
            }
        }



        //Я сундук, значит из меня копируют в другое место
        if (!isPlayerMainInv && !isActionBar)
        {

            moveInOtherInv(Global.Links.getPlayerMainInvUi(), e);

          
            return;
        }

      

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

    public int searchEmpty()
    {
        for (int j = 0; j < size; j++)
        {
            itemElement it = items[j];
            if (!it.isset)
            {
                return j;
            }
        }

        return -1;
    }


    public int searchIsset(string ind, int count)
    {
        for (int j = 0; j < items.Count; j++)
        {
            itemElement it = items[j];
            if (it.isset)
            {
                if (it.ind == ind)
                {
                    if (it.count >= count)
                    {
                        return j;
                    }
                }
            }
        }

        return -1;

    }


    public void ReParentes()
    {
        if (myViewInvUi == null)
        {

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

    }
    public void ReRender(bool fullUpd =false)
    {

        ReParentes();

        if (myViewInvUi != null)
        {

            myViewInvUi.ReRender(fullUpd);
        }

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

        if (!isActionBar)
        {
            if (myViewInvUi != null)
            {

                myViewInvUi.closeCargo();
                print("CLOSE CARGO");
            }
        }


        for (int j = 0; j < items.Count; j++)
        {
            itemRemoveIndex(j);    
            
        }
        //items.Clear();


        string[] lines = inputTextData.Split('\n');

        if (lines.Length < 1)
        {
            print("no valid data inv loader");
            return false;
        }


        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != String.Empty)
            {
                string[] lineOne = lines[i].Split(' ');
               // print(lines[i]);
                 itemAdd(lineOne[0], System.Convert.ToInt32(lineOne[1]) );
                //itemAdd(lineOne[0]);
            }
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

    bool isDrag = false;
    public void dragElement(itemElement E, Vector2 pos)
    {
        if (!isDrag)
        {
            if (!E.isset) return;

            ghost.gameObject.SetActive(true);
            ghost.itemElement = E;
            isDrag = true;
            ghost.transform.GetChild(0).GetComponent<Text>().text = E.count.ToString();
        }

        ghost.isDrag = isDrag;
        ghost.transform.position = pos;

    }


    public void dragEnd(itemElement E, GameObject go)
    {
        ghost.gameObject.SetActive(false);
        isDrag = false;

        if (go == null) return;


        if (go.transform.tag == "invItem")
        {
            invItem it = go.GetComponent<invItem>();
            

            if (it.myData == this)
            {
                if (it.myIndex == E.i) return;
                moveLocal(E.i, it.myIndex);
            }
            else
            {
                moveInOtherInv(it.myData.myViewInvUi, E, it.myIndex);
            }
        
            /*
            if (go.GetComponent<invItem>().myData == go.GetComponent<invItem>().myData)
            {

                moveInOtherInv(go.GetComponent<invItem>().myData.myViewInvUi, E);
                myIndex
                print("Move to me");
            }
            else
            {
                print("Move to other item");
                moveInOtherInv(go.GetComponent<invItem>().myData.myViewInvUi, E);
            }
            */
            return;
        }

        if (go.transform.tag == "invUi")
        {
            print("Move to other");
           // moveInOtherInv(go.GetComponent<inv>(), E);
        }
    }
}
