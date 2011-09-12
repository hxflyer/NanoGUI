using UnityEngine;
using System.Collections;

/**
 * most part of this class is copy from iTween
 */
public class Ease {
	
	public Ease () {
		_defaultEasingFunction	= new EasingFunction(linearFunction);
	}
	
	public delegate float EasingFunction(float start, float end, float value);
	
	private EasingFunction _defaultEasingFunction ;
	
	public EasingFunction defaultEasingFunction{
		get {return _defaultEasingFunction;}
	}
	
	
	public const  string easeInQuad	= "easeInQuad";
	public const  string easeOutQuad = "easeOutQuad";
	public const  string easeInOutQuad = "easeInOutQuad";
	public const  string easeInCubic = "easeInCubic";
	public const  string easeOutCubic = "easeOutCubic";
	public const  string easeInOutCubic = "easeInOutCubic";
	public const  string easeInQuart = "easeInQuart";
	public const  string easeOutQuart = "easeOutQuart";
	public const  string easeInOutQuart = "easeInOutQuart";
	public const  string easeInQuint = "easeInQuint";
	public const  string easeOutQuint = "easeOutQuint";
	public const  string easeInOutQuint = "easeInOutQuint";
	public const  string easeInSine = "easeInSine";
	public const  string easeOutSine = "easeOutSine";
	public const  string easeInOutSine = "easeInOutSine";
	public const  string easeInExpo = "easeInExpo";
	public const  string easeOutExpo = "easeOutExpo";
	public const  string easeInOutExpo = "easeInOutExpo";
	public const  string easeInCirc = "easeInCirc";
	public const  string easeOutCirc = "easeOutCirc";
	public const  string easeInOutCirc = "easeInOutCirc";
	public const  string linear = "linear";
	public const  string spring = "spring";
/* GFX47 MOD START */
//bounce";
	public const  string easeInBounce = "easeInBounce";
	public const  string easeOutBounce = "easeOutBounce";
	public const  string easeInOutBounce = "easeInOutBounce";
/* GFX47 MOD END */
	public const  string easeInBack = "easeInBack";
	public const  string easeOutBack = "easeOutBack";
	public const  string easeInOutBack = "easeInOutBack";
/* GFX47 MOD START */
//elastic";
	public const  string easeInElastic= "easeInElastic";
	public const  string easeOutElastic= "easeOutElastic";
	public const  string easeInOutElastic = "easeInOutElastic";
/* GFX47 MOD END */
	public const  string punch = "punch";
	
	
	
