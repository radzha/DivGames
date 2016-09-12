using Settings;
using UnityEngine;
using System.Linq;

namespace Progress {
	/// <summary>
	/// Юнит - базовый класс для всех субъектов игры.
	/// </summary>
	public class Unit : MonoBehaviour, Damagable {
		/// <summary>
		/// Класс, описывающий цель юнита.
		/// Целью может быть юнит противника или строение:
		/// - диван (для врагов)
		/// - фонтан (для миньонов)
		/// </summary>
		public class Target {
			// Цель юнита.
			public Damagable aim;
			// Является ли юнитом (или зданием)
			public bool isUnit;

			public void SetTarget(Damagable aim, bool isUnit = true) {
				this.aim = aim;
				this.isUnit = isUnit;
			}

			public Target(Damagable aim, bool isUnit) {
				SetTarget(aim, isUnit);
			}
		}

		// Тип юнита.
		public Settings.Unit.UnitType unitType;
		// Префаб маркера выбора.
		public GameObject selectMarkerPrefab;
		// Префаб маркера цели.
		public GameObject aimMarkerPrefab;
		// Цель юнита.
		public Target target;
		// Мертв ли юнит.
		public bool isDead;
		// Уровень юнита.
		public int level;
		// Орудие юнита.
		public Transform gun;
		// Враг ли юнит.
		public bool IsEnemy = true;

		// Маркер выбора.
		protected GameObject selectMarker;
		// Маркер цели.
		protected GameObject aimMarker;
		// Текущий показатель жизни.
		protected int health;
		// Таймер атаки.
		protected float timerAttack;

		// Контроллер аниматора.
		private Animator animator;

		// Набор настроек юнита.
		public Settings.Unit Settings {
			get;
			set;
		}

		// Выделен ли юнит мышью.
		public bool IsSelected {
			get;
			set;
		}

		// Находится ли юнит на ручном управлении.
		public bool IsHandMoving {
			get;
			set;
		}

		public bool IsPlayer {
			get {
				return this is MainCharacter;
			}
		}

		protected virtual void Awake() {
			// Настройки в соответствии с типом юнита.
			Settings = new Settings.Unit(unitType, IsEnemy);
			// Начальный запас жизни.
			health = Settings.Hp;
			// Начальная цель - здание.
			target = new Target(IsPlayer ? null : IsEnemy ? Divan.Instance as Damagable : Fountain.Instance as Damagable, false);
			// Аниматор.
			animator = GetComponent<Animator>();

			PrepareSelectMarker();
			PrepareAimMarker();

			// Подписка на конец игры.
			Divan.Instance.OnGameEnd -= OnGameEnd;
			Divan.Instance.OnGameEnd += OnGameEnd;
		}

		private void OnDestroy() {
			Divan.Instance.OnGameEnd -= OnGameEnd;
		}

		private void OnGameEnd(bool playerWin) {
			target.aim = null;
			if (playerWin && !IsEnemy || !playerWin && IsEnemy) {
				Hurray();
			}
		}

		/// <summary>
		/// Анимация радости выигрышной стороны.
		/// </summary>
		private void Hurray() {
			animator.applyRootMotion = false;
			animator.SetTrigger("hurray");
		}

		/// <summary>
		/// Подготовка маркера выделения юнита.
		/// </summary>
		protected virtual void PrepareSelectMarker() {
			selectMarker = Instantiate(selectMarkerPrefab);
			selectMarker.transform.SetParent(transform);
			selectMarker.transform.localPosition = new Vector3(0f, -0.95f, 0f);
			selectMarker.transform.localScale = new Vector2(0.5f, 0.5f);
			selectMarker.SetActive(false);
		}

		/// <summary>
		/// Подготовка маркера цели.
		/// </summary>
		void PrepareAimMarker() {
			aimMarker = Instantiate(aimMarkerPrefab);
			aimMarker.transform.SetParent(transform);
			var mult = 11f / transform.localScale.y;
			aimMarker.transform.localPosition = new Vector3(0f, mult, 0f);
			aimMarker.SetActive(false);
		}

		protected virtual void FixedUpdate() {
			if (Divan.gameStop) {
				return;
			}

			if (health <= 0) {
				Die();
				return;
			}

			DefineTarget();
			Move();
			CheckMarker();
		}

		/// <summary>
		/// Определяет является ли юнит целью противника и включает/выключает маркер.
		/// </summary>
		private void CheckMarker() {
			aimMarker.SetActive(SpawnersManager.Instance.Units.FirstOrDefault(u => u.IsEnemy != IsEnemy && u.IsSelected && this.Equals(u.target.aim)) != null);
		}

