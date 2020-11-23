#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.EasyTouch4{
	[Category("EasyTouch")]    
	[System.Serializable]
	public class OnTouchEvent : Condition {
		public TouchEventType type;
		private bool isTrigger;

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
	
			isTrigger = false;
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
			isTrigger = true;
		}
		
		public override bool Validate ()
		{
			
			if (isTrigger) {
				isTrigger=false;	
				return true;
			}
			return isTrigger;
		}
	}
}
#endif
