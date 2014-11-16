/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui.renderer;

import java.awt.Component;
import java.awt.Point;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JSpinner;
import javax.swing.JTextField;
import javax.swing.ListCellRenderer;
import javax.swing.SpinnerModel;
import javax.swing.SpinnerNumberModel;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import level.model.ArsonStep;
import level.model.Level;

/**
 *
 * @author asheehan
 */
public class ArsonListItemRenderer extends JPanel implements ListCellRenderer<ArsonStep>{
    private JSpinner timeEntry;
    private JLabel locationLabel;
    private Level level;
    
    public ArsonListItemRenderer(Level level){
        this.level = level;
    }

    @Override
    public Component getListCellRendererComponent(JList<? extends ArsonStep> list, 
            ArsonStep value, 
            int index, 
            boolean isSelected, 
            boolean cellHasFocus) {
        String locationText = String.format("%3d, %3d     ", 
                value.getLocation().x, 
                value.getLocation().y);
        if(locationLabel == null){
            locationLabel = new JLabel(locationText);
            add(locationLabel);
        }else{
            locationLabel.setText(locationText);
        }
        
        String timeText = String.format("%3ds", value.getTime());
        SpinnerModel timeSpinnerModel = new SpinnerNumberModel(value.getTime(), 
                0, level.getDurationSeconds(), 1);
        if(timeEntry == null){
            timeEntry = new JSpinner(timeSpinnerModel);
            add(timeEntry);
        }else{
            timeEntry.setModel(timeSpinnerModel);
        }
        
        timeEntry.addChangeListener(new TimeSpinnerChangeListener(value));
        
        return this;
    }
    
    private class TimeSpinnerChangeListener implements ChangeListener{
        private ArsonStep step;
        public TimeSpinnerChangeListener(ArsonStep step){
            this.step = step;
        }
        
        @Override
        public void stateChanged(ChangeEvent e) {
            JSpinner source = (JSpinner)e.getSource();
            int newTime = (int)source.getValue();
        }
    }
}