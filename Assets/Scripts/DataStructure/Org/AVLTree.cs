
using System;

namespace DataStructure.Org
{
	public class AVLTree<K, V> where K : IComparable<K>
	{
		private AVLTreeNode<K, V> m_root;
		private int m_size;

		public AVLTree ()
		{
			m_root = null;
			m_size = 0;
		}

		public void bulkInsert(SLL<K> p_keys, V p_setTo)
		{
			if(p_keys == null)
				return;

			while(p_keys.look() != null)
				insert(p_keys.remove().getValue(), p_setTo);
		}

		public Boolean insert(K p_key, V p_value)
		{
			if(p_key == null)
				return false;

			AVLTreeNode<K, V> newNode = new AVLTreeNode<K, V>(p_key, p_value);

			m_root = newNode.insert(m_root);

			m_size++;

			return true;
		}

		public V find(K p_search)
		{
			AVLTreeNode<K, V> found = AVLTreeNode<K, V>.find(m_root, p_search);

			if(found == null)
				return default(V);

			return found.getValue();
		}

		public Boolean remove(K p_key)
		{
			if(p_key == null)
				return false;

			AVLTreeNode<K, V> rmNode = AVLTreeNode<K, V>.find(m_root, p_key);

			if(rmNode == null)
				return false;

			m_root = rmNode.remove();

			m_size--;

			return true;
		}

	}
}

