using System.Collections;
using System.Collections.Generic;
using UnityEngine;






[RequireComponent(typeof(CharacterController))]
public class botBody : MonoBehaviour
{

    [Header("Blank Prefabs")]
    public GameObject pref_bulletHole;


    [Header("Ai")]
    public float distAttack = 2f;
    public float distView = 12f;
    public bool gunIsset = false;
    public float hp = 1f;


    float timerAttack;
    public float timerAttackMax = 0.9f;


    [Header("Body")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;





    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;




    [HideInInspector]
    public float rotationX = 0;
    public bool canMove = true;

    public void Damage(float val)
    {

        hp -= val;
        if (hp <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        target = GameObject.Find("Player");


    }


    public void jump()
    {
        if (canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
    }


    public GameObject target;

    public void gunFire()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Transform w = transform.Find("start").transform;

        Vector3 rayOrigin = transform.Find("start").transform.position;

        RaycastHit hit;

        // Set the start position for our visual effect for our laser to the position of gunEnd



        // Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, 25f))
        {

            Transform damTo = hit.transform;



            if (hit.transform.tag == "Untagged")
            {
                damTo = hit.transform.parent;
                if (damTo.transform.tag == "Untagged")
                {
                    return;
                }

            }

            //print(damTo.transform.tag);
            Debug.DrawRay(rayOrigin, transform.forward * 25f, Color.red, 5f);


            if (damTo.transform.tag == "block")
            {

                damTo.GetComponent<BlockController>().Damage(1f, blockMaterial.all, null);
            }
            if (damTo.transform.tag == "Player")
            {
                damTo.GetComponent<PlayerAction>().Damage(1f);
            }
            else
            {
                GameObject bulHole = Instantiate(pref_bulletHole, damTo.transform);

                bulHole.transform.position = hit.point;
                bulHole.transform.LookAt(transform.position, -Vector3.up);
            }
            /*
             * 
            GameObject bulHole = Instantiate(pref_bulletHole);
            bulHole.transform.position = hit.point;
            bulHole.transform.LookAt(transform.position, -Vector3.up);
             */


        }

    }


    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run


        bool isRunning = false;



        /*
        if (Random.Range(1, 6)<3)
        {
            isRunning = true;
        }
        */


        Vector3 lTargetDir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 0.5f);

        // transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));


        float moveForw = 0f;



        if (Vector3.Distance(transform.position, target.transform.position) > distAttack)
        {
            moveForw = 1f;

            if (Random.Range(1, 526) < 2)
            {
                jump();
            }

        }
        else
        {
            moveForw = 0f;

            timerAttack -= Time.deltaTime;
            if (timerAttack <= 0f)
            {

                timerAttack = timerAttackMax;
                if (gunIsset)
                {
                    gunFire();
                }
                else
                {
                    target.GetComponent<PlayerAction>().Damage(1f);
                }
            }

        }


        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * moveForw : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * 0f : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        moveDirection.y = movementDirectionY;


        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);


        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
