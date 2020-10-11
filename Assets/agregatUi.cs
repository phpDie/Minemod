using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class agregatUi : MonoBehaviour
{
    public Image barProgress;
    public Image barBatarey;

    public Text txtTitle;
    public Text txtBtn;
    

    public inv myInvUiInput;
    public inv myInvUiOut;


    public blockTypeAgregat myAgr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        if (myAgr != null)
        {
            myInvUiOut.closeCargo();
            myInvUiInput.closeCargo();
            myAgr.closeUi();
            
            myAgr = null;
        }
    }

    private void OnDestroy()
    {
        if (myAgr != null)
        {
            myInvUiOut.closeCargo();
            myInvUiInput.closeCargo();
            myAgr.closeUi();
            myAgr = null;
        }
    }

    public void btnGo()
    {
        if (myAgr != null)
        {
            myAgr.btnGo();
        }

    }
}
