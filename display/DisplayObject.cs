using UnityEngine;
using System.Collections;

public class DisplayObject : EventDispatcher {
	

	public DisplayObject () {

	}
	
	
	
	
	
	/***************************************
	 * 2d transform
	 ***************************************/
	
	protected Transform2D	_transform	= new Transform2D();
	
	public Transform2D transform {
    	get { return _transform; }
    	private set { _transform = value;}
	}
	
	protected Transform2D	_transformInTree;
	public Transform2D transformInTree {
    	get { return _transformInTree; }
    	private set { _transformInTree = value;}
	}
	
	
	protected bool _isTransformDirty	= true;
	public void setTransFormDirty(){
		_isTransformDirty	= true;
	}
	
	public virtual void updateTransform(){
		this.dispatchEvent(new GuiEvent(this,GuiEvent.ENTER_FRAME));
		if(!_isTransformDirty){
			return;
		}
		_isTransformDirty	= false;
		_transform.identity();
		_transform.setTranslate(_x,_y);
		_transform.rotate(_rotation/Mathf.Rad2Deg);
		_transform.scale(_scaleX,_scaleY);
		
		setTransformInTreeDirty();
		if(_parent!=null){
			_parent.setOriginalSizeDirty();
		}
	}
	
	
	//trnsform in render tree
	
	protected bool _isTransformInTreeDirty	= true;
	
	public virtual void setTransformInTreeDirty(){
		_isTransformInTreeDirty	= true;
	}
	
	
	//transform in tree update
	
	protected Vector2 _transformInTreeScale;
	public Vector2 transformInTreeScale{
		get {return _transformInTreeScale;}
		private set {_transformInTreeScale = value;}
	}
	
	protected float	_transformInTreeRotation	= 0;
	public float transformInTreeRotation{
		get {return _transformInTreeRotation;}
		private set {_transformInTreeRotation = value;}
	}

	
	
	public virtual void updateTransformInTree(){
		
		if(!_isTransformInTreeDirty){
			return;
		}
		_isTransformInTreeDirty	= false;
		_transformInTree		= Transform2D.multiply(_parent.transformInTree,_transform);
		_transformInTreeScale.x	= _scaleX*_parent.transformInTreeScale.x;
		_transformInTreeScale.y	= _scaleY*_parent.transformInTreeScale.y;
		_transformInTreeRotation	= _rotation + _parent.transformInTreeRotation;
		
		setBoundRectDirty();
	}
	
	
	// position , scale , size
	
	protected float _x = 0.0f;
	public float x {
    	get { return _x; }
    	set { 
			_x = value;
			_transform.setTranslateX(_x);
			setTransformInTreeDirty();
			if(_parent!=null){
				_parent.setOriginalSizeDirty();
			}
		}
	}
	
	protected float _y = 0.0f;
	public float y {
    	get { return _y; }
    	set { 
			_y = value;
			_transform.setTranslateY(_y);
			setTransformInTreeDirty();
			if(_parent!=null){
				_parent.setOriginalSizeDirty();
			}
		}
	}
	
	protected float _width = 0.0f;
	public float width {
		get { return _width; }
    	set {
			_width = value;
			if(_originalWidth!=0){
				_scaleX	= _width/_originalWidth;
			}
			_isTransformDirty	= true;
			if(_parent!=null){
				_parent.setOriginalSizeDirty();
			}
		}
	}
	
	protected float _height = 0.0f;
	public float height {
    	get { return _height; }
    	set {
			_height = value;
			if(_originalHeight!=0){
				_scaleY	= _height/_originalHeight;
			}
			_isTransformDirty	= true;
			if(_parent!=null){
				_parent.setOriginalSizeDirty();
			}
		}
	}
	
	protected float _scaleX = 1.0f;
	public float scaleX {
    	get { return _scaleX; }
    	set { 
			_scaleX = value;
			if(_originalWidth!=0){
				_width	= _scaleX*_originalWidth;
			}
			_isTransformDirty	= true;
			if(_parent!=null){
				_parent.setOriginalSizeDirty();
			}
		}
	}
	
	protected float _scaleY = 1.0f;
	public float scaleY {
    	get { return _scaleY; }
    	set {
			_scaleY = value;
			if(_originalHeight!=0){
				_height	= _scaleY*_originalHeight;
			}
			_isTransformDirty	= true;
			if(_parent!=null){
				_parent.setOriginalSizeDirty();
			}
		}
	}
	
	protected float _rotation;
	public float rotation {
    	get { return _rotation; }
    	set {
			_rotation = value;
			_isTransformDirty	= true;
			if(_parent!=null){
				_parent.setOriginalSizeDirty();
			}
		}
	}
	
	
	
	
	/***************************************
	 * original size
	 ***************************************/
	
	protected float _originalWidth	= 0;
	protected float _originalHeight	= 0;
	protected bool _isOriginalSizeDirty = true;
	
	public virtual void updateOriginalSize(){
		
	}
	
	public void setOriginalSizeDirty(){
		if(!_isOriginalSizeDirty){
			return;
		}
		_isOriginalSizeDirty	= true;
		if(_parent!=null){
			_parent.setOriginalSizeDirty();
		}
	}
	
	/***************************************
	 * bound rect
	 ***************************************/
	
	protected bool	_isBoundRectDirty	= true;
	public void setBoundRectDirty(){
		_isBoundRectDirty	= true;
	}
	protected Rect _boundRect	= new Rect();
	
	public Rect boundRect {
    	get { return _boundRect; }
    	private set { _boundRect = value;}
	}
	
	public virtual void updateBoundRect(){
		if(!_isBoundRectDirty){
			return;
		}
		_boundRect.x		= _transformInTree.tx;
		_boundRect.y		= _transformInTree.ty;
		_boundRect.width	= _originalWidth * _transformInTreeScale.x;
		_boundRect.height	= _originalHeight * _transformInTreeScale.y;
	}
	
	
	
	/***************************************
	 * render
	 ***************************************/
	
	protected bool _visible	= true;
	
	public bool visible {
    	get { return _visible; }
    	set { _visible = value;}
	}
	
	protected float _alpha	= 1.0f;
	
	public float alpha {
    	get { return _alpha; }
    	set { _alpha = value;}
	}
	protected float _alphaInTree	= 1.0f;
	
	public float alphaInTree {
    	get { return _alphaInTree; }
    	private set { _alphaInTree = value;}
	}
	
	public virtual void render(){
		_alphaInTree	= _alpha * parent.alphaInTree;
		if(!_visible && _alphaInTree<=0){
			return;
		}
	}
	
	/***************************************
	 * rendering tree
	 ***************************************/
	
	protected DisplayObjectContainer _parent;

	
	public DisplayObjectContainer parent {
    	get { return _parent; }
    	set { 
			_parent = value;
			}
	}
	
	
	/***************************************
	 * hit test
	 ***************************************/
	
	public virtual bool hitTest(Vector2 v){
		return _boundRect.Contains(v);
	}
}
