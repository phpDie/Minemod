using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adminBuilderBlock : MonoBehaviour
{

    public BlockController b;


    void Start()
    {
       
         
    }


    void Update()
    {

    }


    public void createAdminBuildExportData(bool isClear = false)
    {

       // print("MYIND:" + transform.name);

        

        string Out = "";

        int w =11;
        int h = 16;

        int i = -1;

        for (int ix = -w; ix <= w; ix++)
        {
            for (int iz = -w; iz <= w; iz++)
            {
                for (int iy = 0; iy <= h; iy++)
                {


                    
                    string searchBlock = Global.Links.vectorToString(transform.localPosition  + new Vector3(ix, iy + 1, iz));
                    string nameInd = Global.Links.vectorToString(new Vector3(ix, iy, iz));


                    if (transform.parent.Find(searchBlock) != null)
                    {
                        GameObject res = transform.parent.Find(searchBlock).gameObject;

                        
                        i++;
                        Out += "\n" + "b" + i.ToString() + "=" + nameInd + " " + res.GetComponent<BlockController>().itemInd + " 0";

                        if (isClear)
                        {
                            if (res != gameObject)
                            {
                              //  print("CELAR ++");

                                Destroy(res);
                            }
                        }
                    }
                }
            }
        }

       // if (isClear) return;

      //  print("--------- EXPORT (" + i.ToString() + " blocks) --------- ");
        Out = "buildCount = "+(i+1).ToString()  + Out;
        print(Out);

    }



    public void createAdminBuildBlockInit()
    {
        print("АДМИНСКИЙ БЛОК СОЗДАН. Нажмите на его E что бы экспортировать данные");

        b = GetComponent<BlockController>();

        createAdminBuildExportData(true);
    }
}
