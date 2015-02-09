using UnityEngine;
using System.Collections.Generic;

public class TDMap {
	TDTile[,] _tiles;

	int _width;
	public int Width{
		get { return _width; }
	}
	
	int _height;
	public int Height{
		get { return _height; }
	}
	
	float totalDurability;
	Vector2 fireHousePosition;

	public TDMap(TDTile[,] tiles, Vector2 fireHousePos, float totalDurability) {
		_width = tiles.GetLength(0);
		_height = tiles.GetLength(1);

		_tiles = tiles;

		this.totalDurability = totalDurability;

		SetFireHouseCoordinates (fireHousePos);
	}

	public void SetFireHouseCoordinates(Vector2 pos){
		fireHousePosition = pos;
		
		totalDurability -= _tiles [(int)fireHousePosition.x, (int)fireHousePosition.y].GetDurability ();
		_tiles [(int)fireHousePosition.x, (int)fireHousePosition.y].type = TDTile.Type.FIREHOUSE;
	}

	public void GetFireHouseCoordinates(out Vector2 pos){
		pos.x = fireHousePosition.x; 
		pos.y = fireHousePosition.y;
	}

	public TDTile GetTile(int x, int y){
		if (x < 0 || 
		    x >= _width || 
		    y < 0 || 
		    y >= _height) {
			return null;
		}
		return _tiles [x, y];
	}

	public float GetTotalDurability(){
		return totalDurability;
	}

	public float GetCurrentDurability(){
		float currentDurability = 0;
		for (int x = 0; x< _width; x++){
			for( int y = 0; y < _height; y++){
				if(_tiles[x,y].IsFlammable()){
					currentDurability += _tiles[x,y].GetDurability();
				}
			}
		}
		return currentDurability;
	}

	public List<TDTile> GetAdjacentTiles(TDTile tile) {
		List<TDTile> adjacent = new List<TDTile> ();

		int tileX = tile.GetX ();
		int tileY = tile.GetY ();

		if (tileX > 0){
			adjacent.Add(_tiles[tileX-1, tileY]);
		}

		if (tileX < _width - 1) {
			adjacent.Add(_tiles[tileX+1, tileY]);
		}

		if (tileY > 0) {
			adjacent.Add(_tiles[tileX, tileY-1]);
		}

		if (tileY < _height - 1) {
			adjacent.Add(_tiles[tileX, tileY+1]);
		}

		return adjacent;
	}

	public List<TDTile> FindAdjacentFlammableTiles(TDTile tile){
		List<TDTile> adjacentFlammable = new List<TDTile> ();
		List<TDTile> allAdjacent = GetAdjacentTiles (tile);
		
		for(int i=0; i<allAdjacent.Count; i++){
			TDTile other = allAdjacent[i];
			if(other.IsFlammable()){
				adjacentFlammable.Add(other);
			}
		}
		
		return adjacentFlammable;
	}

	public List<TDTile> FindAdjacentTilesOfType(TDTile tile, TDTile.Type tileType) {
		List<TDTile> adjacentOfType = new List<TDTile> ();
		List<TDTile> allAdjacent = GetAdjacentTiles (tile);

		for(int i=0; i<allAdjacent.Count; i++){
			TDTile other = allAdjacent[i];
			if(other.type == tileType){
				adjacentOfType.Add(other);
			}
		}

		return adjacentOfType;
	}
}