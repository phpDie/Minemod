using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genWeapon : MonoBehaviour
{

    public int set_duloLen = 30;

    public int set_ruko1pos = 5;

    public int set_ruko1Len = 16;

    public int set_ruko2pos = 2;
    public int set_ruko2Len = 16;

    public int set_ruko1rot = 90;
    public int set_ruko2rot = 90;


    public int set_width = 100;


    public bool randInStart = true;
    public bool regeneratorOn = true;


    private void Start()
    {
        if (randInStart)
        {
            randomMe();
            buildSetting();
        }
    }


    private void Update()
    {
        if (regeneratorOn)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (Input.GetKey(KeyCode.T))
                {
                    randomMe();

                    print(designExport());
                    string _randDes = designExport();
                    designImport(_randDes);

                    buildSetting();

                }
            }
        }
    }


    public void designImport(string instring)
    {
        string[] lines = instring.Split(':');

        if (lines.Length < 3)
        {
            print("ERro weapon design!");
            return;
        }

        set_duloLen = System.Convert.ToInt32(lines[0]);
        set_ruko1Len = System.Convert.ToInt32(lines[1]);
        set_ruko2Len = System.Convert.ToInt32(lines[2]);
        set_ruko1rot = System.Convert.ToInt32(lines[3]);
        set_ruko2rot = System.Convert.ToInt32(lines[4]);
        set_ruko2pos = System.Convert.ToInt32(lines[5]);
        set_width = System.Convert.ToInt32(lines[6]);

       // buildSetting();
    }



    public string designExport()
    {
        string Out = "";
        Out +=  set_duloLen.ToString();
        Out += ":" + set_ruko1Len.ToString();
        Out += ":" + set_ruko2Len.ToString();
        Out += ":" + set_ruko1rot.ToString();
        Out += ":" + set_ruko2rot.ToString();
        Out += ":" + set_ruko2pos.ToString();
        Out += ":" + set_width.ToString();

        return Out;
    }


    public void randomMe()
    {
        set_duloLen = Random.Range(18, 70);

        set_ruko1Len = Random.Range(9, 20);
        set_ruko2Len = Random.Range(9, 25);


        set_ruko1rot = Random.Range(80,130);
        set_ruko2rot = Random.Range(60,100);


        set_ruko2pos = Random.Range(10, set_duloLen);


        set_width = Random.Range(50, 120);

    }


    public void buildSetting()
    {
        transform.Find("dulo").transform.localScale = new Vector3(1, 1, set_duloLen/100f);


        Vector3 pos = new Vector3();


        set_ruko2pos = Mathf.Min(set_ruko2pos, set_duloLen);

        pos = transform.Find("aim2").transform.localPosition;
        pos.z = set_duloLen / 100f;
        transform.Find("aim2").transform.localPosition = pos;



        pos = transform.localScale;
        pos.x = set_width / 100f;
        transform.localScale= pos;




        pos = transform.Find("hand2").transform.localPosition;
        pos.z = set_ruko2pos / 100f;
        transform.Find("hand2").transform.localPosition = pos;

         
//        transform.Find("hand2").transform.localRotation = new Quaternion(set_ruko2rot-90 ,0f,0f, transform.Find("hand1").transform.localRotation.w);
        transform.Find("hand2").transform.localRotation =  Quaternion.Euler(set_ruko2rot ,0f,0f);
        transform.Find("hand1").transform.localRotation =  Quaternion.Euler(set_ruko1rot, 0f,0f);


       transform.Find("hand1").transform.localScale = new Vector3(0.9f, 1, set_ruko1Len / 100f);
        transform.Find("hand2").transform.localScale = new Vector3(0.85f, 1, set_ruko2Len / 100f);



    }
}
