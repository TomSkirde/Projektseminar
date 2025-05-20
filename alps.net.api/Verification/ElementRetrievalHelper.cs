using alps.net.api.ALPS;
using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace alps.net.api.Verification
{
    /// <summary>
    /// A class imported from the ALPS Verification project (https://github.com/andikra/ALPS-Verification-Thesis), originally created by Andreas Krämer.
    /// </summary>
    public class ElementRetrievalHelper
    {

        //SID Elements

        public IList<Tuple<ISubject, ISubject>> getSubjects(IList<IPASSProcessModel> models)
        {

            IList<ISubject> subjectsFstModel = models[0].getAllElements().Values.OfType<ISubject>().ToList();
            IList<ISubject> subjectsSndModel = models[1].getAllElements().Values.OfType<ISubject>().ToList();


            Console.WriteLine("\nSubject Implementation:");
            var implementingPairs = new List<Tuple<ISubject, ISubject>>();
            int log = 0;

            foreach (ISubject i in subjectsFstModel)
            {

                int subjectsImplementingI = 0;
                Console.WriteLine(i.getUriModelComponentID());

                foreach (ISubject j in subjectsSndModel)
                {
                    // Find a subject j in the second model that implements the current subject i in the first model
                    foreach (string id in j.getImplementedInterfacesIDReferences())
                    {
                        if (i.getUriModelComponentID() != id) continue;

                        subjectsImplementingI++;
                        implementingPairs.Add(new Tuple<ISubject, ISubject>(i, j));
                        //  Console.WriteLine(ID);

                    }


                }

                switch (subjectsImplementingI)
                {
                    case 1:
                        Console.WriteLine("Element implemented!");
                        LogWriter logcor = new LogWriter(i.getUriModelComponentID() + " Element implemented!");
                        break;
                    case > 1:
                        Console.WriteLine("Element implemented " + subjectsImplementingI + " times!");
                        LogWriter logcor2 =
                            new LogWriter(i.getUriModelComponentID() + " Element implemented" + subjectsImplementingI + " times!");

                        break;
                    case < 1:
                        Console.WriteLine("Element not implemented!");
                        LogWriter logcor3 = new LogWriter(i.getUriModelComponentID() + " Element not implemented!");
                        implementingPairs.Add(new Tuple<ISubject, ISubject>(i, i));
                        log++;
                        break;
                }

                if (log > 0)
                {
                    LogWriter logcorresult = new LogWriter("Not all Elements implemented!");

                }
            }

            return implementingPairs;

        }

        public IList<Tuple<IMessageExchange, IImplementingElement<IMessageExchange>>> GetMessages(
            IList<IPASSProcessModel> models)
        {



            IList<IMessageExchange> Message0 = models[0].getAllElements().Values.OfType<IMessageExchange>().ToList();
            IList<IImplementingElement<IMessageExchange>> Message1 = models[1].getAllElements().Values
                .OfType<IImplementingElement<IMessageExchange>>().ToList();


            Console.WriteLine("\nMessage Implementation:");
            var messages = new List<Tuple<IMessageExchange, IImplementingElement<IMessageExchange>>>();


            foreach (IMessageExchange i in Message0)
            {

                int z = 0;
                Console.WriteLine(i.getUriModelComponentID());

                foreach (IImplementingElement<IMessageExchange> j in Message1)
                {
                    foreach (string id in j.getImplementedInterfacesIDReferences())
                    {
                        if (i.getUriModelComponentID() != id) continue;

                        z++;
                        messages.Add(new Tuple<IMessageExchange, IImplementingElement<IMessageExchange>>(i, j));
                        //  Console.WriteLine(ID);

                    }


                }

                switch (z)
                {
                    case 1:
                        Console.WriteLine("Element implemented!");
                        LogWriter logcor = new LogWriter(i + " Element implemented!");
                        break;
                    case > 1:
                        Console.WriteLine("Element implemented " + z + " times!");
                        LogWriter logcor2 = new LogWriter(i + " Element implemented" + z + " times!");

                        break;
                    case < 1:
                        Console.WriteLine("Element not implemented!");
                        LogWriter logcor3 = new LogWriter(i + " Element not implemented!");
                        messages.Add(new Tuple<IMessageExchange, IImplementingElement<IMessageExchange>>(i, null));


                        break;
                }
            }

            return messages;

        }

        public List<Tuple<ICommunicationAct, IImplementingElement<ICommunicationAct>>> getMessageTransitions(
            IList<IPASSProcessModel> models)
        {


            int log = 0;
            IList<ICommunicationAct> messageTrans0 =
                models[0].getAllElements().Values.OfType<ICommunicationAct>().ToList();
            IList<IImplementingElement<ICommunicationAct>> messageTrans1 = models[1].getAllElements().Values
                .OfType<IImplementingElement<ICommunicationAct>>().ToList();


            //Console.WriteLine("\nMessage Transition Implementation:");
            var messages = new List<Tuple<ICommunicationAct, IImplementingElement<ICommunicationAct>>>();


            foreach (ICommunicationAct i in messageTrans0)
            {

                int z = 0;
                Console.WriteLine(i.getUriModelComponentID());

                foreach (IImplementingElement<ICommunicationAct> j in messageTrans1)
                {
                    foreach (string id in j.getImplementedInterfacesIDReferences())
                    {
                        if (i.getUriModelComponentID() != id) continue;

                        z++;
                        messages.Add(new Tuple<ICommunicationAct, IImplementingElement<ICommunicationAct>>(i, j));
                        //  Console.WriteLine(ID);

                    }


                }

                switch (z)
                {
                    case 1:
                        Console.WriteLine("Element implemented!");
                        LogWriter logcor = new LogWriter(i.getUriModelComponentID() + " Element implemented!");
                        break;
                    case > 1:
                        Console.WriteLine("Element implemented " + z + " times!");
                        LogWriter logcor2 =
                            new LogWriter(i.getUriModelComponentID() + " Element implemented" + z + " times!");

                        break;
                    case < 1:
                        Console.WriteLine("Element not implemented!");
                        LogWriter logcor3 = new LogWriter(i.getUriModelComponentID() + " Element not implemented!");
                        messages.Add(new Tuple<ICommunicationAct, IImplementingElement<ICommunicationAct>>(i, null));


                        break;
                }

            }

            if (log > 0)
            {
                LogWriter logcorresult = new LogWriter("Not all Elements implemented!");

            }

            return messages;


        }

        public List<Tuple<ICommunicationRestriction, IImplementingElement<ICommunicationRestriction>>>
            getMessageRestriction(IList<IPASSProcessModel> models)
        {

            LogWriter sub = new LogWriter("Message Restrictions:");


            IList<ICommunicationRestriction> messageTrans0 =
                models[0].getAllElements().Values.OfType<ICommunicationRestriction>().ToList();
            IList<IImplementingElement<ICommunicationRestriction>> messageTrans1 = models[1].getAllElements().Values
                .OfType<IImplementingElement<ICommunicationRestriction>>().ToList();


            Console.WriteLine("\nMessage Restriction Implementation:");
            var messages =
                new List<Tuple<ICommunicationRestriction, IImplementingElement<ICommunicationRestriction>>>();


            foreach (ICommunicationRestriction i in messageTrans0)
            {

                int z = 0;
                Console.WriteLine(i.getUriModelComponentID());
                LogWriter log = new LogWriter(i.getUriModelComponentID());

                foreach (IImplementingElement<ICommunicationRestriction> j in messageTrans1)
                {
                    foreach (string ID in j.getImplementedInterfacesIDReferences())
                    {
                        if (i.getUriModelComponentID() != ID) continue;

                        z++;
                        messages.Add(
                            new Tuple<ICommunicationRestriction, IImplementingElement<ICommunicationRestriction>>(i, j));
                        //  Console.WriteLine(ID);

                    }


                }

                switch (z)
                {
                    case 1:
                        Console.WriteLine("Element implemented!");
                        LogWriter logcor = new LogWriter(i + " Element implemented!");
                        break;
                    case > 1:
                        Console.WriteLine("Element implemented " + z + " times!");
                        LogWriter logcor2 = new LogWriter(i + " Element implemented" + z + " times!");

                        break;
                    case < 1:
                        Console.WriteLine("Element not implemented!");
                        LogWriter logcor3 = new LogWriter(i + " Element not implemented!");
                        messages.Add(
                            new Tuple<ICommunicationRestriction, IImplementingElement<ICommunicationRestriction>>(i,
                                null));


                        break;
                }
            }

            return messages;

        }


        // SBD Elements

        public List<Tuple<IState, IState>> getStates(IList<IPASSProcessModel> models)
        {

            LogWriter sub = new LogWriter("States:");


            IList<IState> State0 = models[0].getAllElements().Values.OfType<IState>().ToList();
            IList<IState> State1 = models[1].getAllElements().Values.OfType<IState>().ToList();


            //Console.WriteLine("\nState Implementation:");
            var elements = new List<Tuple<IState, IState>>();


            foreach (IState i in State0)
            {

                int z = 0;
                Console.WriteLine(i.getUriModelComponentID());
                LogWriter log = new LogWriter(i.getUriModelComponentID());

                foreach (IState j in State1)
                {
                    foreach (string id in j.getImplementedInterfacesIDReferences())
                    {
                        if (i.getUriModelComponentID() != id) continue;

                        z++;
                        elements.Add(new Tuple<IState, IState>(i, j));
                        //  Console.WriteLine(ID);

                    }


                }

                switch (z)
                {
                    case 1:
                        Console.WriteLine("Element implemented!");
                        LogWriter logcor = new LogWriter(i + " Element implemented!");
                        break;
                    case > 1:
                        Console.WriteLine("Element implemented " + z + " times!");
                        LogWriter logcor2 = new LogWriter(i + " Element implemented" + z + " times!");

                        break;
                    case < 1:
                        Console.WriteLine("Element not implemented!");
                        LogWriter logcor3 = new LogWriter(i + " Element not implemented!");
                        elements.Add(new Tuple<IState, IState>(i, i));

                        break;
                }
            }

            return elements;


        }

        public List<Tuple<ITransition, ITransition>> getTransitions(IList<IPASSProcessModel> models)
        {

            LogWriter sub = new LogWriter("Transitions:");


            IList<ITransition> trans0 = models[0].getAllElements().Values.OfType<ITransition>().ToList();
            IList<IState> trans1 = models[1].getAllElements().Values.OfType<IState>().ToList();


            //Console.WriteLine("\nTransition Implementation:");
            var elements = new List<Tuple<ITransition, ITransition>>();


            foreach (ITransition i in trans0)
            {

                int z = 0;
                Console.WriteLine(i.getUriModelComponentID());
                LogWriter log = new LogWriter(i.getUriModelComponentID());

                foreach (ITransition j in trans1)
                {
                    foreach (string ID in j.getImplementedInterfacesIDReferences())
                    {
                        if (i.getUriModelComponentID() != ID) continue;

                        z++;
                        elements.Add(new Tuple<ITransition, ITransition>(i, j) { });
                        //  Console.WriteLine(ID);

                    }


                }

                switch (z)
                {
                    case 1:
                        Console.WriteLine("Element implemented!");
                        LogWriter logcor = new LogWriter(i + " Element implemented!");
                        break;
                    case > 1:
                        Console.WriteLine("Element implemented " + z + " times!");
                        LogWriter logcor2 = new LogWriter(i + " Element implemented" + z + " times!");

                        break;
                    case < 1:
                        Console.WriteLine("Element not implemented!");
                        LogWriter logcor3 = new LogWriter(i + " Element not implemented!");
                        elements.Add(new Tuple<ITransition, ITransition>(i, i));

                        break;
                }
            }

            return elements;


        }
    }
}