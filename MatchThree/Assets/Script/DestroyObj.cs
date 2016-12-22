using UnityEngine;
using System.Collections;

public class DestroyObj : MonoBehaviour {

	[SerializeField] ParticleSystem particle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//Debug.Log ("Hal");
		Destroy (col.gameObject);
		if (particle.isStopped) {
			particle.Play ();
		}
	}

}
