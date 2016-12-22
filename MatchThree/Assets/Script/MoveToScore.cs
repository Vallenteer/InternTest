using UnityEngine;
using System.Collections;

public class MoveToScore : MonoBehaviour {

	GameObject DestObj;
	Vector3 Dest, nowPos;

	[SerializeField] float timeMove = 1f;
	private float moveTimer= 0;
	private bool isMove = false;
	// Use this for initialization
	void Start () {
		DestObj = GameObject.FindWithTag("Score");
		Dest = DestObj.gameObject.transform.position;
	}

	private void StartMove()
	{
		
		isMove = true;
		moveTimer = 0;

		nowPos = gameObject.transform.position;
		
	}

	private void StopMove()
	{
		isMove = false;
		gameObject.transform.position = Dest;
	}

	// Update is called once per frame
	void Update () {
		StartMove ();
		if (isMove) {
			moveTimer += Time.deltaTime;
			if (moveTimer > timeMove) {
				StopMove ();
			} else {
				float ratio = moveTimer / timeMove;
				gameObject.transform.position = Vector3.Lerp (nowPos, Dest, ratio);
			}
		}
	
	}
}
