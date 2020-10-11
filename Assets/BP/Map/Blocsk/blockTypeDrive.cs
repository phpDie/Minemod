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

        if (transform.parent.gameObject.GetComponent<driverChank>() != null) return;
        print("Тачка забилжена!");

        car = transform.parent.gameObject.AddComponent<driverChank>();
        car.init();
        car.myRule = this;
        
    }

  

    private void OnDestroy()
    {
        if (isDrive)
        {
            setDrive(false);
        }
    }

    public void init()
    {

       // GetComponent<BlockController>().myMaterial = blockMaterial.badrock;

        pl = Global.Links.getPlayerAction();

        if (transform.parent.gameObject.GetComponent<driverChank>() == null)
        {
            print("Тачка создана!");

            GameObject objToSpawn = new GameObject("driveChank");
            objToSpawn.transform.position = transform.position;

           // objToSpawn.gameObject.layer = 2;


            GameObject firest = new GameObject("first");
            firest.transform.SetParent(objToSpawn.transform);




            myChank = objToSpawn.AddComponent<ChankController>();

            myChank.transform.position = transform.position;

            transform.SetParent(myChank.transform);

        

            myChank.permGenOn = false;

            myChank.init();

        }
        else
        {
            print("Тачка загружена!");
            myChank = transform.parent.GetComponent<ChankController>();
            car = transform.parent.GetComponent<driverChank>();
            car.myRule = this;
        }
        
     
    }

    public void setDrive(bool ndrive)
    {
        print("DRIVE");
        build();
        isDrive = ndrive;
        pl.GetComponent<Player>().canMove = !isDrive;

        car.rb.isKinematic = !ndrive;
        car.isDrive = isDrive;

        if (ndrive)
        {
            car.myRule = this;
        }
         
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

       
        /*
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


         

            if (!Input.GetButton("Fire2"))
            {
                rotationXcam += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationXcam = Mathf.Clamp(rotationXcam, -86f, 86f);
                pl.GetComponent<Player>().playerCamera.transform.localRotation = Quaternion.Euler(rotationXcam, 0, 0);


                pl.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

            }
            else
            {

                if (Input.GetAxis("Mouse X") != 0f)
                {
                    if (!car.giveEnergy(enSizer)) return;
                    //car.rb.AddTorque(new Vector3(0f, Input.GetAxis("Mouse X") * lookSpeed / 16f, 0f), ForceMode.VelocityChange);
                   car.rb.AddTorque(car.transform.up * Input.GetAxis("Mouse X") * lookSpeed / 16f, ForceMode.VelocityChange);
                }
 

            }

            if (Input.GetAxis("Vertical") != 0f)
            {
                if (!car.giveEnergy(enSizer)) return;
                car.rb.AddForce(pl.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime, ForceMode.Acceleration);
            }


            if (Input.GetAxis("Horizontal") != 0f)
            {
                if (!car.giveEnergy(enSizer)) return;
                car.rb.AddForce(pl.transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime, ForceMode.Acceleration);
            }
          

            if (Input.GetKey(KeyCode.Escape))
            {
                setDrive(false);
            }
            }
            */

        
    }
}