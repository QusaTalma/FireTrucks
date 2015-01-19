using UnityEngine;
using System.Collections;

public class TruckAnimator : MonoBehaviour {
	//the current frame to display
	const int NUM_STATES = 4;
	const int STATE_IDLE = 3;
	const int STATE_ACTIVE = 2;
	const int STATE_SELECTED = 1;
	const int STATE_SELECTED_ACTIVE = 0;

	private EGFiretruck truck;
	private int lastIndex = 0;

	private int currentState = STATE_IDLE;
	
	void Start()
	{
		truck = transform.root.gameObject.GetComponent<EGFiretruck> ();
		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / UnifiedAnimator.FLAME_COLUMNS, 1f / NUM_STATES);
		renderer.sharedMaterial.SetTextureScale("_MainTex", size);
	}
	
	void Update(){
		bool active = truck.IsActive ();
		bool selected = truck.IsSelected ();

		int state;
		
		if(active){
			state = selected ? STATE_SELECTED_ACTIVE : STATE_ACTIVE;
		}else{
			state = selected ? STATE_SELECTED : STATE_IDLE;
		}

		if (lastIndex != UnifiedAnimator.FlameFrame || currentState != state) {
			currentState = state;
			lastIndex = UnifiedAnimator.FlameFrame;

			//split into x and y indexes
			float offsetX = (float)lastIndex / UnifiedAnimator.FLAME_COLUMNS - (lastIndex / UnifiedAnimator.FLAME_COLUMNS);

			
			float offsetY = ((float)state/NUM_STATES);
			//split into x and y indexes
			Vector2 offset = new Vector2(offsetX, offsetY);

			renderer.material.SetTextureOffset("_MainTex", offset);
		}
	}
}