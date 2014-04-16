using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TGMap))]
public class TileMapInspector : Editor {

	float v = .5f;

	public override void OnInspectorGUI(){
		//base.OnInspectorGUI ();
		DrawDefaultInspector ();

		EditorGUILayout.BeginVertical ();
		v = EditorGUILayout.Slider (v, 0, 2f);
		EditorGUILayout.EndVertical ();

		if (GUILayout.Button ("Regenerate")) {
			TGMap tileMap = (TGMap)target;
			tileMap.BuildMesh();
		}
	}
}