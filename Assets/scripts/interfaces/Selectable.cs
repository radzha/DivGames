using UnityEngine;
using System.Collections;

/// <summary>
/// Объекты игры, доступные для выделения левой кнопкой мыши.
/// </summary>
public interface Selectable {
	void SetSelected(bool selected);
	bool IsSelected();
}
