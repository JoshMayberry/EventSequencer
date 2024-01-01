using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.CustomAttributes;

namespace jmayberry.EventSequencer {
	public class EventManagerBase : MonoBehaviour {
		protected SequenceBase currentSequence;
		protected Coroutine currentCoroutine;

		[Readonly] [SerializeField] protected bool isOverriding;
		[Readonly] [SerializeField] protected bool isSequenceRunning;

		public override string ToString() {
			return $"<EventManagerBase:{this.GetHashCode()}>";
		}

		public virtual void StartSequence(IContext context, SequenceBase sequence, bool hardStop = false) {
			if (this.isSequenceRunning) {
				if (!sequence.ShouldOverride(this.currentSequence)) {
					return;
				}

				this.isOverriding = true;
				this.StopSequence(this.currentSequence, this.currentCoroutine, hardStop);
			}

			this.isSequenceRunning = true;
			this.currentSequence = sequence;
			this.isOverriding = false;
			this.currentCoroutine = StartCoroutine(this.currentSequence.Start(context, OnSequenceFinished));
		}

		protected virtual void OnSequenceFinished(SequenceBase sequence) {
			if (sequence == this.currentSequence) {
				this.isSequenceRunning = false;
			}
		}

		public void StopSequence(bool hardStop = false) {
			StopSequence(this.currentSequence, this.currentCoroutine, hardStop);
		}

		public virtual void StopSequence(SequenceBase sequence, Coroutine coroutine, bool hardStop = false) {
			StartCoroutine(this.Cancel(sequence, coroutine, hardStop));
			this.isSequenceRunning = false;
		}

		protected virtual IEnumerator Cancel(SequenceBase sequence, Coroutine coroutine, bool hardStop = false) {
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