using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jmayberry.EventSequencer {
	public class EventManagerBase : MonoBehaviour {
		private SequenceBase currentSequence;
		private Coroutine currentCoroutine;

		public override string ToString() {
			return $"<EventManagerBase:{this.GetHashCode()}>";
        }

        public void StartSequence(IContext context, SequenceBase sequence, bool hardStop = false) {
			if (this.currentCoroutine != null) {
				if (!sequence.ShouldOverride(this.currentSequence)) {
					return;
				}

				this.StopSequence(this.currentSequence, this.currentCoroutine, hardStop);
			}

			this.currentSequence = sequence;
			this.currentCoroutine = StartCoroutine(this.currentSequence.Start(context));
        }

        public void StopSequence(bool hardStop = false) {
            StopSequence(this.currentSequence, this.currentCoroutine, hardStop);
        }

        public void StopSequence(SequenceBase sequence, Coroutine coroutine, bool hardStop = false) {
            StartCoroutine(this.Cancel(sequence, coroutine, hardStop));
            this.currentCoroutine = null;
        }

        private IEnumerator Cancel(SequenceBase sequence, Coroutine coroutine, bool hardStop = false) {
            if (coroutine == null) {
                yield break;
            }

            if (!hardStop && (sequence != null)) {
                yield return sequence.OnCancel();
            }

            StopCoroutine(coroutine);
        }

        public SequenceBase GetCurrentSequence() {
			return this.currentSequence;
		}

		public Coroutine GetCurrentCoroutine() {
			return this.currentCoroutine;
		}
	}
}