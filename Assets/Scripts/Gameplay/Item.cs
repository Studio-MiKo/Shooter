using UnityEngine;
using UnityEngine.UI;

public class Item {
    public string name;
    public string type; // "weapon", "ammo", "healthkit"
    public int quantity;
    public float damage; // Только для оружия
    public Sprite icon; 
}