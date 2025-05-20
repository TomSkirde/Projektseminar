using alps.net.api.util;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface for the MessageSpecification class
    /// </summary>

    public interface IMessageSpecification : IInteractionDescribingComponent
    {
        /// <summary>
        /// Sets the payload description for the message specification, which describes the payload of the message
        /// </summary>
        /// <param name="payloadDescription">the payload description</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setContainedPayloadDescription(IPayloadDescription payloadDescription, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the payload description for the message specification, which describes the payload of the message
        /// </summary>
        /// <returns>the payload description</returns>
        IPayloadDescription getContainedPayloadDescription();


        /// <summary>
        /// For simple simulation of processes: The (expected) transmission time of this kind of message. Necessary only for simulation purposes
        /// </summary>
        ISiSiTimeDistribution simpleSimTransmissionTime { get; set; }

        /// <summary>
        /// for values streamm analysisefine what type of Messag this is. Standard;Conveyance Time (internal);Conveyance Time (external);Information Flow (internal);Information Flow (external);
        /// </summary>
        SimpleSimVSMMessageTypes simpleSimVSMMessageType { get; set; }

    }

    /// <summary>
    /// Message types for Value Stream Mapping Analysis
    /// Values shoudl be: Standard;Conveyance Time (internal);Conveyance Time (external);Information Flow (internal);Information Flow (external);
    /// </summary>
    public enum SimpleSimVSMMessageTypes
    {
        Standard,
        ConveyanceTimeInternal,
        ConveyanceTimeExternal,
        InformationFlowInternal,
        InformationFlowExternal
    }

}
