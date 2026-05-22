using TMPro;
using UnityEngine;

public class TrainingTimer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup endTrainingCanvas;
    [SerializeField] private TMP_Text timeInTrainingText;

    private float currentTime = 0f;
    private float maxTime;
    private bool isRunning = false;

    public void StartTimer(float timeAmount)
    {
        maxTime = timeAmount;
        currentTime = 0f;
        isRunning = true;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        UpdateDisplayedTime();

        if (!isRunning) return;

        if (currentTime >= maxTime)
        {
            currentTime = maxTime;
            isRunning = false;
            TimerFinished();
        }
    }

    private void UpdateDisplayedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timeInTrainingText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetElapsedTime()
    {
        return currentTime;
    }

    public void TimerFinished()
    {
        Application.Quit();
    }
}