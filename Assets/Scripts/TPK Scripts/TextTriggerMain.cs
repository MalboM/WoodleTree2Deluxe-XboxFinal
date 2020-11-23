using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTriggerMain : MonoBehaviour {

    public float textSpeed;
    [HideInInspector] public int textID;
    Animator anim;
    Text theText;

    int currentLanguage;

    int[] nextTextIDs = new int[16];
    [HideInInspector] public bool currentlyDisplaying;
    int cycle;

    public GameObject miniDrawParent;
    public Image miniDrawImage;

    [System.Serializable] public class TextMiniDraw { public int textID; public Sprite miniDraw; }
    public TextMiniDraw[] textMiniDraws;
    int curMiniDraw;

    Font originalFont;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        theText = this.transform.Find("Image").Find("Text").gameObject.GetComponent<Text>();

        originalFont = theText.font;

        cycle = 0;
        int ni = 0;
        while (ni < nextTextIDs.Length){
            nextTextIDs[ni] = -1;
            ni++;
        }
    }

    int tempy = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetText(tempy);
            tempy++;
            if (tempy >= 14)
                tempy = 0;
        }
    }

    public void SetText(int curID)
    {
        if(curID < TextTranslationManager.singleton.textPrompts.Length) {
            if (!currentlyDisplaying){
                currentlyDisplaying = true;
                textID = curID;
                theText.text = "";
                curMiniDraw = -1;
                for(int check = 0; check < textMiniDraws.Length; check++)
                {
                    if (textMiniDraws[check].textID == curID)
                        curMiniDraw = check;
                }
                StartCoroutine(PlayAnimation(curMiniDraw));
            }
            else {
                int ni = 0;
                bool found = false;
                while (ni < nextTextIDs.Length && !found) {
                    if (nextTextIDs[ni] == -1){
                        nextTextIDs[ni] = curID;
                        found = true;
                    }else
                        ni++;
                }
                StartCoroutine(WaitToPlay());
            }
        }
    }

    IEnumerator PlayAnimation(int curMiniDraw) {
        anim.SetBool("Activate", true);
        if (curMiniDraw != -1)
        {
            miniDrawParent.SetActive(true);
            miniDrawImage.sprite = textMiniDraws[curMiniDraw].miniDraw;
            yield return null;
            miniDrawImage.sprite = null;
            yield return null;
            miniDrawImage.sprite = textMiniDraws[curMiniDraw].miniDraw;
        }
        else
            miniDrawParent.SetActive(false);
        yield return new WaitForSeconds(1f);
        string fullText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.textPrompts, textID, PlayerPrefs.GetInt("Language"));
        if (PlayerPrefs.GetInt("Language") == 9)
            theText.fontSize = 50;
        else
            theText.fontSize = 60;
        if (PlayerPrefs.GetInt("Language") == 11)
        {
            theText.alignment = TextAnchor.UpperRight;
            if (theText.lineSpacing > 0f)
                theText.lineSpacing *= -1f;
            //
            theText.font = TextTranslationManager.singleton.arabicFont;
        }
        else
        {
            //
            if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Chinese)
            {
                theText.font = TextTranslationManager.singleton.chineseFont;
                theText.fontStyle = FontStyle.Bold;
                theText.alignment = TextAnchor.UpperLeft;
                if (theText.lineSpacing < 0f)
                    theText.lineSpacing *= -1f;
            }
            else
              if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Japanese)
            {
                theText.font = TextTranslationManager.singleton.japaneseFont;
                theText.fontStyle = FontStyle.Bold;
                theText.alignment = TextAnchor.UpperLeft;
                if (theText.lineSpacing < 0f)
                    theText.lineSpacing *= -1f;
            }
            else
                if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Korean)
            {
                theText.font = TextTranslationManager.singleton.koreanFont;
                theText.fontStyle = FontStyle.Bold;
                theText.alignment = TextAnchor.UpperLeft;
                if (theText.lineSpacing < 0f)
                    theText.lineSpacing *= -1f;
            }
            else
              if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Russian)
            {
                theText.font = TextTranslationManager.singleton.cyrillicFont;
                theText.alignment = TextAnchor.UpperLeft;
                if (theText.lineSpacing < 0f)
                    theText.lineSpacing *= -1f;
            }
            else
            {
                theText.alignment = TextAnchor.UpperLeft;
                if (theText.lineSpacing < 0f)
                    theText.lineSpacing *= -1f;
                theText.font = originalFont;
            }
        }
        for (int t = 0; t < fullText.Length; t++){
            theText.text = theText.text + fullText[t];
            while (Time.timeScale == 0f)
                yield return null;
            if (textSpeed == 0f)
                yield return null;
            else
                yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(fullText.Length * 0.06f);
        anim.SetBool("Activate", false);
        yield return new WaitForSeconds(1.3f);
        currentlyDisplaying = false;
    }

    IEnumerator WaitToPlay() {
        while (currentlyDisplaying)
            yield return null;
        int ni = cycle;
        int checker = 0;
        bool found = false;
        while (!found && checker <= nextTextIDs.Length){
            if (nextTextIDs[ni] != -1){
                SetText(nextTextIDs[ni]);
                nextTextIDs[ni] = -1;
                found = true;
            }
            else{
                ni++;
                if (ni >= nextTextIDs.Length)
                    ni = 0;
                cycle = ni;
            }
            checker++;
        }
        ni = 0;
        found = false;
        while (ni < nextTextIDs.Length && !found){
            if (nextTextIDs[ni] == -1)
                ni++;
            else
                found = true;
        }
        if (found)
            StartCoroutine(WaitToPlay());
    }
}
