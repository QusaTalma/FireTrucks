/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui.model;

import level.model.NPCCue;
import java.util.List;
import javax.swing.table.AbstractTableModel;

/**
 *
 * @author asheehan
 */
public class NPCCueTableModel extends AbstractTableModel{
    private List<NPCCue> cues;
    
    public NPCCueTableModel(List<NPCCue> cues){
        this.cues = cues;
    }

    @Override
    public String getColumnName(int column) {
        switch(column){
            case 0:
                return "Time";
                
            case 1:
                return "Which NPC";
                
            case 2:
                return "Message";
        }
        
        //Code that should never run should always yield amusing results
        return "Antidisestablishmentarianmentalismophobiagorophonicalifragilisticexpialadosidoremifasolatidonut";
    }

    @Override
    public int getRowCount() {
        return cues.size() + 1;
    }

    @Override
    public int getColumnCount() {
        return 3;
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        if(rowIndex == cues.size()){
            return "";
        }
        
        NPCCue cue = cues.get(rowIndex);
        switch(columnIndex){
            case 0:
                return cue.getTimeToShow();
            case 1:
                return cue.getNpcToShow();
                
            case 2:
                return cue.getTextToShow();
        }
        
        return "Balls!";
    }

    @Override
    public void setValueAt(Object aValue, int rowIndex, int columnIndex) {
        NPCCue cue = cues.get(rowIndex);
        switch(columnIndex){
            case 0:
                float time = cue.getTimeToShow();
                try{
                    time = Float.parseFloat(aValue.toString());
                }catch(NumberFormatException e){}
                cue.setTimeToShow(time);
            break;
                
            case 1:
                String npc = aValue.toString();
                cue.setNPCToShow(npc);
                break;
                
            case 2:
                String message = aValue.toString();
                cue.setTextToShow(message);
                break;
        }
    }

    @Override
    public boolean isCellEditable(int rowIndex, int columnIndex) {
        return true;
    }
}