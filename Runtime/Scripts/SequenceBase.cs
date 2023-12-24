using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using jmayberry.CustomAttributes;
using jmayberry.Spawner;
using System;

namespace jmayberry.EventSequencer {
    public abstract class SequenceBase : ISpawnable {
        public SequenceBase() { }

        public override string ToString() {
            return $"<SequenceBase:{this.GetHashCode()}>";
        }

        public abstract EventPriority GetCurrentEventPriority();

        public abstract IEnumerator Start(IContext context, Action<SequenceBase> callbackWhenFinished);

        public abstract IEnumerator OnCancel();

        public virtual bool ShouldOverride(EventPriority otherPriority) {
            return (this.GetCurrentEventPriority() > otherPriority);
        }

        public virtual bool ShouldOverride(SequenceBase otherSequence) {
            return this.ShouldOverride(otherSequence.GetCurrentEventPriority());
        }

        public virtual void OnSpawn(object spawner) { }

        public virtual void OnDespawn(object spawner) { }
    }
}
