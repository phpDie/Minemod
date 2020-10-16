    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockTypeDoor : MonoBehaviour
{


    GameObject turel;

    bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {

  
    }

    public void interact()
    {
        print("Open doors");

        isOpen = !isOpen;

        if (isOpen)
        {
            turel.transform.Find("doorModel").Rotate(Vector3.up, -90f);
        }
        else
        {
            turel.transform.Find("doorModel").Rotate(Vector3.up, 90f);

        }
        //print(turel.transform.Find("doorModel").name);
    }


    public void init()
    {
        GameObject sitDriveBlank = Resources.Load<GameObject>("Door");
        turel = Instantiate(sitDriveBlank, transform);
        turel.transform.localPosition = new Vector3(0f,-1f,0f);
        turel.transform.localPosition = new Vector3(0f,0f,0f);
        
        print("DOOR CREATE");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
