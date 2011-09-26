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
		if(_childList!=null && _childList.Count>0){
			foreach(DisplayObject child in _childList){
				child.dispatchEnterFrame();
			}
		}
	}
	
	/***************************************
	 * childlist
	 ***************************************/
	
	//this array is for cache rendering tree
	protected List<DisplayObject> _childList;
	
	public List<DisplayObject> childList {
    	get { return _childList; }
    	private set { _childList = value;}
	}
	protected List<DisplayObject> getChildList(){
		if(_childList==null){
			_childList	= new List<DisplayObject>();
		}
		return(_childList);
	}
	public virtual void addChild(DisplayObject child){
		if(_childList!=null && _childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		getChildList().Add(child);
		child.stage			= _stage;
		child.parent		= this;
	}
	
	public virtual void addChildAt(int index, DisplayObject child){
		if(_childList!=null && _childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		getChildList().Insert(index,child);
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
		return(_childList!=null && !(_childList.IndexOf(child)==-1));
	}
	
	public void removeAllChildren(){
		if(_childList==null || _childList.Count==0){
			return;
		}
		
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
		if(_childList!=null && _childList.Count>0){
			foreach(DisplayObject child in _childList){
				child.render();
			}
		}
	}
	
	
	
	
	/***************************************
	 * transforms
	 ***************************************/
	
	
	override public void updateTransformInTree(){
		base.updateTransformInTree();
		if(_childList!=null && _childList.Count>0){
			foreach(DisplayObject child in _childList){
				child.updateTransformInTree();
			}
		}
	}
	
	
	
	
	/***************************************
	 * bound rect
	 ***************************************/
	//bound rect is the rendering bound rect on stage
	//using for hittest
	override public void updateBoundRect(){
		// loop though children to get the bound area of children
		
		if(_childList!=null && _childList.Count>0){
			Vector2 minPos	= new Vector2(999999,999999);
			Vector2 maxPos	= new Vector2(-999999,-999999);
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
		}else{
			_boundRect.x			= 0;
			_boundRect.y			= 0;
			_boundRect.width		= 0;
			_boundRect.height		= 0;
		}
		
		if(_parent!=null && _parent.transformInTree!=null){
			_boundRectInTree	= _parent.transformInTree.getBoundRect(_boundRect);
		}
		
		if(_childList!=null && _childList.Count>0){
			Vector2 minPos	= new Vector2(999999,999999);
			Vector2 maxPos	= new Vector2(-999999,-999999);
			
			if(_childList!=null){
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
			}
		//original size has nothing to do with transform
			_originalWidth			= maxPos.x-minPos.x;
			_originalHeight			= maxPos.y-minPos.y;
			_width					= _originalWidth*_scaleX;
			_height					= _originalHeight*_scaleY;
		}else{
			_originalWidth			= 0;
			_originalHeight			= 0;
			_width					= 0;
			_height					= 0;
		}
	}
	
	
	//update parents bound rect and children bound rect
	override public void updateRelatedBoundRect(){
		if(_childList!=null && _childList.Count>0){
			foreach(DisplayObject child in _childList){
				if(child is DisplayObjectContainer){
					(child as DisplayObjectContainer).updateChildBoundRect();
				}else{
					child.updateBoundRect();
				}
			}
		}
		updateBoundRect();
		if(_parent!=null){
			_parent.updateParentBoundRect();
		}
	}
	
	public virtual void updateChildBoundRect(){
		if(_childList!=null && _childList.Count>0){
			foreach(DisplayObject child in _childList){
				if(child is DisplayObjectContainer){
					(child as DisplayObjectContainer).updateChildBoundRect();
				}else{
					child.updateBoundRect();
				}
			}
		}
		updateBoundRect();
	}
	
	
	/***************************************
	 * hit test
	 ***************************************/
	
	override public bool hittest(Vector2 vec){
		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		bool isHit = false;
		DisplayObject child;
		if(_childList!=null && _childList.Count>0){
			for(int i=_childList.Count-1;i>=0;i--){
				child	= _childList[i] as DisplayObject;
				if(child.hittest(vec)){
					isHit	= true;
					break;
				}
				
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
		if(_childList!=null && _childList.Count>0){
			for(int i=_childList.Count-1;i>=0;i--){
				child	= _childList[i] as DisplayObject;
				if(child.hitTestMouseDispatch(type,vec)){
					isHit	= true;
					break;
				}
				
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
		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		bool isHit = false;
		DisplayObject child;
		if(_childList!=null && _childList.Count>0){
			for(int i=_childList.Count-1;i>=0;i--){
				child	= _childList[i] as DisplayObject;
				if(child.hitTestTouchDispatch(type,touch)){
					isHit	= true;
					break;
				}
				
			}
		}
		if(isHit){
			_touchList.Add(touch);
			this.dispatchEvent(new TouchEvent(type,touch));
		}
		return isHit;
	}
	
	
	override public void clearTouchs(){
		if(_childList!=null && _childList.Count>0){
			foreach(DisplayObject child in _childList){
				child.clearTouchs();
			}
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
		//Debug.Log("updateTouchs");
		if(_childList!=null && _childList.Count>0){
			foreach(DisplayObject child in _childList){
				child.updateTouchs();
			}
		}
		base.updateTouchs();
	}
}
