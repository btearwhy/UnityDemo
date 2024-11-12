using UnityEngine;
using TMPro;
using System;

namespace VirtualMind.UI
{
    public class FontDropList : DropList<TMP_FontAsset>
    {
        [Serializable]
        public class FontAsset
        {
            public string name;
            public TMP_FontAsset fontAsset;
        }
        [Tooltip("A value of 0 equal the default size of the content")]
        [SerializeField] protected Vector2 elementSize = Vector2.zero;
        [SerializeField]
        protected FontAsset[] fontAssets;

        protected override void InitializeDropList()
        {
            foreach (var item in fontAssets)
            {
                var go = Instantiate(choicePrefab);
                var dropListElement = go.GetComponent<DropListElement>();
                var rectTransform = go.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(elementSize.x != 0 ? elementSize.x : rectTransform.sizeDelta.x, elementSize.y != 0 ? elementSize.y : rectTransform.sizeDelta.y);
                dropListElement.onClick.AddListener((el) => ElementSelected(el));
                dropListElement.SetText(item.name);
                dropListElement.textElement.font = item.fontAsset;
                go.transform.SetParent(dropListParent);
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
            }
        }

        public override void ElementSelected(DropListElement obj)
        {
            text.font = obj.textElement.font;
            OnElementSelected.Invoke(text.font);
            base.ElementSelected(obj);
        }
    }
}