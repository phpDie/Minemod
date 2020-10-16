using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class driverChank : MonoBehaviour
{
    public Rigidbody rb;
    public blockTypeDrive myRule;

    PlayerAction pl;
    public string myWifiType = "grav";

    //bool isLoaded;
    // Start is called before the first frame update
    void Start()
    {
        pl = Global.Links.getPlayerAction();
        startRot = transform.rotation;
    }



    public void init(string nameData = "not")
    {




        rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        rb.angularDrag = 4f;
        rb.useGravity = false;


        if (nameData == "not")
        {
            transform.name = "ship" + Random.Range(1000, 1000000);
        }
        if (nameData != "not")
        {
            transform.name = nameData;
        }
        GetComponent<ChankController>().pathChankFile = Global.Links.getMapController().mapPathDir + "" + transform.name + ".txt";


        if (nameData != "not")
        {
            GameObject objToSpawn = new GameObject("driveChank");
            objToSpawn.transform.SetParent(transform);

            GetComponent<ChankController>().isLoaded = false;
            GetComponent<ChankController>().loadAutoChank();
            isLoaded = true;
        }

        transform.tag = "driveChank";
        gameObject.layer = 2;

        isLoaded = true;

    }

    float en = 0f;
    float enMax = 90f;

    public bool giveEnergy(float needCount)
    {
        if (en >= needCount)
        {
            en -= needCount;
            return true;
        }
        return false;

    }


    public void giveEnergyFormInnerAuto()
    {

        float needCount = enMax - en;
        //  print($"RELOAD AUTO need: {needCount}");

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<blockTypeAgregat>() != null)
            {

                blockTypeAgregat _ba = transform.GetChild(i).GetComponent<blockTypeAgregat>();




                if (_ba.typeAgregat == agregatType.bat || _ba.typeAgregat == agregatType.gen)
                {

                    if (_ba.wifiBatType == myWifiType)
                    {

                        if (_ba.wifiBatCount > 0f)
                        {

                            float give = Mathf.Min(needCount, _ba.wifiBatCount);
                            _ba.wifiBatCount -= give;
                            en += give;


                        }
                    }

                }


            }

        }
    }
    /*
    public bool giveEnergyFormInner(float needCount)
    { 

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<blockTypeAgregat>()!=null)
            {

                blockTypeAgregat _ba = transform.GetChild(i).GetComponent<blockTypeAgregat>();

                if (_ba.typeAgregat == agregatType.bat || _ba.typeAgregat == agregatType.gen)
                {


                    if (_ba.wifiBatCount >= needCount)
                    {
                        _ba.wifiBatCount -= needCount;

                        return true;

                    }
                    else
                    {
                        print(_ba.wifiBatCount);
                    }


                }


            }

        }

        return false;
    }
    */

    public bool isDrive = false;

    Quaternion startRot;

    public float enSizer = 0.2f;

    float lookSpeed = 0.9f;

    float rotationXcam = 0;
    float rotationX = 0;
    float rotationY = 0;


    public float speed = 467f;
    void Update()

    {

        if (en < enMax * 0.7f)
        {
            giveEnergyFormInnerAuto();

            /*
            if (giveEnergyFormInner(enMax * 0.7f))
            {
                print("RELOAD INNER BANK");
                en += enMax * 0.7f;
            }
            */

        }

        if (!isLoaded) return;


        if (!isDrive)
        {

            Vector3 posRound = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

            if (Vector3.Distance(posRound, transform.position) < 0.1f)
            {
                transform.position = posRound;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, posRound, Time.deltaTime * 1.4f);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, startRot, Time.deltaTime * 48.6f);




        }

        if (isDrive)
        {
            if (myRule == null)
            {
                isDrive = false;
                return;
            }

            pl.GetComponent<Player>().canMove = false;
            pl.transform.position = myRule.transform.position + transform.up * 1.6f;
            // pl.transform.rotation = myRule.transform.parent.transform.rotation;





            /*
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            transform.parent.rotation = Quaternion.Euler(0, 0, rotationX);
            */


            /*
              rotationY += Input.GetAxis("Mouse X") * lookSpeed;
            transform.parent.rotation = Quaternion.Euler(0, rotationY, 0);
            */


            if (Input.GetButton("Fire3") || modeFreeView)
            {
                rotationXcam += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationXcam = Mathf.Clamp(rotationXcam, -86f, 86f);
                pl.GetComponent<Player>().playerCamera.transform.localRotation = Quaternion.Euler(rotationXcam, 0, 0);


                pl.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed , 0);

                //pl.transform.rotation = myRule.transform.parent.transform.rotation;
            }
            else
            {

                pl.transform.rotation = myRule.transform.parent.transform.rotation;
            }

            if (Input.GetButton("Fire2"))
            {
                if (Input.GetAxis("Mouse X") != 0f)
                {
                    if (!giveEnergy(enSizer)) return;
                    //car.rb.AddTorque(new Vector3(0f, Input.GetAxis("Mouse X") * lookSpeed / 16f, 0f), ForceMode.VelocityChange);
                    rb.AddTorque(transform.up * Input.GetAxis("Mouse X") * lookSpeed / 9f, ForceMode.VelocityChange);
                }

            }


            if (Input.GetKeyDown(KeyCode.Q))
            {
                modeFreeView = !modeFreeView;
            }
        }

    }

    bool modeFreeView = true;

    float prevSpeed;
    private void FixedUpdate()
    {
        if (!isDrive) return;

        prevSpeed = rb.velocity.magnitude;



        /*

        if (Input.GetAxis("Mouse Y") != 0f)
        {
            if (!car.giveEnergy(enSizer)) return;
          //  car.rb.AddTorque(new Vector3(Input.GetAxis("Mouse Y") * lookSpeed / 16f, 0f, 0f), ForceMode.VelocityChange);
            car.rb.AddTorque(car.transform.right * Input.GetAxis("Mouse Y") * lookSpeed / 16f, ForceMode.VelocityChange);
        }

*/



        if (Input.GetAxis("Vertical") != 0f)
        {
            if (!giveEnergy(enSizer)) return;
            rb.AddForce(pl.transform.forward * Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }


        if (Input.GetAxis("Horizontal") != 0f)
        {
            if (!giveEnergy(enSizer)) return;
            rb.AddForce(pl.transform.right * Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        if (Input.GetAxis("Vertical") == 0f && Input.GetAxis("Horizontal") == 0f)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0f, rb.velocity.y, 0f), lerpSpeed * Time.fixedDeltaTime);
            
        }

        if (Input.GetKey(KeyCode.F))
        {
            myRule.setDrive(false);
        }



        if (Input.GetKey(KeyCode.Space))
        {
            if (!giveEnergy(enSizer)) return;
            rb.AddForce(new Vector3(0, speed * 0.9f * Time.fixedDeltaTime, 0f), ForceMode.Acceleration);

            //  transform.parent.position += new Vector3(0, speed * Time.deltaTime, 0f);
        }else         if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!giveEnergy(enSizer)) return;
            rb.AddForce(new Vector3(0, -speed * 0.9f * Time.fixedDeltaTime, 0f), ForceMode.Acceleration);
        }
        else
        {
            rb.velocity =Vector3.Lerp(rb.velocity ,  new Vector3(rb.velocity.x,0f, rb.velocity.z), lerpSpeed * Time.fixedDeltaTime);

        }

    }
    public float lerpSpeed = 7f;

    bool isLoaded = false;

    private void OnTransformChildrenChanged()
    {
        checkMyStatus();
    }

    public void checkMyStatus()
    {
        if (!isLoaded) return;
        if (transform.childCount < 2)
        {
            print("Destroy me");
            removeData();
        }

    }

  
  
    private void OnCollisionEnter(Collision collision)
    {

        float dam = prevSpeed;

        if (rb.velocity.magnitude > dam) dam = rb.velocity.magnitude;

        if (dam > 6f)
        {
            if (collision.transform.tag == "block")
            {
                print(dam);
                BlockController b = collision.transform.GetComponent<BlockController>();
                // print(rb.velocity.magnitude);
                
                b.Damage(dam * 2f);


                if (collision.contacts[0].thisCollider.transform.tag == "block")
                {
                    if (collision.contacts[0].thisCollider.transform.parent == transform)
                    {
                        BlockController bm = collision.contacts[0].thisCollider.transform.GetComponent<BlockController>();
                        // print(rb.velocity.magnitude);

                        bm.Damage(dam * 1f);
                        checkMyStatus();

                    }
                }
            }
        }
    }


    public void removeData()
    {

        string p = Global.Links.getMapController().mapPathDir + "/ships/" + transform.name + ".txt";
        if (File.Exists(p)) File.Delete(p);

        p = GetComponent<ChankController>().pathChankFile;
        if (File.Exists(p)) File.Delete(p);

        Destroy(gameObject);
    }

    public void saveData()
    {

        transform.position = Global.Links.vectorRound(transform.position);

        for (int i = 1; i < transform.childCount; i++)
        {

            transform.GetChild(i).transform.name = Global.Links.vectorToString(Global.Links.vectorRound(transform.GetChild(i).transform.position));
        }

        string p = Global.Links.getMapController().mapPathDir + "/ships/" + transform.name + ".txt";


        if (!File.Exists(p)) File.Create(p).Close();

        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        string _data = "gavno " + Global.Links.vectorToString(transform.position);


        File.WriteAllText(p, _data);


        GetComponent<ChankController>().mapCon = Global.Links.getMapController();
        GetComponent<ChankController>().chankSave();
    }


}