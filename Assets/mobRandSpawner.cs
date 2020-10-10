using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobRandSpawner : MonoBehaviour
{
    public float dayDelaySpawn = 150f;
    public float nightDelaySpawn = 27f;
    public float dist = 15f;

    public bool isHardTest = false;

    sUi sui;
     
    // Start is called before the first frame update
    void Start()
    {
        sui = Global.Links.getSui();

        if (isHardTest)
        {
            dayDelaySpawn = 8f;
            nightDelaySpawn = 4f;
            dist = 12f;
        }
    }

    public void spawn()
    {
      //  print("SPAWN");

        Vector3 pos = transform.position+ new Vector3(Random.Range(-dist, dist), 37f, Random.Range(-dist, dist));
        pos = transform.position;
        pos.y = 37f;


        if(Mathf.Abs(pos.x) < 3f && Mathf.Abs(pos.z) < 3f)
        {
            pos+= new Vector3(Random.Range(-dist, dist), 0f, Random.Range(-dist, dist));
        }


            string mobIndBase = "mob";

        if (Random.Range(1, 3) == 1)
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
            lastSpawn = dayDelaySpawn;

            if (sui.Day.dayTime < 8f || sui.Day.dayTime>21f)
            {
                lastSpawn = nightDelaySpawn;
            }
        }
        lastSpawn -= Time.deltaTime;


    }
}
