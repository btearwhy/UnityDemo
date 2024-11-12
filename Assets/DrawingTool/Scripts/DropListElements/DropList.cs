using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace VirtualMind.UI
{
    public class DropList<T> : MonoBehaviour
    {
        protected enum ApparitionDirection
        {
            Down, Up, Right, Left
        }
        [Tooltip("A value of 0 equal the default size of the content")]
        [SerializeField] protected Vector2 maxSize = Vector2.zero;
        [SerializeField] protected ApparitionDirection apparitionDirection;

        [SerializeField] protected GameObject choicePrefab;
        [SerializeField] protected RectTransform dropListParent, dropListPopup;
        [SerializeField] protected TextMeshProUGUI text;

        protected bool isOpen = false;

        public UnityEvent<T> OnElementSelected;

        private void Start()
        {
            InitializeDropList();
            dropListPopup.sizeDelta = new Vector2(dropListPopup.sizeDelta.x, 0);
            if (maxSize.x == 0) maxSize.x = float.MaxValue;
            if (maxSize.y == 0) maxSize.y = float.MaxValue;
        }

        protected virtual void InitializeDropList()
        {
        }

        public void OnDropListClicked()
        {
            if (isOpen)
            {
                CloseList();
            }
            else
            {
                OpenList();
            }
        }

        public virtual void ElementSelected(DropListElement obj)
        {
            text.text = obj.GetText();
            CloseList();
        }

        protected void CloseList()
        {
            isOpen = false;
            dropListPopup.gameObject.SetActive(false);
        }

        protected void OpenList()
        {
            switch (apparitionDirection)
            {
                case ApparitionDirection.Down:
                    dropListPopup.pivot = new Vector2(0.5f, 1);
                    dropListPopup.localPosition = transform.localPosition;
                    dropListPopup.localPosition += -Vector3.up * (transform as RectTransform).sizeDelta.y * 0.5f;
                    break;
                case ApparitionDirection.Up:
                    dropListPopup.pivot = new Vector2(0.5f, 0);
                    dropListPopup.localPosition = transform.localPosition;
                    dropListPopup.localPosition = Vector3.up * (transform as RectTransform).sizeDelta.y * 0.5f;
                    break;
                case ApparitionDirection.Right:
                    dropListPopup.pivot = new Vector2(0, 0.5f);
                    dropListPopup.localPosition = transform.localPosition;
                    dropListPopup.localPosition = Vector3.right * (transform as RectTransform).sizeDelta.x * 0.5f;
                    break;
                case ApparitionDirection.Left:
                    dropListPopup.pivot = new Vector2(1, 0.5f);
                    dropListPopup.localPosition = transform.localPosition;
                    dropListPopup.localPosition = -Vector3.right * (transform as RectTransform).sizeDelta.x * 0.5f;
                    break;
            }
            dropListPopup.localPosition += Vector3.forward * -0.3f;
            isOpen = true;
            dropListPopup.gameObject.SetActive(true);
            float realMaxHeight = Mathf.Min(dropListParent.sizeDelta.y, maxSize.y);
            float realMaxWidth = Mathf.Min(dropListParent.sizeDelta.x, maxSize.x);
            dropListPopup.sizeDelta = new Vector2(realMaxWidth, realMaxHeight);
        }
    }
}