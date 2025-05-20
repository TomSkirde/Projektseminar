
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api.ALPS;
using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace alps.net.api.Verification
{
    /// <summary>
    /// A class imported from the ALPS Verification project (https://github.com/andikra/ALPS-Verification-Thesis), originally created by Andreas Krämer.
    /// This class can be used to verify whether a specific model correctly implements a specifying model.
    /// </summary>
    public class ModelVerifier
    {

        public static void verifyModels(IPASSProcessModel definingModel, IPASSProcessModel implementingModel)
        {
            LogWriter log = new LogWriter("Kra");

            // Used SID Values from the implementing model:
            IList<IImplementingElement<ISubject>> implementingElementsSet =
                implementingModel.getAllElements().Values.OfType<IImplementingElement<ISubject>>().ToList();
            IList<IImplementingElement<IMessageSpecification>> implementingElementsMessage = implementingModel.getAllElements()
                .Values.OfType<IImplementingElement<IMessageSpecification>>().ToList();
            IList<IImplementingElement<IMessageSpecification>> implementingElementsMessageSpecs =
                implementingModel.getAllElements().Values.OfType<IImplementingElement<IMessageSpecification>>().ToList();
            IList<IImplementingElement> implementingElements =
                implementingModel.getAllElements().Values.OfType<IImplementingElement>().ToList();
            IList<IMessageExchange> impElementsMessages =
                implementingModel.getAllElements().Values.OfType<IMessageExchange>().ToList();
            IList<IPASSProcessModelElement> implementingElementsStandard =
                implementingModel.getAllElements().Values.OfType<IPASSProcessModelElement>().ToList();

            // Used SID Values from the specifying model:
            IList<IALPSSIDComponent> specElements =
                definingModel.getAllElements().Values.OfType<IALPSSIDComponent>().ToList();
            IList<ISubject> specifyingElementsSubject = definingModel.getAllElements().Values.OfType<ISubject>().ToList();
            IList<IMessageSpecification> specifyingElementsMessages =
                definingModel.getAllElements().Values.OfType<IMessageSpecification>().ToList();
            IList<ICommunicationRestriction> specifyingElementsRestrictions =
                definingModel.getAllElements().Values.OfType<ICommunicationRestriction>().ToList();
            IList<IPASSProcessModelElement> specifyingElementsStandard =
                definingModel.getAllElements().Values.OfType<IPASSProcessModelElement>().ToList();


            IList<IPASSProcessModel> models = new List<IPASSProcessModel> { definingModel, implementingModel };

            //Methods for SID Implementation Existence checks:
            ElementRetrievalHelper GetAll = new ElementRetrievalHelper();

            IList<Tuple<ISubject, ISubject>> Subjects = GetAll.getSubjects(models);
            IList<Tuple<IMessageExchange, IImplementingElement<IMessageExchange>>> Messages = GetAll.GetMessages(models);
            IList<Tuple<ICommunicationAct, IImplementingElement<ICommunicationAct>>> Transitions =
                GetAll.getMessageTransitions(models);
            IList<Tuple<ICommunicationRestriction, IImplementingElement<ICommunicationRestriction>>> restrictions =
                GetAll.getMessageRestriction(models);
            IList<Tuple<IState, IState>> States = GetAll.getStates(models);
            IList<Tuple<ITransition, ITransition>> SBDTransitions = GetAll.getTransitions(models);



            //Methods for SID Validity checks: 
            CheckSID CheckSID = new CheckSID();
            int ResultCheckSIDRestrictions =
                CheckSID.checkCommunicationRestrictions(specifyingElementsRestrictions, impElementsMessages);
            CheckSID.checkSubject(Subjects);
            CheckSID.checkMessageConnectors(Transitions);

            //Methods for SID Validity checks: 
            CheckSBD CheckSBD = new CheckSBD();









        }

    }

}



