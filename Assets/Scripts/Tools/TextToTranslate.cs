using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextToTranslate : MonoBehaviour
{
    [System.Serializable] public class TextElement { public Text textObject; public TextMesh textMesh; public TextTranslationManager.TextCollection textCollection; public int textID; public string addedText; public bool dontIncludeAutoSpace; public bool makeUpperCase;
        [HideInInspector] public Font originalFont; [HideInInspector] public FontStyle originalFontStyle;
    }
    public TextElement[] textElements;
    int currentLanguage = 0;
    bool firstSet;

    int debugCount;
    int index;

    void Start()
    {
        currentLanguage = PlayerPrefs.GetInt("Language", 0);
        firstSet = true;

        foreach (TextElement t in textElements)
        {
            if (t.textObject != null)
            {
                t.originalFont = t.textObject.font;
                t.originalFontStyle = t.textObject.fontStyle;
            }
            else
            {
                t.originalFont = t.textMesh.font;
                t.originalFontStyle = t.textMesh.fontStyle;
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            debugCount++;
            if (debugCount > 14)
                debugCount = 0;
            PlayerPrefs.SetInt("Language", debugCount);
        }

        if (TextTranslationManager.singleton != null)
        {
            if (firstSet || currentLanguage != PlayerPrefs.GetInt("Language", 0))
            {
                if (firstSet)
                    firstSet = false;
                else
                    currentLanguage = PlayerPrefs.GetInt("Language", 0);

                SetText();
            }
        }
    }

    void SetText()
    {
        index = 0;
        foreach (TextElement t in textElements)
        {
            SetTextElement(t.textObject, t.textMesh, t.textCollection, t.textID, t.addedText, t.dontIncludeAutoSpace, t.makeUpperCase, t.originalFont, t.originalFontStyle);
            index++;
        }
    }

    public void SetTextElement(Text textObject, TextMesh textMesh, TextTranslationManager.TextCollection textCollection, int textID, string addedText, bool dontIncludeAutoSpace, bool makeUpperCase, Font originalFont, FontStyle originalFontStyle)
    {
        if (textObject != null)
        {
            textObject.text = TextTranslationManager.GetText(textCollection, textID, currentLanguage);
            if (addedText != "")
            {
                if (!dontIncludeAutoSpace)
                    textObject.text += " ";
                textObject.text += addedText;
            }
            if (makeUpperCase)
                textObject.text = textObject.text.ToUpper();

            if (this.gameObject.name == "Pause Screen" && index > 26 && index < 34)
                textObject.fontSize = (int)Mathf.Lerp(300, 200, Mathf.Clamp01((textObject.text.Length - 10) / 5f));

            if (textCollection == TextTranslationManager.TextCollection.itemPrompts && textID == 1)
            {
                if (PlayerPrefs.GetInt("Language") == 1)
                    textObject.fontSize = 150;
                else
                    textObject.fontSize = 300;
            }

            if (textCollection == TextTranslationManager.TextCollection.cutscene)
            {
                if (textID <= 21)
                {
                    if (PlayerPrefs.GetInt("Language") == 11)
                    {
                        textObject.alignment = TextAnchor.UpperRight;
                        if (textObject.lineSpacing > 0f)
                            textObject.lineSpacing *= -1f;
                    }
                    else
                    {
                        textObject.alignment = TextAnchor.UpperLeft;
                        if (textObject.lineSpacing < 0f)
                            textObject.lineSpacing *= -1f;
                    }
                }
            }

            if (textCollection == TextTranslationManager.TextCollection.startMenu && textID == 13 && textObject.gameObject.name == "TutorialTextRotCam")
            {
                if (PlayerPrefs.GetInt("Language") == 5 || PlayerPrefs.GetInt("Language") == 13)
                    textObject.fontSize = 100;
                else
                    textObject.fontSize = 130;
            }

            if (PlayerPrefs.GetInt("Language") == 11)
            {
                textObject.font = TextTranslationManager.singleton.arabicFont;
                textObject.fontStyle = FontStyle.Bold;
            }
            else
            if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Chinese)
            {
                textObject.font = TextTranslationManager.singleton.chineseFont;
                textObject.fontStyle = FontStyle.Bold;
            }
            else
            if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Japanese)
            {
                textObject.font = TextTranslationManager.singleton.japaneseFont;
                textObject.fontStyle = FontStyle.Bold;
            }
            else
            if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Korean)
            {
                textObject.font = TextTranslationManager.singleton.koreanFont;
                textObject.fontStyle = FontStyle.Bold;
            }
            else
            if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Russian)
            {
                textObject.font = TextTranslationManager.singleton.cyrillicFont;
                textObject.fontStyle = FontStyle.Bold;
            }
            else
            {
                textObject.font = originalFont;
                textObject.fontStyle = originalFontStyle;
            }
        }
        if (textMesh != null)
        {
            textMesh.text = TextTranslationManager.GetText(textCollection, textID, currentLanguage);
            if (addedText != "")
            {
                if (!dontIncludeAutoSpace)
                    textMesh.text += " ";
                textMesh.text += addedText;
            }
            if (makeUpperCase)
                textMesh.text = textMesh.text.ToUpper();

            if (PlayerPrefs.GetInt("Language") == 11)
            {
                textMesh.font = TextTranslationManager.singleton.arabicFont;
                textMesh.fontStyle = FontStyle.Bold;
            }
            else
            {
                textMesh.font = originalFont;
                textMesh.fontStyle = originalFontStyle;
            }
        }
    }
}
