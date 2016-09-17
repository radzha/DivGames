using UnityEngine;
using UnityEngine.UI;

namespace Progress {
/// <summary>
/// Отображение информаци о юните.
/// </summary>
	public class UnitInfoUI : MonoBehaviour {

		public GameObject background;
		protected Text text;
		protected Selectable thing;
		protected bool isSelected;

		void Start() {
			text = GetComponent<Text>();

			text.enabled = false;
			background.SetActive(false);

			SpawnersManager.Instance.onUnitSelected -= OnUnitSelected;
			SpawnersManager.Instance.onUnitSelected += OnUnitSelected;
		}

		protected virtual void OnUnitSelected(Selectable thing) {
			if (this.thing == thing && this.isSelected == thing.IsSelected()
				|| this.thing != thing && !thing.IsSelected()) {
				return;
			}
			this.thing = thing;
			this.isSelected = thing.IsSelected();
			text.enabled = isSelected;
			background.SetActive(isSelected);
			SetText();
		}

		protected virtual void SetText() {
			string txt = "";
			if (thing is Unit) {
				var unit = thing as Unit;
				txt = string.Format("Тип: {0}\nУровень: {1}\nЗдоровье: {2}\nСкорость: {3}\nАтака: {4}\nСкорость атаки: {5}\nЗона атаки: {6}\nЗащита: {7}",
					unit.PrettyType(),
					unit.Level + 1,
					unit.Health(),
					unit.Settings.Speed,
					unit.Settings.Attack,
					unit.Settings.AttackSpeed,
					unit.Settings.AttackRange,
					unit.Settings.Armor
				);
			} else if (thing is Spawner) {
				var spawner = thing as Spawner;
				txt = string.Format("Тип: {0}\nУровень: {1}\nСкорость тренировки: {2}\nСтоимость улучшения: {3}",
					spawner.PrettyType(),
					spawner.Level + 1,
					spawner.trainingSpeed,
					spawner.settings.Gold
				);
			}
			text.text = txt;
		}

		private void Update() {
			if (isSelected && thing != null) {
				SetText();
			}
		}

		private void OnDestroy() {
			SpawnersManager.Instance.onUnitSelected -= OnUnitSelected;
		}
	}
}
