/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package level.model;

import java.util.Arrays;

/**
 *
 * @author asheehan
 */
public class LevelMap {
    private Tile[][] tileMap;
    
    public LevelMap(int width, int height){
        tileMap = new Tile[width][height];
        for(int x=0; x<width; x++){
            Arrays.fill(tileMap[x], Tile.CITY_FILL);
        }
    }

    public Tile[][] getTileMap() {
        return tileMap;
    }

    public void setTileMap(Tile[][] tileMap) {
        this.tileMap = tileMap;
    }
    
    public void resize(int newWidth, int newHeight){
        int width = tileMap.length;
        int height = tileMap[0].length;
        
        //Use the minimum of new and old values to prevent index out of bounds
        int copyWidth = Math.min(width, newWidth);
        int copyHeight = Math.min(height, newHeight);
        
        //Initialize the new map to fill
        Tile[][] newMap = new Tile[newWidth][newHeight];
        
        for(int x=0; x<newWidth; x++){
            Arrays.fill(newMap[x], Tile.CITY_FILL);
        }
        
        for(int x=0; x<copyWidth; x++){
            for(int y=0; y<copyHeight; y++){
                newMap[x][y] = tileMap[x][y];
            }
        }
        
        tileMap = newMap;
    }
}