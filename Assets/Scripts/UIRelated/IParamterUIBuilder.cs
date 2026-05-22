using System;

public interface IParameterUIBuilder
{
    void AddSlider(string label, float min, float max, float value, Action<float> onChanged);
    void AddToggle(string label, bool value, Action<bool> onChanged);
    void AddDropdown(string label, string[] options, int index, Action<int> onChanged);
}
