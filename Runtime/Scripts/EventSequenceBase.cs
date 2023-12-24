using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using jmayberry.CustomAttributes;
using jmayberry.Spawner;
using System;

namespace jmayberry.EventSequencer {
	public abstract class EventSequenceBase : SequenceBase {
		[Header("EventSequencer")]
		[Readonly] public EventBase currentEvent;

		public EventSequenceBase() : base() { }
		
		public EventSequenceBase(IEnumerable<EventBase> events) {
			this.AddEvent(events);
		}

		public EventSequenceBase(params EventBase[] events) {
			this.AddEvent(events);
		}

		public override string ToString() {
			return $"<EventSequenceBase:{this.GetHashCode()}, {((this.currentEvent != null) ? this.currentEvent : "null")}>";
		}

		public void AddEvent(IEnumerable<EventBase> events) {
			this.AddEvent(events.ToArray());
		}

		public abstract void AddEvent(params EventBase[] events);

		public abstract bool HasAnotherEvent();

		public abstract EventBase GetNextEvent();

		public virtual IEnumerable YieldEvents() {
			while (this.HasAnotherEvent()) {
				yield return this.GetNextEvent();
			}
		}

		public override IEnumerator Start(IContext context, Action<SequenceBase> callbackWhenFinished) {
			foreach (EventBase gameEvent in this.YieldEvents()) {
				yield return gameEvent.Execute(context);
			}

			callbackWhenFinished?.Invoke(this);
        }

		public override IEnumerator OnCancel() {
			if (this.currentEvent != null) {
				yield return this.currentEvent.OnCancel();
				this.currentEvent = null;
			}
		}

		public override bool ShouldOverride(EventPriority otherPriority) {
            if (!this.HasAnotherEvent()) {
                return false; // Avoid empty sequences overriding non-empty ones
            }

            return base.ShouldOverride(otherPriority);
		}

		public override bool ShouldOverride(SequenceBase otherSequence) {
			if (!this.HasAnotherEvent()) {
				return false; // Avoid empty sequences overriding non-empty ones
			}

            return base.ShouldOverride(otherSequence);
        }
    }
}
