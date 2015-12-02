namespace DotKafka.Prototype.Common.Protocol
{
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