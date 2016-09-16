﻿using System;
using UnityEngine;
using Progress;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour {
	public GameObject planePrefab;
	public GraphicRaycaster uiRaycaster;
	public GameObject CameraSwitchUI;
	public GameObject IceArrowClickUI;
	public GameObject MeteoRainClickUI;

	private Vector3 clickedPoint = Vector3.zero;
	private Transform plane;
	private RaycastHit hit;
	private bool planeMode;

	void Update() {
		if (Divan.gameStop) {
			return;
		}
		if (Input.GetMouseButtonDown(0) && !UIClickHandle()) {
			LeftButtonAction();
		} else if (Input.GetMouseButton(0)) {
			LeftButtonHoldAction();
		} else if (Input.GetMouseButtonUp(0)) {
			// Отпускание левой кнопки мыши.
			Clear();
		} else if (Input.GetMouseButtonDown(1)) {
			RightButtonAction();
		}
	}

	/// <summary>
	/// Нажатие левой кнопки мыши.
	/// Используется для:
	/// - выделения одного юнита
	/// - атаки в режиме абилок главного персонажа.
	/// </summary>
	private void LeftButtonAction() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit);
		var player = SpawnersManager.Instance.MainCharacter();

		// Режим одиночного выделения юнита кликом.
		if (player == null || player.attackMode == MainCharacter.AttackMode.Normal) {
			if (hit.collider == null) {
				return;
			}
			var isSpawner = hit.collider.gameObject.CompareTag("Spawner");
			if (!isSpawner) {
				planeMode = !hit.collider.gameObject.CompareTag("Unit");
				if (planeMode) {
					return;
				}
				foreach (var unit in SpawnersManager.Instance.Units) {
					unit.SetSelected(unit.gameObject.Equals(hit.collider.gameObject));
				}
				return;
			} else {
				planeMode = false;
				var spawner = SpawnersManager.Instance.spawners.First(s => s.gameObject.Equals(hit.collider.gameObject));
			}
		}

		// Режим абилок.
		switch (player.attackMode) {
		case MainCharacter.AttackMode.IceArrow:
			if (hit.collider != null) {
				// Клик по юниту или зданию
				var clicked = hit.collider.gameObject;
				var damagable = clicked.GetComponent<Damagable>();
				if (damagable != null) {
					var isUnit = damagable is Unit;
					if (!(isUnit && !(damagable as Unit).IsEnemy)) {
						player.target.SetTarget(damagable, isUnit);
						player.PositionTargetMode = false;
					}
				}
			}
			break;
		case MainCharacter.AttackMode.MeteoRain:
			player.Attack();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	/// <summary>
	/// Обработка клика по UI интерфейсу выбора абилки.
	/// </summary>
	/// <returns>true, если был клик по абилке</returns>
	private bool UIClickHandle() {
		// Обрабатывается только левая кнопка мыши.
		if (!Input.GetMouseButtonDown(0)) {
			return false;
		}

		var ped = new PointerEventData(null) { position = Input.mousePosition };
		var results = new List<RaycastResult>();
		uiRaycaster.Raycast(ped, results);
		if (results.Count != 1) {
			return false;
		}

		var player = SpawnersManager.Instance.MainCharacter();
		if (player == null) {
			return false;
		}
		if (results[0].gameObject.Equals(IceArrowClickUI)) {
			player.PerformAbility(MainCharacter.AttackMode.IceArrow);
		} else if (results[0].gameObject.Equals(MeteoRainClickUI)) {
			player.PerformAbility(MainCharacter.AttackMode.MeteoRain);
		} else if (results[0].gameObject.Equals(CameraSwitchUI)) {
			CameraManager.Instance.SwitchCameraMode();
		}
		return true;
	}

	/// <summary>
	/// Удерживание левой кнопки мыши.
	/// Используется для множественного выделения юнитов.
	/// </summary>
	private void LeftButtonHoldAction() {
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
	}

	/// <summary>
	/// Нажатие правой кнопки мыши.
	/// </summary>
	private void RightButtonAction() {
		var selected = SpawnersManager.Instance.UnitsSelected;
		if (selected == null) {
			return;
		}
		var player = SpawnersManager.Instance.MainCharacter();
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit);
		if (hit.collider == null) {
			return;
		}
		var clicked = hit.collider.gameObject;
		var damagable = clicked.GetComponent<Damagable>();
		// Если выделен единственный юнит - главный персонаж.
		if (selected.Count() == 1 && player != null) {
			var hero = player as MainCharacter;
			// Если не включены абилки, то можно атаковать или перемещать героя
			if (hero.attackMode == MainCharacter.AttackMode.Normal) {
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
			} else {
				// Иначе - выключить абилки.
				hero.TurnOffAbilities();
			}
		} else {
			// Выделена группа юнитов.
			if (damagable != null) {
				var isUnit = damagable is Unit;
				if (!(isUnit && !(damagable as Unit).IsEnemy)) {
					foreach (var unit in selected) {
						unit.IsHandMoving = true;
						unit.target.SetTarget(damagable, isUnit);
					}
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
