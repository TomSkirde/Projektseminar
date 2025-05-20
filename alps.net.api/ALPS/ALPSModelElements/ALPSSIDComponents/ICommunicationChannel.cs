using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface for abstract communication channels.
    /// This also represents Uni- and BiDirectionalCommunicationChannels
    /// The direction can be set using the <see cref="setIsUniDirectional(bool)"/> Method.
    /// </summary>
    public interface ICommunicationChannel : IALPSSIDComponent, IHasSimple2DVisualizationLine
    {

        void setCorrespondents(ISubject correspondentA, ISubject correspondentB, int removeCascadeDepth = 0);

        void setCorrespondentA(ISubject correspondentA, int removeCascadeDepth = 0);

        void setCorrespondentB(ISubject correspondentB, int removeCascadeDepth = 0);

        ISubject getCorrespondentA();

        ISubject getCorrespondentB();

        Tuple<ISubject, ISubject> getCorrespondents();

        /// <summary>
        /// Sets the direction of the channel.
        /// This might be either Uni- or BiDirectional.
        /// In case of a UniDirectional, the CorrespondentA should be assumed as the Sender,
        /// and Correspondent should be assumed as the Receiver.
        /// </summary>
        /// <param name="isUniDirectional">If true, this channel acts as UniDirectionalCommunicationChannel.<br></br>
        /// If false, this channel acts as BiDirectionalCommunicationChannel. </param>
        void setIsUniDirectional(bool isUniDirectional);

        /// <summary>
        /// Returns whether this channel is a Bi- or UniDirectionalCommunicationChannel.
        /// </summary>
        /// <returns>If true, the channel is UniDirectional, assuming the CorrespondentA as the Sender and CorrespondentB as Receiver.
        /// If false, the channel is BiDirectional</returns>
        bool isUniDirectional();
    }
}
