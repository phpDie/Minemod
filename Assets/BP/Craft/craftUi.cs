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
            //print("CRAFT");
        }


        selectRecept(curentRecept);
    }


    public void selectRecept(receptCraft re)
    {
        curentRecept = re;

        sidebar.SetActive(true);
        transform.Find("sidebar/h1").GetComponent<Text>().text = re.name;


        string recText = "";
        foreach (receptCraftElement it in re.ingredients)
        {
            string _name = Global.Links.getModLoader().itemBaseGetFromInd(it.ind).nameView;
            recText += "\n " + _name+ " ("+it.cout+" x) ";

        }

        transform.Find("sidebar/rec").GetComponent<Text>().text = recText;



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
