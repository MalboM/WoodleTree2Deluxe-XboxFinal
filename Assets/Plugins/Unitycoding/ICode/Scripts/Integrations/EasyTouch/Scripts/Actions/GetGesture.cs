#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[Tooltip("Get values from Touch by TouchEventType.")]
	[System.Serializable]
	public class GetGesture : StateAction {
		public TouchEventType type;
		[Shared]
		[NotRequired]
		[Tooltip("The index of the finger that raise the event (Starts at 0), or -1 for a two fingers gesture.")]
		public FsmInt fingerIndex;				
		[Shared]
		[NotRequired]
		[Tooltip("The touches count.")]
		public FsmInt touchCount;				
		[Shared]
		[NotRequired]
		[Tooltip("The start position of the current gesture, or the center position between the two touches for a two fingers gesture.")]
		public FsmVector2 startPosition;		
		[Shared]
		[NotRequired]
		[Tooltip("The current position of the touch that raise the event,  or the center position between the two touches for a two fingers gesture.")]
		public FsmVector2 position;
		[Shared]
		[NotRequired]
		[Tooltip("The position delta since last change.")]
		public FsmVector2 deltaPosition;		
		[Shared]
		[NotRequired]
		[Tooltip("Time since the beginning of the gesture.")]
		public FsmFloat actionTime;			
		[Shared]
		[NotRequired]
		public FsmFloat deltaTime;				
		[Shared]
		[NotRequired]
		[Tooltip("The siwpe or drag  type ( None, Left, Right, Up, Down, Other => look at EayTouch.SwipeType enumeration).")]
		public FsmString swipe;	
		[Shared]
		[NotRequired]
		[Tooltip("The length of the swipe.")]
		public FsmFloat swipeLength;				
		[Shared]
		[NotRequired]
		[Tooltip("The swipe vector direction.")]
		public FsmVector2 swipeVector;			
		[Shared]
		[NotRequired]
		[Tooltip("The pinch length delta since last change.")]
		public FsmFloat deltaPinch;	
		[Shared]
		[NotRequired]
		[Tooltip("The angle of the twist.")]
		public FsmFloat twistAngle;		
		[Shared]
		[NotRequired]
		[Tooltip("The distance between two finger for a two finger gesture.")]
		public FsmFloat twoFingerDistance;
		[Shared]
		[NotRequired]
		[Tooltip("The current picked gameObject under the touch that raise the event.")]
		public FsmGameObject pickObject;	
		[Shared]
		[NotRequired]
		[Tooltip("The pick camera.")]
		public FsmGameObject pickCamera;
		[Shared]
		[NotRequired]
		[Tooltip("Is that the camera is Flage GUI")]
		public FsmBool isGuiCamera;

		public override void OnEnter ()
		{
			switch (type) {
			case TouchEventType.TouchCancel:
				EasyTouch.On_Cancel+=OnTouchCallback;
				break;
			case TouchEventType.Cancel2Fingers:
				EasyTouch.On_Cancel2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.TouchStart:
				EasyTouch.On_TouchStart+=OnTouchCallback;
				break;
			case TouchEventType.TouchDown:
				EasyTouch.On_TouchDown+=OnTouchCallback;
				break;
			case TouchEventType.TouchUp:
				EasyTouch.On_TouchUp+=OnTouchCallback;
				break;
			case TouchEventType.SimpleTap:
				EasyTouch.On_SimpleTap+=OnTouchCallback;
				break;
			case TouchEventType.DoubleTap:
				EasyTouch.On_DoubleTap+=OnTouchCallback;
				break;
			case TouchEventType.LongTapStart:
				EasyTouch.On_LongTapStart+=OnTouchCallback;
				break;
			case TouchEventType.LongTap:
				EasyTouch.On_LongTap+=OnTouchCallback;
				break;
			case TouchEventType.LongTapEnd:
				EasyTouch.On_LongTapEnd+=OnTouchCallback;
				break;
			case TouchEventType.DragStart:
				EasyTouch.On_DragStart+=OnTouchCallback;
				break;
			case TouchEventType.Drag:
				EasyTouch.On_Drag+=OnTouchCallback;
				break;
			case TouchEventType.DragEnd:
				EasyTouch.On_DragEnd+=OnTouchCallback;
				break;
			case TouchEventType.SwipeStart:
				EasyTouch.On_SwipeStart+=OnTouchCallback;
				break;
			case TouchEventType.Swipe:
				EasyTouch.On_Swipe+=OnTouchCallback;
				break;
			case TouchEventType.SwipeEnd:
				EasyTouch.On_SwipeEnd+=OnTouchCallback;
				break;
			case TouchEventType.TouchStart2Fingers:
				EasyTouch.On_TouchStart2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.TouchDown2Fingers:
				EasyTouch.On_TouchDown2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.TouchUp2Fingers:
				EasyTouch.On_TouchUp2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.SimpleTap2Fingers:
				EasyTouch.On_SimpleTap2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.DoubleTap2Fingers:
				EasyTouch.On_DoubleTap2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.LongTapStart2Fingers:
				EasyTouch.On_LongTapStart2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.LongTap2Fingers:
				EasyTouch.On_LongTap2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.LongTapEnd2Fingers:
				EasyTouch.On_LongTapEnd2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.Twist:
				EasyTouch.On_Twist+=OnTouchCallback;
				break;
			case TouchEventType.TwistEnd:
				EasyTouch.On_TwistEnd+=OnTouchCallback;
				break;
			case TouchEventType.PinchIn:
				EasyTouch.On_PinchIn+=OnTouchCallback;
				break;
			case TouchEventType.PinchOut:
				EasyTouch.On_PinchOut+=OnTouchCallback;
				break;
			case TouchEventType.PinchEnd:
				EasyTouch.On_PinchEnd+=OnTouchCallback;
				break;
			case TouchEventType.DragStart2Fingers:
				EasyTouch.On_DragStart2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.Drag2Fingers:
				EasyTouch.On_Drag2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.DragEnd2Fingers:
				EasyTouch.On_DragEnd2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.SwipeStart2Fingers:
				EasyTouch.On_SwipeStart2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.Swipe2Fingers:
				EasyTouch.On_Swipe2Fingers+=OnTouchCallback;
				break;
			case TouchEventType.SwipeEnd2Fingers:
				EasyTouch.On_SwipeEnd2Fingers+=OnTouchCallback;
				break;
			}
		}
		
		public override void OnExit ()
		{
			switch (type) {
			case TouchEventType.TouchCancel:
				EasyTouch.On_Cancel-=OnTouchCallback;
				break;
			case TouchEventType.Cancel2Fingers:
				EasyTouch.On_Cancel2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.TouchStart:
				EasyTouch.On_TouchStart-=OnTouchCallback;
				break;
			case TouchEventType.TouchDown:
				EasyTouch.On_TouchDown-=OnTouchCallback;
				break;
			case TouchEventType.TouchUp:
				EasyTouch.On_TouchUp-=OnTouchCallback;
				break;
			case TouchEventType.SimpleTap:
				EasyTouch.On_SimpleTap-=OnTouchCallback;
				break;
			case TouchEventType.DoubleTap:
				EasyTouch.On_DoubleTap-=OnTouchCallback;
				break;
			case TouchEventType.LongTapStart:
				EasyTouch.On_LongTapStart-=OnTouchCallback;
				break;
			case TouchEventType.LongTap:
				EasyTouch.On_LongTap-=OnTouchCallback;
				break;
			case TouchEventType.LongTapEnd:
				EasyTouch.On_LongTapEnd-=OnTouchCallback;
				break;
			case TouchEventType.DragStart:
				EasyTouch.On_DragStart-=OnTouchCallback;
				break;
			case TouchEventType.Drag:
				EasyTouch.On_Drag-=OnTouchCallback;
				break;
			case TouchEventType.DragEnd:
				EasyTouch.On_DragEnd-=OnTouchCallback;
				break;
			case TouchEventType.SwipeStart:
				EasyTouch.On_SwipeStart-=OnTouchCallback;
				break;
			case TouchEventType.Swipe:
				EasyTouch.On_Swipe-=OnTouchCallback;
				break;
			case TouchEventType.SwipeEnd:
				EasyTouch.On_SwipeEnd-=OnTouchCallback;
				break;
			case TouchEventType.TouchStart2Fingers:
				EasyTouch.On_TouchStart2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.TouchDown2Fingers:
				EasyTouch.On_TouchDown2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.TouchUp2Fingers:
				EasyTouch.On_TouchUp2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.SimpleTap2Fingers:
				EasyTouch.On_SimpleTap2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.DoubleTap2Fingers:
				EasyTouch.On_DoubleTap2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.LongTapStart2Fingers:
				EasyTouch.On_LongTapStart2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.LongTap2Fingers:
				EasyTouch.On_LongTap2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.LongTapEnd2Fingers:
				EasyTouch.On_LongTapEnd2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.Twist:
				EasyTouch.On_Twist-=OnTouchCallback;
				break;
			case TouchEventType.TwistEnd:
				EasyTouch.On_TwistEnd-=OnTouchCallback;
				break;
			case TouchEventType.PinchIn:
				EasyTouch.On_PinchIn-=OnTouchCallback;
				break;
			case TouchEventType.PinchOut:
				EasyTouch.On_PinchOut-=OnTouchCallback;
				break;
			case TouchEventType.PinchEnd:
				EasyTouch.On_PinchEnd-=OnTouchCallback;
				break;
			case TouchEventType.DragStart2Fingers:
				EasyTouch.On_DragStart2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.Drag2Fingers:
				EasyTouch.On_Drag2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.DragEnd2Fingers:
				EasyTouch.On_DragEnd2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.SwipeStart2Fingers:
				EasyTouch.On_SwipeStart2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.Swipe2Fingers:
				EasyTouch.On_Swipe2Fingers-=OnTouchCallback;
				break;
			case TouchEventType.SwipeEnd2Fingers:
				EasyTouch.On_SwipeEnd2Fingers-=OnTouchCallback;
				break;
			}
		}
		
		private void OnTouchCallback(Gesture gesture){
			fingerIndex.Value = gesture.fingerIndex;
			touchCount.Value = gesture.touchCount;
			startPosition.Value = gesture.startPosition;
			position.Value = gesture.position;
			deltaPosition.Value = gesture.deltaPosition;
			actionTime.Value = gesture.actionTime;
			deltaTime.Value = gesture.deltaTime;
			swipe.Value = gesture.swipe.ToString ();
			swipeLength.Value = gesture.swipeLength;
			swipeVector.Value = gesture.swipeVector;
			deltaPinch.Value = gesture.deltaPinch;
			twistAngle.Value = gesture.twistAngle;
			twoFingerDistance.Value = gesture.twoFingerDistance;
			pickObject.Value = gesture.pickedObject;
			pickCamera.Value = gesture.pickedCamera.gameObject;
			isGuiCamera.Value = gesture.isGuiCamera;
			
		}
	}
}
#endif