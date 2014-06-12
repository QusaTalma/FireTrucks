using UnityEngine;
using System.Collections.Generic;

public class TGArsonist
{
	TDMap _map;
	TDTile _currentTile;
	TDTile _previousTile;

	int x;
	int y;

	static float FIRE_LIGHT_INTERVAL = 1f;
	static float MOVE_INTERVAL = .75f;

	float timeSinceFireLight = 0f;
	float timeSinceMove = 0f;

	public TGArsonist (){}

	public void Update(float delta){
		timeSinceFireLight += delta;
		timeSinceMove += delta;

		if (timeSinceFireLight >= FIRE_LIGHT_INTERVAL) {
			StartNearbyHouseOnFire();
			timeSinceFireLight = 0f;
		}

		if (timeSinceMove >= MOVE_INTERVAL) {
			MoveToNearbyTile();
			timeSinceMove = 0f;
		}
	}

	public void SetMap(TDMap map){
		this._map = map;
	}

	public void SetPosition(int x, int y){
		this.x = x;
		this.y = y;

		_currentTile = _map.GetTile (x, y);
	}

	void StartNearbyHouseOnFire(){
		//TODO find a nearby hous eto set on fire:
		//Check for fires nearby, if any present skip starting one this time
		//Find a nearby house
		//Create a flame on that house and set the tile's type to be on fire
		//Object.Instantiate
	}

	void MoveToNearbyTile(){
		List<TDTile> neighboringTiles = _map.FindAdjacentTilesOfType (_currentTile, TDTile.TILE_STREET);
		neighboringTiles.Remove (_previousTile);

		int tileIndex = Random.Range (0, neighboringTiles.Count);

		_previousTile = _currentTile;
		_currentTile = neighboringTiles [tileIndex];
	}
}