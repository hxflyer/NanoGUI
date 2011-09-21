/**
 * VERSION: 0.9
 * DATE: 2011-09-20
 * c#
 * author: huang xiang
 * hxflyer@gmail.com
 * www.hxflyer.com
 **/




/**
 * DisplayObject is the basic class in rendering tree, each visible object on stage must extends from this Class
 * this Class has alot uncomplete methods, the updateTouchs(), updateBoundRect() and hitTestMouseDispatch() are 
 * not fully functional, 
 * nomrally this Class should not be directly use as parent Class by anybody, if you want to make a functional 
 * display Class you should extend it from Sprite
 **/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayObject : EventDispatcher {
	

	public DisplayObject () {
	}
	
	
	// tag: used for storing any variable
	public object tag;
	
	//when mouse enable == alse, it will mute all touch events and mouse events
	public bool mouseEnable	= true;
	
	
	public virtual void destroy(){
		parent.removeChild(this);
	}
	
	public virtual void dispatchEnterFrame(){
		this.dispatchEvent(new GuiEvent(GuiEvent.ENTER_FRAME));
		
	}
	/***************************************
	 * 2d transform
	 ***************************************/
	
	/*
	 * transform and transformInTree are 2d matrix
	 * transform is for store 2d rotation,scaling, movement relate to parent
	 * transformInTree is for store 2d rotation,scaling, movement relate to stage
	 */
	protected Transform2D	_transform	= new Transform2D();
	
	public Transform2D transform {
    	get { return _transform; }
	}
	

	protected Transform2D	_transformInTree;
	
	public Transform2D transformInTree {
    	get { return _transformInTree; }
	}
	
	
	protected Transform2D	_transformInTreeInverted;
	public Transform2D transformInTreeInverted{
		get {	
				if(_transformInTreeInverted == null){
					_transformInTreeInverted = _transformInTree.getInverted();
				}
				return _transformInTreeInverted;
			}
	}
	
	
	//update transform
	
	protected bool _isTransformDirty	= true;
	
	public void setTransFormDirty(){
		_isTransformDirty	= true;
	}
	
	public virtual void updateTransform(){
		_transform.identity();
		_transform.setTranslate(_x,_y);
		_transform.rotate(_rotation/Mathf.Rad2Deg);
		_transform.scale(_scaleX,_scaleY);
	}
	
	
	//trnsform in render tree
	 /*
	  * sometimes transformInTree need to be update but transform don't need, so these 2 update functions are separated
	 */
	protected bool _isTransformInTreeDirty	= true;
	
	public virtual void setTransformInTreeDirty(){
		_isTransformInTreeDirty	= true;
	}
	
	//scale and rotation are both can be get from transformInTree, but it will need trigonometric calculation
	//directly cache these as variable is cheaper
	
	protected Vector2 _transformInTreeScale;
	public Vector2 transformInTreeScale{
		get {return _transformInTreeScale;}
		private set {_transformInTreeScale = value;}
	}
	
	protected float	_transformInTreeRotation = 0;
	public float transformInTreeRotation{
		get {return _transformInTreeRotation;}
		private set {_transformInTreeRotation = value;}
	}

	public virtual void updateTransformInTree(){
		
		if(_parent==null){
			_transformInTree.identity();
			_transformInTreeScale.x	= _scaleX;
			_transformInTreeScale.y	= _scaleY;
			_transformInTreeInverted= null;
		}else{
			_transformInTree		= Transform2D.multiply(_parent.transformInTree,_transform);
			_transformInTreeScale.x	= _scaleX*_parent.transformInTreeScale.x;
			_transformInTreeScale.y	= _scaleY*_parent.transformInTreeScale.y;
			_transformInTreeRotation= _rotation + _parent.transformInTreeRotation;
			_transformInTreeInverted= null;
		}
	}
	
	
	public virtual void updateRelatedBoundRect(){
		updateBoundRect();
		if(_parent!=null){
			_parent.updateParentBoundRect();
		}
	}
	public void updateParentBoundRect(){
		updateBoundRect();
		if(_parent!=null){
			_parent.updateParentBoundRect();
		}
	}
	
	public virtual void updateBoundRectInTree(){
	}
	// position , scale , size
	// transfrom can not be modified from outsied
	protected float _x = 0.0f;
	public virtual float x {
    	get { return _x; }
    	set { 
			_x = value;
			_transform.setTranslateX(_x);
			updateTransformInTree();
			updateRelatedBoundRect();
			
		}
	}
	
	protected float _y = 0.0f;
	public virtual float y {
    	get { return _y; }
    	set { 
			_y = value;
			_transform.setTranslateY(_y);
			updateTransformInTree();
			updateRelatedBoundRect();
			
		}
	}
	
	protected float _width = 0.0f;
	public virtual float width {
		get { return _width; }
    	set {
			_width = value;
			if(_originalWidth!=0){
				_scaleX	= _width/_originalWidth;
			}
			updateTransform();
			updateTransformInTree();
			updateRelatedBoundRect();
			
		}
	}
	
	protected float _height = 0.0f;
	public virtual float height {
    	get { return _height; }
    	set {
			_height = value;
			if(_originalHeight!=0){
				_scaleY	= _height/_originalHeight;
			}
			updateTransform();
			updateTransformInTree();
			updateRelatedBoundRect();
			
		}
	}
	
	protected float _scaleX = 1.0f;
	public virtual float scaleX {
    	get { return _scaleX; }
    	set { 
			_scaleX = value;
			if(_originalWidth!=0){
				_width	= _scaleX*_originalWidth;
			}
			updateTransform();
			updateTransformInTree();
			updateRelatedBoundRect();
			
		}
	}
	
	protected float _scaleY = 1.0f;
	public virtual float scaleY {
    	get { return _scaleY; }
    	set {
			_scaleY = value;
			if(_originalHeight!=0){
				_height	= _scaleY*_originalHeight;
			}
			updateTransform();
			updateTransformInTree();
			updateRelatedBoundRect();
			
		}
	}
	
	protected float _rotation;
	public virtual float rotation {
    	get { return _rotation; }
    	set {
			_rotation = value;
			updateTransform();
			updateTransformInTree();
			updateRelatedBoundRect();
			
		}
	}
	
	
	
	/***************************************
	 * original size
	 ***************************************/
	
	protected float _originalWidth	= 0;
	protected float _originalHeight	= 0;
	
	public float originalWidth{
		get {return _originalWidth;}
	}
	
	public float originalHeight{
		get {return _originalHeight;}
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
	
	protected Rect _boundRectInTree  = new Rect();
	
	public Rect boundRectInTree {
    	get { return _boundRectInTree; }
    	private set { _boundRectInTree = value;}
	}
	
	protected Rect _selfBoundRect	= new Rect();
	
	public Rect selfBoundRect {
    	get { return _selfBoundRect; }
    	private set { _selfBoundRect = value;}
	}
	
	public virtual void updateBoundRect(){
		if(_parent!=null){
			_parent.updateBoundRect();
		}
		//need to be override
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
		//need to be override
	}
	
	/***************************************
	 * rendering tree
	 ***************************************/
	
	protected DisplayObjectContainer _parent;

	
	public DisplayObjectContainer parent {
    	get { return _parent; }
    	set { 
				_parent = value;
				updateRelatedBoundRect();
			}
	}
	
	protected Stage	_stage;
	
	public Stage stage {
    	get { return _stage; }
    	set { 
				_stage = value;
			}
	}
	
	/***************************************
	 * hit test
	 ***************************************/
	
	public virtual bool hittest(Vector2 vec){
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		return (_selfBoundRect.Contains(vec));
	}
	
	
	public virtual bool hitTestMouseDispatch(string type,Vector2 vec){
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		bool isHit	= false;
		if(!_selfBoundRect.Contains(newVec)){
			return false;
		}
		
		if(isHit){
			this.dispatchEvent(new MouseEvent(type,newVec,vec));
		}
		return isHit;
	}
	
	//the touch instance will be cached in array if hit test valid
	public virtual bool hitTestTouchDispatch(string type,Touch touch){
		if(!mouseEnable || !_visible){
			return false;
		}
		Vector2 vec = new Vector2(touch.position.x,Stage.instance.stageHeight- touch.position.y);
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		bool isHit	= false;
		if(!_selfBoundRect.Contains(newVec)){
			return false;
		}
		
		if(isHit){
			_touchList.Add(touch);
			this.dispatchEvent(new TouchEvent(type,touch));
		}
		return isHit;
	}
	
	
	/***************************************
	 * touches
	 ***************************************/
	
	protected List<Touch> _touchList	= new List<Touch>();
	
	protected int _swipeCounter		= 0;
	protected string _swipeDirection;
	
	public virtual void clearTouchs(){
		_touchList.Clear();
	}
	
	public virtual void updateTouchs(){
		//need to be update
	}
}
