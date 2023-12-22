using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

using jmayberry.EventSequencer;
using System.Collections.Generic;

public class EventSequenceTests {

	private EventSequenceQueue CreateTestSequence(params EventPriority[] priorityList) {
		EventSequenceQueue sequence = new EventSequenceQueue();
		foreach (EventPriority priority in priorityList) {
			sequence.AddEvent(new TestEvent(priority));
		}
		return sequence;
	}

	[Test]
	public void AddEvent() {
		var sequence = CreateTestSequence(EventPriority.Medium, EventPriority.Low, EventPriority.High);
		Assert.AreEqual(3, sequence.eventQueue.Count);
	}

	[Test]
	public void GetNextEvent() {
		var sequence = CreateTestSequence(EventPriority.Medium, EventPriority.Low, EventPriority.High);
		Assert.AreEqual(EventPriority.Medium, sequence.GetNextEvent()?.Priority, "First event is Medium");
		Assert.AreEqual(EventPriority.Low, sequence.GetNextEvent()?.Priority, "Second event is Low");
		Assert.AreEqual(EventPriority.High, sequence.GetNextEvent()?.Priority, "Third event is High");
    }

    [Test]
    public void ShouldOverride() {
        var sequenceLow = CreateTestSequence(EventPriority.Low);
        var sequenceMedium = CreateTestSequence(EventPriority.Medium);
        var sequenceNever = new TestWontRunSequence(new TestEvent(EventPriority.High));

        Assert.True(sequenceMedium.ShouldOverride(sequenceLow), "Medium overrides Low sequence");
        Assert.False(sequenceLow.ShouldOverride(sequenceMedium), "Low does not override Medium sequence");
        Assert.False(sequenceNever.ShouldOverride(sequenceLow), "Empty does not override Low sequence");

        Assert.True(sequenceMedium.ShouldOverride(EventPriority.Low), "Medium overrides Low priority");
        Assert.False(sequenceMedium.ShouldOverride(EventPriority.Medium), "Medium does not override Medium priority");
        Assert.False(sequenceMedium.ShouldOverride(EventPriority.High), "Medium does not override Medium priority");
        Assert.False(sequenceNever.ShouldOverride(EventPriority.Low), "Empty does not override Low sequence");
    }

    [UnityTest]
	public IEnumerator Start() {
		var sequence = CreateTestSequence(EventPriority.High);
		yield return sequence.Start(new TestContext());
		Assert.AreEqual(0, sequence.eventQueue.Count);
	}

	[UnityTest]
	public IEnumerator Context() {
		var context = new TestContext();
		Assert.AreEqual(0, context.count, "No events have run");

		var sequence = CreateTestSequence(EventPriority.High, EventPriority.Medium, EventPriority.Low);
		yield return sequence.Start(context);
		Assert.AreEqual(3, context.count, "3 events have run");
	}
}


public class TestEvent : EventBase {
	public TestEvent(EventPriority priority) : base(priority) { }

	public override IEnumerator OnExecute(IContext context) {
		if (context is TestContext testContext) {
			testContext.count++;
		}

		yield return null;
	}
}

public class TestWontRunSequence : EventSequenceQueue {
    public TestWontRunSequence(params EventBase[] events) : base(events) { }

    public override bool HasAnotherEvent() {
		return false;
	}
}

public class TestContext : IContext {
	public int count = 0;
}