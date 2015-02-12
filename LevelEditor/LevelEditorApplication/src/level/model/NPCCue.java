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
public class NPCCue {
    
    private float timeToShow;
    public float getTimeToShow() {
        return timeToShow;
    }
    public void setTimeToShow(float timeToShow){
        this.timeToShow = timeToShow;
    }
    
    private String npcToShow;
    public String getNpcToShow() {
        return npcToShow;
    }
    public void setNPCToShow(String npcToShow){
        this.npcToShow = npcToShow;
    }

    private String textToShow;
    public String getTextToShow() {
        return textToShow;
    }
    public void setTextToShow(String textToShow){
        this.textToShow = textToShow;
    }
    
    public NPCCue(float timeToShow, String npcToShow, String textToShow){
        this.timeToShow = timeToShow;
        this.npcToShow = npcToShow;
        this.textToShow = textToShow;
    }
    
    @Override
    public String toString(){
        return String.format("%f%s%s%s%s", timeToShow, Level.VALUE_DELIMITER, 
                                           npcToShow, Level.VALUE_DELIMITER, 
                                           textToShow);
    }
}