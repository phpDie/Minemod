using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class day : MonoBehaviour
{

    public Light dirLight;
    public Color cDay;
    public Color cNight;

    // Start is called before the first frame update
    void Start()
    {
        
    }


  public  float dayTime = 0f;
    void Update()
    {

        dayTime += 0.003f;

        if (dayTime <= 8f || dayTime > 19f)
        {
            dirLight.intensity = Mathf.SmoothStep(dirLight.intensity, 0.86f, 0.009f); ;
            dirLight.color = Color.Lerp(dirLight.color, cNight, 0.003f);
        }
        else
        {
            dirLight.intensity = Mathf.SmoothStep(dirLight.intensity, 1.3f, 0.009f); ;
            dirLight.color = Color.Lerp(dirLight.color, cDay, 0.003f);
        }
        if (dayTime > 24f) dayTime = 0f;



    }
}
