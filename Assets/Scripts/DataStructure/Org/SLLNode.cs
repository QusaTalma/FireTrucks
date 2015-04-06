
using System;

namespace DataStructure.Org
{
	public class SLLNode<T>
	{
		private SLLNode<T> m_next;
		private T m_value;

		public SLLNode (T p_value)
		{
			m_value = p_value;
			m_next = null;
		}

		public SLLNode<T> setNext(SLLNode<T> p_next)
		{
			SLLNode<T> output = m_next;

			m_next = p_next;

			return output;
		}

		public SLLNode<T> getNext()
		{
			return m_next;
		}

		public T getValue()
		{
			return m_value;
		}
	}
}

