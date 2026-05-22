using UnityEngine;
using UnityEngine.Rendering;

public abstract class PostProcessingVisualDelegate<T> : VisualDelegate where T : VolumeComponent
{
    protected T effect;

    protected Volume GetVolume()
    {
        return GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<Volume>();
    }

    protected virtual void EnsureEffect()
    {
        Volume volume = GetVolume();

        if (!volume.profile.TryGet(out effect))
        {
            effect = volume.profile.Add<T>();
            effect.SetAllOverridesTo(true);
        }
        else
        {
            effect.active = true;
        }
    }

    public override void AddVisualDelegateToTraining()
    {
        EnsureEffect();
        ApplySettings();
    }

    public override void RemoveVisualDelegate()
    {
        if (effect != null)
        {
            effect.active = false;
        }
    }

    protected abstract void ApplySettings();
}