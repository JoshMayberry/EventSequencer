using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace jmayberry.EventSequencer {
	public class EventSequence {
		private EventBase currentEvent;
		private Queue<EventBase> eventList = new Queue<EventBase>();

		public EventSequence() { }

		
		public EventSequence(IEnumerable<EventBase> eventList) {
			this.AddEvent(eventList);
		}

		public EventSequence(params EventBase[] eventList) {
			this.AddEvent(eventList);
        }

		public override string ToString() {
			return $"<EventSequence:{this.GetHashCode()}, {this.eventList.Count}, {((this.currentEvent != null) ? this.currentEvent : "null")}>";
        }

        public void AddEvent(IEnumerable<EventBase> events) {
            foreach (EventBase item in events) {
                this.eventList.Enqueue(item);
            }
        }

        public void AddEvent(params EventBase[] events) {
            foreach (EventBase item in events) {
				this.eventList.Enqueue(item);
            }
        }

		public int GetEventCount() {
			return this.eventList.Count;
		}

		public EventBase GetNextEvent() {
			this.currentEvent = eventList.Dequeue();
			return this.currentEvent;
		}

		public IEnumerable YieldEvents() {
			while (this.eventList.Count > 0) {
				yield return this.GetNextEvent();
			}
		}

		public IEnumerator Start(IContext context) {
            foreach (EventBase gameEvent in this.YieldEvents()) {
				yield return gameEvent.Execute(context);
			}
        }

        public IEnumerator OnCancel() {
			if (this.currentEvent != null) {
				yield return this.currentEvent.OnCancel();
				this.currentEvent = null;
				this.eventList.Clear();
			}
		}

		public virtual bool ShouldOverride(EventPriority otherPriority) {
			return (this.GetCurrentEventPriority() > otherPriority);
		}

		public virtual bool ShouldOverride(EventSequence otherSequence) {
			return this.ShouldOverride(otherSequence.GetCurrentEventPriority());
		}

		public EventPriority GetCurrentEventPriority() {
			if (this.currentEvent != null) {
				return this.currentEvent.Priority;
			}

			EventBase gameEvent;
			this.eventList.TryPeek(out gameEvent);
			return (gameEvent == null) ? EventPriority.None : gameEvent.Priority;
		}
	}
}
