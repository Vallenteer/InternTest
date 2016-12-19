using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tiles
{
	public GameObject tile_Obj;
	public string type;
	public Tiles(GameObject tile, string t)
	{
		tile = tile_Obj;
		t = type;
	}

}

public class CreateGame : MonoBehaviour {

	GameObject tile1=null;
	GameObject tile2=null;

	[SerializeField] int rows;
	[SerializeField] int cols;

	[SerializeField] GameObject[] tile;
	List<GameObject> tile_Bank = new List<GameObject>();

	HashSet<GameObject> tileSelect = new HashSet<GameObject> ();


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
			Destroy (i); // cuma untuk test
		}
	}

}
