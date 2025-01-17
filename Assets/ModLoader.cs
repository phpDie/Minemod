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
    handTool = 3, //ручной инструмент. Крика,лопата,топор, так же меч, копье
    eat = 4,
    bat = 5,
    granate = 15, 
}


public class builderThemElement
{
   public string ind;
    public Vector3 pos;
}


public class builderThem
{
    public List<builderThemElement> list = new List<builderThemElement>();
}


public class blockGenWorld
{
    public int minY = 0;
    public int maxY = 0;
    public int rand = 0;
    public string ind= "Core:dirt";
    public bool top;
}



public class itemBlockSettings
{
    public float width = 1f;
    public float height = 1f;
    public int hpMax = 5; 

     

    
    public int buildCount = 0;  //блок билдер
    public string dropInd="not"; 

    public blockMaterial material = blockMaterial.ground; 
    public blockType type = blockType.none;

    public Texture2D icon;
}


public class wifiType
{
    public string name = "Жижа";
    public string ind = "not";
    public string ed = "j";
}


public class itemSave
{

    public int startSize = 5;


    public bool emptyIsset = true; //удалять предмет, когда у него колв = 0. Нет для пушек.

    public string nameView; //название
    public string descr; //название

    public string modName; 
    public string ind;  //инд
    public string wifi="nont";  //инд
    public string iniFilePath; //путь до ини файла
    public int stackSize = 0;//размер стака
    public bool stackHpMode; //количество предмета это хп. То есть для лопат, кирок, пушку. стак=хп

    public itemType type; //тип предмета, блок, оружие..
    //public IniFile iniF; 
    public Texture2D icon;
    public Texture2D iconInHand; //иконка в руке, если отличается
    public blockGenWorld genWorl;
}

public class ModLoader : MonoBehaviour
{



    private string pathMods;

    public Texture2D defIcon;


    public itemSave itemBaseGetFromInd(string ind)
    {

        if (!itemBase.ContainsKey(ind))
        {
            return null;
        }

        itemSave res = itemBase[ind] as itemSave;
        return res;
    }

    public wifiType wifiGetFromInd(string ind)
    {

        if (!wifi.ContainsKey(ind))
        {
            return null;
        }

        wifiType res = wifi[ind] as wifiType;
        return res;
    }

    void modInstall_wifi(string modName)
    {
        string pModDir = pathMods + modName + "/energy/";
 
        if (!Directory.Exists(pModDir)) return;

        DirectoryInfo dir = new DirectoryInfo(pModDir);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            IniFile MyIni = new IniFile(f.FullName);
            string en_name = MyIni.Read("name", "energy", "not");

            if (en_name != "not")
            {

                wifiType wNew = new wifiType();
                wNew.ind = f.Name.Replace(".ini", "");
                wNew.ed = MyIni.Read("ed", "energy", "xz");
                wNew.name = MyIni.Read("name", "energy", "xz");
                 
                wifi.Add(wNew.ind, wNew);

            }

        }
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

            

            newItem.modName = modName;

            newItem.ind = modName+":"+nameItem;

            newItem.wifi = MyIni.Read("wifi", "itemInfo","not");

            newItem.nameView = MyIni.Read("name", "itemInfo");
            newItem.descr = MyIni.Read("descr", "itemInfo","");
            newItem.emptyIsset = MyIni.ReadBool("emptyIsset", "itemInfo");


            newItem.stackSize = MyIni.ReadInt("stackSize", "itemInfo",1);


            newItem.startSize = MyIni.ReadInt("startSize", "itemInfo",-1);
            if (newItem.startSize == -1)
            {
                newItem.startSize = newItem.stackSize;
            }



            newItem.stackSize = MyIni.ReadInt("stackSize", "itemInfo",1);
            newItem.stackHpMode =MyIni.ReadBool("stackHpMode", "itemInfo",false);



            newItem.icon = defIcon;

