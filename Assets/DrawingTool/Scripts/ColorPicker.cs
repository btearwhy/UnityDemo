using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VirtualMind.DrawTool
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField] protected Slider hueSlider, satSlider, valueSlider;
        [SerializeField] protected Image colorPreview;

        protected Color color;

        public UnityEvent<Color> onColorChanged = new UnityEvent<Color>();

        protected float hue, sat, value;

        protected bool preventEvent = false;

        public void SetHue(float hue)
        {
            float sat, value;
            Color.RGBToHSV(color, out this.hue, out sat, out value);
            this.hue = hue;
            UpdateColor(Color.HSVToRGB(this.hue, this.sat, this.value));
        }

        public void SetSaturation(float sat)
        {
            float hue, value;
            Color.RGBToHSV(color, out hue, out this.sat, out value);
            this.sat = sat;
            UpdateColor(Color.HSVToRGB(this.hue, this.sat, this.value));
        }

        public void SetValue(float value)
        {
            float sat, hue;
            Color.RGBToHSV(color, out hue, out sat, out this.value);
            this.value = value;
            UpdateColor(Color.HSVToRGB(this.hue, this.sat, this.value));
        }

        private void UpdateColor(Color color)
        {
            this.color = color;
            colorPreview.color = color;
            onColorChanged.Invoke(color);
            valueSlider.SetValueWithoutNotify(value);
            satSlider.SetValueWithoutNotify(sat);
            hueSlider.SetValueWithoutNotify(hue);
        }

        public void ForceColor(Color color)
        {
            Color.RGBToHSV(color, out hue, out sat, out value);
            UpdateColor(color);
        }
    }
}