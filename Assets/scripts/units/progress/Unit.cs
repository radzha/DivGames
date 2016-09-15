﻿using System.Collections;
using Settings;
using UnityEngine;
using System.Linq;
using UnityEditor;

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
		protected float attackTimer;

		// Контроллер аниматора.
		private Animator animator;

		// Основной цвет юнита.
		private Color unitColor;

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
			SettingsRead();
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

		/// <summary>
		/// Чтение первоначальных настроек юнита.
		/// </summary>
		private void SettingsRead() {
			Settings = new Settings.Unit(unitType, IsEnemy);
			health = Settings.Hp;
			speed = Settings.Speed;
			attackSpeed = Settings.AttackSpeed;
		}

		protected float speed;
		protected float attackSpeed;

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

		protected virtual void Update() {
			attackTimer -= Time.deltaTime;
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
		public int TakeDamage(Unit unit, float damage) {
			health -= (int)(damage * (1f - Settings.Armor));
			return 0;
		}

		/// <summary>
		/// Принять удар от абилки "ледяная стрела"
		/// </summary>
		/// <param name="unit">Атакующий юнит.</param>
		/// <param name="damage">Урон.</param>
		/// <param name="slow">Коэффициент замедления.</param>
		/// <param name="attackSlow">Коэффициент замедления атаки.</param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public int TakeDamage(Unit unit, float damage, float slow, float attackSlow, float duration) {
			speed *= 1 - slow;
			attackSpeed *= 1 - attackSlow;
			FreezeVisually(true);
			StartCoroutine(Defreeze(duration));
			return TakeDamage(unit, damage);
		}

		/// <summary>
		/// Сменить/вернуть юниту цвет после заморозки.
		/// </summary>
		/// <param name="freeze"></param>
		private void FreezeVisually(bool freeze) {
			var material = GetComponent<Renderer>().material;
			var color = unitColor;
			if (freeze) {
				unitColor = material.GetColor("_EmissionColor");
				color = Color.white;
			}
			material.SetColor("_EmissionColor", color);
		}

		/// <summary>
		/// Разморозить юнит после ледяной стрелы
		/// </summary>
		/// <param name="duration">Продолжительность заморозки.</param>
		/// <returns></returns>
		private IEnumerator Defreeze(float duration) {
			while (duration >= 0f) {
				duration -= Time.deltaTime;
				yield return null;
			}
			if (gameObject == null) {
				yield break;
			}
			speed = Settings.Speed;
			attackSpeed = Settings.AttackSpeed;
			FreezeVisually(false);
		}

		/// <summary>
		/// Сменить/вернуть юниту цвет после атаки метеоритным дождем.
		/// </summary>
		/// <param name="enable"></param>
		public void MeteoRainVisually(bool enable) {
			var material = GetComponent<Renderer>().material;
			var color = unitColor;
			if (enable) {
				unitColor = material.GetColor("_EmissionColor");
				color = Color.magenta;
			}
			material.SetColor("_EmissionColor", color);
			if (enable) {
				StartCoroutine(DeRain(1f));
			}
		}

		/// <summary>
		/// Визуально вернуть состояние юнита после атаки метеоритным дождем.
		/// </summary>
		/// <param name="duration">Продолжительность.</param>
		/// <returns></returns>
		private IEnumerator DeRain(float duration) {
			while (duration >= 0f) {
				duration -= Time.deltaTime;
				yield return null;
			}
			if (gameObject == null) {
				yield break;
			}
			MeteoRainVisually(false);
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

		/// <summary>
		/// Квадрат расстояния в двух нужных координатах.
		/// </summary>
		public float DistanceSqr(Vector2 v1, Vector2 v2) {
			var xDist = v1.x - v2.x;
			var zDist = v1.y - v2.y;
			return xDist * xDist + zDist + zDist;
		}

		protected virtual void Fire() {
			animator.SetTrigger("fire");
		}

		/// <summary>
		/// Атака противника.
		/// </summary>
		public virtual void Attack() {
			if (AttackTimer > 0f) {
				return;
			}
			AttackTimer = CoolDown;
			Fire();
			MakeDamage();
		}

		public virtual float CoolDown {
			get {
				return 1f / attackSpeed;
			}
		}

		/// <summary>
		/// Непосредственный ущерб юниту, дивану или подпитка жизни из фонтана.
		/// </summary>
		public virtual void MakeDamage() {
			if (target.aim == null) {
				return;
			}
			health += target.aim.TakeDamage(this, Settings.Attack);
		}

		public bool AimTriggered {
			get;
			set;
		}

		protected virtual float AttackTimer {
			get {
				return attackTimer;
			}
			set {
				attackTimer = value;
			}
		}

		/// <summary>
		/// Попал ли юнит, удалённый на расстояние distance в зону атаки.
		/// </summary>
		protected virtual bool IsInRange(float distance) {
			return distance <= Settings.AttackRange || AimTriggered;
		}

		/// <summary>
		/// Попал ли юнит в зону поражения метеоритным дождем.
		/// </summary>
		public bool IsInMeteoRainRange(float radius) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);
			var mousePoint = new Vector2(hit.point.x, hit.point.z);
			var unitPoint = new Vector2(transform.position.x, transform.position.z);
			var distance = DistanceSqr(mousePoint, unitPoint);
			return distance <= radius;
		}

		/// <summary>
		/// Движение юнита к цели.
		/// </summary>
		protected virtual void Move() {
			if ((MonoBehaviour)target.aim == null) {
				IsHandMoving = false;
				return;
			}
			var targetGo = ((MonoBehaviour)target.aim).gameObject;
			var targetPos = new Vector2(targetGo.transform.position.x, targetGo.transform.position.z);
			var myPos = new Vector2(transform.position.x, transform.position.z);
			var distance = Vector2.Distance(myPos, targetPos);
			// Если юнит на расстоянии атаки, то не двигаться, а атаковать.
			if (IsInRange(distance)) {
				Attack();
				return;
			}
			var moveTo = Vector2.Lerp(myPos, targetPos, speed * Time.deltaTime / distance);
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