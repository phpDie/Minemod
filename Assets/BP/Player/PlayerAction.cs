using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using MyProg;
using System;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
     

    public float hp = 100f;
    public float hpMax = 100f;

    public float eat = 100f;

    public AudioSource myAudio;

    [HideInInspector]
    public ModLoader modLoader;
    public inv myInv;


    [Header("WeaponCurrent")]
    public genWeapon myWeapon;


    [Header("Ссылки на руки")]
    public handR hand;
    public GameObject handWeapon;
    public GameObject centerCamHand;


    [Header("Кубы и tools")]
    public GameObject handSprite;
    public GameObject handCube;

    [Header("Blank Prefabs")]
    public GameObject pref_bulletHole;

    [HideInInspector]
    public string ammoReloadInd; //патры нужные для перезарядки
    public Camera mCam;
    public blockMaterial toolMaterialTarget; //в руке инструмент нацеленый на этот материал
    sUi sui;
    packSound myPackSound;
    public float toolDamage = 1f;
    public float toolDist = 15f;


    public float camFiledView = 65;

    public int giveUse = 0; //забирать N предмета при каждом использование


    float timeEat = 0f;

    public bool pause = false;

    void Start()
    {


        hp = hpMax;
        mCam = GetComponent<Player>().playerCamera;

       

        modLoader = GameObject.Find("Main").GetComponent<ModLoader>();




        myInv = Global.Links.getPlayerInvUi();

        sui = Global.Links.getSui();
        myPackSound = sui.GetComponent<packSound>();

        myAudio = GetComponent<AudioSource>();

        setCurLock(true);

        setHandItem(null);

        suiRender();

        Global.Links.getSui().winCloseAll();
        setCurLock(true);
    }



    void Update()
    {

        updEat();

        if (!pause)
        {
            updConroll();
        }

        updCenterCam();

        mCam.fieldOfView = Mathf.SmoothStep(mCam.fieldOfView, camFiledView, 0.19f);

    }

    public void updCenterCam()
    {

        float trSpeed = 12.6f;
        if (Vector3.Distance(centerCamHand.transform.localPosition, new Vector3(0f, 0.647f, -0.127f) )< 0.07f){
            trSpeed = 3.7f;
        }
        centerCamHand.transform.localPosition = Vector3.Lerp(centerCamHand.transform.localPosition, new Vector3(0f, 0.647f, -0.127f), Time.deltaTime * trSpeed);
        centerCamHand.transform.rotation = Quaternion.Lerp(centerCamHand.transform.rotation, mCam.transform.rotation, Time.deltaTime *18.6f);
    }


    public void animActionSend()
    {
        hand.GetComponent<Animator>().SetBool("attack", false);


        if (myItemType == itemType.block)
        {

            ActionItem_CrackBlock();

        }


        if (myItemType == itemType.handTool)
        {

            if (giveUse > 0)
            {
                if (!myInv.giveActive(giveUse, false)) return;

                bool actIsset =  ActionItem_CrackBlock();

                if (actIsset)
                {
                    myInv.giveActive(giveUse);
                }
            }
            else
            {
                ActionItem_CrackBlock();
            }
          
        }


        if (myItemType == itemType.eat)
        {

            if (myInv.giveActive(1))
            {

                myAudio.PlayOneShot(myPackSound.getSound(myPackSound.souEat));
                hp += MyItemIni.ReadInt("hpAdd", "eat", 0);
                eat += MyItemIni.ReadInt("eatAdd", "eat", 0);

                if (hp > hpMax) hp = hpMax;
                if (eat > 100) eat = 100;
            }

            suiRender();
        }


        if (myItemType == itemType.gun)
        {
            ActionItem_Fire(); 
        }

    }

    public itemType myItemType;


     IniFile MyItemIni;

    public void setHandItem(itemElement it)
    {
        hand.gameObject.SetActive(true);
        handWeapon.gameObject.SetActive(false);


        if (it == null)
        {
            myItemType = itemType.handTool;
            giveUse = 0;
            toolDamage =1;
            toolDist =2;
            timeAttackMax =0.3f;

            handSprite.SetActive(false);
            handCube.SetActive(false);
            return;
        }


        setSpriteHand(it.infoItemSave.iconInHand, it.infoItemSave.type == itemType.block);

        myItemType = it.infoItemSave.type;

        IniFile MyIni = new IniFile(it.infoItemSave.iniFilePath);

        MyItemIni = MyIni;

        giveUse = MyIni.ReadInt("giveUse", "action", 0);


        ammoReloadInd = MyIni.Read("ammo", "itemInfo", "not");


        if (myItemType == itemType.eat)
        {
            toolDamage = MyIni.ReadInt("eatAdd", "eat", 1);
        //    toolDamage = MyIni.ReadInt("hpAdd", "eat", 1);
        }


        if (myItemType == itemType.handTool || myItemType == itemType.gun )
        {
            toolDamage = MyIni.ReadInt("damage", "action", 1);
            toolDist = MyIni.ReadInt("distance", "action", 1) / 1f;
            timeAttackMax = MyIni.ReadInt("fireDelay", "action", 40) / 100f;
        }


        if(it.infoItemSave.type == itemType.handTool)
        {
           /*
            toolDist = MyIni.ReadFloat("distance", "action", 1);
            print(toolDist);
            */ 

            
            toolMaterialTarget = (blockMaterial)Enum.Parse(typeof(blockMaterial), MyIni.Read("materialTarget", "action", "ground"));
        }

        if (it.infoItemSave.type == itemType.gun)
        {
             
            hand.gameObject.SetActive(false);
            handWeapon.gameObject.SetActive(true);
            myWeapon.designImport(MyIni.Read("design", "gun", "not"));
            myWeapon.buildSetting();

             
        }

        suiRender();

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




    void updConrollUi()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!Cursor.visible) setCurLock(false);

            Global.Links.getSui().winCloseAll();
            Global.Links.getSui().winOpen("winCraft");


            Global.Links.getSui().winOpen("invMain");
            Global.Links.getPlayerMainInvUi().openCargo(Global.Links.getIndDataPlayerCargo());


        }

        /*
        if (Input.GetKeyDown(KeyCode.T))
        {
            setCurLock(false);

            Global.Links.getSui().winCloseAll();
            Global.Links.getSui().winOpen("winAgregat");
            Global.Links.getSui().winOpen("invMain"); 
        }
        */


        //Админский инвентарь
        if (Input.GetKeyDown(KeyCode.U))
        {
            Global.Links.getSui().winCloseAll();


            Global.Links.getSui().winOpen("invMain");
            Global.Links.getSui().winOpen("winInvOther");
            Global.Links.getOtherInvUi().openCargo(Global.Links.getIndDataAdminCargo());


            setCurLock(false);

        }

        if (Input.GetKeyDown(KeyCode.I))
        {

            if (Global.Links.getSui().winInvMain.gameObject.active)
            {


                Global.Links.getSui().winCloseAll();
                setCurLock(true);

            }
            else
            {
                Global.Links.getSui().winCloseAll();


                Global.Links.getSui().winOpen("invMain");
                Global.Links.getPlayerMainInvUi().openCargo(Global.Links.getIndDataPlayerCargo());


                setCurLock(false);
            }


        }
    }
    void updConroll()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            myInv.selectNewActiveInde(myInv.actveIndex + 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            myInv.selectNewActiveInde(myInv.actveIndex - 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) myInv.selectNewActiveInde(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) myInv.selectNewActiveInde(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) myInv.selectNewActiveInde(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) myInv.selectNewActiveInde(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) myInv.selectNewActiveInde(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) myInv.selectNewActiveInde(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) myInv.selectNewActiveInde(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) myInv.selectNewActiveInde(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) myInv.selectNewActiveInde(8);


        if (timeAttack <= 0f)
        {

        }
        else
        {
            timeAttack -= Time.deltaTime;
        }

        updConrollUi();

        if (Cursor.visible) return;


        if (Input.GetButton("Fire1"))
        {
            if (timeAttack <= 0f)
            {

                if (myItemType == itemType.block)
                {
                    hand.GetComponent<Animator>().SetBool("attack", true);
                    timeAttack = timeAttackMax;
                }

                if (myItemType == itemType.handTool || myItemType == itemType.eat)
                {


                    hand.GetComponent<Animator>().SetBool("attack", true);
                    timeAttack = timeAttackMax;
                }


                if (myItemType == itemType.gun)
                {
                    ActionItem_Fire();
                    timeAttack = timeAttackMax;
                }
            }

        }

        if (myItemType == itemType.gun)
        {

            if (Input.GetKeyDown(KeyCode.H))
            {
                handWeapon.GetComponent<Animator>().SetTrigger("show");
            }


            if (Input.GetButton("Fire2"))
            {
                camFiledView = 60;
            }
            else
            {
                camFiledView = 80;
            }
        }
        else
        {
            camFiledView = 80;
        }




        if (Input.GetButtonDown("Fire2"))
        {

            if (myItemType == itemType.block)
            {
                ActionItem_Build();
            }
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            ActionItem_Use(true);
        }

        //test

        if (Input.GetButtonDown("Fire2"))
        {
            if (myItemType != itemType.gun && myItemType != itemType.block)
            {
                ActionItem_Use(false);
            }
        }


      


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammoReloadInd != "not")
            {

                if (myInv.activeElement.infoItemSave.type == itemType.gun)
                {

                }

                int needCount = myInv.activeElement.infoItemSave.stackSize;

                needCount = needCount - myInv.activeElement.count;

                if (needCount > 0)
                {


                    if (myInv.myData.giveFromInd(ammoReloadInd, needCount))
                    {
                        myInv.activeElement.count += needCount;
                        myInv.ReRender();
                    }
                }


            }
        }

    }


    GameObject GetBlockFromRayCamera(bool useForwardHandCenter=false)
    {
        Vector3 rayOrigin = mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

       
        RaycastHit hit;

        
        Vector3 forw = mCam.transform.forward;
        if (useForwardHandCenter)
        {
            forw = centerCamHand.transform.forward;
            forw = myWeapon.transform.forward;
        }

        if (Physics.Raycast(rayOrigin + forw*0.1f, forw, out hit, toolDist))
        {

            lastHitTestPoint = hit.point;
            //  print(hit.transform.name);
            return hit.transform.gameObject;

        }
        return null;
    }

    Vector3 lastHitTestPoint;


    void ActionItem_Fire()
    {
        if (!myInv.giveActive(1, false)) return;


        myAudio.PlayOneShot(myPackSound.getSound(myPackSound.souGunFire));

        myInv.giveActive(1);

        GameObject go = GetBlockFromRayCamera(true);


        mCam.fieldOfView += 1.3f;

        GetComponent<Player>().rotationX -= 0.35f;
        centerCamHand.transform.rotation *= Quaternion.Euler(-2.2f, 0f, 0);
        //centerCamHand.transform.position += new Vector3(0f, 0.009f, 0f) ;

        if (go == null)
        {
            return;
        }

        if (go == gameObject)
        {
            return;
        }

            GameObject bulHole =  Instantiate(pref_bulletHole, go.transform);
        bulHole.transform.position = lastHitTestPoint;
        //bulHole.transform.rotation =  Quaternion.LookRotation(lastHitTestPoint, mCam.transform.position);
        bulHole.transform.LookAt(mCam.transform.position, -Vector3.up);// =  Quaternion.Euler(mCam.transform.forward);


        if (go.transform.tag == "block")
        {
            //print("Fire block");
            //Destroy(go); 
            go.GetComponent<BlockController>().Damage(toolDamage/4f, blockMaterial.all, null);
        }

        if (go.transform.tag == "meatPart")
        {
            go.transform.GetComponent<meatPart>().Damage(toolDamage);
           
            return;


        }
            if (go.transform.tag == "mob")
            {
                go.transform.GetComponent<botBody>().Damage(toolDamage);
                //Destroy(go);
            }
      
 

    }

    

    bool ActionItem_CrackBlock()
    {


        GameObject go = GetBlockFromRayCamera();
        if (go == null)
        {
            return false;
        }



        if (go.transform.tag == "block")
        {
            BlockController b = go.GetComponent<BlockController>();

            if (toolDamage < 0)
            {
                if (b.hp >= b.hpMax) return false;
            }

            myAudio.PlayOneShot(myPackSound.getDigSound(b.myMaterial));
            //myAudio.PlayOneShot(myPackSound.getDigSound());

            b.Damage(toolDamage, toolMaterialTarget, gameObject);
            return true;
        }



        if (go.transform.tag == "meatPart")
        {
            if (go.transform.parent.tag == "mob")
            {
                 
                go.transform.parent.GetComponent<botBody>().Damage(toolDamage * 3);
                //Destroy(go);
            }

            Destroy(go);
            return true;


        }
        if (go.transform.tag == "mob")
        {
            myAudio.PlayOneShot(myPackSound.getSound(myPackSound.souDamage));
            go.transform.GetComponent<botBody>().Damage(toolDamage);
            //Destroy(go);
            return true;
        }


        return false;
    }

    bool ActionItem_Use(bool isInfoBlockOn =false)
    {

        GameObject go = GetBlockFromRayCamera();
        if (go == null)
        {
            return false;
        }
        if (go.transform.tag == "block")
        {
            BlockController b = go.GetComponent<BlockController>();

            b.initBlock();

            if (b.myType == blockType.cargo)

            {
                myAudio.PlayOneShot(myPackSound.souChest);
                Global.Links.getSui().winOpen("winInvOther");

                Global.Links.getSui().winOpen("invMain");
                //Global.Links.getOtherInvUi().gameObject.SetActive(true);

                Global.Links.getOtherInvUi().openCargo(b.GetComponent<invData>());

                setCurLock(false);
                return true;
                // print("Open cargo");
            }

            if (b.myType == blockType.agregat)
            {

                myAudio.PlayOneShot(myPackSound.souClick);
                setCurLock(false);
                Global.Links.getSui().winOpen("invMain");
                Global.Links.getSui().winOpen("winAgregat");
                b.GetComponent<blockTypeAgregat>().open();

                return true;
            }


            if (b.myType == blockType.door)
            {
                print("Open door");
            }


            if (b.itemInd == "Core:adminBuild")
            {


                myAudio.PlayOneShot(myPackSound.souClick);
                b.GetComponent<adminBuilderBlock>().createAdminBuildExportData();
                return true;
            }


            if (isInfoBlockOn)
            {
                print("\n ---- -----");
                print(b.transform.name);
                print(b.itemInd);
                print(b.myMaterial);
                print("HP:" + b.hp);
            }


         

            
        }
        return false;

    }
    void ActionItem_Build()
    {

            if (ActionItem_Use(false)) return;

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
        newBlock.transform.name = newBlock.transform.position.x.ToString() + ":" + newBlock.transform.position.y.ToString() + ":" + newBlock.transform.position.z.ToString();

        newBlock.itemInd = myInv.activeElement.ind;
        newBlock.hp =0f;

        newBlock.initBlockPreload();
        newBlock.initBlock();
        newBlock.GetComponent<MeshRenderer>().enabled = true;

        myAudio.PlayOneShot(myPackSound.getDigSound(newBlock.myMaterial));
        //newBlock.GetComponent<MeshRenderer>().material.mainTexture = myInv.activeElement.infoItemSave.icon;


        myInv.giveActive(1);


        //newBlock.transform.localPosition = newBlock.transform.localPosition + new Vector3(0, 1, 0);

    }

    public void suiRender()
    {
        Global.Links.getSui().transform.Find("winGame/hp").GetComponent<Text>().text = "HP " + hp.ToString()+" / " + hpMax.ToString();
        Global.Links.getSui().transform.Find("winGame/eat").GetComponent<Text>().text = "EAT " + eat.ToString()+" / 100";


        Global.Links.getSui().transform.Find("aim").gameObject.SetActive( myItemType != itemType.gun);


    }

    public void Damage(float count)
    {


        mCam.fieldOfView -= 2.7f;
        hp -= count;
        if (hp <= 0)
        {
            GetComponent<Player>().canMove = false;

            Global.Links.getSui().winOpen("winDie");
            pause = true;
            hp = 0;
        }
        else
        {

            myAudio.PlayOneShot(myPackSound.getSound(myPackSound.souDamage));
        }
        suiRender();
    }

    public void setCurLock(bool canMoveAndLockCur)
    {
        if (canMoveAndLockCur)
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        Cursor.visible = !canMoveAndLockCur;

        GetComponent<Player>().canMove = canMoveAndLockCur;

        if (canMoveAndLockCur)
        {
            Global.Links.getSui().winInvOther.closeCargo();
            //Global.Links.getSui().winInvOther.gameObject.SetActive(false);
        }
    }

    public void updEat()
    {


        if (timeEat <= 0f)
        { 
            timeEat = 5f;
 

            if (eat > 0f)
            {
                eat -= 0.2f;
            }

            



            if (eat <= 30f)
            {
                Damage(1f);
            }
            else
            {
                suiRender();
            }

        }


        timeEat -= Time.deltaTime;
    }
}