using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sUi : MonoBehaviour
{
    [Header("WINDOWS")]
    public inv winInvAction;
    public inv winInvOther;

 
    public inv winInvMain;
    public craftUi winCraft;

    public void winCloseAll()
    {
        for(int i=0; i < transform.childCount; i++)
        {
            bool closeThis = true;

            GameObject g  = transform.GetChild(i).gameObject;
            if (g.transform.name == "winDie") closeThis = false;
            if (g.transform.name == "inv") closeThis = false;
            if (g.transform.name == "winGame") closeThis = false;
            if (g.transform.name == "aim") closeThis = false;

            if (closeThis)
            {
               
                g.SetActive(false);
            }
            else
            { 
            }

        }
    }
    public void winOpen(string win)
    {

        //transform.Find(win).gameObject.SetActive(true);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.name == win)
            {
                transform.GetChild(i).transform.gameObject.SetActive(true);
                return;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal")!=0f)
            {
                winCloseAll();
                Global.Links.getPlayerAction().setCurLock(true);

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            winCloseAll();
            Global.Links.getPlayerAction().setCurLock(true);
        }
    }
}
