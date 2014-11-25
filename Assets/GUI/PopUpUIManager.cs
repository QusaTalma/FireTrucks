using UnityEngine;
using System.Collections;

public class PopUpUIManager : MonoBehaviour {
	public GameObject fireChiefImage;
	public GameObject policeChiefImage;
	public GameObject mayorImage;

	void Start(){
		fireChiefImage.SetActive (false);
		policeChiefImage.SetActive (false);
		mayorImage.SetActive (false);
	}

	void OnGUI(){

	}
}
