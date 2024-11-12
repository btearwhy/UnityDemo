using TMPro;
using UnityEngine;

namespace VirtualMind.DrawTool
{
    public abstract class Brush : MonoBehaviour
    {
        public virtual void OnClick(Drawer drawer, Vector3 screenPosition) { }

        public virtual void OnPressedUpdate(Drawer drawer, Vector3 screenPosition)
        {
            var go = Instantiate(gameObject, drawer.renderersParent.transform);
            go.transform.localPosition = screenPosition - drawer.NumberOfVisualElement * 0.01f * Vector3.forward;
            drawer.AddDrawElement(go);
        }

        public virtual void OnScrollWheel(float wheel)
        {
            transform.Rotate(wheel * 4 * Vector3.forward);
        }

        public virtual void OnRelease(Drawer drawer, Vector3 screenPosition) { }

        public virtual void SetColor(Color color) { }

        public virtual void SetWidth(Vector3 size)
        {
            transform.localScale = size;
        }

        public virtual void SetTextFont(TMP_FontAsset fontAsset) { }

        public virtual void SetText(string txt) { }
    }
}
