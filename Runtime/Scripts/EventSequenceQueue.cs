using System.Collections;
using System.Collections.Generic;
using System.Linq;

using jmayberry.CustomAttributes;

namespace jmayberry.EventSequencer {
	public class EventSequenceQueue : EventSequenceBase {
		[Readonly] public Queue<EventBase> eventQueue = new Queue<EventBase>();

        public EventSequenceQueue() : base() { }

        public EventSequenceQueue(IEnumerable<EventBase> events) : base(events) { }

        public EventSequenceQueue(params EventBase[] events) : base(events) { }

        public override string ToString() {
			return $"<EventSequenceQueue:{this.GetHashCode()}, {this.eventQueue.Count}, {((this.currentEvent != null) ? this.currentEvent : "null")}>";
		}

		public override void AddEvent(params EventBase[] events) {
			foreach (EventBase item in events) {
				this.eventQueue.Enqueue(item);
			}
		}

		public override bool HasAnotherEvent() {
			return this.eventQueue.Count > 0;
		}

		public override EventBase GetNextEvent() {
			this.currentEvent = eventQueue.Dequeue();
			return this.currentEvent;
		}

		public override EventPriority GetCurrentEventPriority() {
			if (this.currentEvent != null) {
				return this.currentEvent.Priority;
			}

			EventBase gameEvent;
			this.eventQueue.TryPeek(out gameEvent);
			return (gameEvent == null) ? EventPriority.None : gameEvent.Priority;
		}

		public override IEnumerator OnCancel() {
			yield return base.OnCancel();
			this.eventQueue.Clear();
		}
	}
}
