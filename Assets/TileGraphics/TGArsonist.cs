using UnityEngine;
using System.Collections.Generic;

public class TGArsonist : MonoBehaviour
{
	private TGMap _map;
	private float elapsedTime = 0f;

	public GameObject flamePrefab;

	public bool captureShown = false;

	private EDArsonPath arsonPath;
	public EDArsonPath ArsonPath{
		set { arsonPath = value;
			arsonStepCount = value.GetStepCount(); }
	}

	private int arsonStepCount;
	public int ArsonStepCount {
		get { return arsonPath.GetStepCount (); }
	}

	public void Start(){
		PopUpUIManager.Instance.ShowFiresStartingMessage ();
	}

	public void Update(){
		elapsedTime += Time.deltaTime;

		if (arsonPath.HasMoreSteps () && arsonPath.TimeForNextStep (elapsedTime)) {
			TDTile tileToLight = arsonPath.PopStep ();
			StartTileOnFire (tileToLight);
			if(arsonPath.GetStepCount() == (int)(arsonStepCount/2)){
				PopUpUIManager.Instance.ShowFiresHalfFinishedMessage();
			}else if(arsonPath.GetStepCount() == (int)(arsonStepCount/4)){
				PopUpUIManager.Instance.ShowFiresAlmostFinishedMessage();
			}
		} else if (!arsonPath.HasMoreSteps () && !captureShown) {
			captureShown = true;
			PopUpUIManager.Instance.ShowFiresFinishedMessage();
		}
	}

	public void SetMap(TGMap map){
		this._map = map;
	}

	void StartTileOnFire(TDTile tile){
		GameObject flame = (GameObject)Instantiate (flamePrefab);
		
		Vector3 flamePos = _map.GetPositionForTile (tile.GetX(), tile.GetY());
		flamePos.x += 0.5f;
		flamePos.z -= 0.5f;
		
		flame.transform.position = flamePos;
		
		EGFlame egFlame = flame.GetComponent<EGFlame>();
		egFlame.SetTile(tile);
		egFlame.SetMap(_map);
		egFlame.SetSpreadPrefab(flamePrefab);
		
		tile.OnFire = true;
	}
}