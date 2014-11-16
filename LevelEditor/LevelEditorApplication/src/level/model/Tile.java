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
    START_FIRE(5);
    
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
        }
        
        return tile;
    }

    public int getNumVal() {
        return numVal;
    }

    @Override
    public String toString() {
        int val = getNumVal();
        if(this == HOUSE_ON_FIRE || this == START_FIRE){
            val = Tile.HOUSE.getNumVal();
        }
        
        return Integer.toString(val);
    }
}
