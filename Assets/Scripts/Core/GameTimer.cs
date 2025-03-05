using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText; 
    private float timeRemaining = 2f * 60f; // 2 минут в секундах

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Уменьшаем время
            UpdateTimerDisplay(); // Обновляем отображение
        }
        else
        {
            EndGame(); // Завершаем игру
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // Минуты
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // Секунды

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame()
    {
        Debug.Log("Время вышло!");
    }
}