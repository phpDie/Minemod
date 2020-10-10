using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class driverChank : MonoBehaviour
{
    public Rigidbody rb;
    public blockTypeDrive myRule;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void init()
    {
        rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = false;

   
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    float en = 100f;
    float enMax= 100f;

    public bool giveEnergy(float needCount)
    {
        if (en >= needCount)
        {
            en -= needCount;
            return true;
        }
        return false;

    }


    public bool giveEnergyFormInner(float needCount)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<blockTypeAgregat>())
            {

                blockTypeAgregat _ba = transform.GetChild(i).GetComponent<blockTypeAgregat>();

                if (_ba.typeAgregat == agregatType.bat || _ba.typeAgregat == agregatType.gen)
                {


                    if (_ba.wifiBatCount >= needCount)
                    {
                        _ba.wifiBatCount -= needCount;
                        return true;

                    }


                }


            }

        }

        return false;
    }

    void Update()

    {

        if (en < enMax * 0.2f)
        {

            if(giveEnergyFormInner(enMax * 0.8f))
            {
                print("RELOAD INNER BANK");
                en += enMax * 0.8f;
            }

        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 2f)
        {
            if (collision.transform.tag == "block")
            {
                BlockController b = collision.transform.GetComponent<BlockController>();
               // print(rb.velocity.magnitude);

                b.Damage(rb.velocity.magnitude * 2f);
            }
        }
    }
}