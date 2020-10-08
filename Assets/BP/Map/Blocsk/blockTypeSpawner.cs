using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockTypeSpawner : MonoBehaviour
{
    public float MytimeSpawn = 10f; //колв секунд между спавнами

    public GameObject mobBlank;


    string mobIndBase = "mob";

    GameObject player;
    GameObject myMob;
    public void init()
    {
         
        //  mobBlank = Resources.Load("Resources/Mob/mob") as GameObject;

        if (Random.Range(1, 9) == 1)
        {
            mobIndBase = "mob People";
        }
        

        mobBlank = Resources.Load<GameObject>("Mob/"+ mobIndBase);

        if (mobBlank == null)
        {
            print("ERROR MOB LOADER");
        }

    }
    // Start is called before the first frame update
    void Start()
    {

        player = Global.Links.getPlayerAction().gameObject;
    }

     

    float timer = 3.5f;
    void slowUpdate()
    {
        if (myMob == null)
        {
            timeSpawn -= 1f;
            if (timeSpawn > 0f) return;
            timeSpawn = MytimeSpawn;
        }

        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);



        if (distToPlayer <= 31f)
        {

            if (myMob == null)
            {
              //  print("SPAWN MOB");
                myMob = Instantiate(mobBlank);
                myMob.transform.position = transform.position + Vector3.up * 2f;
            }
        }
        

    }

    private float timeSpawn = 1f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            slowUpdate();
            timer = 1f;
        }
    }
}
