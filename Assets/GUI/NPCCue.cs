using System;

public class NPCCue {
	private float timeToShow;
	public float TimeToShow{
		get { return timeToShow; }
	}

	private string npcToShow;
	public float NPCToShow{
		get { return npcToShow; }
	}

	private string textToShow;
	public float TextToShow{
		get { return textToShow; }
	}

	public NPCCue(float timeToShow, string npcToShow, string textToShow){
		this.timeToShow = timeToShow;
		this.npcToShow = npcToShow;
		this.textToShow = textToShow;
	}
}