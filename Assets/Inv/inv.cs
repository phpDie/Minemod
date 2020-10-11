using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.UI;

 

public class inv : MonoBehaviour
{

    public GameObject hoverOb;
    public Text hoverText;

    public Transform contentBox;

    public invItem blankItem;
    public List<itemElement> items= new List<itemElement>();



    public invData myData;

    public void showHover(string t)
    {
        hoverOb.SetActive(true);
        hoverText.text = t.ToString();
        hoverTime = 5.6f;
    }




    public bool giveActive(int countGive, bool giveOnOrCheck = true)
    {
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

                if (!it.infoItemSave.emptyIsset) if (it.count <= 0)
                    {

                        myData.itemRemoveIndex(it.i);
                        ReRender(true);

                    }


                ReRender();


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

      //  myData.ReParentes();

        int i = 0;
        foreach (itemElement it in items)
        {

       

            if (actveIndex == i)
            {
                activeElement = it;
                it.active = true;

                it.e.transform.GetChild(3).gameObject.SetActive(true);

                if (isHard)
                {
                    if (isPlayer)
                    {

                        if (it.isset)
                        {
                            Global.Links.getPlayerAction().setHandItem(it);
                        }
                        else
                        {
                            Global.Links.getPlayerAction().setHandItem(null);

                        }
                       
                    }
                }
            }
            else
            {
                it.e.transform.GetChild(3).gameObject.SetActive(false);
                it.active = false;
            }


            if (it.e == null)
            {
                print("ERROR");
                return;

            }



            it.e.GetComponent<RectTransform>().rotation = new Quaternion(0,0,0,0);

            if (it.isset)
            {

                if (it.infoItemSave != null)
                {

                    it.e.transform.GetChild(1).GetComponent<Text>().text = it.count.ToString() + " / " + it.infoItemSave.stackSize.ToString();
                    it.e.transform.GetChild(3).gameObject.SetActive(it.active);

                }
                else
                {
                    print("ERROR SAVE DATA");
                }

            }


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
        if (myData != null)
        {
            myData.myViewInvUi = null;
            myData.ReRender();
        }

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
        hoverOb.SetActive(false);


        /*
        itemAdd();
        itemAdd();
        itemAdd();
        itemAdd();
        */

    }

    float hoverTime=0f;
    // Update is called once per frame
    void Update()
    {
        if (hoverTime > 0f)
        {
            hoverOb.transform.position = Input.mousePosition; 
            hoverTime -= Time.deltaTime;
            if (hoverTime <= 0f)
            {
                hoverOb.SetActive(false);
            }
        }
        
    }
}
