using System.Collections;
using UnityEngine;

namespace VirtualMind.DrawTool
{
    public class ColorPickerBrush : Brush
    {
        Drawer drawer;

        public override void OnClick(Drawer drawer, Vector3 screenPosition)
        {
            this.drawer = drawer;
            StartCoroutine(GetScreenPixelColor());
        }

        public override void OnPressedUpdate(Drawer drawer, Vector3 screenPosition) { }

        public IEnumerator GetScreenPixelColor()
        {
            yield return new WaitForEndOfFrame();

            int x = (int)Input.mousePosition.x;
            int y = (int)Input.mousePosition.y;

            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGB24, false);

            // Read the pixels at the specified screen coordinates
            texture.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
            texture.Apply();

            drawer.ForceColor(texture.GetPixel(0, 0));
            Destroy(texture);
        }
    }
}
