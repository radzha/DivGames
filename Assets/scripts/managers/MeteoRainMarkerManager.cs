using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Класс, контроллирующий маркер абилки метеоритного дождя.
/// </summary>
public class MeteoRainMarkerManager : Singleton<MeteoRainMarkerManager> {

	public GameObject meteoRainMarkerPrefab;

	[NonSerialized]
	public Image meteoRainMarker;

	private GameObject marker;

	void Awake() {
		marker = Instantiate(meteoRainMarkerPrefab);
		meteoRainMarker = marker.GetComponent<Image>();
		marker.SetActive(false);
	}

	private RaycastHit hit;

	void Update() {
		if (marker.activeSelf) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(ray, out hit);
			if (hit.collider == null) {
				return;
			}

			marker.transform.position = new Vector3(hit.point.x, 0.06f, hit.point.z);
		}
	}
}
