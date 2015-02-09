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
    
    public static int[] cityFillRGB;
    public static int[] appleCartRGB;
    public static int[] constructionRGB;
    public static int[] grassRGB;
    public static int[] treeRGB;
    public static int[] riverRGB;
    
    public static int[] streetRGB;
    
    public static int[] houseRGB;
    public static int[] greenHouseRGB;
    public static int[] yellowHouseRGB;
    
    public static int[] onFireHouseRGB;
    public static int[] greenOnFireRGB;
    public static int[] yellowOnFireRGB;
    
    public static int[] fireStationRGB;
    
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
            appleCartRGB = tileSet.getRGB(TILE_SIZE*5, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            riverRGB = tileSet.getRGB(TILE_SIZE*6, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            grassRGB = tileSet.getRGB(TILE_SIZE*7, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            treeRGB = tileSet.getRGB(TILE_SIZE*8, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            constructionRGB = tileSet.getRGB(TILE_SIZE*9, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            greenHouseRGB = tileSet.getRGB(TILE_SIZE*10, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            greenOnFireRGB = tileSet.getRGB(TILE_SIZE*11, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            yellowHouseRGB = tileSet.getRGB(TILE_SIZE*12, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
            yellowOnFireRGB = tileSet.getRGB(TILE_SIZE*13, 0, TILE_SIZE, TILE_SIZE, null, 0, TILE_SIZE);
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
                        
                    case APPLE_CART:
                        tileRGB = appleCartRGB;
                        break;
                        
                    case CONSTRUCTION:
                        tileRGB = constructionRGB;
                        break;
                        
                    case GRASS:
                        tileRGB = grassRGB;
                        break;
                        
                    case TREE:
                        tileRGB = treeRGB;
                        break;
                        
                    case RIVER:
                        tileRGB = riverRGB;
                        break;
                
                    case GREEN_HOUSE:
                        tileRGB = greenHouseRGB;
                        break;

                    case YELLOW_HOUSE:
                        tileRGB = yellowHouseRGB;
                        break;

                    case GREEN_HOUSE_ON_FIRE:
                        tileRGB = greenOnFireRGB;
                        break;

                    case YELLOW_HOUSE_ON_FIRE:
                        tileRGB = yellowOnFireRGB;
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