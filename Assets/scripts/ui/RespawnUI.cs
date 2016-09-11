using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RespawnUI : MonoBehaviour {

	private Text text;

	protected void Awake(){
		text = GetComponent<Text>();
	}

	void Update () {
		text.text = "Время до воскрешения: " + Progress.Fountain.Instance.gameObject.GetComponent<Spawner>().RespawnTime();
	}

}
