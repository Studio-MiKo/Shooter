using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость движения

    void Update()
    {
        // Получаем ввод с клавиатуры
        float moveHorizontal = Input.GetAxis("Horizontal"); // Лево/Право (A/D)
        float moveVertical = Input.GetAxis("Vertical");     // Вперед/Назад (W/S)

        // Создаем направление движения
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        // Применяем движение
        transform.Translate(movement * speed * Time.deltaTime);
    }
}