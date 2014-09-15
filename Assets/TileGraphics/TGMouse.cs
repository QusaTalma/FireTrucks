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
		//Calculate the half height of the screen
		float horizExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
		float mapX = _tileMap.size_x * _tileMap.tileSize;
		float mapZ = _tileMap.size_z * _tileMap.tileSize;
		//Limit the camera to within half the width/height of the screen
		//of the bounds of the map
		minX = 0 + horizExtent;
		maxX = mapX - horizExtent;
		minZ = -(mapZ - vertExtent);
		maxZ = 0 - vertExtent;
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch (0);

			Ray rayCast = Camera.main.ScreenPointToRay (touch.position);
			RaycastHit hitInfo;
			float distance = Mathf.Infinity;

			if(Physics.Raycast(rayCast, out hitInfo, distance)) {
				if(hitInfo.transform.root.gameObject.tag.Equals("Map")){
					switch (touch.phase) {
					case TouchPhase.Began:
							HandleTouchMapStart (hitInfo.point);
							break;

					case TouchPhase.Moved:
							HandleTouchMapMoved (touch.position);
							break;

					case TouchPhase.Ended:
							HandleTouchMapEnded (hitInfo.point);
							break;
					}
				}else if(hitInfo.transform.root.gameObject.tag.Equals("TruckRoot")){
					switch (touch.phase) {
					case TouchPhase.Began:
						HandleTouchTruckStart (hitInfo);
						break;
						
					case TouchPhase.Moved:
						HandleTouchTruckMoved (hitInfo);
						break;
						
					case TouchPhase.Ended:
						HandleTouchTruckEnded (hitInfo);
						break;
					}
				}
			}else{
				dragging = false;
				singleTouchDown = false;
			}
		} else {
			Ray rayCast = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			float distance = Mathf.Infinity;
			
			if(Physics.Raycast(rayCast, out hitInfo, distance)) {
				if(hitInfo.transform.root.gameObject.tag.Equals("Map")){
					if(Input.GetMouseButtonDown(0)){
						HandleTouchMapStart(hitInfo.point);
					}else if(Input.GetMouseButtonUp(0)){
						HandleTouchMapEnded(hitInfo.point);
					}else if(singleTouchDown){
						HandleTouchMapMoved(Input.mousePosition);
					}
				}else if(hitInfo.transform.root.gameObject.tag.Equals("TruckRoot")){
					if(Input.GetMouseButtonDown(0)){
						HandleTouchTruckStart(hitInfo);
					}else if(Input.GetMouseButtonUp(0)){
						HandleTouchTruckEnded(hitInfo);
					}else if(singleTouchDown){
						HandleTouchTruckMoved(hitInfo);
					}
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

	void HandleTouchTruckStart(RaycastHit hitInfo){
		singleTouchStart = hitInfo.point;
		singleTouchDown = true;

		//Not sure what else this needs
	}

	void HandleTouchTruckMoved(RaycastHit hitInfo){
		//Not sure what to do here
	}

	void HandleTouchTruckEnded(RaycastHit hitInfo){
		singleTouchDown = false;

		EGFiretruck truck = hitInfo.transform.root.gameObject.GetComponent<EGFiretruck> ();
		if (truck != null) {
			_tileMap.SetSelectedTruck(truck);
		}
	}

	void HandleTouchMapStart(Vector3 touchPos){
		singleTouchStart = touchPos;
		singleTouchDown = true;
	}

	void HandleTouchMapMoved(Vector3 touchPos){
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

	void HandleTouchMapEnded(Vector3 touchPos){
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
