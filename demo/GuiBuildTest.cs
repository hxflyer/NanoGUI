using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	
	
	private EquipmentBar	_equipmentBar;
	
	void Start () {

		Texture2D texture = Resources.Load("testimg",typeof(Texture2D)) as Texture2D;
		
		_equipmentBar	= new EquipmentBar();
		
		Stage.instance.addChild(_equipmentBar);
	}
	
	
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(bigimg.textureRenderRect);
		//bigimg.rotation++;
	}
 
}
