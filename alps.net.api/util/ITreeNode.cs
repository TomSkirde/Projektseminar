using System.Collections.Generic;

namespace alps.net.api
{
    /// <summary>
    /// Interface to the tree node class
    /// </summary>
    public interface ITreeNode<T>
    {
        /// <summary>
        /// Sets the parent node
        /// </summary>
        /// <param name="parent">the parent node</param>
        void setParentNode(ITreeNode<T> parent);

        /// <summary>
        /// Overrides the current child nodes with a list of new child nodes
        /// </summary>
        /// <param name="childNodes">the new child nodes</param>
        void setChildNodes(IList<ITreeNode<T>> childNodes);

        /// <summary>
        /// Adds a child to the list of child nodes
        /// </summary>
        /// <param name="child">the node of the child</param>
        void addChild(ITreeNode<T> child);

        /// <summary>
        /// Returns the parent node
        /// </summary>
        /// <returns>the parent node</returns>
        ITreeNode<T> getParentNode();

        /// <summary>
        /// Returns the child nodes
        /// </summary>
        /// <returns>the child nodes</returns>
        IList<ITreeNode<T>> getChildNodes();

        ITreeNode<T> getChild(int index);

        /// <summary>
        /// Sets the content of the node
        /// </summary>
        /// <param name="content">the content</param>
        void setContent(T content);

        /// <summary>
        /// Returns the content of the node
        /// </summary>
        /// <returns>the content of the node</returns>
        T getContent();

        /// <summary>
        /// Checks whether the node contains a given string as content
        /// </summary>
        /// <param name="content">the string that will be checked as reference</param>
        /// <returns>true if the string equals the content, false if not</returns>
        bool containsContent(T compare, out ITreeNode<T> node);

        /// <summary>
        /// Checks whether the given node is sublcass of a specified node
        /// </summary>
        /// <param name="parent">the other node the given instance might be subclass of</param>
        /// <param name="direct">if true is passed, this method only returns true if the current instance is a direct sublass of the parent class</param>
        /// <returns>true if it is a sublass of the specified parent, false if not</returns>
        bool isSubClassOf(ITreeNode<T> parent, bool direct = false);

        /// <summary>
        /// Returns the root node of the current tree node
        /// </summary>
        /// <returns>the root node</returns>
        ITreeNode<T> getRoot();

        /// <summary>
        /// Returns the height of the longest path to a leaf starting from this node.
        /// If this node is already a leaf, it returns 0
        /// </summary>
        /// <returns></returns>
        int getHeigthToLastLeaf();
    }
}
