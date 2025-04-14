using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance {get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwables hoveredThrowable = null;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
           GameObject objectHitByRaycast = hit.transform.gameObject;

            if(objectHitByRaycast.GetComponent<Weapon>() && (objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false))
            {
                // Disable the outline of previoudly selected item
                if(hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }

                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.F))
                {
                    // hoveredWeapon.GetComponent<Outline>().enabled = false;
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if(hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

           // Ammo Box   
           if(objectHitByRaycast.GetComponent<AmmoBox>())
            {
                // Disable the outline of previoudly selected item
                if(hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.F))
                {
                    // hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if(hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }

             // Throwable   
           if(objectHitByRaycast.GetComponent<Throwables>())
            {
                // Disable the outline of previoudly selected item
                if(hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }

                hoveredThrowable = objectHitByRaycast.gameObject.GetComponent<Throwables>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.F))
                {
                    // hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                    WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                }
            }
            else
            {
                if(hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
        // else
        // { 
        //     if (hoveredWeapon) 
        //     { 
        //         hoveredWeapon.GetComponent<Outline>().enabled = false; 
        //     }
        // }
    }
}
