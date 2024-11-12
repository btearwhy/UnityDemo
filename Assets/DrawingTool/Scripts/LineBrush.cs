using UnityEngine;

namespace VirtualMind.DrawTool
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LineBrush : Brush
    {
        [SerializeField]
        private Material lineRendererMaterial;
        private SpriteRenderer spriteRenderer;
        private LineRenderer lineRenderer;
        private int capPrecision = 3;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void OnClick(Drawer drawer, Vector3 screenPosition)
        {
            var go = new GameObject("LineRenderer");
            go.layer = drawer.m_CameraLayer;
            go.transform.SetParent(drawer.renderersParent.transform, false);
            go.transform.localRotation = Quaternion.identity;
            go.transform.localPosition = -drawer.NumberOfVisualElement * 0.01f * Vector3.forward;
            lineRenderer = go.AddComponent<LineRenderer>();
            lineRenderer.colorGradient = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey(spriteRenderer.color, 0) }, alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(spriteRenderer.color.a, 0) } };
            lineRenderer.startWidth = lineRenderer.endWidth = spriteRenderer.transform.localScale.x / 6;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = false;
            lineRenderer.numCapVertices = capPrecision;
            lineRenderer.material = lineRendererMaterial;
            lineRenderer.SetPosition(0, screenPosition);
        }

        public override void OnPressedUpdate(Drawer drawer, Vector3 screenPosition)
        {
            lineRenderer.SetPosition(1, screenPosition);
        }

        public override void OnRelease(Drawer drawer, Vector3 screenPosition)
        {
            drawer.AddDrawElement(lineRenderer.gameObject);
            lineRenderer = null;
        }

        public override void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }

        public override void SetWidth(Vector3 size)
        {
            spriteRenderer.transform.localScale = size;
            capPrecision = (int)Mathf.Ceil(size.x * 3);
        }

        public override void OnScrollWheel(float wheel) { }
    }
}