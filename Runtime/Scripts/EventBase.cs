using System;
using System.Collections;

namespace jmayberry.EventSequencer {
	public enum EventPriority {
		None,
		Low,
		Medium,
		High,
		Critical
	}

	[Serializable]
	public abstract class EventBase {
        public EventPriority Priority;
		public IContext context;

		public EventBase(EventPriority priority = EventPriority.Low) {
			this.Priority = priority;
		}

		public override string ToString() {
			return $"<EventBase:{this.GetHashCode()}, {this.Priority}>";
		}

		// Override this to impliment your event
		public IEnumerator Execute(IContext context) {
			this.context = context;
			yield return this.OnExecute(context);
			this.context = null;
		}

		public abstract IEnumerator OnExecute(IContext context);

		// Override this to cancel gracefully
		public virtual IEnumerator OnCancel() {
			this.context = null;
			yield return null;
		}
	}
}
