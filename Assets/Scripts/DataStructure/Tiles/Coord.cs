
using System;
using UnityEngine;
using Enums;

namespace DataStructure.Tiles
{
	public class Coord : IComparable<Coord>
	{
		private int m_x;
		public int X
		{
			get
			{
				return m_x;
			}

			set
			{
				m_x = value;
			}
		}

		private int m_y;
		public int Y
		{
			get
			{
				return m_y;
			}

			set
			{
				m_y = value;
			}
		}

		public Coord (int p_x, int p_y)
		{
			m_x = p_x;
			m_y = p_y;
		}

		public Coord(Coord p_dup)
		{
			m_x = p_dup.m_x;
			m_y = p_dup.m_y;
		}

		public static Coord toCoord(Vector2 p_v)
		{
			return new Coord(Mathf.FloorToInt(p_v.x), Mathf.FloorToInt(p_v.y));
		}

		public static Coord toCoord(Direction p_direction)
		{
			switch(p_direction)
			{
			case Direction.NORTH:
				return new Coord(0, 1);
				
			case Direction.SOUTH:
				return new Coord(0, -1);
				
			case Direction.EAST:
				return new Coord(1, 0);
				
			case Direction.WEST:
				return new Coord(-1, 0);
				
			default:
				return new Coord(0, 0);
			}
		}

		public static Vector3 toVector(Direction p_direction)
		{
			switch(p_direction)
			{
				case Direction.NORTH:
					return new Vector3(0, 0, 1);

				case Direction.SOUTH:
					return new Vector3(0, 0, -1);
				
				case Direction.EAST:
					return new Vector3(1, 0, 0);
				
				case Direction.WEST:
					return new Vector3(-1, 0, 0);

				default:
					return new Vector3(0, 0, 0);
			}
		}

		public Coord getDir(Direction p_direction)
		{
			switch(p_direction)
			{
				case Direction.NORTH:
					return new Coord(m_x, m_y + 1);

				case Direction.SOUTH:
					return new Coord(m_x, m_y - 1);

				case Direction.EAST:
					return new Coord(m_x + 1, m_y);

				case Direction.WEST:
					return new Coord(m_x - 1, m_y);

				default:
					return new Coord(this);
			}
		}

		public static Direction findDir(Coord p_a, Coord p_b)
		{
			if(p_a == null || p_b == null)
				return Direction.NONE;

			int w = p_a.m_x - p_b.m_x;
			int h = p_a.m_y - p_b.m_y;

			if(Math.Abs(w) >= Math.Abs(h))
			{
				if(w > 0)
					return Direction.WEST;

				if(w < 0)
					return Direction.EAST;

				return Direction.NONE;
			}

			if(h > 0)
				return Direction.SOUTH;

			return Direction.NORTH;
		}

		/** Returns the manhattan distance between two points. */
		public int minDist(Coord p_other)
		{
			return Math.Abs(m_x - p_other.m_x) + Math.Abs(m_y - p_other.m_y);
		}

		public int CompareTo(Coord p_other)
		{
			if(p_other.m_x < m_x)
				return 1;

			if(p_other.m_x > m_x)
				return -1;

			if(p_other.m_y < m_y)
				return 1;

			if(p_other.m_y > m_y)
				return -1;

			return 0;
		}

		public bool checkBounds(Coord p_min, Coord p_max)
		{
			return checkBounds(p_min.X, p_max.X, p_min.Y, p_max.Y);
		}

		public bool checkBounds(int p_minX, int p_maxX, int p_minY, int p_maxY)
		{
			if(m_x < p_minX)
				return false;

			if(m_x >= p_maxX)
				return false;

			if(m_y < p_minY)
				return false;

			if(m_y >= p_maxY)
				return false;

			return true;
		}

		public override string ToString()
		{
			return "[" + m_x + ", " + m_y + "]";
		}

	}
}

