using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Progress;

public class UnitInfoUI : MonoBehaviour {

	public GameObject background;
	private Text text;
	private Unit unit;
	private bool isSelected;

	void Start() {
		text = GetComponent<Text>();

		text.enabled = false;
		background.SetActive(false);

		SpawnersManager.Instance.onUnitSelected -= OnUnitSelected;
		SpawnersManager.Instance.onUnitSelected += OnUnitSelected;
	}

	private void OnUnitSelected(Unit unit, bool isSelected) {
		if (this.unit == unit && this.isSelected == isSelected
			|| this.unit != unit && !isSelected) {
			return;
		}
		this.unit = unit;
		this.isSelected = isSelected;
		text.enabled = isSelected;
		background.SetActive(isSelected);
		SetText();
	}

	private void SetText() {
		var txt = string.Format("Тип: {0}\nУровень: {1}\nЗдоровье: {2}\nСкорость: {3}\nАтака: {4}\nСкорость атаки: {5}\nЗона атаки: {6}\nЗащита: {7}",
			unit.PrettyType(),
			unit.level,
			unit.Health(),
			unit.Settings.Speed,
			unit.Settings.Attack,
			unit.Settings.AttackSpeed,
			unit.Settings.AttackRange,
			unit.Settings.Armor
		);
		if (unit.IsPlayer) {
			txt = string.Format("\nЦель: {0}\nМетеор. дождь: {1}\nЛедяная стрела: {2}", unit.target.aim, ((MainCharacter)unit).MeteoRainTimerString, ((MainCharacter)unit).IceArrowTimerString);
		}
		text.text = txt;
	}

	private void Update() {
		if (isSelected && unit != null) {
			SetText();
		}
	}

	private void OnDestroy() {
		SpawnersManager.Instance.onUnitSelected -= OnUnitSelected;
	}

}
