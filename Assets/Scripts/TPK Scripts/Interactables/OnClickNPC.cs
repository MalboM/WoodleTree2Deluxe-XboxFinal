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
/// 
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// Unity Behavior's eventsystem needs a:
/// 1a) collider and phyics raycaster component on the camera(for 3d gameobject) or
/// 1b) a graphics raycaster component on the Canvas (for UI)
/// 2) a monobehavior script (like the one below)
/// 3) and an eventsystem gameobject (added by default when you create a UI->Canvas, 
///     only one can exist in the scene) to work
/// 
/// This script simply catches click events related to the object and passes it to where 
/// you need it to go
/// </summary>
public class OnClickNPC : MonoBehaviour,  IPointerClickHandler
{
	public NPC npcScript;

	public void OnPointerClick(PointerEventData eventData)
	{
		//passes data to your button controller (or whater you need to know about the
		//button that was pressed), informing that this game object was pressed.
		npcScript.HitReply(eventData);
	}
}