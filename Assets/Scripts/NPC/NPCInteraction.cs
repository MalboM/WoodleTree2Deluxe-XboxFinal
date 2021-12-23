using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{

    public bool debug;

    public Text speech;
    public int textID;
    int currentLanguage;
    int origTextID;

    public float textSpeed;
    GameObject pivotObj;
    string fullText;

    public AudioClip voiceClip;
    public float timeBetweenClips;
    int voiceIterate;

    AudioSource sound;
    Animator anim;
    GameObject focus;

    [HideInInspector] public bool speaking;
    bool talking;

    Font originalFont;
    FontStyle originalFontStyle;
    MultiPlatfromGUIManager.ControllerType origCT = MultiPlatfromGUIManager.ControllerType.mk;

    void Start()
    {
        origTextID = textID;

        sound = this.gameObject.GetComponent<AudioSource>();
        if (voiceClip != null)
            sound.clip = voiceClip;
        anim = this.gameObject.GetComponentInChildren<Animator>();
        pivotObj = this.gameObject;

        originalFont = speech.font;
        originalFontStyle = speech.fontStyle;

        if (textID != -1)
            UpdateText(TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, textID, currentLanguage));
        else
            fullText = speech.text;
    }

    public void UpdateText(string newText)
    {
        fullText = "";
        fullText = newText.ToString();

        if (PlayerPrefs.GetInt("Language") == 1)
            speech.fontSize = 100;
        else
        {
            if (PlayerPrefs.GetInt("Language") == 6 || PlayerPrefs.GetInt("Language") == 11 || PlayerPrefs.GetInt("Language") == 13)
            {
                if (newText.ToString().Length >= 100)
                    speech.fontSize = 120;
                else
                    speech.fontSize = 140;
            }
            else
            {
                if (PlayerPrefs.GetInt("Language") == 7)
                {
                    if (newText.ToString().Length >= 105)
                        speech.fontSize = 120;
                    else
                        speech.fontSize = 140;
                }
                else
                {
                    if (PlayerPrefs.GetInt("Language") == 9 || PlayerPrefs.GetInt("Language") == 14)
                    {
                        if (newText.ToString().Length >= 30)
                            speech.fontSize = 110;
                        else
                            speech.fontSize = 140;
                    }
                    else
                    {
                        if (newText.ToString().Length >= 110)
                            speech.fontSize = 120;
                        else
                            speech.fontSize = 140;
                    }
                }
            }
        }

        if (PlayerPrefs.GetInt("Language") == 11)
        {
            speech.alignment = TextAnchor.UpperRight;
            if (speech.lineSpacing > 0f)
                speech.lineSpacing *= -1f;

            speech.font = TextTranslationManager.singleton.arabicFont;
            speech.fontStyle = FontStyle.Bold;
        }
        else
        {
            speech.alignment = TextAnchor.UpperLeft;
            if (speech.lineSpacing < 0f)
                speech.lineSpacing *= -1f;

            speech.font = originalFont;
            speech.fontStyle = originalFontStyle;
        }


        speech.text = "";
        if (speaking)
        {
            ExitText();
            EnterText(true);
        }
    }

    void LateUpdate()
    {
#if UNITY_EDITOR
        if (debug && Input.GetKeyDown(KeyCode.L))
        {
            textID++;
            if (textID >= 81)
                textID = 0;

            Debug.Log(textID + " " + TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, textID, currentLanguage).Length);
        }
#endif
        if (pivotObj != null && Camera.main != null)
        {
            pivotObj.transform.LookAt(new Vector3(Camera.main.transform.position.x, pivotObj.transform.position.y, Camera.main.transform.position.z));
            pivotObj.transform.RotateAround(pivotObj.transform.position, Vector3.up, 180f);
        }

        if (textID != -1)
        {
            if (currentLanguage != PlayerPrefs.GetInt("Language") || textID != origTextID || origCT != MultiPlatfromGUIManager.singleton.curControllerType)
            {
                currentLanguage = PlayerPrefs.GetInt("Language");
                origTextID = textID;
                origCT = MultiPlatfromGUIManager.singleton.curControllerType;
                UpdateText(TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, textID, currentLanguage));
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !speaking)
        {

            if (textID <= 4 && textID != -1)
                UpdateText(TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, textID, currentLanguage));
            EnterText(false);

            if (textID == 19)
            {
//#if !UNITY_EDITOR
//                if (SteamManager.Initialized) {                
//                SteamUserStats.SetAchievement("Is Anyone There?");
//                SteamUserStats.StoreStats();
//                }
//#endif
            }

            if (textID >= 26 && textID <= 29)
            {
                PlayerPrefs.SetInt("PTree" + textID.ToString(), 1);

                if ((PlayerPrefs.GetInt("PTree26", 0) + PlayerPrefs.GetInt("PTree27", 0) + PlayerPrefs.GetInt("PTree28", 0) + PlayerPrefs.GetInt("PTree29", 0)) == 4)
                {
#if !UNITY_EDITOR
            //    if (SteamManager.Initialized) {                
            //        SteamUserStats.SetAchievement("Wisdom Is Everything");
            //        SteamUserStats.StoreStats();
            //    }
#endif
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && speaking)
        {
            ExitText();
        }
    }

    public void EnterText(bool wait)
    {
        speaking = true;
        anim.SetBool("npcActivate", true);
        StartCoroutine("Interaction", wait);
        speech.text = "";
    //    if (activateNPCTrophy && !NPCTrophyActivated)
        {
     //       NPCTrophyActivated = true;

#if UNITY_PS4
            //
            // check maze npc trophy
            //
          //  PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.MEET_THE_LOST_VILLAGER);
#endif


#if UNITY_XBOXONE
            //
            // check trophy : items >= 3 and items = all 
            //
        //    PlayerPrefs.SetInt("IsThere", 1);
       //     XONEAchievements.SubmitAchievement((int)XONEACHIEVS.IS_THERE_ANYONE); 
#endif
        }
    }

    public void ExitText()
    {
        speaking = false;
        anim.SetBool("npcActivate", false);
        StopCoroutine("Interaction");
        StopCoroutine("PlayVoice");
        speech.text = "";
        sound.Stop();
    }

    IEnumerator Interaction(bool wait)
    {
        if (wait)
            yield return null;
        talking = true;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("BubbleOn"))
            yield return null;
        StartCoroutine("PlayVoice");
        for (int t = 0; t < fullText.Length; t++)
        {
            speech.text = speech.text + fullText[t];
            if (textSpeed == 0f)
                yield return null;
            else
                yield return new WaitForSeconds(textSpeed);
        }
        talking = false;
    }

    IEnumerator PlayVoice()
    {
        sound.Stop();
        sound.pitch = Random.Range(0.8f, 1.2f);
        sound.PlayDelayed(0f);
        if (timeBetweenClips == 0f)
            yield return new WaitForSeconds(sound.clip.length);
        else
            yield return new WaitForSeconds(timeBetweenClips);
        if (talking)
            StartCoroutine("PlayVoice");
    }
}
