namespace alps.net.api.StandardPASS
{
    public interface IExtendingElement<T>
    {
        void setExtendedElement(T element);

        void setExtendedElementID(string elementID);

        T getExtendedElement();

        string getExtendedElementID();

        bool isExtension();
    }
}
