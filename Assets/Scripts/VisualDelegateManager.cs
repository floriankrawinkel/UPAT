using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDelegateManager : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private ResearchDataCollector dataCollector;

    [Header("Visual Delegate Settings")]
    public float timeAmountBeforeVisualDelegateStart = 3f;
    public float fadeInTime = 3f;
    public float fadeOutTime = 1f;

    public List<VisualDelegate> visualDelegates = new List<VisualDelegate>();
    private List<string> acitveRoutines = new List<string>();
    private bool visualDelegateTimerStarted = false;
    private float timer;

    private int focusLossCount = 0;
    private float focusLostTimestamp = -1f;
    private List<float> refocusDurations = new List<float>();

    private void Start()
    {
        timer = timeAmountBeforeVisualDelegateStart;
    }
    private void Update()
    {
        if (visualDelegateTimerStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                visualDelegateTimerStarted = false;
                timer = timeAmountBeforeVisualDelegateStart;

                foreach (VisualDelegate visualDelegate in visualDelegates)
                {
                    visualDelegate.ChangeParameterValueOverTime(true, this);
                }
            }
        }
    }

    public void GazeSelect()
    {
        StopAllCoroutines();
        visualDelegateTimerStarted = false;
        timer = timeAmountBeforeVisualDelegateStart;

        if (focusLostTimestamp > 0f)
        {
            float refocusTime = Time.time - focusLostTimestamp;
            refocusDurations.Add(refocusTime);

            if (dataCollector != null)
            {
                dataCollector.RegisterRefocusTime(refocusTime);
            }

            focusLostTimestamp = -1f;
        }

        foreach (VisualDelegate visualDelegate in visualDelegates)
        {
            visualDelegate.ChangeParameterValueOverTime(false, this);
        }
    }

    public void GazeUnselect()
    {
        visualDelegateTimerStarted = true;

        focusLossCount++;
        focusLostTimestamp = Time.time;

        if (dataCollector != null)
        {
            dataCollector.RegisterFocusLoss(focusLossCount);
        }
    }

    public void StartVisualDelegateFade(VisualDelegate visualDelegate, float targetValue, float duration)
    {
        Coroutine newRoutine = StartCoroutine(FadeRoutine(visualDelegate, targetValue, duration));
        acitveRoutines.Add(visualDelegate.visualDelegateName);
    }

    private IEnumerator FadeRoutine(VisualDelegate visualDelegate, float targetValue, float duration)
    {
        float startValue = visualDelegate.GetChangingParameter();
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float value = Mathf.Lerp(startValue, targetValue, time / duration);
            visualDelegate.SetChangingParameter(value);

            yield return null;
        }

        visualDelegate.SetChangingParameter(targetValue);
    }
}
