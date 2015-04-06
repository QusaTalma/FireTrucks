
using System;

namespace DataStructure.Org
{
	public class SLL<T>
	{
		private SLLNode<T> m_head;
		private SLLNode<T> m_tail;

		public SLL ()
		{
			m_head = null;
			m_tail = null;
		}

		/** adds to the end */
		public void push(SLLNode<T> p_node)
		{
			if(m_tail == null)
			{
				m_head = p_node;
				m_tail = p_node;

				return;
			}

			m_tail.setNext(p_node);
			m_tail = p_node;
		}

		/** looks at the end */
		public SLLNode<T> peak()
		{
			return m_tail;
		}

		/** adds to the start */
		public void insert(SLLNode<T> p_node)
		{
			if(m_head == null)
			{
				m_head = p_node;
				m_tail = p_node;

				return;
			}

			p_node.setNext(m_head);
			m_head = p_node;
		}

		/** looks at the start */
		public SLLNode<T> look()
		{
			return m_head;
		}

		public int Count()
		{
			int i = 0;
			SLLNode<T> curr = m_head;
			while(curr != null)
			{
				curr = curr.getNext();
				i++;
			}
			return i;
		}

		public SLLNode<T> getItem(int p_i)
		{
			SLLNode<T> curr = m_head;
			while(curr != null && p_i > 0)
			{
				curr = curr.getNext();
				p_i--;
			}

			if(p_i > 0)
				return null;

			return curr;
		}

		/** takes from the start */
		public SLLNode<T> remove()
		{
			SLLNode<T> output = m_head;

			if(m_head != null)
			{
				SLLNode<T> tmp = m_head.getNext();
				m_head.setNext(null);
				m_head = tmp;

				if(m_head == null)
					m_tail = null;
			}

			return output;
		}
	}
}

