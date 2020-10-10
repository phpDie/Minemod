using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildController : MonoBehaviour
{
    public GameObject ghostBlank;

    [HideInInspector]
    public GameObject ghost;
    public PlayerAction plAct;

    // Start is called before the first frame update
    void Start()
    {
        plAct = GetComponent<PlayerAction>();
        ghost = Instantiate(ghostBlank);
    }

    Vector3 lastPos;
    Vector3 lastPosLocal;
    public Transform lastChank;

    public Vector3 rayTickGetPost()
    {
        Vector3 rayOrigin = plAct.mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Declare a raycast hit to store information about what our raycast has hit
        RaycastHit hit;

        // Set the start position for our visual effect for our laser to the position of gunEnd


        // Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin + plAct.mCam.transform.forward*0.1f, plAct.mCam.transform.forward, out hit, 15f))
        {

            if (hit.transform.tag == "driveChank___OLDVERsion")
            {
                Vector3 pos = hit.point;



                pos = hit.point;

                pos += plAct.mCam.transform.forward * -0.6f;

                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));

               // pos = hit.transform.parent.position + pos;



                if (hit.transform.position == pos)
                {
                    pos = hit.point;
                    // print("Double fix");
                    pos += plAct.mCam.transform.forward * -0.3f;
                    pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                }

                //ghost.transform.position = pos;

                lastPos = pos;


                lastChank = hit.transform;

            }

            if (hit.transform.tag == "driveChank")
            {

                Vector3 pos = hit.point;


                pos = hit.collider.transform.localPosition;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                pos = hit.transform.position + pos;





                if (hit.collider.transform.position == pos)
                {
                    pos = hit.point;
                    // print("Double fix");
                    pos += plAct.mCam.transform.forward * -0.3f;
                    pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                }

                //ghost.transform.position = pos;

                lastPos = pos;
                lastChank = hit.transform;
            }


            if (hit.transform.tag == "block")
            {

                    Vector3 pos = hit.point ;


                pos =   hit.transform.localPosition;                
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                pos = hit.transform.parent.position + pos;





                if (hit.transform.position == pos)
                {
                    pos = hit.point;
                   // print("Double fix");
                    pos += plAct.mCam.transform.forward * -0.3f;
                    pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                }

                //ghost.transform.position = pos;

                lastPos = pos;
                lastChank = hit.transform.parent;

               

            }
             // hit.transform.gameObject;

        }
        return lastPos;
    }

    float timer = 0.4f;
    // Update is called once per frame
    void Update()
    {
        
        if (timer <= 0f)
        {
            rayTickGetPost();
            timer = 0.11f;
        }
        timer -= Time.deltaTime;
        

    }
}
