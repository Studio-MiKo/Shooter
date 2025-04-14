using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZmobieHand zmobieHand;

    public int zombieDamage;

    private void Start()
    {
        zmobieHand.damage = zombieDamage;
    }
}
