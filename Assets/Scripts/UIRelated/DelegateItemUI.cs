using TMPro;
using UnityEngine;

public class DelegateItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text vdFrameText;

    public void Setup(VisualDelegate vd)
    {
        nameText.text = vd.visualDelegateName;

        vdFrameText.text =
            $"Depth: {vd.depthLevel}\n" +
            $"Abstraction: {vd.abstractionLevel}\n" +
            $"Control: {vd.controlLevel}";
    }
}