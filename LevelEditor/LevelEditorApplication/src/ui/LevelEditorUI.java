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
import javax.swing.DefaultCellEditor;
import javax.swing.JComboBox;
import javax.swing.JFileChooser;
import javax.swing.JSpinner;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.SpinnerModel;
import javax.swing.SpinnerNumberModel;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.TableColumn;
import level.model.*;
import ui.model.ArsonPathTableModel;
import ui.model.NPCCueTableModel;
import ui.model.TileListModel;
import ui.renderer.TilePaletteRenderer;

/**
 *
 * @author asheehan
 */
public class LevelEditorUI extends javax.swing.JFrame implements ArsonPathTableModel.ArsonStepChangeListener{
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
        
        mapScrollPane.setLayout(null);
        mapPanel.updateMap(level);
        mapPanel.addMouseListener(new PanelMouseListener());
        
        fireTable.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        fireTable.getSelectionModel().addListSelectionListener(new FireListSelectionListener());
                
        npcCueTable.addMouseListener(new NPCCueTableMouseListener());
        npcCueTable.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        npcCueTable.setAutoResizeMode(JTable.AUTO_RESIZE_LAST_COLUMN);
        
        //Creates list models
        refreshList();
        
        paletteList.setModel(new TileListModel());
        paletteList.setCellRenderer(new TilePaletteRenderer());
        paletteList.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        
        npcDeleteButton.addActionListener(new DeleteNPCClickListener());
        
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
        fireListContainer = new javax.swing.JScrollPane();
        fireTable = new javax.swing.JTable();
        saveButton = new javax.swing.JButton();
        loadButton = new javax.swing.JButton();
        paddingSpinner = new javax.swing.JSpinner();
        paddingLabel = new javax.swing.JLabel();
        npcCueContainer = new javax.swing.JScrollPane();
        npcCueTable = new javax.swing.JTable();
        npcDeleteButton = new javax.swing.JButton();
        paletteScrollPane = new javax.swing.JScrollPane();
        paletteList = new javax.swing.JList();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        mapScrollPane.setBackground(new java.awt.Color(204, 204, 204));
        mapScrollPane.setHorizontalScrollBarPolicy(javax.swing.ScrollPaneConstants.HORIZONTAL_SCROLLBAR_ALWAYS);
        mapScrollPane.setVerticalScrollBarPolicy(javax.swing.ScrollPaneConstants.VERTICAL_SCROLLBAR_ALWAYS);
        mapScrollPane.setPreferredSize(new java.awt.Dimension(256, 256));
        mapScrollPane.setRequestFocusEnabled(false);

        mapPanel.setBackground(new java.awt.Color(255, 255, 255));
        mapPanel.setMaximumSize(new java.awt.Dimension(10000, 10000));

