using alps.net.api.StandardPASS;
using System.Collections.Generic;
using System.Linq;

namespace alps.net.api.parsing
{

    /// <summary>
    /// A basic factory that creates standard ModelElements contained inside the alps.net.api library
    /// </summary>
    public class BasicPASSProcessModelElementFactory : IPASSProcessModelElementFactory<IParseablePASSProcessModelElement>
    {
        public string createInstance(IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict,
            IList<string> names, out IParseablePASSProcessModelElement element)
        {
            element = new PASSProcessModelElement();
            ISet<string> bestParseableNames = new HashSet<string>();
            int lowestParseDiff = int.MaxValue;

            // Check how good the instantiations for the names are.
            // Only use the names where instantiation-pairs have lowest numbers
            foreach (string uriName in names)
            {
                string name = removeUri(uriName);
                if (!parsingDict.ContainsKey(name)) continue;

                // The Item2 for each item signalizes how far off the c# class is mapped to an owl class.
                // Example: in owl, "BlueSubject" is subclass of "Subject". In c# we only know "Subject".
                // The owl "Subject" class has a mapping score of 0 and is mapped to the c# class "Subject"
                // The owl "BlueSubject" class has a mapping score of 1 and is mapped to the c# class "Subject", the last known parent class
                foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) pair in parsingDict[name])
                {
                    if (pair.Item2 > lowestParseDiff) continue;

                    // If the mapping is equally good as the mappings which were already found, add it
                    if (pair.Item2 == lowestParseDiff)
                    {
                        bestParseableNames.Add(name);
                    }

                    // If the mapping is better than the mappings which were already found, delete the old mappings and add it
                    else
                    {
                        lowestParseDiff = pair.Item2;
                        bestParseableNames.Clear();
                        bestParseableNames.Add(name);
                    }
                }
            }

            IDictionary<IParseablePASSProcessModelElement, string> possibleElements = new Dictionary<IParseablePASSProcessModelElement, string>();
            // Gather all possible instantiations
            foreach (string name in bestParseableNames)
            {
                foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) pair in parsingDict[name])
                    possibleElements.Add(pair.Item1.getContent(), name);
            }

            if (bestParseableNames.Count > 1)
            {
                bool foundSubclass;

                // If one of the possible instances is superclass to another possible instance, throw the superclass out (only want most specific instance)
                do
                {
                    foundSubclass = false;
                    IList<IParseablePASSProcessModelElement> remove = new List<IParseablePASSProcessModelElement>();
                    foreach (IParseablePASSProcessModelElement someElement in possibleElements.Keys)
                    {
                        // If none of the elements are equal and no other type in the list is subclass of the current type, continue
                        if (!possibleElements.Keys.Any(someOtherElement => someElement.Equals(someOtherElement) &&
                                                                           someOtherElement.GetType()
                                                                               .IsSubclassOf(someElement.GetType()))) continue;

                        // Else add the redundant/parent type to be removed later 
                        remove.Add(someElement);
                        foundSubclass = true;
                    }

                    // Delete after finishing iteration over list
                    foreach (IParseablePASSProcessModelElement someElement in remove)
                    {
                        possibleElements.Remove(someElement);
                    }
                } while (foundSubclass);
            }

            switch (possibleElements.Count)
            {
                // Take the only element and return a new instance
                case 1:
                    element = possibleElements.Keys.First().getParsedInstance();
                    return possibleElements.Values.First();

                // Still some elements left that are both
                // - equally good in parsing
                // - no superclass to another class
                // parse the one having the longest matching name (longer name -> more specific instance ?)
                case > 1:
                    {
                        KeyValuePair<IParseablePASSProcessModelElement, string> selectedPair = decideForElement(possibleElements);
                        element = selectedPair.Key.getParsedInstance();
                        return selectedPair.Value;
                    }
                default:
                    return null;
            }
        }
        private static string removeUri(string stringWithUri)
        {
            string[] splitStr = stringWithUri.Split('#');

            // return only the last part
            return splitStr[splitStr.Length -1 ];
        }

        protected virtual KeyValuePair<IParseablePASSProcessModelElement, string> decideForElement(IDictionary<IParseablePASSProcessModelElement, string> possibleElements)
        {
            int max = -1;
            KeyValuePair<IParseablePASSProcessModelElement, string> maxPair = new KeyValuePair<IParseablePASSProcessModelElement, string>();
            int counter = 0;
            foreach (KeyValuePair<IParseablePASSProcessModelElement, string> pair in possibleElements)
            {
                int parseability = pair.Key.canParse(pair.Value);
                if (parseability > max)
                {
                    max = parseability;
                    maxPair = pair;
                }
                counter++;
            }

            return maxPair;
            
        }
    }
}
