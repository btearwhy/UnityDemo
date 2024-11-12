using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace VirtualMind.UI
{
    public class DropListElement : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Sprite sprite;
        [HideInInspector]
        public UnityEvent<DropListElement> onClick;

        public TextMeshProUGUI textElement { get { return text; } }

        public void OnClick()
        {
            onClick.Invoke(this);
        }

        public virtual void SetText(string text)
        {
            this.text.text = text;
        }

        public virtual string GetText()
        {
            return text.text;
        }
    }
}