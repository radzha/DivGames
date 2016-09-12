using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Progress {
	public class MainCharacter : Unit {
		protected override void PrepareSelectMarker() {
			base.PrepareSelectMarker();
			var image = selectMarker.GetComponentInChildren<Image>();
			image.color = Color.cyan;
		}

	}
}
