using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alps.net.api.parsing;
using alps.net.api.StandardPASS;

// Dieser Kommentar kann wieder gelöscht werden, ist nur ein Test
// Moders Test Kommentar

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating paths inside project
            string currentDirectory = Directory.GetCurrentDirectory();
            string passPath = Path.Combine(currentDirectory, "PASS_Files", "ONTs", "standard_PASS_ont_dev.owl");
            string alpsPath = Path.Combine(currentDirectory, "PASS_Files", "ONTs", "ALPS_ont_v_0.8.0.owl");
            string urlaubAntragOWL = Path.Combine(currentDirectory, "PASS_Files", "Models", "Urlaubsantrag_Discord_Uebung.owl");

            Console.WriteLine("Start Loading PASS Models and Ontologies.");

            IPASSReaderWriter io = PASSReaderWriter.getInstance();

            // Prepare the paths to the structure-defining owl files
            IList<string> paths = new List<string>
            {
                passPath,
                alpsPath,
            };
            // Load these files once (no future calls neded)
            io.loadOWLParsingStructure(paths);

            IList<IPASSProcessModel> models = io.loadModels(new List<string> { urlaubAntragOWL });

            IDictionary<string, IPASSProcessModelElement> processModelElements = models[0].getAllElements();
            /*
            foreach (KeyValuePair<string, IPASSProcessModelElement> kvp in processModelElements)
            {
                Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            }
            */
            var subjects = new List<ISubject>();

            foreach (var element in processModelElements.Values)
            {
                if (element is ISubject subject)
                {
                    subjects.Add(subject);
                }
            };
            /*
            // Display states
            foreach (var subject in subjects)
            {
                Console.WriteLine("$ State:" {subject.get")
            }
            // Display subject URIs
            */
            /*
            foreach (var subject in subjects)
            {
                Console.WriteLine($"Subject: {subject.getUriModelComponentID()}");
            }
            // Get the name of the subject, element
            foreach (var subject in subjects)
            {
                var labels = subject.getModelComponentLabelsAsStrings();
                if (labels.Count>0)
                {
                    Console.WriteLine("Subject name: " + labels[0] );
                }

            }
           */
            // Get Subject Behaviors and display the states and their corresponding transitions 
            foreach ( var subject in subjects){
                var subj_name = subject.getModelComponentLabelsAsStrings()[0];
                Console.WriteLine($"Subject Name: {subj_name} " );
                foreach (var element in processModelElements.Values)
            {
                    if(element is ISubjectBehavior subjectBehavior)
                {
                        if(subjectBehavior.getSubject()!= null && subjectBehavior.getSubject().getModelComponentID() == subject.getModelComponentID())
                        {
                            Console.WriteLine("Behavior of Subject  " + subjectBehavior.getSubject().getModelComponentID() + " is :" + subjectBehavior.getModelComponentLabelsAsStrings()[0]);
                                
                            foreach (var component in subjectBehavior.getBehaviorDescribingComponents().Values)
                            {
                                if (component is IState state){
                                    var stateName = state.getModelComponentLabelsAsStrings().FirstOrDefault() ?? state.getModelComponentID();
                                    Console.WriteLine($"State : {stateName}");

                                    // List transitions
                                    foreach(var transition in state.getOutgoingTransitions().Values)
                                    {
                                        var targetState = transition.getTargetState();
                                        var targetName = targetState.getModelComponentLabelsAsStrings().FirstOrDefault()
                                            ?? targetState.getModelComponentID();
                                        Console.WriteLine($"Transition to: {targetName}");
                                    }
                                }
                            }
                        }
                            
                }
            }
                }
           
            /*
            var states = new List<IState>();

            foreach (var element in processModelElements.Values)
            {
                if(element is IState state)
                {
                    states.Add(state);
                }
            };
            */
            // Display state URIs
            /*
            foreach (var state in states)
            {
                Console.WriteLine($"State: {state.getUriModelComponentID()}");
            }
            */

            Console.ReadKey();

        }

    }
}


