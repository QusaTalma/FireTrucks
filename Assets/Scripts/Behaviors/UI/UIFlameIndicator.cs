﻿using UnityEngine;
using System.Collections;
using Behaviors.EntityGraphics;
using Utils;
using Managers;

namespace Behaviors.UI{
	public class UIFlameIndicator : MonoBehaviour {
		private EGFlame flame;

		public GameObject indicator;

		public EGFlame Flame {
			get{ return flame; }
			set{ this.flame = value; }
		}

		void Update () {
			if (flame != null) {
				if(flame.IsOnCamera() || !PopUpUIManager.Instance.RadioOn){
					Destroy(this.transform.root.gameObject);
				}else{
					PointToFlame();
				}
			}else{
				Destroy(this.transform.root.gameObject);
			}
		}

		private void PointToFlame(){
			Vector3 camPos = Camera.main.transform.position;
			Rect cameraRect = GetCameraRect (camPos);
			float angleToFlame = FindAngleToFlame (camPos);

			Vector2 intersection = MathUtils.IntersectionWithRayFromCenter (cameraRect, new Vector2 (flame.transform.position.x, flame.transform.position.z));

			Vector3 pos = new Vector3 (intersection.x, transform.position.y, intersection.y);
			transform.position = pos;

			Vector3 eulerAngles = new Vector3 (0, angleToFlame, 0);
			indicator.transform.eulerAngles = eulerAngles;
		}

		private Rect GetCameraRect(Vector3 camPos){
			//The orthographic size if half the height of the camera
			float vertExtent = Camera.main.orthographicSize - transform.localScale.z*5;
			//Calculate the half height of the screen
			float horizExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
			horizExtent = horizExtent - transform.localScale.x*6;
			
			return new Rect (camPos.x - horizExtent, camPos.z - vertExtent, horizExtent * 2, vertExtent * 2);
		}

		private float FindAngleToFlame(Vector3 camPos){
			Vector3 flamePos = flame.transform.position;
			return Mathf.Rad2Deg * Mathf.Atan2 (-(flamePos.z - camPos.z), flamePos.x - camPos.x);
		}
	}
}