using Settings;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform gun;
	public float gunAmplitude = 1f;
	public float gunFreq = 0.5f;
	public	Unit.UnitType unitType;
	public GameObject selectMarkerPrefab;

	protected GameObject selectMarker;
	protected int health;
	protected float gunShift = 0f;
	protected float gunStep;
	protected bool firingMode = false;
	protected Unit settings;

	private Vector3 gunAxis;
	private bool isEnemy = true;
	private bool isSelected;

	public Unit Settings {
		get {
			return settings;
		}
		set {
			settings = value;
		}
	}

	public bool IsEnemy {
		get {
			return isEnemy;
		}
		set {
			isEnemy = value;
		}
	}

	public bool IsSelected {
		get {
			return isSelected;
		}
		set {
			isSelected = value;
		}
	}

	protected virtual void Awake() {
		// Настройки в соответствии с типом юнита.
		Settings = new Settings.Unit(unitType);
		// Начальный запас жизни
		health = settings.Hp;

		PrepareSelectMarker();
		PrepareGun();
	}

	/// <summary>
	/// Подготовка маркера выделения юнита.
	/// </summary>
	void PrepareSelectMarker() {
		selectMarker = Instantiate(selectMarkerPrefab);
		selectMarker.transform.SetParent(transform);
		selectMarker.transform.localPosition = new Vector3(0f, -0.95f, 0f);
		selectMarker.transform.localScale = new Vector2(0.5f, 0.5f);
		selectMarker.SetActive(false);
	}

	/// <summary>
	/// Подготовка оружия.
	/// </summary>
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


	/// <summary>
	/// Назначить/снять выделение юнита.
	/// </summary>
	public void SetSelected(bool selected){
		selectMarker.SetActive(selected);
		IsSelected = selected;
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