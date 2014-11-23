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
    FIRE_STATION(4);
    
    private final int numVal;

    Tile(int numVal) {
        this.numVal = numVal;
    }

    public int getNumVal() {
        return numVal;
    }
}
