using UnityEngine;
using System.Collections;

public class DisplayObjectContainer : DisplayObject {

	public DisplayObjectContainer() {

	}
	
	
	override public void destroy(){
		removeAllChildren();
		parent.removeChild(this);
	}
	/***************************************
	 * childlist
	 ***************************************/
	
	protected ArrayList _childList	= new ArrayList();
	
	public ArrayList childList {
    	get { return _childList; }
    	private set { 
			_childList = value;
		}
	}
	
	public virtual void addChild(DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Add(child);
		child.parent		= this;
		child.stage			= _stage;
		setTransformInTreeDirty();
		setBoundRectDirty();
	}
	
	public virtual void addChildAt(int index, DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Insert(index,child);
		child.parent		= this;
		child.stage			= _stage;
		setTransformInTreeDirty();
		setBoundRectDirty();
	}
	
	public void setChildIndex(DisplayObject child,int index){
		_childList.Remove(child);
		_childList.Insert(index,child);
	}
	
	public void removeChild(DisplayObject child){
		
		_childList.Remove(child);
		child.parent		= null;
		child.stage			= null;
		setBoundRectDirty();
	}
	
	public bool hasChild(DisplayObject child){
		return(!(_childList.IndexOf(child)==-1));
	}
	
	public void removeAllChildren(){
		for(int i=0;i<_childList.Count;i++){
			DisplayObject child = _childList[i] as DisplayObject;
			child.parent	= null;
			child.stage		= null;
		}
		_childList.RemoveRange(0,_childList.Count);
		setBoundRectDirty();
	}
	
	public int numChildren{
		get {return _childList.Count;}
	}
	
	
	/***************************************
	 * transform
	 ***************************************/

	override public void updateTransform(){
		base.updateTransform();
		for(int i=0;i<_childList.Count;i++){
			(_childList[i] as DisplayObject).updateTransform();
		}
		
	}
	
	//transform in tree update
	
	override public void setTransformInTreeDirty(){
		_isTransformInTreeDirty	= true;
		for(int i=0;i<_childList.Count;i++){
			DisplayObject child = _childList[i] as DisplayObject;
			child.setTransformInTreeDirty();
		}
	}
	
	
	override public void updateTransformInTree(){
		base.updateTransformInTree();
		for(int i=0;i<_childList.Count;i++){
			DisplayObject child = _childList[i] as DisplayObject;
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
		//GUI.DrawTexture(_boundRectInTree,Resources.Load("frame",typeof(Texture2D)) as Texture2D);
		renderChildren();
	}
	
	protected void renderChildren(){
		for(int i=0;i<_childList.Count;i++){
			(_childList[i] as DisplayObject).render();
		}
	}
	
	
	/***************************************
	 * bound rect
	 ***************************************/
	
	override public void updateBoundRect(){

		for(int i=0;i<_childList.Count;i++){
			(_childList[i] as DisplayObject).updateBoundRect();
		}
		if(!_isBoundRectDirty){
			return;
		}
		_isBoundRectDirty	= false;
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
		
		minPos			= new Vector2(999999,999999);
		maxPos			= new Vector2(-999999,-999999);
		
		if(_parent!=null){
			_boundRectInTree	= _parent.transformInTree.getBoundRect(minPos,maxPos);
		}
		
		
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
	
	
	override public bool hitTestMouseDispatch(string type,Vector2 vec){
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
	
	
	override public bool hitTestTouchDispatch(string type,Touch touch){
		
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
	
	
	override public void updateTouchs(){
		//Debug.Log(id+ " updateTouchs");
		/*if(_touchList.Count==0){
			return;
		}*/
		foreach(DisplayObject child in _childList){
			child.updateTouchs();
		}
		
		if(_touchList.Count==1){
			//Debug.Log("solotouch check");
			Touch soloTouch	= (Touch)_touchList[0];
			
			if(soloTouch.phase	== TouchPhase.Ended){
				if(_swipeCounter>GestureEvent.swipeCountThreshold){
					this.dispatchEvent(new GestureEvent(GestureEvent.SWIPE));
				}
				_swipeCounter = 0;
			}else if(soloTouch.phase == TouchPhase.Moved || soloTouch.phase	== TouchPhase.Began){
				if(Mathf.Abs(soloTouch.deltaPosition.x)>GestureEvent.swipeDeltaThreshold){
					_swipeCounter ++;
				}else{
					_swipeCounter = 0;
				}
			}else{
				_swipeCounter = 0;
			}
		
		}
		if(_touchList.Count==2){
			//Debug.Log("double check");
			Touch firstTouch	= (Touch)_touchList[0];
			Touch secondTouch	= (Touch)_touchList[1];
			//scale
			float lastDistance	= Vector2.Distance(firstTouch.position-firstTouch.deltaPosition,secondTouch.position-secondTouch.deltaPosition);
			float distance		= Vector2.Distance(firstTouch.position,secondTouch.position);
			float deltaScale	= distance/lastDistance;
			
			if(Mathf.Abs(deltaScale-1.0f)>0.0001){
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
