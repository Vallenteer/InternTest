using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SummonObj : MonoBehaviour {

	[SerializeField] GameObject Obj;
	void summon()
	{
		Instantiate (Obj, Obj.transform.position, Obj.transform.rotation);

	}
	void backtomenu()
	{
		SceneManager.LoadScene (0);
	}
}
