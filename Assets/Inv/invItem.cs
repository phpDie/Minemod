using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invItem : MonoBehaviour
{
   public invData myData;
    public itemElement myElement;
    

    public void setNull()
    {
        transform.GetChild(2).GetComponent<Text>().text = "";
        transform.GetChild(1).GetComponent<Text>().text = "";
        transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(3).gameObject.SetActive(false);
    }

    public void clickUse()
    {
        myData.clickItemElemet(myElement);
        

    }
}
