using System.Collections.Generic;
using UnityEngine;

public class TDLevel {
	/*****
	 *Level text file format
	 *<int width>|<int height>
	 *<width x height grid of characters, each representing a tile>
	 *<int % value of city needed to win>
	 *<int seconds of play time>
	 *<int number of arsonist steps>
	 *<previous number of lines representing arsonist steps, format on following line>
	 *<int fireX| int fireY| float timeToPlace> //Note, these MUST be in chronological order
	 *<int number of NPC cues in the level>
	 *<previous number of lines representing NPC cues, format on following line>
	 *<float timeToShow| string npcToShow(f=firechief, p=policechief, m=mayor)| string textToShow
	 *
	 *Update 1: 1/11/2015
	 *Added count for arson steps
	 *Added count and data for npc cues
	 *
	 *Update 2: 2/11/2015
	 *Changed delimiter from , to |
	 *****/
	private const char VALUE_DELIMITER = '|';

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

	private List<NPCCue> npcCues;
	public List<NPCCue> NPCCues{
		get { return npcCues; }
	}

	public TDLevel(string levelData){
		string[] splitLevelData = levelData.Split ('\n');
		int offset = ReadInMap (splitLevelData);
		offset = ReadLevelParams (splitLevelData, offset);
		offset = ReadArsonPath (splitLevelData, offset);
		offset = ReadNPCCues (splitLevelData, offset);

		_map = new TDMap(tiles, firehouseLocation, totalDurability);
	}

	int ReadInMap(string[] splitLevelData){
		string mapSize = splitLevelData [0];
		string[] mapSizes = mapSize.Split (VALUE_DELIMITER);
		mapWidth = int.Parse (mapSizes [0]);
		mapHeight = int.Parse (mapSizes [1]);
		int offset = 1;//Read one line for width and height

		tiles = new TDTile[mapWidth, mapHeight];
		for (int y=offset; y<mapHeight+offset; y++) {
			string mapRowData = splitLevelData[y];
			for(int x=0; x<mapWidth; x++){
				TDTile tile = new TDTile(x,y-offset);
				tile.type = TDTile.GetTypeForString(mapRowData[x].ToString());
				if(tile.type == TDTile.Type.FIREHOUSE){
					firehouseLocation = new Vector2(x,y-offset);
				}else if(tile.IsFlammable()){
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

	int ReadArsonPath(string[] splitLevelData, int offset){
		int numSteps = int.Parse(splitLevelData [offset]);
		offset++;
		List<TDTile> pathSteps = new List<TDTile> ();
		List<float> pathTimes = new List<float> ();
		for(int i=0; i<numSteps; i++){
			if(splitLevelData[offset] == null || !splitLevelData[offset].Contains(VALUE_DELIMITER.ToString())){
				continue;
			}
			string[] stepData = splitLevelData[offset].Split(VALUE_DELIMITER);
			int x = int.Parse(stepData[0]);
			int y = int.Parse(stepData[1]);
			pathSteps.Add(tiles[x,y]);

			float time = float.Parse(stepData[2]);
			pathTimes.Add(time);

			offset++;
		}

		arsonPath = new EDArsonPath (pathSteps, pathTimes);

		return offset;
	}

	int ReadNPCCues(string[] splitLevelData, int offset){
		npcCues = new List<NPCCue> ();

		int numCues = int.Parse(splitLevelData[offset]);
		offset++;

		for (int i=0; i<numCues; i++) {
			string[] cueData = splitLevelData[offset].Split(VALUE_DELIMITER);
			float timeToShow = float.Parse(cueData[0]);
			string npcToShow = cueData[1];
			string textToShow = cueData[2];

			npcCues.Add(new NPCCue(timeToShow, npcToShow, textToShow));

			offset++;
		}
		return offset;
	}
}