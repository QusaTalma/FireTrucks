using System;

namespace DataStructure.UI{
	public class NPCCue {
		public const string MAYOR = "m";
		public const string FIRE_CHIEF = "f";
		public const string POLICE_CHIEF = "p";

		private float timeToShow;
		public float TimeToShow{
			get { return timeToShow; }
		}

		private float duration;
		public float Duration {
			get { return duration; }
		}

		private string npcToShow;
		public string NPCToShow{
			get { return npcToShow; }
		}

		private string textToShow;
		public string TextToShow{
			get { return textToShow; }
		}

		public NPCCue(float timeToShow, float duration, string npcToShow, string textToShow){
			this.timeToShow = timeToShow;
			this.duration = duration;
			this.npcToShow = npcToShow;
			this.textToShow = textToShow;
		}
	}
}