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

        public static inv getPlayerInv()
        {
            return GameObject.Find("SUI/inv").GetComponent<inv>();

        }

        public static inv getOtherInv()
        {
            return getSui().invOther;
            //return GameObject.Find("SUI/invOther").GetComponent<inv>();

        }

        public static PlayerAction getPlayerAction()
        {
            return GameObject.Find("Player").GetComponent<PlayerAction>();

        }

        public static MapController getMapController()
        {
            return GameObject.Find("Map").GetComponent<MapController>();

        }
    }
}
