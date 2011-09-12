using UnityEngine;
using System.Collections;

/*
 * Transform2D is a 2d matrix contribute for rotation/translation/scaling
 * component structure:
 *  | a c tx |
 *  | b d ty |
 *  | u v w  |
 */

public class Transform2D : Object {

	
	
	public Transform2D () {
	
	}
	public DisplayObject target;
	
	public float a	= 1.0f;
	
	public float b	= 0.0f;
	
	public float c	= 0.0f;
	
	public float d	= 1.0f;
	
	public float tx = 0.0f;
	
	public float ty = 0.0f;
	
	public float u	= 0.0f;
	
	public float v	= 0.0f;
	
	public float w	= 1.0f;
	
	
	/*
	 * reset to identity
	 * this is nessary when every time recalculate this matrix
	 */	
	
	public void identity(){
		a	= 1.0f;
		b	= 0.0f;
		c	= 0.0f;
		d	= 1.0f;
		tx	= 0.0f;
		ty	= 0.0f;
		u	= 0.0f;
		v	= 0.0f;
		w	= 1.0f;
	}
	
	
	
	//translation
	
	public void translateX(float t){
		tx	+= t;
	}
	public void setTranslateX(float t){
		tx	= t;
	}
	public void translateY(float t){
		ty	+= t;
	}
	public void setTranslateY(float t){
		ty	= t;
	}
	public void setTranslate(float x,float y){
		tx	= x;
		ty	= y;
	}
	
	
	
	//scaling
	/*public void scaleX(float scale){
		a	*= scale;
	}
	
	public void scaleY(float scale){
		d	*= scale;
	}*/
	public void scale(float x,float y){
		
		a	= a * x;
		b	= b * x;
		c	= c * y;
		d	= d * y;
	}
	
	
	
	
	//rotation
	
	/*public void setRotation(float rad){
		float cos = Mathf.Cos(rad);
		float sin = Mathf.Sin(rad);
		a	= cos;
		b	= sin;
		c	= -sin;
		d	= cos;
	}*/
	public void rotate(float rad){
		float cos	= Mathf.Cos(rad);
		float sin	= Mathf.Sin(rad);
		
		float ra	= a * cos  +  c * sin ;
		float rc	= -a * sin  +  c * cos ;
		float rb	= b * cos  +  d * sin ;
		float rd	= -b * sin  +  d * cos ;

		a	= ra;
		b	= rb;
		c	= rc;
		d	= rd;
	}
	
	
	
	
	
	/*
	 * invert
	 * Transform2D matrix can only be rotate once;
	 */
	
	public void invert(){
		float A	= d;
		float B	= -b;
		//float C = 0;
		float D	= -c;
		float E = a;
		//float F = 0;
		float G = c*ty-tx*d;
		float H = tx*b-a*ty;
		float K	= a*d- c*b;
		float det = 1/(a*A + c*B);
		a	= det * A;
		b	= det * B;
		c	= det * D;
		d	= det * E;
		tx	= det * G;
		ty	= det * H;
		u	= 0;//det * C;
		v	= 0;//det * F;
		w	= det * K;
		/*
		 * | A D G |
		 * | B E H |
		 * | C F K |
		 * 
		 */ 
	}
	
	
	
	//mutiply
	
	public static Transform2D multiply(Transform2D mtx1,Transform2D mtx2){

		Transform2D	result	= new Transform2D();
		
		result.a	= mtx1.a * mtx2.a  +  mtx1.c * mtx2.b ;
		result.c	= mtx1.a * mtx2.c  +  mtx1.c * mtx2.d ;
		result.tx	= mtx1.a * mtx2.tx  +  mtx1.c * mtx2.ty  +  mtx1.tx ;
		
		result.b	= mtx1.b * mtx2.a  +  mtx1.d * mtx2.b ;
		result.d	= mtx1.b * mtx2.c  +  mtx1.d * mtx2.d ;
		result.ty	= mtx1.b * mtx2.tx  +  mtx1.d * mtx2.ty  +  mtx1.ty ;
		
		result.u	= 0 ;
		result.v	= 0 ;
		result.w	= 1 ;
		
		return result;
	}
	
	
}
