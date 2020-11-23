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
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Collider))]

public class NPC : MonoBehaviour {
	
	private GamePadInputs gPI;
	private Camera cam;
	private CameraFollower camF;
	private ThirdPersonController tPC;

	[SerializeField] private Canvas npcCanvas;					//The GUI Canvas that you would like to display text boxes on
	[SerializeField] private EventSystem eventSys;				//The event system that the buttons displayed on the npcCanvas will use
	[SerializeField] private Image textBoxGUIPos;				//The text box that will be used by this particular NPC
	[SerializeField] private Button replyButtonsPosition;		//The GUI Button that will be used to draw the reply buttons (if any) on the npcCanvas
	[SerializeField] private GameObject buttonPrompt;			//An icon that will appear above the sign when it can be interacted with
	private GameObject prompt;									//The instantiation of buttonPrompt

	[SerializeField] private bool hideHUDDuringCutscene;		//Should the HUD be hidden while interacting with an NPC?
	[SerializeField] private float textSpeed;					//The speed at which the text is written on the canvas (0 for instant, higher for slower)

	[SerializeField] private int maxReplyButtonsPerRow;			//The amount of reply buttons to be drawn on a single row
	[SerializeField] private float repliesYSpacing;				//The space between each reply GUI Button in the Vertical axis
	[SerializeField] private float repliesXSpacing;				//The space between each reply GUI Button in the Horizontal axis
	[SerializeField] private bool drawRepliesLeftwards;						//If you want the replies to be drawn to the left of the GUIElement
	[SerializeField] private bool drawRepliesUpwards;						//If you want the replies to be drawn above the GUIElement

	[System.Serializable] public class Replies{ public int replyID; [TextAreaAttribute(0,10)] public string text; public int resultingPageID; }
	[System.Serializable] public class Dialogue{ public int pageID; [TextAreaAttribute(0,10)] public string content; public Vector3 cameraPosition; 
													public Transform cameraFocus; public Replies[] replies; public int nextPageID; }

	public Dialogue[] dialogue;									//The Dialogue that this NPC will display on the npcCanvas
	private Button[] replyButtons = new Button[0];				//An array of reply GUI Buttons for each page of text (if any)
	private Transform chara;									//A reference to the characters transform
	private bool inCutscene;									//If this NPC is currently being interacted with
	private int prevPage;										//The previous dialogue element that the player viewed
	private int currentPage;									//The current dialogue element that the player is viewing

	private int currentString;									//The current string that is being displayed in the text box
	private bool counting;										//Is true while the text is being 'typed' into the text box

	private bool canInteract;									//A delay used so that the character cannot interact with the same NPC immediately after interacting with it
	private bool organizedReplies;								//Is true once the reply Buttons have been displayed for the current dialogue element
	private float useSpeed;										//The current speed used to type out the text (is set to zero if the text is to be 'skipped')
	private bool changedSpeed;									//Is set to true after useSpeed is set to zero and the text has been 'skipped'


	void Start () {
		gPI = GameObject.FindWithTag ("Handler").GetComponent<GamePadInputs> ();
		tPC = GameObject.FindWithTag ("Player").GetComponent<ThirdPersonController> ();

		if (Camera.main != null) {
			cam = Camera.main;
			camF = cam.GetComponent<CameraFollower> ();
		}
		currentPage = 0;
		if (textSpeed < 0)
			textSpeed = 0f;

		RefreshValues ();
		currentPage = 0;

		canInteract = true;
		organizedReplies = false;
		changedSpeed = true;

		textBoxGUIPos.gameObject.SetActive (false);
		replyButtonsPosition.gameObject.SetActive (false);

		prompt = Instantiate (buttonPrompt) as GameObject;
		prompt.transform.SetParent (this.transform.parent);
		prompt.transform.position = this.transform.position + Vector3.up*1.6f;
		prompt.gameObject.SetActive (false);
	}

