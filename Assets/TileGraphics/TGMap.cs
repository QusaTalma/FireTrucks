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

	public GameObject firehousePrefab;
	public GameObject arsonistPrefab;

	TDMap _map;
	TGArsonist arsonist;


	void Start () {
		//Create the map
		BuildMesh ();
		BuildTexture ();
		//Initiate game state
		PlaceFireHouse ();
		PlaceArsonist ();
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

	//Returns the underlying map data
	public TDMap GetDataMap(){
		return this._map;
	}

	public float GetCityDurabilityPercent(){
		return _map.GetCurrentDurability () / _map.GetTotalDurability ();
	}
}