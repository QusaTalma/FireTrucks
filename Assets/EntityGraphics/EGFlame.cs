using UnityEngine;
using System.Collections.Generic;

public class EGFlame : MonoBehaviour {
	public float growthRate;
	public float maxSize;
	public int spreadChance;
	public float spreadInterval;

	float timeSinceSpreadAttempt = 0;

	GameObject spreadPrefab;

	TGMap map;
	TDTile tile;
	
	// Update is called once per frame
	void Update () {
		float delta = Time.deltaTime;

		Vector3 scale = transform.localScale;
		if (scale.x < maxSize){
			scale.x += Mathf.Min(growthRate * delta, maxSize);
			scale.y += Mathf.Min(growthRate * delta, maxSize);
			scale.z += Mathf.Min(growthRate * delta, maxSize);

			transform.localScale = scale;
		}

		if (scale.x >= maxSize) {
			timeSinceSpreadAttempt += delta;

			if(timeSinceSpreadAttempt >= spreadInterval){
				int rand = Random.Range (0, spreadChance);
				if (rand % spreadChance == 0){
					List<TDTile> houses = map.GetDataMap().FindAdjacentTilesOfType(tile, TDTile.TILE_HOUSE);
					
					if(houses.Count > 0) {
						rand = Random.Range(0, houses.Count);
						TDTile tileToIgnite = houses[rand];
						GameObject flame = (GameObject)Instantiate (spreadPrefab);
						
						Vector3 flamePos = map.GetPositionForTile (tileToIgnite.GetX(), tileToIgnite.GetY());
						flamePos.x += 0.5f;
						flamePos.z -= 0.5f;
						
						flame.transform.position = flamePos;
						
						EGFlame egFlame = flame.GetComponent<EGFlame>();
						egFlame.SetTile(tileToIgnite);
						egFlame.SetMap(map);
						egFlame.SetSpreadPrefab(spreadPrefab);
						
						tileToIgnite.type = TDTile.TILE_HOUSE_ON_FIRE;
					}

					timeSinceSpreadAttempt = 0;
				}
			}
		}
	}

	public void SetMap(TGMap map){
		this.map = map;
	}

	public void SetTile(TDTile tile){
		this.tile = tile;
	}

	public void SetSpreadPrefab(GameObject prefab){
		this.spreadPrefab = prefab;
	}
}