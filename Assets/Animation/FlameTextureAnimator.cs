using UnityEngine;
using System.Collections;

public class FlameTextureAnimator : MonoBehaviour {
	//the current frame to display
	private static int lastIndex = 0;
	
	void Start()
	{
		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / UnifiedAnimator.FLAME_COLUMNS, 1f / UnifiedAnimator.FLAME_ROWS);
		GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
	}

	void Update(){
		if (lastIndex != UnifiedAnimator.FlameFrame) {
			lastIndex = UnifiedAnimator.FlameFrame;
			//split into x and y indexes
			Vector2 offset = new Vector2((float)lastIndex / UnifiedAnimator.FLAME_COLUMNS - (lastIndex / UnifiedAnimator.FLAME_COLUMNS), //x index
			                             (lastIndex / UnifiedAnimator.FLAME_COLUMNS) / (float)UnifiedAnimator.FLAME_ROWS);          //y index
			
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
		}
	}
}