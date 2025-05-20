using VDS.RDF;

namespace alps.net.api.util
{
    public class NodeHelper
    {
        public static string getNodeContent(INode node, string baseuri = null)
        {
            string nodeString;
            if (node is ILiteralNode literal) nodeString = literal.Value;
            else nodeString = node.ToString();
            if (baseuri != null)
            
                nodeString = StaticFunctions.replaceSpecificBaseUriWithGeneric(nodeString, baseuri);
            
            return nodeString;
        }

        public static string getLangIfContained(INode node)
        {
            if (node is ILiteralNode literal) return literal.Language;
            return "";
        }

        public static string getDataTypeIfContained(INode node)
        {
            if (node is ILiteralNode literal && literal.DataType != null) return literal.DataType.ToString();
            return "";
        }

        public static IStringWithExtra getObjAsStringWithExtra(INode node)
        {
            if (node is ILiteralNode literal)
            {
                IStringWithExtra extraString = null;
                var lang = literal.Language;
                var dataType = literal.DataType;
                if (lang is null && dataType is null)
                    extraString = new StringWithoutExtra(literal.Value);
                else
                {
                    if (dataType is not null)
                    {
                        extraString = new DataTypeString(literal.Value, literal.DataType.ToString());
                    }

                    if (lang is not null)
                    {
                        extraString = new LanguageSpecificString(literal.Value, literal.Language);
                    }
                }
                return extraString;
            }
            return null;
        }

        public static string cutURI(string uri)
        {
            string firstCut = uri.Split('#')[uri.Split('#').Length - 1];
            return firstCut.Split(':')[firstCut.Split(':').Length - 1];
        }
    }
}
