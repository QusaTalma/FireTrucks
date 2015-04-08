using System;

namespace DataStructure.UI{
	public class PopUpEvent {
		public string message;
		public float duration;
		public NPC speaker;
		public PopUpEvent(string message, float duration, NPC speaker) {
			this.message = message;
			this.duration = duration;
			this.speaker = speaker;
		}
	}
}