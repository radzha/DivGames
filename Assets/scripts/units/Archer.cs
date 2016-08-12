using Settings;
using UnityEngine;

public class Archer : Enemy {
	private Unit settings;

	public Transform gun;

	public float gunAmplitude = 5f;
	public float gunFreq = 2f;

	private float gunShift = 0f;
	private float gunStep;
	private bool firingMode = true;
	private Vector3 gunAxis;

	protected override void Awake() {
		gunStep = Time.deltaTime * 4 * gunAmplitude * gunFreq;
		var angle = Mathf.PI * (90f - gun.rotation.eulerAngles.x) / 180f;
		print( gun.rotation.eulerAngles.x + "===");
		gunAxis = new Vector3(0f, -Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
		print(gunAxis);
	}

	protected override void Update() {
		base.Update();
		if (firingMode) {
			Fire();
		}
	}

	private void Fire() {
		if (Mathf.Abs(gunShift) <= gunAmplitude) {
			gunShift += gunStep;
		} else {
			gunStep *= -1f;
			gunShift += gunStep;
		}
		gun.position += gunStep * gunAxis;
	}
}