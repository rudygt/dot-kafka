using DotKafka.Prototype.Common.Errors;
using System;

namespace DotKafka.Prototype.Common.Protocol
{
    public static class ErrorHelper
    {
        public static ApiException ToException(this Error errorCode, Exception inner = null)
        {
            switch (errorCode)
            {
                case Error.Unknown: { return new UnknownServerException("The server experienced an unexpected error when processing the request", inner); }
                case Error.None: { return null; }
                case Error.OffsetOutOfRange: { return new ApiException("The requested offset is not within the range of offsets maintained by the server.", inner); }
                case Error.CorruptMessage: { return new CorruptRecordException("The message contents does not match the message CRC or the message is otherwise corrupt.", inner); }
                case Error.UnknownTopicOrPartition: { return new UnknownTopicOrPartitionException("This server does not host this topic-partition.", inner); }
                case Error.LeaderNotAvailable: { return new LeaderNotAvailableException("There is no leader for this topic-partition as we are in the middle of a leadership election.", inner); }
                case Error.NotLeaderForPartition: { return new NotLeaderForPartitionException("This server is not the leader for that topic-partition.", inner); }
                case Error.RequestTimedOut: { return new Errors.TimeoutException("The request timed out.", inner); }
                case Error.BrokerNotAvailable: { return new BrokerNotAvailableException("The broker is not available.", inner); }
                case Error.ReplicaNotAvailable: { return new ApiException("The replica is not available for the requested topic-partition", inner); }
                case Error.MessageTooLarge: { return new RecordTooLargeException("The request included a message larger than the max message size the server will accept.", inner); }
                case Error.StaleControllerEpoch: { return new ControllerMovedException("The controller moved to another broker.", inner); }
                case Error.OffsetMetadataTooLarge: { return new OffsetMetadataTooLarge("The metadata field of the offset request was too large.", inner); }
                case Error.NetworkException: { return new NetworkException("The server disconnected before a response was received.", inner); }
                case Error.GroupLoadInProgress: { return new GroupLoadInProgressException("The coordinator is loading and hence can't process requests for this group.", inner); }
                case Error.GroupCoordinatorNotAvailable: { return new GroupCoordinatorNotAvailableException("The group coordinator is not available.", inner); }
                case Error.NotCoordinatorForGroup: { return new NotCoordinatorForGroupException("This is not the correct coordinator for this group.", inner); }
                case Error.InvalidTopicException: { return new InvalidTopicException("The request attempted to perform an operation on an invalid topic.", inner); }
                case Error.RecordListTooLarge: { return new RecordBatchTooLargeException("The request included message batch larger than the configured segment size on the server.", inner); }
                case Error.NotEnoughReplicas: { return new NotEnoughReplicasException("Messages are rejected since there are fewer in-sync replicas than required.", inner); }
                case Error.NotEnoughReplicasAfterAppend: { return new NotEnoughReplicasAfterAppendException("Messages are written to the log, but to fewer in-sync replicas than required.", inner); }
                case Error.InvalidRequiredAcks: { return new InvalidRequiredAcksException("Produce request specified an invalid value for required acks.", inner); }
                case Error.IllegalGeneration: { return new IllegalGenerationException("Specified group generation id is not valid.", inner); }
                case Error.InconsistentGroupProtocol: { return new ApiException("The group member's supported protocols are incompatible with those of existing members.", inner); }
                case Error.InvalidGroupId: { return new ApiException("The configured groupId is invalid", inner); }
                case Error.UnknownMemberId: { return new UnknownMemberIdException("The coordinator is not aware of this member.", inner); }
                case Error.InvalidSessionTimeout: { return new ApiException("The session timeout is not within an acceptable range.", inner); }
                case Error.RebalanceInProgress: { return new RebalanceInProgressException("The group is rebalancing, so a rejoin is needed.", inner); }
                case Error.InvalidCommitOffsetSize: { return new ApiException("The committing offset data size is not valid", inner); }
                case Error.TopicAuthorizationFailed: { return new AuthorizationException("Topic authorization failed.", inner); }
                case Error.GroupAuthorizationFailed: { return new AuthorizationException("Group authorization failed.", inner); }
                case Error.ClusterAuthorizationFailed: { return new AuthorizationException("Cluster authorization failed.", inner); }
            }

            return null;
        }
    }

    public enum Error
    {
        Unknown = -1,
        None = 0,
        OffsetOutOfRange = 1,
        CorruptMessage = 2,
        UnknownTopicOrPartition = 3,
        InvalidFetchSize = 4,
        LeaderNotAvailable = 5,
        NotLeaderForPartition = 6,
        RequestTimedOut = 7,
        BrokerNotAvailable = 8,
        ReplicaNotAvailable = 9,
        MessageTooLarge = 10,
        StaleControllerEpoch = 11,
        OffsetMetadataTooLarge = 12,
        NetworkException = 13,
        GroupLoadInProgress = 14,
        GroupCoordinatorNotAvailable = 15,
        NotCoordinatorForGroup = 16,
        InvalidTopicException = 17,
        RecordListTooLarge = 18,
        NotEnoughReplicas = 19,
        NotEnoughReplicasAfterAppend = 20,
        InvalidRequiredAcks = 21,
        IllegalGeneration = 22,
        InconsistentGroupProtocol = 23,
        InvalidGroupId = 24,
        UnknownMemberId = 25,
        InvalidSessionTimeout = 26,
        RebalanceInProgress = 27,
        InvalidCommitOffsetSize = 28,
        TopicAuthorizationFailed = 29,
        GroupAuthorizationFailed = 30,
        ClusterAuthorizationFailed = 31
    }
}