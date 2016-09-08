using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {
	public GameObject planePrefab;

	private Vector3 clickedPoint = Vector3.zero;
	private Transform plane;
	private	RaycastHit hit;

	void Update() {
		if (Input.GetMouseButton(0)) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(ray, out hit);
			if ((hit.point.y) > 0.1f) {
				Clear();
				return;
			}
			hit.point = new Vector3(hit.point.x, 0.1f, hit.point.z);
			if (clickedPoint == Vector3.zero) {
				clickedPoint = hit.point;
				plane = (GameObject.Instantiate(planePrefab, clickedPoint, Quaternion.identity)as GameObject).transform;
				plane.localScale = Vector3.zero;
			} else {
				var direction = hit.point - clickedPoint;
				plane.position = clickedPoint + 0.5f * direction;
				plane.localScale = new Vector3(direction.x / 10f, 1f, direction.z / 10f);
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			Clear();
		}
	}

	void Clear() {
		clickedPoint = Vector3.zero;
		Destroy(plane.gameObject);
	}

}
