using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Progress;
using System.Linq;

/// <summary>
/// Менеджер производителей юнитов.
/// </summary>
public class SpawnersManager : Singleton<SpawnersManager> {
	// Максимум врагов на поле
	public int enemySpawnLimit = 10;
	// Максимум миньонов на поле
	public int mignonSpawnLimit = 10;

	// Все созданные юниты игры.
	private HashSet<Unit> units;

	public HashSet<Unit> Units {
		get {
			return units;
		}
	}

	protected void Awake() {
		units = new HashSet<Progress.Unit>();
	}

//	public HashSet<Unit> GetMignons(){
//		return Units.Where(u=>u.Settings.GetType() == )
//	}

	public void AddUnit(Unit unit) {
		units.Add(unit);
	}
}
