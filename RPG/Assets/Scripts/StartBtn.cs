using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBtn : MonoBehaviour {

	public void OnClick()
	{
		if (GameManager.instance.CheckEnd()) {
			GameManager.instance.routeTurn = false;
			GameManager.instance.ControllMove();
		}
		else
		{
			GameManager.instance.ErrorMessage();
		}    
	}
}
