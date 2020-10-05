using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapController : MonoBehaviour
{

    public BlockController blockBlank;
    public ChankController chankBlank;

    public int blockCountWidth = 50;  //x, z
    public int blockCountHeight = 50; //y


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

        for (int ix = 0; ix < chankCountStart; ix++)
        {
            for (int iz = 0; iz < chankCountStart; iz++)
            {
                ChankController b = Instantiate(chankBlank, transform);
                b.transform.name = "chank:" + ix.ToString() + ":" + iz.ToString();
                b.transform.localPosition = new Vector3(ix * blockCountWidth, 0, iz * blockCountWidth);

                b.pathChankFile = mapPathDir + "" + b.transform.name.Replace(":", "_") + ".txt";


                b.mapCon = this;
                // b.genChank();
            }
        }

    }





    public void mapSave()
    {
        print("MAP SAVE");

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ChankController>().chankSave();

        }

    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {

            mapSave();

        }


    }
}