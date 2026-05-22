using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "VisualDelegate/PostProcessing/Bloom")]
public class BloomVisualDelegate : PostProcessingVisualDelegate<Bloom>
{
    [Header("Parameter")]
    [SerializeField] private float threshold = 0.9f;
    [SerializeField] private float intensity = 0f;
    [SerializeField, Range(0, 1)] private float scatter = 0.7f;

    public override float GetChangingParameter()
    {
        return effect.intensity.value;
    }

    public override void SetChangingParameter(float value)
    {
        effect.intensity.value = value;
    }

    protected override void ApplySettings()
    {
        effect.threshold.value = threshold;
        effect.intensity.value = intensity;
        effect.scatter.value = scatter;
    }
}
