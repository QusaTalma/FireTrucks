using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TGMap : MonoBehaviour {
	public int size_x;
	public int size_z;
	public float tileSize;

	public Texture2D terrainTiles;
	public int tileResolution;

	public GameObject firetruckPrefab;
	public GameObject firehousePrefab;
	public GameObject arsonistPrefab;

	TDMap _map;
	TGArsonist arsonist;

	//Requested locations for new trucks to go to
	List<Vector2> _truckSpawnQueue;

	//Actual trucks that exist
	List<EGFiretruck> _activeTrucks;
	List<EGFiretruck> _idleTrucks;

	EGFirehouse firehouse;

	// Use this for initialization
	void Start () {
		BuildMesh ();
		BuildTexture ();
		PlaceFireHouse ();
		PlaceArsonist ();
		
		_activeTrucks = new List<EGFiretruck> ();
		_truckSpawnQueue = new List<Vector2> ();
		_idleTrucks = new List<EGFiretruck> ();
	}

	void Update(){
		List<EGFiretruck> toMoveToIdle = new List<EGFiretruck> ();
		for (int i=0; i<_activeTrucks.Count; i++) {
			EGFiretruck truck = _activeTrucks[i];

			if(!truck.IsActive() && !truck.IsPuttingOutFire()){
				toMoveToIdle.Add(truck);
			}
		}

		for (int i=0; i<toMoveToIdle.Count; i++) {
			EGFiretruck truck = toMoveToIdle[i];
			SetTruckIdle(truck);
		}

		List<EGFiretruck> toDestroy = new List<EGFiretruck> ();

		for (int i=0; i<_idleTrucks.Count; i++) {
			EGFiretruck truck = _idleTrucks[i];

			TDTile truckTile = GetTileForWorldPosition(truck.transform.position);
			if(truckTile.type == TDTile.TILE_FIREHOUSE){
				toDestroy.Add(truck);
			}
		}
		
		for (int i=0; i<toDestroy.Count; i++) {
			EGFiretruck truck = toDestroy[i];
			_idleTrucks.Remove(truck);
			Destroy(truck.gameObject);
		}

		if (!firehouse.ContainsTruck ()) {
			SpawnNextTruck ();
		}
	}

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

	void PlaceFireHouse(){
		Vector2 fireHouseTilePos = new Vector2 ();
		_map.GetFireHouseCoordinates (out fireHouseTilePos);

		Vector3 housePos = GetPositionForTile (Mathf.FloorToInt(fireHouseTilePos.x),
		                                       Mathf.FloorToInt(fireHouseTilePos.y));

		housePos.x += 0.5f;
		housePos.z -= 0.5f;

		GameObject house = (GameObject)Instantiate (firehousePrefab);
		firehouse = house.GetComponent<EGFirehouse> ();
		firehouse.transform.position = housePos;
	}

	void PlaceArsonist(){
		GameObject arsonistObject = (GameObject)Instantiate (arsonistPrefab);
		arsonist = arsonistObject.GetComponent<TGArsonist> ();
		arsonist.SetMap (this);

		int arsonistX = Random.Range (0, _map.GetWidth ());
		int arsonistY = Random.Range (0, _map.GetHeight ());
		arsonist.SetPosition (arsonistX, arsonistY);
	}

	public void BuildTexture(){
		_map = new TDMap (size_x, size_z);

		int texWidth = size_x * tileResolution;
		int textHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, textHeight);

		Color[][] tiles = ChopUpTiles ();

		for(int y=0; y<size_z; y++){
			for(int x=0; x < size_x; x++) {
				Color[] p = tiles[_map.GetTile(x,y).type];
				texture.SetPixels(x*tileResolution, y*tileResolution,
				                  tileResolution, tileResolution, p);
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply ();
		
		MeshRenderer mesh_rendererer = GetComponent<MeshRenderer> ();
		mesh_rendererer.sharedMaterials [0].mainTexture = texture;
	}

	public TDTile GetTileAt(int x, int z){
		return _map.GetTile (x, z);
	}

	public void BuildMesh()
	{
		int numTiles = size_x * size_z;
		int numTris = numTiles * 2;
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_x * vsize_z;

		//Generate mesh data
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uvs = new Vector2[numVerts];

		int[] triangles = new int[numTris * 3 /*3 per triangle*/];

		int x, z;
		for (z = 0; z < vsize_z; z++) {
			for(x = 0; x < vsize_x; x++){
				vertices[z * vsize_x + x] = GetPositionForTile(x, z);
				normals[z * vsize_x + x] = Vector3.up;
				uvs[z * vsize_x + x] = new Vector2((float)x/(float)size_x,
				                                   (float)z/(float)size_z);
			}
		}

		for (z = 0; z < size_z; z++) {
			for(x = 0; x < size_x; x++){
				int squareIndex = z * size_x + x;
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

		//Assign mesh to filter,renderer,collider
		MeshFilter mesh_filterer = GetComponent<MeshFilter> ();
		MeshCollider mesh_colliderer = GetComponent<MeshCollider> ();

		mesh_filterer.mesh = mesh;

		mesh_colliderer.sharedMesh = mesh;
	}

	public void SpawnFireTruck(int x, int z){
		Vector2 fireHouseTilePos;
		_map.GetFireHouseCoordinates (out fireHouseTilePos);

		Vector3 truckPos = GetPositionForTile (Mathf.FloorToInt(fireHouseTilePos.x),
		                                       Mathf.FloorToInt(fireHouseTilePos.y));
		truckPos.x += .5f;
		truckPos.z -= .5f;

		firehouse.AddTruck ();

		GameObject truck = (GameObject)Instantiate (firetruckPrefab);
		truck.transform.position = truckPos;

		EGFiretruck firetruck = truck.GetComponent<EGFiretruck>();
		firetruck.SetPosition (truckPos);

		TDPath truckPath = new TDPath ();
		firetruck.SetMap (this);
		truckPath.BuildPath (_map,
		                     _map.GetTile(Mathf.FloorToInt(fireHouseTilePos.x), Mathf.FloorToInt(fireHouseTilePos.y)),
		                     _map.GetTile (x, -z));
		
		firetruck.SetPath (truckPath);
		_activeTrucks.Add (firetruck);
	}

	public Vector3 GetPositionForTile(int x, int z){
		Vector3 pos = new Vector3 ();
		pos.x = tileSize * x;
		pos.y = 0;
		pos.z = tileSize * -z;

		return pos;
	}

	public TDTile GetTileForWorldPosition(Vector3 position){
		int x, y;

		x = Mathf.FloorToInt (position.x/tileSize);
		y = Mathf.FloorToInt (-position.z / tileSize);

		return _map.GetTile(x, y);
	}
	
	public void AddPositionToSpawnQueue(Vector2 truckPosition){
		_truckSpawnQueue.Add (truckPosition);
	}

	public void SendIdleToPosition(int x, int z){
		if (_idleTrucks.Count > 0) {
			EGFiretruck truck = PopIdleTruck();

			TDPath truckPath = new TDPath ();
			truckPath.BuildPath (_map,
			                     _map.GetTile(Mathf.FloorToInt(truck.GetPosition().x), Mathf.FloorToInt(-truck.GetPosition().z)),
			                     _map.GetTile (x, -z));

			truck.SetPath(truckPath);
			truck.SetIdle(false);

			_activeTrucks.Add(truck);
		}
	}
	
	public void SpawnNextTruck(){
		if (_truckSpawnQueue.Count > 0) {
			Vector2 truckPos = _truckSpawnQueue[0];
			_truckSpawnQueue.Remove(truckPos);
			SpawnFireTruck((int)truckPos.x, (int)truckPos.y);
		}
	}
	
	public EGFiretruck PopIdleTruck(){
		EGFiretruck poppedTruck = null;
		if (_idleTrucks.Count > 0) {
			poppedTruck = _idleTrucks[0];
			_idleTrucks.Remove(poppedTruck);
		}
		
		return poppedTruck;
	}
	
	public void SetTruckIdle(EGFiretruck truck){
		if(truck != null){
			_activeTrucks.Remove(truck);
			_idleTrucks.Add(truck);
			
			Vector2 fireHouseTilePos = new Vector2 ();
			_map.GetFireHouseCoordinates (out fireHouseTilePos);

			TDTile start = GetTileForWorldPosition(truck.transform.position);
			TDTile end = _map.GetTile(Mathf.FloorToInt(fireHouseTilePos.x), Mathf.FloorToInt(fireHouseTilePos.y));
			TDPath pathToFirehouse = new TDPath();
			pathToFirehouse.BuildPath (_map,start,end);

			truck.SetPath(pathToFirehouse);

			truck.SetIdle(true);
		}
	}

	public TDMap GetDataMap(){
		return this._map;
	}
}