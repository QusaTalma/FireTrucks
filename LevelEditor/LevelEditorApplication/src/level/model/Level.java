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
import java.util.ArrayList;
import java.util.List;

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
    public static final String VALUE_DELIMITER = "|";
    public static final String VALUE_DELIMITER_REGEX = "\\|";
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
    private List<NPCCue> npcCues;
    
    public Level(){
        width = MIN_WIDTH;
        height = MIN_HEIGHT;
        map = new LevelMap(width, height);
        durationSeconds = DEFAULT_DURATION;
        winPercent = DEFAULT_PERCENT;
        arsonPath = new ArsonPath();
        npcCues = new ArrayList<NPCCue>();
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
    
    public List<NPCCue> getNPCCues(){
        return npcCues;
    }
    
    public void addNPCCue(NPCCue cue){
        npcCues.add(cue);
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
            offset = level.readArsonPath (splitLevelData, offset);
            offset = level.readNPCCues(splitLevelData, offset);
        }
        
        return level;
    }
    

    private int readInMap(String[] splitLevelData){
        String mapSize = splitLevelData [0];
        String[] mapSizes = mapSize.split (VALUE_DELIMITER_REGEX);
        width = Integer.parseInt(mapSizes [0]);
        height = Integer.parseInt(mapSizes [1]);
        int offset = 1;//Read one line for width and height

        map = new LevelMap(width, height);

        for (int y=offset; y<height+offset; y++) {
            String mapRowData = splitLevelData[y];
            for(int x=0; x<width; x++){
                int type = -1;
                String rowString = Character.toString(mapRowData.charAt(x));
                try{
                    type = Integer.parseInt(rowString);
                }catch(NumberFormatException e){
                }

                Tile tile;
                if(type >= 0){
                    tile = Tile.fromVal(type);
                }else{
                    tile = Tile.fromString(rowString);
                }
                
                if(tile == Tile.FIRE_STATION){
                    map.setFireHousePos(new Point(x, y-offset));
                }

                map.setTile(x, y-offset, tile);
            }
        }

        return height + offset;
    }

    private int readLevelParams(String[] splitLevelData, int offset){
        winPercent = Integer.parseInt(splitLevelData[offset]);
        offset++;
        durationSeconds = Integer.parseInt(splitLevelData [offset]);
        offset++;
        return offset;
    }

    private int readArsonPath(String[] splitLevelData, int offset){
        int endIndex;
        
        try{
            int steps = Integer.parseInt(splitLevelData[offset]);
            offset++;
            endIndex = offset + steps;
        }catch(NumberFormatException e){
            endIndex = splitLevelData.length;
        }
        
        while(offset < endIndex) {
            if(splitLevelData[offset] == null || !splitLevelData[offset].contains(VALUE_DELIMITER)){
                    continue;
            }
            String[] stepData = splitLevelData[offset].split(VALUE_DELIMITER_REGEX);
            int x = Integer.parseInt(stepData[0]);
            int y = Integer.parseInt(stepData[1]);

            arsonPath.addStep(x, y);
            
            String timeString = stepData[2].replaceAll("(\\r|\\n)", "");
            int time = Integer.parseInt(timeString);
            arsonPath.setStepTime(x, y, time);
            Tile tile = map.getTileMap()[x][y];
            
            if(tile == Tile.HOUSE){
                tile = Tile.HOUSE_ON_FIRE;
            }else if(tile == Tile.GREEN_HOUSE){
                tile = Tile.GREEN_HOUSE_ON_FIRE;
            }else if(tile == Tile.YELLOW_HOUSE){
                tile = Tile.YELLOW_HOUSE_ON_FIRE;
            }
            
            map.setTile(x, y, tile);
            
            offset++;
        }
        
        return offset;
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
        writeBuilder.append(String.format("%d\n", arsonPath.getSteps().size()));
        writeBuilder.append(arsonPath.toString(map.getFillPadding()));
        writeBuilder.append(String.format("%d\n", npcCues.size()));
        for(int i=0; i<npcCues.size(); i++){
            writeBuilder.append(npcCues.get(i).toString().concat("\n"));
        }
        
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

    private int readNPCCues(String[] splitLevelData, int offset) {
        npcCues = new ArrayList<NPCCue>();
        if(offset >= splitLevelData.length){
            return offset;
        }
        int endIndex;
        
        try{
            int numCues = Integer.parseInt(splitLevelData[offset]);
            offset++;
            endIndex = offset + numCues;
        }catch(NumberFormatException e){
            endIndex = splitLevelData.length;
        }
        
        while(offset < endIndex) {
            if(splitLevelData[offset] == null || !splitLevelData[offset].contains(VALUE_DELIMITER)){
                    continue;
            }
            
            String[] cueData = splitLevelData[offset].split(VALUE_DELIMITER_REGEX);
            float timeToShow = Float.parseFloat(cueData[0]);
            String npcToShow = cueData[1];
            String textToShow = cueData[2];
            
            NPCCue cue = new NPCCue(timeToShow, npcToShow, textToShow);
            npcCues.add(cue);
            
            offset++;
        }
        
        return offset;
    }
}