using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * For tracking the game state
 * 
 **/
public class TDGameSession{
	float targetTime;
	float currentTime;
	List<NPCCue> cues;
	TGArsonist arsonist;

	public TDGameSession (float targetTime, List<NPCCue> cues, TGArsonist arsonist){
		this.targetTime = targetTime;
		this.arsonist = arsonist;
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
		bool active = false;
		bool timeRemaining = GetRemainingTime () > 0;
		if (timeRemaining) {
			if(arsonist.ArsonStepCount == 0){
				GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");
				Debug.Log("Fire count: " + fires.Length);
				active = fires.Length > 0;
			}else{
				active = true;
			}
		}
		return active;
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