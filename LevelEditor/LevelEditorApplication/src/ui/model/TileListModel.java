/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui.model;

import java.util.ArrayList;
import java.util.List;
import javax.swing.AbstractListModel;
import level.model.Tile;

/**
 *
 * @author asheehan
 */
public class TileListModel extends AbstractListModel<Tile>{
    private final List<Tile> tiles;
    
    public TileListModel(){
        tiles = new ArrayList<Tile>();
        tiles.add(Tile.CITY_FILL);
        tiles.add(Tile.APPLE_CART);
        tiles.add(Tile.CONSTRUCTION);
        tiles.add(Tile.GRASS);
        tiles.add(Tile.TREE);
        tiles.add(Tile.RIVER);
        
        tiles.add(Tile.STREET);
        
        tiles.add(Tile.HOUSE);
        tiles.add(Tile.GREEN_HOUSE);
        tiles.add(Tile.YELLOW_HOUSE);
        tiles.add(Tile.FIRE_STATION);
        
        tiles.add(Tile.HOUSE_ON_FIRE);
    }

    @Override
    public int getSize() {
        return tiles.size();
    }

    @Override
    public Tile getElementAt(int index) {
        return tiles.get(index);
    }
}
