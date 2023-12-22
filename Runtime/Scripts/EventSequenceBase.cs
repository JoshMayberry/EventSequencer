using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using jmayberry.CustomAttributes;
using jmayberry.Spawner;

namespace jmayberry.EventSequencer {
	public abstract class EventSequenceBase : ISpawnable {
		[Header("EventSequencer")]
		[Readonly] public EventBase currentEvent;

		public EventSequenceBase() { }
		
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

		public abstract EventPriority GetCurrentEventPriority();

		public virtual IEnumerable YieldEvents() {
			while (this.HasAnotherEvent()) {
				yield return this.GetNextEvent();
			}
		}

		public virtual IEnumerator Start(IContext context) {
			foreach (EventBase gameEvent in this.YieldEvents()) {
				yield return gameEvent.Execute(context);
			}
		}

		public virtual IEnumerator OnCancel() {
			if (this.currentEvent != null) {
				yield return this.currentEvent.OnCancel();
				this.currentEvent = null;
			}
		}

		public virtual bool ShouldOverride(EventPriority otherPriority) {
            if (!this.HasAnotherEvent()) {
                return false; // Avoid empty sequences overriding non-empty ones
            }

            return (this.GetCurrentEventPriority() > otherPriority);
		}

		public virtual bool ShouldOverride(EventSequenceBase otherSequence) {
			if (!this.HasAnotherEvent()) {
				return false; // Avoid empty sequences overriding non-empty ones
			}

			return this.ShouldOverride(otherSequence.GetCurrentEventPriority());
        }
        public void OnSpawn(object spawner) { }

        public void OnDespawn(object spawner) { }
    }
}
