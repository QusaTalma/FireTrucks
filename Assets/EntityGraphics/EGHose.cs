using UnityEngine;
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
		if (truck.IsActive () && !truck.IsWaitingForTraffic () && !truck.IsPuttingOutFire()) {
			if (waterStream != null) {
					Destroy (waterStream);
					waterStream = null;
					truck.SetPuttingOutFire (false);
					truck.TargetFlame = null;
			}
		} else if (!truck.IsActive () || truck.IsWaitingForTraffic ()) {
			if (foundFlames.Count > 0) {
				truck.SetPuttingOutFire (true);

				if (waterStream == null) {
					waterStream = (GameObject)Instantiate (waterStreamPrefab);
					waterStream.transform.position = transform.position;
				}

				for(int i=foundFlames.Count-1; i>=0; i--){
					if(foundFlames[i] == null){
						foundFlames.RemoveAt(i);
					}
				}

				GameObject closestFlame = null;
				float shortestDist = 100;
				int shortestIndex = 0;
				for(int i=0; i<foundFlames.Count; i++){
					GameObject flame = foundFlames[i];
					if(closestFlame == null){
						closestFlame = flame;
						shortestDist = Vector3.Distance(transform.position, closestFlame.transform.position);
						shortestIndex = i;
					}else{
						float dist = Vector3.Distance(transform.position, flame.transform.position);
						if(dist < shortestDist){
							closestFlame = flame;
							shortestDist = dist;
							shortestIndex = i;
						}
					}
				}

				if (closestFlame != null) {
					truck.TargetFlame = closestFlame;
					float dist = Vector3.Distance (transform.position, closestFlame.transform.position);
					Vector3 streamScale = waterStream.transform.localScale;
					streamScale.x = dist;
					waterStream.transform.localScale = streamScale;

					float angle = Mathf.Atan2 (closestFlame.transform.position.z - transform.position.z,
					                           closestFlame.transform.position.x - transform.position.x);
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

					Vector3 flameScale = closestFlame.transform.localScale;
					flameScale.x -= waterRate * Time.deltaTime;
					flameScale.y -= waterRate * Time.deltaTime;
					flameScale.z -= waterRate * Time.deltaTime;

					closestFlame.transform.localScale = flameScale;

					if (flameScale.x <= waterRate) {
						EGFlame egFlame = closestFlame.transform.gameObject.GetComponent<EGFlame> ();
						if (egFlame != null) {
							egFlame.PutOut ();
						}
						Destroy (closestFlame.transform.gameObject);
						foundFlames.Remove (closestFlame);
						truck.SetPuttingOutFire (false);
						truck.TargetFlame = null;
					}
				}
			} else {
				if (waterStream != null) {
					Destroy (waterStream);
					waterStream = null;
				}
				truck.SetPuttingOutFire (false);
				truck.TargetFlame = null;
			}
		} else {
			truck.SetPuttingOutFire (false);
			truck.TargetFlame = null;
		}
	}
}