using UnityEngine;
using System.Collections.Generic;
using System.IO;

/*****
 * 
 * This class handles GameObject interactions related to the map:
 * Creates the texture for the map and displays it.
 * Responds to touches on the map
 *
 *****/

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TGMap : MonoBehaviour {
	public float tileSize;

	public Texture2D terrainTiles;
	public int tileResolution;

	public GameObject firehousePrefab;
	public GameObject arsonistPrefab;

	public TDMap Map{
		get { return _level.Map; }
	}

	public float TimeInSecondsToPlay{
		get { return _level.TimeInSecondsToPlay; }
	}

	public float PercentToWin{
		get { return _level.PercentRemainingToWin; }
	}

	TDGameSession _gameSession;
	TGArsonist arsonist;

	TDLevel _level;

	private bool paused = false;

	void Start () {
		string levelFileName = LevelManager.Instance.GetCurrentLevelFilePath ();
		string levelText = "";

		TextAsset levelFile = Resources.Load(levelFileName) as TextAsset;
		if(levelFile != null){
			levelText = levelFile.text;
		}

		_level = new TDLevel(levelText);
		_gameSession = new TDGameSession(_level.TimeInSecondsToPlay);

		//Create the map
		BuildMesh ();
		BuildTexture ();
		//Initiate game state
		PlaceFireHouse ();
		PlaceArsonist ();

		Time.timeScale = 1f;
	}

	void Update(){
		_gameSession.AddToCurrentTime (Time.deltaTime);
		if (!_gameSession.IsActive()) {
			Time.timeScale = 0f;
			LevelGUIManager levelManager = gameObject.GetComponent<LevelGUIManager>();
			if(levelManager != null){
				levelManager.ShowEndGameDialog();
			}
		}
	}

	public void TogglePause(){
		if (!paused) {
			paused = true;
			Time.timeScale = 0f;
		} else {
			paused = false;
			Time.timeScale = 1f;
		}
	}

	public bool IsPaused(){
		return paused;
	}

	//Loads the texture for each tile from the sprite strip into 
	//individual tile color data
	Color[][] ChopUpTiles(){
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;

		Color[][] tiles = new Color[numTilesPerRow * numRows][];

		for (int y=0; y<numRows; y++) {
			for (int x=0; x<numTilesPerRow; x++) {
				tiles[y*numTilesPerRow+x] = terrainTiles.GetPixels(x*tileResolution, 
				                                                   y*tileResolution, 
				                                                   tileResolution, 
				                                                   tileResolution);
			}
		}

		return tiles;
	}

	//Places the firehouse GameObject on the map
	//TODO this should be determined by the level, once levels are a thing
	void PlaceFireHouse(){
		Vector2 fireHouseTilePos = new Vector2 ();
		Map.GetFireHouseCoordinates (out fireHouseTilePos);

		Vector3 housePos = GetPositionForTile (Mathf.FloorToInt(fireHouseTilePos.x),
		                                       Mathf.FloorToInt(fireHouseTilePos.y));

		housePos.x += 0.5f;
		housePos.z -= 0.5f;

		GameObject house = (GameObject)Instantiate (firehousePrefab);
		house.transform.position = housePos;

		EGFirehouse firehouse = house.GetComponent<EGFirehouse>();
		firehouse.SetTGMap(this);
		EGDispatcher dispatcher = GetComponent<EGDispatcher>();
		dispatcher.SetFirehouse(firehouse);
	}

	//Places the arsonist, who will wander the map starting fires
	void PlaceArsonist(){
		GameObject arsonistObject = (GameObject)Instantiate (arsonistPrefab);
		arsonist = arsonistObject.GetComponent<TGArsonist> ();
		arsonist.SetMap (this);
		arsonist.ArsonPath = _level.ArsonPath;
	}

	//Creates the texture for the map
	public void BuildTexture(){
		//Create a texture of adequate size
		int texWidth = Map.Width * tileResolution;
		int textHeight = Map.Height * tileResolution;
		Texture2D texture = new Texture2D(texWidth, textHeight);
		//Get the tile color info
		Color[][] tiles = ChopUpTiles ();

		//Loop over the map, stitching tiles of the appropriate
		//type together into the texture
		for(int y=0; y<Map.Height; y++){
			for(int x=0; x < Map.Width; x++) {
				Color[] p = tiles[Map.GetTile(x,y).type];
				texture.SetPixels(x*tileResolution, y*tileResolution,
				                  tileResolution, tileResolution, p);
			}
		}
		//Finalize the texture
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply ();

		//Apply the texture to the renderer
		MeshRenderer mesh_rendererer = GetComponent<MeshRenderer> ();
		mesh_rendererer.sharedMaterials [0].mainTexture = texture;
	}

	public TDTile GetTileAt(int x, int z){
		return Map.GetTile (x, z);
	}

	public void BuildMesh()
	{
		int numTiles = Map.Width * Map.Height;
		int numTris = numTiles * 2;
		int vsize_x = Map.Width + 1;
		int vsize_z = Map.Height + 1;
		int numVerts = vsize_x * vsize_z;

		//Generate mesh data
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uvs = new Vector2[numVerts];

		int[] triangles = new int[numTris * 3 /*3 per triangle*/];

		int x, z;
		//Create the vertices, normals, and uvs for each point in the mesh
		for (z = 0; z < vsize_z; z++) {
			for(x = 0; x < vsize_x; x++){
				vertices[z * vsize_x + x] = GetPositionForTile(x, z);
				normals[z * vsize_x + x] = Vector3.up;
				uvs[z * vsize_x + x] = new Vector2((float)x/(float)Map.Width,
				                                   (float)z/(float)Map.Height);
			}
		}

		//Define the triangles for the mesh
		for (z = 0; z < Map.Height; z++) {
			for(x = 0; x < Map.Width; x++){
				int squareIndex = z * Map.Width + x;
				int triIndex = squareIndex * 6;
				triangles[triIndex+0] = z * vsize_x + x + 0;
				triangles[triIndex+2] = z * vsize_x + x + vsize_x + 0;
				triangles[triIndex+1] = z * vsize_x + x + vsize_x + 1;

				triangles[triIndex+3] = z * vsize_x + x + 0;
				triangles[triIndex+5] = z * vsize_x + x + vsize_x + 1;
				triangles[triIndex+4] = z * vsize_x + x +  1;
			}
		}

		//Create a new mesh
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uvs;

		//Assign mesh to filter and collider
		MeshFilter mesh_filterer = GetComponent<MeshFilter> ();
		MeshCollider mesh_colliderer = GetComponent<MeshCollider> ();

		mesh_filterer.mesh = mesh;
		mesh_colliderer.sharedMesh = mesh;
	}


	//Returns the game world position for the upper left corner
	//of the given tile
	public Vector3 GetPositionForTile(int x, int z){
		Vector3 pos = new Vector3 ();
		pos.x = tileSize * x;
		pos.y = 0;
		pos.z = tileSize * -z;

		return pos;
	}

	//Returns the tile for the given world position
	public TDTile GetTileForWorldPosition(Vector3 position){
		int x, y;

		x = Mathf.FloorToInt (position.x/tileSize);
		y = Mathf.FloorToInt (-position.z / tileSize);

		return Map.GetTile(x, y);
	}

	public TDGameSession GetGameSession(){
		return this._gameSession;
	}

	public float GetCityDurabilityPercent(){
		return Map.GetCurrentDurability () / Map.GetTotalDurability ();
	}
}