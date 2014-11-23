/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui;

import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;
import javax.swing.JPanel;

/**
 *
 * @author asheehan
 */
public class MapPanel extends JPanel{
    private static final int TILE_SIZE = 16;
    private static final String TILE_SET_TEXTURE_FILE = "./Resources/FiretruckCityTextures.jpg";
    private BufferedImage tileSet;
    
    private int[] cityFillRGB;
    private int[] streetRGB;
    private int[] houseRGB;
    private int[] onFireHouseRGB;
    private int[] fireStationRGB;
    
    private BufferedImage mapImage;
    
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
        
        mapImage = new BufferedImage(1024, 1024, BufferedImage.TYPE_INT_RGB);
        
        int[] tileToSet = cityFillRGB;
        
        for(int x=0; x<1024; x+=16){
            for(int y=0; y<1024; y+=16){
                mapImage.setRGB(x, y, TILE_SIZE, TILE_SIZE, tileToSet, 0, TILE_SIZE);
            }
        }
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g); //To change body of generated methods, choose Tools | Templates.
        g.drawImage(mapImage, 0, 0, null);
    }
}
