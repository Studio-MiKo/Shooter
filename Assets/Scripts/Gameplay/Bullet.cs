using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;

    private void OnCollisionEnter(Collision objectWeHeat)
    {
        if(objectWeHeat.gameObject.CompareTag("Target"))
        {
            print("hit " + objectWeHeat.gameObject.name + " !");
            CreateBulletImpactEffect(objectWeHeat);
            Destroy(gameObject); 
        }   

        if(objectWeHeat.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");
            CreateBulletImpactEffect(objectWeHeat);
            Destroy(gameObject); 
        }  

        if(objectWeHeat.gameObject.CompareTag("Bottle"))
        {
            print("hit a bottle");
            objectWeHeat.gameObject.GetComponent<Bottle>().Shatter(); 
        } 

        if(objectWeHeat.gameObject.CompareTag("Enemy"))
        {
            print("hit a zombie");

            if(objectWeHeat.gameObject.GetComponent<Enemy>().isDead == false)
            {
                objectWeHeat.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage); 
            }

            CreateBloodSprayEffect(objectWeHeat);

            Destroy(gameObject); 
        }   
    }

    private void CreateBloodSprayEffect(Collision objectWeHeat)
    {
        ContactPoint contact = objectWeHeat.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        bloodSprayPrefab.transform.SetParent(objectWeHeat.gameObject.transform);
    }

    private void CreateBulletImpactEffect(Collision objectWeHeat)
    {
        ContactPoint contact = objectWeHeat.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        hole.transform.SetParent(objectWeHeat.gameObject.transform);
    }
}