		/// <summary>
		/// Похоронные процедуры.
		/// </summary>
		public void Die() {
			SpawnersManager.Instance.Units.Remove(this);
			SetSelected(false);
			Destroy(gameObject);
		}

		/// <summary>
		/// Принять удар от юнита.
		/// </summary>
		/// <returns>Юниты всегда возвращают ноль.</returns>
		/// <param name="unit">Юнит.</param>
		/// <param name="damage">Урон.</param>
		public int TakeDamage(Progress.Unit unit, float damage) {
			health -= (int)(damage * (1f - Settings.Armor));
			return 0;
		}

		/// <summary>
		/// Назначить/снять выделение юнита.
		/// </summary>
		public void SetSelected(bool selected) {
			if (IsSelected == selected) {
				return;
			}
			selectMarker.SetActive(selected);
			IsSelected = selected;
			SpawnersManager.Instance.onUnitSelected(this, selected);
		}

		/// <summary>
		/// Определить цель юнита.
		/// </summary>
		protected virtual void DefineTarget() {
			if (IsEnemy) {
				if (SpawnersManager.Instance.MignonsCount() == 0) {
					target.SetTarget(Divan.Instance as Damagable, false);
				} else {
					target.SetTarget(FindOpponent(this), true);
				}
			} else if (!IsHandMoving) {
				if (SpawnersManager.Instance.EnemiesCount() == 0) {
					target.SetTarget(Fountain.Instance as Damagable, false);
				} else {
					target.SetTarget(FindOpponent(this), true);
				}
			}
		}

		/// <summary>
		/// Найти ближайшую цель юниту.
		/// </summary>
		private Damagable FindOpponent(Unit unit) {
			var opponents = unit.IsEnemy ? SpawnersManager.Instance.Mignons() : SpawnersManager.Instance.Enemies();
			var closest = opponents.Aggregate((agg, next) => DistanceSqr(next.gameObject, gameObject) < DistanceSqr(agg.gameObject, gameObject) ? next : agg);
			return closest;
		}

		/// <summary>
		/// Квадрат расстояния в двух нужных координатах.
		/// </summary>
		public float DistanceSqr(GameObject g1, GameObject g2) {
			var xDist = g1.transform.position.x - g2.transform.position.x;
			var zDist = g1.transform.position.z - g2.transform.position.z;
			return xDist * xDist + zDist + zDist;
		}

		protected virtual void Fire() {
			animator.SetTrigger("fire");
		}

		/// <summary>
		/// Атака противника.
		/// </summary>
		protected virtual void Attack() {
			if (timerAttack <= 0f) {
				Fire();
				MakeDamage();
				timerAttack = 1f / Settings.AttackSpeed;
			} else {
				timerAttack -= Time.deltaTime;
			}
		}

		/// <summary>
		/// Непосредственный ущерб противнику, дивану или подпитка жизни из фонтана.
		/// </summary>
		private void MakeDamage() {
			if (target.aim != null) {
				health += target.aim.TakeDamage(this, Settings.Attack);
			}
		}

		public bool AimTriggered { 
			get;
			set;
		}

		private bool IsInRange(float distance) {
			return distance <= Settings.AttackRange || AimTriggered;
		}

		/// <summary>
		/// Движение юнита к цели.
		/// </summary>
		protected virtual void Move() {
			if (target.aim == null || target.aim as MonoBehaviour == null) {
				IsHandMoving = false;
				return;
			}
			var targetGo = (target.aim as MonoBehaviour).gameObject;
			var targetPos = new Vector2(targetGo.transform.position.x, targetGo.transform.position.z);
			var myPos = new Vector2(transform.position.x, transform.position.z);
			var distance = Vector2.Distance(myPos, targetPos);
			// Если юнит на расстоянии атаки, то не двигаться, а атаковать.
			if (IsInRange(distance)) {
				Attack();
				return;
			}
			var moveTo = Vector2.Lerp(myPos, targetPos, Settings.Speed * Time.deltaTime / distance);
			var y = transform.position.y;
			transform.position = new Vector3(moveTo.x, y, moveTo.y);

			// Повернуться в сторону цели.
			transform.LookAt(targetGo.transform.position);
			transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
		}

		/// <summary>
		/// Возвращает текущее значение жизни.
		/// </summary>
		public int Health() {
			return health; 
		}

		/// <summary>
		/// Максимальное значение жизни.
		/// </summary>
		public int MaxHealth() {
			return Settings.Hp; 
		}

		/// <summary>
		/// Мертв ли юнит.
		/// </summary>
		public bool IsDead() {
			return health <= 0;
		}

		/// <summary>
		/// Строка, описывающая тип и принадлежность юнита.
		/// </summary>
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