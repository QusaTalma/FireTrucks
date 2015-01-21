/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;
import javax.swing.JPanel;
import javax.swing.Scrollable;
import level.model.Level;
import level.model.Tile;

/**
 *
 * @author asheehan
 */
public class MapPanel extends JPanel implements Scrollable{
    public static final int TILE_SIZE = 16;
    private static final String TILE_SET_TEXTURE_FILE = "./Resources/FiretruckCityTextures.jpg";
    private BufferedImage tileSet;
    
    private int[] cityFillRGB;
    private int[] streetRGB;
    private int[] houseRGB;
    private int[] onFireHouseRGB;
    private int[] fireStationRGB;
    
    private BufferedImage mapImage;
    
    private Point selectedPoint = null; 
    
    public MapPanel(){
        super();
        try{
            tileSet = ImageIO.read(new File(TILE_SET_TEXTURE_FILE));
        }catch(IOException e){
            
        }
        
        if(tileSet != null){
            cityFillRGB = tileSet.getRGB(0, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            streetRGB = tileSet.getRGB(TILE_SIZE, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            houseRGB = tileSet.getRGB(TILE_SIZE*2, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            onFireHouseRGB = tileSet.getRGB(TILE_SIZE*3, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            fireStationRGB = tileSet.getRGB(TILE_SIZE*4, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
        }
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g); //To change body of generated methods, choose Tools | Templates.
        if(mapImage != null){
            g.drawImage(mapImage, 0, 0, null);
        }
        
        if(selectedPoint != null){
            g.setColor(Color.RED);
            g.fillOval(selectedPoint.x*TILE_SIZE,
                    selectedPoint.y*TILE_SIZE,
                    TILE_SIZE, 
                    TILE_SIZE);
        }
    }
    
    public void setSelectedPoint(Point point){
        this.selectedPoint = point;
        repaint();
    }
    
    public void updateMap(Level level){
        int width = level.getWidth();
        int height = level.getHeight();
        
        resizePanel(width, height);
        
        updateMapImage(level);
    }
    
    protected void resizePanel(int width, int height){
        int scaledWidth = width*TILE_SIZE;
        int scaledHeight = height*TILE_SIZE;
        setSize(scaledWidth, scaledHeight);
        setPreferredSize(new Dimension(scaledWidth, scaledHeight));
        repaint();
    }
    
    protected void updateMapImage(Level level){
        final int mapImageSize = 1024;
        BufferedImage newMapImage = new BufferedImage(mapImageSize, mapImageSize, BufferedImage.TYPE_INT_RGB);
        Graphics g = newMapImage.createGraphics();
        g.setColor(Color.BLUE);
        g.fillRect(0, 0, mapImageSize, mapImageSize);
        g.dispose();
        
        Tile[][] tileMap = level.getMap().getTileMap();
        
        for(int x=0; x<level.getWidth(); x++){
            for(int y=0; y<level.getHeight(); y++){
                Tile tile = tileMap[x][y];
                int[] tileRGB;
                switch(tile){
                    case STREET:
                        tileRGB = streetRGB;
                        break;
                        
                    case HOUSE:
                        tileRGB = houseRGB;
                        break;
                        
                    case HOUSE_ON_FIRE:
                        tileRGB = onFireHouseRGB;
                        break;
                        
                    case FIRE_STATION:
                        tileRGB = fireStationRGB;
                        break;
                        
                    default:
                    case CITY_FILL:
                        tileRGB = cityFillRGB;
                        break;
                }
                
                newMapImage.setRGB(x*TILE_SIZE, y*TILE_SIZE, TILE_SIZE, TILE_SIZE, tileRGB, 0, TILE_SIZE);
            }
        }
        
        mapImage = newMapImage;
    }

    @Override
    public Dimension getPreferredScrollableViewportSize() {
        return new Dimension(256,256);
    }

    @Override
    public int getScrollableUnitIncrement(Rectangle visibleRect, int orientation, int direction) {
        return TILE_SIZE;
    }

    @Override
    public int getScrollableBlockIncrement(Rectangle visibleRect, int orientation, int direction) {
        return TILE_SIZE;
    }

    @Override
    public boolean getScrollableTracksViewportWidth() {
        return false;
    }

    @Override
    public boolean getScrollableTracksViewportHeight() {
        return false;
    }
}