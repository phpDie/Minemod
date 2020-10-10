using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class driverChank : MonoBehaviour
{
    public Rigidbody rb;
    public blockTypeDrive myRule;

    // Start is called before the first frame update
    void Start()
    {

    }

    

    public void init(string nameData = "not")
    {
      

         

        rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
       

        if (nameData == "not")
        {
            transform.name = "ship" + Random.Range(1000, 1000000);
        }
        if (nameData != "not")
        {
            transform.name = nameData;

        }
            GetComponent<ChankController>().pathChankFile = Global.Links.getMapController().mapPathDir + "" + transform.name + ".txt";

        
        if (nameData != "not")
        {
            GameObject objToSpawn = new GameObject("driveChank");
            objToSpawn.transform.SetParent(transform);

            GetComponent<ChankController>().isLoaded = false;
            GetComponent<ChankController>().loadAutoChank();
        }

        transform.tag = "driveChank";
        gameObject.layer = 2;

    }

    float en = 100f;
    float enMax = 90f;

    public bool giveEnergy(float needCount)
    {
        if (en >= needCount)
        {
            en -= needCount;
            return true;
        }
        return false;

    }


    public void giveEnergyFormInnerAuto()
    {
       
        float needCount = enMax - en;
      //  print($"RELOAD AUTO need: {needCount}");

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<blockTypeAgregat>() != null)
            {

                blockTypeAgregat _ba = transform.GetChild(i).GetComponent<blockTypeAgregat>();

                
                if (_ba.typeAgregat == agregatType.bat || _ba.typeAgregat == agregatType.gen)
                {

                     
                    if (_ba.wifiBatCount >0f)
                    {

                        float give = Mathf.Min(needCount, _ba.wifiBatCount);
                        _ba.wifiBatCount -= give;
                        en += give;


                    }


                }


            }

        }
    }

    public bool giveEnergyFormInner(float needCount)
    { 

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<blockTypeAgregat>()!=null)
            {

                blockTypeAgregat _ba = transform.GetChild(i).GetComponent<blockTypeAgregat>();

                if (_ba.typeAgregat == agregatType.bat || _ba.typeAgregat == agregatType.gen)
                {


                    if (_ba.wifiBatCount >= needCount)
                    {
                        _ba.wifiBatCount -= needCount;

                        return true;

                    }
                    else
                    {
                        print(_ba.wifiBatCount);
                    }


                }


            }

        }

        return false;
    }

    void Update()

    {

        if (en < enMax * 0.7f)
        {
            giveEnergyFormInnerAuto();

            /*
            if (giveEnergyFormInner(enMax * 0.7f))
            {
                print("RELOAD INNER BANK");
                en += enMax * 0.7f;
            }
            */

        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 2f)
        {
            if (collision.transform.tag == "block")
            {
                BlockController b = collision.transform.GetComponent<BlockController>();
                // print(rb.velocity.magnitude);

                b.Damage(rb.velocity.magnitude * 2f);
            }
        }
    }


    public void saveData()
    {
         
        transform.position = Global.Links.vectorRound(transform.position);

        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.name=Global.Links.vectorToString(transform.GetChild(i).transform.position);
        }

        string p = Global.Links.getMapController().mapPathDir + "/ships/" + transform.name + ".txt";
   

        if (!File.Exists(p)) File.Create(p).Close();

        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        string _data ="gavno "+ Global.Links.vectorToString(transform.position);

        
        File.WriteAllText(p, _data);


        GetComponent<ChankController>().mapCon= Global.Links.getMapController();
        GetComponent<ChankController>().chankSave();
    }
}