	public EasingFunction getEasingFunction(string type){
		EasingFunction ease	= _defaultEasingFunction;
		switch (type){
			case easeInQuad:
				ease = new EasingFunction(easeInQuadFunction);
				break;
			case easeOutQuad:
				ease = new EasingFunction(easeOutQuadFunction);
				break;
			case easeInOutQuad:
				ease = new EasingFunction(easeInOutQuadFunction);
				break;
			case easeInCubic:
				ease = new EasingFunction(easeInCubicFunction);
				break;
			case easeOutCubic:
				ease = new EasingFunction(easeOutCubicFunction);
				break;
			case easeInOutCubic:
				ease = new EasingFunction(easeInOutCubicFunction);
				break;
			case easeInQuart:
				ease = new EasingFunction(easeInQuartFunction);
				break;
			case easeOutQuart:
				ease = new EasingFunction(easeOutQuartFunction);
				break;
			case easeInOutQuart:
				ease = new EasingFunction(easeInOutQuartFunction);
				break;
			case easeInQuint:
				ease = new EasingFunction(easeInQuintFunction);
				break;
			case easeOutQuint:
				ease = new EasingFunction(easeOutQuintFunction);
				break;
			case easeInOutQuint:
				ease = new EasingFunction(easeInOutQuintFunction);
				break;
			case easeInSine:
				ease = new EasingFunction(easeInSineFunction);
				break;
			case easeOutSine:
				ease = new EasingFunction(easeOutSineFunction);
				break;
			case easeInOutSine:
				ease = new EasingFunction(easeInOutSineFunction);
				break;
			case easeInExpo:
				ease = new EasingFunction(easeInExpoFunction);
				break;
			case easeOutExpo:
				ease = new EasingFunction(easeOutExpoFunction);
				break;
			case easeInOutExpo:
				ease = new EasingFunction(easeInOutExpoFunction);
				break;
			case easeInCirc:
				ease = new EasingFunction(easeInCircFunction);
				break;
			case easeOutCirc:
				ease = new EasingFunction(easeOutCircFunction);
				break;
			case easeInOutCirc:
				ease = new EasingFunction(easeInOutCircFunction);
				break;
			case linear:
				ease = new EasingFunction(linearFunction);
				break;
			case spring:
				ease = new EasingFunction(springFunction);
				break;
			/* GFX47 MOD START */
			case easeInBounce:
				ease = new EasingFunction(easeInBounceFunction);
				break;
			case easeOutBounce:
				ease = new EasingFunction(easeOutBounceFunction);
				break;
			case easeInOutBounce:
				ease = new EasingFunction(easeInOutBounceFunction);
				break;
			/* GFX47 MOD END */
			case easeInBack:
				ease = new EasingFunction(easeInBackFunction);
				break;
			case easeOutBack:
				ease = new EasingFunction(easeOutBackFunction);
				break;
			case easeInOutBack:
				ease = new EasingFunction(easeInOutBackFunction);
				break;
			/* GFX47 MOD START */
			case easeInElastic:
				ease = new EasingFunction(easeInElasticFunction);
				break;
			case easeOutElastic:
				ease = new EasingFunction(easeOutElasticFunction);
				break;
			case easeInOutElastic:
				ease = new EasingFunction(easeInOutElasticFunction);
				break;
			/* GFX47 MOD END */
			}
		return ease;
	}
	
	#region Easing Curves
	
	private float linearFunction(float start, float end, float value){
		return Mathf.Lerp(start, end, value);
	}
	
