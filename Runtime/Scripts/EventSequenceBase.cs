using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using jmayberry.CustomAttributes;

namespace jmayberry.EventSequencer {
	public abstract class EventSequenceBase {
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
			return (this.GetCurrentEventPriority() > otherPriority);
		}

		public virtual bool ShouldOverride(EventSequenceBase otherSequence) {
			return this.ShouldOverride(otherSequence.GetCurrentEventPriority());
		}
	}
}
