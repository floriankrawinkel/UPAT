using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "VisualDelegate/PostProcessing/Vignette")]
public class VignetteVisualDelegate : PostProcessingVisualDelegate<Vignette>
{
    [Header("Vignette Parameters")]
    [SerializeField] private colors colorPresets = colors.BLACK;
    [SerializeField] private Vector2 center = new Vector2(0.5f, 0.5f);
    [SerializeField, Range(0f, 1f)] private float intensity = 0f;
    [SerializeField, Range(0f, 1f)] private float smoothness = 1f;
    [SerializeField] private bool isRounded = false;

    private enum colors
    {
        BLACK,
        RED,
        BLUE,
        GREEN
    }

    private Color GetColorFromPreset(colors preset)
    {
        switch (preset)
        {
            case colors.BLACK:
                return Color.black;
            case colors.RED:
                return Color.red;
            case colors.BLUE:
                return Color.blue;
            case colors.GREEN:
                return Color.green;
            default:
                return Color.black;
        }
    }
   
    protected override void ApplySettings()
    {
        effect.color.value = GetColorFromPreset(colorPresets);
        effect.center.value = center;
        effect.intensity.value = intensity;
        effect.smoothness.value = smoothness;
        effect.rounded.value = isRounded;
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