using UnityEngine;
using UnityEngine.UI;

namespace VirtualMind.DrawTool
{
    public class SpriteMemory : MonoBehaviour
    {
        [HideInInspector]
        public Sprite sprite;
        [SerializeField]
        private Image img;

        public void AssignSpriteToDrawer(Drawer drawer)
        {
            if (sprite == null)
            {
                sprite = img.sprite;
            }
            drawer.SetSprite(sprite);
        }

        public void SetSprite(Sprite sprite)
        {
            this.sprite = sprite;
            img.sprite = sprite;
        }
    }
}