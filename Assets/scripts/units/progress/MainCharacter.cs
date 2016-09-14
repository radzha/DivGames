using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

namespace Progress {
	public class MainCharacter : Unit {

		public Texture2D iceCursorTexture;

		// У главного персонажа целью может быть просто точка (x,z).
		public Vector2 PositionTarget {
			get;
			set;
		}

		public bool PositionTargetMode {
			get;
			set;
		}

		public enum AttackMode {
			Normal,
			MeteoRain,
			IceArrow
		}

		public AttackMode attackMode {
			get;
			set;
		}

		private float iceArrowTimer;
		private float meteoRainTimer;

		protected override float AttackTimer {
			get {
				switch (attackMode) {
				case AttackMode.Normal:
					return base.AttackTimer;
				case AttackMode.MeteoRain:
					return meteoRainTimer;
				case AttackMode.IceArrow:
					return iceArrowTimer;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			set {
				switch (attackMode) {
				case AttackMode.Normal:
					base.AttackTimer = value;
					break;
				case AttackMode.MeteoRain:
					meteoRainTimer = value;
					break;
				case AttackMode.IceArrow:
					iceArrowTimer = value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		protected override void Update() {
			base.Update();

			iceArrowTimer -= Time.deltaTime;
			meteoRainTimer -= Time.deltaTime;

			if (Input.GetKeyDown(KeyCode.L)) {
				if (iceArrowTimer <= 0f && IsSelected) {
					IceArrowMode(attackMode != AttackMode.IceArrow);
				}
			} else if (Input.GetKeyDown(KeyCode.Escape)) {
				IceArrowMode(false);
			}
		}

		private void IceArrowMode(bool enable) {
			attackMode = enable ? AttackMode.IceArrow : AttackMode.Normal;
			if (!enable) {
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
				target.aim = null;
				return;
			}
			Cursor.SetCursor(iceCursorTexture, Vector2.zero, CursorMode.Auto);
		}

		protected override bool IsInRange(float distance) {
			var normalAttack = attackMode == AttackMode.Normal && base.IsInRange(distance);
			var iceArrowAttack = attackMode == AttackMode.IceArrow && distance <= LevelEditor.Instance.iceArrow[level].radius;
			var meteoRainAttack = attackMode == AttackMode.MeteoRain && distance <= LevelEditor.Instance.meteoRain[level].radius;
			return normalAttack || iceArrowAttack || meteoRainAttack;
		}

		public override float CoolDown {
			get {
				switch (attackMode) {
				case AttackMode.Normal:
					return 1f / attackSpeed;
				case AttackMode.MeteoRain:
					return LevelEditor.Instance.meteoRain[level].cooldown;
				case AttackMode.IceArrow:
					return LevelEditor.Instance.iceArrow[level].cooldown;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public string IceArrowTimerString {
			get {
				return iceArrowTimer <= 0f ? "Готово" : string.Format("{0:###.#}", iceArrowTimer);
			}
		}

		public string MeteoRainTimerString {
			get {
				return meteoRainTimer <= 0f ? "Готово" : string.Format("{0:###.#}", meteoRainTimer);
			}
		}

		/// <summary>
		/// Непосредственный ущерб юниту, дивану или подпитка жизни из фонтана.
		/// </summary>
		protected override void MakeDamage() {
			if (target.aim == null) {
				return;
			}

			switch (attackMode) {
			case AttackMode.Normal:
				base.MakeDamage();
				break;
			case AttackMode.MeteoRain:
				break;
			case AttackMode.IceArrow:
				target.aim.TakeDamage(this, Settings.Attack, LevelEditor.Instance.iceArrow[level].slow, LevelEditor.Instance.iceArrow[level].attackSlow, LevelEditor.Instance.iceArrow[level].duration);
				IceArrowMode(false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
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
			IceArrowMode(false);
		}
	}
}
