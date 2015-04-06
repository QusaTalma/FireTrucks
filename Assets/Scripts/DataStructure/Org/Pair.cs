
using System;

namespace DataStructure.Org
{
	public class Pair<A, B>
	{
		private A m_a;
		public A PartA
		{
			get { return m_a; }
			set { m_a = value; }
		}

		private B m_b;
		public B PartB
		{
			get { return m_b; }
			set { m_b = value; }
		}

		public Pair (A p_a, B p_b)
		{
			m_a = p_a;
			m_b = p_b;
		}

	}
}

