using UnityEngine;
using System.Collections;
//using Steamworks;

public class ActivateItemsLeafs : MonoBehaviour {

	public int itemID;
    int promptID;

    public GameObject woodleleaf;
	public Material leafmaterial;
	public Material defaultLeafmaterial;

	public GameObject windball;
	public GameObject priceObj;
    public float windballspeed;

	public GameObject icon;
	public GameObject bought;

	bool touchedThis;
	public TPC tpc;
	int thisLeafNo;

	public ActivateItemsLeafs otherRedLeaf;
	public ActivateItemsLeafs otherYellowLeaf;
	public ActivateItemsLeafs otherWhiteLeaf;
    int curLang;

    int itemsBoughtCount;
    int abiliesCount;

    Font originalFont;
    FontStyle originalFontStyle;

    void Start()
    {
        originalFont = priceObj.GetComponent<TextMesh>().font;
        originalFontStyle = priceObj.GetComponent<TextMesh>().fontStyle;

        if (leafmaterial.name == "WoodleLeafYellow")
			thisLeafNo = 2;
		
		if (leafmaterial.name == "WoodleLeafRed")
			thisLeafNo = 3;

		if (leafmaterial.name == "WoodleLeafBlue")
			thisLeafNo = 4;

        //  Debug.Log("A "+ itemID +": "+ PlayerPrefs.GetInt("PaidForItem" + itemID.ToString()) +" . "+ PlayerPrefs.GetInt("UsingItem" + itemID.ToString()));

        //
        if (!PlayerPrefs.HasKey("PaidItemsCount"))
        {
            PlayerPrefs.SetInt("PaidItemsCount", 0);
            itemsBoughtCount = 0;
        }
        else
            itemsBoughtCount = PlayerPrefs.GetInt("PaidItemsCount");

        //
        if (!PlayerPrefs.HasKey ("PaidForItem" + itemID.ToString ()))
			PlayerPrefs.SetInt ("PaidForItem" + itemID.ToString (), 0);
		else {
			if (PlayerPrefs.GetInt ("PaidForItem" + itemID.ToString (), 0) == 1) {
				if (PlayerPrefs.GetInt ("UsingItem" + itemID.ToString (), 0) == 1) {
					touchedThis = true;
					icon.SetActive (false);
					bought.SetActive (true);
                    StartCoroutine("WaitASec");
				}
			}
        }

        ConvetForItemPrompt();

        curLang = PlayerPrefs.GetInt("Language", 0);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString()) == 1)
        {
            if (curLang != PlayerPrefs.GetInt("Language"))
            {
                curLang = PlayerPrefs.GetInt("Language");
                SetPriceText();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
            ItemPromptManager.DisplayPrompt(promptID, itemID, this, null, null, (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString(), 0) == 1), touchedThis);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
            ItemPromptManager.ExitPrompt(promptID);
    }

    //
    bool threeItemsTrophy, allItemsTrophy;
    //
    public void TouchThis()
    {
     //   Debug.Log("B " + itemID + ": " + PlayerPrefs.GetInt("PaidForItem" + itemID.ToString()) + " . " + PlayerPrefs.GetInt("UsingItem" + itemID.ToString()));
        touchedThis = true;
		this.GetComponent<AudioSource> ().Play ();

		// SteamUserStats.SetAchievement("Leaf weapon");

		ActivateAbility (false);

		icon.SetActive (false);
		bought.SetActive (true);

		PlayerPrefs.SetInt ("UsingItem" + itemID.ToString (), 1);

        if (!PlayerPrefs.HasKey("PaidItemsCount"))
             PlayerPrefs.SetInt("PaidItemsCount", itemsBoughtCount);
        else
            itemsBoughtCount = PlayerPrefs.GetInt("PaidItemsCount");

        if (PlayerPrefs.GetInt ("PaidForItem" + itemID.ToString (), 0) == 0) {
			PlayerPrefs.SetInt ("PaidForItem" + itemID.ToString (), 1);
            SetPriceText();
            Transform t = this.transform;
			while (t.gameObject.GetComponent<UnlockItemMarket> () == null && t.parent != null)
				t = t.parent;
			if (t.gameObject.GetComponent<UnlockItemMarket> () != null)
				t.gameObject.GetComponent<UnlockItemMarket> ().DisplayIt ();
		}

        //
        itemsBoughtCount++;
        PlayerPrefs.SetInt("PaidItemsCount", itemsBoughtCount);
        //


#if UNITY_PS4
        //
        // check trophy : items >= 3 and items = all 
        //
        if (itemsBoughtCount >= 3 && !threeItemsTrophy)
        {
            threeItemsTrophy = true;
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.BUY_3_ITEMS_AT_THE_SHOP);
        }

        if (itemsBoughtCount >= 17 && !allItemsTrophy)
        {
            allItemsTrophy = true;
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.BUY_ALL_ITEMS_AT_THE_SHOP);
        }
