using Settings;
using UnityEngine;

public class Enemy : MonoBehaviour {
	protected Unit settings;

	public Unit Settings {
		get {
			return settings;
		}
		set {
			settings = value;
		}
	}

	public Transform gun;

	public float gunAmplitude = 1f;
	public float gunFreq = 0.5f;
	public	Unit.UnitType unitType;

	protected int health;
	protected float gunShift = 0f;
	protected float gunStep;
	protected bool firingMode = true;
	private Vector3 gunAxis;

	protected virtual void Awake() {
		Settings = new Settings.Unit(unitType);
		health = settings.Hp;
		PrepareGun();
	}

	protected virtual void PrepareGun() {
		gunStep = Time.deltaTime * 4 * gunAmplitude * gunFreq;
		var angle = Mathf.PI * (90f - gun.rotation.eulerAngles.x) / 180f;
		gunAxis = new Vector3(0f, -Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
	}

	protected virtual void Update() {
		Move();
		if (firingMode) {
			Fire();
		}
	}

	protected virtual void Fire() {
		if (Mathf.Abs(gunShift) <= gunAmplitude) {
			gunShift += gunStep;
		} else {
			gunStep *= -1f;
			gunShift += gunStep;
		}
		gun.position += gunStep * gunAxis;
	}

	protected void Move() {
		var target = Divan.Instance.transform;
		var speed = 2f;
		var distance = Vector3.Distance(transform.position, target.position);
		var y = transform.position.y;
		var moveTo = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime / distance);
		transform.position = new Vector3(moveTo.x, y, moveTo.z);
	}

	public int GetHealth() {
		return health;
	}

}