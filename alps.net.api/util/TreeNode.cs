using System;
using System.Collections.Generic;

namespace alps.net.api
{
    /// <summary>
    /// Class that represents a tree node
    /// </summary>
    public class TreeNode<T> : ITreeNode<T>
    {
        private ITreeNode<T> parentNode;
        private readonly IList<ITreeNode<T>> childNodes = new List<ITreeNode<T>>();
        private T content;

        /// <summary>
        /// Constructor that creates a empty tree node
        /// </summary>
        public TreeNode()
        {

        }

        /// <summary>
        /// Constructor creating a TreeNode with content
        /// </summary>
        /// <param name="content">A string that is contained in the tree node</param>
        public TreeNode(T content)
        {
            setContent(content);
        }

        /// <summary>
        /// Constructor creating a TreeNode with content, a parent and a child
        /// </summary>
        /// <param name="content">A string that is contained in the tree node</param>
        /// <param name="parent">A node that is parent of this node</param>
        /// <param name="child">A node that is a child of this node</param>
        public TreeNode(T content, ITreeNode<T> parent, IList<ITreeNode<T>> childNodes)
        {
            setContent(content);
            setParentNode(parent);
            setChildNodes(childNodes);
        }

        /// <summary>
        /// Constructor creating a TreeNode with content, a parent and a child
        /// </summary>
        /// <param name="childNodes">the child nodes</param>
        public TreeNode(IList<ITreeNode<T>> childNodes)
        {
            setChildNodes(childNodes);
        }


        public void setParentNode(ITreeNode<T> parent)
        {
            parentNode = parent;
        }


        public void setChildNodes(IList<ITreeNode<T>> childNodes)
        {
            if (childNodes is null)
            {
                this.childNodes.Clear();
            }
            else
            {
                foreach (ITreeNode<T> childNode in childNodes)
                    this.childNodes.Add(childNode);
            }
        }


        public void addChild(ITreeNode<T> child)
        {
            if (child is not null)
            {
                childNodes.Add(child);
                child.setParentNode(this);
            }
        }


        public ITreeNode<T> getParentNode()
        {
            return parentNode;
        }


        public IList<ITreeNode<T>> getChildNodes()
        {
            return childNodes;
        }


        public T getContent()
        {
            return content;
        }


        public override string ToString()
        {
            string result = "";

            if (childNodes != null)
            {

                foreach (ITreeNode<T> i in childNodes)
                {
                    result += i.getContent();

                    Console.WriteLine(i.getContent());
                    i.ToString();

                }
            }

            return result;
        }


        public bool containsContent(T content, out ITreeNode<T> node)
        {
            bool test = false;
            node = null;

            foreach (ITreeNode<T> t in childNodes)
            {
                if (t.getContent().Equals(content))
                {
                    test = true;
                    node = this;
                    break;
                }
                else
                {
                    test = t.containsContent(content, out node);
                    if (test) break;
                }
            }

            return test;
        }

        public void setContent(T content)
        {
            this.content = content;
        }

        public bool isSubClassOf(ITreeNode<T> parent, bool direct = false)
        {
            if (parentNode is null) return false;
            if (parentNode.Equals(parent)) return true;
            if (!direct) return parentNode.isSubClassOf(parent);
            return false;
        }

        public ITreeNode<T> getChild(int index)
        {
            if (index < 0 || index > (childNodes.Count - 1)) return null;
            return childNodes[index];
        }

        public ITreeNode<T> getRoot()
        {
            ITreeNode<T> currentNode = this;
            ITreeNode<T> parent = null;
            while ((parent = currentNode.getParentNode()) is not null) currentNode = parent;
            return currentNode;
        }

        public int getHeigthToLastLeaf()
        {
            if (getChildNodes().Count == 0)
                return 0;
            int height = 0;
            foreach (ITreeNode<T> child in getChildNodes())
            {
                height = Math.Max(height, child.getHeigthToLastLeaf());
            }
            return height + 1;
        }
    }
}
