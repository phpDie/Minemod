using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class receptCraftElement
{
    //public string name;
    public string ind;
    public int cout = 1;
}


public class receptCraft
{
    public string name = "Рецепт";
    public string ind = "not";
    public int cout = 1;
    public Texture2D icon;
    public List<receptCraftElement> ingredients = new List<receptCraftElement>();
}


public class craftUi : MonoBehaviour
{

    public Transform receptBox;
    public Transform contentBox;
    public craftItem blankElemnt;

    public List<receptCraft> items = new List<receptCraft>();

    public receptCraft curentRecept;

    public GameObject sidebar;

    public GameObject btnBlocker;

     

    public void btnShowIssetCraft()
    {
        RenderList(true);
    }

    public void btnShowAllCraft()
    {
        RenderList();
    }


    public void btnCraft()
    {

        if (curentRecept == null) return;

        bool issetAll = checkReceptIssetIngridients(curentRecept);
        btnBlocker.SetActive(!issetAll);

        if (issetAll)
        {
            checkReceptIssetIngridients(curentRecept,true);


            Global.Links.getIndDataPlayerCargo().itemAdd(curentRecept.ind, curentRecept.cout);

            RenderList(true);
            //print("CRAFT");
        }


        selectRecept(curentRecept);
    }
     

    public void selectRecept(receptCraft re)
    {
        if (re == null) return;
        curentRecept = re;

        sidebar.SetActive(true);
        transform.Find("sidebar/h1").GetComponent<Text>().text = re.name;


   
        for (int i = 0; i < receptBox.childCount; i++)
        {
            Destroy(receptBox.GetChild(i).gameObject);
        }

        foreach (receptCraftElement it in re.ingredients)
        {
           
            itemSave _loadItemData = Global.Links.getModLoader().itemBaseGetFromInd(it.ind);
            if (_loadItemData == null)
            {
                print("Error load recept for : " + it.ind);
            }
            else
            {
                craftItem e = Instantiate(blankElemnt, receptBox);
                e.myRecept = new receptCraft();
                e.myRecept.name = _loadItemData.nameView;
                e.myRecept.icon = _loadItemData.icon;
                e.myRecept.cout = it.cout;

                e.myCallBack = null;
                e.RenderThis();
            }
        }



        bool issetAll = checkReceptIssetIngridients(re);
        btnBlocker.SetActive(!issetAll);

    }

    public bool checkReceptIssetIngridients(receptCraft re, bool isGive =false)
    {

        inv myInv =  Global.Links.getPlayerMainInvUi();
        
        foreach (receptCraftElement it in re.ingredients)
        {

            int j = myInv.myData.searchIsset(it.ind, it.cout);
            if (j == -1) return false;

            if (isGive)
            {
                myInv.myData.giveFromInd(it.ind, it.cout);
            }
            
        }
        return true;
    }


    public void firstRender()
    {
        for (int i = 0; i < contentBox.childCount; i++)
        {
            Destroy(contentBox.GetChild(i));
        }

        foreach (receptCraft it in items)
        {
            craftItem e = Instantiate(blankElemnt, contentBox);
            e.myRecept = it;

            e.myCallBack = this;
            e.RenderThis();
        }

    }



    public void RenderList( bool showIsset = false)
    {
        sidebar.SetActive(false);
        curentRecept = null;

        for (int i=0; i < contentBox.childCount; i++)
        {

            craftItem e = contentBox.GetChild(i).GetComponent<craftItem>();


            bool _thisShow = true;

            if (showIsset)
            {
                _thisShow = checkReceptIssetIngridients(e.myRecept);
            }


            e.gameObject.SetActive(_thisShow);
            
        }

  



    }

    private void OnEnable()
    { 
        RenderList(true);
    }

    
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
