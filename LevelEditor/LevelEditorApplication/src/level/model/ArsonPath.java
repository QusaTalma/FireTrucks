/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package level.model;

import java.awt.Point;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 *
 * @author asheehan
 */
public class ArsonPath {
    private Map<Point, ArsonStep> steps;
    
    public ArsonPath(){
        steps = new HashMap<>();
    }
    
    public List<ArsonStep> getSteps(){
        List<ArsonStep> stepList = new ArrayList<>(steps.values());
        
        Collections.sort(stepList);
        
        return stepList;
    }
    
    public void addStep(int x, int y){
        Point stepPoint = new Point(x,y);
        
        ArsonStep step;
        
        if(steps.containsKey(stepPoint)){
            step = steps.get(stepPoint);
        }else{
            step = new ArsonStep(stepPoint);
        }
        
        step.setTime(calculateNextTime());
        
        steps.put(stepPoint, step);
    }
    
    public void setStepTime(int x, int y, int time){
        Point stepPoint = new Point(x,y);
        ArsonStep step;
        
        if(steps.containsKey(stepPoint)){
            step = steps.get(stepPoint);
        }else{
            step = new ArsonStep(stepPoint);
        }
        
        step.setTime(time);
        
        steps.put(stepPoint, step);
    }
    
    public void removeStep(int x, int y){
        Point stepPoint = new Point(x,y);
        
        if(steps.containsKey(stepPoint)){
            steps.remove(stepPoint);
        }
    }
    
    private int calculateNextTime(){
        int nextTime = 0;
        List<ArsonStep> stepList = getSteps();
        if(stepList.size() > 0){
            ArsonStep lastStep = stepList.get(stepList.size()-1);
            nextTime = lastStep.getTime()+1;
        }
        
        return nextTime;
    }

    public String toString(int fillPadding) {
        StringBuilder builder = new StringBuilder();
        
        List<ArsonStep> stepList = getSteps();
        for(ArsonStep step : stepList){
            builder.append(step.toString(fillPadding));
            builder.append("\n");
        }
        
        return builder.toString();
    }
}