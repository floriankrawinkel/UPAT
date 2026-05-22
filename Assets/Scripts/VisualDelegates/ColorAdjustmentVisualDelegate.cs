using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "VisualDelegate/PostProcessing/ColorAdjustment")]
public class ColorAdjustmentVisualDelegate : PostProcessingVisualDelegate<ColorAdjustments>
{
    [Header("Color Adjustment Parameters")]
    [SerializeField, Range(-5f, 5f)] private float postExposure;
    [SerializeField, Range(-100f, 100f)] private float contrast;
    [SerializeField] private colors colorPresets = colors.WHITE;
    [SerializeField, Range(-180f, 180f)] private float hueShift;
    [SerializeField, Range(-100f, 100f)] private float saturation;

    private enum colors
    {
        BLACK,
        RED,
        BLUE,
        GREEN,
        WHITE
    }
    protected override void ApplySettings()
    {
        effect.postExposure.value = postExposure;
        effect.contrast.value = contrast;
        effect.colorFilter.value = GetColorFromPreset(colorPresets);
        effect.hueShift.value = hueShift;
        effect.saturation.value = saturation;
    }
    public override float GetChangingParameter()
    {
        return effect.saturation.value;
    }
    public override void SetChangingParameter(float value)
    {
        effect.saturation.value = value;
    }
    private Color GetColorFromPreset(colors preset)
    {
        switch (preset)
        {
            case colors.BLACK:
                return Color.black;
            case colors.WHITE:
                return Color.white;
            case colors.RED:
                return Color.red;
            case colors.BLUE:
                return Color.blue;
            case colors.GREEN:
                return Color.green;
            default:
                return Color.white;
        }
    }
}
