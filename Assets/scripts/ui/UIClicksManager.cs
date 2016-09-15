using UnityEngine;
using System.Collections.Generic;
using Progress;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Обработчик кликов по UI элементам, вызывающим абилки.
/// </summary>

public class UIClicksManager : MonoBehaviour {

	public GameObject IceArrowClickAim;
	public GameObject MeteoRainClickAim;

	void Update() {
		// Обрабатывается только левая кнопка мыши.
		if (!Input.GetMouseButtonDown(0)) {
			return;
		}

		var gr = GetComponent<GraphicRaycaster>();
		var ped = new PointerEventData(null) { position = Input.mousePosition };
		var results = new List<RaycastResult>();
		gr.Raycast(ped, results);
		if (results.Count != 1) {
			return;
		}

		var player = SpawnersManager.Instance.MainCharacter();
		if (player == null) {
			return;
		}
		if (results[0].gameObject.Equals(IceArrowClickAim)) {
			player.PerformAbility(MainCharacter.AttackMode.IceArrow);
		} else if (results[0].gameObject.Equals(MeteoRainClickAim)) {
			player.PerformAbility(MainCharacter.AttackMode.MeteoRain);
		}
	}
}
