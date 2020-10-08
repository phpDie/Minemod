using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatPart : MonoBehaviour
{

    public botBody myBot;


    public float cofDamage = 1f;
    public float hp = 1f; 



    void Start()
    {

        if (myBot == null)
        {
            if (transform.parent.parent.GetComponent<botBody>())
            {
                myBot = transform.parent.parent.GetComponent<botBody>();
            }
        }

    }



    public void Damage(float count,GameObject auth =null)
    {
        if (auth == myBot.gameObject) return ;

        hp -= count;
        if (hp > 0f) return;



        
        if (myBot == null) {
            Destroy(gameObject); 
            return;
        }

         
        myBot.Damage(count * cofDamage);
        Destroy(gameObject);
    }
}
