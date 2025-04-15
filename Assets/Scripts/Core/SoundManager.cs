using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;

    public AudioClip M1911Shot;
    public AudioClip M107Shot;

    public AudioSource reloadingSundM1911;
    public AudioSource reloadingSundM107;
    public AudioSource emptyMagazinSundM1911;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChanel;
    public AudioSource zombieChanel2;

    public AudioSource playerChanel;
    public AudioClip playerHurt;
    public AudioClip playerDie;
    public AudioClip gameOverMusic;

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

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.PistolM1911:
                ShootingChannel.PlayOneShot(M1911Shot);
                break;
            case WeaponModel.M107:
                ShootingChannel.PlayOneShot(M107Shot);
                break;
            default:
                break;
        }
    }

    public void PlayReloadingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.PistolM1911:
                reloadingSundM1911.Play();
                break;
            case WeaponModel.M107:
                reloadingSundM107.Play();
                break;
            default:
                break;
        }
    }

}
