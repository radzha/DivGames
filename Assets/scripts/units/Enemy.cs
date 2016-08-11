using Settings;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private Unit settings;

	void Awake() {
		//settings = new Unit(Unit.UnitType.Archer);
	}

	void Update() {
		var target = Divan.Instance.transform;
		var speed = 2f;
		var distance = Vector3.Distance(transform.position, target.position);

		transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime / distance);
	}
}