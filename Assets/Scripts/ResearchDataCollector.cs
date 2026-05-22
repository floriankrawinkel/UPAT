using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class ResearchDataCollector : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private TrainingTimer trainingTimer;
    [SerializeField] private VisualDelegateManager visualDelegateManager;

    [Header("Data Specifics")]
    [SerializeField] private string dataFolderName;

    private string folderPath;
    private string researchDataPath;
    private StreamWriter writer;

    private int totalFocusLosses = 0;

    private List<float> refocusTimes = new List<float>();

    private float lastFocusLossTimestamp = -1f;
    private float currentTimeBetweenLosses = 0f;

    private void Awake()
    {
        folderPath = Path.Combine(Application.persistentDataPath, dataFolderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public void CreateCSV(string participantID, string collectionName, List<VisualDelegate> activeVisualDelegates)
    {
        //can be enabled if counter should only be increased per participant
        //int nextIndex = Directory.GetFiles(folderPath, $"{participantID}_Version{Application.version}_*.csv").Length + 1;

        int nextIndex = Directory.GetFiles(folderPath, "*.csv").Length + 1;

        researchDataPath = Path.Combine(folderPath,$"{participantID}_{collectionName}_{nextIndex}.csv");
        
        writer = new StreamWriter(researchDataPath, false);

        writer.WriteLine("VisualDelegates");
        writer.WriteLine("DelegateName,Depth,Abstraction,Control,FadeIn,FadeOut");

        foreach (var vd in activeVisualDelegates)
        {
            var row = new List<string>
            {
                vd.visualDelegateName,
                vd.depthLevel.ToString(),
                vd.abstractionLevel.ToString(),
                vd.controlLevel.ToString(),
                visualDelegateManager.fadeInTime.ToString(CultureInfo.InvariantCulture),
                visualDelegateManager.fadeOutTime.ToString(CultureInfo.InvariantCulture)
            };

            writer.WriteLine(string.Join(",", row));
        }

        writer.WriteLine();
        writer.WriteLine("Events");
        writer.WriteLine("FocusLossCount,FocusLossStartTime,FocusLossEndTime, RefocusTime,TimeBetweenFocusLosses");

        writer.Flush();

        Debug.Log("CSV created at: " + researchDataPath);
    }

    public void RegisterFocusLoss(int currentCount)
    {
        totalFocusLosses = currentCount;

        float currentTime = trainingTimer != null ? trainingTimer.GetElapsedTime(): 0f;

        if (lastFocusLossTimestamp >= 0f)
        {
            currentTimeBetweenLosses = currentTime - lastFocusLossTimestamp;
        }
        else
        {
            currentTimeBetweenLosses = 0f;
        }

        lastFocusLossTimestamp = currentTime;
    }

    public void RegisterRefocusTime(float time)
    {
        string formattedTime = time.ToString("F3", CultureInfo.InvariantCulture);

        float elapsedTrainingTime = trainingTimer != null ? trainingTimer.GetElapsedTime(): 0f;

        refocusTimes.Add(time);

        if (writer != null)
        {
            writer.WriteLine($"{totalFocusLosses}," +$"{lastFocusLossTimestamp.ToString("F3", CultureInfo.InvariantCulture)},"+$"{elapsedTrainingTime.ToString("F3", CultureInfo.InvariantCulture)},"  +$"{formattedTime}," +$"{currentTimeBetweenLosses.ToString("F3", CultureInfo.InvariantCulture)}");

            writer.Flush();
        }
    }

    public void FinishCSV()
    {
        if (writer != null)
        {
            float avgRefocus = refocusTimes.Count > 0 ? (float)refocusTimes.Average(): 0f;

            float totalTrainingTime = trainingTimer != null ? trainingTimer.GetElapsedTime(): 0f;

            writer.WriteLine();
            writer.WriteLine("Summary");
            writer.WriteLine("TimeInTraining,OverallFocusLossCount,AverageRefocusTime");

            writer.WriteLine($"{totalTrainingTime.ToString("F3", CultureInfo.InvariantCulture)}," +$"{totalFocusLosses}," +$"{avgRefocus.ToString("F3", CultureInfo.InvariantCulture)}");

            writer.Flush();
            writer.Close();
            writer = null;

            Debug.Log("CSV saved successfully.");
        }
    }

    private void OnApplicationQuit()
    {
        FinishCSV();
    }
}