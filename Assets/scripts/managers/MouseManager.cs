﻿using UnityEngine;
using System.Collections;
using Progress;
using System.Linq;

public class MouseManager : MonoBehaviour {
	public GameObject planePrefab;

	private Vector3 clickedPoint = Vector3.zero;
	private Transform plane;
	private	RaycastHit hit;
	private bool planeMode;

	void Update() {
		if (Divan.gameStop) {
			return;
		}
		// Нажатие левой кнопки мыши.
		if (Input.GetMouseButtonDown(0)) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// Если плоскости еще нет нужно проверить ткнули в юнит или нет.
			Physics.Raycast(ray, out hit);
			if (hit.collider == null) {
				return;
			}
			planeMode = !hit.collider.gameObject.CompareTag("Unit");
			// Режим одиночного выделения кликом.
			if (!planeMode) {
				foreach (var unit in SpawnersManager.Instance.Units) {
					unit.SetSelected(unit.gameObject.Equals(hit.collider.gameObject));
				}
			}
		} else if (Input.GetMouseButton(0)) {
			// Удерживание левой кнопки мыши.
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (planeMode) {
				Physics.Raycast(ray, out hit, Mathf.Infinity, Constants.FLOOR_LAYER);
			}
			// Режим множественного выделения плоскостью.
			if (planeMode) {
				hit.point = new Vector3(hit.point.x, 0.1f, hit.point.z);
				if (clickedPoint == Vector3.zero) {
					clickedPoint = hit.point;
					plane = (GameObject.Instantiate(planePrefab, clickedPoint, Quaternion.identity) as GameObject).transform;
					plane.localScale = Vector3.zero;
				} else {
					var direction = hit.point - clickedPoint;
					plane.position = clickedPoint + 0.5f * direction;
					plane.localScale = new Vector3(direction.x / 10f, 1f, direction.z / 10f);
				}
				SelectUnits(clickedPoint, hit.point);
			} 
		} else if (Input.GetMouseButtonUp(0)) {
			// Отпускание левой кнопки мыши.
			Clear();
		} else if (Input.GetMouseButtonDown(1)) {
			// Нажатие правой кнопки мыши.
			var selected = SpawnersManager.Instance.UnitsSelected;
			var player = selected.FirstOrDefault(u => u.unitType == Settings.Unit.UnitType.Player);
			// Если выделен единственный юнит - главный персонаж.
			if (selected != null && selected.Count() == 1 && player != null) {
				var hero = player as MainCharacter;
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Physics.Raycast(ray, out hit);
				if (hit.collider == null) {
					return;
				}
				var clicked = hit.collider.gameObject;
				var damagable = clicked.GetComponent<Damagable>();
				// Клик по юниту или зданию
				if (damagable != null) {
					var isUnit = damagable is Unit;
					if (!(isUnit && !(damagable as Unit).IsEnemy)) {
						hero.target.SetTarget(damagable, isUnit);
						hero.PositionTargetMode = false;
						return;
					}
				} else {
					hero.target.SetTarget(null);
					hero.PositionTargetMode = true;
					hero.PositionTarget = new Vector2(hit.point.x, hit.point.z);
				}
			}
		}
	}

	/// <summary>
	/// Выделить (своих) юнитов, попавших в зону выделения.
	/// </summary>
	/// <param name="start">Начальная точка выделения.</param>
	/// <param name="end">Конечная точка выделения.</param>
	private void SelectUnits(Vector3 start, Vector3 end) {
		foreach (Unit unit in SpawnersManager.Instance.Units) {
			unit.SetSelected(unit.IsEnemy ? false : IsInArea(unit.transform.position, start, end));
		}
	}

	/// <summary>
	/// Попал ли юнит в область выделения.
	/// </summary>
	private bool IsInArea(Vector3 position, Vector3 start, Vector3 end) {
		var leftX = Mathf.Min(start.x, end.x);
		var rightX = Mathf.Max(start.x, end.x);
		var leftZ = Mathf.Min(start.z, end.z);
		var rightZ = Mathf.Max(start.z, end.z);
		return leftX <= position.x && position.x <= rightX && leftZ <= position.z && position.z <= rightZ;
	}

	/// <summary>
	/// Очистить данные о клике и плоскости.
	/// </summary>
	void Clear() {
		clickedPoint = Vector3.zero;
		if (plane != null) {
			Destroy(plane.gameObject);
		}
	}

}