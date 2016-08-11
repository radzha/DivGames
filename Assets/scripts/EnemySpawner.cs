using UnityEngine;

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
	public float bossSpawnProbability = 0.3f;

	/// <summary>
	/// Префабы врагов
	/// </summary>
	public GameObject[] enemyPrefabs;

	private float timer;

	private void Awake() {
		timer = startDelay;
	}

	private void Update() {
		if (timer > 0f) {
			timer -= Time.deltaTime;
			return;
		}
		timer = trainingSpeed;

		var spawnPoint = new Vector3(transform.position.x + Random.Range(-25f, 25f), 0f, transform.position.z);
		print(transform.position);
		var type = Random.Range(0, 1f) < bossSpawnProbability ? 1 : 0;
		Instantiate(enemyPrefabs[type], spawnPoint, Quaternion.identity);
	}

}
