using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateItemsMasks : MonoBehaviour
{

    public int itemID;
    int promptID;

    public GameObject itemonwoodle;
    public GameObject priceObj;
    Text priceText;
    public GameObject icon;
    public GameObject bought;
    // Use this for initialization

    bool touchedThis;

    public bool isAMask;
    public bool isAnItem;
    public bool isAHat;

    public TPC tpc;
    public MainMarket mainMarket;
    int curLang;

    Font originalFont;
    FontStyle originalFontStyle;

    public int itemsBoughtCount;
    public bool threeItemsTrophy;
    public bool allItemsTrophy;

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

        tpc = PlayerManager.GetMainPlayer();

        if (!PlayerPrefs.HasKey("PaidForItem" + itemID.ToString()))
            PlayerPrefs.SetInt("PaidForItem" + itemID.ToString(), 0);
        else
        {
            if (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString(), 0) == 1)
            {
                if (PlayerPrefs.GetInt("UsingItem" + itemID.ToString(), 0) == 1)
                {
                    itemonwoodle.SetActive(true);
                    touchedThis = true;
                    if (isAMask)
                        tpc.wearingMask = true;
                    if (isAnItem)
                        tpc.holdingItem = true;
                    if (isAHat)
                        tpc.wearingHat = true;
                    icon.SetActive(false);
                    bought.SetActive(true);
                    tpc.ToggleAbility(itemID, true);
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
            ItemPromptManager.DisplayPrompt(promptID, itemID, null, null, this, (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString(), 0) == 1), touchedThis);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
            ItemPromptManager.ExitPrompt(promptID);
    }

    public void TouchThis()
    {
        mainMarket.ReturnCurrentAddon((isAMask && tpc.wearingMask), (isAnItem && tpc.holdingItem), (isAHat && tpc.wearingHat));

        touchedThis = true;
        if (isAMask)
        {
            tpc.wearingMask = true;
            mainMarket.SetCurrentID(itemID, 0);
        }
        if (isAnItem)
        {
            tpc.holdingItem = true;
            mainMarket.SetCurrentID(itemID, 1);
        }
        if (isAHat)
        {
            tpc.wearingHat = true;
            mainMarket.SetCurrentID(itemID, 2);
        }

        tpc.ToggleAbility(itemID, true);

        this.GetComponent<AudioSource>().Play();

        itemonwoodle.SetActive(true);

        icon.SetActive(false);
        bought.SetActive(true);

        PlayerPrefs.SetInt("UsingItem" + itemID.ToString(), 1);

        if (PlayerPrefs.GetInt("PaidForItem" + itemID.ToString(), 0) == 0)
        {
            PlayerPrefs.SetInt("PaidForItem" + itemID.ToString(), 1);

            if (itemID != 24)
            {
                int amountBought = PlayerPrefs.GetInt("BoughtItems", 0) + 1;
                PlayerPrefs.SetInt("BoughtItems", amountBought);

                if (amountBought >= 3 && !threeItemsTrophy)
                {
                    threeItemsTrophy = true;
#if UNITY_XBOXONE
                    XONEAchievements.SubmitAchievement((int)XONEACHIEVS.GO_SHOPPING);
#endif
                }

                if (amountBought >= 17 && !allItemsTrophy)
                {
                    allItemsTrophy = true;
#if UNITY_XBOXONE
                    XONEAchievements.SubmitAchievement((int)XONEACHIEVS.GO_SHOPPING_FOR_EVERYTHING);
#endif
                }
            }

            SetPriceText();
            Transform t = this.transform;
            while (t.gameObject.GetComponent<UnlockItemMarket>() == null && t.parent != null)
                t = t.parent;
            if (t.gameObject.GetComponent<UnlockItemMarket>() != null)
                t.gameObject.GetComponent<UnlockItemMarket>().DisplayIt();
        }
    }

    public void UntouchThis()
    {
        touchedThis = false;

        if (isAMask)
            tpc.wearingMask = false;
        if (isAnItem)
            tpc.holdingItem = false;
        if (isAHat)
            tpc.wearingHat = false;

        tpc.ToggleAbility(itemID, false);

        itemonwoodle.SetActive(false);

        bought.SetActive(false);
        icon.SetActive(true);

        PlayerPrefs.SetInt("UsingItem" + itemID.ToString(), 0);
        //    PlayerPrefs.Save();
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

        if (itemID == 22)   //Auto Windball
            promptID = 20;
        if (itemID == 23)   //Auto Windball
            promptID = 21;

        if (itemID == 24)   //Blue Berry
            promptID = 22;

    }
}
