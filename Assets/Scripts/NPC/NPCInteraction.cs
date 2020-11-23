using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour {

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

    //
    public bool activateNPCTrophy;
    bool NPCTrophyActivated;

    void Start() {
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

        //
        if (PlayerPrefs.GetInt("Language") == 1) //russian
        {
            speech.font = TextTranslationManager.singleton.cyrillicFont;
            speech.fontSize = 100;
        }
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

                //
                if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Chinese)
                {
                    speech.font = TextTranslationManager.singleton.chineseFont;
                    speech.fontSize = 110;
                }
                else
                if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Japanese)
                {
                    speech.font = TextTranslationManager.singleton.japaneseFont;
                    speech.fontSize = 110;
                }
                else
                if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Korean)
                {
                    speech.font = TextTranslationManager.singleton.koreanFont;
                    speech.fontSize = 110;
                }
                else
                if (PlayerPrefs.GetInt("Language") == (int)LANGUAGES.Russian)
                {
                    speech.font = TextTranslationManager.singleton.cyrillicFont;
                    speech.fontSize = 110;
                }
                else
                if (PlayerPrefs.GetInt("Language") == 7 )
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

        //
        speech.text = "";
        if (speaking)
        {
            ExitText();
            EnterText(true);
        }
    }

	void LateUpdate(){
        if (debug && Input.GetKeyDown(KeyCode.L))
        {
            textID++;
            if (textID >= 81)
                textID = 0;

            Debug.Log(textID + " " + TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, textID, currentLanguage).Length);
        }

        if (pivotObj != null && Camera.main != null) {
            pivotObj.transform.LookAt(new Vector3(Camera.main.transform.position.x, pivotObj.transform.position.y, Camera.main.transform.position.z));
            pivotObj.transform.RotateAround(pivotObj.transform.position, Vector3.up, 180f);
        }

        if (textID != -1)
        {
            if (currentLanguage != PlayerPrefs.GetInt("Language") || textID != origTextID)
            {
                currentLanguage = PlayerPrefs.GetInt("Language");
                origTextID = textID;
                UpdateText(TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, textID, currentLanguage));
            }
        }
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player" && !speaking) {

            if (textID <= 4 && textID != -1)
                UpdateText(TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, textID, currentLanguage));
            EnterText(false);
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player" && speaking) {
            ExitText();
		}
	}

    //
    public void EnterText(bool wait)
    {
        speaking = true;
        anim.SetBool("npcActivate", true);
        StartCoroutine("Interaction", wait);
        speech.text = "";

        //
        if (activateNPCTrophy && !NPCTrophyActivated)
        {
            NPCTrophyActivated = true;

#if UNITY_PS4
            //
            // check maze npc trophy
            //
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.MEET_THE_LOST_VILLAGER);
#endif


#if UNITY_XBOXONE
            //
            // check trophy : items >= 3 and items = all 
            //
            PlayerPrefs.SetInt("IsThere", 1);
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.IS_THERE_ANYONE); 
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

	IEnumerator Interaction(bool wait){
        if (wait)
            yield return null;
        talking = true;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("BubbleOn"))
			yield return null;
		StartCoroutine ("PlayVoice");
		for (int t = 0; t < fullText.Length; t++) {
			speech.text = speech.text + fullText [t];
            if (textSpeed == 0f)
				yield return null;
			else
				yield return new WaitForSeconds (textSpeed);
		}
        talking = false;
    }

	IEnumerator PlayVoice(){
		sound.Stop ();
		sound.pitch = Random.Range (0.8f, 1.2f);
		sound.PlayDelayed (0f);
		if (timeBetweenClips == 0f)
			yield return new WaitForSeconds (sound.clip.length);
		else
			yield return new WaitForSeconds (timeBetweenClips);
		if(talking)	
			StartCoroutine ("PlayVoice");
	}
}
