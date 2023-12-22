using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.EventSequencer;

public class EventSequencerTests {

    [Test]
    public void StartSequence() {
        GameObject gameObject = new GameObject();
        var sequencer = gameObject.AddComponent<TestEventSequencer>();

        var context = new TestContext();

        EventSequence lowPrioritySequence = new EventSequence(new TestEvent(EventPriority.Low));
        EventSequence mediumPrioritySequence = new EventSequence(new TestEvent(EventPriority.Medium));
        EventSequence highPrioritySequence = new EventSequence(new TestEvent(EventPriority.High));
        sequencer.StartSequence(context, lowPrioritySequence);
        Assert.AreEqual(lowPrioritySequence, sequencer.GetCurrentSequence());

        sequencer.StartSequence(context, highPrioritySequence);
        Assert.AreEqual(highPrioritySequence, sequencer.GetCurrentSequence());

        sequencer.StartSequence(context, mediumPrioritySequence);
        Assert.AreEqual(highPrioritySequence, sequencer.GetCurrentSequence());
    }
}

public class TestEventSequencer : EventSequencerBase { }