using System.Collections.Generic;
using UnityEngine;

public class TDLevel {
	/*****
	 *Level text file format
	 *<int width>,<int height>
	 *<width x height grid of characters, each representing a tile>
	 *<int % value of city needed to win>
	 *<int seconds of play time>
	 *<any number of lines representing arsonist steps, format on following line>
	 *<int fireX, int fireY, float timeToPlace> //Note, these MUST be in chronological order
	 *****/

	private int mapWidth = 26;
	private int mapHeight = 13;
	private float totalDurability = 0;

	private TDTile[,] tiles;
	private TDMap _map;
	public TDMap Map{
		get {return _map; }
	}

	private EDArsonPath arsonPath;
	public EDArsonPath ArsonPath{
		get { return arsonPath; }
	}

	private float percentRemainingToWin;
	public float PercentRemainingToWin{
		get { return percentRemainingToWin; }
	}

	private float timeInSecondsToPlay;
	public float TimeInSecondsToPlay{
		get { return timeInSecondsToPlay; }
	}

	private Vector2 firehouseLocation;
	public Vector2 FirehouseLocation{
		get { return firehouseLocation; }
	}

	public TDLevel(string levelData){
		string[] splitLevelData = levelData.Split ('\n');
		int offset = ReadInMap (splitLevelData);
		offset = ReadLevelParams (splitLevelData, offset);
		ReadArsonPath (splitLevelData, offset);

		_map = new TDMap(tiles, firehouseLocation, totalDurability);
	}

	int ReadInMap(string[] splitLevelData){
		string mapSize = splitLevelData [0];
		string[] mapSizes = mapSize.Split (',');
		mapWidth = int.Parse (mapSizes [0]);
		mapHeight = int.Parse (mapSizes [1]);
		int offset = 1;//Read one line for width and height


		tiles = new TDTile[mapWidth, mapHeight];
		for (int y=offset; y<mapHeight+offset; y++) {
			string mapRowData = splitLevelData[y];
			for(int x=0; x<mapWidth; x++){
				TDTile tile = new TDTile(x,y-offset);
				tile.type = int.Parse(mapRowData[x].ToString());
				if(tile.type == TDTile.TILE_FIREHOUSE){
					firehouseLocation = new Vector2(x,y-offset);
				}else if(tile.type == TDTile.TILE_HOUSE){
					totalDurability += tile.durability;
				}

				tiles[x,y-offset] = tile;
			}
		}

		return mapHeight + offset;
	}

	int ReadLevelParams(string[] splitLevelData, int offset){
		percentRemainingToWin = float.Parse(splitLevelData[offset])/100f;
		offset++;
		timeInSecondsToPlay = float.Parse (splitLevelData [offset]);
		offset++;
		return offset;
	}

	void ReadArsonPath(string[] splitLevelData, int offset){
		List<TDTile> pathSteps = new List<TDTile> ();
		List<float> pathTimes = new List<float> ();
		for(int i=offset; i<splitLevelData.Length; i++){
			string[] stepData = splitLevelData[i].Split(',');
			Debug.Log(splitLevelData[i]);
			int x = int.Parse(stepData[0]);
			int y = int.Parse(stepData[1]);
			pathSteps.Add(tiles[x,y]);

			float time = float.Parse(stepData[2]);
			pathTimes.Add(time);
		}

		arsonPath = new EDArsonPath (pathSteps, pathTimes);
	}

	TDTile[,] GenerateTilesForLevel(){
		TDTile[,] tiles = new TDTile[mapWidth, mapHeight];
		for (int x = 0; x< mapWidth; x++){
			for( int y = 0; y < mapHeight; y++){
				tiles[x,y] = new TDTile(x, y);
				if(x == 0 ||
				   x == mapWidth -1 ||
				   x%3 == 0 ||
				   y == 0 ||
				   y == mapHeight - 1 ||
				   y%4 == 0){
					tiles[x,y].type = TDTile.TILE_STREET;
				}
			}
		}

		PlaceHousesOnStreets (tiles);

		return tiles;
	}
	
