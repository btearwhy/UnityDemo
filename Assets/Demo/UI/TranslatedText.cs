using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslatedText : MonoBehaviour
{
    // Start is called before the first frame update

    public int languageId;
    private TMP_Text text;
    void OnEnable()
    {
        text = GetComponent<TMP_Text>();

        if (text != null)
        {
            text.text = LanguageManager.GetById(languageId);
        }
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
