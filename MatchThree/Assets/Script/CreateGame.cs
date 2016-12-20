using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tiles
{
	public GameObject tile_Obj;
	public string type;
	public Tiles(GameObject tile, string t)
	{
		tile_Obj=tile;
		type=t;
	}

}

public class CreateGame : MonoBehaviour {


	[SerializeField] int rows;
	[SerializeField] int cols;

	[SerializeField] GameObject[] tile;
	List<GameObject> tile_Bank = new List<GameObject>();

	HashSet<GameObject> tileSelect = new HashSet<GameObject> ();

	bool renewBoard=false;

	Tiles[,] tiles;

	void ShuffleList()
	{
		System.Random rand = new System.Random ();
		int r = tile_Bank.Count;
		while (r > 1) {
			r--;
			int n = rand.Next (r + 1);
			GameObject val = tile_Bank [n];
			tile_Bank[n] = tile_Bank[r];
			tile_Bank [r] = val;		
			
		}
	}

	// Use this for initialization
	void Start () {
		tiles = new Tiles[cols, rows];
		int num_Copies = (rows * cols) / 3;
		for (int i = 0; i < num_Copies; i++) {
			for (int j = 0; j < tile.Length; j++) {
				GameObject o = (GameObject)Instantiate (tile [j], new Vector3 (-10, -10, 0), tile [j].transform.rotation);
				o.SetActive(false);
				tile_Bank.Add (o);					
			}
			
		}
		//shuffle list biar random;
		ShuffleList();

		//buat tile grid
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {

				Vector3 tile_Pos = new Vector3 (col, row, 0);
				for (int n = 0; n < tile_Bank.Count; n++) {
					GameObject o = tile_Bank [n];
					if (!o.activeSelf) {
						o.transform.position = new Vector3 (tile_Pos.x, tile_Pos.y, tile_Pos.z);
						o.SetActive (true);
						tiles [col, row] = new Tiles (o, o.name);
						n = tile_Bank.Count + 1;
					}
				
				}
			}
			
		}

	
	}
	
	// Update is called once per frame
	void Update () {
		CheckGrid ();
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection (ray, 1000);

			if (hit) {
				tileSelect.Add(hit.collider.gameObject);

			}

		
		} else if (Input.GetMouseButtonUp (0) && tileSelect.Count>0) {
			FlipTile (tileSelect); // men-flip tile yang telah dipilih
			tileSelect.Clear ();
		
		}
			
	
	}

	void FlipTile(HashSet<GameObject> hash)
	{
		foreach (GameObject i in hash) {
			//ubah tile ke lain
			tiles[(int)i.transform.position.x,(int)i.transform.position.y].tile_Obj.SetActive(false); // cuma untuk test
			tiles[(int)i.transform.position.x,(int)i.transform.position.y]=null;
		}
		renewBoard = true; //untuk test
	}

	void CheckGrid()
	{
		int counter = 1;
		//check kolom
		for (int row = 0; row < rows; row++) {
			counter = 1;
			for (int col = 1; col < cols; col++) {
				if (tiles [col, row] != null && tiles [col - 1, row] != null) {
					//jika tiles aktif
					if (tiles [col, row].type == tiles [col - 1, row].type) {
						counter++;
					} else {
						//jika ada 3 atau lebih apus 

						if (counter >= 3) {
							//Debug.Log (counter+" " +col+" "+row);
							AddPointsCol (counter,col-1,row);
							renewBoard = true;
						}	
						counter = 1; //reset counter
					}
				}
			}
		}
		// check Baris
		for (int col = 0; col < cols; col++) {
			counter = 1;
			for (int row = 1; row < rows; row++) {

				if (tiles [col, row] != null && tiles [col , row-1] != null) {
					//jika tiles aktif
					if (tiles [col, row].type == tiles [col , row-1].type) {
						counter++;
					} else {
						//jika ada 3 atau lebih apus 
						if (counter >= 3) {
							//Debug.Log("row "+counter);
							AddPointsRow (counter,col,row-1);
							renewBoard = true;
						}
						counter = 1; //reset counter
					}

				}

			}

		}
		if (renewBoard) {
			RenewGrid ();
			renewBoard=false;
		}

	}

	void AddPointsCol(int counter,int col,int row)
	{
		//Debug.Log ("jajaj");
		for (int i = 0; i < counter; i++) {
			if (tiles [col-i, row] != null) {
				tiles [col-i, row].tile_Obj.SetActive (false);
			}
			tiles [col - i, row] = null;
		}


	}
	void AddPointsRow(int counter,int col,int row)
	{
		for (int i = 0; i < counter; i++) {
			if (tiles [col, row-i] != null) {
				tiles [col, row-i].tile_Obj.SetActive (false);
			}
			tiles [col, row-i] = null;
		}
	}

	void RenewGrid()
	{
		bool anyMoved = false;
		ShuffleList ();
		for (int row = 1; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				//jika di paling atas dan gk ada tile
				if (row == rows - 1 && tiles [col, row] == null) {
					Vector3 tile_Pos = new Vector3 (col, row, 0);
					for (int n = 0; n < tile_Bank.Count; n++) {
						GameObject o = tile_Bank [n];
						if (!o.activeSelf) {
							o.transform.position = new Vector3 (tile_Pos.x, tile_Pos.y, tile_Pos.z);
							o.SetActive (true);
							tiles [col, row] = new Tiles (o, o.name);
							n = tile_Bank.Count + 1;
						}
					}
				}
				if (tiles [col, row] != null) {
					//kalau dibawahnya kosong maka turun
					if (tiles [col, row - 1] == null) {
						tiles [col, row - 1] = tiles [col, row];
						tiles [col, row].tile_Obj.transform.position = new Vector3 (col, row - 1, 0);
						tiles [col, row] = null;
						anyMoved = true;
					}
				}
			}
		}
		if (anyMoved) {
			Invoke ("RenewGrid", 0.5f);// kasi delay biar cakep

		}

	}
}
