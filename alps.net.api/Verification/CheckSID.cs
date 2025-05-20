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
    public class CheckSID
    {
        /// <summary>
        /// Use the communications with the getcorrespondents method to check whether communication exists while being forbidden
        /// </summary>
        public int checkCommunicationRestrictions(IList<ICommunicationRestriction> specifiedRestrictions,
            IList<IMessageExchange> implementingMessages)
        {
            // Klären: Bidirektional oder nicht?

            int result = 0;
            Console.WriteLine("\nCheck Communication Restrictions:");
            foreach (ICommunicationRestriction r in specifiedRestrictions)

            {
                ISubject A = r.getCorrespondentA();
                ISubject B = r.getCorrespondentB();

                foreach (IMessageExchange m in implementingMessages)
                {

                    ISubject C = m.getSender();
                    ISubject D = m.getReceiver();
                    if ((C != A && C != B) || (D != A && D != B)) continue;

                    Console.WriteLine(r + " violated!");
                    result++;
                }



            }

            if (result == 0)
            {
                Console.WriteLine("SID Restriction Implementation valid.");
                return 1;
            }
            else
            {
                Console.WriteLine("SID Restriction Implementation not valid.");
                return 0;
            }
        }

        /// <summary>
        /// This method checks whether the implementation references still fulfill the restrictions given by the specification 
        /// -- example: a fully specified subject must be implemented as a fully specified subject and cannot be an abstract subject
        /// </summary>
        public bool checkSubject(IList<Tuple<ISubject, ISubject>> subjects)
        {
            Console.WriteLine("\nCheck SID Subject Implementation:");
            int z = 0;
            bool result = true;
            int fullySpecified = 0;

            foreach (Tuple<ISubject, ISubject> t in subjects.Where((a => a.Item1 != a.Item2)))
            {
                Console.WriteLine(t.Item1.GetType());
                Console.WriteLine(t.Item2.GetType());

                switch (t.Item1.GetType().ToString())
                {
                    case "alps.net.api.StandardPASS.FullySpecifiedSubject":
                        if (t.Item1.GetType() != t.Item2.GetType())
                        {
                            Console.WriteLine("Implementation not correct!");
                            fullySpecified++;
                        }

                        break;

                        //insert other subject forms here
                }


            }

            if (fullySpecified > 0)
            {
                result = false;
            }

            return result;
        }

        public bool checkMessageConnectors(
            IList<Tuple<ICommunicationAct, IImplementingElement<ICommunicationAct>>> messageTransitions)
        {
            //Console.WriteLine("\nCheck SID Transition Implementation:");
            bool result = true;
            int fullySpecified = 0;

            foreach (Tuple<ICommunicationAct, IImplementingElement<ICommunicationAct>> t in messageTransitions.Where(
                         (a => a.Item1 != null)))
            {
                Console.WriteLine(t.Item1.GetType());
                Console.WriteLine(t.Item2.GetType());

                switch (t.Item1.GetType().ToString())
                {
                    case "alps.net.api.StandardPASS.FullySpecifiedSubject":
                        if (t.Item1.GetType() != t.Item2.GetType())
                        {
                            Console.WriteLine("Implementation not correct!");
                            fullySpecified++;
                        }

                        break;

                        //insert other subject forms here
                }


            }

            if (fullySpecified > 0)
            {
                result = false;
            }

            return result;
        }


    }
}
