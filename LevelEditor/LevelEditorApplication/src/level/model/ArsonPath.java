/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package level.model;

import java.awt.Point;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 *
 * @author asheehan
 */
public class ArsonPath {
    private List<ArsonStep> steps;
    
    public ArsonPath(){
        steps = new ArrayList<>();
    }
    
    public List<ArsonStep> getSteps(){
        return steps;
    }
    
    public void addStep(int x, int y){
        Point stepPoint = new Point(x,y);
        
        ArsonStep step = new ArsonStep(stepPoint);
        
        if(steps.contains(step)){
            step = steps.get(steps.indexOf(step));
        }else{
            steps.add(step);
        }
        
        step.setTime(calculateNextTime());
    }
    
    public void setStepTime(int x, int y, int time){
        Point stepPoint = new Point(x,y);
        
        ArsonStep step = new ArsonStep(stepPoint);
        
        if(steps.contains(step)){
            step = steps.get(steps.indexOf(step));
        }else{
            steps.add(step);
        }
        
        step.setTime(time);
    }
    
    public void removeStep(int x, int y){
        Point stepPoint = new Point(x,y);
        ArsonStep step = new ArsonStep(stepPoint);
        
        if(steps.contains(step)){
            steps.remove(step);
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
        Collections.sort(stepList);
        for(ArsonStep step : stepList){
            builder.append(step.toString(fillPadding));
            builder.append("\n");
        }
        
        return builder.toString();
    }
}