using UnityEngine;

public class ItemData : MonoBehaviour {
    public string nameItem;
    public string type;
    public int quantity;
    public float damage;
    public Sprite icon; 

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
                if (playerInventory != null)
                {
                    playerInventory.PickupItem(new Item
                    {
                        name = this.name,
                        type = this.type,
                        quantity = this.quantity,
                        damage = this.damage,
                        icon = this.icon
                    }
                    );

                    Destroy(gameObject); // Удаляем объект предмета
                }
        }
    }
}