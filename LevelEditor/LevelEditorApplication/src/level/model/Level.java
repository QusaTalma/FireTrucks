/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package level.model;

import java.awt.Point;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

/**
 *
 * @author asheehan
 */
/*****
    *Level text file format
    *<int width>,<int height>
    *<width x height grid of characters, each representing a tile>
    *<int % value of city needed to win>
    *<int seconds of play time>
    *<any number of lines representing arsonist steps, format on following line>
    *<int fireX, int fireY, float timeToPlace> //Note, these MUST be in chronological order
*****/
public class Level {
    public static final int MIN_WIDTH = 26;
    public static final int MIN_HEIGHT = 14;
    public static final int MAX_WIDTH = 128;
    public static final int MAX_HEIGHT = 128;
    
    public static final int DEFAULT_DURATION = 90;
    public static final int MIN_DURATION = 0;
    public static final int MAX_DURATION = 1000;
    
    public static final int DEFAULT_PERCENT = 70;
    public static final int MIN_PERCENT = 1;
    public static final int MAX_PERCENT = 100;//duh
    
    private int width, height;
    private LevelMap map;
    private int durationSeconds;
    private int winPercent;
    private ArsonPath arsonPath;
    
    public Level(){
        width = MIN_WIDTH;
        height = MIN_HEIGHT;
        map = new LevelMap(width, height);
        durationSeconds = DEFAULT_DURATION;
        winPercent = DEFAULT_PERCENT;
        arsonPath = new ArsonPath();
    }

    public int getWidth() {
        return width;
    }

    public int getHeight() {
        return height;
    }

    public LevelMap getMap() {
        return map;
    }

    public void setMap(LevelMap map) {
        this.map = map;
    }

    public int getDurationSeconds() {
        return durationSeconds;
    }

    public void setDurationSeconds(int durationSeconds) {
        this.durationSeconds = durationSeconds;
    }

    public int getWinPercent() {
        return winPercent;
    }

    public void setWinPercent(int winPercent) {
        this.winPercent = winPercent;
    }
    
    public ArsonPath getArsonPath(){
        return arsonPath;
    }
    
    public void resize(int newWidth, int newHeight){
        for(int x=0; x<width; x++){
            for(int y = 0; y<height; y++){
                if(x >= newWidth || y >= newHeight){
                    Tile tile = map.getTileMap()[x][y];

                    if(map.getTileMap()[x][y] == Tile.HOUSE_ON_FIRE){
                        arsonPath.removeStep(x, y);
                    }else if(tile == Tile.FIRE_STATION){
                        map.setFireHousePos(null);
                    }
                }
            }
        }
        
        width = newWidth;
        height = newHeight;
        map.resize(width, height);
    }
    
    public static Level loadFromFile(File file){
        Level level = new Level();
        InputStream in = null;
        String rawLevelData = null;
        try {
            in = new FileInputStream(file);
            StringBuilder strBuff = new StringBuilder();
            int count;
            byte[] buff = new byte[1024];
            while((count = in.read(buff, 0, 1024)) > -1){
                strBuff.append(new String(buff, 0, count));
            }
            rawLevelData = strBuff.toString();
        } catch (FileNotFoundException ex) {
        } catch (IOException e){
        }finally{
            if(in != null){
                try {
                    in.close();
                } catch (IOException ex) {
                }
            }
        }
        
        if(rawLevelData != null){
            String[] splitLevelData = rawLevelData.split("\n");

            int offset = level.readInMap (splitLevelData);
            offset = level.readLevelParams (splitLevelData, offset);
            level.readArsonPath (splitLevelData, offset);
        }
        
        return level;
    }
    

    private int readInMap(String[] splitLevelData){
        String mapSize = splitLevelData [0];
        String[] mapSizes = mapSize.split (",");
        width = Integer.parseInt(mapSizes [0]);
        height = Integer.parseInt(mapSizes [1]);
        int offset = 1;//Read one line for width and height

        map = new LevelMap(width, height);

        for (int y=offset; y<height+offset; y++) {
            String mapRowData = splitLevelData[y];
            for(int x=0; x<width; x++){
                int type = Integer.parseInt(Character.toString(mapRowData.charAt(x)));
                Tile tile = Tile.fromVal(type);
                if(tile == Tile.FIRE_STATION){
                    map.setFireHousePos(new Point(x, y-offset));
                }

                map.setTile(x, y-offset, tile);
            }
        }

        return height + offset;
    }

    private int readLevelParams(String[] splitLevelData, int offset){
        winPercent = Integer.parseInt(splitLevelData[offset])/100;
        offset++;
        durationSeconds = Integer.parseInt(splitLevelData [offset]);
        offset++;
        return offset;
    }

    private void readArsonPath(String[] splitLevelData, int offset){
        for(int i=offset; i<splitLevelData.length; i++){
            if(splitLevelData[i] == null || !splitLevelData[i].contains(",")){
                    continue;
            }
            String[] stepData = splitLevelData[i].split(",");
            int x = Integer.parseInt(stepData[0]);
            int y = Integer.parseInt(stepData[1]);

            arsonPath.addStep(x, y);
            
            String timeString = stepData[2].replaceAll("(\\r|\\n)", "");
            int time = Integer.parseInt(timeString);
            arsonPath.setStepTime(x, y, time);
            map.getTileMap()[x][y] = Tile.HOUSE_ON_FIRE;
        }
    }

    public void writeToFile(File file) {
        if(!file.exists()){
            try {
                file.createNewFile();
            } catch (IOException ex) {
                return;
            }
        }
        
        String toWrite;
        
        StringBuilder writeBuilder = new StringBuilder();
        writeBuilder.append(String.format("%d,%d\n", width+map.getFillPadding()*2,
                height+map.getFillPadding()*2));
        writeBuilder.append(String.format("%s", map.toString()));
        writeBuilder.append(String.format("%d\n", winPercent));
        writeBuilder.append(String.format("%d\n", durationSeconds));
        writeBuilder.append(arsonPath.toString(map.getFillPadding()));
        
        OutputStream fileOutput = null;
        
        try {
            fileOutput = new FileOutputStream(file);
            fileOutput.write(writeBuilder.toString().getBytes());
            fileOutput.flush();
        } catch (FileNotFoundException ex) {
        } catch (IOException e){
            
        }finally{
            if(fileOutput != null){
                try{
                    fileOutput.close();
                }catch(IOException e){
                    
                }
            }
        }        
    }

    public void setFillPadding(int newPadding) {
        map.setFillPadding(newPadding);
    }
}
