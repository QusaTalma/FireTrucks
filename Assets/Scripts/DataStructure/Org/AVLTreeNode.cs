
using System;

namespace DataStructure.Org
{

	public class AVLTreeNode<K, V> where K : IComparable<K>
	{

		protected AVLTreeNode<K, V> m_parent;
		protected AVLTreeNode<K, V> m_left;
		protected AVLTreeNode<K, V> m_right;
		protected int m_height;

		protected K m_key;
		protected V m_value;

		public AVLTreeNode (K p_key, V p_value)
		{
			m_parent = null;
			m_left = null;
			m_right = null;
			m_height = 1;

			m_key = p_key;
			m_value = p_value;
		}

		public K getKey()
		{
			return m_key;
		}

		public V getValue()
		{
			return m_value;
		}

		public static AVLTreeNode<K, V>
			find(AVLTreeNode<K, V> p_tree, K p_search)
		{
			p_tree = findPlace(p_tree, p_search);

			if(p_tree == null)
				return null;

			if(p_search.CompareTo(p_tree.m_key) == 0)
				return p_tree;

			return null;
		}


		public AVLTreeNode<K, V> insert(AVLTreeNode<K, V> p_tree)
		{
			if(p_tree == null)
				return this;

			AVLTreeNode<K, V> place = findPlace(p_tree, m_key);

			Boolean placeLeft = m_key.CompareTo(place.m_key) < 0;

			AVLTreeNode<K, V> child = getChild(place, placeLeft);

			if(child != null)
			{
				place = place.findLeaf(getHeight(m_left) < getHeight(m_right));

				placeLeft = m_key.CompareTo(place.m_key) < 0;

				if(getChild(place, placeLeft) != null)
					placeLeft = !placeLeft;
			}

			place.bind(placeLeft, this);

			return balance();
		}

		public AVLTreeNode<K, V> remove()
		{
			swap(findLeaf(getHeight(m_left) < getHeight(m_right)));

			Boolean pLeft = isParentsLeft(this);
			AVLTreeNode<K, V> parent = m_parent;
			if(parent != null)
				parent.bind(pLeft, null);

			AVLTreeNode<K, V> child = m_left;
			if(child == null)
			{
				child = m_right;
				bind(false, null);
			}
			else
				bind(true, null);

			if(child != null)
			{
				child.bindParent(parent, pLeft);

				return rmBalance(child);
			}

			return rmBalance(parent);
		}

		protected static AVLTreeNode<K, V>
			findPlace(AVLTreeNode<K, V> p_tree, K p_search)
		{
			if(p_tree == null || p_search == null)
				return null;

			int comp = p_search.CompareTo(p_tree.m_key);

			if(comp != 0)
			{
				Boolean goLeft = comp < 0;

				AVLTreeNode<K, V> child = getChild(p_tree, goLeft);

				if(child != null)
					return findPlace(child, p_search);
			}

			return p_tree;
		}

		protected AVLTreeNode<K, V> findLeaf(Boolean p_isLeft)
		{
			AVLTreeNode<K, V> child = getChild(this, p_isLeft);

			if(child == null)
				return this;

			AVLTreeNode<K, V> currNode = child;

			while(getChild(currNode, !p_isLeft) != null)
				currNode = getChild(currNode, !p_isLeft);

			return currNode;
		}

		protected static Boolean
			isParentsLeft(AVLTreeNode<K, V> p_node)
		{
			return getChild(getParent(p_node), true) == p_node;
		}

		protected static AVLTreeNode<K, V>
			getSibbling(AVLTreeNode<K, V> p_node)
		{
			if(getParent(p_node) == null)
				return null;

			return getChild(p_node.m_parent, !isParentsLeft(p_node));
		}

		protected static AVLTreeNode<K, V>
			getParent(AVLTreeNode<K, V> p_node)
		{
			if(p_node == null)
				return null;

			return p_node.m_parent;
		}

		protected static AVLTreeNode<K, V>
			getChild(AVLTreeNode<K, V> p_node, Boolean p_isLeft)
		{
			if(p_node == null)
				return null;

			if(p_isLeft)
				return p_node.m_left;
			
			return p_node.m_right;
		}


		protected Boolean rotate()
		{
			if(m_parent == null)
				return false;

			Boolean gpDir = isParentsLeft(m_parent);
			Boolean pDir = isParentsLeft(this);

			AVLTreeNode<K, V> grandParent = m_parent.m_parent;

			AVLTreeNode<K, V> parent = m_parent;

			AVLTreeNode<K, V> child = getChild(this, !pDir);

			parent.bind(pDir, child);
			bind( !pDir, parent);
			bindParent(grandParent, gpDir);

			return true;
		}

		protected Boolean swap(AVLTreeNode<K, V> p_other)
		{
			if(p_other == null || this == p_other)
				return false;

			AVLTreeNode<K, V> leftA = p_other.m_left;
			AVLTreeNode<K, V> rightA = p_other.m_right;
			Boolean parentDirA = isParentsLeft(p_other);
			AVLTreeNode<K, V> parentA = p_other.m_parent;

			AVLTreeNode<K, V> leftB = m_left;
			AVLTreeNode<K, V> rightB = m_right;
			Boolean parentDirB = isParentsLeft(this);
			AVLTreeNode<K, V> parentB = m_parent;

			if(parentA == this)
			{
				parentA = p_other;

				if(leftB == p_other)
					leftB = this;
				else
					rightB = this;
			}
			else if(parentB == p_other)
			{
				parentB = this;

				if(leftA == this)
					leftA = p_other;
				else
					rightA = p_other;
			}

			bind(true, leftA);
			bind(false, rightA);
			bindParent(parentA, parentDirA);

			p_other.bind(true, leftB);
			p_other.bind(false, rightB);
			p_other.bindParent(parentB, parentDirB);

			return true;
		}

		protected Boolean bind(Boolean p_isLeft, AVLTreeNode<K, V> p_child)
		{
			if(this == p_child)
				return false;

			if(p_child != null)
				p_child.m_parent = this;

			if(p_isLeft)
				m_left = p_child;
			else
				m_right = p_child;

			updateHeight();

			return true;
		}


		private Boolean bindParent(AVLTreeNode<K, V> p_parent, Boolean p_isLeft)
		{
			if(p_parent == null)
			{
				m_parent = null;
				return false;
			}

			p_parent.bind(p_isLeft, this);

			return true;
		}

		private void updateHeight()
		{
			m_height = Math.Max(getHeight(m_left), getHeight(m_right)) + 1;
		}

		private static int
			getHeight(AVLTreeNode<K, V> p_node)
		{
			if(p_node == null)
				return 0;

			return p_node.m_height;
		}

		private static AVLTreeNode<K, V>
			rmBalance(AVLTreeNode<K, V> p_node)
		{
			if(p_node == null)
				return null;

			return p_node.balance();
		}

		private AVLTreeNode<K, V> balance()
		{
			if(m_parent != null)
				m_parent.updateHeight();

			if(getParent(m_parent) != null
			   && isParentsLeft(this) != isParentsLeft(m_parent)
			   && m_height > getHeight(getSibbling(this)))
			{
				rotate();

				return balance();
			}

			AVLTreeNode<K, V> nextNode = this;

			int diff = getHeight(m_left) - getHeight(m_right);

			if(Math.Abs(diff) > 1)
			{
				nextNode = getChild(this, diff > 0);

				nextNode.rotate();
			}

			if(nextNode.m_parent != null)
				return nextNode.m_parent.balance();
			else
				return this;

		}

	}

}

