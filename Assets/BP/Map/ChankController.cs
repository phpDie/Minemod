using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChankController : MonoBehaviour
{


    public bool isLoaded; //загружен ли этот чанк
    public bool isActive; //активен ли этот чанк
    //public bool isGeneredBlank = false; //активен ли этот чанк



    public MapController mapCon;
    public ModLoader mod;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
       // init();



    }

    public void init()
    {
        player = Global.Links.getPlayerAction().gameObject;
        mod = Global.Links.getModLoader();


    }

    float chankDist = 23f;

    float timer=0f;
    void slowUpdate()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);



        if (distToPlayer <= chankDist)
        {
            loadAutoChank();
        }
        else
        {
            if (distToPlayer > chankDist*1.2f)
            {
                deloadChank((distToPlayer> chankDist*6f));
            }
        }

    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            slowUpdate();
            timer = 0.5f;
        }
    }

    //Сохраняем чанк в файл
    public void chankSave()
    {
        if (!isLoaded) return;
        //if (!isActive) return false;
    
       // var watch = System.Diagnostics.Stopwatch.StartNew();
        
       
        string chData = getData();
    

        metaDataBlocksSave();

        
        if (!File.Exists(pathChankFile))
        {
            File.Create(pathChankFile).Close();


        }
        File.WriteAllText(pathChankFile, chData);

      //  watch.Stop();
      //  print($"Save chank Time: {watch.ElapsedMilliseconds} ms");
    }


    public string pathChankFile;


    public bool deloadChank(bool isDelte = false)
    {
        if (!isLoaded) return false;

         
        


        //Полное удаление чанка
        if (isDelte)
        {
            chankSave();

            isLoaded = false;

            //print("delete chank");

            for (int i = 1; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

        }


        if (!isActive) return false;
        //Только отключ блоков, чтоб потом не подгружать
        if (!isDelte)
        {

            //  print("deActive chank");
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            isActive = false;

        }
         

        return true;
    }

    //Подгрузить этот чанк. Он сам решает, генерится ему, или загрузится
    public bool loadAutoChank()
    {


        if (isLoaded)
        {
            if (isActive) return false; //чанк активен и загружен


            //Чанк даективирован просто
            if (!isActive)
            {
              //  print("active chank");
                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                isActive = true;
                return true;
            }

        }


        
        isLoaded = true;

        

         
        /*
        if (!isGeneredBlank)
        {
            genChank();
            isGeneredBlank = true;
        }
        */


        Global.Links.getOtherInvUi().closeCargo();
        Global.Links.getPlayerAction().setCurLock(true);

        if (File.Exists(pathChankFile))
        {

           
           // print("Load FILE chank: "+ pathChankFile);

            string saveDataChank = "";

            StreamReader writer = new StreamReader(pathChankFile, true);
            saveDataChank = writer.ReadToEnd();
            writer.Close();

            //print("loadEnd");
         


            return loadData(saveDataChank);
        }
        else
        {
            genNewStructureChank();
        }

        return true;
    }



    public bool loadData(string dataLoad)
    {


        var watch = System.Diagnostics.Stopwatch.StartNew();
       

        string[] lines = dataLoad.Split('\n');



        if (lines.Length < 10)
        {
            print("no valid data chank loader");
            return false;
        }

        //удаляем блоки внутри
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


        for (int i=1;i< lines.Length; i++)
        {
            
            string[] lineOne = lines[i].Split(' ');

      

            BlockController b = Instantiate(mapCon.blockBlank, transform);
            b.transform.name = lineOne[0];
            b.itemInd = lineOne[1];


            b.mod = mod;

            b.hp = float.Parse(lineOne[2]);



            itemBlockSettings myBlockSetting = mod.blockBase[b.itemInd];
            b.myBlockSetting = myBlockSetting;

            b.myType = myBlockSetting.type;

            b.initBlockPreload();
 



            string[] posBlock = lineOne[0].Split(':');


            //getCubePosFix здесь не нужен, потому что он должен быть только при создание мира
            //  b.transform.localPosition = getCubePosFix(new Vector3(System.Convert.ToInt32(posBlock[0]), System.Convert.ToInt32(posBlock[1]), System.Convert.ToInt32(posBlock[2])));
            b.transform.localPosition = (new Vector3(System.Convert.ToInt32(posBlock[0]), System.Convert.ToInt32(posBlock[1]), System.Convert.ToInt32(posBlock[2])));
           

            //b.transform.name = Global.Links.vectorToString(b.transform.localPosition);

            if (b.myType == blockType.cargo)
            {
                b.initBlock();
                loadDataInvInBlock(b);
            }

            if (b.myType == blockType.agregat)
            {
                string path = mapCon.mapPathDir + "agregatChank" + transform.name.Replace(":", "_") + "_cube" + b.transform.name.Replace(":", "_") + "_agr.ini";
                b.initBlock();
                b.GetComponent<blockTypeAgregat>().loadData(path);
            }
        }


        watch.Stop();
      //  print($"Parse chank file Time: {watch.ElapsedMilliseconds} ms");


        return true;

    }

    public void loadDataInvInBlock(BlockController b)
    {
        string path = mapCon.mapPathDir + "cargoChank" + transform.name.Replace(":", "_") + "_cube" + b.transform.name.Replace(":", "_") + "_inv.txt";
        //string path = mapCon.mapPathDir + "" + b.transform.name.Replace(":", "_") + "_inv.txt";

        if (File.Exists(path))
        {
            string saveDataChank;
            StreamReader writer = new StreamReader(path, true);
            saveDataChank = writer.ReadToEnd();
            writer.Close();


            b.GetComponent<invData>().dataSet(saveDataChank);
             
        }
        else
        { 
        }


    }
    /*
    public void metaSaveCargo(BlockController b, )
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        File.WriteAllText(path, b.GetComponent<invData>().dataGet());
    }
    */
    public void metaDataBlocksSave()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<BlockController>() != null)
            {
                BlockController b = transform.GetChild(i).GetComponent<BlockController>();

                if (b.myType == blockType.agregat)
                {
                    string path = mapCon.mapPathDir + "agregatChank" + transform.name.Replace(":", "_") + "_cube" + b.transform.name.Replace(":", "_") + "_agr.ini";
                    b.GetComponent<blockTypeAgregat>().saveData(path);
                }

                    if (b.myType == blockType.cargo)
                {

                    string path = mapCon.mapPathDir + "cargoChank"+transform.name.Replace(":", "_") + "_cube" + b.transform.name.Replace(":", "_") + "_inv.txt";
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }

                    File.WriteAllText(path, b.GetComponent<invData>().dataGet());

                }

            }
        }
    }

    public string getData()
    {
        string Out ="";

        for (int i = 1; i <transform.childCount; i++)
        {
            string line = "\n";
            line += transform.GetChild(i).transform.name;

           
                BlockController b = transform.GetChild(i).GetComponent<BlockController>();

                string itemInd = b.itemInd;
                if (itemInd == "" || itemInd == " ") itemInd = "not";

                line += " " + itemInd;
                line += " " + Mathf.RoundToInt(b.hp).ToString();
                Out += line;
            
           // print(line);
        }


            return Out;

    }

    public void genNewStructureChank()
    {

        var watch = System.Diagnostics.Stopwatch.StartNew();

        // print("Gen new chank");

        int hCorrent = 1;

        for (int ix = 0; ix < mapCon.blockCountWidth; ix++)
        {


            for (int iz = 0; iz < mapCon.blockCountWidth; iz++)
            {

                /*
                if (Random.Range(1, 14) <= 3)
                {
                    hCorrent = hCorrent + Random.Range(-1, 1);
                    if (hCorrent < -2) hCorrent = -2;
                    if (hCorrent > 5) hCorrent = 5;
                }
                */


                for (int iy = 0; iy < mapCon.blockCountHeight; iy++)
                {



                   // if (iy + hCorrent >= mapCon.blockCountHeight -4)
                  //  {

                        if (Random.Range(1f, 100f) <=6f)
                        {
                            // print("RAND");
                          //  if (Vector2.Distance(new Vector2(ix, iz), new Vector2(mapCon.blockCountWidth / 2f, mapCon.blockCountWidth / 2)) <= mapCon.blockCountWidth / 3f)
                           
                                hCorrent += Random.Range(-2,2);
                                if (mapCon.blockCountHeight + hCorrent<3) hCorrent = -mapCon.blockCountHeight +3 +Random.Range(0, 1);
                                if (hCorrent > 1) hCorrent = 1;

                        }

                   // }


                    if (iy <= mapCon.blockCountHeight + hCorrent)
                    {
                        BlockController b = Instantiate(mapCon.blockBlank, transform);

                        b.mod = mod;

                        // if (iy + hCorrent > 0) yPos = iy + hCorrent;

                        int yPos = iy;
                        yPos = iy ;

                       
                        //  b.transform.localPosition = new Vector3(ix, yPos, iz);
                        b.transform.localPosition = getCubePosFix(new Vector3(ix, yPos, iz));


                       // b.transform.name = "" + ix.ToString() + ":" + iy.ToString() + ":" + iz.ToString();
                        b.transform.name = Global.Links.vectorToString(b.transform.localPosition ) ;

                        b.hp = 0;
                        b.itemInd = genBlockFromYPos(yPos, yPos == mapCon.blockCountHeight + hCorrent);


                        itemBlockSettings myBlockSetting = mod.blockBase[b.itemInd];
                        b.myBlockSetting = myBlockSetting;

                        b.initBlockPreload();
                        //b.initBlock();




                        /*
                    //Решаем когда билдить блок
                    if (yPos >= mapCon.blockCountHeight + hCorrent - 1)
                    {
                        b.initBlock();
                    }
                    else
                    {
                        itemBlockSettings myBlockSetting = mod.blockBase[b.itemInd];
                        if (myBlockSetting.buildCount > 0)
                        {
                            b.initBlock();
                        }
                    }
                    */

                    }
                }
            }
        }

        // chankSave();

        watch.Stop();
        print($"Chank gen Time: {watch.ElapsedMilliseconds} ms");
    }

    public string genBlockFromYPos(int yPos=0, bool top = false)
    {
        int myRand = Random.Range(0, 50);
        myRand += Random.Range(0, 30);
        myRand += Random.Range(0, 20);


        string altVar = "Core:dirt";

        if (yPos <= 0)
        {
            return "Core:bad"; 
        }

        for (int i = 0; i < mod.blockGenMap.Count; i++)
        {
          
                if (yPos >= mod.blockGenMap[i].minY && yPos <= mod.blockGenMap[i].maxY || (top && mod.blockGenMap[i].top))
                {

                   // print(mod.blockGenMap[i].ind);

                    if (mod.blockGenMap[i].rand >= myRand)
                    {

                        if (Random.Range(0, 4) <= 2)
                        {
                            return mod.blockGenMap[i].ind;
                        }
                        else
                        {

                            altVar = mod.blockGenMap[i].ind;
                        }

                    }

                }

            
        }



        return altVar;

    }


    public Vector3 getCubePosFix( Vector3 posIn)
    {
        posIn.x -=Mathf.Round( mapCon.blockCountWidth / 2f);
        posIn.z -=Mathf.Round( mapCon.blockCountWidth / 2f);

        return posIn;
    }
}
