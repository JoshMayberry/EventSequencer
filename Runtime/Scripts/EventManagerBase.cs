using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jmayberry.EventSequencer {
	public class EventManagerBase : MonoBehaviour {
		private EventSequenceBase currentSequence;
		private Coroutine currentCoroutine;

		public override string ToString() {
			return $"<EventManagerBase:{this.GetHashCode()}>";
        }

        public void StartSequence(IContext context, EventSequenceBase sequence) {
			if (this.currentSequence != null) {
				if (!sequence.ShouldOverride(this.currentSequence)) {
					return;
				}

				this.StopCurrentSequence();
			}

			this.currentSequence = sequence;
			this.currentCoroutine = StartCoroutine(this.currentSequence.Start(context));
		}

		public void StopCurrentSequence() {
			if (this.currentCoroutine == null) {
				return;
			}

			StopCoroutine(this.currentCoroutine);
			StartCoroutine(this.currentSequence.OnCancel());
			this.currentCoroutine = null;
		}

		public EventSequenceBase GetCurrentSequence() {
			return this.currentSequence;
		}

		public Coroutine GetCurrentCoroutine() {
			return this.currentCoroutine;
		}
	}
}