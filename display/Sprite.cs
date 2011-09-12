using UnityEngine;
using System.Collections;

public class Sprite : DisplayObjectContainer {
	
	
	public Sprite (Texture texture){
		_texture	= texture;
		_originalWidth	= _texture.width;
		_originalHeight	= _texture.height;
	}
	public Sprite (){
		
	}
	
	/***************************************
	 * texture
	 ***************************************/
	protected Texture _texture;
	public Texture texture {
    	get { return _texture; }
    	private set { _texture = value;
			setOriginalSizeDirty();}
	}
	
	protected Rect _textureRenderRect	= new Rect();
	protected Vector2 _texturRenderRotatePivot	= new Vector2();
	
	public Rect textureRenderRect {
    	get { return _textureRenderRect; }
    	private set { _textureRenderRect = value;}
	}
	
	/***************************************
	 * render
	 ***************************************/
	
	
	override public void render(){
		_alphaInTree	= _alpha * parent.alphaInTree;
		if(!_visible && _alphaInTree<=0){
			return;
		}
		
		if(_texture){
			_textureRenderRect.x	= _transformInTree.tx;
			_textureRenderRect.y	= _transformInTree.ty;
			_textureRenderRect.width	= _texture.width * _transformInTreeScale.x;
			_textureRenderRect.height	= _texture.height * _transformInTreeScale.y;
			GUI.color = new Color( 1, 1, 1, _alphaInTree );
			_texturRenderRotatePivot.x	= _transformInTree.tx;
			_texturRenderRotatePivot.y	= _transformInTree.ty;
			
			GUIUtility.RotateAroundPivot (_transformInTreeRotation, _texturRenderRotatePivot);
			//Debug.Log(id+" : "+_transformInTreeRotation);
			GUI.DrawTexture(_textureRenderRect,_texture);
			GUIUtility.RotateAroundPivot (-_transformInTreeRotation, _texturRenderRotatePivot);
		}
		//Debug.Log("render:"+id+"  rect:"+_textureRenderRect);
		renderChildren();
		
	}
	
	
	override public void updateBoundRect(){
		
		for(int i=0;i<_childList.Count;i++){
			(_childList[i] as DisplayObject).updateBoundRect();
		}
		if(!_isBoundRectDirty){
			return;
		}
		
		Vector2	minChildPos;
		if(_texture){
			minChildPos	= new Vector2(0,0);
		}else{
			minChildPos	= new Vector2(999999,999999);
		}
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
		_boundRect.y		= minChildPos.x * _transformInTreeScale.y + _transformInTree.ty;
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
		//Debug.Log("updateOriginalSize:"+id);
		Vector2	minChildPos;
		if(_texture){
			minChildPos		= new Vector2(0,0);
			_originalWidth	= _texture.width;
		}else{
			
			_originalHeight	= _texture.height;minChildPos		= new Vector2(999999,999999);
			_originalWidth	= 0;
			_originalHeight	= 0;
		}
		DisplayObject child;
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
		
		if(_texture){
			if(minChildPos.x<0){
				_originalWidth	-= minChildPos.x;
			}
			if(minChildPos.y<0){
				_originalHeight	-= minChildPos.y;
			}
		}else{
			_originalWidth	-= minChildPos.x;
			_originalHeight	-= minChildPos.y;
		}
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
		if(_texture && _textureRenderRect.Contains(v)){
			return true;
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
