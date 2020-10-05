using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.UI;


public class itemElement
{
    public int count = 1;
    public string ind;
    public itemSave infoItemSave;
    public invItem e;
    public bool active = false; 
}


public class inv : MonoBehaviour
{

    public invItem blankItem;
    public List<itemElement> items= new List<itemElement>();


    public invData myData;


    


    public bool giveActive(int countGive, bool giveOnOrCheck =true)
    {
        int i = 0;
        foreach (itemElement it in items)
        {
            if (it == activeElement)
            {
                if (it.count - countGive < 0)
                {
                 
                    return false;
                }

                if (!giveOnOrCheck)
                {
                    return true;
                }

                it.count -= countGive;
                if(!it.infoItemSave.emptyIsset)if (it.count <= 0)
                {

                    print("DEsty item");
                    Destroy(it.e.gameObject);
                    
                    items.Remove(it);
                    selectNewActiveInde(0);
                }
                ReRender();

                i++;
                return true;
            }
        }

        return false;
    }
     
    public int actveIndex = 2;
    public itemElement activeElement;
    public bool isPlayer;

    public void selectNewActiveInde(int i)
    {
        if (i < 0) i = items.Count - 1;
        if (i > items.Count-1) i = 0;
        actveIndex = i;
        /*
        for(int j = actveIndex;  j < items.Count; j++)
        {
            items[j]
            if (items[j] != null)
            {
                actveIndex = j;
            }
        }
        */
        ReRender(true);

    }

    public void ReRender(bool isHard = false)
    {
        int i = 0;
        foreach (itemElement it in items)
        {
        
            if (actveIndex == i)
            {
                activeElement = it;
                it.active = true;

                if (isHard)
                {
                    if (isPlayer)
                    {

                        Global.Links.getPlayerAction().setHandItem(it);
                        //Global.Links.getPlayerAction().setSpriteHand(it.infoItemSave.iconInHand, it.infoItemSave.type == itemType.block);
                    }
                }
            }
            else
            {
                it.active = false;
            }

            it.e.transform.GetChild(1).GetComponent<Text>().text = it.count.ToString() + " / " + it.infoItemSave.stackSize.ToString();
            it.e.transform.GetChild(3).gameObject.SetActive(it.active);

            i++;
        }
    }

    public void closeCargo()
    {
        if (myData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        myData.myViewInvUi = null;
        myData.ReRender();
        myData = null;
        items = null;
        gameObject.SetActive(false);

    }

    public void openCargo(invData newCargo)
    {
        myData = newCargo;
        items = newCargo.items;
        newCargo.myViewInvUi = this;
        newCargo.ReRender(true);

    }


    // Start is called before the first frame update
    void Start()
    {
        if (myData != null)
        {
          //  items = myData.items;
            openCargo(myData);
        }

        
        /*
        itemAdd();
        itemAdd();
        itemAdd();
        itemAdd();
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
