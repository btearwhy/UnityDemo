using UnityEngine;

namespace VirtualMind.DrawTool
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteBrush : Brush
    {
        protected SpriteRenderer sprite;

        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        public override void SetColor(Color color)
        {
            sprite.color = color;
        }
    }
}