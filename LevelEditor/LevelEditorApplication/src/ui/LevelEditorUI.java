/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui;

import java.awt.Point;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.io.File;
import javax.swing.JFileChooser;
import javax.swing.JSpinner;
import javax.swing.SpinnerModel;
import javax.swing.SpinnerNumberModel;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import level.model.*;
import ui.model.ArsonPathTableModel;
import ui.model.NPCCueTableModel;

/**
 *
 * @author asheehan
 */
public class LevelEditorUI extends javax.swing.JFrame implements ArsonPathTableModel.TimeChangeDelegate{
    private Level level;

    /**
     * Creates new form LevelEditorUI
     */
    public LevelEditorUI() {
        initComponents();
        
        level = new Level();
        
        SpinnerModel widthSpinnerModel = new SpinnerNumberModel(Level.MIN_WIDTH,
                Level.MIN_WIDTH, Level.MAX_WIDTH, 1);
        widthSpinner.setModel(widthSpinnerModel);
        widthSpinner.addChangeListener(new WidthSpinnerChangeListener());
        
        SpinnerModel heightSpinnerModel = new SpinnerNumberModel(Level.MIN_HEIGHT,
                Level.MIN_HEIGHT, Level.MAX_HEIGHT, 1);
        heightSpinner.setModel(heightSpinnerModel);
        heightSpinner.addChangeListener(new HeightSpinnerChangeListener());
        
        SpinnerModel durationSpinnerModel = new SpinnerNumberModel(Level.DEFAULT_DURATION,
            Level.MIN_DURATION, Level.MAX_DURATION, 1);
        durationSpinner.setModel(durationSpinnerModel);
        durationSpinner.addChangeListener(new DurationSpinnerChangeListener());
        
        SpinnerModel winSpinnerModel = new SpinnerNumberModel(Level.DEFAULT_PERCENT,
            Level.MIN_PERCENT, Level.MAX_PERCENT, 1);
        winSpinner.setModel(winSpinnerModel);
        winSpinner.addChangeListener(new WinSpinnerChangeListener());
        
        SpinnerModel paddingSpinnerModel = new SpinnerNumberModel(0, 0, 100, 1);
        paddingSpinner.setModel(paddingSpinnerModel);
        paddingSpinner.addChangeListener(new PaddingSpinnerChangeListener());
        
        mapPanel.updateMap(level);
        mapPanel.addMouseListener(new PanelMouseListener());
        
        tileButtonGroup.add(streetRadioButton);
        tileButtonGroup.add(houseRadioButton);
        tileButtonGroup.add(fireStationRadioButton);
        tileButtonGroup.add(cityFillRadioButton);
        tileButtonGroup.add(startFireRadioButton);
        
        npcCueTable.addMouseListener(new NPCCueTableMouseListener());
        //Creates list models
        refreshList();
        
        final JFileChooser fc = new JFileChooser();
        saveButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                int returnVal = fc.showSaveDialog(LevelEditorUI.this);

                if (returnVal == JFileChooser.APPROVE_OPTION) {
                    File file = fc.getSelectedFile();
                    level.writeToFile(file);
                }
            }
        });
        
        loadButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                int returnVal = fc.showOpenDialog(LevelEditorUI.this);

                if (returnVal == JFileChooser.APPROVE_OPTION) {
                    File file = fc.getSelectedFile();
                    level = Level.loadFromFile(file);
                    mapPanel.updateMap(level);
                    winSpinner.setValue(level.getWinPercent());
                    durationSpinner.setValue(level.getDurationSeconds());
                    widthSpinner.setValue(level.getWidth());
                    heightSpinner.setValue(level.getHeight());
                    
                    refreshList();
                }
            }
        });
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        tileButtonGroup = new javax.swing.ButtonGroup();
        mapScrollPane = new javax.swing.JScrollPane();
        mapPanel = new ui.MapPanel();
        widthSpinner = new javax.swing.JSpinner();
        heightSpinner = new javax.swing.JSpinner();
        widthLabel = new javax.swing.JLabel();
        heightLabel = new javax.swing.JLabel();
        durationLabel = new javax.swing.JLabel();
        percentLabel = new javax.swing.JLabel();
        durationSpinner = new javax.swing.JSpinner();
        winSpinner = new javax.swing.JSpinner();
        streetRadioButton = new javax.swing.JRadioButton();
        houseRadioButton = new javax.swing.JRadioButton();
        fireStationRadioButton = new javax.swing.JRadioButton();
        cityFillRadioButton = new javax.swing.JRadioButton();
        startFireRadioButton = new javax.swing.JRadioButton();
        fireListContainer = new javax.swing.JScrollPane();
        fireTable = new javax.swing.JTable();
        saveButton = new javax.swing.JButton();
        loadButton = new javax.swing.JButton();
        paddingSpinner = new javax.swing.JSpinner();
        paddingLabel = new javax.swing.JLabel();
        npcCueContainer = new javax.swing.JScrollPane();
        npcCueTable = new javax.swing.JTable();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setMaximumSize(new java.awt.Dimension(10000, 10000));
        setPreferredSize(new java.awt.Dimension(800, 600));

        mapScrollPane.setOpaque(false);
        mapScrollPane.setPreferredSize(new java.awt.Dimension(256, 256));
        mapScrollPane.setRequestFocusEnabled(false);
        mapScrollPane.setSize(new java.awt.Dimension(256, 256));

        mapPanel.setBackground(new java.awt.Color(51, 51, 255));
        mapPanel.setPreferredSize(new java.awt.Dimension(1024, 1024));

        javax.swing.GroupLayout mapPanelLayout = new javax.swing.GroupLayout(mapPanel);
        mapPanel.setLayout(mapPanelLayout);
        mapPanelLayout.setHorizontalGroup(
            mapPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 1024, Short.MAX_VALUE)
        );
        mapPanelLayout.setVerticalGroup(
            mapPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 1024, Short.MAX_VALUE)
        );

        mapScrollPane.setViewportView(mapPanel);

        widthLabel.setText("Width");

        heightLabel.setText("Height");

        durationLabel.setText("Level Duration");

        percentLabel.setText("Percent to win");

        streetRadioButton.setSelected(true);
        streetRadioButton.setText("Street");

        houseRadioButton.setText("House");

        fireStationRadioButton.setText("Fire Station");

        cityFillRadioButton.setText("Fill");

        startFireRadioButton.setText("Start a Fire");

        fireTable.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null}
            },
            new String [] {
                "Title 1", "Title 2", "Title 3", "Title 4"
            }
        ));
        fireListContainer.setViewportView(fireTable);

        saveButton.setText("Save");

        loadButton.setText("Load");

        paddingLabel.setText("Padding");

        npcCueTable.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null}
            },
            new String [] {
                "Title 1", "Title 2", "Title 3", "Title 4"
            }
        ));
        npcCueContainer.setViewportView(npcCueTable);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(mapScrollPane, javax.swing.GroupLayout.DEFAULT_SIZE, 512, Short.MAX_VALUE)
                    .addComponent(npcCueContainer))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                        .addComponent(streetRadioButton)
                        .addComponent(houseRadioButton)
                        .addComponent(fireStationRadioButton)
                        .addComponent(cityFillRadioButton)
                        .addComponent(startFireRadioButton)
                        .addComponent(fireListContainer, javax.swing.GroupLayout.PREFERRED_SIZE, 0, Short.MAX_VALUE)
                        .addGroup(layout.createSequentialGroup()
                            .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                .addComponent(widthLabel)
                                .addComponent(heightLabel)
                                .addComponent(durationLabel)
                                .addComponent(percentLabel)
                                .addComponent(paddingLabel))
                            .addGap(27, 27, 27)
                            .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                                .addComponent(paddingSpinner)
                                .addComponent(widthSpinner, javax.swing.GroupLayout.DEFAULT_SIZE, 64, Short.MAX_VALUE)
                                .addComponent(heightSpinner)
                                .addComponent(durationSpinner)
                                .addComponent(winSpinner))))
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                        .addComponent(loadButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(saveButton)))
                .addContainerGap(312, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(widthLabel)
                            .addComponent(widthSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(heightLabel)
                            .addComponent(heightSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(durationSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(durationLabel))
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(winSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                            .addComponent(percentLabel))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(paddingLabel)
                            .addComponent(paddingSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 18, Short.MAX_VALUE)
                        .addComponent(streetRadioButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(houseRadioButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(fireStationRadioButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(cityFillRadioButton)
                        .addGap(29, 29, 29)
                        .addComponent(startFireRadioButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(fireListContainer, javax.swing.GroupLayout.PREFERRED_SIZE, 184, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(mapScrollPane, javax.swing.GroupLayout.PREFERRED_SIZE, 512, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(saveButton)
                        .addComponent(loadButton))
                    .addComponent(npcCueContainer, javax.swing.GroupLayout.PREFERRED_SIZE, 160, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addContainerGap(324, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JRadioButton cityFillRadioButton;
    private javax.swing.JLabel durationLabel;
    private javax.swing.JSpinner durationSpinner;
    private javax.swing.JScrollPane fireListContainer;
    private javax.swing.JRadioButton fireStationRadioButton;
    private javax.swing.JTable fireTable;
    private javax.swing.JLabel heightLabel;
    private javax.swing.JSpinner heightSpinner;
    private javax.swing.JRadioButton houseRadioButton;
    private javax.swing.JButton loadButton;
    private ui.MapPanel mapPanel;
    private javax.swing.JScrollPane mapScrollPane;
    private javax.swing.JScrollPane npcCueContainer;
    private javax.swing.JTable npcCueTable;
    private javax.swing.JLabel paddingLabel;
    private javax.swing.JSpinner paddingSpinner;
    private javax.swing.JLabel percentLabel;
    private javax.swing.JButton saveButton;
    private javax.swing.JRadioButton startFireRadioButton;
    private javax.swing.JRadioButton streetRadioButton;
    private javax.swing.ButtonGroup tileButtonGroup;
    private javax.swing.JLabel widthLabel;
    private javax.swing.JSpinner widthSpinner;
    private javax.swing.JSpinner winSpinner;
    // End of variables declaration//GEN-END:variables
    
    int prevX = -1;
    int prevY = -1;
    
    private void paintMouseEvent(MouseEvent e){
        int x = (int)Math.floor(e.getX() / (float)MapPanel.TILE_SIZE);
        int y = (int)Math.floor(e.getY() / (float)MapPanel.TILE_SIZE);

        if(x != prevX || y != prevY){
            prevX = x;
            prevY = y;

            Tile toSet = null;

            if(streetRadioButton.isSelected()){
                toSet = Tile.STREET;
            }else if(houseRadioButton.isSelected()){
                toSet = Tile.HOUSE;
            }else if(fireStationRadioButton.isSelected()){
                toSet = Tile.FIRE_STATION;
            }else if(cityFillRadioButton.isSelected()){
                toSet = Tile.CITY_FILL;
            }else if(startFireRadioButton.isSelected()){
                toSet = Tile.HOUSE_ON_FIRE;
            }

            if(toSet != null){
                if(x < level.getWidth() && y < level.getHeight()){
                    if(toSet == Tile.HOUSE_ON_FIRE){
                        level.getArsonPath().addStep(x, y);
                    }else if(level.getMap().getTileMap()[x][y] == Tile.HOUSE_ON_FIRE){
                        level.getArsonPath().removeStep(x, y);
                    }
                    
                    level.getMap().setTile(x, y, toSet);
                    mapPanel.updateMap(level);
                    refreshList();
                }
            }
        }
    }
    
    private void refreshList(){
        fireTable.setModel(new ArsonPathTableModel(level.getArsonPath().getSteps(), this));
        npcCueTable.setModel(new NPCCueTableModel(level.getNPCCues()));
    }

    @Override
    public void setStepTime(Point location, int time) {
        level.getArsonPath().setStepTime(location.x, location.y, time);
        refreshList();
    }
    
    private class PanelMouseListener implements MouseListener{
        private MapPanelMouseMoveListener moveListener = null;
        @Override
        public void mouseClicked(MouseEvent e) {
            paintMouseEvent(e);
            prevX = -1;
            prevY = -1;
        }

        @Override
        public void mousePressed(MouseEvent e) {
            moveListener = new MapPanelMouseMoveListener();
            mapPanel.addMouseMotionListener(moveListener);
        }

        @Override
        public void mouseReleased(MouseEvent e) {
            if(moveListener != null){
                mapPanel.removeMouseMotionListener(moveListener);
                moveListener = null;
            }
            
            paintMouseEvent(e);
            prevX = -1;
            prevY = -1;
        }

        @Override
        public void mouseEntered(MouseEvent e) {}

        @Override
        public void mouseExited(MouseEvent e) {
            if(moveListener != null){
                mapPanel.removeMouseMotionListener(moveListener);
                moveListener = null;
            }
            prevX = -1;
            prevY = -1;
        }
    }
    
    private class MapPanelMouseMoveListener implements MouseMotionListener{
        @Override
        public void mouseDragged(MouseEvent e) {
            paintMouseEvent(e);
        }

        @Override
        public void mouseMoved(MouseEvent e) {}
    }
    
    private class WidthSpinnerChangeListener implements ChangeListener{
        @Override
        public void stateChanged(ChangeEvent e) {
            JSpinner source = (JSpinner)e.getSource();
            int newWidth = (int)source.getValue();
            level.resize(newWidth, level.getHeight());
            mapPanel.updateMap(level);
            refreshList();
        }
    }
    
    private class HeightSpinnerChangeListener implements ChangeListener{
        @Override
        public void stateChanged(ChangeEvent e) {
            JSpinner source = (JSpinner)e.getSource();
            int newHeight = (int)source.getValue();
            level.resize(level.getWidth(), newHeight);
            mapPanel.updateMap(level);
            refreshList();
        }
    }
    
    private class DurationSpinnerChangeListener implements ChangeListener{
        @Override
        public void stateChanged(ChangeEvent e) {
            JSpinner source = (JSpinner)e.getSource();
            int newDuration = (int)source.getValue();
            level.setDurationSeconds(newDuration);
            refreshList();;
        }
    }
    
    private class WinSpinnerChangeListener implements ChangeListener{
        @Override
        public void stateChanged(ChangeEvent e) {
            JSpinner source = (JSpinner)e.getSource();
            int newWin = (int)source.getValue();
            level.setWinPercent(newWin);
        }
    }
    
    private class PaddingSpinnerChangeListener implements ChangeListener{
        @Override
        public void stateChanged(ChangeEvent e) {
            JSpinner source = (JSpinner)e.getSource();
            int newPadding = (int)source.getValue();
            level.setFillPadding(newPadding);
        }
    }
    
    private class NPCCueTableMouseListener implements MouseListener{
        @Override
        public void mouseClicked(MouseEvent e) {
            int rowClicked = npcCueTable.rowAtPoint(e.getPoint());
            if(rowClicked >= level.getNPCCues().size()){
                NPCCue cue = new NPCCue(0f, "m", "Don't go into the light");
                level.addNPCCue(cue);
                refreshList();
            }
        }

        @Override
        public void mousePressed(MouseEvent e) {
        }

        @Override
        public void mouseReleased(MouseEvent e) {
        }

        @Override
        public void mouseEntered(MouseEvent e) {
            System.out.println("Entered NPC table");
        }

        @Override
        public void mouseExited(MouseEvent e) {
        }
    }
}