	void Update(){
		if(gPI == null)
			gPI = GameObject.FindWithTag ("Handler").GetComponent<GamePadInputs> ();
		if(tPC == null)
			tPC = GameObject.FindWithTag ("Player").GetComponent<ThirdPersonController> ();

		if (!inCutscene && tPC.inACutscene && this.gameObject.name == tPC.currentNPCName) {
			textBoxGUIPos.GetComponentInChildren<Text> ().text = "";
			textBoxGUIPos.gameObject.SetActive(false);
			if(currentPage != 0)
				RemoveReplies();
			canInteract = false;
			StartCoroutine(CloseDelay());
			camF.transitionBackFromCutscene = true;
			camF.prevLookAt = dialogue[currentPage].cameraFocus.position;
			tPC.inACutscene = false;
			camF.cutsceneMode = false;
			camF.canFreeMode = true;
			if(!hideHUDDuringCutscene)
				GameObject.FindWithTag("Handler").GetComponent<HUD>().DrawHUD();
		}

		if (currentPage >= dialogue.Length && inCutscene && this.gameObject.name == tPC.currentNPCName) {
			inCutscene = false;
			if (!hideHUDDuringCutscene)
				GameObject.FindWithTag ("Handler").GetComponent<HUD> ().DrawHUD ();
		}

		if (inCutscene && this.gameObject.name == tPC.currentNPCName) {
			if (gPI.pressLeftB() && changedSpeed) {
				if(currentString >= dialogue [currentPage].content.Length){
					if(dialogue[currentPage].replies.Length == 0){
						if(dialogue[currentPage].nextPageID < 0 || dialogue[currentPage].nextPageID > dialogue.Length)
							inCutscene = false;
						else{
							prevPage = currentPage;
							currentPage = dialogue[currentPage].nextPageID;
							RefreshValues();
						}
					}else{
						if(eventSys.currentSelectedGameObject != null){
							int s = 0;
							while(s < dialogue[currentPage].replies.Length){
								if(replyButtons[s].gameObject == eventSys.currentSelectedGameObject){
									prevPage = currentPage;
									currentPage = dialogue[currentPage].replies[s].resultingPageID;
									RefreshValues();
								}
								s++;
							}
						}
					}
				}else{
					useSpeed = 0f;
					changedSpeed = false;
				}
			}

			if(eventSys.currentSelectedGameObject == null && Vector3.Magnitude (new Vector3 (gPI.LH(), 0f, gPI.LV())) > 0.4f
			   && dialogue[currentPage].replies.Length > 0 && currentString == dialogue[currentPage].content.Length)
				replyButtons [0].Select ();

			if(currentString != dialogue[currentPage].content.Length && !counting){
				counting = true;
				StartCoroutine(DisplayText());
			}
			if(currentString >= dialogue[currentPage].content.Length && dialogue[currentPage].replies.Length > 0 && !organizedReplies){
				organizedReplies = true;
				int a = 0;
				int x = 0;
				int y = 0;
				replyButtons = new Button[dialogue[currentPage].replies.Length];
				RectTransform rectTransform = replyButtonsPosition.GetComponent<RectTransform>();
				while(a < dialogue[currentPage].replies.Length){
					if(Mathf.Abs(x) == maxReplyButtonsPerRow){
						x = 0;
						if(drawRepliesUpwards)
							y++;
						else
							y--;
					}
					replyButtons[a] = Instantiate(replyButtonsPosition) as Button;
					replyButtons[a].transform.SetParent (npcCanvas.transform);
					replyButtons[a].gameObject.SetActive(true);
					replyButtons[a].name = "Reply Option " + a;
					replyButtons[a].GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition + 
																							new Vector2((rectTransform.rect.width + repliesXSpacing) * x, 
							            													(rectTransform.rect.height + repliesYSpacing) * y);
					replyButtons[a].GetComponentInChildren<Text>().text = dialogue[currentPage].replies[a].text;
					a++;
					if(drawRepliesLeftwards)
						x--;
					else
						x++;
				}
				eventSys.firstSelectedGameObject = replyButtons[0].gameObject;
				replyButtons[0].Select();
			}
		}
	}

	void LateUpdate(){
		if (inCutscene) {
			if(!camF.cutsceneMode){
				camF.cutsceneMode = true;
				camF.canFreeMode = false;
			}
			if(camF.cutsceneMode){
				if(cam.transform.position != dialogue[currentPage].cameraPosition){
					if(Vector3.Distance(cam.transform.position, dialogue[currentPage].cameraPosition) > 0.05f)
						cam.transform.position = Vector3.Lerp(cam.transform.position, dialogue[currentPage].cameraPosition, Time.deltaTime*10f);
					else
						cam.transform.position = dialogue[currentPage].cameraPosition;
				}
				cam.transform.LookAt(dialogue[currentPage].cameraFocus.position);
			}
		}
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player" && !tPC.masterBool && tPC.onGround) {
			chara = col.gameObject.transform;
			if(Vector3.Angle(this.transform.forward, chara.forward) >= 90f){
				if(!inCutscene && canInteract)
					prompt.gameObject.SetActive(true);
				if(gPI.pressAction() && !inCutscene && canInteract){
					inCutscene = true;
					tPC.inACutscene = true;
					tPC.currentNPCName = this.gameObject.name;
					currentPage = 0;
					RefreshValues();
					textBoxGUIPos.gameObject.SetActive(true);
					if(!hideHUDDuringCutscene)
						GameObject.FindWithTag("Handler").GetComponent<HUD>().HideHUD();
				}
			}else
				prompt.gameObject.SetActive(false);
		}else
			prompt.gameObject.SetActive(false);
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player")
			prompt.gameObject.SetActive(false);
	}

	void RefreshValues(){
		useSpeed = textSpeed;
		currentString = 0;
		if (currentPage != 0) {
			textBoxGUIPos.GetComponentInChildren<Text> ().text = "";
			RemoveReplies ();
		}
		organizedReplies = false;
		changedSpeed = true;
		tPC.cutsceneLookAtPos = dialogue [currentPage].cameraFocus.position;
	}

	public void HitReply(PointerEventData ev){
		int e = 0;
		bool foundReply = false;
		while (e < dialogue [currentPage].replies.Length && !foundReply) {
			if(replyButtons[e].gameObject == ev.selectedObject.gameObject){
				prevPage = currentPage;
				currentPage = dialogue [currentPage].replies [e].resultingPageID;
				RefreshValues();
				foundReply = true;
			}
			e++;
		}
	}

	void TextType(){
		textBoxGUIPos.GetComponentInChildren<Text>().text += dialogue [currentPage].content [currentString];
		currentString++;
	}

	void RemoveReplies(){
		int r = 0;
		while(r < dialogue[prevPage].replies.Length){
			replyButtons[r].gameObject.SetActive(false);
			r++;
		}
	}
	
	IEnumerator DisplayText(){
		yield return new WaitForSeconds (useSpeed);
		if (useSpeed == 0f) {
			while (currentString < dialogue [currentPage].content.Length) { 
				TextType ();
			}
			changedSpeed = true;
		}else 
			TextType();
		counting = false;
	}

	IEnumerator CloseDelay(){
		yield return new WaitForSeconds(0.5f);
		canInteract = true;
	}
}