#endif

#if UNITY_XBOXONE
        //
        // check trophy : items >= 3 and items = all 
        //
        if (itemsBoughtCount >= 3 && !threeItemsTrophy)
        {
            threeItemsTrophy = true;
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.GO_SHOPPING);
        }

        if (itemsBoughtCount >= 17 && !allItemsTrophy)
        {
            allItemsTrophy = true;
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.GO_SHOPPING_FOR_EVERYTHING);
        }
#endif

    }

    void SetPriceText()
    {
        priceObj.GetComponent<TextMesh>().text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 2, curLang);
        if (curLang == 1 || curLang == 2 || curLang == 3 || curLang == 6 || curLang == 7 || curLang == 8 || curLang == 9 || curLang == 10 || curLang == 11 || curLang == 12)
            priceObj.GetComponent<TextMesh>().fontSize = 50;
        else
            priceObj.GetComponent<TextMesh>().fontSize = 80;
        if (curLang == 11)
        {
            priceObj.GetComponent<TextMesh>().font = TextTranslationManager.singleton.arabicFont;
            priceObj.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
        }
        else
        {
            priceObj.GetComponent<TextMesh>().font = originalFont;
            priceObj.GetComponent<TextMesh>().fontStyle = originalFontStyle;
        }
    }

    public void UnTouchThis(){
		touchedThis = false;

		DeactivateAbility ();

		bought.SetActive (false);
		icon.SetActive (true);

		PlayerPrefs.SetInt ("UsingItem" + itemID.ToString (), 0);
    //    PlayerPrefs.Save();
    }

	void ActivateAbility(bool firstCheck){
		if (!firstCheck && tpc.leafNo != 1) {
			if (tpc.leafNo == 2)
				otherYellowLeaf.UnTouchThis ();
			if (tpc.leafNo == 3)
				otherRedLeaf.UnTouchThis ();
			if (tpc.leafNo == 4)
				otherWhiteLeaf.UnTouchThis ();
		}

		woodleleaf.GetComponent<Renderer> ().material = leafmaterial;

        tpc.leafNo = thisLeafNo;
        tpc.leafAS.attackAmount = thisLeafNo;
        tpc.SetDefaultLeafMat();
	}

	void DeactivateAbility(){
		woodleleaf.GetComponent<Renderer> ().material = defaultLeafmaterial;
		tpc.leafNo = 1;

        tpc.SetDefaultLeafMat();
    }

    IEnumerator WaitASec()
    {
        yield return null;
        yield return null;
        ActivateAbility(true);
    }

    void ConvetForItemPrompt()
    {
        if (itemID == 0)    //Chubby Pixel
            promptID = 6;
        if (itemID == 1)    //Elder
            promptID = 7;
        if (itemID == 2)    //Goofy
            promptID = 8;
        if (itemID == 3)    //Flower
            promptID = 9;
        if (itemID == 4)    //Shield
            promptID = 10;
        if (itemID == 5)    //Umbrella
            promptID = 11;
        if (itemID == 6)    //Acorn
            promptID = 12;
        if (itemID == 7)    //Elf
            promptID = 13;
        if (itemID == 8)    //Bandana
            promptID = 14;

        if (itemID == 9)    //Red
            promptID = 4;
        if (itemID == 10)   //White
            promptID = 5;
        if (itemID == 11)   //Yellow
            promptID = 3;

        if (itemID == 13)   //Festive
            promptID = 15;
        if (itemID == 14)   //Pumpkin
            promptID = 16;
        if (itemID == 15)   //Ghost
            promptID = 17;

        if (itemID == 18)   //Triple Jump
            promptID = 0;
        if (itemID == 19)   //Leaf Slide
            promptID = 1;
        if (itemID == 20)   //Auto Windball
            promptID = 2;
        if (itemID == 21)   //Ultra Windball
            promptID = 19;
    }
}
