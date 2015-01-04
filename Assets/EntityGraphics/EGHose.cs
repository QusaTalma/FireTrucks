﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EGHose : MonoBehaviour {
	public EGFiretruck truck;
	public GameObject waterStreamPrefab;
	public float waterRate;

	GameObject waterStream;

	List<GameObject> foundFlames;

	// Use this for initialization
	void Start () {
		foundFlames = new List<GameObject>();
	}

	public void AddFlame(GameObject flame){
		foundFlames.Add (flame);
	}

	public void RemoveFlame(GameObject flame){
		foundFlames.Remove (flame);
	}
	
	// Update is called once per frame
	void Update () {
		if (truck.IsActive () && !truck.IsWaitingForTraffic ()) {
			if (waterStream != null) {
					Destroy (waterStream);
					waterStream = null;
					truck.SetPuttingOutFire (false);
			}
		} else if (!truck.IsActive () || truck.IsWaitingForTraffic ()) {
			if (foundFlames.Count > 0) {
				truck.SetPuttingOutFire (true);

				if (waterStream == null) {
					waterStream = (GameObject)Instantiate (waterStreamPrefab);
					waterStream.transform.position = transform.position;
				}

				GameObject flame = foundFlames [0];
				while (flame == null && foundFlames.Count > 0) {
					foundFlames.RemoveAt (0);
					if (foundFlames.Count > 0) {
						flame = foundFlames [0];
					} else {
						break;
					}
				}

				if (flame != null) {
					float dist = Vector3.Distance (transform.position, flame.transform.position);
					Vector3 streamScale = waterStream.transform.localScale;
					streamScale.x = dist;
					waterStream.transform.localScale = streamScale;

					float angle = Mathf.Atan2 (flame.transform.position.z - transform.position.z,
                          flame.transform.position.x - transform.position.x);
					angle = angle * Mathf.Rad2Deg;

					//Convert to local coordinate system
					angle = 90 - angle;

					float localizedCurrentAngle = waterStream.transform.eulerAngles.y;
					angle = angle - localizedCurrentAngle;
					angle = (angle + 360) % 360;

					if (angle > 180) {
						angle = angle - 360;
					}

					angle = angle - 90;

					waterStream.transform.Rotate (0, angle, 0);

					Vector3 flameScale = flame.transform.localScale;
					flameScale.x -= waterRate * Time.deltaTime;
					flameScale.y -= waterRate * Time.deltaTime;
					flameScale.z -= waterRate * Time.deltaTime;

					flame.transform.localScale = flameScale;

					if (flameScale.x <= waterRate) {
						EGFlame egFlame = flame.transform.gameObject.GetComponent<EGFlame> ();
						if (egFlame != null) {
							egFlame.PutOut ();
						}
						Destroy (flame.transform.gameObject);
						foundFlames.Remove (flame);
						truck.SetPuttingOutFire (false);
					}
				}
			} else {
				if (waterStream != null) {
					Destroy (waterStream);
					waterStream = null;
				}
				truck.SetPuttingOutFire (false);
			}
		} else {
			truck.SetPuttingOutFire (false);
		}
	}
}