﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Global
{
    class Links   // revision 11
    {

        public static string[] stringVectorElementsFixCeil(string[] posBlock)
        {
            for(int i=0; i< posBlock.Length; i++)
            {
                posBlock[i] = posBlock[i].Split(',')[0];
            }

            return posBlock;
        }

        public static Vector3 stringToVector(string v)
        {
            string[] posBlock = v.Split(':');
            return new Vector3(System.Convert.ToInt32(posBlock[0]), System.Convert.ToInt32(posBlock[1]), System.Convert.ToInt32(posBlock[2]));

        }

        public static Vector3 vectorRound(Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));

        }
      
        

        public static string vectorToString(Vector3 v, string razdel=":")
        {

            return v.x.ToString() + razdel + v.y.ToString() + razdel + v.z.ToString();


        }

        public static sUi getSui()
        {        
           return GameObject.Find("SUI").GetComponent<sUi>();
                      
        }

        public static ModLoader getModLoader()
        {
            return GameObject.Find("Main").GetComponent<ModLoader>();

        }

        public static inv getPlayerInvUi()
        {
            return getSui().winInvAction;
            //return GameObject.Find("SUI/inv").GetComponent<inv>();

        }

        public static inv getPlayerMainInvUi()
        {
            return getSui().winInvMain;
           // return GameObject.Find("SUI/invMain").GetComponent<inv>();

        }
        

        public static agregatUi getAgregatUi()
        {
            return getSui().winAgregat;
           // return GameObject.Find("SUI/invMain").GetComponent<inv>();

        }


        public static inv getOtherInvUi()
        {
            return getSui().winInvOther;
            //return GameObject.Find("SUI/winInvOther").GetComponent<inv>();

        }

        public static PlayerAction getPlayerAction()
        {
            return GameObject.Find("Player").GetComponent<PlayerAction>();

        }

        public static MapController getMapController()
        {
            return GameObject.Find("Map").GetComponent<MapController>();

        }

        public static craftUi getCraftUi()
        {
            return getSui().winCraft;

        }


        public static invData getIndDataPlayerAction()
        {
            return GameObject.Find("Player/invAction").GetComponent<invData>();

        }

        public static invData getIndDataPlayerCargo()
        {
            return GameObject.Find("Player/invCargo").GetComponent<invData>();
        }

        public static invData getIndDataAdminCargo()
        {
            return GameObject.Find("Player/invAdmin").GetComponent<invData>();
        }

    }
}
