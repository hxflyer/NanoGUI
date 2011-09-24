/**
 * VERSION: 0.9
 * DATE: 2011-09-20
 * c#
 * author: huang xiang
 * hxflyer@gmail.com
 * www.hxflyer.com
 **/




/**
 * DisplayObjectContainer can contain a list of displayobjects, the instance of this Class will be represent a node in rendering tree
 * nomrally this Class should not be directly use as parent Class by anybody, if you want to make a functional 
 * display Class you should extend it from Sprite
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayObjectContainer : DisplayObject {

	public DisplayObjectContainer() {

	}
	
	
	override public void destroy(){
		removeAllChildren();
		parent.removeChild(this);
	}
	
	
	override public void dispatchEnterFrame(){
		this.dispatchEvent(new GuiEvent(GuiEvent.ENTER_FRAME));
		foreach(DisplayObject child in _childList){
			child.dispatchEnterFrame();
		}
	}
	
	/***************************************
	 * childlist
	 ***************************************/
	
	//this array is for cache rendering tree
	protected List<DisplayObject> _childList	= new List<DisplayObject>();
	
	public List<DisplayObject> childList {
    	get { return _childList; }
    	private set { _childList = value;}
	}
	
	public virtual void addChild(DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Add(child);
		child.stage			= _stage;
		child.parent		= this;
	}
	
	public virtual void addChildAt(int index, DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Insert(index,child);
		child.stage			= _stage;
		child.parent		= this;
	}
	
	public void setChildIndex(DisplayObject child,int index){
		_childList.Remove(child);
		_childList.Insert(index,child);
	}
	
	public void removeChild(DisplayObject child){
		
		_childList.Remove(child);
		child.stage			= null;
		child.parent		= null;
		updateParentBoundRect();
	}
	
	public bool hasChild(DisplayObject child){
		return(!(_childList.IndexOf(child)==-1));
	}
	
	public void removeAllChildren(){
		foreach(DisplayObject child in _childList){
			child.stage		= null;
			child.parent	= null;
		}
		_childList.RemoveRange(0,_childList.Count);
		updateParentBoundRect();
	}
	
	public int numChildren{
		get {return _childList.Count;}
	}
	
	
	override public void updateTransformInTree(){
		base.updateTransformInTree();
		foreach(DisplayObject child in _childList){
			child.updateTransformInTree();
		}
	}
	
	/***************************************
	 * render
	 ***************************************/
	
	override public void render(){
		_alphaInTree	= _alpha * parent.alphaInTree;
		if(!_visible && _alphaInTree<=0){
			return;
		}
		renderChildren();
	}
	
	protected void renderChildren(){
		foreach(DisplayObject child in _childList){
			child.render();
		}
	}
	
	
	/***************************************
	 * bound rect
	 ***************************************/
	//bound rect is the rendering bound rect on stage
	//using for hittest
	override public void updateBoundRect(){
		Vector2 minPos	= new Vector2(999999,999999);
		Vector2 maxPos	= new Vector2(-999999,-999999);
		// loop though children to get the bound area of children
		
		foreach(DisplayObject child in _childList){
			
			Rect childBoundRect	= _transform.getBoundRect(child.boundRect);
			if(childBoundRect.x < minPos.x){
				minPos.x	= childBoundRect.x;
			}
			if(childBoundRect.x+childBoundRect.width > maxPos.x){
				maxPos.x	= childBoundRect.x+childBoundRect.width;
			}
			if(childBoundRect.y < minPos.y){
				minPos.y	= childBoundRect.y;
			}
			if(childBoundRect.y+childBoundRect.height > maxPos.y){
				maxPos.y	= childBoundRect.y+childBoundRect.height;
			}
		}
		
		_boundRect.x			= minPos.x;
		_boundRect.y			= minPos.y;
		_boundRect.width		= maxPos.x-minPos.x;
		_boundRect.height		= maxPos.y-minPos.y;
		
		//Debug.Log(id + "  updateBoundRect"+ _boundRect);
		
		if(_parent!=null && _parent.transformInTree!=null){
			_boundRectInTree	= _parent.transformInTree.getBoundRect(_boundRect);
		}
		
		minPos	= new Vector2(999999,999999);
		maxPos	= new Vector2(-999999,-999999);
		
		
		foreach(DisplayObject child in _childList){
			if(child.boundRect.x < minPos.x){
				minPos.x	= child.boundRect.x;
			}
			if(child.boundRect.x+child.boundRect.width > maxPos.x){
				maxPos.x	= child.boundRect.x+child.boundRect.width;
			}
			if(child.boundRect.y < minPos.y){
				minPos.y	= child.boundRect.y;
			}
			if(child.boundRect.y+child.boundRect.height > maxPos.y){
				maxPos.y	= child.boundRect.y+child.boundRect.height;
			}
		}
		_originalWidth			= maxPos.x-minPos.x;
		_originalHeight			= maxPos.y-minPos.y;
		_width					= _originalWidth*_scaleX;
		_height					= _originalHeight*_scaleY;
		
		//Debug.Log(id + "  updateBoundRectInTree" + _boundRectInTree);
	}
	
	
	
	override public void updateRelatedBoundRect(){
		foreach(DisplayObject child in _childList){
			if(child is DisplayObjectContainer){
				(child as DisplayObjectContainer).updateChildBoundRect();
			}else{
				child.updateBoundRect();
			}
		}
		updateBoundRect();
		if(_parent!=null){
			_parent.updateParentBoundRect();
		}
	}
	public virtual void updateChildBoundRect(){
		foreach(DisplayObject child in _childList){
			if(child is DisplayObjectContainer){
				(child as DisplayObjectContainer).updateChildBoundRect();
			}else{
				child.updateBoundRect();
			}
		}
		updateBoundRect();
	}
	
	
	/***************************************
	 * hit test
	 ***************************************/
	
	override public bool hittest(Vector2 vec){
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		bool isHit = false;
		DisplayObject child;
		for(int i=_childList.Count-1;i>=0;i--){
			child	= _childList[i] as DisplayObject;
			if(child.hittest(vec)){
				isHit	= true;
				break;
			}
			
		}
		return isHit;
	}
	
	//mouse event hit test, dispatch the mouse event if hittest valid
	override public bool hitTestMouseDispatch(string type,Vector2 vec){
		if(!mouseEnable || !_visible){
			return false;
		}
		Vector2 newVec = transformInTreeInverted.transformVector(vec);

		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		bool isHit = false;
		DisplayObject child;
		for(int i=_childList.Count-1;i>=0;i--){
			child	= _childList[i] as DisplayObject;
			if(child.hitTestMouseDispatch(type,vec)){
				isHit	= true;
				break;
			}
			
		}
		if(isHit){
			this.dispatchEvent(new MouseEvent(type,newVec,vec));
		}
		return isHit;
	}
	
	//touch event hit test, dispatch the touch event if hittest valid
	override public bool hitTestTouchDispatch(string type,Touch touch){
		if(!mouseEnable || !_visible){
			return false;
		}
		Vector2 vec = new Vector2(touch.position.x,Stage.instance.stageHeight- touch.position.y);
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		bool isHit = false;
		DisplayObject child;
		for(int i=_childList.Count-1;i>=0;i--){
			child	= _childList[i] as DisplayObject;
			if(child.hitTestTouchDispatch(type,touch)){
				isHit	= true;
				break;
			}
			
		}
		if(isHit){
			_touchList.Add(touch);
			this.dispatchEvent(new TouchEvent(type,touch));
		}
		return isHit;
	}
	
	
	override public void clearTouchs(){
		foreach(DisplayObject child in _childList){
			child.clearTouchs();
		}
		_touchList.Clear();
	}
	
	
	
	
	/***************************************
	 * gesture recognize
	 ***************************************/
	
	// recognize gesture and dispatch gesture events
	
	override public void updateTouchs(){
		
		if(!mouseEnable || !_visible || _touchList.Count==0){
			return;
		}
		foreach(DisplayObject child in _childList){
			child.updateTouchs();
		}
		
		if(_touchList.Count==1){
			//if only one finger touch this object, check if the swipe gesture valid
			Touch soloTouch	= (Touch)_touchList[0];
			
			if(soloTouch.phase	== TouchPhase.Ended){
				if(_swipeCounter>GestureEvent.swipeCountThreshold){
					GestureEvent e = new GestureEvent(GestureEvent.SWIPE);
					e.swipeDirection = _swipeDirection;
					_swipeDirection	= null;
					this.dispatchEvent(e);
				}
				_swipeCounter = 0;
			}else if(soloTouch.phase == TouchPhase.Moved || soloTouch.phase	== TouchPhase.Began){
				
				if(Mathf.Abs(soloTouch.deltaPosition.x)>GestureEvent.swipeDeltaThreshold && 
				   (soloTouch.deltaPosition.x>0?GestureEvent.SWIPE_RIGHT:GestureEvent.SWIPE_LEFT)==_swipeDirection){
					if(_swipeCounter==0){
						_swipeDirection	= soloTouch.deltaPosition.x>0?GestureEvent.SWIPE_RIGHT:GestureEvent.SWIPE_LEFT;
					}
					_swipeCounter ++;
				}else{
					_swipeCounter = 0;
					_swipeDirection	= null;
				}
			}else{
				_swipeCounter = 0;
				_swipeDirection	= null;
			}
		
		}
		
		if(_touchList.Count==2)
		{
			//if only one finger touch this object, check scale, rotate and pan
			Touch firstTouch	= (Touch)_touchList[0];
			Touch secondTouch	= (Touch)_touchList[1];
			
			//scale
			
			float lastDistance	= Vector2.Distance(firstTouch.position-firstTouch.deltaPosition,secondTouch.position-secondTouch.deltaPosition);
			float distance		= Vector2.Distance(firstTouch.position,secondTouch.position);
			float deltaScale	= distance/lastDistance;
			
			if(Mathf.Abs(deltaScale-1.0f)>0.001){
				GestureEvent	e = new GestureEvent(GestureEvent.ZOOM);
				e.deltaScale	= deltaScale;
				this.dispatchEvent(e);
			}
			
			//rotation
			
			float lastAng		= Mathf.Atan2( (secondTouch.position.y-secondTouch.deltaPosition.y)- (firstTouch.position.y-firstTouch.deltaPosition.y)  ,
			                             (firstTouch.position.x-firstTouch.deltaPosition.x) - (secondTouch.position.x-secondTouch.deltaPosition.x))*Mathf.Rad2Deg;
			float ang			= Mathf.Atan2(secondTouch.position.y - firstTouch.position.y , firstTouch.position.x - secondTouch.position.x)*Mathf.Rad2Deg;
			float deltaRotation		= ang-lastAng;
			if(Mathf.Abs(deltaRotation)>.01){
				GestureEvent	e = new GestureEvent(GestureEvent.ROTATE);
				e.deltaRotation	= deltaRotation;
				this.dispatchEvent(e);
			}
			
			//pan
			
			Vector2 avgDeltaPos	= (firstTouch.deltaPosition+secondTouch.deltaPosition)/2;
			if(Mathf.Abs(avgDeltaPos.x)>GestureEvent.panThreshold || Mathf.Abs(avgDeltaPos.y)>GestureEvent.panThreshold){
				GestureEvent	e = new GestureEvent(GestureEvent.PAN);
				avgDeltaPos.y	= -avgDeltaPos.y;
				e.deltaPan		= avgDeltaPos;
				this.dispatchEvent(e);
			}
		}
	}
}
