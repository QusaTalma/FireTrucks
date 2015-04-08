/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package level.model;

/**
 *
 * @author asheehan
 */
public enum Tile {
    CITY_FILL(0),
    STREET(1),
    HOUSE(2),
    HOUSE_ON_FIRE(3),
    FIRE_STATION(4),
    START_FIRE(5),
    RIVER(6),
    CONSTRUCTION(7),
    TREE(8),
    GRASS(9),
    APPLE_CART(10),
    GREEN_HOUSE(11),
    GREEN_HOUSE_ON_FIRE(12),
    YELLOW_HOUSE(13),
    YELLOW_HOUSE_ON_FIRE(14),
    TREES_ON_FIRE(15);
    
    public static final String FILL_CHAR = "f";
    public static final String RIVER_CHAR = "~";
    public static final String CONSTRUCTION_CHAR = "x";
    public static final String TREE_CHAR = "T";
    public static final String GRASS_CHAR = "|";
    public static final String APPLE_CART_CHAR = "@";
    
    public static final String STREET_CHAR = "-";
    public static final String HOUSE_CHAR = "H";
    public static final String GREEN_HOUSE_CHAR = "G";
    public static final String YELLOW_HOUSE_CHAR = "Y";
    public static final String FIRE_STATION_CHAR = "F";
    
    private final int numVal;

    Tile(int numVal) {
        this.numVal = numVal;
    }
    
    public static Tile fromVal(int val){
        Tile tile = CITY_FILL;
        switch(val){
            case 1:
                tile = STREET;
                break;
            case 2:
                tile = HOUSE;
                break;
            case 3:
                tile = HOUSE_ON_FIRE;
                break;
            case 4:
                tile = FIRE_STATION;
                break;
            case 5:
                tile = START_FIRE;
                break;
            case 6:
                tile = RIVER;
                break;
            case 7:
                tile = CONSTRUCTION;
                break;
            case 8:
                tile = TREE;
                break;
            case 9:
                tile = GRASS;
                break;
            case 10:
                tile = APPLE_CART;
                break;
            case 11:
                tile = GREEN_HOUSE;
                break;
            case 12:
                tile = GREEN_HOUSE_ON_FIRE;
                break;
            case 13:
                tile = YELLOW_HOUSE;
                break;
            case 14:
                tile = YELLOW_HOUSE_ON_FIRE;
                break;
            case 15:
                tile = TREES_ON_FIRE;
                break;
        }
        
        return tile;
    }

    public int getNumVal() {
        return numVal;
    }
    
    public String getLabel(){
        String label = "";
        
        switch(this){
            case APPLE_CART:
                label = "Apple Cart";
                break;
            case CITY_FILL:
                label = "Fill";
                break;
            case CONSTRUCTION:
                label = "Construction";
                break;
            case FIRE_STATION:
                label = "Fire Station";
                break;
            case GRASS:
                label = "Grass";
                break;
            case GREEN_HOUSE:
                label = "Green house";
                break;
            case HOUSE:
                label = "Blue house";
                break;
            case HOUSE_ON_FIRE:
                label = "Start fire";
                break;
            case RIVER:
                label = "River";
                break;
            case STREET:
                label = "Street";
                break;
            case TREE:
                label = "Tree";
                break;
            case YELLOW_HOUSE:
                label = "Yellow house";
                break;
        }
        
        return label;
    }
    
    public static Tile fromString(String str){
        Tile tile = CITY_FILL;
        
        if(str.equals(APPLE_CART_CHAR)){
            tile = APPLE_CART;
        }else if(str.equals(CONSTRUCTION_CHAR)){
            tile = CONSTRUCTION;
        }else if(str.equals(FIRE_STATION_CHAR)){
            tile = FIRE_STATION;
        }else if(str.equals(GRASS_CHAR)){
            tile = GRASS;
        }else if(str.equals(HOUSE_CHAR)){
            tile = HOUSE;
        }else if(str.equals(RIVER_CHAR)){
            tile = RIVER;
        }else if(str.equals(STREET_CHAR)){
            tile = STREET;
        }else if(str.equals(TREE_CHAR)){
            tile = TREE;
        }else if(str.equals(GREEN_HOUSE_CHAR)){
            tile = GREEN_HOUSE;
        }else if(str.equals(YELLOW_HOUSE_CHAR)){
            tile = YELLOW_HOUSE;
        }
        
        return tile;
    }

    @Override
    public String toString() {
        String retval = "";
        switch(this){
            case APPLE_CART:
                retval = APPLE_CART_CHAR;
                break;
            case CITY_FILL:
                retval = FILL_CHAR;
                break;
            case CONSTRUCTION:
                retval = CONSTRUCTION_CHAR;
                break;
            case FIRE_STATION:
                retval = FIRE_STATION_CHAR;
                break;
            case GRASS:
                retval = GRASS_CHAR;
                break;
            case HOUSE:
            case HOUSE_ON_FIRE:
            case START_FIRE:
                retval = HOUSE_CHAR;
                break;
            case RIVER:
                retval = RIVER_CHAR;
                break;
            case STREET:
                retval = STREET_CHAR;
                break;
            case TREE:
            case TREES_ON_FIRE:
                retval = TREE_CHAR;
                break;
            case GREEN_HOUSE:
            case GREEN_HOUSE_ON_FIRE:
                retval = GREEN_HOUSE_CHAR;
                break;
            case YELLOW_HOUSE:
            case YELLOW_HOUSE_ON_FIRE:
                retval = YELLOW_HOUSE_CHAR;
                break;
        }
        
        return retval;
    }
}
