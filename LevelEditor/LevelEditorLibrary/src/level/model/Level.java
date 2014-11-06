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
    
    public Level(){
        width = MIN_WIDTH;
        height = MIN_HEIGHT;
        map = new LevelMap(width, height);
        durationSeconds = DEFAULT_DURATION;
        winPercent = DEFAULT_PERCENT;
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
    
    public void resize(int newWidth, int newHeight){
       width = newWidth;
       height = newHeight;
       map.resize(width, height);
    }
}
