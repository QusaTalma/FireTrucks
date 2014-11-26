using System;

public class PopUpEvent {
	public string message;
	public NPC speaker;
	public PopUpEvent(string message, NPC speaker) {
		this.message = message;
		this.speaker = speaker;
	}
}