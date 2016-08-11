﻿using UnityEngine;

class EnemySpawner : MonoBehaviour {
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

	private void Awake() {
		timer = startDelay;
	}


	private readonly float length = 25f / Mathf.Sqrt(2);

	private void Update() {
		if (timer > 0f) {
			timer -= Time.deltaTime;
			return;
		}
		timer = trainingSpeed;

		var rand = Random.Range(-length, length); // случайный разброс 
		var spawnPoint = new Vector3(transform.position.x + rand, 0f, transform.position.z + rand); // рождение юнита в случайном месте споунера
		var randType = Random.Range(0, 1f);
		var type = randType < bossSpawnProbability ? 0 : randType < bossSpawnProbability + warriorSpawnProbability ? 1 : 2;
		Instantiate(enemyPrefabs[type], spawnPoint, Quaternion.identity);
	}

}