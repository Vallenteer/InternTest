using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PowerUpButtonBev : MonoBehaviour {

	[SerializeField] Sprite spriteOn;
	[SerializeField] Sprite spriteOff;
	
	// Update is called once per frame
	void Update () {
		if (GameManager.MinePowerUp) {
			gameObject.GetComponent<Button> ().image.overrideSprite = spriteOn;	
		} else {

			gameObject.GetComponent<Button> ().image.overrideSprite = spriteOff;
		}
			
	
	}
}
