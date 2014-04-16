using UnityEngine;
using System.Collections;

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

	TDMap _map;

	// Use this for initialization
	void Start () {
		BuildMesh ();
		BuildTexture ();
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

		Debug.Log ("Texture!");
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
		MeshRenderer mesh_rendererer = GetComponent<MeshRenderer> ();
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

		GameObject truck = (GameObject)Instantiate (firetruckPrefab);
		truck.transform.position = truckPos;
	}

	Vector3 GetPositionForTile(int x, int z){
		Vector3 pos = new Vector3 ();
		pos.x = tileSize * x;
		pos.y = 0;
		pos.z = tileSize * -z;

		return pos;
	}
}