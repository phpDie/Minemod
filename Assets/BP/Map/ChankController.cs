using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChankController : MonoBehaviour
{


    public MapController mapCon; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

            b.setToItemInd();



            
           string[] posBlock = lineOne[0].Split(':');
            
            b.transform.localPosition = new Vector3(System.Convert.ToInt32(posBlock[0]), System.Convert.ToInt32(posBlock[1]), System.Convert.ToInt32(posBlock[2]));
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
            line += " "+ transform.GetChild(i).GetComponent<BlockController>().itemInd;
            Out += line;
           // print(line);
        }


            return Out;

    }

    public void genChank()
    {
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
                    b.transform.localPosition = new Vector3(ix, yPos, iz);
                }
            }
        }

    }
}
