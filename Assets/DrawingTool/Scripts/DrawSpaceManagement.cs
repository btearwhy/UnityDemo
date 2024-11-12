using UnityEngine;

namespace VirtualMind.DrawTool
{
    public class DrawSpaceManagement : MonoBehaviour
    {
        public Renderer background;
        public Camera cam;
        public Transform renderersParent;
        public RenderTexture renderTexture;

        public int RenderTextureSizeRatio = 10;


        public void SetCanvasSize(Vector2 size)
        {
            background.transform.localScale = new Vector3(2 * size.x / size.y, 2, 1);
            renderTexture.width = (int)size.x * RenderTextureSizeRatio;
            renderTexture.height = (int)size.y * RenderTextureSizeRatio;
            cam.aspect = size.x / size.y;
        }
    }
}