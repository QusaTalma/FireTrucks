using UnityEngine;
using System.Collections;

public class UnifiedAnimator : MonoBehaviour{
	public int flameColumns = 1;
	public static int FLAME_COLUMNS = 1;
	public int flameRows = 1;
	public static int FLAME_ROWS = 1;
	public float flameFramesPerSecond = 10f;
	public static float FLAME_FPS = 10f;

	public static int FlameFrame = 0;

	// Use this for initialization
	void Start(){
		FLAME_COLUMNS = flameColumns;
		FLAME_ROWS = flameRows;
		FLAME_FPS = flameFramesPerSecond;

		StartCoroutine(updateFlameIndex());
	}
	
	private IEnumerator updateFlameIndex()
	{
		while (true)
		{
			//move to the next index
			FlameFrame++;
			if (FlameFrame >= flameRows * flameColumns){
				FlameFrame = 0;
			}

			yield return new WaitForSeconds(1f / flameFramesPerSecond);
		}
		
	}
}