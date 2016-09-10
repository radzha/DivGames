using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
	// "Палка", на которой держится камера. Определяет фиксированное расстояние до героя.
	public Vector3 rod;
	// Сглаживание автоматического перемещения камеры.
	public float smoothness = 1f;
	// Сглаживание перемещения камеры за мышью.
	public float mouseSmoothness = 0.1f;
	// Доля от половины экрана. Отсчитывается от края. Если мышь заходит в эту область, начинается перемещение камеры.
	public float edgeCoeff = 0.2f;
	// Задержка между сменой режима управления камерой.
	public float switchCameraDelay = 0.2f;

	// Включен ли режим автоматического слежения камерой за игроком.
	private bool autoMove = true;
	// Центр экрана.
	private Vector3 screenCenter;
	// Таймер задержки переключения камеры.
	private float switchTimer;
	// Игрок, за которым следит камера.
	private GameObject player;

	void Start() {
		screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
	}

	void FixedUpdate() {
		// Смена режима камеры.
		if (switchTimer > 0f) {
			switchTimer -= Time.deltaTime;
		}
		if (Input.GetKey("c") && switchTimer <= 0f) {
			autoMove = !autoMove;
			switchTimer = switchCameraDelay;
		}

		if (autoMove) {
			if (player == null) {
				var hero = SpawnersManager.Instance.MainCharacter();
				if (hero != null) {
					player = hero.gameObject;
				}
			}
			if (player != null) {
				var targetPosition = player.transform.position + rod;
				transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
			}
		} else {
			var mouseShift = Input.mousePosition - screenCenter;
			var normalShift = new Vector2(Mathf.Abs(mouseShift.x) / Screen.width * 2f, Mathf.Abs(mouseShift.y) / Screen.height * 2f);
			if (normalShift.x > 1 - edgeCoeff || normalShift.y > 1 - edgeCoeff) {
				var targetPosition = mouseShift + rod;
				transform.position = Vector3.Lerp(transform.position, targetPosition, mouseSmoothness * Time.deltaTime);
			}
		}
	}
}
