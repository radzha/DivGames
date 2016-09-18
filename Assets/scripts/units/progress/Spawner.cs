using UnityEngine;
using System.Collections.Generic;
using Progress;
using System.Linq;

/// <summary>
/// Производитель юнитов.
/// </summary>
public class Spawner : MonoBehaviour, Selectable {
	// Скорость тренировки - юнитов в секунду.
	public float trainingSpeed;

	// Типы юнитов, которые можно здесь производить.
	public Settings.Unit.UnitType[] spawnUnitTypes = null;
	public bool isEnemy = false;

	public int Level {
		get {
			return level;
		}
		set {
			if (value <= level || value > LevelEditor.Instance.spawner.Length - 1) {
				return;
			}
			level = value;
			OnLevelUp();
		}
	}

	public Settings.Spawner settings;
	private Dictionary<Settings.Unit.UnitType,float> typeChances;
	private readonly float length = 25f / Mathf.Sqrt(2);
	private float trainingTimer;
	private bool selected;
	private int level;

	// Основной цвет казармы.
	private Color defaultColor;

	private void Awake() {
		settings = new Settings.Spawner(Level);
		trainingSpeed = spawnUnitTypes[0] == Settings.Unit.UnitType.Player ? LevelEditor.Instance.playerRespawnTime[Level] : settings.SpawnSpeed;
		trainingTimer = spawnUnitTypes[0] == Settings.Unit.UnitType.Player ? 0f : trainingSpeed > 0f ? 1 / trainingSpeed : Mathf.Infinity;
		typeChances = new Dictionary<Settings.Unit.UnitType, float>();
		var weightSum = SpawnersManager.Instance.UnitPrefabs.Where(u => u.isEnemy == isEnemy && spawnUnitTypes.Contains(u.type)).Sum(u => u.weight);
		foreach (var type in spawnUnitTypes) {
			var chance = SpawnersManager.Instance.UnitPrefabs.First(u => u.isEnemy == isEnemy && u.type == type).weight / (float)weightSum;
			typeChances.Add(type, chance);
		}
		var material = GetComponent<Renderer>().material;
		defaultColor = material.GetColor("_EmissionColor");
	}

	public int RespawnTime() {
		if (spawnUnitTypes[0] == Settings.Unit.UnitType.Player) {
			return SpawnersManager.Instance.MainCharacter() != null ? 0 : trainingTimer > 0f ? (int)trainingTimer : 0;
		}
		return 0;
	}

	private bool IsMainCharacterSpawner() {
		return spawnUnitTypes[0] == Settings.Unit.UnitType.Player;
	}

	private void Update() {
		if (Divan.gameStop) {
			return;
		}

		if (IsMainCharacterSpawner() && SpawnersManager.Instance.MainCharacter() != null) {
			return;
		}

		if (trainingTimer > 0f) {
			trainingTimer -= Time.deltaTime;
			return;
		}
		trainingTimer = trainingSpeed > 0f ? 1 / trainingSpeed : Mathf.Infinity;

		if (!IsMainCharacterSpawner() && !SpawnersManager.Instance.CanSpawn(isEnemy)) {
			return;
		}

		MakeUnit();
	}

	void MakeUnit() {
		var type = RandomType();
		var rand = Random.Range(-length, length);
		// случайный разброс 
		var spawnPoint = new Vector3(transform.position.x + rand, 0f, transform.position.z + rand);
		// рождение юнита в случайном месте споунера
		var prefab = SpawnersManager.Instance.UnitPrefabs.First(u => u.isEnemy == isEnemy && u.type == type).prefab;
		var unit = (GameObject)Instantiate(prefab, spawnPoint, Quaternion.identity);
		var unitComp = unit.GetComponent<Unit>();
		unitComp.Level = Level;
		unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.localScale.y, unit.transform.position.z);
		SpawnersManager.Instance.AddUnit(unitComp);
	}

	private Settings.Unit.UnitType RandomType() {
		var randForType = Random.Range(0, 1f);
		var sum = 0f;
		foreach (var typeChance in typeChances) {
			sum += typeChance.Value;
			if (randForType <= sum) {
				return typeChance.Key;
			}
		}
		return spawnUnitTypes.Last();
	}

	public void SetSelected(bool selected) {
		this.selected = selected;
		SpawnersManager.Instance.onUnitSelected(this);
		SelectVisually(selected);
	}

	/// <summary>
	/// Выделить/cнять выделение визуально.
	/// </summary>
	private void SelectVisually(bool select) {
		var material = GetComponent<Renderer>().material;
		var color = defaultColor;
		if (select) {
			defaultColor = material.GetColor("_EmissionColor");
			color = Color.cyan;
		}
		material.SetColor("_EmissionColor", color);
	}

	public bool IsSelected() {
		return selected;
	}

	public void Upgrade() {
		if (settings.Gold <= Player.GoldAmount) {
			Level++;
			Player.GoldAmount -= settings.Gold;
			settings = new Settings.Spawner(Level);
		}
	}

	/// <summary>
	/// Строка, описывающая тип и принадлежность казармы.
	/// </summary>
	public string PrettyType() {
		var type = "";
		switch (spawnUnitTypes[0]) {
			case Settings.Unit.UnitType.Archer:
				type = "стрелков";
				break;
			case Settings.Unit.UnitType.Warrior:
				type = "воинов";
				break;
			case Settings.Unit.UnitType.Boss:
				type = "босса";
				break;
			case Settings.Unit.UnitType.Player:
				type = "героя";
				break;
			default:
				throw new System.ArgumentOutOfRangeException();
		}
		return  "Казарма " + type;
	}

	private void OnLevelUp() {
		var units = SpawnersManager.Instance.Units.Where(u => u.IsEnemy == isEnemy && spawnUnitTypes.Contains(u.unitType));
		foreach(var unit in units){
			unit.Level++;
		}
	}
}
