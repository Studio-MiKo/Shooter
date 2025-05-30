using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance {get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;

    [Header("Throwables")]
    // public int grenades = 0;
    public float throwForce = 10f;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0;
    public float forceMultiplierLimit = 2f;

    [Header("Lethals")]
    public int maxLethals = 2;
    public int lethalsCount = 0;
    public Throwables.ThrowableType equippedLethalType;
    public GameObject grenadePrefabe;

    [Header("Tactical")]
    public int maxTacticals = 2;
    public int tacticalCount = 0;
    public Throwables.ThrowableType equippedTacticalType;
    public GameObject SmokeGrenadePrefab;

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
        equippedLethalType = Throwables.ThrowableType.None;
        equippedTacticalType = Throwables.ThrowableType.None;
    }

    private void Update()
    {
        foreach(GameObject weaponSlot in weaponSlots)
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if(Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T))
        {
            forceMultiplier += Time.deltaTime;

            if(forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if(Input.GetKeyUp(KeyCode.G))
        {
            if(lethalsCount > 0)
            {
                ThrowLethal();
            }

            forceMultiplier = 0;
        }

        if(Input.GetKeyUp(KeyCode.T))
        {
            if(tacticalCount > 0)
            {
                ThrowTactical();
            }

            forceMultiplier = 0;
        }
    }

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

    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaonIntoActiveSlot(pickedupWeapon); 
    }

    private void AddWeaonIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);
        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    }
    
    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }
        
        activeWeaponSlot = weaponSlots[slotNumber];

        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    internal void DecreseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch(thisWeaponModel)
        {
            case Weapon.WeaponModel.M107:
                totalRifleAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.PistolM1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
        }
    }

     public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch(thisWeaponModel)
        {
            case Weapon.WeaponModel.M107:
                return totalRifleAmmo;
            case Weapon.WeaponModel.PistolM1911:
                return totalPistolAmmo;
            default:
                return 0;
        }
    }   
    

    #region || ---- Throwables ---- ||
    public void PickupThrowable(Throwables pickedupThrowable)
    {
        switch (pickedupThrowable.throwableType)
        {
            case Throwables.ThrowableType.Grenade:
                PickupThrowableAsLethal(Throwables.ThrowableType.Grenade);
                break;
            case Throwables.ThrowableType.Smoke_Grenade:
                PickupThrowableAsTactical(Throwables.ThrowableType.Smoke_Grenade);
                break;
        }
    }

    private void PickupThrowableAsTactical(Throwables.ThrowableType tactical)
    {
        if(equippedTacticalType == tactical || equippedTacticalType == Throwables.ThrowableType.None)
        {
            equippedTacticalType = tactical;

            if(tacticalCount < maxTacticals)
            {
                tacticalCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowablesUI();
            }
            else{
                print("Tactical limit reached/ Ch in WM 200");
            }
        }
        else
        {
            // Swap tactical
        }
    }

    private void PickupThrowableAsLethal(Throwables.ThrowableType lethal)
    {
        if(equippedLethalType == lethal || equippedLethalType == Throwables.ThrowableType.None)
        {
            equippedLethalType = lethal;

            if(lethalsCount < maxLethals)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowablesUI();
            }
            else{
                print("Lethal limit reached/ Ch in WM 200");
            }
        }
        else
        {
            // Swap lethal
        }
    }
    
    private void ThrowLethal()
    {
        GameObject LethalPrefab = GetThrowablePrefab(equippedLethalType);
        GameObject throwable = Instantiate(LethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);
        
        throwable.GetComponent<Throwables>().hasBeenThrown = true;

        lethalsCount -= 1;

        if(lethalsCount <= 0)
        {
            equippedLethalType = Throwables.ThrowableType.None;
        }

        HUDManager.Instance.UpdateThrowablesUI();

    }

    private void ThrowTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);
        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwables>().hasBeenThrown = true;

        tacticalCount -= 1;

        if(tacticalCount <= 0)
        {
            equippedLethalType = Throwables.ThrowableType.None;
        }

        HUDManager.Instance.UpdateThrowablesUI();
    }

    private GameObject GetThrowablePrefab(Throwables.ThrowableType throwableType)
    {
        switch(throwableType)
        {
            case Throwables.ThrowableType.Grenade:
                return grenadePrefabe;
            case Throwables.ThrowableType.Smoke_Grenade:
                return SmokeGrenadePrefab;
        }

        return new();
    }
    #endregion
}
