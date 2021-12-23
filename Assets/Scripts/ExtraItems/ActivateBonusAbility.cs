using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateBonusAbility : MonoBehaviour
{

    public int itemID;
    int promptID;

    public int abilityID; //0 for triple jump, 1 for leaf slide, 2 for supercharge
    public TPC tpc;

    public GameObject icon;
    public GameObject bought;
    public GameObject priceObj;
    Text priceText;

    bool touchedThis;
    int curLang;

    Font originalFont;
    FontStyle originalFontStyle;

    public int abiliesCount;
    public bool allPowerupsTrophy;


    private void Awake()
    {
        foreach (Transform t in priceObj.GetComponentsInChildren<Transform>(true))
        {
            if (t.gameObject.GetComponent<Text>() != null)
                priceText = t.gameObject.GetComponent<Text>();
        }
        originalFont = priceText.font;
        originalFontStyle = priceText.fontStyle;
    }

    void Start()
    {

        if (!PlayerPrefs.HasKey("PaidForItem" + itemID.ToString()))
        {
            PlayerPrefs.SetInt("PaidForItem" + itemID.ToString(), 0);
            //    PlayerPrefs.Save();
        }
        else
        {
            if (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString(), 0) == 1)
            {
                if (PlayerPrefs.GetInt("UsingItem" + itemID.ToString(), 0) == 1)
                {
                    ActivateAbility();
                    touchedThis = true;
                    icon.SetActive(false);
                    bought.SetActive(true);
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
            ItemPromptManager.DisplayPrompt(promptID, itemID, null, this, null, (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString()) == 1), touchedThis);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
            ItemPromptManager.ExitPrompt(promptID);
    }

    public void TouchThis()
    {
        touchedThis = true;

        ActivateAbility();

        icon.SetActive(false);
        bought.SetActive(true);

        PlayerPrefs.SetInt("UsingItem" + itemID.ToString(), 1);
        //    PlayerPrefs.Save();

        if (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString()) == 0)
        {
            PlayerPrefs.SetInt("PaidForItem" + itemID.ToString(), 1);

            int amountBought = PlayerPrefs.GetInt("BoughtPowers", 0) + 1;
            PlayerPrefs.SetInt("BoughtPowers", amountBought);
            if (amountBought >= 4)
            {
#if UNITY_PS4

        //
        // check trophy : powerUps == 4
        if (abiliesCount >= 4 && !allPowerupsTrophy)
        {
            allPowerupsTrophy = true;
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.GET_ALL_POWER_UPS);
        }
#endif

#if UNITY_XBOXONE
        //
        // check trophy musicians if all cages are 1 countedPrefs = 4
        if (abiliesCount >= 4 && !allPowerupsTrophy)
        {
            allPowerupsTrophy = true;
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_POWER);
        }
#endif
            }

            SetPriceText();
            //    PlayerPrefs.Save();
            Transform t = this.transform;
            while (t.gameObject.GetComponent<UnlockItemMarket>() == null && t.parent != null)
                t = t.parent;
            if (t.gameObject.GetComponent<UnlockItemMarket>() != null)
                t.gameObject.GetComponent<UnlockItemMarket>().DisplayIt();
        }
    }

    void SetPriceText()
    {
        priceText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 2, curLang);
        /*    if (curLang == 1 || curLang == 2 || curLang == 3 || curLang == 6 || curLang == 7 || curLang == 8 || curLang == 9 || curLang == 10 || curLang == 11 || curLang == 12)
                priceText.fontSize = 50;
            else
                priceText.fontSize = 80;*/
        if (curLang == 11)
        {
            priceText.font = TextTranslationManager.singleton.arabicFont;
            priceText.fontStyle = FontStyle.Bold;
        }
        else
        {
            priceText.font = originalFont;
            priceText.fontStyle = originalFontStyle;
        }
    }

    public void UnTouchThis()
    {
        touchedThis = false;

        bought.SetActive(false);
        icon.SetActive(true);

        PlayerPrefs.SetInt("UsingItem" + itemID.ToString(), 0);
        //    PlayerPrefs.Save();

        DeactivateAbility();
    }

    void ActivateAbility()
    {
        if (abilityID == 0)
            tpc.hasTripleJump = true;
        if (abilityID == 1)
            tpc.hasLeafSlide = true;
        if (abilityID == 2)
            tpc.hasSuperLeaf = true;
        if (abilityID == 3)
            tpc.hasUltraLeaf = true;
    }

    void DeactivateAbility()
    {
        if (abilityID == 0)
            tpc.hasTripleJump = false;
        if (abilityID == 1)
            tpc.hasLeafSlide = false;
        if (abilityID == 2)
            tpc.hasSuperLeaf = false;
        if (abilityID == 3)
            tpc.hasUltraLeaf = false;
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
