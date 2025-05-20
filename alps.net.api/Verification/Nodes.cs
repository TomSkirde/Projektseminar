


//adjust this package and make it functional for precedence/trigger transitions
namespace VisioAddIn.OwlShapes
{
    /*
    /// <summary>
    /// A class imported from the ALPS Verification project (https://github.com/andikra/ALPS-Verification-Thesis), originally created by Andreas Krämer.
    /// </summary>
    public class VisioSubjectBehavior : SubjectBehavior, IVisioExportableWithShape
    {

        private Simple2DPosParser parser;

        protected VisioSubjectBehavior() { }

        public VisioSubjectBehavior(IModelLayer layer, string labelForID = null, ISubject subject = null,
            ISet<IBehaviorDescribingComponent> behaviorDescribingComponents = null, IState initialStateOfBehavior = null,
            int priorityNumber = 0, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, subject, behaviorDescribingComponents, initialStateOfBehavior, priorityNumber, comment,
                  additionalLabel, additionalAttribute)
        { }


        public void exportToVisio(Visio.Page currentPage, ISimple2DVisualizationBounds bounds = null)
        {
            foreach (IState state in behaviorDescriptionComponents.Values.OfType<IState>())
            {
                if (state is IVisioExportable exportable)
                    exportable.exportToVisio(currentPage);
            }
            foreach (ITransition transition in behaviorDescriptionComponents.Values.OfType<ITransition>())
            {
                if (transition is IVisioExportable exportable)
                    exportable.exportToVisio(currentPage);
            }

            IList<IState> possibleFirstStates = new List<IState>();
            IDictionary<string, IGraphNode<IPASSProcessModelElement>> allCreatedNodes = new Dictionary<string, IGraphNode<IPASSProcessModelElement>>();

            // Search all the first states
            foreach (IState state in getBehaviorDescribingComponents().Values.OfType<IState>())
            {
                if (state.getIncomingTransitions().Count == 0)
                {
                    possibleFirstStates.Add(state);

                    // Create a tree node wrapping the state, add it to known states (used for building the tree)
                    IGraphNode<IPASSProcessModelElement> createdNode = new DirectedGraphNode<IPASSProcessModelElement>(state);
                    allCreatedNodes.Add(state.getModelComponentID(), createdNode);
                }
            }
            foreach (IState state in possibleFirstStates)
            {
                buildTree(state, allCreatedNodes);
            }

            exportTree(allCreatedNodes[possibleFirstStates[0].getModelComponentID()], currentPage);


        }

        private void exportTree(IGraphNode<IPASSProcessModelElement> rootNode, Visio.Page currentPage)
        {
            int pageWidth = int.Parse(currentPage.PageSheet.CellsU["PageWidth"].FormulaU.Replace(" mm", ""));
            int pageHeight = int.Parse(currentPage.PageSheet.CellsU["PageHeight"].FormulaU.Replace(" mm", ""));
            __exportTree(rootNode, currentPage, 25, pageHeight - 25);
        }

        private int __exportTree(IGraphNode<IPASSProcessModelElement> rootNode, Visio.Page currentPage, int xpos, int ypos)
        {
            if (rootNode.getContent() is IVisioExportableWithShape ex)
            {
                Visio.Shape exportedShape = ex.getShape();
                exportedShape.CellsU["PinX"].Formula = "\"" + xpos.ToString() + " mm\"";
                exportedShape.CellsU["PinY"].Formula = "\"" + ypos.ToString() + " mm\"";

                int newX = xpos + 70;
                int newY = ypos;

                foreach (var childNode in rootNode.getOutputNodes())
                {
                    newY = __exportTree(childNode, currentPage, newX, newY) - 40;
                }

                if (rootNode.getOutputNodes().Count > 0)
                    newY += 40;
                return newY;
            }
            return ypos;

        }

        private void buildTree(IState state, IDictionary<string, IGraphNode<IPASSProcessModelElement>> allCreatedNodes)
        {
            IGraphNode<IPASSProcessModelElement> originNode = allCreatedNodes[state.getModelComponentID()];
            foreach (ITransition outgoing in state.getOutgoingTransitions().Values)
            {
                IState targetState = outgoing.getTargetState();
                if (targetState != null)
                {
                    // Node is already somewhere in the tree
                    if (allCreatedNodes.ContainsKey(targetState.getModelComponentID()))
                    {
                        originNode.addOutputNode(allCreatedNodes[targetState.getModelComponentID()]);
                    }

                    // Node is not in the tree --> create it, build tree recusively
                    else
                    {
                        IGraphNode<IPASSProcessModelElement> outputNode = new DirectedGraphNode<IPASSProcessModelElement>(targetState);
                        originNode.addOutputNode(outputNode);
                        allCreatedNodes.Add(targetState.getModelComponentID(), outputNode);
                        buildTree(targetState, allCreatedNodes);
                    }
                }
            }
        }

        public Visio.Shape getShape()
        {
            throw new NotImplementedException();
        }

        public void setShape(Visio.Shape shape)
        {
            throw new NotImplementedException();
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisioSubjectBehavior();
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (parser is null) parser = new Simple2DPosParser(this);
            if (!parser.parseAttribute(predicate, objectContent, lang, dataType, element))
            {
                return base.parseAttribute(predicate, objectContent, lang, dataType, element);
            }
            return true;
        }

    }*/
}