	private float clerpFunction(float start, float end, float value){
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) / 2.0f);
		float retval = 0.0f;
		float diff = 0.0f;
		if ((end - start) < -half){
			diff = ((max - start) + end) * value;
			retval = start + diff;
		}else if ((end - start) > half){
			diff = -((max - end) + start) * value;
			retval = start + diff;
		}else retval = start + (end - start) * value;
		return retval;
    }

	private float springFunction(float start, float end, float value){
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}

	private float easeInQuadFunction(float start, float end, float value){
		end -= start;
		return end * value * value + start;
	}

	private float easeOutQuadFunction(float start, float end, float value){
		end -= start;
		return -end * value * (value - 2) + start;
	}

	private float easeInOutQuadFunction(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value + start;
		value--;
		return -end / 2 * (value * (value - 2) - 1) + start;
	}

	private float easeInCubicFunction(float start, float end, float value){
		end -= start;
		return end * value * value * value + start;
	}

	private float easeOutCubicFunction(float start, float end, float value){
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}

	private float easeInOutCubicFunction(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value + start;
		value -= 2;
		return end / 2 * (value * value * value + 2) + start;
	}

	private float easeInQuartFunction(float start, float end, float value){
		end -= start;
		return end * value * value * value * value + start;
	}

	private float easeOutQuartFunction(float start, float end, float value){
		value--;
		end -= start;
		return -end * (value * value * value * value - 1) + start;
	}

	private float easeInOutQuartFunction(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value * value + start;
		value -= 2;
		return -end / 2 * (value * value * value * value - 2) + start;
	}

	private float easeInQuintFunction(float start, float end, float value){
		end -= start;
		return end * value * value * value * value * value + start;
	}

	private float easeOutQuintFunction(float start, float end, float value){
		value--;
		end -= start;
		return end * (value * value * value * value * value + 1) + start;
	}

	private float easeInOutQuintFunction(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value * value * value + start;
		value -= 2;
		return end / 2 * (value * value * value * value * value + 2) + start;
	}

	private float easeInSineFunction(float start, float end, float value){
		end -= start;
		return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
	}

	private float easeOutSineFunction(float start, float end, float value){
		end -= start;
		return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
	}

	private float easeInOutSineFunction(float start, float end, float value){
		end -= start;
		return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
	}

	private float easeInExpoFunction(float start, float end, float value){
		end -= start;
		return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
	}

	private float easeOutExpoFunction(float start, float end, float value){
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
	}

	private float easeInOutExpoFunction(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
		value--;
		return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
	}

	private float easeInCircFunction(float start, float end, float value){
		end -= start;
		return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
	}

	private float easeOutCircFunction(float start, float end, float value){
		value--;
		end -= start;
		return end * Mathf.Sqrt(1 - value * value) + start;
	}

	private float easeInOutCircFunction(float start, float end, float value){
		value /= .5f;
		end -= start;
		if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
		value -= 2;
		return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
	}

	/* GFX47 MOD START */
	private float easeInBounceFunction(float start, float end, float value){
		end -= start;
		float d = 1f;
		return end - easeOutBounceFunction(0, end, d-value) + start;
	}
	/* GFX47 MOD END */

	/* GFX47 MOD START */
	private float easeOutBounceFunction(float start, float end, float value){
		value /= 1f;
		end -= start;
		if (value < (1 / 2.75f)){
			return end * (7.5625f * value * value) + start;
		}else if (value < (2 / 2.75f)){
			value -= (1.5f / 2.75f);
			return end * (7.5625f * (value) * value + .75f) + start;
		}else if (value < (2.5 / 2.75)){
			value -= (2.25f / 2.75f);
			return end * (7.5625f * (value) * value + .9375f) + start;
		}else{
			value -= (2.625f / 2.75f);
			return end * (7.5625f * (value) * value + .984375f) + start;
		}
	}
	/* GFX47 MOD END */

	/* GFX47 MOD START */
	private float easeInOutBounceFunction(float start, float end, float value){
		end -= start;
		float d = 1f;
		if (value < d/2) return easeInBounceFunction(0, end, value*2) * 0.5f + start;
		else return easeOutBounceFunction(0, end, value*2-d) * 0.5f + end*0.5f + start;
	}
	/* GFX47 MOD END */

	private float easeInBackFunction(float start, float end, float value){
		end -= start;
		value /= 1;
		float s = 1.70158f;
		return end * (value) * value * ((s + 1) * value - s) + start;
	}

	private float easeOutBackFunction(float start, float end, float value){
		float s = 1.70158f;
		end -= start;
		value = (value / 1) - 1;
		return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
	}

	private float easeInOutBackFunction(float start, float end, float value){
		float s = 1.70158f;
		end -= start;
		value /= .5f;
		if ((value) < 1){
			s *= (1.525f);
			return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
		}
		value -= 2;
		s *= (1.525f);
		return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
	}

	private float punchFunction(float amplitude, float value){
		float s = 9;
		if (value == 0){
			return 0;
		}
		if (value == 1){
			return 0;
		}
		float period = 1 * 0.3f;
		s = period / (2 * Mathf.PI) * Mathf.Asin(0);
		return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
    }
	
	/* GFX47 MOD START */
	private float easeInElasticFunction(float start, float end, float value){
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d) == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
			}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		return -(a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
	}		
	/* GFX47 MOD END */

	/* GFX47 MOD START */
	private float easeOutElasticFunction(float start, float end, float value){
	/* GFX47 MOD END */
		//Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d) == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
			}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
	}		
	
	/* GFX47 MOD START */
	private float easeInOutElasticFunction(float start, float end, float value){
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s = 0;
		float a = 0;
		
		if (value == 0) return start;
		
		if ((value /= d/2) == 2) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
			}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
		return a * Mathf.Pow(2, -10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
	}		
	/* GFX47 MOD END */
	
	#endregion	
}
