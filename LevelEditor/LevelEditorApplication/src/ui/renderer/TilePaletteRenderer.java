/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui.renderer;

import java.awt.Color;
import java.awt.Component;
import java.awt.image.BufferedImage;
import javax.swing.Icon;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.ListCellRenderer;
import level.model.Tile;
import ui.MapPanel;
import static ui.MapPanel.appleCartRGB;
import static ui.MapPanel.cityFillRGB;
import static ui.MapPanel.constructionRGB;
import static ui.MapPanel.fireStationRGB;
import static ui.MapPanel.grassRGB;
import static ui.MapPanel.houseRGB;
import static ui.MapPanel.onFireHouseRGB;
import static ui.MapPanel.riverRGB;
import static ui.MapPanel.streetRGB;
import static ui.MapPanel.treeRGB;

/**
 *
 * @author asheehan
 */
public class TilePaletteRenderer extends JLabel implements ListCellRenderer<Tile>{

    @Override
    public Component getListCellRendererComponent(JList<? extends Tile> list, Tile value, int index, boolean isSelected, boolean cellHasFocus) {
        setText(value.getLabel());
        
        BufferedImage iconImage = new BufferedImage(MapPanel.TILE_SIZE, MapPanel.TILE_SIZE, BufferedImage.TYPE_INT_RGB);
        
        int[] tileRGB;
        switch(value){
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
                tileRGB = MapPanel.greenHouseRGB;
                break;
                
            case YELLOW_HOUSE:
                tileRGB = MapPanel.yellowHouseRGB;
                break;

            default:
            case CITY_FILL:
                tileRGB = cityFillRGB;
                break;
        }
        
        iconImage.setRGB(0, 0, MapPanel.TILE_SIZE, MapPanel.TILE_SIZE, tileRGB, 0, MapPanel.TILE_SIZE);
        
        setIcon(new ImageIcon(iconImage));
        
        setOpaque(true);
        if(isSelected){
            setBackground(Color.BLUE);
        }else{
            setBackground(Color.WHITE);
        }
        
        return this;
    }   
}