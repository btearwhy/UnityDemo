using UnityEngine;
using TMPro;


namespace VirtualMind.DrawTool
{
    [RequireComponent(typeof(TextMeshPro))]
    public class TextBrush : Brush
    {
        protected TextMeshPro textMesh;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        public override void OnPressedUpdate(Drawer drawer, Vector3 screenPosition) { }

        public override void OnClick(Drawer drawer, Vector3 screenPosition)
        {
            base.OnPressedUpdate(drawer, screenPosition);
        }

        public override void SetColor(Color color)
        {
            textMesh.color = color;
        }

        public override void SetText(string txt)
        {
            textMesh.text = txt;
        }

        public override void SetTextFont(TMP_FontAsset fontAsset)
        {
            textMesh.font = fontAsset;
        }
    }
}