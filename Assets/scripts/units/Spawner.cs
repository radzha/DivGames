using UnityEngine;
using System.Collections.Generic;
using Progress;

class Spawner : MonoBehaviour {
	/// <summary>
	/// Скорость тренировки - юнитов в секунду
	/// </summary>
	public float trainingSpeed = 1f;

	/// <summary>
	/// Стартовая задержка производства юнитов
	/// </summary>
	public int startDelay = 30;

	/// <summary>
	/// Вероятность появления босса
	/// </summary>
	public float bossSpawnProbability = 0.1f;

	/// <summary>
	/// Вероятность появления воина
	/// </summary>
	public float warriorSpawnProbability = 0.45f;

	/// <summary>
	/// Вероятность появления стрелка
	/// </summary>
	public float archerSpawnProbability = 0.45f;

	/// <summary>
	/// Префабы врагов
	/// </summary>
	public GameObject[] enemyPrefabs;

	private float timer;
	private readonly float length = 25f / Mathf.Sqrt(2);

	private void Awake() {
		timer = startDelay;
	}

	private void Update() {
		if (timer > 0f) {
			timer -= Time.deltaTime;
			return;
		}
		timer = trainingSpeed;

//		if (enemies.Count >= spawnLimit) {
//			return;
//		}

		var rand = Random.Range(-length, length); // случайный разброс 
		var spawnPoint = new Vector3(transform.position.x + rand, 0f, transform.position.z + rand); // рождение юнита в случайном месте споунера
		var randType = Random.Range(0, 1f);
		var type = randType < bossSpawnProbability ? 0 : randType < bossSpawnProbability + warriorSpawnProbability ? 1 : 2;
		GameObject unit = (GameObject)Instantiate(enemyPrefabs[type], spawnPoint, Quaternion.identity);
		unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.localScale.y, unit.transform.position.z);
		SpawnersManager.Instance.AddUnit(unit.GetComponent<Unit>());
	}

}
