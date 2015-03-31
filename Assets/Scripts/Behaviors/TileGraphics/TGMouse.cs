using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(TGMap))]
[RequireComponent(typeof(Camera))]
public class TGMouse : MonoBehaviour {
	bool dragging = false;
	bool singleTouchDown = false;
	Vector3 singleTouchStart;
	Vector3 previousTouch;
	TGMap _tileMap;

	EGDispatcher _dispatcher;

	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;

	private const float DRAG_THRESDHOLD = 0.1f;

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
//		EventSystem eventSys = EventSystem.current;
		//Check for up to 5 pointers, after that it's just rediculous
//		if (eventSys.IsPointerOverGameObject() ||
//		    eventSys.IsPointerOverGameObject(0) ||
//		    eventSys.IsPointerOverGameObject(1) ||
//		    eventSys.IsPointerOverGameObject(2) ||
//		    eventSys.IsPointerOverGameObject(3) ||
//		    eventSys.IsPointerOverGameObject(4)){
//			if(Input.touchCount > 0 ||
//			   Input.GetMouseButtonDown(0)){
//				PopUpUIManager.Instance.HideAlert();
//			}
//			return;
//		}
		Ray rayCast;
		RaycastHit hitInfo;
		float distance = Mathf.Infinity;
		bool down = false;
		bool moved = false;
		bool up = false;
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			rayCast = Camera.main.ScreenPointToRay (touch.position);

			switch(touch.phase){
			case TouchPhase.Began:
				down = true;
				break;

			case TouchPhase.Moved:
				moved = true;
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
			}
		}

		int layerMask = 1 << 8;//Layer 8
		if(Physics.Raycast(rayCast, out hitInfo, distance, layerMask)) {
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
					HandleTouchMapMoved (hitInfo.point);
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
		if (hitInfo.transform.gameObject.tag.Equals("Truck")) {
			EGFiretruck truck = hitInfo.transform.root.gameObject.GetComponent<EGFiretruck> ();
			if (truck != null) {
				_dispatcher.SetSelectedTruck(truck);
			}
		}
	}

	void HandleTouchTruckMoved(RaycastHit hitInfo){
		//Not sure what to do here
	}

	void HandleTouchTruckEnded(RaycastHit hitInfo){
		singleTouchDown = false;
		dragging = false;

		if (hitInfo.transform.gameObject.tag.Equals("Truck")) {
			EGFiretruck truck = hitInfo.transform.root.gameObject.GetComponent<EGFiretruck> ();
			if (truck != null) {
				_dispatcher.SetSelectedTruck(truck);
			}
		}
	}

	void HandleTouchMapStart(Vector3 touchPos){
		singleTouchStart = touchPos;
		previousTouch = touchPos;
		singleTouchDown = true;
	}

	void HandleTouchMapMoved(Vector3 touchPos){
		if (!dragging) {
			Vector3 distFromStart = singleTouchStart - touchPos;
			dragging = distFromStart.magnitude >= DRAG_THRESDHOLD;
		}

		if (dragging){
			Vector3 dragDelta = previousTouch - touchPos;
			Vector3 moveBy = new Vector3(dragDelta.x, 0, dragDelta.z);

			Camera.main.transform.Translate(moveBy, Space.World);
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
