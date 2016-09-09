using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MignonesUI : MonoBehaviour {

	private Text text;

	protected void Awake(){
		text = GetComponent<Text>();
	}

	void Update () {
		text.text = "Миньонов: " + SpawnersManager.Instance.Mignons().Count;
	}

}
