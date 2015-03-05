using UnityEngine;
using System.Collections;

public class EGHouse : MonoBehaviour {
	private static int HOUSE_ROWS = 4;

	private TDTile _tile = null;
	
	void Start()
	{
		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / UnifiedAnimator.FLAME_COLUMNS, 1f / HOUSE_ROWS);
		renderer.sharedMaterial.SetTextureScale("_MainTex", size);

		int state = HOUSE_ROWS - 1;
		float offsetY = ((float)state/HOUSE_ROWS);
		//split into x and y indexes
		Vector2 offset = new Vector2(0, offsetY);
		
		renderer.material.SetTextureOffset("_MainTex", offset);
	}
	
	void Update(){
		float offsetX = 0;
		int state = HOUSE_ROWS - 1;

		if (_tile.durability == 0) {
			state = 0;
		}else if(_tile.OnFire){
			state = 2; 
		}else if(_tile.IsDamaged()){
			state = 1;
		}

		float offsetY = ((float)state/HOUSE_ROWS);
		//split into x and y indexes
		Vector2 offset = new Vector2(offsetX, offsetY);
		
		renderer.material.SetTextureOffset("_MainTex", offset);
	}

	public void setTile(TDTile tile){
		this._tile = tile;
	}
}