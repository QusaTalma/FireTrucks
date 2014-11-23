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
			if(splitLevelData[i] == null || !splitLevelData[i].Contains(",")){
				continue;
			}
			string[] stepData = splitLevelData[i].Split(',');
			int x = int.Parse(stepData[0]);
			int y = int.Parse(stepData[1]);
			pathSteps.Add(tiles[x,y]);

			float time = float.Parse(stepData[2]);
			pathTimes.Add(time);
		}

		arsonPath = new EDArsonPath (pathSteps, pathTimes);
	}
}