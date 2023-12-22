using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.EventSequencer;

public class Interactable : MonoBehaviour {

	[SerializeField] public List<DialogEvent> dialogEvents;
    [SerializeField] public DialogContext context = new DialogContext();

    public EventSequence currentSequence;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			this.currentSequence = DialogManager.instance.StartSequence(this.context, this.dialogEvents);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
			if (DialogManager.instance.GetCurrentSequence() == this.currentSequence) {
				DialogManager.instance.StopCurrentSequence();
            }
			
			this.currentSequence = null;
        }
    }
}
