using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using jmayberry.CustomAttributes;

namespace jmayberry.EventSequencer {
	public class EventSequenceList : EventSequenceBase {
		[Readonly] public List<EventBase> eventList = new List<EventBase>();
		[Readonly] public int currentIndex = 0;

        public EventSequenceList() : base() { }

        public EventSequenceList(IEnumerable<EventBase> events) : base(events) { }

        public EventSequenceList(params EventBase[] events) : base(events) { }

        public override string ToString() {
			return $"<EventSequenceList:{this.GetHashCode()}, {this.eventList.Count}, {((this.currentEvent != null) ? this.currentEvent : "null")}>";
		}

		public override void AddEvent(params EventBase[] events) {
			foreach (EventBase item in events) {
				this.eventList.Add(item);
			}
		}

		public override bool HasAnotherEvent() {
			return this.currentIndex < this.eventList.Count;
		}

		public override EventBase GetNextEvent() {
			this.currentEvent = this.eventList[currentIndex];
			return this.currentEvent;
		}

		public override EventPriority GetCurrentEventPriority() {
			if (this.currentEvent != null) {
				return this.currentEvent.Priority;
			}

			EventBase gameEvent = this.eventList[currentIndex];
			return (gameEvent == null) ? EventPriority.None : gameEvent.Priority;
		}

		public override IEnumerator Start(IContext context, Action<SequenceBase> callbackWhenFinished) {
			this.currentIndex = 0;
			return base.Start(context, callbackWhenFinished);
		}
	}
}
