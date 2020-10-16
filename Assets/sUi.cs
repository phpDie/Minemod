using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sUi : MonoBehaviour
{
    [Header("TextElements")]
    public Text txtDayTime;


    [Header("WINDOWS")]
    public inv winInvAction;
    public inv winInvOther;
    public agregatUi winAgregat;


    public inv winInvMain;
    public craftUi winCraft;
     
    public Transform flexDiv;

    [HideInInspector]
    public day Day;

    public packSound myPackSound;


    public sLoot lootCreate( Vector3 pos, string ind = "Core:dirt", int count = 1)
    {


        GameObject newLootGo = Instantiate(Resources.Load<GameObject>("loot"));

        sLoot newLoot = newLootGo.GetComponent<sLoot>();
        newLoot.init(ind, count);
        newLootGo.transform.position = pos;
        return newLoot; 

    }


    public void winCloseAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool closeThis = true;

            GameObject g = transform.GetChild(i).gameObject;
            if (g.transform.name == "winDie") closeThis = false;
            if (g.transform.name == "inv") closeThis = false;
            if (g.transform.name == "winGame") closeThis = false;
            if (g.transform.name == "aim") closeThis = false;
            if (g.transform.name == "flexDiv") closeThis = false;

            if (closeThis)
            {

                g.SetActive(false);
            }
            else
            {
            }

        }


        for (int i = 0; i < flexDiv.transform.childCount; i++)
        {
            flexDiv.transform.GetChild(i).transform.gameObject.SetActive(false);
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

        for (int i = 0; i < flexDiv.transform.childCount; i++)
        {
            if (flexDiv.transform.GetChild(i).transform.name == win)
            {

                flexDiv.transform.GetChild(i).transform.gameObject.SetActive(true);
                return;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {

        Day = GetComponent<day>();
        myPackSound = GetComponent<packSound>();
    }

    void slowUpd()
    {
        
        string _time = Mathf.Floor(Day.dayTime).ToString() + ":" + Mathf.Floor((60f / 1f * (Day.dayTime - Mathf.Floor(Day.dayTime)))*1f ).ToString();
//        print(_time );
        txtDayTime.text = _time;
    }

    float timeSlow = 0f;
    void Update()
    {
        timeSlow -= Time.deltaTime;
        if (timeSlow <= 0f)
        {
            slowUpd();
            timeSlow = 1f;
        }



        if (Cursor.lockState == CursorLockMode.Confined)
        {
            if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
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