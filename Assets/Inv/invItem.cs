using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class invItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

   public invData myData;
    public itemElement myElement;

    public int myIndex = -1;

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

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        myData.dragElement(myElement,  eventData.position);
        //Debug.Log ("OnDrag");

        //this.transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        GameObject tempObj = eventData.pointerCurrentRaycast.gameObject;
       

        myData.dragEnd(myElement, tempObj);


    }
}

