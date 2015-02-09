using UnityEngine;
using System.Collections.Generic;

public class EGFlame : MonoBehaviour {
	public float growthRate;
	public float maxSize;
	public int spreadChance;
	public float spreadInterval;
	public float damagePerSecond;

	float timeSinceSpreadAttempt = 0;

	GameObject spreadPrefab;

	TGMap map;
	TDTile tile;

	void Start(){
		PopUpUIManager.Instance.ShowFlameIndicator (this);
	}

	// Update is called once per frame
	void Update () {
		if (tile != null) {
			UpdateScale ();
			UpdateSpread ();
			DoDamage ();
		} else {
			PutOut();
			Destroy(transform.root.gameObject);
		}
	}

	void UpdateScale(){
		float delta = Time.deltaTime;
		
		Vector3 scale = transform.root.localScale;

		if(tile.durability <= 0){
			scale.x -= growthRate * delta;
			scale.y -= growthRate * delta;
			scale.z -= growthRate * delta;
			
			transform.root.localScale = scale;
		}else if (scale.x < maxSize && scale.x > 0){
			scale.x += Mathf.Min(growthRate * delta, maxSize);
			scale.y += Mathf.Min(growthRate * delta, maxSize);
			scale.z += Mathf.Min(growthRate * delta, maxSize);
			
			transform.root.localScale = scale;
		}

		if(scale.x <= 0){
			PutOut();
			Destroy(transform.root.gameObject);
		}
	}

	void UpdateSpread(){
		float delta = Time.deltaTime;
		
		Vector3 scale = transform.root.localScale;
		
		if (scale.x >= maxSize || tile.durability <= 0) {
			timeSinceSpreadAttempt += delta;
			
			if(timeSinceSpreadAttempt >= spreadInterval){
				int rand = Random.Range (0, spreadChance);
				if (rand % spreadChance == 0){
					List<TDTile> houses = map.Map.FindAdjacentFlammableTiles(tile);
					
					if(houses.Count > 0) {
						rand = Random.Range(0, houses.Count);
						TDTile tileToIgnite = houses[rand];
						if(!tileToIgnite.OnFire && tile.durability > 0){
							GameObject flame = (GameObject)Instantiate (spreadPrefab);
							
							Vector3 flamePos = map.GetPositionForTile (tileToIgnite.GetX(), tileToIgnite.GetY());
							flamePos.x += 0.5f;
							flamePos.z -= 0.5f;
							
							flame.transform.position = flamePos;
							
							EGFlame egFlame = flame.GetComponent<EGFlame>();
							egFlame.SetTile(tileToIgnite);
							egFlame.SetMap(map);
							egFlame.SetSpreadPrefab(spreadPrefab);
							
							PopUpUIManager.Instance.ShowFireChief("Fires are spreading! get them under control!");
							
							tileToIgnite.OnFire = true;
						}
					}
					
					timeSinceSpreadAttempt = 0;
				}
			}
		}
	}

	void DoDamage(){
		float deltaTime = Time.deltaTime;
		Vector3 scale = transform.root.localScale;
		float damage = (deltaTime * damagePerSecond * (scale.x / maxSize));
		tile.Damage (damage);
	}

	public void PutOut(){
		if (tile.durability <= 0) {
			PopUpUIManager.Instance.ShowMayor("Oh dear, a building burned down");
		}

		tile.OnFire = false;
	}

	public void SetMap(TGMap map){
		this.map = map;
	}

	public void SetTile(TDTile tile){
		this.tile = tile;
		tile.OnFire = true;
	}

	public void SetSpreadPrefab(GameObject prefab){
		this.spreadPrefab = prefab;
	}

	public bool IsOnCamera(){
		Vector3 camPos = Camera.main.transform.position;
		camPos.x = camPos.x - LevelGUIManager.Instance.statusPanel.transform.root.localScale.x;
		//The orthographic size if half the height of the camera
		float vertExtent = Camera.main.orthographicSize;
		//Calculate the half height of the screen
		float horizExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
		horizExtent = horizExtent - LevelGUIManager.Instance.statusPanel.transform.root.localScale.x;

		Rect cameraRect = new Rect (camPos.x - horizExtent, camPos.z - vertExtent, horizExtent * 2, vertExtent * 2);
		Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
		return cameraRect.Contains (myPos);
	}
}