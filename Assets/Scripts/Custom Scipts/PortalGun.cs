using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    //Prefabs for entrance & exit
    public GameObject portalPrefabEntrance;
    public GameObject portalPrefabExit;

    //Current portal objects
    private GameObject curPortalEntrance;
    private GameObject curPortalExit;

    //Index for spawning
    private int portalIndex = 0;

    private void PortalFire(Vector3 position, GameObject player)
    {
        //If the portal index is 0, spawn the entrance
        if(portalIndex == 0)
        {
            if (curPortalEntrance != null || curPortalExit != null)
            {
                //Do the two portals need to be destroyed? If so, destroy them
                Destroy(curPortalEntrance);
                Destroy(curPortalExit);

                //Set both to null
                curPortalEntrance = curPortalExit = null;
            }

            //Spawn it in the world
            curPortalEntrance = Instantiate(portalPrefabEntrance, position, Quaternion.identity);

            //Set up variables
            curPortalEntrance.GetComponent<Portal>().shouldTeleport = false;
        }

        //Otherwise, if the portal index is 1, spawn the exit
        else if(portalIndex == 1)
        {
            //Spawn it in the world
            curPortalExit = Instantiate(portalPrefabExit, position, Quaternion.identity);

            //Set up variables for entrance
            curPortalEntrance.GetComponent<Portal>().player = player;
            curPortalEntrance.GetComponent<Portal>().portal1 = curPortalEntrance;
            curPortalEntrance.GetComponent<Portal>().portal2 = curPortalExit;

            //Set up variables for exit
            curPortalExit.GetComponent<Portal>().player = player;
            curPortalExit.GetComponent<Portal>().portal1 = curPortalEntrance;
            curPortalExit.GetComponent<Portal>().portal2 = curPortalExit;

            //Set them both to teleport
            curPortalEntrance.GetComponent<Portal>().shouldTeleport = true;
            curPortalEntrance.GetComponent<Portal>().shouldTeleport = false;
        }

        //Increment portal index, but wrap it around 2
        portalIndex = (portalIndex + 1) % 2;
    }
}
