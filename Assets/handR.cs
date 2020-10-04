using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handR : MonoBehaviour
{

    public PlayerAction plAct;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void animAction_send()
    {
        plAct.animActionSend();
       // print("Fire");
    }
}
