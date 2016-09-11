using Settings;
using UnityEngine;
using System.Linq;

namespace Progress {
	public class Unit : MonoBehaviour, Damagable {
		public class Target {
			public Damagable aim;
			public bool isUnit;

			public void SetTarget(Damagable aim, bool isUnit = true) {
				this.aim = aim;
				this.isUnit = isUnit;
			}

			public Target(Damagable aim, bool isUnit) {
				SetTarget(aim, isUnit);
			}
		}

		public Transform gun;
		public float gunAmplitude = 1f;
		public float gunFreq = 0.5f;
		public Settings.Unit.UnitType unitType;
		public GameObject selectMarkerPrefab;
		public Target target;
		public bool isDead;
		public int level;

		protected GameObject selectMarker;
		protected int health;
		protected float gunShift = 0f;
		protected float gunStep;
		protected bool firingMode = false;
		protected Settings.Unit settings;

		private Vector3 gunAxis;
		public bool IsEnemy = true;
		private bool isSelected;
		private RaycastHit hit;

		public Settings.Unit Settings {
			get {
				return settings;
			}
			set {
				settings = value;
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
			Settings = new Settings.Unit(unitType, IsEnemy);
			// Начальный запас жизни
			health = settings.Hp;

			target = new Target(IsEnemy ? Divan.Instance as Damagable : Fountain.Instance as Damagable, false);
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

		protected virtual void FixedUpdate() {
			if (health <= 0) {
				Die();
				return;
			}
			DefineTarget();
			Move();
		}

		private void Die() {
			SpawnersManager.Instance.Units.Remove(this);
			SetSelected(false);
			Destroy(gameObject);
		}

		public int TakeDamage(Progress.Unit unit, float damage) {
			health -= (int)(damage * (1f - settings.Armor));
			return 0;
		}

		/// <summary>
		/// Назначить/снять выделение юнита.
		/// </summary>
		public void SetSelected(bool selected) {
			if (isSelected == selected) {
				return;
			}
			selectMarker.SetActive(selected);
			IsSelected = selected;
			SpawnersManager.Instance.onUnitSelected(this, selected);
		}

		private void DefineTarget() {
			if (IsEnemy) {
				if (SpawnersManager.Instance.MignonsCount() == 0) {
					target.SetTarget(Divan.Instance as Damagable, false);
				} else {
					target.SetTarget(FindOpponent(this), true);
				}
			} else {
				if (SpawnersManager.Instance.EnemiesCount() == 0) {
					target.SetTarget(Fountain.Instance as Damagable, false);
				} else {
					target.SetTarget(FindOpponent(this), true);
				}
			}
		}

		private Damagable FindOpponent(Unit unit) {
			var opponents = unit.IsEnemy ? SpawnersManager.Instance.Mignons() : SpawnersManager.Instance.Enemies();
			var closest = opponents.Aggregate((agg, next) => DistanceSqr(next.gameObject, gameObject) < DistanceSqr(agg.gameObject, gameObject) ? next : agg);
			return closest;
		}

		public float DistanceSqr(GameObject g1, GameObject g2) {
			var xDist = g1.transform.position.x - g2.transform.position.x;
			var zDist = g1.transform.position.z - g2.transform.position.z;
			return xDist * xDist + zDist + zDist;
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

		protected float timerAttack;

		protected void Attack() {
			if (timerAttack <= 0f) {
				Fire();
				MakeDamage();
				timerAttack = 1f / settings.AttackSpeed;
			} else {
				firingMode = false;
				timerAttack -= Time.deltaTime;
			}
		}

		private void MakeDamage() {
			if (target.aim != null) {
				health += target.aim.TakeDamage(this, settings.Attack);
			}
		}

		protected void Move() {
			var speed = settings.Speed;
			if (target.aim == null) {
				return;
			}
			var targetGo = (target.aim as MonoBehaviour).gameObject;
			var targetPos = new Vector2(targetGo.transform.position.x, targetGo.transform.position.z);
			var myPos = new Vector2(transform.position.x, transform.position.z);
			var distance = Vector2.Distance(myPos, targetPos);
			if (distance <= settings.AttackRange) {
				Attack();
				return;
			}
			var moveTo = Vector2.Lerp(myPos, targetPos, speed * Time.deltaTime / distance);
			var y = transform.position.y;
			transform.position = new Vector3(moveTo.x, y, moveTo.y);
		}

		public int Health() {
			return health; 
		}

		public int MaxHealth() {
			return Settings.Hp; 
		}

		public string PrettyType() {
			var type = "";
			switch (unitType) {
				case global::Settings.Unit.UnitType.Archer:
					type = "стрелок";
					break;
				case global::Settings.Unit.UnitType.Warrior:
					type = "воин";
					break;
				case global::Settings.Unit.UnitType.Boss:
					type = "босс";
					break;
				case global::Settings.Unit.UnitType.Player:
					type = "герой";
					break;
			}
			var own = IsEnemy ? "Вражеский" : "Наш";
			return own + " " + type;
		}
	}
}