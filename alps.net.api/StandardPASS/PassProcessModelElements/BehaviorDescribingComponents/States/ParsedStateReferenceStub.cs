using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;
using VDS.RDF;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a StateReference
    /// Because a StateReference acts the same as the class of the state it references,
    /// it s not possible to use the StateReference as standalone C#-class.<br></br>
    /// Example: SendTransition from a StateReference would not work, because the Transition needs a SendState as Origin.
    /// One solution would be creating a new class for each possible State, implementing the IStateReference interface end extending the state -> many new classes.<br></br>
    /// The current approach is to move the functionality into the State class. Every state the extends the standard State class can reference other states,
    /// to use the functionality the state must be casted to IStateReference.
    /// <br></br>
    /// <br></br>
    /// <b>This class is only for parsing reasons (loads references and converts them to states) and should not be used to model!</b>
    /// </summary>
    public class ParsedStateReferenceStub : State, IStateReference
    {
        //protected IState referenceState;
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "StateReference";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ParsedStateReferenceStub();
        }

        public ParsedStateReferenceStub() { }



        public void setReferencesState(IState state, int removeCascadeDepth = 0)
        {
            return;
        }


        public IState getReferencesState()
        {
            return null;
        }


        public IState transformToState(IDictionary<string, IParseablePASSProcessModelElement> allElements)
        {
            IList<IPASSTriple> allTriples = getIncompleteTriples();

            foreach (IPASSTriple t in allTriples)
            {
                if (t.getPredicate().Contains(OWLTags.references))
                {
                    string objID = StaticFunctions.removeBaseUri(t.getObject().ToString(), null);
                    if (allElements.TryGetValue(objID, out IParseablePASSProcessModelElement element))
                    {
                        IState state = (IState)element.getParsedInstance();

                        if (state is IParseablePASSProcessModelElement parseable)
                            parseable.addTriples(allTriples);
                        state.setModelComponentID(getModelComponentID());
                        if (state is IStateReference reference)
                            reference.setReferencedState((IState)element);
                        return state;
                    }
                }
            }
            return null;
        }
    }
}