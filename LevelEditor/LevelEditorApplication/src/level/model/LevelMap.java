/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package level.model;

import java.awt.Point;
import java.util.Arrays;

/**
 *
 * @author asheehan
 */
public class LevelMap {
    private Tile[][] tileMap;
    private Point fireHousePos;
    
    public LevelMap(int width, int height){
        tileMap = new Tile[width][height];
        for(int x=0; x<width; x++){
            Arrays.fill(tileMap[x], Tile.CITY_FILL);
        }
    }

    public Tile[][] getTileMap() {
        return tileMap;
    }
    
    public Point getFireHousePos(){
        return fireHousePos;
    }
    
    public void setFireHousePos(Point fireHousePos){
        this.fireHousePos = fireHousePos;
    }

    public void setTile(int x, int y, Tile toSet){
        if(fireHousePos != null){
            //If setting to firestation then update the location
            if(toSet == Tile.FIRE_STATION){
                //Revert the firehouse into a regular house
                tileMap[fireHousePos.x][fireHousePos.y] = Tile.HOUSE;
                fireHousePos.x = x;
                fireHousePos.y = y;
            }else{
                //If setting the firestation to a non-firestation then unset
                if(x == fireHousePos.x && y == fireHousePos.y){
                    fireHousePos = null;
                }
            }
        }else if(toSet == Tile.FIRE_STATION){
            fireHousePos = new Point(x, y);
        }
        
        tileMap[x][y] = toSet;
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

    @Override
    public String toString() {
        StringBuilder builder = new StringBuilder();
        
        for(int y=0; y<tileMap[0].length; y++){
            for(int x=0; x<tileMap.length; x++){
                builder.append(tileMap[x][y].toString());
            }
            builder.append("\n");
        }
        
        return builder.toString();
    }
}