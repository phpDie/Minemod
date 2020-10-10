using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockTypeDrive : MonoBehaviour
{
    bool isDrive = false;

    PlayerAction pl;

    ChankController myChank;
    driverChank car;


    bool isBuild = false;
    public void build()
    {
        if (isBuild) return;
        isBuild = true;

        car = transform.parent.gameObject.AddComponent<driverChank>();
        car.init();
        car.myRule = this;
        
    }

    public void init()
    {
        print("Тачка создана!");

        pl = Global.Links.getPlayerAction();

        GameObject objToSpawn = new GameObject("driveChank");
        objToSpawn.transform.position = transform.position;

        objToSpawn.gameObject.layer = 2;


      myChank  = objToSpawn.AddComponent<ChankController>();

        myChank.transform.position = transform.position;

        transform.SetParent(myChank.transform);

        myChank.transform.tag = "driveChank";

        myChank.permGenOn = false;

        myChank.init();

        
     
        



       


    }

    public void setDrive(bool ndrive)
    {
        build();
        isDrive = ndrive;
        pl.GetComponent<Player>().canMove = !isDrive;
    }

    Quaternion startRot;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;

    }

    public float speed = 367f;
    float lookSpeed = 0.5f;

    float rotationXcam = 0;
    float rotationX = 0;
    float rotationY = 0;



    public float enSizer = 0.2f;
    // Update is called once per frame
    void Update()
    {
        if (!isDrive)
        {
            if (isBuild)
            {

                car.transform.position = Vector3.Lerp(car.transform.localPosition, new Vector3(Mathf.Round(car.transform.position.x), Mathf.Round(car.transform.position.y), Mathf.Round(car.transform.position.z)), Time.deltaTime * 1f);

                car.transform.rotation = Quaternion.Lerp(car.transform.rotation, startRot, Time.deltaTime * 48.6f);


            }

        }

        if (isDrive)
        {


            pl.GetComponent<Player>().canMove = false;
            pl.transform.position = transform.position + transform.up * 2.8f;
            pl.transform.rotation = transform.parent.transform.rotation;


            if (Input.GetKey(KeyCode.Space))
            {
                if (!car.giveEnergy(enSizer)) return;
                car.rb.AddForce(new Vector3(0, speed*1.2f * Time.deltaTime, 0f),ForceMode.Acceleration);

              //  transform.parent.position += new Vector3(0, speed * Time.deltaTime, 0f);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!car.giveEnergy(enSizer)) return;
                car.rb.AddForce(new Vector3(0, -speed * 0.4f * Time.deltaTime, 0f), ForceMode.Acceleration);
            }


            /*
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            transform.parent.rotation = Quaternion.Euler(0, 0, rotationX);
            */


            /*
              rotationY += Input.GetAxis("Mouse X") * lookSpeed;
            transform.parent.rotation = Quaternion.Euler(0, rotationY, 0);
            */

           
                rotationXcam += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationXcam = Mathf.Clamp(rotationXcam, -6f, 66f);
                pl.GetComponent<Player>().playerCamera.transform.localRotation = Quaternion.Euler(rotationXcam, 0, 0);


            if (Input.GetAxis("Mouse Y") != 0f)
            {
                if (!car.giveEnergy(enSizer)) return;
                car.rb.AddTorque(new Vector3(0f, Input.GetAxis("Mouse X") * lookSpeed / 16f, 0f), ForceMode.VelocityChange);
            }

            if (Input.GetAxis("Vertical") != 0f)
            {
                if (!car.giveEnergy(enSizer)) return;
                car.rb.AddForce(pl.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime, ForceMode.Acceleration);
            }


            if (Input.GetAxis("Vertical") != 0f)
            {
                if (!car.giveEnergy(enSizer)) return;
                car.rb.AddForce(pl.transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime, ForceMode.Acceleration);
            }
          

            if (Input.GetKey(KeyCode.Escape))
            {
                setDrive(false);
            }

        }
    }
}