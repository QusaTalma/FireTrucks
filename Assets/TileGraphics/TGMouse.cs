using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TGMap))]
[RequireComponent(typeof(Camera))]
public class TGMouse : MonoBehaviour {
	bool dragging = false;
	bool singleTouchDown = false;
	Vector3 singleTouchStart;
	Vector3 dragOrigin;
	TGMap _tileMap;

	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;

	private const float DRAG_SPEED = 15;

	void Start(){
		_tileMap = GetComponent<TGMap>();

		//The orthographic size if half the height of the camera
		float vertExtent = Camera.main.orthographicSize;
		float horizExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		float mapX = _tileMap.size_x * _tileMap.tileSize;
		float mapZ = _tileMap.size_z * _tileMap.tileSize;
		Debug.Log ("MapX: " + mapX);
		Debug.Log ("MapZ: " + mapZ);

		minX = 0 + horizExtent;
		//(float)(horizExtent - mapX / 2.0);
		maxX = mapX - horizExtent;
		//(float)(mapX / 2.0 - horizExtent);
		minZ = -(mapZ - vertExtent);
		//(float)(vertExtent - mapY / 2.0);
		maxZ = 0 - vertExtent;
		//(float)(mapY / 2.0 - vertExtent);

		Debug.Log ("MinX: " + minX);
		Debug.Log ("MaxX: " + maxX);
		Debug.Log ("MinZ: " + minZ);
		Debug.Log ("MaxZ: " + maxZ);
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch (0);

			Ray rayCast = Camera.main.ScreenPointToRay (touch.position);
			RaycastHit hitInfo;
			float distance = Mathf.Infinity;

			if (collider.Raycast (rayCast, out hitInfo, distance)) {
					switch (touch.phase) {
					case TouchPhase.Began:
							HandleTouchStart (hitInfo.point);
							break;

					case TouchPhase.Moved:
							HandleTouchMoved (touch.position);
							break;

					case TouchPhase.Ended:
							HandleTouchEnded (hitInfo.point);
							break;
					}
			}else{
				dragging = false;
				singleTouchDown = false;
			}
		} else {
			Ray rayCast = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			float distance = Mathf.Infinity;
			
			if (collider.Raycast (rayCast, out hitInfo, distance)) {
				if(Input.GetMouseButtonDown(0)){
					HandleTouchStart(hitInfo.point);
				}else if(Input.GetMouseButtonUp(0)){
					HandleTouchEnded(hitInfo.point);
				}else if(singleTouchDown){
					HandleTouchMoved(Input.mousePosition);
				}
			}else{
				dragging = false;
				singleTouchDown = false;
			}
		}

		Vector3 camPos = Camera.main.transform.position;
		camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
		camPos.z = Mathf.Clamp(camPos.z, minZ, maxZ);;

		Camera.main.transform.position = camPos;
	}

	void HandleTouchStart(Vector3 touchPos){
		singleTouchStart = touchPos;
		singleTouchDown = true;
	}

	void HandleTouchMoved(Vector3 touchPos){
		if (!dragging) {
			Vector3 worldTouch = Camera.main.ScreenToWorldPoint(touchPos);

			int tileX = Mathf.FloorToInt (worldTouch.x / _tileMap.tileSize);
			int tileZ = Mathf.FloorToInt (worldTouch.z / _tileMap.tileSize);
			tileZ += 1;

			int startTileX = Mathf.FloorToInt (singleTouchStart.x / _tileMap.tileSize);
			int startTileZ = Mathf.FloorToInt (singleTouchStart.z / _tileMap.tileSize);
			startTileZ += 1;

			if (startTileX != tileX ||
				startTileZ != tileZ) {
				dragging = true;
				dragOrigin = touchPos;
			}
		}

		if (dragging) {	
			Vector3 dragDelta = dragOrigin - touchPos;
			Vector3 screenDelta = Camera.main.ScreenToViewportPoint(dragDelta);
			Vector3 moveTo = new Vector3(screenDelta.x * DRAG_SPEED, 0, screenDelta.y * DRAG_SPEED);
			Camera.main.transform.Translate(moveTo, Space.World);

			dragOrigin = touchPos;
		}
	}

	void HandleTouchEnded(Vector3 touchPos){
		int tileX = Mathf.FloorToInt (touchPos.x / _tileMap.tileSize);
		int tileZ = Mathf.FloorToInt (touchPos.z / _tileMap.tileSize);
		tileZ += 1;

		if(!dragging && singleTouchDown){
			//Negate the z
			_tileMap.AddPositionToSpawnQueue(new Vector2(tileX, tileZ));
			_tileMap.SendIdleToPosition(tileX, tileZ);
		}else{
			dragging = false;
		}

		singleTouchDown = false;
	}
}
