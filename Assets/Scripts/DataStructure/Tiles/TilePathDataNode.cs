
using System;
using Enums;
using DataStructure.TileData;
using DataStructure.Org;

namespace DataStructure.Tiles
{
	
	/** This class contains all of the collision specific dynamic tile information. */
	public class TilePathDataNode
	{
		private SLL<TimeClaim> m_claimTimes;
		
		/** Creates tile collision data from the tilemap. */
		public TilePathDataNode (TDMap p_map, Coord p_tileLoc)
		{
			TDTile tile = p_map.GetTile(p_tileLoc.X, p_tileLoc.Y);
		}

		/** Will this tile be claimed at the given time? */
		public bool isClaimed(float p_gameTime)
		{
			return false;
		}

		/** Who has claimed this tile, at the given time? */
		public EntityID getClaimeeID(float p_gameTime)
		{
			return null;
		}

		/** Give the next valid start time for a claim of the given duration. */
		public float nextClaimTime(float p_claimDuration)
		{
			return 0f;
		}

		public bool validClaim(float p_claimStartTime, float p_claimDuration)
		{
			return false;
		}

		/** Register a new claim on this tile. Returns false only when a prior claim already exists. */
		public bool registerClaim(int p_claimeeID,
		                          float p_claimStartTime, float p_claimDuration,
		                          Direction p_claimDirection)
		{
			if(!validClaim(p_claimStartTime, p_claimDuration))
				return false;

			return true;
		}

	}

}

