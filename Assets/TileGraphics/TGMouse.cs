using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TGMap))]
public class TGMouse : MonoBehaviour {
	TGMap _tileMap;

	Vector3 currentTileCoord;

	public Transform selectionCube;

	void Start(){
		_tileMap = GetComponent<TGMap>();
		currentTileCoord = new Vector3 ();
	}

	// Update is called once per frame
	void Update () {
		Ray rayCast = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		float distance = Mathf.Infinity;

		if (collider.Raycast (rayCast, out hitInfo, distance)) {
			//transform.localToWorldMatrix
			int x = Mathf.FloorToInt (hitInfo.point.x / _tileMap.tileSize);
			int z = Mathf.FloorToInt (hitInfo.point.z / _tileMap.tileSize);
			z += 1;

			currentTileCoord.x = x + 0.5f;
			currentTileCoord.y = 0;
			currentTileCoord.z = z - 0.5f;

			selectionCube.transform.position = currentTileCoord;

			bool clicked = Input.GetMouseButtonDown(0);
			if(!clicked){
				for(int i=0; i<Input.touchCount; i++){
					Touch touch = Input.touches[i];
					if (touch.phase == TouchPhase.Ended){
						clicked = true;
						break;
					}
				}
			}

			if (clicked) {
				//Clicked a tile!?
				Debug.Log("Clicked tile: " + x + ", " + z);
				//Negate the z
				_tileMap.AddPositionToSpawnQueue(new Vector2(x, z));
				_tileMap.SendIdleToPosition(x,z);
			}
		} else {
			currentTileCoord.y = -1;
			
			selectionCube.transform.position = currentTileCoord;
		}
	}
}
