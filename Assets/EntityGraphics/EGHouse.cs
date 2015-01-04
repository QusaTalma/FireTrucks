using UnityEngine;
using System.Collections;

public class EGHouse : MonoBehaviour {
	private static int HOUSE_ROWS = 4;
	
	//the current frame to display
	private int lastIndex = 0;

	private TDTile _tile = null;
	
	void Start()
	{
		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / UnifiedAnimator.FLAME_COLUMNS, 1f / HOUSE_ROWS);
		renderer.sharedMaterial.SetTextureScale("_MainTex", size);
	}
	
	void Update(){
		if (lastIndex != UnifiedAnimator.FlameFrame) {
			lastIndex = UnifiedAnimator.FlameFrame;
			float offsetX = (float)lastIndex / UnifiedAnimator.FLAME_COLUMNS - (lastIndex / UnifiedAnimator.FLAME_COLUMNS);
			int state = HOUSE_ROWS - 1;

			switch(_tile.type){
			case TDTile.TILE_HOUSE:
				state = 3;
				break;

			case TDTile.TILE_HOUSE_ON_FIRE:
				state = 2;
				break;

			case TDTile.TILE_DAMAGED_HOUSE:
				state = 1;
				break;

			case TDTile.TILE_BURNED_DOWN_HOUSE:
				state = 0;
				break;
			}

			float offsetY = ((float)state/HOUSE_ROWS);
			//split into x and y indexes
			Vector2 offset = new Vector2(offsetX, offsetY);
			
			renderer.material.SetTextureOffset("_MainTex", offset);
		}
	}

	public void setTile(TDTile tile){
		this._tile = tile;
	}
}