        javax.swing.GroupLayout mapPanelLayout = new javax.swing.GroupLayout(mapPanel);
        mapPanel.setLayout(mapPanelLayout);
        mapPanelLayout.setHorizontalGroup(
            mapPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 435, Short.MAX_VALUE)
        );
        mapPanelLayout.setVerticalGroup(
            mapPanelLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 311, Short.MAX_VALUE)
        );

        mapScrollPane.setViewportView(mapPanel);

        widthLabel.setText("Width");

        heightLabel.setText("Height");

        durationLabel.setText("Level Duration");

        percentLabel.setText("Percent to win");

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
        npcCueTable.setAutoResizeMode(javax.swing.JTable.AUTO_RESIZE_OFF);
        npcCueContainer.setViewportView(npcCueTable);

        npcDeleteButton.setText("Delete selected mesage");

        paletteList.setModel(new javax.swing.AbstractListModel() {
            String[] strings = { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" };
            public int getSize() { return strings.length; }
            public Object getElementAt(int i) { return strings[i]; }
        });
        paletteScrollPane.setViewportView(paletteList);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                    .addGroup(javax.swing.GroupLayout.Alignment.LEADING, layout.createSequentialGroup()
                        .addContainerGap()
                        .addComponent(npcCueContainer))
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addGroup(layout.createSequentialGroup()
                                .addContainerGap()
                                .addComponent(mapScrollPane, javax.swing.GroupLayout.PREFERRED_SIZE, 454, javax.swing.GroupLayout.PREFERRED_SIZE))
                            .addComponent(npcDeleteButton))
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                        .addComponent(paletteScrollPane, javax.swing.GroupLayout.PREFERRED_SIZE, 182, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                    .addComponent(fireListContainer, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.PREFERRED_SIZE, 182, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(javax.swing.GroupLayout.Alignment.LEADING, layout.createSequentialGroup()
                        .addComponent(paddingLabel)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(paddingSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, 64, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(percentLabel)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(winSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, 64, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(durationLabel)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(durationSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, 64, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(heightLabel)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(heightSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, 64, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(widthLabel)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(widthSpinner, javax.swing.GroupLayout.PREFERRED_SIZE, 64, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(loadButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(saveButton)))
                .addContainerGap(51, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addGroup(layout.createSequentialGroup()
                                .addComponent(paletteScrollPane)
                                .addGap(3, 3, 3))
                            .addComponent(mapScrollPane, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.PREFERRED_SIZE, 330, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addGap(9, 9, 9)
                        .addComponent(npcCueContainer, javax.swing.GroupLayout.PREFERRED_SIZE, 151, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
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
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(fireListContainer, javax.swing.GroupLayout.PREFERRED_SIZE, 330, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGap(18, 18, 18)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(saveButton)
                            .addComponent(loadButton))
                        .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                    .addGroup(layout.createSequentialGroup()
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 18, Short.MAX_VALUE)
                        .addComponent(npcDeleteButton)
                        .addGap(465, 465, 465))))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JLabel durationLabel;
    private javax.swing.JSpinner durationSpinner;
    private javax.swing.JScrollPane fireListContainer;
    private javax.swing.JTable fireTable;
    private javax.swing.JLabel heightLabel;
    private javax.swing.JSpinner heightSpinner;
    private javax.swing.JButton loadButton;
    private ui.MapPanel mapPanel;
    private javax.swing.JScrollPane mapScrollPane;
    private javax.swing.JScrollPane npcCueContainer;
    private javax.swing.JTable npcCueTable;
    private javax.swing.JButton npcDeleteButton;
    private javax.swing.JLabel paddingLabel;
    private javax.swing.JSpinner paddingSpinner;
    private javax.swing.JList paletteList;
    private javax.swing.JScrollPane paletteScrollPane;
    private javax.swing.JLabel percentLabel;
    private javax.swing.JButton saveButton;
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

            int index = paletteList.getSelectedIndex();
            Tile toSet = null;
            if(index >= 0){
                toSet = (Tile)paletteList.getModel().getElementAt(index);
            }

            if(toSet != null){
                if(x < level.getWidth() && y < level.getHeight()){
                    Tile currentTile = level.getMap().getTileMap()[x][y];
                    
                    if(toSet == Tile.HOUSE_ON_FIRE){
                        level.getArsonPath().addStep(x, y);
                    }else if(currentTile == Tile.HOUSE_ON_FIRE || 
                            currentTile == Tile.GREEN_HOUSE_ON_FIRE ||
                            currentTile == Tile.YELLOW_HOUSE_ON_FIRE){
                        level.getArsonPath().removeStep(x, y);
                    }
                    
                    if((currentTile == Tile.GREEN_HOUSE || currentTile == Tile.GREEN_HOUSE_ON_FIRE) &&
                            toSet == Tile.HOUSE_ON_FIRE){
                        toSet = Tile.GREEN_HOUSE_ON_FIRE;
                    }else if((currentTile == Tile.YELLOW_HOUSE || currentTile == Tile.YELLOW_HOUSE_ON_FIRE) &&
                            toSet == Tile.HOUSE_ON_FIRE){
                        toSet = Tile.YELLOW_HOUSE_ON_FIRE;
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
        
        TableColumn column0 = npcCueTable.getColumnModel().getColumn(0);
        column0.setMinWidth(100);
        column0.setMaxWidth(100);
        
        //Set a combo box for npc options: fire chief, mayor, and police chief
        TableColumn column1 = npcCueTable.getColumnModel().getColumn(1);
        column1.setMinWidth(100);
        column1.setMaxWidth(100);
        JComboBox npcComboBox = new JComboBox();
        npcComboBox.addItem("f");
        npcComboBox.addItem("m");
        npcComboBox.addItem("p");
        column1.setCellEditor(new DefaultCellEditor(npcComboBox));
        
        TableColumn column2 = npcCueTable.getColumnModel().getColumn(2);
        column2.setMaxWidth(1000);
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
    
    private class FireListSelectionListener implements ListSelectionListener{
        @Override
        public void valueChanged(ListSelectionEvent e) {
            int row = fireTable.getSelectedRow();
            
            Point firePos = null;
            
            if(row >= 0){
                ArsonStep step = level.getArsonPath().getSteps().get(row);
                firePos = step.getLocation();
            }
            
            mapPanel.setSelectedPoint(firePos);
        }
    }
    
    private class NPCCueTableMouseListener implements MouseListener{
        @Override
        public void mouseClicked(MouseEvent e) {
            int rowClicked = npcCueTable.rowAtPoint(e.getPoint());
            if(rowClicked >= level.getNPCCues().size()){
                NPCCue cue = new NPCCue(0f, "m", "");
                level.addNPCCue(cue);
                refreshList();
            }
        }
        @Override
        public void mousePressed(MouseEvent e) {}
        @Override
        public void mouseReleased(MouseEvent e) {}
        @Override
        public void mouseEntered(MouseEvent e) {}
        @Override
        public void mouseExited(MouseEvent e) {}
    }
    
    private class DeleteNPCClickListener implements ActionListener{
        @Override
        public void actionPerformed(ActionEvent e) {
            int selectedNPC = npcCueTable.getSelectedRow();
            
            if(selectedNPC >= 0){
                level.getNPCCues().remove(selectedNPC);
                refreshList();
            }
        }
    }
}