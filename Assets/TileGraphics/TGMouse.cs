using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(TGMap))]
[RequireComponent(typeof(Camera))]
public class TGMouse : MonoBehaviour {
	bool dragging = false;
	bool singleTouchDown = false;
	Vector3 singleTouchStart;
	Vector3 dragOrigin;
	TGMap _tileMap;

	EGDispatcher _dispatcher;

	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;

	private const float DRAG_SPEED = 15;

	void Start(){
		_tileMap = GetComponent<TGMap>();
		_dispatcher = GetComponent<EGDispatcher>();

		//The orthographic size if half the height of the camera
		float vertExtent = Camera.main.orthographicSize;
		//Calculate the half height of the screen
		float horizExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
		float mapX = _tileMap.Map.Width * _tileMap.tileSize;
		float mapZ = _tileMap.Map.Height * _tileMap.tileSize;
		//Limit the camera to within half the width/height of the screen
		//of the bounds of the map
		minX = 0 + horizExtent;
		maxX = mapX - horizExtent;
		minZ = -(mapZ - vertExtent);
		maxZ = 0 - vertExtent;
	}

	// Update is called once per frame
	void Update () {
		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}
		Ray rayCast;
		RaycastHit hitInfo;
		float distance = Mathf.Infinity;
		bool down = false;
		bool moved = false;
		bool up = false;
		Vector2 mapMovePos = new Vector2();
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch (0);
			rayCast = Camera.main.ScreenPointToRay (touch.position);

			switch(touch.phase){
			case TouchPhase.Began:
				down = true;
				break;

			case TouchPhase.Moved:
				moved = true;
				mapMovePos = touch.position;
				break;

			case TouchPhase.Ended:
				up = true;
				break;
			}
		} else {
			rayCast = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Input.GetMouseButtonDown(0)){
				down = true;
			}else if(Input.GetMouseButtonUp(0)){
				up = true;
			}else if(singleTouchDown){
				moved = true;
				mapMovePos = Input.mousePosition;
			}
		}

		if(Physics.Raycast(rayCast, out hitInfo, distance)) {
			bool onTruck = hitInfo.transform.root.gameObject.tag.Equals("TruckRoot");
			if(onTruck){
				if(down){
					HandleTouchTruckStart (hitInfo);
				}else if(moved){
					HandleTouchTruckMoved (hitInfo);
				}else if(up){
					HandleTouchTruckEnded (hitInfo);
				}
			}else{
				if(down){
					HandleTouchMapStart (hitInfo.point);
				}else if(moved){
					HandleTouchMapMoved (mapMovePos);
				}else if(up){
					HandleTouchMapEnded (hitInfo.point);
				}
			}
		} else {
			dragging = false;
			singleTouchDown = false;
		}

		Vector3 camPos = Camera.main.transform.position;
		camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
		camPos.z = Mathf.Clamp(camPos.z, minZ, maxZ);;

		Camera.main.transform.position = camPos;
	}

	void HandleTouchTruckStart(RaycastHit hitInfo){
		EGFiretruck truck = hitInfo.transform.root.gameObject.GetComponent<EGFiretruck> ();
		if (truck != null) {
			_dispatcher.SetSelectedTruck(truck);
		}
	}

	void HandleTouchTruckMoved(RaycastHit hitInfo){
		//Not sure what to do here
	}

	void HandleTouchTruckEnded(RaycastHit hitInfo){
		singleTouchDown = false;
		dragging = false;

		EGFiretruck truck = hitInfo.transform.root.gameObject.GetComponent<EGFiretruck> ();
		if (truck != null) {
			_dispatcher.SetSelectedTruck(truck);
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
			_dispatcher.AddPositionToSpawnQueue(new Vector2(tileX, tileZ));
			_dispatcher.SendIdleToPosition(tileX, tileZ);
		}else{
			dragging = false;
		}

		singleTouchDown = false;
	}
}
