using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "VisualDelegate/PostProcessing/FilmGrain")]
public class FilmGrainVisualDelegate : PostProcessingVisualDelegate<FilmGrain>
{
    [Header("Film Grain Parameters")]
    [SerializeField] private FilmGrainLookup type = FilmGrainLookup.Medium1;
    [SerializeField, Range(0f, 1f)] private float intensity = 0;
    [SerializeField, Range(0f, 1f)] private float response = 0;

    protected override void ApplySettings()
    {
        effect.type.value = type;
        effect.intensity.value = intensity;
        effect.response.value = response;
    }

    public override float GetChangingParameter()
    {
        return effect.intensity.value;
    }
    public override void SetChangingParameter(float value)
    {
        effect.intensity.value = value;
    }
}
