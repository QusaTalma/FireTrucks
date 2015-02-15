/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package level.model;

import java.awt.Point;

/**
 *
 * @author asheehan
 */
public class ArsonStep implements Comparable<ArsonStep>{
    private Point location;
    private int time;
    
    public ArsonStep(Point location){
        this.location = location;
    }
    
    public Point getLocation(){
        return location;
    }
    
    public int getTime(){
        return time;
    }
    
    public void setTime(int time){
        this.time = time;
    }

    @Override
    public int compareTo(ArsonStep o) {
        return Integer.compare(time, o.getTime());
    }

    @Override
    public boolean equals(Object obj) {
        if(obj instanceof ArsonStep){
            ArsonStep other = (ArsonStep)obj;
            return other.getLocation().equals(location);
        }
        
        return false; 
    }

    public String toString(int fillPadding) {
        return String.format("%d%s%d%s%d", location.x+fillPadding, Level.VALUE_DELIMITER,
                location.y+fillPadding, Level.VALUE_DELIMITER,
                time);
    }
}