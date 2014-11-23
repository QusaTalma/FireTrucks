using System;

/**
 * 
 * For tracking the game state
 * 
 **/
public class TDGameSession{
	float targetTime;
	float currentTime;

	public TDGameSession (float targetTime){
		this.targetTime = targetTime;
		currentTime = 0;
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
}