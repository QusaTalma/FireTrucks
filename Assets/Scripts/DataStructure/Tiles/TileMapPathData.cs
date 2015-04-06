
using System;
using DataStructure.TileData;

namespace DataStructure.Tiles
{

	public class TileMapPathData
	{
		private TilePathDataNode[,] m_tilePathMap;

		public TileMapPathData (TDMap p_map)
		{
			m_tilePathMap = new TilePathDataNode[p_map.Width, p_map.Height];

			for(int i = 0; i < p_map.Width; i++)
			{
				for(int j = 0; j < p_map.Height; j++)
				{
					m_tilePathMap[i, j] = new TilePathDataNode(p_map, new Coord(i, j));
				}
			}
		}

		public TilePathDataNode getTileAt(Coord p_coord)
		{
			if(!p_coord.checkBounds(0, m_tilePathMap.GetLength(0), 0, m_tilePathMap.GetLength(1)))
				return null;

			return m_tilePathMap[p_coord.X, p_coord.Y];
		}

	}

}

