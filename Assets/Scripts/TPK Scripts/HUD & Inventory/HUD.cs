/// <summary>
/// CURRENT VERSION: 1.5 (Nov '15)
/// This script was originally written by Yan Dawid of Zelig Games.
/// 
/// KEY (for the annotations in this script):
/// -The one referred to as 'User' is the person who uses/edits this asset
/// -The one referred to as 'Player' is the person who plays the project build
/// -The one referred to as 'Character' is the in-game character that the player controls
/// 
/// This script is to NOT be redistributed and can only be used by the person that has purchased this asset.
/// Editing or altering this script does NOT make redistributing this script valid.
/// This asset can be used in both personal and commercial projects.
/// The user is free to edit/alter this script to their hearts content.
/// You can contact the author for support via the Zelig Games official website: http://zeliggames.weebly.com/contact.html
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

	private GamePadInputs gPI;

	[SerializeField] public Canvas hudCanvas;								//The canvas on which your HUD elements will be displayed on
	[SerializeField] private Canvas inventoryCanvas;						//The canvas on which your GUI elements will be displayed on
	[SerializeField] private EventSystem eventSys;							//The event system which the GUI buttons are using
	[SerializeField] private bool useHearts;								//Use and display HEARTS on the HUD
	[SerializeField] private bool useBars;									//Use and display any BARS on the HUD
	[SerializeField] private bool useMoney;									//Use and display MONEY on the HUD
	[SerializeField] public bool useInventory;								//Use the Inventory feature when pausing
	[HideInInspector] public bool displayHUD;								//A bool used to display the HUD

	public Image aimGUIElement;
	[SerializeField] private Image heartGUIElement;							//The GUI Image on the hudCanvas to be used for displaying all the hearts
	[System.Serializable] public class HUDBars{ public Slider slider; public bool autoRegen; 
		public float changeSpeed; public float regenSpeed; }
																			//The class HUDBars is used to create a dictionary (hudBars, below) of any amount of
																			// bars/meters you would like displayed on the hudCanvas.

	public HUDBars[] hudBars;												//The Dictionary of HUDBars that you would like displayed
	[SerializeField] private Text moneyGUIElement;							//The GUI Text on the hudCanvas to be used for displaying money
	[SerializeField] public Button hotKeyGUIElement;						//The GUI Button on the hudCanvas to be used for displaying all the hotkeys

	[SerializeField] private int maxNoOfHearts;								//The maximum number of hearts that can be acquired
	[SerializeField] private int amountOfAcquiredHearts;					//The amount of hearts that have currently been acquired (how many will be displayed)
	[SerializeField] private bool drawHeartsLeftwards;						//If you want the hearts to be drawn to the left of the GUIElement
	[SerializeField] private bool drawHeartsUpwards;						//If you want the hearts to be drawn above the GUIElement
	[SerializeField] private int heartsPerRow;								//The amount of hearts to be displayed in a single horizontal row
	[HideInInspector] private int health;									//The current health of the character (4 units per heart)
	[SerializeField] private float healthChangeSpeed;						//The speed at which hearts refill/deplete (0 for instant, higher for slower)
	[SerializeField] private float heartsXSpacing;							//How far apart should the hearts be in the horizontal axis
	[SerializeField] private float heartsYSpacing;							//How far apart should the hearts be in the vertical axis

	[HideInInspector] public int money;										//How much money the character is holding
	[SerializeField] private int maxMoney;									//The maximum amount of money the character can hold (wallet size?)
	[SerializeField] private float moneyChangeSpeed;						//The speed at which money increases/decreases

	[SerializeField] public int hotkeysAmount;						//The amount of hotkeys you would like to have displayed
	[SerializeField] private int hotkeysPerRow;								//The maximum amount of hotkeys in a single row
	[SerializeField] private bool drawHotkeysLeftwards;						//If you want the hotkeys to be drawn to the left of the GUIElement
	[SerializeField] private bool drawHotkeysUpwards;						//If you want the hotkeys to be drawn above the GUIElement
	[SerializeField] private float hotKeysXSpacing;							//How far apart should the hotkeys be in the horizontal axis
	[SerializeField] private float hotKeysYSpacing;							//How far apart should the hotkeys be in the vertical axis

	private Image[] hearts = new Image[0];									//An Array of the hearts being displayed on the hudCanvas
	[HideInInspector] public Button[] hotKeys = new Button[0];				//An Array of the hotkeys being displayed on the hudCanvas
	
	[SerializeField] private Sprite emptyHeartIcon;							//The sprite used for an empty heart
	[SerializeField] private Sprite lowHeartIcon;							//The sprite used for a one-quarter-full heart
	[SerializeField] private Sprite midHeartIcon;							//The sprite used for a half-full heart
	[SerializeField] private Sprite highHeartIcon;							//The sprite used for a three-quarter-full heart
	[SerializeField] private Sprite fullHeartIcon;							//The sprite used for a full heart


	
	private ThirdPersonController character;								//A reference to the characters ThirdPersonController.cs script
	private bool pause;

	void Start () {

		gPI = this.GetComponent<GamePadInputs> ();
		character = GameObject.Find ("Woodle Character").GetComponent<ThirdPersonController> ();

		hearts = new Image[maxNoOfHearts];
		hudCanvas.GetComponent<CanvasScaler> ().referenceResolution = new Vector2 (Screen.width, Screen.height);
		aimGUIElement.gameObject.SetActive (false);

	////Setting Bools
		displayHUD = true;

	////Deactivating the initial GUI Elements that were dragged into the inspector
		if (heartGUIElement.gameObject != null) 
			heartGUIElement.gameObject.SetActive(false);
		if (hotKeyGUIElement.gameObject != null) 
			hotKeyGUIElement.gameObject.SetActive(false);

	////Health Init
		DisplayHearts ();
		health = 0;
	//	if(useHearts)
	//		StartCoroutine (ChangeHealth (amountOfAcquiredHearts * 4));
        
	////money Init
		DisplayMoney ();

	////Error Handling
		CheckForErrors ();
	}

	void Update(){
		CheckForErrors ();

	////Pausing the game
		if (gPI.pressStart()) {
			pause = !pause;
			character.isPaused = pause;
			if(pause){
				Time.timeScale = 0f;
				if(useInventory){
		////Displaying the inventory if useInventory is true
					inventoryCanvas.gameObject.SetActive(true);
				}
			}else{
				Time.timeScale = 1f;
				if(useInventory)
					inventoryCanvas.gameObject.SetActive(false);
			}
		}
		if (!pause && eventSys.firstSelectedGameObject != null && !character.inACutscene)
			eventSys.firstSelectedGameObject = null;
	}

	/// <summary>
	/// Draws all the specified hearts on the hudCanvas
	/// </summary>
	public void DisplayHearts(){
		if (useHearts) {
			heartGUIElement.gameObject.SetActive(true);
			int h = 0;
			int hy = 0;
			int hx = 0;
			while (h < maxNoOfHearts) {
				if (h % heartsPerRow == 0 && h != 0) {
					if(drawHeartsUpwards)
						hy++;
					else
						hy--;
					hx = 0;
				}
				hearts [h] = Instantiate (heartGUIElement) as Image;
				hearts [h].transform.SetParent (hudCanvas.transform);
				hearts [h].rectTransform.position = new Vector2 (heartGUIElement.rectTransform.position.x + ((heartGUIElement.rectTransform.rect.width + heartsXSpacing) * hx), 
			                                               heartGUIElement.rectTransform.position.y + ((heartGUIElement.rectTransform.rect.height + heartsYSpacing) * hy));
				hearts [h].gameObject.name = "Heart " + h.ToString ();
				if (h < amountOfAcquiredHearts)
					hearts [h].enabled = true;
				else
					hearts [h].enabled = false;
				h++;
				if(drawHeartsLeftwards)
					hx--;
				else
					hx++;
			}
			heartGUIElement.gameObject.SetActive(false);
		}else
			heartGUIElement.gameObject.SetActive(false);
	}

	/// <summary>
	/// Displays the money GUI element
	/// </summary>
	public void DisplayMoney(){
		if (useMoney)
			StartCoroutine (ChangeMoney (0));
		else
			moneyGUIElement.gameObject.SetActive (false);
	}

	/// <summary>
	/// Draws all the specified hotkeys to the hudCanvas
	/// </summary>
	public void DisplayHotkeys(){
		if (useInventory) {
			hotKeyGUIElement.gameObject.SetActive (true);
			hotKeys = new Button[hotkeysAmount];
			int b = 0;
			RectTransform hotKeyRectTran = hotKeyGUIElement.GetComponent<RectTransform> ();
			Rect hotKeyRect = hotKeyRectTran.rect;
			int y = 0;
			int x = 0;
			while (b < hotkeysAmount) {
				if(Mathf.Abs(x) == hotkeysPerRow){
					x = 0;
					if(drawHotkeysUpwards)
						y++;
					else
						y--;
				}
				hotKeys [b] = Instantiate (hotKeyGUIElement) as Button;
				hotKeys [b].transform.SetParent (hudCanvas.transform);
				hotKeys [b].GetComponent<RectTransform> ().anchoredPosition = hotKeyRectTran.anchoredPosition +
					new Vector2 ((hotKeyRect.width + hotKeysXSpacing) * x, (hotKeyRect.height + hotKeysYSpacing) * y);
				hotKeys [b].gameObject.name = "Hot Key " + b.ToString ();
				if(drawHotkeysLeftwards)
					x--;
				else
					x++;
				b++;
			}
			hotKeyGUIElement.gameObject.SetActive (false);
		}else
			hotKeyGUIElement.gameObject.SetActive (false);
	}

	/// <summary>
	/// Use to add another heart to the total hearts displayed in the hudCanvas
	/// </summary>
	public void AcquireNewHeart(){
		amountOfAcquiredHearts += 1;
		if(amountOfAcquiredHearts <= maxNoOfHearts)
			hearts [amountOfAcquiredHearts - 1].enabled = true;
		StartCoroutine(ChangeHealth (amountOfAcquiredHearts * 4));
	}

	/// <summary>
	/// Set the new maximum amount of money
	/// </summary>
	/// <param name="newAmount">New amount.</param>
	public void UpgradeMoney(int newAmount){
		maxMoney = newAmount;
		StartCoroutine (ChangeMoney (0));
	}

	/// <summary>
	/// Hide the HUD
	/// </summary>
	public void HideHUD(){
		int d = 0;
		while (d < hudCanvas.transform.childCount) {
			hudCanvas.transform.GetChild(d).gameObject.SetActive(false);
			d++;
		}
	}

	/// <summary>
	/// Draw the HUD after using HideHUD()
	/// </summary>
	public void DrawHUD(){
		int u = 0;
		while (u < hudCanvas.transform.childCount) {
			bool notOkayToDraw = false;
			if(hudCanvas.transform.GetChild(u).gameObject == hotKeyGUIElement.gameObject ||
			   hudCanvas.transform.GetChild(u).gameObject == heartGUIElement.gameObject ||
			   !useMoney && hudCanvas.transform.GetChild(u).gameObject == moneyGUIElement.gameObject)
				notOkayToDraw = true;
			if(!useBars){
				int s = 0;
				while(s<hudBars.Length){
					if(hudCanvas.transform.GetChild(u).gameObject == hudBars[s].slider.gameObject)
						notOkayToDraw = true;
					s++;
				}
			}
			if(!notOkayToDraw)
				hudCanvas.transform.GetChild(u).gameObject.SetActive(true);
			u++;
		}
	}

	/// <summary>
	/// Checks for errors.
	/// </summary>
	private void CheckForErrors(){
		
	}

	/// <summary>
	/// Increase or decrease the amount of health in the hearts (4 units per heart)
	/// </summary>
	/// <returns>The health.</returns>
	/// <param name="amount">Amount, can be positive or negative</param>
	public IEnumerator ChangeHealth(int amount){
		int a = 0;
		int bit = Mathf.RoundToInt((Mathf.Sign(amount)));
		while (a < Mathf.Abs(amount)) {
			yield return new WaitForSeconds(healthChangeSpeed);
			if(health + bit < 0)
				health = 0;
			else{
				if(health + bit > amountOfAcquiredHearts * 4)
					health = amountOfAcquiredHearts * 4;
				else
					health += bit;
			}
			int b = 0;
			int hlthAmount = health;
			while(b < amountOfAcquiredHearts){
				if(hlthAmount >= 4){
					hearts[b].sprite = fullHeartIcon;
				}else{
					if(hlthAmount == 3){
						hearts[b].sprite = highHeartIcon;
					}else{
						if(hlthAmount == 2){
							hearts[b].sprite = midHeartIcon;
						}else{
							if(hlthAmount == 1){
								hearts[b].sprite = lowHeartIcon;
							}else{
								if(hlthAmount <= 0){
									hearts[b].sprite = emptyHeartIcon;
								}
							}
						}
					}
				}
				b++;
				hlthAmount -= 4;
			}
			a++;
		}
	}

	/// <summary>
	/// Change the value of one of the HUD Bars
	/// </summary>
	/// <param name="hudBarIndex">Which HUD Bar would you like to access.</param>
	/// <param name="amount">Amount, can be positive or negative</param>
	public IEnumerator ChangeHUDBarValue(int hudBarIndex, int amount){
		if (hudBarIndex >= hudBars.Length) {
		
		} else {
			int bit = Mathf.RoundToInt ((Mathf.Sign (amount)));
			int m = 0;
			Slider staminaGUIElement = hudBars [hudBarIndex].slider;
			while (m < Mathf.Abs(amount)) {
				yield return new WaitForSeconds (hudBars [hudBarIndex].changeSpeed);
				if (staminaGUIElement.value + bit > staminaGUIElement.maxValue)
					staminaGUIElement.value = staminaGUIElement.maxValue;
				else {
					if (staminaGUIElement.value + bit < 0)
						staminaGUIElement.value = 0;
					else
						staminaGUIElement.value += bit;
				}
				m++;
			}
			if (staminaGUIElement.value < staminaGUIElement.maxValue && hudBars [hudBarIndex].autoRegen)
				StartCoroutine (RegenerateStamina (hudBarIndex));
		}
	}

	/// <summary>
	/// Regenerates the stamina.
	/// </summary>
	/// <param name="hudBarIndex">Which HUD Bar would you like to access.</param>
	public IEnumerator RegenerateStamina(int hudBarIndex){
		while (hudBars [hudBarIndex].slider.value < hudBars [hudBarIndex].slider.maxValue) {
			yield return new WaitForSeconds(hudBars [hudBarIndex].regenSpeed);
			hudBars [hudBarIndex].slider.value++;
		}
	}

	/// <summary>
	/// Changes the amount of money.
	/// </summary>
	/// <param name="amount">Amount, can be positive or negative.</param>
	public IEnumerator ChangeMoney(int amount){
		int bit = Mathf.RoundToInt((Mathf.Sign(amount)));
		int m = 0;
		while (m < Mathf.Abs(amount)) {
			yield return new WaitForSeconds(moneyChangeSpeed);
			if(money + bit < 0)
				money = 0;
			else{
				if(money + bit > maxMoney)
					money = maxMoney;
				else
					money += bit;
			}
			string digits = "";
			int d = 0;
			int lng = maxMoney.ToString().Length;
			while(d < lng){
				digits += "0";
				d++;
			}
			moneyGUIElement.text = "" + money.ToString(digits);
			m++;
		}
		if (m == 0) {
			string digits = "";
			int d = 0;
			int lng = maxMoney.ToString().Length;
			while(d < lng){
				digits += "0";
				d++;
			}
			moneyGUIElement.text = "" + money.ToString(digits);
		}
	}
}
