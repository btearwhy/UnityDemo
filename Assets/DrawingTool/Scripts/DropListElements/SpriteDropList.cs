using UnityEngine;
using UnityEngine.UI;

namespace VirtualMind.UI
{
    public class SpriteDropList : DropList<Sprite>
    {
        public Sprite[] listElement;

        public Image img;

        protected override void InitializeDropList()
        {
            foreach (var item in listElement)
            {
                var go = Instantiate(choicePrefab, dropListParent);
                var dropListElement = go.GetComponent<DropListElement>();
                dropListElement.onClick.AddListener((el) => ElementSelected(el));
                dropListElement.sprite = item;
                go.GetComponent<Image>().sprite = item;
            }
        }

        public override void ElementSelected(DropListElement obj)
        {
            img.sprite = obj.sprite;
            CloseList();
            OnElementSelected.Invoke(img.sprite);
        }
    }
}
