using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;

namespace VirtualMind.DrawTool
{
    public class Drawer : MonoBehaviour
    {
        [SerializeField, Layer, Tooltip("The layer the drawing camera is currently looking at (default by default)")]
        public int m_CameraLayer = 0;

        public GraphicRaycaster m_Raycaster;
        PointerEventData m_PointerEventData;
        Canvas canvas;

        public RectTransform drawImage;
        public DrawSpaceManagement drawSpaceManagement;
        public Transform renderersParent { get { return drawSpaceManagement.renderersParent; } }

        public int NumberOfVisualElement { get { return numberOfBrushElement; } }
        private int numberOfBrushElement = 1;

        private Vector2 drawCanvasSize;
        public GameObject brushPrefab;

        [Tooltip("The default ColorPicker. Not mandatory to be attributed")]
        public ColorPicker colorPicker;

        [SerializeField, Tooltip("The visual to make the button selected. Not mandatory to be attributed")]
        protected Transform selectedBrushVisual;

        [Tooltip("Max graphic element in the scene before making a Save of the texture and destroying them. This number has an influence on the ctrl z historic")]
        public int MaxGraphicElementAtTheSameTime = 200;
        public Color colorPicked = Color.black;
        public float brushWidth = 0.2f, brushOpacity;

        private Brush currentBrush;
        private Transform brushTransform;
        private bool wasPressed = false;
        private string textString = "";
        public TMP_FontAsset fontAsset;

        private class ObjTrace
        {
            public GameObject go;
            public DateTime creationDate;
            public ObjTrace(GameObject go, DateTime creationDate)
            {
                this.go = go;
                this.creationDate = creationDate;
            }
        }
        private List<ObjTrace> traces = new List<ObjTrace>();

        private void Awake()
        {
            drawSpaceManagement.SetCanvasSize(drawImage.sizeDelta);
            drawCanvasSize = drawSpaceManagement.background.transform.lossyScale;
            if (colorPicker != null) colorPicker.ForceColor(colorPicked);
            canvas = m_Raycaster.GetComponent<Canvas>();

        }

        private void OnEnable()
        {
            if (currentBrush == null)
            {
                SetBrush(brushPrefab);
            }
        }

        private void Undo()
        {
            if (traces.Count == 0) return;

            var lastDate = traces[traces.Count - 1].creationDate;
            for (int i = traces.Count - 1; i >= 0; i--)
            {
                var trace = traces[i];
                if ((lastDate - trace.creationDate).Seconds < 0.5f)
                {
                    Destroy(trace.go);
                }
                else
                {
                    int numberElementToRemove = traces.Count - i - 1;
                    traces.RemoveRange(i + 1, numberElementToRemove);
                    numberOfBrushElement -= numberElementToRemove;
                    return;
                }
            }
            numberOfBrushElement = 1;
            traces.Clear();
        }

        public void ResetColor()
        {
            ForceColor(Color.white);
        }

        public void ForceColor(Color color)
        {
            SetColor(color);
            if (colorPicker != null)
                colorPicker.ForceColor(colorPicked);
        }

        public void SetColor(Color color)
        {
            colorPicked = color;
            if (currentBrush != null)
                currentBrush.SetColor(new Color(colorPicked.r, colorPicked.g, colorPicked.b, brushOpacity));
        }

        public void SetWidth(float width)
        {
            brushWidth = width;
            if (currentBrush != null)
                currentBrush.SetWidth(Vector3.one * brushWidth);
        }

        public void SetOpacity(float opacity)
        {
            brushOpacity = opacity;
            if (currentBrush != null)
                currentBrush.SetColor(new Color(colorPicked.r, colorPicked.g, colorPicked.b, brushOpacity));
        }

        public void SetBrush(GameObject brushPrefab)
        {
            if (currentBrush != null)
            {
                Destroy(currentBrush.gameObject);
            }
            currentBrush = Instantiate(brushPrefab, drawSpaceManagement.background.transform.parent).GetComponent<Brush>();
            currentBrush.gameObject.layer = m_CameraLayer;
            brushTransform = currentBrush.transform;
            SetWidth(brushWidth);
            SetOpacity(brushOpacity);
            currentBrush.SetText(textString);
            currentBrush.SetTextFont(fontAsset);
        }

