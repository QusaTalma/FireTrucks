public class TDTile {
	
	public const int TILE_CITY_FILL     = 0;
	public const int TILE_STREET = 1;
	public const int TILE_HOUSE     = 2;
	public const int TILE_HOUSE_ON_FIRE  = 3;
	public const int TILE_FIREHOUSE = 4;

	public int type = TILE_CITY_FILL;

	public TDTile(){

	}

	public TDTile(int tileType){
		type = tileType;
	}

}