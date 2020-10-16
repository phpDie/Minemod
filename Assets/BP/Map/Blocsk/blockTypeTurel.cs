using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum turelType
{
    not=0,
    gun=1,
    bur=2,

}

public class blockTypeTurel : MonoBehaviour
{

    GameObject target;

    GameObject turel;

    public void init()
    {

    }



    void Start()
    {
        GameObject sitDriveBlank = Resources.Load<GameObject>("Turel");
        turel = Instantiate(sitDriveBlank, transform);
        turel.transform.localPosition = new Vector3();

    }


    void Update()
    {
        if (target != null)
        {
            turel.transform.LookAt(target.transform);
        }
        else
        {
            target = Global.Links.getIndDataPlayerAction().gameObject;
        }
    }
}
