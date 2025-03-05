using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public Text interactionText;
    private bool isInRange = false; // Признак, находится ли игрок рядом

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
        Debug.Log("Взаимодействие с объектом!");
        // Здесь можно добавить логику взаимодействия
    }
}