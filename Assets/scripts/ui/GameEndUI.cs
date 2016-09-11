using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Progress;

public class GameEndUI : MonoBehaviour {

	private Image toner;
	private Text text;

	void Start() {
		toner = GetComponent<Image>();
		text = GetComponentInChildren<Text>();

		text.enabled = false;
		toner.enabled = false;

		Divan.Instance.OnGameEnd -= OnGameEnd;
		Divan.Instance.OnGameEnd += OnGameEnd;
	}

	private void OnGameEnd(bool win) {
		toner.enabled = true;
		text.enabled = true;
		text.text = win ? "Вы выиграли!" : "Вы проиграли!";
	}

	private void OnDestroy() {
		Divan.Instance.OnGameEnd -= OnGameEnd;
	}
	
}
