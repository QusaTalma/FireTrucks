using System;
using UnityEngine;
public class TDTile {
	public enum Type{
		FILL, APPLE_CART, CONSTRUCTION, GRASS, TREE, WATER,
		STREET,
		BLUE_HOUSE, GREEN_HOUSE, YELLOW_HOUSE,
		FIREHOUSE
	}

	public const int CITY_FILL_INDEX = 0;
	public const int STREET_INDEX = 1;
	public const int BLUE_HOUSE_INDEX = 2;
	public const int HOUSE_ON_FIRE_INDEX = 3;
	public const int FIRE_STATION_INDEX = 4;
	public const int START_FIRE_INDEX = 5;
	public const int WATER_INDEX = 6;
	public const int CONSTRUCTION_INDEX = 7;
	public const int TREE_INDEX = 8;
	public const int GRASS_INDEX = 9;
	public const int APPLE_CART_INDEX = 10;
	public const int GREEN_HOUSE_INDEX = 11;
	public const int GREEN_HOUSE_ON_FIRE_INDEX = 12;
	public const int YELLOW_HOUSE_INDEX = 13;
	public const int YELLOW_HOUSE_ON_FIRE_INDEX = 14;

	public const string FILL_CHAR = "f";
	public const string WATER_CHAR = "~";
	public const string CONSTRUCTION_CHAR = "x";
	public const string TREE_CHAR = "T";
	public const string GRASS_CHAR = "|";
	public const string APPLE_CART_CHAR = "@";
	 
	public const string STREET_CHAR = "-";
	public const string BLUE_HOUSE_CHAR = "H";
	public const string GREEN_HOUSE_CHAR = "G";
	public const string YELLOW_HOUSE_CHAR = "Y";
	public const string FIRE_STATION_CHAR = "F";

	public Type type = Type.FILL;
	public float maxDurability = 15;
	public float durability;
	private bool onFire;
	public bool OnFire{
		get { return onFire; }
		set { onFire = value; }
	}

	private int x, y;

	public TDTile(int x, int y){
		this.x = x;
		this.y = y;
		durability = maxDurability;
		onFire = false;
	}

	public bool IsDamaged(){
		return durability < maxDurability;
	}

	public bool IsFlammable(){
		return type == Type.BLUE_HOUSE ||
			type == Type.GREEN_HOUSE ||
			type == Type.YELLOW_HOUSE;
	}

	public float GetDurability(){
		return durability;
	}

	public void Damage(float damage){
		durability -= damage;
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

	public int GetIndex(){
		int index = CITY_FILL_INDEX;

		switch (type) {
		case Type.APPLE_CART:
			index = APPLE_CART_INDEX;
			break;
		case Type.BLUE_HOUSE:
			//Since there's an animated house for this show a street background
			index = STREET_INDEX;
			break;
		case Type.CONSTRUCTION:
			index = CONSTRUCTION_INDEX;
			break;
		case Type.FILL:
			index = CITY_FILL_INDEX;
			break;
		case Type.FIREHOUSE:
			//Since there's an animated house for this show a street background
			index = STREET_INDEX;
			break;
		case Type.GRASS:
			index = GRASS_INDEX;
			break;
		case Type.GREEN_HOUSE:
			index = GREEN_HOUSE_INDEX;
			break;
		case Type.STREET:
			index = STREET_INDEX;
			break;
		case Type.TREE:
			index = TREE_INDEX;
			break;
		case Type.WATER:
			index = WATER_INDEX;
			break;
		}

		return index;
	}

	public static Type GetTypeForString(string typeString){
		Type type = Type.FILL;

		switch (typeString) {
		case APPLE_CART_CHAR:
			type = Type.APPLE_CART;
			break;
		case CONSTRUCTION_CHAR:
			type = Type.CONSTRUCTION;
			break;
		case FILL_CHAR:
			type = Type.FILL;
			break;
		case FIRE_STATION_CHAR:
			type = Type.FIREHOUSE;
			break;
		case GRASS_CHAR:
			type = Type.GRASS;
			break;
		case GREEN_HOUSE_CHAR:
			type = Type.GREEN_HOUSE;
			break;
		case BLUE_HOUSE_CHAR:
			type = Type.BLUE_HOUSE;
			break;
		case WATER_CHAR:
			type = Type.WATER;
			break;
		case STREET_CHAR:
			type = Type.STREET;
			break;
		case TREE_CHAR:
			type = Type.TREE;
			break;
		case YELLOW_HOUSE_CHAR:
			type = Type.YELLOW_HOUSE;
			break;
		}

		return type;
	}
}