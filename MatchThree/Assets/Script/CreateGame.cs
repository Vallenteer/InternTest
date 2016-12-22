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
	[SerializeField] GameObject[] tile_moves;
	//0 - Blutile
	//1- Purtile
	//2- Siltile
	//3-Yetile
	List<GameObject> tile_Bank = new List<GameObject>();

	HashSet<GameObject> tileSelect = new HashSet<GameObject> ();
	HashSet<GameObject> temporaryTile= new HashSet<GameObject>();

	bool renewBoard=false;
	bool anyMoved = false;
	bool AnyMatch=false;
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
	void InitGrid()
	{
		//buat tile grid
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {

				Vector3 tile_Pos = new Vector3 (col, row, 0);
				for (int n = 0; n < tile_Bank.Count; n++) {
					GameObject o = tile_Bank [n];
					if (!o.activeSelf) {
						o.transform.position = new Vector3 (tile_Pos.x, tile_Pos.y, tile_Pos.z);
						o.SetActive (true);
						tiles [col, row] = new Tiles (o, o.gameObject.tag);
						n = tile_Bank.Count + 1;
					}

				}
			}

		}
	}
	void EmptyGrid()
	{
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				if (tiles [col, row] != null) {
					tiles [col, row].tile_Obj.SetActive (false);
				}
				tiles [col, row] = null;
						
					
			}				
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
		InitGrid ();	

	}
	
	// Update is called once per frame
	void Update () {
		
		CheckGrid ();
		//selama ada yang gerak dia gk mau jalan
		if (!anyMoved && GameManager.MoveLeft>0 && temporaryTile.Count<=0 ) {
			
			if (Input.GetMouseButton (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D hit = Physics2D.GetRayIntersection (ray, 1000);

				if (hit) {
					tileSelect.Add (hit.collider.gameObject);

				}

		
			} else if (Input.GetMouseButtonUp (0) && tileSelect.Count > 0) {

				FlipTile (tileSelect);
				tileSelect.Clear ();
				AnyMatch = CheckGrid ();
				if(!AnyMatch){
					Invoke("SelectTiles",1f);// men-flip tile yang telah dipilih
				}else {
					GameManager.Move();
					temporaryTile.Clear ();
				}

		
			}
			if (GameManager.MoveLeft <= 0 || GameManager.tileforFinish <= 0) {
				//GameOver
				GameManager.GameOver=true;
			}

		}

	
	}
	void SelectTiles()
	{
		FlipBackTile (temporaryTile);
		temporaryTile.Clear ();

	}

	void FlipTile(HashSet<GameObject> hash)
	{
		foreach (GameObject i in hash) {
			int col = (int)i.transform.position.x;
			int row= (int)i.transform.position.y;
			Vector3 tile_Pos = new Vector3 (col, row, 0);
			GameObject o=null;
			//ubah tile ke lain
			//Debug.Log(col+" +" +row);
			if (tiles [col,row].tile_Obj.tag == "Blutile") {
				//merubah dari Blue ke purple
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Purtile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [1], new Vector3 (-10, -10, 0), tile [1].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Purtile") &&  (x.activeSelf==false));
				}
			} else if (tiles [col,row].tile_Obj.tag == "Purtile") {
				//merubah dari purple to blue
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Blutile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [0], new Vector3 (-10, -10, 0), tile [0].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Blutile") &&  (x.activeSelf==false));
				}
			}else if (tiles [col,row].tile_Obj.tag == "Siltile") {
				//merubah dari sil to ye
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Yetile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [3], new Vector3 (-10, -10, 0), tile [3].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Yetile") &&  (x.activeSelf==false));
				}
			}else if (tiles[col,row].tile_Obj.tag == "Yetile") {
				//merubah dari ye ke sil
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Siltile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [2], new Vector3 (-10, -10, 0), tile [2].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Siltile") &&  (x.activeSelf==false));
				}
			}  

			o.transform.position = new Vector3 (tile_Pos.x, tile_Pos.y, tile_Pos.z);
			tiles [col, row].tile_Obj.SetActive (false);
			tiles [col, row].tile_Obj.transform.position = new Vector3 (-10, -10, 0);
			tiles [col,row] = new Tiles(o,o.gameObject.tag); // dari blutile jadi purtile
			temporaryTile.Add(o);
			o.SetActive(true);	
		}
	}

	bool CheckGrid()
	{
		int counter = 1;
		bool match = false;
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
						//Debug.Log("("+col + ", "+row+")"+counter);
						if (counter >= 3) {
							//Debug.Log (counter+" " +col+" "+row);
							AddPointsCol (counter,col-1,row);
							renewBoard = true;
							match = true;
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
							match = true;
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
		return match;

	}

	//cek apakah ada bisa diselesaikan
	bool CheckHint()
	{
		Tiles[,] FutureTile = tiles.Clone() as Tiles[,];

	
		int counter = 1;
		//check kolom
		for (int row = 0; row < rows; row++) {
			counter = 1;
			for (int col = 1; col < cols; col++) {
				//rubah bentuk tile
				ChangeFutureTile(FutureTile [col, row]);
				if (FutureTile [col, row] != null && FutureTile [col - 1, row] != null) {
					//jika tiles aktif
					if (FutureTile [col, row].type == FutureTile [col - 1, row].type) {
						counter++;
					} else {
						//jika ada 3 atau lebih apus 

						if (counter >= 2) {
							return true;
						}	
						counter = 1; //reset counter
					}
				}
				//Rubah Balik
				ChangeFutureTile(FutureTile [col, row]);
			}
		}
		// check Baris
		for (int col = 0; col < cols; col++) {
			counter = 1;
			for (int row = 1; row < rows; row++) {
				//rubah bentuk tile
				ChangeFutureTile(FutureTile [col, row]);
				if (FutureTile [col, row] != null && FutureTile [col , row-1] != null) {
					//jika tiles aktif
					if (FutureTile [col, row].type == FutureTile [col , row-1].type) {
						counter++;
					} else {
						//jika ada 3 atau lebih apus 
						//Debug.Log("test" + counter);
						if (counter >= 2) {
							
							return true;

						}
						counter = 1; //reset counter
					}

				}
				//Rubah Balik
				ChangeFutureTile(FutureTile [col, row]);

			}

		}
		return false;
	}


	void AddPointsCol(int counter,int col,int row)
	{
		//Debug.Log ("jajaj");
		for (int i = 0; i < counter; i++) {
			if (tiles [col-i, row] != null) {
				tiles [col-i, row].tile_Obj.SetActive (false);
				for (int j = 0; j < tile_moves.Length; j++) {
					if (tiles [col - i, row].type == tile_moves [j].gameObject.tag) {
						Instantiate (tile_moves [j], tiles [col - i, row].tile_Obj.transform.position, tiles [col - i, row].tile_Obj.transform.rotation);
					}
				}
			}

			tiles [col - i, row] = null;
		}


	}
	void AddPointsRow(int counter,int col,int row)
	{
		for (int i = 0; i < counter; i++) {
			if (tiles [col, row-i] != null) {
				tiles [col, row-i].tile_Obj.SetActive (false);
				for (int j = 0; j < tile_moves.Length; j++) {
					if (tiles [col, row-i].type == tile_moves [j].gameObject.tag) {
						Instantiate (tile_moves [j], tiles [col, row-i].tile_Obj.transform.position, tiles [col, row-i].tile_Obj.transform.rotation);
					}
				}
			}
			tiles [col, row-i] = null;
		}
	}

	void RenewGrid()
	{
		anyMoved = false;
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
							tiles [col, row] = new Tiles (o, o.gameObject.tag);
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

		} else {
			//shuffel ulang kalau gk ada jawaban
			if (!CheckHint()) {
				EmptyGrid ();
				ShuffleList ();
				InitGrid ();
				
			}

		}

	}

	void ChangeFutureTile(Tiles FutureTile)
	{
		GameObject o=null;
		//ubah tile ke lain
		if (FutureTile.tile_Obj.tag == "Blutile") {
			//merubah dari Blue ke purple
			if (!tile_Bank.Exists (x => (x.gameObject.tag == "Purtile") && (x.activeSelf == false))) {
				o = (GameObject)Instantiate (tile [1], new Vector3 (-10, -10, 0), tile [1].transform.rotation);
				o.SetActive(false);
				tile_Bank.Add (o); 
			} else {
				o = tile_Bank.Find (x => (x.gameObject.tag == "Purtile") &&  (x.activeSelf==false));
			}
		} else if (FutureTile.tile_Obj.tag == "Purtile") {
			//merubah dari purple to blue
			if (!tile_Bank.Exists (x => (x.gameObject.tag == "Blutile") && (x.activeSelf == false))) {
				o = (GameObject)Instantiate (tile [0], new Vector3 (-10, -10, 0), tile [0].transform.rotation);
				o.SetActive(false);
				tile_Bank.Add (o); 
			} else {
				o = tile_Bank.Find (x => (x.gameObject.tag == "Blutile") &&  (x.activeSelf==false));
			}
		}else if (FutureTile.tile_Obj.tag == "Siltile") {
			//merubah dari sil to ye
			if (!tile_Bank.Exists (x => (x.gameObject.tag == "Yetile") && (x.activeSelf == false))) {
				o = (GameObject)Instantiate (tile [3], new Vector3 (-10, -10, 0), tile [3].transform.rotation);
				o.SetActive(false);
				tile_Bank.Add (o); 
			} else {
				o = tile_Bank.Find (x => (x.gameObject.tag == "Yetile") &&  (x.activeSelf==false));
			}
		}else if (FutureTile.tile_Obj.tag == "Yetile") {
			//merubah dari ye ke sil
			if (!tile_Bank.Exists (x => (x.gameObject.tag == "Siltile") && (x.activeSelf == false))) {
				o = (GameObject)Instantiate (tile [2], new Vector3 (-10, -10, 0), tile [2].transform.rotation);
				o.SetActive(false);
				tile_Bank.Add (o); 
			} else {
				o = tile_Bank.Find (x => (x.gameObject.tag == "Siltile") &&  (x.activeSelf==false));
			}
		}  
		FutureTile= new Tiles(o,o.gameObject.tag); 

	}
	void FlipBackTile(HashSet<GameObject> hash)
	{
		foreach (GameObject i in hash) {
			int col = (int)i.transform.position.x;
			int row= (int)i.transform.position.y;
			Vector3 tile_Pos = new Vector3 (col, row, 0);
			GameObject o=null;
			//ubah tile ke lain
			//Debug.Log(col+" +" +row);
			if (tiles [col,row].tile_Obj.tag == "Blutile") {
				//merubah dari Blue ke purple
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Purtile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [1], new Vector3 (-10, -10, 0), tile [1].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Purtile") &&  (x.activeSelf==false));
				}
			} else if (tiles [col,row].tile_Obj.tag == "Purtile") {
				//merubah dari purple to blue
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Blutile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [0], new Vector3 (-10, -10, 0), tile [0].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Blutile") &&  (x.activeSelf==false));
				}
			}else if (tiles [col,row].tile_Obj.tag == "Siltile") {
				//merubah dari sil to ye
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Yetile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [3], new Vector3 (-10, -10, 0), tile [3].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Yetile") &&  (x.activeSelf==false));
				}
			}else if (tiles[col,row].tile_Obj.tag == "Yetile") {
				//merubah dari ye ke sil
				if (!tile_Bank.Exists (x => (x.gameObject.tag == "Siltile") && (x.activeSelf == false))) {
					o = (GameObject)Instantiate (tile [2], new Vector3 (-10, -10, 0), tile [2].transform.rotation);
					o.SetActive(false);
					tile_Bank.Add (o); 
				} else {
					o = tile_Bank.Find (x => (x.gameObject.tag == "Siltile") &&  (x.activeSelf==false));
				}
			}  

			o.transform.position = new Vector3 (tile_Pos.x, tile_Pos.y, tile_Pos.z);
			tiles [col, row].tile_Obj.SetActive (false);
			tiles [col, row].tile_Obj.transform.position = new Vector3 (-10, -10, 0);
			tiles [col,row] = new Tiles(o,o.gameObject.tag); // dari blutile jadi purtile
			o.SetActive(true);	
		}
	}
}
