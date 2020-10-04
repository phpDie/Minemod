using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using MyProg;
using System;

public class PlayerAction : MonoBehaviour
{

    [HideInInspector]
    public ModLoader modLoader;
    public inv myInv;



    public handR hand;
    public GameObject handSprite;
    public GameObject handCube;

    [HideInInspector]
    public Camera mCam;
    public blockMaterial toolMaterialTarget; //в руке инструмент нацеленый на этот материал

    public float toolDamage = 1f;
    public float toolDist = 15f;


    void Start()
    {

        mCam = GetComponent<Player>().playerCamera;

       

        modLoader = GameObject.Find("Main").GetComponent<ModLoader>();




        myInv = Global.Links.getPlayerInv();

        setCurLock(true);
    }

    public void animActionSend()
    {
        hand.GetComponent<Animator>().SetBool("attack", false);

        if (myInv.activeElement.infoItemSave.type == itemType.handTool)
        {
            ActionItem_CrackBlock(); 
        }


        if (myInv.activeElement.infoItemSave.type == itemType.gun)
        {
            ActionItem_Fire(); 
        }

    }

    public void setHandItem(itemElement it)
    {

        setSpriteHand(it.infoItemSave.iconInHand, it.infoItemSave.type == itemType.block);

        IniFile MyIni = new IniFile(it.infoItemSave.iniFilePath);
        if(it.infoItemSave.type == itemType.handTool)
        {
           /*
            toolDist = MyIni.ReadFloat("distance", "action", 1);
            print(toolDist);
            */
            toolDamage= MyIni.ReadInt("damage", "action", 1);
            toolMaterialTarget = (blockMaterial)Enum.Parse(typeof(blockMaterial), MyIni.Read("materialTarget", "action", "ground"));
        }


    }


    public void setSpriteHand(Texture2D newTexture, bool isCube = false)
    {



        handSprite.SetActive(!isCube);
        handCube.SetActive(isCube);
        if (isCube)
        {
            handCube.GetComponent<MeshRenderer>().material.mainTexture = newTexture;

            
        }

        if (!isCube)
        {
            SpriteRenderer sr = handSprite.GetComponent<SpriteRenderer>();


            Texture2D blurredTexture = newTexture;
            float w = blurredTexture.width;
            float h = blurredTexture.height;


            sr.sprite = Sprite.Create(blurredTexture, new Rect(0, 0, w, h), new Vector2(0.5f, 0f));

        }
    }

    public float timeAttack = 0f;
    public float timeAttackMax = 0.5f;

    public bool btnAttackDown = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            myInv.selectNewActiveInde(myInv.actveIndex + 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            myInv.selectNewActiveInde(myInv.actveIndex - 1);
        }

        if (timeAttack <= 0f)
        {

        }
        else { 
            timeAttack -= Time.deltaTime;
        }

        if (!Cursor.visible)
        {

            if (Input.GetButton("Fire1"))
            {
                if (timeAttack<=0f)
                {
                    if (myInv.activeElement.infoItemSave.type == itemType.handTool)
                    {
                       // ActionItem_CrackBlock();

                        hand.GetComponent<Animator>().SetBool("attack", true);
                        timeAttack = timeAttackMax;
                    }


                    if (myInv.activeElement.infoItemSave.type == itemType.gun)
                    {
                       // ActionItem_Fire();
                        hand.GetComponent<Animator>().SetBool("attack", true);
                        timeAttack = timeAttackMax;
                    }
                }

            }

            if (Input.GetButtonDown("Fire2"))
            {
                if (myInv.activeElement.infoItemSave.type == itemType.block)
                {
                    ActionItem_Build();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ActionItem_Use();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            setCurLock(Cursor.visible);
        }
    

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (myInv.activeElement.infoItemSave.type == itemType.gun)
            { 
                int needCount = myInv.activeElement.infoItemSave.stackSize;

                needCount = needCount - myInv.activeElement.count;

                if (needCount > 0)
                {
                    
                    if (myInv.myData.giveFromInd("Weapon:arrow", needCount))
                    { 
                        myInv.activeElement.count += needCount;
                        myInv.ReRender();
                    }
                }


            }
        }

    }


    GameObject GetBlockFromRayCamera()
    {
        Vector3 rayOrigin = mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Declare a raycast hit to store information about what our raycast has hit
        RaycastHit hit;

        // Set the start position for our visual effect for our laser to the position of gunEnd


        // Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin, mCam.transform.forward, out hit, 15f))
        {
            //  print(hit.transform.name);
            return hit.transform.gameObject;

        }
        return null;
    }


    void ActionItem_Fire()
    {
        if (!myInv.giveActive(2, false)) return;

        print("Fire");

        myInv.giveActive(1);
    }


        void ActionItem_CrackBlock()
    {


        GameObject go = GetBlockFromRayCamera();
        if (go == null)
        {
            return;
        }



        if (go.transform.tag == "block")
        {
            //print("Fire block");
            //Destroy(go); 
            go.GetComponent<BlockController>().Damage(toolDamage, toolMaterialTarget,gameObject);
        }
    }

    void ActionItem_Use()
    {

        GameObject go = GetBlockFromRayCamera();
        if (go == null)
        {
            return;
        }
        if (go.transform.tag == "block")
        {
            BlockController b = go.GetComponent<BlockController>();
         

            if (b.myType == blockType.cargo)

            {
                Global.Links.getOtherInv().gameObject.SetActive(true);

                Global.Links.getOtherInv().openCargo(b.GetComponent<invData>());

                setCurLock(false);
                return;
                // print("Open cargo");
            }

            if (b.myType == blockType.door)
            {
                print("Open door");
            }

            //info

            print("\n ---- -----");
            print(b.itemInd);
            print(b.myMaterial);
            print("HP:"+b.hp);

            return;
        }
            

    }
    void ActionItem_Build()
    {
        if (!myInv.giveActive(1, false)) return;

        /*
        GameObject go = GetBlockFromRayCamera();
        if (go == null)
        {
            return;
        }


        if (go.transform.tag != "block")
        {
            return;
        }
        */

        BlockController blankBlock = Global.Links.getMapController().blockBlank;


        Vector3 pos = new Vector3();



        pos = GetComponent<buildController>().rayTickGetPost();
        Transform newParent = GetComponent<buildController>().lastChank;


        BlockController newBlock = Instantiate(blankBlock, newParent);

        


        newBlock.transform.position = pos;
        /*
        if (Physics.OverlapBox(pos + new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f) * 0.2f, Quaternion.identity).Length <=0)
        {

            newBlock.transform.position = pos;
        }
        else

        { 
            newBlock.transform.localPosition = go.transform.localPosition;
            newBlock.transform.localPosition = newBlock.transform.localPosition + new Vector3(0, 1, 0);
            
        }
        */
        newBlock.transform.name = newBlock.transform.localPosition.x.ToString() + ":" + newBlock.transform.localPosition.y.ToString() + ":" + newBlock.transform.localPosition.z.ToString();

        newBlock.itemInd = myInv.activeElement.ind;
        newBlock.setToItemInd();

        //newBlock.GetComponent<MeshRenderer>().material.mainTexture = myInv.activeElement.infoItemSave.icon;


           myInv.giveActive(1);


        //newBlock.transform.localPosition = newBlock.transform.localPosition + new Vector3(0, 1, 0);

    }

    public void setCurLock(bool newLock)
    {
        if (newLock)
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        Cursor.visible = !newLock;

        if (newLock)
        {
            Global.Links.getSui().invOther.closeCargo();
            //Global.Links.getSui().invOther.gameObject.SetActive(false);
        }
    }
}