using System;
using UnityEngine;
public class TDTile {
	
	public const int TILE_CITY_FILL     = 0;
	public const int TILE_STREET = 1;
	public const int TILE_HOUSE     = 2;
	public const int TILE_HOUSE_ON_FIRE  = 3;
	public const int TILE_FIREHOUSE = 4;
	public const int TILE_BURNED_DOWN_HOUSE = 5;

	public int type = TILE_CITY_FILL;
	public float maxDurability = 15;
	public float durability;

	private int x, y;

	public TDTile(int x, int y){
		this.x = x;
		this.y = y;
		durability = maxDurability;
	}

	public float GetDurability(){
		return durability;
	}

	public void Damage(float damage){
		durability -= damage;

		if (durability <= 0) {
			type = TILE_BURNED_DOWN_HOUSE;
		}
	}

	public bool Equals(TDTile other){
		return other != null &&
			other.GetX() == x &&
			other.GetY() == y &&
			other.type == type;
	}

	override public string ToString(){
		return "TDTile: (" + x + ", " + y + ") type:"+type;
	}

	public void SetX(int x){
		this.x = x;
	}

	public int GetX(){
		return x;
	}

	public void SetY(int y){
		this.y = y;
	}

	public int GetY(){
		return y;
	}

	public TDTile(int tileType){
		type = tileType;
	}

}