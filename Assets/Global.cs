using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Global
{
    class Links   // revision 11
    {

        public static string vectorToString(Vector3 v)
        {

            return v.x.ToString() + ":" + v.y.ToString() + ":" + v.z.ToString();


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
