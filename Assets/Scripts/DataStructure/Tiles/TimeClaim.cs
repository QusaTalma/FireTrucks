
using System;

namespace DataStructure.Tiles
{

	public class TimeClaim
	{
		private EntityID m_claimeeID;
		public EntityID ClaimeeID
		{
			get
			{
				return m_claimeeID;
			}
		}

		private float m_startTime;
		public float StartTime
		{
			get
			{
				return m_startTime;
			}
		}

		private float m_duration;
		public float Duration
		{
			get
			{
				return m_duration;
			}
		}

		public float EndTime
		{
			get
			{
				return m_startTime + m_duration;
			}
		}


		public TimeClaim (EntityID p_claimeeID, float p_startTime, float p_duration)
		{
			m_claimeeID = p_claimeeID;
			m_startTime = p_startTime;
			m_duration = p_duration;
		}
	}

}
