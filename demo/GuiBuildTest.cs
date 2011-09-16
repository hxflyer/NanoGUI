using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	public Sprite s;
	void Start () {
		Texture2D texture = Resources.Load("testimg",typeof(Texture2D)) as Texture2D;
		s	= new Sprite(texture);
		GuiManager.stage.addChild(s);
	}
	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log(bigimg.textureRenderRect);
		//bigimg.rotation++;
	}
 
}
