using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.EventSequencer;

public class EventSequencerTests {

    [Test]
    public void StartSequence() {
        GameObject gameObject = new GameObject();
        var sequencer = gameObject.AddComponent<TestEventManager>();

        var context = new TestContext();

        EventSequenceList lowPrioritySequence = new EventSequenceList(new TestEvent(EventPriority.Low));
        EventSequenceList mediumPrioritySequence = new EventSequenceList(new TestEvent(EventPriority.Medium));
        EventSequenceList highPrioritySequence = new EventSequenceList(new TestEvent(EventPriority.High));
        sequencer.StartSequence(context, lowPrioritySequence);
        Assert.AreEqual(lowPrioritySequence, sequencer.GetCurrentSequence());

        sequencer.StartSequence(context, highPrioritySequence);
        Assert.AreEqual(highPrioritySequence, sequencer.GetCurrentSequence());

        sequencer.StartSequence(context, mediumPrioritySequence);
        Assert.AreEqual(highPrioritySequence, sequencer.GetCurrentSequence());
    }
}

public class TestEventManager : EventManagerBase { }