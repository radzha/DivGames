using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Progress {
	public class MainCharacter : Unit {

		// У главного персонажа целью может быть просто точка (x,z).
		public Vector2 PositionTarget {
			get;
			set;
		}

		public bool PositionTargetMode {
			get;
			set;
		}

		protected override void PrepareSelectMarker() {
			base.PrepareSelectMarker();
			var image = selectMarker.transform.GetChild(0).GetComponent<Image>();
			image.color = Color.cyan;
		}

		protected override void DefineTarget() {
			// Главный персонаж не определяет цель автоматически.
		}

		protected override void Move() {
			base.Move();
			if (PositionTargetMode) {
				var myPos = new Vector2(transform.position.x, transform.position.z);
				var distance = Vector2.Distance(myPos, PositionTarget);
				var moveTo = Vector2.Lerp(myPos, PositionTarget, Settings.Speed * Time.deltaTime / distance);
				var y = transform.position.y;
				transform.position = new Vector3(moveTo.x, y, moveTo.y);

				// Повернуться в сторону цели.
				transform.LookAt(new Vector3(PositionTarget.x, y, PositionTarget.y));
				transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
			}
		}

		public void OnEnable() {
			CameraManager.Instance.AutoMove = true;
		}

		public void OnDisable() {
			CameraManager.Instance.AutoMove = false;
		}

		protected override void Attack() {
			base.Attack();
		}

	}
}
