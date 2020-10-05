using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sAutoDesrtoy : MonoBehaviour
{

    public float timerDelete = 15f;
    float timermy;
    // Start is called before the first frame update
    void Start()
    {

        timermy = timerDelete;
    }

    // Update is called once per frame
    void Update()
    {
        timermy -= Time.deltaTime;
        if (timermy <= 0f)
        {
            Destroy(gameObject);
        }


    }
}
