using UnityEngine;
using UnityEngine.Rendering;

public abstract class VisualDelegate : ScriptableObject
{
    [Header("Visual Delegate Name")]
    public string visualDelegateName;

    [Header("VD-Frame Categories")]
    public depth depthLevel;
    public abstraction abstractionLevel;
    public control controlLevel;

    [Header("Fade In- & Out Target Value")]
    public float fadeInTargetValue;
    public float fadeOutTargetValue;

    public enum depth
    {
        NONE,
        MENU,
        ENV,
        REF,
        CHAR,
        FIL
    }
    public enum abstraction
    {
        NONE,
        DESC,
        SYM,
        NAT
    }
    public enum control
    {
        NONE,
        NOCO,
        ACT,
        MOD
    }
    public abstract void AddVisualDelegateToTraining();
    public abstract void RemoveVisualDelegate();
    public virtual void ChangeParameterValueOverTime(bool fadeIn, VisualDelegateManager visualDelegateManager)
    {
        float target = fadeIn ? fadeInTargetValue : fadeOutTargetValue;
        float duration = fadeIn ? visualDelegateManager.fadeInTime : visualDelegateManager.fadeOutTime;

        visualDelegateManager.StartVisualDelegateFade(this, target, duration);
    }
    public abstract float GetChangingParameter();
    public abstract void SetChangingParameter(float value);
}