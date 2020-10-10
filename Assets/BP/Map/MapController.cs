using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapController : MonoBehaviour
{

    public Dictionary<string, bool> map = new Dictionary<string, bool>();


    public BlockController blockBlank;
    public ChankController chankBlank;

    public int preloadChankInStart = 0; //Прелодить чанки при старте игры, это долго зато без фриз
                                        //x, z
    public int blockCountHeight = 9; //y


    [HideInInspector]
    public int blockCountWidth = 16;
    private int chankCountStart = 5; //y
    public string mapPathDir;
    // Start is called before the first frame update


    void Start()
    {

        mapPathDir = Application.dataPath + "/../m_map/";
        Directory.CreateDirectory(mapPathDir);


        // mapInit();


    }


    bool isInit = false;
    public void mapInit()
    {
        if (isInit) return;

        for (int ix = -chankCountStart; ix <= chankCountStart; ix++)
        {
            for (int iz = -chankCountStart; iz <= chankCountStart; iz++)
            {

                ChankController b = Instantiate(chankBlank, transform);
                b.transform.name = "chank:" + ix.ToString() + ":" + iz.ToString();
                b.transform.localPosition = new Vector3(ix * blockCountWidth, 0, iz * blockCountWidth);

                b.pathChankFile = mapPathDir + "" + b.transform.name.Replace(":", "_") + ".txt";


                b.myPosGlobalX = ix;
                b.myPosGlobalZ = iz;

                b.mapCon = this;
                b.init();

                if (Mathf.Abs(ix) <= preloadChankInStart && Mathf.Abs(iz) <= preloadChankInStart)
                {
                    //print($"CHANK PRELOA {ix}:{iz}");
                    b.loadAutoChank();
                }
                // b.genChank();
            }
        }

    }





    public void gameLoad()
    {
        string path = mapPathDir + "invMain.txt";
        if (File.Exists(path))
        {
            string saveDataChank;
            StreamReader writer = new StreamReader(path, true);
            saveDataChank = writer.ReadToEnd();
            writer.Close();

            Global.Links.getIndDataPlayerCargo().dataSet(saveDataChank);
        }


        path = mapPathDir + "invAction.txt";
        if (File.Exists(path))
        {
            string saveDataChank;
            StreamReader writer = new StreamReader(path, true);
            saveDataChank = writer.ReadToEnd();
            writer.Close();

            Global.Links.getIndDataPlayerAction().dataSet(saveDataChank);
        }


        path = mapPathDir + "ships/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
           
            string saveDataChank;
            StreamReader writer = new StreamReader(f.FullName, true);
            saveDataChank = writer.ReadToEnd();
            writer.Close();

            string[] lines = saveDataChank.Split(' ');


            GameObject objToSpawn = new GameObject("driveChank");
            objToSpawn.transform.position = Global.Links.stringToVector(lines[1]);

            driverChank car = objToSpawn.AddComponent<driverChank>();
            car.gameObject.AddComponent<ChankController>().permGenOn=false;
            car.gameObject.GetComponent<ChankController>().mapCon = this;
            car.gameObject.GetComponent<ChankController>().init();
            

            car.init(f.Name.Replace(".txt", ""));
             


        }
    }

    public void gameSave()
    {

        string path = mapPathDir + "invMain.txt";
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
        File.WriteAllText(path, Global.Links.getIndDataPlayerCargo().dataGet());


        path = mapPathDir + "invAction.txt";
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
        File.WriteAllText(path, Global.Links.getIndDataPlayerAction().dataGet());



    }


    public void mapSave()
    {

        gameSave();


        driverChank[] gos = FindObjectsOfType(typeof(driverChank)) as driverChank[];
        foreach (driverChank go in gos)
        {
            go.saveData();

        }


        var watch = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ChankController>().chankSave();

        }
        watch.Stop();
        print($"FULL MAP SAVE Time: {watch.ElapsedMilliseconds} ms");

    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {

            mapSave();

        }


    }
}