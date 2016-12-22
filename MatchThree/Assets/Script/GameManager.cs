using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

	[SerializeField] Text MovesText;
	[SerializeField] Text TilesToGo;

	[SerializeField] GameObject GameOverText;

	[SerializeField] int move;
	[SerializeField] int Finish;

	//power up
	public static bool MinePowerUp=false;

	public static bool GameOver=false;
	public static int MoveLeft;
	public static int tileforFinish;

	// Use this for initialization
	void Start () {
		if(GameOverText!=null)
			GameOverText.SetActive (false);
		GameOver=false;
		MoveLeft=move;
		tileforFinish = Finish;
		Camera.main.aspect = 10f / 16f;
	
	}

	public  void MineSet()
	{
		if (MinePowerUp) {
			MinePowerUp = false;
		} else{
			MinePowerUp = true;
		}
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

		if (MovesText != null || TilesToGo != null) {
			MovesText.text = MoveLeft.ToString ();
			TilesToGo.text = tileforFinish.ToString ();
		}if (GameOver) {
			GameOverText.SetActive (true);
		}
	
	}

	public void PlayButton()
	{
		SceneManager.LoadScene (1);
	}

	public void BacktoMenu()
	{
		SceneManager.LoadScene (0);
	}

	public void quit()
	{
		Application.Quit ();
	}
}
