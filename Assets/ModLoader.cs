﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Global;
using MyProg;
using System;

public enum itemType
{
    none = 0,
    block = 1,
    gun = 2,
    sword = 3,
    eat = 4,
    granate = 15,
}

public class itemSave
{

     
    public string nameView; 
    public string modName; 
    public string ind; 
    public string iniFilePath;
    public int stackSize;

    public itemType type;
    //public IniFile iniF; 
    public Texture2D icon;
    public Texture2D iconInHand;
}

public class ModLoader : MonoBehaviour
{

    private string pathMods;

    public Texture2D defIcon;


    public  itemSave itemBaseGetFromInd(string ind)
    {
        itemSave res = itemBase[ind] as itemSave; 
        return res;
    }

    void modInstall(string modName)
    {
        string pModDir = pathMods + modName + "/";



        DirectoryInfo dir = new DirectoryInfo(pModDir + "items");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {


            IniFile MyIni = new IniFile(f.FullName);

            string nameItem = f.Name;
            nameItem = nameItem.Replace(".ini", "");
            string ind = modName + ":" + nameItem;



            itemSave newItem = new itemSave();
            // newItem.iniF = MyIni;
            newItem.iniFilePath = f.FullName;
            newItem.ind = nameItem;
            newItem.modName = modName;

            newItem.nameView = MyIni.Read("name", "itemInfo");

            
            newItem.stackSize = Convert.ToInt32(MyIni.Read("stackSize", "itemInfo"));
            
             
            newItem.icon = defIcon; 

            string pToIcon = pModDir + "texture/" + MyIni.Read("icon", "itemInfo");
            if (File.Exists(pToIcon))
            {
                Texture2D tNew = LoadPNG(pToIcon);
                newItem.icon = tNew;
            }
            else
            {
                print("["+ ind + "] No exist icon item path: " + pToIcon);
            }


            newItem.iconInHand = newItem.icon;

            pToIcon = pModDir + "texture/" + MyIni.Read("iconInHand", "itemInfo");
            if (string.Empty != pToIcon)
            {
                if (File.Exists(pToIcon))
                {
                    Texture2D tNew = LoadPNG(pToIcon);
                    newItem.iconInHand = tNew;
                }
            }



            

            string _myType = MyIni.Read("type", "itemInfo");
            if (String.Empty != _myType)
            {
                newItem.type = (itemType)Enum.Parse(typeof(itemType), _myType);
            }
            else
            {
                newItem.type = 0;
            }

            


            itemBase.Add(ind, newItem);

            Global.Links.getPlayerInv().myData.itemAdd(ind);

        }


    }

    void createMod(string modName)
    {
   

        //string p = ("Assets/Resources/Mods/" + modName+"/");
        string p = (pathMods + modName+"/");
        if (Directory.Exists(p))
        {
            //return;
        }


        Directory.CreateDirectory(p);
        Directory.CreateDirectory(p+"blocks");
        Directory.CreateDirectory(p+"items");
        Directory.CreateDirectory(p+"texture");

        var MyIni = new IniFile(p + "/items/testItem.ini");
        MyIni.Write("price", "12", "itemInfo");
        MyIni.Write("stackSize", "64", "itemInfo");
        MyIni.Write("icon", "null", "itemInfo");
        MyIni.Write("icon", "test.png", "itemInfo");
        MyIni.Write("mass", "35", "itemInfo");




        MyIni = new IniFile(p + "/blocks/testItem.ini");
        MyIni.Write("price", "3", "itemInfo");
        MyIni.Write("stackSize", "64", "itemInfo");
        MyIni.Write("icon", "null", "itemInfo");
        MyIni.Write("icon", "test.png", "itemInfo");
        MyIni.Write("mass", "35", "itemInfo");

        MyIni.Write("hp", "35", "block");
        MyIni.Write("damageType", "axe", "block");
        MyIni.Write("texture", "wood.png", "block"); 


    }

    void modInstallAll()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            modInstall(f.Name);
            print(f.Name);
        }

    }


    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }


  


    public Texture2D getItemSprite()
    {
        string p = pathMods + "Mine/texture/" + "wood_hoe.png";
        Texture2D tNew = LoadPNG(p);
        return tNew;
    }


    void textureTest(BlockController block)
    {
        string p = pathMods + "Mine/texture/" + "planks_big_oak.png";
        Texture2D tNew = LoadPNG(p);

        block.GetComponent<MeshRenderer>().material.mainTexture = tNew;
    }

     public Dictionary<string, itemSave> itemBase = new Dictionary<string, itemSave>();

 

    

    void Start()
    {

         


        pathMods = Application.dataPath + "/../Mods/";
        modInstall("Weapon");
        modInstall("Block");


        Global.Links.getMapController().mapLoad();

      //  createMod("Mine");
       


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
