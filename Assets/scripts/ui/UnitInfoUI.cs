using UnityEngine;
using UnityEngine.UI;
using Progress;

/// <summary>
/// Отображение информаци о юните.
/// </summary>
public class UnitInfoUI : MonoBehaviour {

	public GameObject background;
	protected Text text;
	protected Unit unit;
	protected bool isSelected;

	void Start() {
		text = GetComponent<Text>();

		text.enabled = false;
		background.SetActive(false);

		SpawnersManager.Instance.onUnitSelected -= OnUnitSelected;
		SpawnersManager.Instance.onUnitSelected += OnUnitSelected;
	}

	protected virtual void OnUnitSelected(Unit unit, bool isSelected) {
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

	protected virtual void SetText() {
		var txt = string.Format("Тип: {0}\nУровень: {1}\nЗдоровье: {2}\nСкорость: {3}\nАтака: {4}\nСкорость атаки: {5}\nЗона атаки: {6}\nЗащита: {7}",
			unit.PrettyType(),
			unit.Level + 1,
			unit.Health(),
			unit.Settings.Speed,
			unit.Settings.Attack,
			unit.Settings.AttackSpeed,
			unit.Settings.AttackRange,
			unit.Settings.Armor
		);
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
