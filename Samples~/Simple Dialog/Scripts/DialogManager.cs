using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.EventSequencer;

public class DialogManager : EventManagerBase {
	public static DialogManager instance { get; private set; }

	private void Awake() {
		if (DialogManager.instance != null && DialogManager.instance != this) {
			Debug.LogError("Found more than one DialogManager in the scene.");
			Destroy(this.gameObject);
			return;
		}

		DialogManager.instance = this;
	}
}
