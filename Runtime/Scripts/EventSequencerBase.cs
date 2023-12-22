using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jmayberry.EventSequencer {
	public class EventSequencerBase : MonoBehaviour {
		private EventSequence currentSequence;
		private Coroutine currentCoroutine;

		public override string ToString() {
			return $"<EventSequencerBase:{this.GetHashCode()}>";
        }

        public EventSequence StartSequence(IContext context, IEnumerable<EventBase> eventList) {
            EventSequence sequence = new EventSequence(eventList);
            this.StartSequence(context, sequence);
            return sequence;
        }

        public void StartSequence(IContext context, EventSequence sequence) {
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

		public EventSequence GetCurrentSequence() {
			return this.currentSequence;
		}

		public Coroutine GetCurrentCoroutine() {
			return this.currentCoroutine;
		}
	}
}