        public void SetSprite(Sprite sprite)
        {
            if (currentBrush != null)
            {
                Destroy(currentBrush.gameObject);
            }
            var go = new GameObject("Brush");
            go.transform.SetParent(drawSpaceManagement.background.transform.parent);
            go.transform.localRotation = Quaternion.identity;
            go.layer = m_CameraLayer;
            go.AddComponent<SpriteRenderer>().sprite = sprite;
            currentBrush = go.AddComponent<SpriteBrush>();
            brushTransform = currentBrush.transform;
            SetWidth(brushWidth);
            SetColor(colorPicked);
        }

        public void SetTextFont(TMP_FontAsset fontAsset)
        {
            this.fontAsset = fontAsset;
            if (currentBrush != null)
                currentBrush.SetTextFont(fontAsset);
        }

        public void SetText(string txt)
        {
            textString = txt;
            if (currentBrush != null)
                currentBrush.SetText(txt);
        }

        public void HighlightButton(Button button)
        {
            if (selectedBrushVisual == null) return;
            selectedBrushVisual.SetParent(button.transform, true);
            selectedBrushVisual.localPosition = Vector3.zero;
        }

        public void AddDrawElement(GameObject element)
        {
            traces.Add(new ObjTrace(element, DateTime.Now));
            numberOfBrushElement++;
            if (numberOfBrushElement > MaxGraphicElementAtTheSameTime)
            {
                SaveTexture();
            }
        }

        void Update()
        {
            brushTransform.gameObject.SetActive(true);

            m_PointerEventData = new PointerEventData(EventSystem.current);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            if (results.Count > 0 && results[0].gameObject == drawImage.gameObject)
            {
                RectTransform rectTransform = drawImage;

                Vector3 position;

                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint);

                    float xPosRatio = (localPoint.x - rectTransform.rect.x) / rectTransform.rect.width;
                    float xPos = (xPosRatio - 0.5f) * drawCanvasSize.x;
                    float yPosRatio = (localPoint.y - rectTransform.rect.y) / rectTransform.rect.height;
                    float yPos = (yPosRatio - 0.5f) * drawCanvasSize.y;

                    position = new Vector3(xPos, yPos);
                }
                else
                {
                    var bounds = RectTransformToScreenSpace(rectTransform);
                    float xPosRatio = (Input.mousePosition.x - bounds.min.x) / bounds.size.x;
                    float xPos = (xPosRatio - 0.5f) * drawCanvasSize.x;
                    float yPosRatio = (Input.mousePosition.y - bounds.min.y) / bounds.size.y;
                    float yPos = (yPosRatio - 0.5f) * drawCanvasSize.y;

                    position = new Vector3(xPos, yPos);
                }

                brushTransform.transform.localPosition = position - 2 * Vector3.forward;

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (!wasPressed)
                    {
                        wasPressed = true;
                        currentBrush.OnClick(this, position);
                    }
                    currentBrush.OnPressedUpdate(this, position);
                }
                else if (wasPressed)
                {
                    wasPressed = false;
                    currentBrush.OnRelease(this, position);
                }

                if (Input.mouseScrollDelta.y != 0)
                {
                    currentBrush.OnScrollWheel(Input.mouseScrollDelta.y);
                }

            }
            else
            {
                brushTransform.gameObject.SetActive(false);
            }
        }

        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            return new Rect((Vector2)transform.position - (size * 0.5f), size);
        }

        // Frequently save the texture to remove all the renderers and just create a texture showing the current draw
        void SaveTexture()
        {
            numberOfBrushElement = 1;
            RenderTexture.active = drawSpaceManagement.renderTexture;
            Texture2D tex = new Texture2D(drawSpaceManagement.renderTexture.width, drawSpaceManagement.renderTexture.height, TextureFormat.RGB565, false);
            tex.ReadPixels(new Rect(0, 0, drawSpaceManagement.renderTexture.width, drawSpaceManagement.renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
            Material mat = drawSpaceManagement.background.material;
            mat.mainTexture = tex;
            drawSpaceManagement.background.material = mat;
            for (int i = 0; i < drawSpaceManagement.renderersParent.transform.childCount; i++)
            {
                Destroy(drawSpaceManagement.renderersParent.transform.GetChild(i).gameObject);
            }
            traces.Clear();
        }
    }



    public class LayerAttribute : PropertyAttribute
    {
        // NOTHING - just oxygen.
    }


    [CustomPropertyDrawer(typeof(LayerAttribute))]
    class LayerAttributeEditor : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // One line of  oxygen free code.
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }

    }
}