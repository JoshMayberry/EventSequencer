using System;
using System.Collections;
using UnityEngine;

using jmayberry.EventSequencer;

[Serializable]
public class DialogEvent : EventBase {
    public DialogEvent(EventPriority priority) : base(priority) { }

    public override IEnumerator OnExecute(IContext context) {
        if (context is DialogContext dialogContext) {
            Debug.Log($"{dialogContext.Speaker}:");
            foreach (string text in dialogContext.DialogueText.Split(" ")) {
                Debug.Log(text);
                yield return new WaitForSeconds(1);
            }
            Debug.Log(".");

            // Wait for the player to advance the dialogue (e.g., button press)
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
    }

    public override IEnumerator OnCancel() {
        Debug.Log("Nevermind");
        yield return null;
    }
}
