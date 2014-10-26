using UnityEngine;
using System.Collections.Generic;

public class TGArsonist : MonoBehaviour
{
	TGMap _map;
	TDTile _currentTile;
	TDTile _previousTile;

	public float fireLightInterval;
	public float moveInterval;

	float timeSinceFireLight = 0f;
	float timeSinceMove = 0f;

	public GameObject flamePrefab;

	public void Update(){
		timeSinceFireLight += Time.deltaTime;
		timeSinceMove += Time.deltaTime;

		if (timeSinceFireLight >= fireLightInterval) {
			StartNearbyHouseOnFire();
			timeSinceFireLight = 0f;
		}

		if (timeSinceMove >= moveInterval) {
			MoveToNearbyTile();
			timeSinceMove = 0f;
		}
	}

	public void SetMap(TGMap map){
		this._map = map;
	}

	public void SetPosition(int x, int y){
		_currentTile = _map.Map.GetTile (x, y);
	}

	void StartNearbyHouseOnFire(){
		if (!HaveNeighboringFires ()) {
			List<TDTile> houses = GetNeighboringHouses();
			if(houses.Count > 0){
				int houseIndexToIgnite = Random.Range(0, houses.Count);
				TDTile tileToIgnite = houses[houseIndexToIgnite];
				GameObject flame = (GameObject)Instantiate (flamePrefab);
				
				Vector3 flamePos = _map.GetPositionForTile (tileToIgnite.GetX(), tileToIgnite.GetY());
				flamePos.x += 0.5f;
				flamePos.z -= 0.5f;
				
				flame.transform.position = flamePos;

				EGFlame egFlame = flame.GetComponent<EGFlame>();
				egFlame.SetTile(tileToIgnite);
				egFlame.SetMap(_map);
				egFlame.SetSpreadPrefab(flamePrefab);
				
				tileToIgnite.type = TDTile.TILE_HOUSE_ON_FIRE;
			}
		}
	}

	bool HaveNeighboringFires(){
		List<TDTile> neighboringFires = _map.Map.FindAdjacentTilesOfType (_currentTile, TDTile.TILE_HOUSE_ON_FIRE);
		return neighboringFires.Count > 0;
	}

	List<TDTile> GetNeighboringHouses(){
		return _map.Map.FindAdjacentTilesOfType (_currentTile, TDTile.TILE_HOUSE);
	}

	void MoveToNearbyTile(){
		List<TDTile> neighboringTiles = _map.Map.FindAdjacentTilesOfType (_currentTile, TDTile.TILE_STREET);
		neighboringTiles.Remove (_previousTile);

		int tileIndex = Random.Range (0, neighboringTiles.Count);

		_previousTile = _currentTile;
		_currentTile = neighboringTiles [tileIndex];
	}
}