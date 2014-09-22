using UnityEngine;
using System.Collections.Generic;

/*****
 * 
 * This class handles GameObject interactions related to the map:
 * Creates the texture for the map and displays it.
 * Responds to touches on the map
 * 
 * Manages firetrucks by organizing them by idle and non-idle
 * //As I write this comment, admiteddly too late, I realize this class
 * //Shouldn't be managing firetrucks. 
 * //TODO Create a Dispatcher class
 * //that can take signals from the map about where to send firetrucks,
 * //and then the dispatcher class organizes the trucks
 *
 *****/

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
	EGFiretruck selectedTruck = null;

	void Start () {
		//Create the map
		BuildMesh ();
		BuildTexture ();
		//Initiate game state
		PlaceFireHouse ();
		PlaceArsonist ();
		
		_activeTrucks = new List<EGFiretruck> ();
		_truckSpawnQueue = new List<Vector2> ();
		_idleTrucks = new List<EGFiretruck> ();
	}

	void Update(){
		//TODO everything in this method is out of scope for this class,
		//create a Dispatcher class to manage all this, also manage it smarter

		//Sort through trucks so that they are kept in the right sets, active and idle
		List<EGFiretruck> toMoveToIdle = new List<EGFiretruck> ();
		for (int i=0; i<_activeTrucks.Count; i++) {
			EGFiretruck truck = _activeTrucks[i];

			if(!truck.IsActive() && !truck.IsPuttingOutFire()){
				//Don't remove them while iterating, remove afterwards
				toMoveToIdle.Add(truck);
			}
		}

		//Set the trucks that need setting
		for (int i=0; i<toMoveToIdle.Count; i++) {
			EGFiretruck truck = toMoveToIdle[i];
			SetTruckIdle(truck);
		}

		//Identify trucks that need to be removes
		List<EGFiretruck> toDestroy = new List<EGFiretruck> ();

		for (int i=0; i<_idleTrucks.Count; i++) {
			EGFiretruck truck = _idleTrucks[i];

			TDTile truckTile = GetTileForWorldPosition(truck.transform.position);
			//Trucks that are idle at the firehouse need to be removed
			if(truckTile.type == TDTile.TILE_FIREHOUSE){
				toDestroy.Add(truck);
			}
		}

		//Remove trucks that need to be removed
		for (int i=0; i<toDestroy.Count; i++) {
			EGFiretruck truck = toDestroy[i];
			_idleTrucks.Remove(truck);
			Destroy(truck.gameObject);
		}

		//Spawn truck is ready for it
		if (!firehouse.ContainsTruck ()) {
			SpawnNextTruck ();
		}
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
		_map.GetFireHouseCoordinates (out fireHouseTilePos);

		Vector3 housePos = GetPositionForTile (Mathf.FloorToInt(fireHouseTilePos.x),
		                                       Mathf.FloorToInt(fireHouseTilePos.y));

		housePos.x += 0.5f;
		housePos.z -= 0.5f;

		GameObject house = (GameObject)Instantiate (firehousePrefab);
		firehouse = house.GetComponent<EGFirehouse> ();
		firehouse.transform.position = housePos;
	}

	//Places the arsonist, who will wander the map starting fires
	void PlaceArsonist(){
		GameObject arsonistObject = (GameObject)Instantiate (arsonistPrefab);
		arsonist = arsonistObject.GetComponent<TGArsonist> ();
		arsonist.SetMap (this);

		int arsonistX = Random.Range (0, _map.GetWidth ());
		int arsonistY = Random.Range (0, _map.GetHeight ());
		arsonist.SetPosition (arsonistX, arsonistY);
	}

	//Creates the texture for the map
	public void BuildTexture(){
		//Initialize the map data
		_map = new TDMap (size_x, size_z);

		//Create a texture of adequate size
		int texWidth = size_x * tileResolution;
		int textHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, textHeight);
		//Get the tile color info
		Color[][] tiles = ChopUpTiles ();

		//Loop over the map, stitching tiles of the appropriate
		//type together into the texture
		for(int y=0; y<size_z; y++){
			for(int x=0; x < size_x; x++) {
				Color[] p = tiles[_map.GetTile(x,y).type];
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
		//Create the vertices, normals, and uvs for each point in the mesh
		for (z = 0; z < vsize_z; z++) {
			for(x = 0; x < vsize_x; x++){
				vertices[z * vsize_x + x] = GetPositionForTile(x, z);
				normals[z * vsize_x + x] = Vector3.up;
				uvs[z * vsize_x + x] = new Vector2((float)x/(float)size_x,
				                                   (float)z/(float)size_z);
			}
		}

		//Define the triangles for the mesh
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

		//Assign mesh to filter and collider
		MeshFilter mesh_filterer = GetComponent<MeshFilter> ();
		MeshCollider mesh_colliderer = GetComponent<MeshCollider> ();

		mesh_filterer.mesh = mesh;
		mesh_colliderer.sharedMesh = mesh;
	}

	//Creates a firetruck at the firehouse with
	//the tile at the given point as destination
	//TODO Dispatcher class
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

		return _map.GetTile(x, y);
	}

	//TODO Dispatcher
	public void AddPositionToSpawnQueue(Vector2 truckPosition){
		_truckSpawnQueue.Add (truckPosition);
	}
	
	//TODO Dispatcher
	public void SetSelectedTruck(EGFiretruck selected){
		selectedTruck = selected;
	}
	
	//TODO Dispatcher
	public void SendIdleToPosition(int x, int z){
		EGFiretruck truckToSend = null;

		if (selectedTruck != null) {
			truckToSend = selectedTruck;
			if(_idleTrucks.Contains(selectedTruck)){
				_idleTrucks.Remove(selectedTruck);
			}else if(_activeTrucks.Contains(selectedTruck)){
				_activeTrucks.Remove(selectedTruck);
			}
			Debug.Log("Using selected truck");
			selectedTruck = null;
		} else if (_idleTrucks.Count > 0) {
			truckToSend = PopIdleTruck ();
		}

		if(truckToSend != null){
			TDPath truckPath = new TDPath ();
			truckPath.BuildPath (_map,
			                     _map.GetTile(Mathf.FloorToInt(truckToSend.GetPosition().x), Mathf.FloorToInt(-truckToSend.GetPosition().z)),
			                     _map.GetTile (x, -z));

			Debug.Log("Destination: " + x + ", " + -z);
			Debug.Log("Truck pos: " + truckToSend.GetPosition());
			Debug.Log("Setting path: " + truckPath.ToString());
			truckToSend.SetPath(truckPath);
			truckToSend.SetIdle(false);

			_activeTrucks.Add(truckToSend);
		}
	}
	
	//TODO Dispatcher
	public void SpawnNextTruck(){
		if (_truckSpawnQueue.Count > 0) {
			Vector2 truckPos = _truckSpawnQueue[0];
			_truckSpawnQueue.Remove(truckPos);
			SpawnFireTruck((int)truckPos.x, (int)truckPos.y);
		}
	}
	
	//TODO Dispatcher
	public EGFiretruck PopIdleTruck(){
		EGFiretruck poppedTruck = null;
		if (_idleTrucks.Count > 0) {
			poppedTruck = _idleTrucks[0];
			_idleTrucks.Remove(poppedTruck);
		}
		
		return poppedTruck;
	}
	
	//TODO Dispatcher
	public void SetTruckIdle(EGFiretruck truck){
		if(truck != null){
			_activeTrucks.Remove(truck);
			_idleTrucks.Add(truck);

			if(truck.returnWhenIdle){
				Vector2 fireHouseTilePos = new Vector2 ();
				_map.GetFireHouseCoordinates (out fireHouseTilePos);
				
				TDTile start = GetTileForWorldPosition(truck.transform.position);
				TDTile end = _map.GetTile(Mathf.FloorToInt(fireHouseTilePos.x), Mathf.FloorToInt(fireHouseTilePos.y));
				TDPath pathToFirehouse = new TDPath();
				pathToFirehouse.BuildPath (_map,start,end);
				
				truck.SetPath(pathToFirehouse);
			}

			truck.SetIdle(true);
		}
	}

	//Returns the underlying map data
	public TDMap GetDataMap(){
		return this._map;
	}
	
	//TODO Dispatcher
	public int GetTruckCount(){
		return _idleTrucks.Count + _activeTrucks.Count;
	}
}