using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

	[SerializeField] Text MovesText;
	[SerializeField] Text TilesToGo;

	[SerializeField] int move;
	[SerializeField] int Finish;

	public static int MoveLeft;
	public static int tileforFinish;

	// Use this for initialization
	void Start () {
		MoveLeft=move;
		tileforFinish = Finish;
		Camera.main.aspect = 10f / 16f;
	
	}

	public static void Move()
	{
		MoveLeft--;
	}

	public static void TileLeft()
	{
		if (tileforFinish > 0) {
			tileforFinish--;
		}
	}

	// Update is called once per frame
	void Update () {

		MovesText.text = MoveLeft.ToString ();
		TilesToGo.text = tileforFinish.ToString ();
	
	}
}
