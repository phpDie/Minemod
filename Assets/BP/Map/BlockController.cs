using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using MyProg;
using System;


public enum blockMaterial
{
    all= 0,
    wood=1,
    ground=2,
    glass=3,
    metal=4,
    badrock=5,
    meat = 6,
    stone = 7,
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
    public blockMaterial myMaterial;

    public float hpMax = 9f;
    public float hp = 8f;
    public float myWidth = 1f;
    public float myHeight = 1f;

    

    public string dropInd = "";



    public GameObject myCrackEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    //Вернуть дроп
    public string getDrop()
    {
      

        return dropInd;
    }

    public void Damage(float dam, blockMaterial attackMaterial = blockMaterial.all,GameObject author =null)
    {
       

        float cofDamage = 1f;

        if (attackMaterial != myMaterial)
        {
            cofDamage = 0.4f;
            if (attackMaterial== blockMaterial.all) cofDamage = 1f;
            if(attackMaterial== blockMaterial.meat) cofDamage = 0.3f;
            if(myMaterial == blockMaterial.glass) cofDamage = 1f;
            if(myMaterial == blockMaterial.badrock) cofDamage = 0f;
            if(myMaterial == blockMaterial.all) cofDamage = 1f;
            if(myMaterial == blockMaterial.metal && attackMaterial==blockMaterial.stone) cofDamage = 1f;
        } 

        dam = dam * cofDamage;
        hp -= dam;


        if (hp < hpMax * 0.5f)
        {
            myCrackEffect.SetActive(true);
        }

        if (hp <= 0f)
        {
            if (author != null)
            {
                string _drop = getDrop();
                if (_drop != "")
                {
                    author.GetComponent<PlayerAction>().myInv.myData.itemAdd(_drop,1);
                }
            }
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


        if (itemInd != "Chest:adminCargo")
        {

            //myInv.dataSet();


        }


        if (itemInd == "Chest:adminCargo")
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

        myData = Global.Links.getModLoader().itemBaseGetFromInd(itemInd);

        if (myData == null)
        {
            return;
        }


        IniFile MyIni = new IniFile(myData.iniFilePath);

        myWidth = MyIni.ReadInt("width", "block", 100)/100f;
        myHeight = MyIni.ReadInt("height", "block", 100)/100f;


        hpMax = MyIni.ReadInt("hp", "block", 5);
        hp = hpMax;


        myMaterial = (blockMaterial)Enum.Parse(typeof(blockMaterial), MyIni.Read("material", "block", "ground"));


        myType = (blockType)Enum.Parse(typeof(blockType), MyIni.Read("blockType", "block", "none"));


        
        if (   MyIni.ReadBool("hp", "block", true) == true)
        {

            dropInd = MyIni.Read("drop", "block", itemInd);
            if (dropInd == "self") dropInd = itemInd;
            if (dropInd == "not") dropInd = "";

        }



        transform.localScale = new Vector3(myWidth, myHeight, myWidth);

        GetComponent<MeshRenderer>().material.mainTexture = myData.icon;
        //GetComponent<Material>().mod

        if (myType == blockType.cargo)
        {
            createCargo();
        }
    }
}