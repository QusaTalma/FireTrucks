/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui.model;

import java.awt.Point;
import java.util.List;
import javax.swing.table.AbstractTableModel;
import level.model.ArsonStep;

/**
 *
 * @author asheehan
 */
public class ArsonPathTableModel extends AbstractTableModel{
    private List<ArsonStep> steps;
    private TimeChangeDelegate timeChangeDelegate;
    
    public ArsonPathTableModel(List<ArsonStep> steps, TimeChangeDelegate delegate){
        this.steps = steps;
        this.timeChangeDelegate = delegate;
    }

    @Override
    public String getColumnName(int column) {
        switch(column){
            case 0:
                return "X";
                
            case 1:
                return "Y";
                
            case 2:
                return "Time";
        }
        
        return "Fnord";
    }

    @Override
    public int getRowCount() {
        return steps.size();
    }

    @Override
    public int getColumnCount() {
        return 3;
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        ArsonStep step = steps.get(rowIndex);
        switch(columnIndex){
            case 0:
                return step.getLocation().x;
                
            case 1:
                return step.getLocation().y;
                
            case 2:
                return step.getTime();
        }
        
        return "NULL!";
    }

    @Override
    public boolean isCellEditable(int rowIndex, int columnIndex) {
        return columnIndex == 2;
    }

    @Override
    public void setValueAt(Object aValue, int rowIndex, int columnIndex) {
        ArsonStep step = steps.get(rowIndex);
        if(columnIndex == 2){
            int time = step.getTime();
            try{
                time = Integer.parseInt(aValue.toString());
            }catch(NumberFormatException e){}
            
            if(timeChangeDelegate != null){
                timeChangeDelegate.setStepTime(step.getLocation(), time);
            }
        }
    }
    
    public static interface TimeChangeDelegate{
        public void setStepTime(Point location, int time);
    }
}