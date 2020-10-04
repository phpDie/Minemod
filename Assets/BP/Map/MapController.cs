using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapController : MonoBehaviour
{

    public BlockController blockBlank;
    public ChankController chankBlank;

    public int blockCountWidth = 50;  //x, z
    public int blockCountHeight =50; //y


    private int chankCountStart = 1; //y


   public string mapPathDir;
    // Start is called before the first frame update
    void Start()
    {

        mapPathDir = Application.dataPath + "/../m_map/";
        Directory.CreateDirectory(mapPathDir);

        for (int ix = 0; ix < chankCountStart; ix++)
        {
            for (int iz = 0; iz < chankCountStart; iz++)
            {
                ChankController b = Instantiate(chankBlank, transform);
                b.transform.name = "chank:"+ix.ToString() + ":" + iz.ToString();
                b.transform.localPosition = new Vector3(ix * blockCountWidth, 0, iz * blockCountWidth);
                //b.blockCountWidth = blockCountWidth;
                //b.blockBlank = blockBlank;
                b.mapCon = this;
                b.genChank();
            }
        }

      


    }

 
    public void mapSave()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            string chData = (transform.GetChild(i).GetComponent<ChankController>().getData());


            transform.GetChild(i).GetComponent<ChankController>().metaDataBlocksSave();

            string path = mapPathDir + "" + transform.GetChild(i).name.Replace(":", "_") + ".txt";
            
              

            //FileStream f=  File.OpenWrite(path);
            if (!File.Exists(path))
            { 
                File.Create(path).Close();

            
            }
            File.WriteAllText(path, chData);

            
        }

    }

    public void mapLoad()
    {

        Global.Links.getOtherInv().closeCargo();
        Global.Links.getPlayerAction().setCurLock(true);

        for (int i = 0; i < transform.childCount; i++)
        {
            string path = mapPathDir + "" + transform.GetChild(i).name.Replace(":", "_") + ".txt";

            if (File.Exists(path))
            {
                string saveDataChank ="";

                StreamReader writer = new StreamReader(path, true);
                saveDataChank= writer.ReadToEnd();
                writer.Close();

                

                transform.GetChild(i).GetComponent<ChankController>().loadData(saveDataChank);
            }
            else
            {

            }
        }
    }

        void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            mapSave();

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            mapLoad();

        }
    }
}
