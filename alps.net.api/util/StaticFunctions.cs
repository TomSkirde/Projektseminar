using alps.net.api.parsing;
using System.Linq;

namespace alps.net.api.util
{
    public class StaticFunctions
    {

        /// <summary>
        /// Replaces a base uri contained in a string with a generic placeholder.
        /// Example: http://www.imi.kit.edu/exampleBaseURI#MyModel1 will return baseuri:MyModel1.
        /// The baseuri name mapping is defined in the triple store graph.
        /// This way, the base uri can be changed without changing all triples.
        /// </summary>
        /// <param name="input">The string that MAY contain a base uri</param>
        /// <returns>the string without specific base uri</returns>
        public static string replaceSpecificBaseUriWithGeneric(string input, string baseuri, string newPlaceholder = ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER)
        {
            string output = input;
            string uriToSearchFor = baseuri.EndsWith(ParserConstants.URI_SEPARATOR.ToString()) ? baseuri:baseuri+ ParserConstants.URI_SEPARATOR;

            // If a base uri is contained, replace it with generic one
            if (output.Equals(baseuri))
            {
                output = baseuri;
            }

            // If a base uri is contained, replace it with generic one
            else if (output.Contains(uriToSearchFor))
            {
                output = output.Replace(uriToSearchFor, ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER);
            }

            if (!newPlaceholder.Equals(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER) && output.Contains(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER))
            {
                output = output.Replace(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER, newPlaceholder);
            }

            return output;
        }

        /*public static string replaceBaseUriWithGeneric(string input, string baseuri,string genericPlaceholder)
        {
            if (genericPlaceholder == null)
            return addGenericBaseURI(removeBaseUri(input, baseuri));
            else return addGenericBaseURI(removeBaseUri(input, baseuri), genericPlaceholder);
        }*/


        public static string removeAllUriPrefix(string input) {
            string output = input;
            var splitted = input.Split(ParserConstants.URI_SEPARATOR);

            if (splitted.Length > 1)
                output = splitted.Last();

            var splitted2 = output.Split(ParserConstants.PLACEHOLDER_SEPARATOR);

            if (splitted2.Length > 1)
                output = splitted2.Last();

            return output;
        }


        /// <summary>
        /// Replaces a base uri contained in a string with a generic placeholder.
        /// Example: http://www.imi.kit.edu/exampleBaseURI#MyModel1 will return baseuri:MyModel1.
        /// The baseuri name mapping is defined in the triple store graph.
        /// This way, the base uri can be changed without changing all triples.
        /// </summary>
        /// <param name="input">The string that MAY contain a base uri</param>
        /// <returns>the string without specific base uri</returns>
        public static string removeBaseUri(string input, string baseuri)
        {
            string output = input;

            if (baseuri != null)
            {
                // If a base uri is contained, replace it with generic one
                if (output.Contains(baseuri + ParserConstants.URI_SEPARATOR))
                {
                    output = output.Replace(baseuri + ParserConstants.URI_SEPARATOR, "");
                }

                // If a base uri is contained, replace it with generic one
                if (output.Contains(baseuri))
                {
                    output = output.Replace(baseuri, "");
                }
            }

            if (output.Contains(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER))
            {
                output = output.Replace(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER, "");
            }
            return output;
        }

        public static string addGenericBaseURI(string input, string generic = ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER)
        {

            if (input.StartsWith(ParserConstants.EXAMPLE_BASE_URI_PLACEHOLDER)) return input;
            if (input.StartsWith("http")) return input;

            return generic + input;
        }

        
    }
}
