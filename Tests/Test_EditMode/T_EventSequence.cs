using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

using jmayberry.EventSequencer;

public class EventSequenceTests {

	private EventSequence CreateTestSequence(params EventPriority[] priorityList) {
		EventSequence sequence = new EventSequence();
		foreach (EventPriority priority in priorityList) {
			sequence.AddEvent(new TestEvent(priority));
		}
		return sequence;
	}

	[Test]
	public void AddEvent() {
		var sequence = CreateTestSequence(EventPriority.Medium, EventPriority.Low, EventPriority.High);
		Assert.AreEqual(3, sequence.GetEventCount());
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

		Assert.True(sequenceMedium.ShouldOverride(sequenceLow), "Medium overrides Low sequence");
		Assert.False(sequenceLow.ShouldOverride(sequenceMedium), "Low does not override Medium sequence");

		Assert.True(sequenceMedium.ShouldOverride(EventPriority.Low), "Medium overrides Low priority");
		Assert.False(sequenceMedium.ShouldOverride(EventPriority.Medium), "Medium does not override Medium priority");
		Assert.False(sequenceMedium.ShouldOverride(EventPriority.High), "Medium does not override Medium priority");
    }

    [UnityTest]
    public IEnumerator Start() {
        var sequence = CreateTestSequence(EventPriority.High);
        yield return sequence.Start(new TestContext());
        Assert.AreEqual(0, sequence.GetEventCount());
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

public class TestContext : IContext {
	public int count = 0;
}