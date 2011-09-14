using UnityEngine;
using System.Collections;

public class DisplayObjectContainer : DisplayObject {

	public DisplayObjectContainer() {

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
	
	public void addChild(DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Add(child);
		child.parent		= this;
		setTransformInTreeDirty();
		setOriginalSizeDirty();
	}
	
	public void addChildAt(int index, DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Insert(index,child);
		child.parent		= this;
		setTransformInTreeDirty();
		setOriginalSizeDirty();
	}
	
	public void setChildIndex(DisplayObject child,int index){
		_childList.Remove(child);
		_childList.Insert(index,child);
	}
	
	public void removeChild(DisplayObject child){
		
		_childList.Remove(child);
		child.parent		= null;
		setOriginalSizeDirty();
	}
	
	public bool hasChild(DisplayObject child){
		return(!(_childList.IndexOf(child)==-1));
	}
	
	public void removeAllChildren(){
		for(int i=0;i<_childList.Count;i++){
			DisplayObject child = _childList[i] as DisplayObject;
			child.parent	= null;
		}
		_childList.RemoveRange(0,_childList.Count);
		setOriginalSizeDirty();
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
		
		Vector2	minChildPos	= new Vector2(999999,999999);
		DisplayObject child;
		
		for(int i=0;i<_childList.Count;i++){
			child	= _childList[i] as DisplayObject;
			if(child.x<minChildPos.x){
				minChildPos.x	= child.x;
			}
			if(child.y<minChildPos.y){
				minChildPos.y	= child.y;
			}
		}
		
		_boundRect.x		= minChildPos.x * _transformInTreeScale.x + _transformInTree.tx;
		_boundRect.y		= minChildPos.y * _transformInTreeScale.y + _transformInTree.ty;
		_boundRect.width	= _originalWidth * _transformInTreeScale.x;
		_boundRect.height	= _originalHeight * _transformInTreeScale.y;
	}
	
	/***************************************
	 * original size
	 ***************************************/
	
	override public void updateOriginalSize(){
		if(!_isOriginalSizeDirty){
			return;
		}
		_originalWidth	= 0;
		_originalHeight	= 0;
		DisplayObject child;
		Vector2	minChildPos	= new Vector2(999999,999999);
		
		for(int i=0;i<_childList.Count;i++){
			child	= _childList[i] as DisplayObject;
			
			child.updateOriginalSize();
			if(child.x+child.width>_originalWidth){
				_originalWidth	= child.x+child.width;
			}
			if(child.y+child.height>_originalHeight){
				_originalHeight	= child.y+child.height;
			}
			if(child.x<minChildPos.x){
				minChildPos.x	= child.x;
			}
			if(child.y<minChildPos.y){
				minChildPos.y	= child.y;
			}
		}
		_originalWidth	-= minChildPos.x;
		_originalHeight	-= minChildPos.y;
		_width	= _originalWidth *_scaleX;
		_height	= _originalHeight *_scaleY;
		setBoundRectDirty();
	}
	
	/***************************************
	 * hit test
	 ***************************************/
	
	override public bool hitTest(Vector2 v){
		if(!_boundRect.Contains(v)){
			return false;
		}
		bool isHit = false;
		DisplayObject child;
		for(int i=_childList.Count-1;i>=0;i--){
			child	= _childList[i] as DisplayObject;
			if(child.hitTest(v)){
				isHit	= true;
				break;
			}
			
		}
		return isHit;
	}
}
