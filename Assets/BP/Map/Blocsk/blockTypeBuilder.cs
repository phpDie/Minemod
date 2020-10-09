using MyProg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockTypeBuilder : MonoBehaviour
{

    Transform pl;
    string myiniFilePath;

    public ModLoader mod;

    public void init(string iniFilePath, ModLoader m)
    {
        mod = m;
        myiniFilePath = iniFilePath;

       // createBuild(myiniFilePath);
    }

    void Start()
    {
        pl = Global.Links.getPlayerAction().transform;

        InvokeRepeating("SlowUpdate", 0.0f, 0.25f + Random.Range(0f,1.6f));
    }

    bool isBuild = false;
    // Update is called once per frame



    void SlowUpdate()
    {
        if (!isBuild)
        {
            if (Vector3.Distance(pl.position, transform.position) < 18f)
            {
                createBuild(myiniFilePath);
                Destroy(this);
                isBuild = true;

            }
        }
    }


    void createBuild(string ind)
    {
     

        ChankController CH =  transform.parent.GetComponent<ChankController>();
      

        for (int i = 0; i < mod.builderThemplates[ind].list.Count; i++)
        {
                builderThemElement _p=  mod.builderThemplates[ind].list[i];


                Vector3 newPos = transform.position + _p.pos + new Vector3(0,1f,0);

            

                BlockController b = Instantiate(CH.mapCon.blockBlank, transform.parent);


                b.transform.position = newPos;

                //                b.transform.name = b.transform.localPosition.x.ToString() + ":" + b.transform.localPosition.y.ToString() + ":" + b.transform.localPosition.z.ToString();
                b.transform.name = Global.Links.vectorToString(b.transform.position);


                b.itemInd = _p.ind;
                b.hp = 0;


            /*
            if (!mod.blockBase.ContainsKey(b.itemInd) )
            {
                print($"{b.itemInd} не найден! в {ind} (builder)");
                Destroy(gameObject);
                return;
            }

    */

                itemBlockSettings myBlockSetting = mod.blockBase[b.itemInd];
                b.myBlockSetting = myBlockSetting;
                b.mod = mod;


                b.initBlockPreload();

                b.GetComponent<MeshRenderer>().enabled = true;
                
           

            }

        Destroy(gameObject);

    }

        

    




    void createBuildOld(string iniFilePath)
    {
        IniFile MyIni = new IniFile(iniFilePath);



        int buildCount = MyIni.ReadInt("buildCount", "build", 0);

        for (int i = 0; i < buildCount; i++)
        {

            string line = MyIni.Read("b" + i.ToString(), "build", "not");
            if (line != "not")
            {
                // print("Ошибка чтения билда: " + i.ToString());
                //  return;


                string[] conf = line.Split(' ');


                string[] posBlock = conf[0].Split(':');

                Vector3 newPos = new Vector3(transform.position.x + System.Convert.ToInt32(posBlock[0]), 1 + transform.position.y + System.Convert.ToInt32(posBlock[1]), transform.position.z + System.Convert.ToInt32(posBlock[2]));


                BlockController b = Instantiate(transform.parent.GetComponent<ChankController>().mapCon.blockBlank, transform.parent);

                if (transform.parent.Find(Global.Links.vectorToString(newPos)) != null)
                {
                    Destroy(transform.parent.Find(Global.Links.vectorToString(newPos)).gameObject);
                }


                b.transform.position = newPos;

                //                b.transform.name = b.transform.localPosition.x.ToString() + ":" + b.transform.localPosition.y.ToString() + ":" + b.transform.localPosition.z.ToString();
                b.transform.name = Global.Links.vectorToString(b.transform.position);


                b.itemInd = conf[1];
                b.hp = 0;



                itemBlockSettings myBlockSetting = mod.blockBase[b.itemInd];
                b.myBlockSetting = myBlockSetting;
                b.initBlockPreload();
                b.GetComponent<MeshRenderer>().enabled = true;

                // b.initBlock();



            }


        }

        Destroy(gameObject);
    }
}
