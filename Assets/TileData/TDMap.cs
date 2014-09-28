using UnityEngine;
using System.Collections.Generic;

public class TDMap {
	TDTile[,] _tiles;
	int _width;
	int _height;

	int fireHouseX = 5;
	int fireHouseY = 5;

	public TDMap(int width, int height) {
		_width = width;
		_height = height;

		_tiles = new TDTile[_width,_height];

		for (int x = 0; x< _width; x++){
			for( int y = 0; y < _height; y++){
				_tiles[x,y] = new TDTile(x, y);
				if(x == 0 ||
				   x == _width -1 ||
				   x%3 == 0 ||
				   y == 0 ||
				   y == _height - 1 ||
				   y%4 == 0){
					_tiles[x,y].type = TDTile.TILE_STREET;
				}
			}
		}

		PlaceHousesOnStreets ();

		SetFireHouseCoordinates (CalculateFirehousePosition ());
	}

	Vector2 CalculateFirehousePosition(){
		Vector2 position = new Vector2 ();

		position.x = (int)(_width / 2);
		position.y = (int)(_height / 2);

		TDTile tile = GetTile ((int)position.x, (int)position.y);

		if (tile.type != TDTile.TILE_HOUSE) {
			List<TDTile> houses = FindAdjacentTilesOfType(tile, TDTile.TILE_HOUSE);
			if(houses.Count > 0){
				position.x = houses[0].GetX();
				position.y = houses[0].GetY();
			}
		}

		return position;
	}

	public void SetFireHouseCoordinates(Vector2 pos){
		fireHouseX = (int)pos.x;
		fireHouseY = (int)pos.y;

		_tiles [fireHouseX, fireHouseY].type = TDTile.TILE_FIREHOUSE;
	}

	public void GetFireHouseCoordinates(out Vector2 pos){
		pos.x = fireHouseX;
		pos.y = fireHouseY;
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

	void PlaceHousesOnStreets(){
		for (int x = 0; x< _width; x++){
			for( int y = 0; y < _height; y++){
				if(_tiles[x,y].type == TDTile.TILE_STREET){
					PlaceHousesOnStreet(x, y);
				}
			}
		}
	}

	void PlaceHousesOnStreet(int x, int y){
		if (x > 0 && _tiles[x - 1, y].type == TDTile.TILE_CITY_FILL) {
			_tiles[x-1,y].type = TDTile.TILE_HOUSE;
		}

		if (x < _width - 1 && _tiles [x + 1, y].type == TDTile.TILE_CITY_FILL) {
			_tiles[x+1,y].type = TDTile.TILE_HOUSE;
		}
		
		if (y > 0 && _tiles[x, y-1].type == TDTile.TILE_CITY_FILL) {
			_tiles[x,y-1].type = TDTile.TILE_HOUSE;
		}
		
		if (y < _height - 1 && _tiles [x, y+1].type == TDTile.TILE_CITY_FILL) {
			_tiles[x,y+1].type = TDTile.TILE_HOUSE;
		}
	}

	public int GetWidth(){
		return _width;
	}

	public int GetHeight(){
		return _height;
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

	public List<TDTile> FindAdjacentTilesOfType(TDTile tile, int tileType) {
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