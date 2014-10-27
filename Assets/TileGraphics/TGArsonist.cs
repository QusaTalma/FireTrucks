using UnityEngine;
using System.Collections.Generic;

public class TGArsonist : MonoBehaviour
{
	private TGMap _map;
	private float elapsedTime = 0f;

	public GameObject flamePrefab;

	private EDArsonPath arsonPath;
	public EDArsonPath ArsonPath{
		set { arsonPath = value; }
	}

	public void Update(){
		elapsedTime += Time.deltaTime;

		if (arsonPath.HasMoreSteps () && arsonPath.TimeForNextStep(elapsedTime) ) {
			TDTile tileToLight = arsonPath.PopStep();
			StartTileOnFire(tileToLight);
		}
	}

	public void SetMap(TGMap map){
		this._map = map;
	}

	void StartTileOnFire(TDTile tile){
		GameObject flame = (GameObject)Instantiate (flamePrefab);
		
		Vector3 flamePos = _map.GetPositionForTile (tile.GetX(), tile.GetY());
		flamePos.x += 0.5f;
		flamePos.z -= 0.5f;
		
		flame.transform.position = flamePos;
		
		EGFlame egFlame = flame.GetComponent<EGFlame>();
		egFlame.SetTile(tile);
		egFlame.SetMap(_map);
		egFlame.SetSpreadPrefab(flamePrefab);
		
		tile.type = TDTile.TILE_HOUSE_ON_FIRE;
	}
}