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


        string mobIndBase = "mob";
        if (Random.Range(1, 9) == 1)
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
            lastSpawn = 10f;
            if (sui.Day.dayTime < 8f)
            {
                lastSpawn = 8f;
            }
        }
        lastSpawn -= Time.deltaTime;


    }
}
