using UnityEngine;
using System.Collections;

public class LevelEditor : Singleton<LevelEditor> {
	[System.Serializable]
	public struct UnitSettings {
		public int hp;
		public float armor;
		public int attack;
		public float attackSpeed;
		public float speed;
		public float attackRange;
		public int gold;
		public int xp;
	}

	[System.Serializable]
	public struct MeteoRain {
		public float radius;
		public int damage;
		public float cooldown;
	}

	[System.Serializable]
	public struct IceArrow {
		public float radius;
		public int damage;
		public float slow;
		public float attackSlow;
		public float duration;
	}

	[System.Serializable]
	public struct Fountain {
		public int playerCureSpeed;
		public int mignonCureSpeed;
	}

	[System.Serializable]
	public struct Spawner {
		public int gold;
		public float spawnSpeed;
	}

	[Header("Buildings")]
	public int DivanHealth;
	public Fountain fountain;
	public Spawner[] spawner;

	[Header("Player team")]
	public UnitSettings[] warrior;
	public UnitSettings[] archer;
	public UnitSettings[] player;
	public float[] playerRespawnTime;

	[Header("Enemy team")]
	public UnitSettings[] enemyWarrior;
	public UnitSettings[] enemyArcher;
	public UnitSettings[] boss;

	[Header("Abilities")]
	public MeteoRain[] meteoRain;
	public IceArrow[] iceArrow;
	public float ArcherFirePeriod = 0.2f;

}
