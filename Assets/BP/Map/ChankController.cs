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

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = Global.Links.getPlayerAction().gameObject;


    }

    float timer=0f;
    void slowUpdate()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);



        if (distToPlayer <= 20f)
        {
            loadAutoChank();
        }
        else
        {
            if (distToPlayer > 19f)
            {
                deloadChank((distToPlayer>40f));
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
        string chData = getData();
    

        metaDataBlocksSave();

        
        if (!File.Exists(pathChankFile))
        {
            File.Create(pathChankFile).Close();


        }
        File.WriteAllText(pathChankFile, chData);
    }


    public string pathChankFile;


    public bool deloadChank(bool isDelte = false)
    {
        if (!isLoaded) return false;

        

        //Полное удаление чанка
        if (isDelte)
        {
            isLoaded = false;

            print("delete chank");

            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

        }


        if (!isActive) return false;
        //Только отключ блоков, чтоб потом не подгружать
        if (!isDelte)
        {
          //  print("deActive chank");
            for (int i = 0; i < transform.childCount; i++)
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
                for (int i = 0; i < transform.childCount; i++)
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


        Global.Links.getOtherInv().closeCargo();
        Global.Links.getPlayerAction().setCurLock(true);

        if (File.Exists(pathChankFile))
        {


            print("Load chank: "+ pathChankFile);

            string saveDataChank = "";

            StreamReader writer = new StreamReader(pathChankFile, true);
            saveDataChank = writer.ReadToEnd();
            writer.Close();



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


        string[] lines = dataLoad.Split('\n');



        if (lines.Length < 10)
        {
            print("no valid data chank loader");
            return false;
        }

        //удаляем блоки внутри
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


        for (int i=1;i< lines.Length; i++)
        {
            
            string[] lineOne = lines[i].Split(' ');

      

            BlockController b = Instantiate(mapCon.blockBlank, transform);
            b.transform.name = lineOne[0];
            b.itemInd = lineOne[1];

           

            b.hp = float.Parse(lineOne[2]);
            //if (b.hp < 0) b.hp = 1f;

            b.setToItemInd();


            string[] posBlock = lineOne[0].Split(':');
            
            b.transform.localPosition = getCubePosFix(new Vector3(System.Convert.ToInt32(posBlock[0]), System.Convert.ToInt32(posBlock[1]), System.Convert.ToInt32(posBlock[2])));
            //lineOne[0]


            if (b.myType == blockType.cargo)
            {

                loadDataInvInBlock(b);
            }
        }


        return true;

    }

    public void loadDataInvInBlock(BlockController b)
    {

        string path = mapCon.mapPathDir + "" + b.transform.name.Replace(":", "_") + "_inv.txt";

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


    public void metaDataBlocksSave()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            BlockController b = transform.GetChild(i).GetComponent<BlockController>();
            if (b.myType == blockType.cargo)
            {

                string path = mapCon.mapPathDir + "" + b.transform.name.Replace(":", "_") + "_inv.txt";
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }

                File.WriteAllText(path, b.GetComponent<invData>().dataGet());

            }

        }
    }

    public string getData()
    {
        string Out ="";

        for (int i = 0; i <transform.childCount; i++)
        {
            string line = "\n";
            line += transform.GetChild(i).transform.name;

           BlockController b = transform.GetChild(i).GetComponent<BlockController>();

            string itemInd = b.itemInd;
            if (itemInd == "" || itemInd == " ") itemInd = "not";

            line += " "+ itemInd;
            line += " "+ Mathf.RoundToInt(b.hp).ToString();
            Out += line;
           // print(line);
        }


            return Out;

    }

    public void genNewStructureChank()
    {

        

        print("Gen new chank");

        int hCorrent = 1;

        for (int ix = 0; ix < mapCon.blockCountWidth; ix++)
        {
            
            hCorrent = hCorrent + Random.Range(-1, 1);

            if (hCorrent < 0) hCorrent = 0;

            for (int iz = 0; iz < mapCon.blockCountWidth; iz++)
            {


                if (Random.Range(1, 14) <= 2)
                {
                    hCorrent = hCorrent + Random.Range(-1, 1);
                    if (hCorrent < 0) hCorrent = 0;
                }


                for (int iy = 0; iy < mapCon.blockCountHeight; iy++)
                {
                    BlockController b = Instantiate(mapCon.blockBlank, transform);
                    b.transform.name = "" + ix.ToString() + ":" + iy.ToString() + ":" + iz.ToString();
                    int yPos = iy;
                    if (iy + hCorrent > 0) yPos = iy + hCorrent;

                  //  b.transform.localPosition = new Vector3(ix, yPos, iz);
                    b.transform.localPosition = getCubePosFix( new Vector3(ix, yPos, iz));
                }
            }
        }

    }

    public Vector3 getCubePosFix( Vector3 posIn)
    {
        posIn.x -=Mathf.Round( mapCon.blockCountWidth / 2);
        posIn.z -=Mathf.Round( mapCon.blockCountWidth / 2);

        return posIn;
    }
}
