using System.Collections.Generic;
using UnityEngine;

public class TDPath {
	List<TDStep> steps;

	public TDPath (){
	}

	public TDStep GetStepAt(int index){
		return steps [index];
	}

	public int GetStepCount(){
		return steps.Count;
	}

	public TDStep PopStep(){
		TDStep step = null;

		if (steps != null && steps.Count > 0) {
			step = steps[0];
			steps.Remove(step);
		}

		return step;
	}

	public void BuildPath(TDMap map, TDTile start, TDTile end){
		if (end.type != TDTile.TILE_STREET) {
			List<TDTile> nearbyStreets = map.FindAdjacentTilesOfType(end, TDTile.TILE_STREET);
			if(nearbyStreets.Count > 0){
				end = nearbyStreets[0];
			}
		}
		Debug.Log ("Building path from " + start.ToString () + " to " + end.ToString ());
		List<TDTile> closed = new List<TDTile> ();
		List<TDTile> open = new List<TDTile> ();
		open.Add (start);
		Dictionary<TDTile, TDTile> cameFrom = new Dictionary<TDTile, TDTile> ();

		while (open.Count > 0) {
			TDTile current = open[0];

			if(current.Equals(end)){
				steps = ReconstructPath(cameFrom, end);

				Debug.Log("Final path:");

				for(int i=0; i<steps.Count; i++){
					Debug.Log("Step #"+i+": " + steps[i].ToString());
				}
				break;
			}

			open.Remove(current);
			closed.Add(current);

			List<TDTile> neighborStreets = map.FindAdjacentTilesOfType(current, TDTile.TILE_STREET);

			for(int i=0; i<neighborStreets.Count; i++){
				TDTile neighbor = neighborStreets[i];
				if(closed.Contains(neighbor)){
					continue;
				}

				if(!open.Contains(neighbor)){
					cameFrom.Add(neighbor, current);
					open.Add(neighbor);
				}
			}
		}
	}

	override public string ToString(){
		string asString = "TDPath: (" + steps.Count + ")";

		for (int i=0; i<steps.Count; i++) {
			asString += "\r\nStep #"+i+": " + steps[i].ToString();
		}

		return asString;
	}

	private List<TDStep> ReconstructPath(Dictionary<TDTile, TDTile> cameFrom, TDTile current){
		List<TDStep> pathSteps = new List<TDStep> ();
		if (cameFrom.ContainsKey (current)) {
			TDTile previous;
			if(cameFrom.TryGetValue(current, out previous)){
				pathSteps = ReconstructPath(cameFrom, previous);
				pathSteps.Add(new TDStep(current));
			}
		} else {
			pathSteps = new List<TDStep>();
			pathSteps.Add(new TDStep(current));
		}

		for (int i=0; i<pathSteps.Count; i++) {
			Debug.Log(pathSteps[i].ToString());
		}

		return pathSteps;
	}
}