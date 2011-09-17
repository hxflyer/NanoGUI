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
		
		_originalWidth			= maxPos.x-minPos.x;
		_originalHeight			= maxPos.y-minPos.y;
		
		
		// get boundRect, boundRect is related with it's parent
		_boundRect.x			= minPos.x;
		_boundRect.y			= minPos.y;
		_boundRect.width		= maxPos.x-minPos.x;
		_boundRect.height		= maxPos.y-minPos.y;
		_width					= _boundRect.width;
		_height					= _boundRect.height;
		
		//Debug.Log(id+_boundRect);
		if(_parent!=null){
			_boundRectInTree	= _parent.transformInTree.getBoundRect(minPos,maxPos);
		}
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
			this.dispatchEvent(new MouseEvent(this,type,newVec,vec));
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
			this.dispatchEvent(new TouchEvent(this,type,touch));
		}
		return isHit;
	}
}
