using UnityEngine;
using System.Collections.Generic;

public class TDMap {
	TDTile[,] _tiles;
	int _width;
	int _height;

	int fireHouseX = 5;
	int fireHouseZ = 5;

	public TDMap(int width, int height) {
		_width = width;
		_height = height;

		_tiles = new TDTile[_width,_height];

		for (int x = 0; x< _width; x++){
			for( int y = 0; y < _height; y++){
				_tiles[x,y] = new TDTile();
				if(x == 0 ||
				   x == _width -1 ||
				   x%3 == 0 ||
				   y == 0 ||
				   y == _height - 1){
					_tiles[x,y].type = TDTile.TILE_STREET;
				}
			}
		}

		PlaceHousesOnStreets ();

		_tiles [fireHouseX, fireHouseZ].type = TDTile.TILE_FIREHOUSE;
	}

	public void GetFireHouseCoordinates(out Vector2 pos){
		pos.x = fireHouseX;
		pos.y = fireHouseZ;
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
}