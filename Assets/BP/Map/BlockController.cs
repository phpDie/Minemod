using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using MyProg;
using System;


public enum blockMaterial
{
    stone=0,
    wood=1,
    ground=2,
    glass=3,
    metal=4,
    badrock=5,
}


public enum blockType
{
    none = 0,
    cargo = 1,
    door = 2,
}

public class BlockController : MonoBehaviour
{
    public string itemInd = "def:not";
    itemSave myData;
    public blockType myType;

    public float hpMax =8f;
    public float hp = 8f;


    public GameObject myCrackEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }
     
    public void Damage(float dam)
    {
        hp -= dam;

        if (hp < hpMax * 0.5f)
        {
            myCrackEffect.SetActive(true);
        }

        if (hp <= 0f)
        {
            //transform.parent.GetComponent<ChankController>();
            Destroy(gameObject);
        }
    }

  


    public void createCargo()
    {
        invData myInv = gameObject.AddComponent<invData>();



        myInv.init();
        //myInv.itemAdd("Weapon:arrow");
        // myInv.itemAdd("Weapon:arrow");


        if (itemInd != "Block:adminCargo")
        {

            //myInv.dataSet();


        }


        if (itemInd == "Block:adminCargo")
        {
          //  print("Admin chest");

            foreach (KeyValuePair<string, itemSave> item in Global.Links.getModLoader().itemBase)
            {
                myInv.itemAdd(item.Key);

            }


        }

    }
    public void setToItemInd()
    {
        if (itemInd == string.Empty) return;
        if (itemInd == "def:not") return;

        myData =Global.Links.getModLoader().itemBaseGetFromInd(itemInd);

        

       IniFile MyIni = new IniFile(myData.iniFilePath);
      
        string _myType = MyIni.Read("blockType", "block");
        if (String.Empty != _myType)
        {
            myType = (blockType)Enum.Parse(typeof(blockType), _myType);
        }
        else
        {
            myType = 0;
        }


        GetComponent<MeshRenderer>().material.mainTexture = myData.icon;
        //GetComponent<Material>().mod

        if (myType == blockType.cargo)
        {
            createCargo();
        }
    }
}
