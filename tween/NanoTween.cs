using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NanoTween : EventDispatcher {

	
	public enum LoopType{
		none,
		loop,
		pingPong
	}
	
	public static string ON_COMPLETE = "onComplete";
	public static string ON_START	 = "onStart";
	public static string ON_UPDATE	 = "onUpdate";
	public static string ON_COMPLETE_PARAMS = "onCompleteParams";
	public static string ON_START_PARAMS	= "onStartParams";
	public static string ON_UPDATE_PARAMS	= "onUpdateParams";
	
	public delegate void CallBack(object[] args);
	
	
	public static NanoTween to(object target,float time,params object[] args){
		NanoTween tween = new NanoTween(target,time,args);
		AddTween(tween);
		return tween;
	}
	
	
	public static void removeTweenOf(object target){
		int i	= 0;
		while (i< _tweenList.Count){
			NanoTween tween	= _tweenList[i];
			if(tween.target == target){
				RemoveTween(tween);
			}else{
				i++;
			}
		}
	}
	
	public static NanoDelayCall delayCall(float time,object target,CallBack delayCallBack,object[] args){
		NanoDelayCall delaycall	= new NanoDelayCall(time,target,delayCallBack,args);
		_delayCallList.Add(delaycall);
		return delaycall;
	}
	
	public static NanoDelayCall delayCall(float time,object target,CallBack delayCallBack){
		NanoDelayCall delaycall	= new NanoDelayCall(time,target,delayCallBack);
		_delayCallList.Add(delaycall);
		return delaycall;
	}
	
	public static void removeDelayCallOf(object target){
		int i	= 0;
		while (i< _tweenList.Count){
			NanoDelayCall delaycall	= _delayCallList[i];
			if(delaycall.target == target){
				RemoveDelayCall(delaycall);
			}else{
				i++;
			}
		}
	}
	
	
	
	public static object[] Pack(params object[] args){
		return args;
	}	
	
	/*
	 * ease is for providing ease type and ease function
	 * Making this class as singleton instance is not on purpose, just because I'm new in C#
	 * this is the best way I can make the compilor work
	 */
	
	private static Ease _ease	= new Ease();
	
	/*
	 * every tween instance will get a unique id from this counter
	 */
	private static uint _tweenCount 	= 0;
	
	/*
	 * all tween instances are stored in this array list;
	 */
	
	private static List<NanoTween> _tweenList 			= new List<NanoTween>();
	private static List<NanoDelayCall> _delayCallList 	= new List<NanoDelayCall>();
	
	private static void AddTween(NanoTween tween){
		_tweenList.Add(tween);
	}
	
	private static void RemoveTween(NanoTween tween){
		_tweenList.Remove(tween);
	}
	private static void RemoveDelayCall(NanoDelayCall delaycall){
		_delayCallList.Remove(delaycall);
	}
	/*
	 * update need to be call every frame;
	 */
	public static void update(){
		int i	= 0;
		while (i< _tweenList.Count){
			NanoTween tween	= _tweenList[i];
			tween.updateTween();
			if(tween.percentage>=1){
				RemoveTween(tween);
			}else{
				i++;
			}
		}
		i=0;
		while (i< _delayCallList.Count){
			NanoDelayCall delayCall	= _delayCallList[i];
			delayCall.updateDelayCall();
			if(delayCall.percentage>=1){
				RemoveDelayCall(delayCall);
			}else{
				i++;
			}
		}
	}
	
	
	/*
	 * instance intiation
	 */
	public NanoTween(object target,float time,params object[] args) {
		_target			= target;
		_time			= time;
		_easeFunction	= _ease.defaultEasingFunction;
		
		id				= _tweenCount.ToString();
		
		_currentTime	= 0.0f;
		_startTime		= Time.time;
		_endTime		= _time;
		
		int i = 0;
		string argName;
		_argsName		= new ArrayList();
		_endArgs		= new ArrayList();
		_startArgs		= new ArrayList();
		
		while(i < args.Length - 1) {
			argName	= args[i] as string;
			if(argName	== "ease"){
				_easeType		= args[i+1] as string;
				_easeFunction	= _ease.getEasingFunction(_easeType);
			}else if(argName == "delay"){
				_delay	= (float)args[i+1];
			}else if(argName == ON_COMPLETE){
				_onCompleteCallBack	= args[i+1] as CallBack;
			}else if(argName == ON_COMPLETE_PARAMS){
				_onCompleteParams	= args[i+1] as object[];
			}else if(argName == ON_START){
				_onStartCallBack	= args[i+1] as CallBack;
			}else if(argName == ON_START_PARAMS){
				_onStartParams	= args[i+1] as object[];
			}else if(argName == ON_UPDATE){
				_onUpdateCallBack	= args[i+1] as CallBack;
			}else if(argName == ON_UPDATE_PARAMS){
				_onUpdateParams	= args[i+1] as object[];
			}else{
				_argsName.Add(argName);
				_endArgs.Add(args[i+1]);
				_startArgs.Add(_target.GetType().GetProperty(argName).GetValue(_target,null));
				
			}
			i += 2;
		}
		_tweenCount++;
	}
	
	
	
	/*
	 * instance variable
	 */
	
	private object _target;
	public object target{
		get {return _target;}
	}
	
	private float _time	= 1;
	public float time{
		get {return _time;}
	}
	
	private string _easeType;
	public string easeType{
		get {return _easeType;}
	}
	
	private Ease.EasingFunction _easeFunction;
	public Ease.EasingFunction easeFunction{
		get {return _easeFunction;}
	}
	

	private ArrayList _startArgs;
	private ArrayList _endArgs;
	private ArrayList _argsName;
	
	
	
	//call backs
	private CallBack _onCompleteCallBack;
	private object[] _onCompleteParams;
	
	private CallBack _onStartCallBack;
	private object[] _onStartParams;
	
	private CallBack _onUpdateCallBack;
	private object[] _onUpdateParams;
	
	//time
	private float _delay	= 0;
	private float _currentTime;
	private float _startTime;
	private float _endTime;
	
	private float _percentage;
	public float percentage{
		get {return _percentage;}
	}
	
	
	//this will be call each frame
	
	private void updateTween(){
		
		if(_delay>0){
			_delay -=Time.deltaTime;
			if(_delay<=0){
				for (int i=0;i<_startArgs.Count;i++){
					_startArgs[i] = _target.GetType().GetProperty( _argsName[i] as string ).GetValue(_target,null);
				}
				if(_onStartCallBack!=null){
					_onStartCallBack(_onStartParams);
				}
			}
			return;
		}
		
		_currentTime += Time.deltaTime;
		_percentage	= _currentTime/_endTime;
		
		if(_percentage<1){
			for (int i=0;i<_startArgs.Count;i++){
				_target.GetType().GetProperty( _argsName[i] as string ).SetValue(target, _easeFunction( (float)_startArgs[i], (float)_endArgs[i], _percentage ), null);
			}
			if(_onUpdateCallBack!=null){
				_onUpdateCallBack(_onUpdateParams);
			}
		}else{
			for (int i=0;i<_startArgs.Count;i++){
				_target.GetType().GetProperty( _argsName[i] as string ).SetValue( target, _endArgs[i], null );
			}
			if(_onCompleteCallBack!=null){
				_onCompleteCallBack(_onCompleteParams);
			}
		}
	}
	
}
