using System.Collections;
using TMPro;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    // Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // Burst
    public int bulletPerBurst = 3;
    public int burstBulletsLeft;

    // Spread
    public float spreadIntensity;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpaw;
    public float bulletVelocity = 30;
    public float bulletPrefabeLifeTime = 3f;  //seconds

    public GameObject muzzleEffect;
    private Animator animator;

    // Loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

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
    }

    void Update()
    {
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

        if(Input.GetKeyDown(KeyCode.R)  &&  bulletsLeft < magazineSize && isReloading == false)
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

        if(AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletPerBurst}/{magazineSize/bulletPerBurst}";
        }
    }
    
    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        
        // SoundManager.Instance.shootingSundM1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionSpread().normalized;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpaw.position, Quaternion.identity);

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
        isReloading = false;
        bulletsLeft = magazineSize;
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

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0); // Returning the shooting direction and spread 
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}