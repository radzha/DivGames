﻿using UnityEngine;
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
		if (this.unit == unit && this.isSelected == isSelected) {
			return;
		}
		this.unit = unit;
		this.isSelected = isSelected;
		text.enabled = isSelected;
		background.SetActive(isSelected);
		SetText();
	}

	private void SetText() {
		text.text = string.Format("Тип: {0}\nУровень: {1}\nЗдоровье: {2}\nСкорость: {3}\nАтака: {4}\nСкорость атаки: {5}\nЗона атаки: {6}\nЗащита: {7}",
			unit.PrettyType(),
			unit.level,
			unit.Health(),
			unit.Settings.Speed,
			unit.Settings.Attack,
			unit.Settings.AttackSpeed,
			unit.Settings.AttackRange,
			unit.Settings.Armor
		);
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