using jmayberry.EventSequencer;
using System;

[Serializable]
public class DialogContext : IContext {
    public string Speaker;
    public string DialogueText;

    public override string ToString() {
        return $"<EventSequence:{this.GetHashCode()}, {this.Speaker}>";
    }
}

