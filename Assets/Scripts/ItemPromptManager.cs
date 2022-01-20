using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ItemPromptManager : MonoBehaviour
{
    [System.Serializable] public class MarketItemInfo { public int itemID; public int price; public bool useBlueBerries; public Text priceText; }
    public MarketItemInfo[] marketItems;

    static ItemPromptManager singleton;

    public Animator animator;

    public Text promptTitle;
    public Text promptInfo;
    public Image promptIcon;

    public Text yesText;
    public Text noText;

    public Image priceIcon;
    public Sprite redBerry;
    public Sprite blueBerry;
    public Text priceText;

    Player input;
    TPC tpc;
    CameraFollower camF;

    [System.Serializable] public class ItemPrompt { public string title; [TextArea] public string information; public Sprite icon; }
    public List<ItemPrompt> itemPrompts;

    int debugID;
    bool delay;
    int berryCount;

    MarketItemInfo currentMI;

    ActivateItemsLeafs aiLeafs;
    ActivateBonusAbility aiBonuses;
    ActivateItemsMasks aiMasks;

    public AudioSource sound;
    public AudioClip openPromptClip;
    public AudioClip tooExpensiveClip;
    public AudioClip buyClip;
    public AudioClip declineClip;
    public AudioClip equipClip;

    [HideInInspector] public bool displaying;
    int currentID;
    Dictionary<int, bool> inPromptTrigger;

    public enum YesText { buy, equip, unequip, cont };
    YesText curYestText;

    Font soldFont;
    Font titleFont;
    Font infoFont;
    Font yesFont;
    Font noFont;
    Font priceFont;

    FontStyle soldFontStyle;
    FontStyle titleFontStyle;
    FontStyle infoFontStyle;
    FontStyle yesFontStyle;
    FontStyle noFontStyle;
    FontStyle priceFontStyle;

    void Awake()
    {
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }
        singleton = this;

        inPromptTrigger = new Dictionary<int, bool>(itemPrompts.Count);
        input = ReInput.players.GetPlayer(0);

        soldFont = marketItems[0].priceText.font;
        titleFont = promptTitle.font;
        infoFont = promptInfo.font;
        yesFont = yesText.font;
        noFont = noText.font;
        priceFont = priceText.font;

        soldFontStyle = marketItems[0].priceText.fontStyle;
        titleFontStyle = promptTitle.fontStyle;
        infoFontStyle = promptInfo.fontStyle;
        yesFontStyle = yesText.fontStyle;
        noFontStyle = noText.fontStyle;
        priceFontStyle = priceText.fontStyle;
    }

    private void Start()
    {
        singleton._UpdateAllStatuses();
    }

    int CheckForPurchasedAblities()
    {
        int blueBerriesSpent = 0;

        if (PlayerPrefs.GetInt("PaidForItem" + 18) == 1)
        {
            blueBerriesSpent += 290;
        }

        if (PlayerPrefs.GetInt("PaidForItem" + 19) == 1)
        {
            blueBerriesSpent += 110;
        }

        if (PlayerPrefs.GetInt("PaidForItem" + 20) == 1)
        {
            blueBerriesSpent += 70;
        }

        if (PlayerPrefs.GetInt("PaidForItem" + 21) == 1)
        {
            blueBerriesSpent += 350;
        }

        Debug.Log("Blue Berries spent by the player are : " + blueBerriesSpent);

        return blueBerriesSpent;
    }

    public int CheckBlueBerriesCount()
    {
        //CONTROLLA SE LE ABILTA' SONO STATE COMPRATE
        int blueBerriesSpent = CheckForPurchasedAblities();

        int blueBerriesTotal = PlayerPrefs.GetInt("BlueBerryTotal");

        int blueBerriesOwnedByThePlayer = PlayerPrefs.GetInt("BlueBerries");

        int blueBerriesTotalLessSpent = blueBerriesTotal - blueBerriesSpent;

        if (blueBerriesOwnedByThePlayer > blueBerriesTotalLessSpent)
        {
            blueBerriesOwnedByThePlayer = blueBerriesTotalLessSpent;
            PlayerPrefs.SetInt("BlueBerries", blueBerriesOwnedByThePlayer);
        }

        return blueBerriesOwnedByThePlayer;
    }

    public static ItemPromptManager GetItemPromptManager()
    {
        return singleton;
    }

    private void Update()
    {
        if (delay)
        {
            delay = false;
            currentID = -1;
            displaying = false;
            tpc.inShop = false;
            //    tpc.inCutscene = false;
            //    tpc.disableControl = false;
            //    tpc.rb.isKinematic = false;
            //    camF.disableControl = false;
            //    Time.timeScale = 1f;
        }

        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Prompt Continue") || animator.GetCurrentAnimatorStateInfo(0).IsName("Prompt Collectable")) && input.GetButtonDown("Submit"))
        {
            if (curYestText == YesText.buy)
            {
                if (currentMI.useBlueBerries)
                    berryCount = PlayerPrefs.GetInt("BlueBerries", 0);
                else
                    berryCount = PlayerPrefs.GetInt("Berries", 0);
                
                if (berryCount < currentMI.price || (currentMI.price == -1 && PlayerPrefs.GetInt("AllBlueBerries", 0) == 0))
                {
                    animator.SetTrigger("tooExpensive");
                    PlaySound(tooExpensiveClip);
                }
                else
                {
                    if (aiLeafs)
                        aiLeafs.TouchThis();
                    if (aiBonuses)
                        aiBonuses.TouchThis();
                    if (aiMasks)
                        aiMasks.TouchThis();

                    if (currentMI.useBlueBerries)
                    {
                        tpc.blueberryCount -= currentMI.price;
                        tpc.blueberryHUD.SetTrigger("bounceText");
                        tpc.blueberryText.text = tpc.blueberryCount.ToString();
                        PlayerPrefs.SetInt("BlueBerries", tpc.blueberryCount);
                    }
                    else
                    {
                        tpc.berryCount -= currentMI.price;
                        tpc.berryHUD.SetTrigger("bounceText");
                        tpc.berryText.text = tpc.berryCount.ToString();
                        PlayerPrefs.SetInt("Berries", tpc.berryCount);
                    }

                    animator.SetBool("boughtIt", true);
                    animator.SetBool("promptOn", false);
                    animator.SetTrigger("nextState");
                    tpc.berryHUD.SetBool("function", false);
                    tpc.blueberryHUD.SetBool("function", false);
                    delay = true;
                    inPromptTrigger[currentID] = false;
                    PlaySound(buyClip);
                }
            }
            if(curYestText == YesText.equip)
            {
                if (aiLeafs)
                    aiLeafs.TouchThis();
                if (aiBonuses)
                    aiBonuses.TouchThis();
                if (aiMasks)
                    aiMasks.TouchThis();

                PlaySound(equipClip);
                _ExitPrompt(currentID);
            }
            if (curYestText == YesText.unequip)
            {
                if (aiLeafs)
                    aiLeafs.UnTouchThis();
                if (aiBonuses)
                    aiBonuses.UnTouchThis();
                if (aiMasks)
                    aiMasks.UntouchThis();

                PlaySound(equipClip);
                _ExitPrompt(currentID);
            }
            if (curYestText == YesText.cont)
            {
                PlaySound(equipClip);
                _ExitPrompt(currentID);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Prompt Continue") && input.GetButtonDown("Back"))
        {
            PlaySound(declineClip);
            _ExitPrompt(currentID);
        }
    }

    public static bool IsDisplaying()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return false;
        }

        return singleton._IsDisplaying();
    }

    public bool _IsDisplaying()
    {
        return displaying;
    }

    public static void ExitPrompt(int promptID)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        singleton._ExitPrompt(promptID);
    }
    
    public void _ExitPrompt(int promptID)
    {
        animator.SetBool("boughtIt", false);
        animator.SetBool("promptOn", false);
        animator.SetTrigger("nextState");
        tpc.berryHUD.SetBool("function", false);
        tpc.blueberryHUD.SetBool("function", false);
        inPromptTrigger[promptID] = false;
        delay = true;
    }

    public static void DisplayPrompt(int promptID, int itemID, ActivateItemsLeafs aiLeaf, ActivateBonusAbility aiBonus, ActivateItemsMasks aiMask, bool bought, bool touched)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        if(promptID >= singleton.itemPrompts.Count)
        {
            Debug.LogError("Prompt ID " + promptID.ToString() +" doesn't exist.");
            return;
        }

        singleton._DisplayPrompt(promptID, itemID, aiLeaf, aiBonus, aiMask, bought, touched);
    }

    void _DisplayPrompt(int promptID, int itemID, ActivateItemsLeafs aiLeaf, ActivateBonusAbility aiBonus, ActivateItemsMasks aiMask, bool bought, bool touched)
    {
        inPromptTrigger[promptID] = true;
        StartCoroutine(WaitToDisplay(promptID, itemID, aiLeaf, aiBonus, aiMask, bought, touched));
    }

    public static void UpdateAllStatuses()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._UpdateAllStatuses();
    }

    void _UpdateAllStatuses()
    {
        foreach (MarketItemInfo mi in marketItems)
            UpdateItem(mi);
    }

    void UpdateItem(MarketItemInfo mi)
    {
        if(PlayerPrefs.GetInt("PaidForItem" + mi.itemID.ToString(), 0) == 1)
        {
            mi.priceText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 2, PlayerPrefs.GetInt("Language"));
        }
        else
        {
            if (mi.price == -1)
                mi.priceText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 3, PlayerPrefs.GetInt("Language"));
            else
                mi.priceText.text = mi.price.ToString();
        }

        if (PlayerPrefs.GetInt("Language") == 11)
        {
            mi.priceText.font = TextTranslationManager.singleton.arabicFont;
            mi.priceText.fontStyle = FontStyle.Bold;
        }
        else
        {
            mi.priceText.font = soldFont;
            mi.priceText.fontStyle = soldFontStyle;
        }
    }

    void PlaySound(AudioClip clip)
    {
        sound.Stop();
        sound.clip = clip;
        sound.PlayDelayed(0f);
    }

    IEnumerator WaitToDisplay(int promptID, int itemID, ActivateItemsLeafs aiLeaf, ActivateBonusAbility aiBonus, ActivateItemsMasks aiMask, bool bought, bool touched)
    {
        while (displaying)
            yield return null;

        if (inPromptTrigger[promptID] == true)
        {
            displaying = true;
            animator.ResetTrigger("nextState");

            if (tpc == null)
            {
                tpc = PlayerManager.GetMainPlayer();
                camF = tpc.cam.GetComponent<CameraFollower>();
            }
            tpc.inShop = true;

            //   Time.timeScale = 0f;
            //   tpc.inCutscene = true;
            //   tpc.disableControl = true;
            //   tpc.rb.isKinematic = true;
            //   camF.disableControl = true;

            yesText.fontSize = 40;
            promptTitle.fontSize = 300;
            promptInfo.fontSize = 300;

            promptTitle.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, (promptID*2)+4, PlayerPrefs.GetInt("Language"));
            if (promptID == 6 && PlayerPrefs.GetInt("Language") == 9)
                promptTitle.fontSize = 200;
            else
                promptTitle.fontSize = (int)Mathf.Lerp(300, 150, Mathf.Clamp01((promptTitle.text.Length - 14) / 5f));
            
            promptInfo.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, (promptID*2)+5, PlayerPrefs.GetInt("Language"));
            if (PlayerPrefs.GetInt("Language") == 1)
                promptInfo.fontSize = 280;
            else
                promptInfo.fontSize = 300;

            if (PlayerPrefs.GetInt("Language") == 11)
            {
                promptInfo.alignment = TextAnchor.MiddleRight;
                if (promptInfo.lineSpacing > 0f)
                    promptInfo.lineSpacing *= -1f;
            }
            else
            {
                promptInfo.alignment = TextAnchor.MiddleLeft;
                if (promptInfo.lineSpacing < 0f)
                    promptInfo.lineSpacing *= -1f;
            }

            promptIcon.sprite = itemPrompts[promptID].icon;

            if (bought)
            {
                if (touched)
                {
                    yesText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 53, PlayerPrefs.GetInt("Language"));
                    if (PlayerPrefs.GetInt("Language") == 8)
                        yesText.fontSize = 35;
                    else
                        yesText.fontSize = 40;

                    curYestText = YesText.unequip;
                }
                else
                {
                    yesText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 52, PlayerPrefs.GetInt("Language"));
                    curYestText = YesText.equip;
                }
                noText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 54, PlayerPrefs.GetInt("Language"));
            }
            else
            {
                yesText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 50, PlayerPrefs.GetInt("Language"));
                noText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 51, PlayerPrefs.GetInt("Language"));
                curYestText = YesText.buy;
            }
            
            if (aiLeaf == null && aiBonus == null && aiMask == null)
            {
                yesText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 1, PlayerPrefs.GetInt("Language"));
                if (PlayerPrefs.GetInt("Language") == 1)
                    yesText.fontSize = 35;
                else
                    yesText.fontSize = 40;
                curYestText = YesText.cont;
                noText.transform.parent.gameObject.SetActive(false);
                priceText.transform.parent.parent.gameObject.SetActive(false);
                animator.SetBool("isCollectable", true);
            }
            else
            {
                if (!noText.transform.parent.gameObject.activeInHierarchy)
                    noText.transform.parent.gameObject.SetActive(true);
                animator.SetBool("isCollectable", false);
            }

            aiLeafs = aiLeaf;
            aiBonuses = aiBonus;
            aiMasks = aiMask;

            foreach (MarketItemInfo mi in marketItems)
            {
                if (mi.itemID == itemID)
                    currentMI = mi;
            }

            if (curYestText != YesText.cont && currentMI != null)
            {
                if (!bought)
                {
                    priceText.transform.parent.parent.gameObject.SetActive(true);
                    if (currentMI.useBlueBerries)
                    {
                        tpc.blueberryText.text = tpc.blueberryCount.ToString();
                        tpc.blueberryHUD.SetBool("function", true);
                        priceIcon.sprite = blueBerry;
                    }
                    else
                    {
                        tpc.berryText.text = tpc.berryCount.ToString();
                        tpc.berryHUD.SetBool("function", true);
                        priceIcon.sprite = redBerry;
                    }
                    if (currentMI.price == -1)
                    {
                        priceText.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.itemPrompts, 3, PlayerPrefs.GetInt("Language"));
                    }
                    else
                        priceText.text = currentMI.price.ToString();
                }
                else
                    priceText.transform.parent.parent.gameObject.SetActive(false);
            }

            if (PlayerPrefs.GetInt("Language") == 11)
            {
                promptTitle.font = TextTranslationManager.singleton.arabicFont;
                promptInfo.font = TextTranslationManager.singleton.arabicFont;
                yesText.font = TextTranslationManager.singleton.arabicFont;
                noText.font = TextTranslationManager.singleton.arabicFont;
                priceText.font = TextTranslationManager.singleton.arabicFont;

                promptTitle.fontStyle = FontStyle.Bold;
                promptInfo.fontStyle = FontStyle.Bold;
                yesText.fontStyle = FontStyle.Bold;
                noText.fontStyle = FontStyle.Bold;
                priceText.fontStyle = FontStyle.Bold;
            }
            else
            {
                promptTitle.font = titleFont;
                promptInfo.font = infoFont;
                yesText.font = yesFont;
                noText.font = noFont;
                priceText.font = priceFont;

                promptTitle.fontStyle = titleFontStyle;
                promptInfo.fontStyle = infoFontStyle;
                yesText.fontStyle = yesFontStyle;
                noText.fontStyle = noFontStyle;
                priceText.fontStyle = priceFontStyle;
            }

            PlaySound(openPromptClip);
            animator.SetBool("promptOn", true);
        }
    }
}