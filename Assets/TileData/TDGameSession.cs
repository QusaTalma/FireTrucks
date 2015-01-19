using System.Collections.Generic;

/**
 * 
 * For tracking the game state
 * 
 **/
public class TDGameSession{
	float targetTime;
	float currentTime;
	List<NPCCue> cues;

	public TDGameSession (float targetTime, List<NPCCue> cues){
		this.targetTime = targetTime;
		currentTime = 0;
		this.cues = cues;
	}

	public void AddToCurrentTime(float increment){
		currentTime += increment;
	}

	public float GetRemainingTime(){
		return targetTime - currentTime;
	}

	public bool IsActive(){
		return GetRemainingTime() > 0;
	}

	public void ShowNPCCueIfReady(){
		if (cues.Count > 0) {
			NPCCue nextCue = cues[0];
			if(nextCue.TimeToShow <= currentTime){
				switch(nextCue.NPCToShow){
				case NPCCue.MAYOR:
					PopUpUIManager.Instance.ShowMayor(nextCue.TextToShow, true);
					break;

				case NPCCue.FIRE_CHIEF:
					PopUpUIManager.Instance.ShowFireChief(nextCue.TextToShow, true);
					break;

				case NPCCue.POLICE_CHIEF:
					PopUpUIManager.Instance.ShowPoliceChief(nextCue.TextToShow, true);
					break;
				}

				cues.Remove(nextCue);
			}
		}
	}
}