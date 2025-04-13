using System.Collections;
using TMPro;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    // Shooting
    [Header("Shooting")]
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // Burst
    [Header("Burst")]
    public int bulletPerBurst = 3;
    public int burstBulletsLeft;

    // Spread
    [Header("Spread")]
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    // Bullet
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpaw;
    public float bulletVelocity = 500;
    public float bulletPrefabeLifeTime = 3f;  //seconds

    public GameObject muzzleEffect;
    internal Animator animator;

    // Loading
    [Header("Loading")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    bool isADS;

    public enum WeaponModel
    {
        PistolM1911,
        M107,
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto,
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
        
        spreadIntensity = hipSpreadIntensity;
    }

    void Update()
    {
        if(isActiveWeapon)
        {
            gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            // foreach(Transform child in transform)
            // {
            //     child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            //     Debug.Log($"{child.name} layer set to {LayerMask.LayerToName(child.gameObject.layer)}");
            // }

            if(Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }

            if(Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }

            GetComponent<Outline>().enabled = false;
            
            if(bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazinSundM1911.Play();
            }

            if(currentShootingMode == ShootingMode.Auto)
            {
                //Holding Down Left Mouse Button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if(currentShootingMode == ShootingMode.Burst ||
               currentShootingMode == ShootingMode.Single)
            {
                //Clicked Left Mouse Button once
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

             if(Input.GetKeyDown(KeyCode.R)  &&  bulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            // Automatically reload when magazineSize is empty
            if(readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                // Reload();
            }

            if(readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletPerBurst;
                FireWeapon();
            }
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            // foreach(Transform child in transform)
            // {
            //     child.gameObject.layer = LayerMask.NameToLayer("Default");
            // }
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;        
    }

    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if(isADS)
        {
            animator.SetTrigger("RECIOL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }
        
        // SoundManager.Instance.shootingSundM1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionSpread().normalized;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpaw.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        //Pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        // Shoot the bullet
        rb.AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        
        // Destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabeLifeTime));

        //Checking if we are done shooting
        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //Burst Mode
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // we already shoot once before this check
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        // SoundManager.Instance.reloadingSundM1911.Play();
        SoundManager.Instance.PlayReloadingSound(thisWeaponModel);

        animator.SetTrigger("RELOAD"); 

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft =  WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreseTotalAmmo(bulletsLeft, thisWeaponModel);
        }

        isReloading = false;
    }
    
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionSpread()
    {
        //Shooting from the midle of the screen to check where are we pointing at
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            //Hitting Something
            targetPoint = hit.point;
        }
        else
        {
            // Shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpaw.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        Debug.Log("Shooting Direction: " + direction + "Spread: " + new Vector3(0, y, z));
        return direction + new Vector3(0, y, z); // Returning the shooting direction and spread 
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}