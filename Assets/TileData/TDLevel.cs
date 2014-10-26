using System.Collections.Generic;
using UnityEngine;

public class TDLevel {
	private TDMap _map;
	public TDMap Map{
		get {return _map; }
	}

	private EDArsonPath arsonPath;
	public EDArsonPath ArsonPath{
		get { return arsonPath; }
	}

	private float percentRemainingToWin;
	public float PercentRemainingToWin{
		get { return percentRemainingToWin; }
	}

	private float timeInSecondsToPlay;
	public float TimeInSecondsToPlay{
		get { return timeInSecondsToPlay; }
	}

	private Vector2 firehouseLocation;
	public Vector2 FirehouseLocation{
		get { return firehouseLocation; }
	}

	public TDLevel(){
		_map = new TDMap(32, 32);
		percentRemainingToWin = 0.90f;
		timeInSecondsToPlay = 90f;
		firehouseLocation = new Vector2(5f, 5f);
	}
}