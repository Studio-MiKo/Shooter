using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour {
    public string nameItem;
    public string type;
    public int quantity;
    public float damage;
    public Sprite icon; 
    public Transform handTransform;

    public Text interactionText;
    private bool isInRange = false; // Признак, находится ли игрок рядом
    private Collider playerCollider;
    private static bool isBusyHand = false;

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E)) // Нажатие клавиши E
        {
            Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Если игрок вошел в триггер
        {
            playerCollider = other;
            isInRange = true;
             interactionText.text = "Press E"; // Отображаем подсказку
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Если игрок покинул триггер
        {
            isInRange = false;
            interactionText.text = ""; // Скрываем подсказку
        }
    }

    void Interact()
    {
                PlayerInventory playerInventory = playerCollider.GetComponent<PlayerInventory>();
                if (playerInventory != null)
                {
                    if(this.type == "weapon" && !isBusyHand)
                    {
                        Collider weaponCollider = this.GetComponent<Collider>();
                        if (weaponCollider != null)
                        {
                            Debug.LogWarning("выкл триггер" + this.name);
                            weaponCollider.isTrigger = false;
                        }

                         // Перемещаем оружие в руку
                        this.transform.SetParent(handTransform); // Делаем оружие дочерним объектом руки
                        this.transform.localPosition = new Vector3(0, 0, 0.600000024f); // Устанавливаем позицию в руке
                        this.transform.localRotation = Quaternion.Euler(0, 260, 0); // Устанавливаем поворот в руке
                         
                       isBusyHand = true;
                    }
                    else if(this.type != "weapon" || isBusyHand)
                    {
                         playerInventory.PickupItem(new Item
                        {
                            name = this.name,
                            type = this.type,
                            quantity = this.quantity,
                            damage = this.damage,
                            icon = this.icon
                        });

                        Destroy(gameObject); // Удаляем объект предмета
                    }
                    
                    isInRange = false;
                    interactionText.text = "";
                }
    }
}