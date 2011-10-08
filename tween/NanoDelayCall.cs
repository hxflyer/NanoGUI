using UnityEngine;
using System.Collections;

public class NanoDelayCall:EventDispatcher  {

	private NanoTween.CallBack _delayCallBack;
	public NanoTween.CallBack time{
		get {return _delayCallBack;}
	}
	
	private object _target;
	public object target{
		get {return _target;}
	}
	private float _endTime	= 1;

	private float _currentTime = 0;
	
	private object[] _callbackParams;
	
	private float _percentage;
	public float percentage{
		get {return _percentage;}
	}
	
	public NanoDelayCall(float delayTime,object target, NanoTween.CallBack delayCallBack, object[] args){
		_target			= target;
		_endTime			= delayTime;
		_delayCallBack	= delayCallBack;
		_callbackParams	= args;
	}
	
	public void updateDelayCall(){
		_currentTime	+= Time.deltaTime;
		_percentage		= _currentTime/_endTime;
		if(_percentage>=1){
			_delayCallBack(_callbackParams);
		}
	}
}
