
using System;
using Enums;

namespace DataStructure
{

	public class EntityID : IComparable<EntityID>
	{
		private EntityType m_type;
		public EntityType Type
		{
			get
			{
				return m_type;
			}
		}

		private int m_index = -1;
		private static int nextIndex = 0;
		public int Index
		{
			get
			{
				if(m_index == -1)
					m_index = nextIndex++;

				return m_index;
			}
		}

		public EntityID(EntityType p_type)
		{
			m_type = p_type;
		}

		public int CompareTo(EntityID p_other)
		{
			if(Index < p_other.Index)
				return -1;
			if(Index > p_other.Index)
				return 1;

			return 0;
		}

	}

}

