using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VisualDelegateParameterMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup visualDelegateMenuCanvasGroup;
    [SerializeField] private CanvasGroup parameterCanvasGroup;
    [SerializeField] private CanvasGroup endTrainingCanvas;
    [SerializeField] private Transform visualDelegateUIGridParent;

    [SerializeField] private DelegateItemUI itemPrefab;

    [SerializeField] private TMP_Dropdown visualDelegateDropdownMenu;

    [SerializeField] private TMP_InputField participantIdentifier;

    [SerializeField] private TMP_Text trainingTimeAmountText;
    [SerializeField] private TMP_Text beforeFadeInTimeAmountText;
    [SerializeField] private TMP_Text fadeInTimeAmountText;
    [SerializeField] private TMP_Text fadeOutTimeAmountText;

    [Header("Slider References")]
    [SerializeField] private Slider trainingTimeAmountSlider;
    [SerializeField] private Slider beforeFadeInTimeAmountSlider;
    [SerializeField] private Slider fadeInTimeAmountSlider;
    [SerializeField] private Slider fadeOutTimeAmountSlider;

    [Header("Script References")]
    [SerializeField] private TrainingTimer trainingTimer;
    [SerializeField] private ResearchDataCollector researchDataCollector;
    [SerializeField] private VisualDelegateManager visualDelegateManager;

    private VisualDelegateCollection[] collections;
    private VisualDelegateCollection selectedCollection;

    private readonly List<VisualDelegate> activeVisualDelegates = new();

    private bool infiniteTime = true;
    private bool testRun = false;

    void Start()
    {
        Time.timeScale = 0;

        CanvasVisibilityHelper(visualDelegateMenuCanvasGroup, true);

        CanvasVisibilityHelper(endTrainingCanvas, false);

        trainingTimeAmountSlider.onValueChanged.AddListener((value) => UpdateTimeText(value, trainingTimeAmountText));
        beforeFadeInTimeAmountSlider.onValueChanged.AddListener((value) => UpdateTimeText(value, beforeFadeInTimeAmountText));
        fadeInTimeAmountSlider.onValueChanged.AddListener((value) => UpdateTimeText(value, fadeInTimeAmountText));
        fadeOutTimeAmountSlider.onValueChanged.AddListener((value) => UpdateTimeText(value, fadeOutTimeAmountText));

        beforeFadeInTimeAmountSlider.value = visualDelegateManager.timeAmountBeforeVisualDelegateStart;
        fadeInTimeAmountSlider.value = visualDelegateManager.fadeInTime;
        fadeOutTimeAmountSlider.value = visualDelegateManager.fadeOutTime;

        UpdateTimeText(trainingTimeAmountSlider.value, trainingTimeAmountText);
        UpdateTimeText(beforeFadeInTimeAmountSlider.value, beforeFadeInTimeAmountText);
        UpdateTimeText(fadeInTimeAmountSlider.value, fadeInTimeAmountText);
        UpdateTimeText(fadeOutTimeAmountSlider.value, fadeOutTimeAmountText);

        PopulateCollectionDropdown();
    }
    private void PopulateCollectionDropdown()
    {
        collections = Resources.LoadAll<VisualDelegateCollection>("VisualDelegateCollections");
        
        List<string> dropdownOptions = new();

        foreach (VisualDelegateCollection collection in collections)
        {
            dropdownOptions.Add(collection.collectionName);
        }

        visualDelegateDropdownMenu.ClearOptions();
        visualDelegateDropdownMenu.AddOptions(dropdownOptions);

        visualDelegateDropdownMenu.onValueChanged.AddListener(OnDropdownChanged);

        if (collections.Length > 0)
        {
            selectedCollection = collections[0];
            ShowCollection(selectedCollection);
        }
    }

    private void OnDropdownChanged(int index)
    {
        RemoveVisualDelegates();
        selectedCollection = collections[index];
        ShowCollection(selectedCollection);
    }

    private void ShowCollection(VisualDelegateCollection collection)
    {
        CanvasVisibilityHelper(parameterCanvasGroup, true);

        foreach (Transform child in visualDelegateUIGridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (VisualDelegate vd in collection.visualDelegates)
        {
            DelegateItemUI item = Instantiate(itemPrefab, visualDelegateUIGridParent);

            item.Setup(vd);
        }
    }

    public void ApplyVisualDelegates()
    {
        if (selectedCollection == null)
            return;

        foreach (VisualDelegate vd in selectedCollection.visualDelegates)
        {
            VisualDelegate runtimeCopy = Instantiate(vd);

            runtimeCopy.AddVisualDelegateToTraining();

            if (!activeVisualDelegates.Contains(runtimeCopy))
            {
                activeVisualDelegates.Add(runtimeCopy);
            }
        }
    }

    public void RemoveVisualDelegates()
    {
        foreach (VisualDelegate vd in activeVisualDelegates)
        {
            vd.RemoveVisualDelegate();
        }
        activeVisualDelegates.Clear();
    }

    public void TestRunSwitch()
    {
        if (testRun == false) 
        {
            testRun = true;
        }
        else
        {
            testRun = false;
        }
    }

    public void InfiniteTimeSwitch()
    {
        if (trainingTimeAmountSlider.gameObject.activeInHierarchy)
        {
            //sind im endless modus
            trainingTimeAmountSlider.gameObject.SetActive(false);
            infiniteTime = true;
        }
        else 
        {
            //sind im zeit modus
            trainingTimeAmountSlider.gameObject.SetActive(true);
            infiniteTime = false;
        }
    }
    private void UpdateTimeText(float value, TMP_Text targetText)
    {
        int totalSeconds = Mathf.RoundToInt(value);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        targetText.text = $"{minutes:00}:{seconds:00}";
    }
    public void StartUserTesting()
    {
        ApplyVisualDelegates();
        Time.timeScale = 1;

        CanvasVisibilityHelper(parameterCanvasGroup, false);
        CanvasVisibilityHelper(visualDelegateMenuCanvasGroup, false);
        CanvasVisibilityHelper(endTrainingCanvas, true);

        visualDelegateManager.visualDelegates = activeVisualDelegates;

        if(testRun == true)
        {
            researchDataCollector.CreateCSV("TEST", selectedCollection.collectionName, activeVisualDelegates);
        }
        else
        {
            researchDataCollector.CreateCSV(participantIdentifier.text, selectedCollection.collectionName, activeVisualDelegates);
        }
            

        if (!infiniteTime)
        {
            trainingTimer.StartTimer(trainingTimeAmountSlider.value);
        }
    }   
    private void CanvasVisibilityHelper(CanvasGroup targetCanvas, bool visible)
    {
        if (visible)
        {
            targetCanvas.alpha = 1f;
            targetCanvas.interactable = true;
            targetCanvas.blocksRaycasts = true;
        }
        if(!visible)
        { 
            targetCanvas.alpha = 0f;
            targetCanvas.interactable = false;
            targetCanvas.blocksRaycasts = false;
        }
    }
    public void EndUserTesting()
    {
        Application.Quit();
    }
}