	void PlaceHousesOnStreets(TDTile[,] tiles){
		for (int x = 0; x< mapWidth; x++){
			for( int y = 0; y < mapHeight; y++){
				if(tiles[x,y].type == TDTile.TILE_STREET){
					PlaceHousesOnStreet(tiles, x, y);
				}
			}
		}
	}

	void PlaceHousesOnStreet(TDTile[,] tiles, int x, int y){
		if (x > 0 && tiles[x - 1, y].type == TDTile.TILE_CITY_FILL) {
			tiles[x-1,y].type = TDTile.TILE_HOUSE;
			totalDurability += tiles[x-1,y].GetDurability();
		}
		
		if (x < mapWidth - 1 && tiles [x + 1, y].type == TDTile.TILE_CITY_FILL) {
			tiles[x+1,y].type = TDTile.TILE_HOUSE;
			totalDurability += tiles[x+1,y].GetDurability();
		}
		
		if (y > 0 && tiles[x, y-1].type == TDTile.TILE_CITY_FILL) {
			tiles[x,y-1].type = TDTile.TILE_HOUSE;
			totalDurability += tiles[x,y-1].GetDurability();
		}
		
		if (y < mapHeight - 1 && tiles [x, y+1].type == TDTile.TILE_CITY_FILL) {
			tiles[x,y+1].type = TDTile.TILE_HOUSE;
			totalDurability += tiles[x,y+1].GetDurability();
		}
	}

	void CreateArsonistPath(){
		List<TDTile> pathSteps = new List<TDTile> ();
		List<float> pathTimes = new List<float>();

		//1,1
		pathSteps.Add(_map.GetTile(1,1));
		pathTimes.Add (1f);
		//4,1
		pathSteps.Add(_map.GetTile(4,1));
		pathTimes.Add (2f);
		//7,1
		pathSteps.Add(_map.GetTile(7,1));
		pathTimes.Add (3f);
		//10,1
		pathSteps.Add(_map.GetTile(10,1));
		pathTimes.Add (4f);
		//13,1
		pathSteps.Add(_map.GetTile(13,1));
		pathTimes.Add (5f);
		//16,1
		pathSteps.Add(_map.GetTile(16,1));
		pathTimes.Add (6f);
		//19,1
		pathSteps.Add(_map.GetTile(19,1));
		pathTimes.Add (7f);
		//22,1
		pathSteps.Add(_map.GetTile(22,1));
		pathTimes.Add (8f);

		
		//1,5
		pathSteps.Add(_map.GetTile(1,5));
		pathTimes.Add (9f);
		//4,5
		pathSteps.Add(_map.GetTile(4,5));
		pathTimes.Add (10f);
		//7,5
		pathSteps.Add(_map.GetTile(7,5));
		pathTimes.Add (11f);
		//10,5
		pathSteps.Add(_map.GetTile(10,5));
		pathTimes.Add (12f);
		//13,5
		pathSteps.Add(_map.GetTile(13,5));
		pathTimes.Add (13f);
		//16,5
		pathSteps.Add(_map.GetTile(16,5));
		pathTimes.Add (14f);
		//19,5
		pathSteps.Add(_map.GetTile(19,5));
		pathTimes.Add (15f);
		//22,5
		pathSteps.Add(_map.GetTile(22,5));
		pathTimes.Add (16f);
		
		//1,9
		pathSteps.Add(_map.GetTile(1,9));
		pathTimes.Add (17f);
		//4,9
		pathSteps.Add(_map.GetTile(4,9));
		pathTimes.Add (18f);
		//7,9
		pathSteps.Add(_map.GetTile(7,9));
		pathTimes.Add (19f);
		//10,9
		pathSteps.Add(_map.GetTile(10,9));
		pathTimes.Add (20f);
		//13,9
		pathSteps.Add(_map.GetTile(13,9));
		pathTimes.Add (21f);
		//16,9
		pathSteps.Add(_map.GetTile(16,9));
		pathTimes.Add (22f);
		//19,9
		pathSteps.Add(_map.GetTile(19,9));
		pathTimes.Add (23f);
		//22,9
		pathSteps.Add(_map.GetTile(22,9));
		pathTimes.Add (24f);


		arsonPath = new EDArsonPath (pathSteps, pathTimes);
	}
}