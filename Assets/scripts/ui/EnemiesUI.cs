using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemiesUI : MonoBehaviour {

	private Text text;

	protected void Awake(){
		text = GetComponent<Text>();
	}

	void Update () {
		text.text = "Врагов: " + SpawnersManager.Instance.Enemies().Count;
	}

}
