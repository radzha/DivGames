using UnityEngine;
using System.Collections.Generic;
using Progress;
using System.Linq;

/// <summary>
/// Производитель юнитов.
/// </summary>
class Spawner : MonoBehaviour {
	// Скорость тренировки - юнитов в секунду.
	public float trainingSpeed;

	// Типы юнитов, которые можно здесь производить.
	public Settings.Unit.UnitType[] spawnUnitTypes = null;

	public bool isEnemy = false;

	private Dictionary<Settings.Unit.UnitType,float> typeChances;
	private readonly float length = 25f / Mathf.Sqrt(2);
	private float trainingTimer;

	private void Awake() {
		var level = 0;
		trainingSpeed = spawnUnitTypes[0] == Settings.Unit.UnitType.Player ? LevelEditor.Instance.playerRespawnTime[level] : LevelEditor.Instance.spawner[level].spawnSpeed;
		trainingTimer = spawnUnitTypes[0] == Settings.Unit.UnitType.Player ? 0f : trainingSpeed > 0f ? 1 / trainingSpeed : Mathf.Infinity;
		typeChances = new Dictionary<Settings.Unit.UnitType, float>();
		var weightSum = SpawnersManager.Instance.UnitPrefabs.Where(u => u.isEnemy == isEnemy && spawnUnitTypes.Contains(u.type)).Sum(u => u.weight);
		foreach (var type in spawnUnitTypes) {
			var chance = SpawnersManager.Instance.UnitPrefabs.First(u => u.isEnemy == isEnemy && u.type == type).weight / (float)weightSum;
			typeChances.Add(type, chance);
		}
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

		var type = RandomType();
		var rand = Random.Range(-length, length); // случайный разброс 
		var spawnPoint = new Vector3(transform.position.x + rand, 0f, transform.position.z + rand); // рождение юнита в случайном месте споунера
		var prefab = SpawnersManager.Instance.UnitPrefabs.First(u => u.isEnemy == isEnemy && u.type == type).prefab;
		var unit = (GameObject)Instantiate(prefab, spawnPoint, Quaternion.identity);
		unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.localScale.y, unit.transform.position.z);
		SpawnersManager.Instance.AddUnit(unit.GetComponent<Unit>());
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

}
