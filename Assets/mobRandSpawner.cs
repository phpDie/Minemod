using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobRandSpawner : MonoBehaviour
{

    sUi sui;
    // Start is called before the first frame update
    void Start()
    {
        sui = Global.Links.getSui();


    }

    public void spawn()
    {
        Vector3 pos = transform.position+ new Vector3(Random.Range(-15f, 15f), 37f, Random.Range(-15f, 15f));
        pos = transform.position;
        pos.y = 37f;


        if(Mathf.Abs(pos.x) < 4f && Mathf.Abs(pos.z) < 4f)
        {
            pos+= new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(-15f, 15f));
        }


            string mobIndBase = "mob";

        if (Random.Range(1, 4) == 1)
        {
            mobIndBase = "mob People";
        }
        GameObject mobBlank = Resources.Load<GameObject>("Mob/" + mobIndBase);

        if (mobBlank == null)
        {
            print("ERROR MOB LOADER");
            return;
        }
        Instantiate(mobBlank).transform.position = pos;
    }


    float lastSpawn = 0f;
    void Update()
    {
        if (lastSpawn <= 0f)
        {
            spawn();
            lastSpawn = 200f;

            if (sui.Day.dayTime < 8f || sui.Day.dayTime>21f)
            {
                lastSpawn = 26f;
            }
        }
        lastSpawn -= Time.deltaTime;


    }
}
