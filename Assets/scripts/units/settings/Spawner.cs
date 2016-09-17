using UnityEngine;
using System.Collections;

namespace Settings {
	/// <summary>
	/// Настройки казарм.
	/// </summary>
	public class Spawner {

		private int gold;
		private float spawnSpeed;

		/// <summary>
		/// Количество золота (Gold) — количество золота, требуемое для апгрейда казармы.
		/// </summary>
		public int Gold {
			get {
				return gold;
			}
			set {
				gold = value;
			}
		}

		/// <summary>
		/// Скорость производства юнитов, шт./сек.
		/// </summary>
		public float SpawnSpeed {
			get {
				return spawnSpeed;
			}
			set {
				spawnSpeed = value;
			}
		}

		/// <summary>
		/// Прочитать настройки из редактора уровней.
		/// </summary>
		/// <param name="unitSettings">Набор настроек.</param>
		/// <param name="level">Уровень.</param>
		private void ReadSettings(LevelEditor.Spawner[] settings, int level) {
			var spawner = settings[level];
			Gold = spawner.gold;
			SpawnSpeed = spawner.spawnSpeed;
		}

		/// <summary>
		/// Первичное заполнение настроек.
		/// </summary>
		public Spawner(int level = 0) {
			ReadSettings(LevelEditor.Instance.spawner, level);
		}

	}
}
