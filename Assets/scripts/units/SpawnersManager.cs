using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Progress;
using System.Linq;

/// <summary>
/// Менеджер производителей юнитов.
/// </summary>
public class SpawnersManager : Singleton<SpawnersManager> {
	// Максимум врагов на поле.
	public int enemySpawnLimit = 10;

	// Максимум миньонов на поле.
	public int mignonSpawnLimit = 10;

	// Стартовая задержка производства юнитов.
	public int startDelay = 10;

	// Длительность одной волны врагов.
	public int enemyWaveDuration = 20;

	// Промежуток между волнами врагов.
	public int enemyWaveDelay = 10;

	// Вес вероятности появления босса.
	public int bossSpawnWeight = 2;

	// Вес вероятности появления вражеского воина.
	public int enemyWarriorSpawnWeight = 5;

	// Вес вероятности появления вражеского стрелка.
	public int enemyArcherSpawnWeight = 6;

	// Вес вероятности появления воина.
	public int warriorSpawnWeight = 5;

	// Вес вероятности появления стрелка.
	public int archerSpawnWeight = 6;

	// Префабы юнитов.
	[System.Serializable]
	public struct unitPrefabs {
		public bool isEnemy;
		public Settings.Unit.UnitType type;
		public int weight;
		public GameObject prefab;
	}

	public unitPrefabs[] UnitPrefabs;


	// Все созданные юниты игры.
	private HashSet<Unit> units;

	public HashSet<Unit> Units {
		get {
			return units;
		}
	}

	private float delayTimer;
	private float waveTimer = -1f;

	protected void Awake() {
		units = new HashSet<Progress.Unit>();
		StartCoroutine(TimeControl());
	}

	/// <summary>
	/// Управление волнами врагов.
	/// </summary>
	private IEnumerator TimeControl() {
		delayTimer = startDelay;
		while (delayTimer > 0f) {
			delayTimer -= Time.deltaTime;
			yield return null;
		}
		while (true) {
			waveTimer = enemyWaveDuration;
			while (waveTimer > 0f) {
				waveTimer -= Time.deltaTime;
				yield return null;
			}
			delayTimer = enemyWaveDelay;
			while (delayTimer > 0f) {
				delayTimer -= Time.deltaTime;
				yield return null;
			}
		}
	}

	private void Update() {
	}

	public HashSet<Unit> Mignons() {
		return new HashSet<Unit>(Units.Where(u => !u.IsEnemy));
	}

	public int MignonsCount() {
		return Units.Where(u => !u.IsEnemy).Count();
	}

	public HashSet<Unit> Enemies() {
		return new HashSet<Unit>(Units.Where(u => u.IsEnemy));
	}

	public int EnemiesCount() {
		return Units.Where(u => u.IsEnemy).Count();
	}

	public void AddUnit(Unit unit) {
		units.Add(unit);
	}

	public bool CanSpawn(bool isEnemy) {	
		print(EnemiesCount() + "/" + MignonsCount());
		print ("can: " + (waveTimer > 0f && isEnemy ? EnemiesCount() < enemySpawnLimit : MignonsCount() < mignonSpawnLimit));
		return waveTimer > 0f && isEnemy ? EnemiesCount() < enemySpawnLimit : MignonsCount() < mignonSpawnLimit;
	}

	public bool CanSpawnType(Settings.Unit.UnitType type) {
		return false;	
	}
}