            string pToIcon = pModDir + "texture/" + MyIni.Read("icon", "itemInfo");
            if (File.Exists(pToIcon))
            {
                Texture2D tNew = LoadPNG(pToIcon);
                newItem.icon = tNew;
            }
            else
            {
                print("[" + ind + "] No exist icon item path: " + pToIcon);
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





            newItem.type = (itemType)Enum.Parse(typeof(itemType), MyIni.Read("type", "itemInfo", "none"));


            if (ind != "Core:dirt")
            {
                if (MyIni.ReadInt("minY", "gen", -1) > -1)
                {


                    blockGenWorld genWorl = new blockGenWorld();
                    genWorl.minY = MyIni.ReadInt("minY", "gen", 0);
                    genWorl.maxY = MyIni.ReadInt("maxY", "gen", 0);
                    genWorl.rand = MyIni.ReadInt("rand", "gen", 0);
                    genWorl.top = MyIni.ReadBool("top", "gen", false);
                    genWorl.ind = ind;

                    blockGenMap.Add(genWorl); //ЭТОТ БЛОК ГЕНЕРИТ МИР
                }
            }

        
            itemBase.Add(ind, newItem);
            

            Global.Links.getIndDataAdminCargo().itemAdd(ind,-1);

            if (MyIni.Read("material", "block", "not") != "not")
            {

                itemBlockSettings newblockBase = new itemBlockSettings();
                newblockBase.icon = newItem.icon;

                newblockBase.width = MyIni.ReadInt("width", "block", 100) / 100f;
                newblockBase.height = MyIni.ReadInt("height", "block", 100) / 100f;
                newblockBase.hpMax = MyIni.ReadInt("hp", "block", 5);
                newblockBase.material = (blockMaterial)Enum.Parse(typeof(blockMaterial), MyIni.Read("material", "block", "ground"));
                newblockBase.type = (blockType)Enum.Parse(typeof(blockType), MyIni.Read("blockType", "block", "none"));


                newblockBase.buildCount = MyIni.ReadInt("buildCount", "build", 0);
                if (newblockBase.buildCount > 0)
                {
                    builderThem bTnew = new builderThem();

                    for (int i = 0; i < newblockBase.buildCount; i++)
                    {

                        string line = MyIni.Read("b" + i.ToString(), "build", "not");
                        if (line != "not")
                        {
                            // print("Ошибка чтения билда: " + i.ToString());
                            //  return;
                             

                            string[] conf = line.Split(' ');


                            string[] posBlock = conf[0].Split(':');

                            builderThemElement _bE= new builderThemElement();

                            _bE.pos = new Vector3(System.Convert.ToInt32(posBlock[0]),  System.Convert.ToInt32(posBlock[1]),  System.Convert.ToInt32(posBlock[2]));

                            _bE.ind = conf[1];

                            bTnew.list.Add(_bE);
                        }
                    }

                    builderThemplates.Add(ind,bTnew);

                }


                        newblockBase.dropInd = MyIni.Read("drop", "block", "self");
                if (newblockBase.dropInd == "self") newblockBase.dropInd = ind;
                if (newblockBase.dropInd == "not") newblockBase.dropInd = "not";

                
                blockBase.Add(ind, newblockBase);
            }


            //Подгрузка рецептов крафат
            int craftIsset = MyIni.ReadInt("craftCount", "craft", 0);
            if (craftIsset > 0)
            {

                receptCraft _craft = new receptCraft();
                _craft.name = newItem.nameView;
                _craft.ind = ind;
                _craft.icon = newItem.icon;
                _craft.cout = MyIni.ReadInt("craftGive", "craft", 0);  ///мы получем

                for (int i = 0; i < craftIsset; i++)
                {
                    receptCraftElement _ingrCraft = new receptCraftElement();
                    _ingrCraft.ind = MyIni.Read("craftIngr" + i.ToString(), "craft", "not");

                    string[] _pars = _ingrCraft.ind.Split(' ');

                    if (_pars.Length != 2)
                    {
                        print(ind + " error recept craft "+ " " + _ingrCraft.ind);
                    }

                    _ingrCraft.ind = _pars[0];
                    _ingrCraft.cout = System.Convert.ToInt32(_pars[1]);


                    if (_ingrCraft.ind != "not")
                    {
                        _craft.ingredients.Add(_ingrCraft);
                    }
                }

                Global.Links.getCraftUi().items.Add(_craft);

            }


        }


    }

    void createMod(string modName)
    {


        //string p = ("Assets/Resources/Mods/" + modName+"/");
        string p = (pathMods + modName + "/");
        if (Directory.Exists(p))
        {
            //return;
        }


        Directory.CreateDirectory(p);
        Directory.CreateDirectory(p + "blocks");
        Directory.CreateDirectory(p + "items");
        Directory.CreateDirectory(p + "texture");

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
    public Dictionary<string, wifiType> wifi = new Dictionary<string, wifiType>();


    //Это кэш предметов которые блоки. Что бы не читать ини файлы и тд.
    public Dictionary<string, itemBlockSettings> blockBase = new Dictionary<string, itemBlockSettings>();
    public Dictionary<string, builderThem> builderThemplates = new Dictionary<string, builderThem>();



    // public Dictionary<string, itemSave> blockGenMap = new Dictionary<string, itemSave>();

    public List<blockGenWorld> blockGenMap = new List<blockGenWorld>();


    void craftChecker()
    {

        foreach (receptCraft it in Global.Links.getCraftUi().items)
        {

            foreach (receptCraftElement ingr in it.ingredients)
            {

                if (it.ind == ingr.ind)
                {
                    print($"В крафте {it.ind} рекурсивный ингредиент");
                }

                //ingr.ind
                if (itemBaseGetFromInd(ingr.ind) == null)
                {
                    print($"В крафте {it.ind} ингр которого нет в игре = {ingr.ind}");
                }

            }
        }
    }


    void Start()
    {



        var watch = System.Diagnostics.Stopwatch.StartNew();


        pathMods = Application.dataPath + "/../Mods/";
        /*
        modInstall("Agregat");
        modInstall("Eat");
        modInstall("Weapon");
        modInstall("Block");
        modInstall("Chest");
        modInstall("Core");
        modInstall("Ruda");
        modInstall("Hlam");
        */

        string pModDir = pathMods ;
        DirectoryInfo dir = new DirectoryInfo(pModDir);
        DirectoryInfo[] info = dir.GetDirectories("*.*");

        foreach (DirectoryInfo f in info)
        { 
           modInstall_wifi(f.Name);
        }
        foreach (DirectoryInfo f in info)
        {
            modInstall(f.Name);
        }

      



            watch.Stop();
        print($"Mod Loaded: {watch.ElapsedMilliseconds} ms");


         
        Global.Links.getPlayerInvUi().selectNewActiveInde(5);

        Global.Links.getCraftUi().firstRender();


        Global.Links.getIndDataAdminCargo().ReRender(true);

        if (Global.Links.getIndDataAdminCargo().size < itemBase.Count)
        {
            print($"Не хватает места в админ сундуке, у нас вещей: {itemBase.Count}");
        }

        craftChecker();


    }

    void FirstTick()
    {

        Global.Links.getMapController().mapInit();

        //print("LOAD GAME");
        Global.Links.getMapController().gameLoad();
    }





    float timerFirstTick = 0.4f;
    // Update is called once per frame
    void Update()
    {
        if (timerFirstTick != -1f)
        {
            if (timerFirstTick > 0f)
            {
                timerFirstTick -= Time.deltaTime;
            }
            else
            {
                FirstTick();
                timerFirstTick = -1f;
            }
        }

    }

   
}
