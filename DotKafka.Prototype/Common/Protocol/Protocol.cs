using DotKafka.Prototype.Common.Errors;
using DotKafka.Prototype.Common.Protocol.Types;
using System;
using System.Linq;

namespace DotKafka.Prototype.Common.Protocol
{
    public enum SecurityProtocol
    {
        PlainText = 0,
        SSL = 1,
        SASLPlainText = 2,
        SASLSSL = 3,
        Trace = short.MaxValue
    }

    public class Protocol
    {
        public static readonly Schema REQUEST_HEADER = new Schema(new Field("api_key", KafkaTypesHelper.Int16, "The id of the request type."),
                                                           new Field("api_version", KafkaTypesHelper.Int16, "The version of the API."),
                                                           new Field("correlation_id",
                                                                    KafkaTypesHelper.Int32,
                                                                     "A user-supplied integer value that will be passed back with the response"),
                                                           new Field("client_id",
                                                                     KafkaTypesHelper.String,
                                                                     "A user specified identifier for the client making the request."));

        public static readonly Schema RESPONSE_HEADER = new Schema(new Field("correlation_id",
                                                                     KafkaTypesHelper.Int32,
                                                                      "The user-supplied value passed in with the request"));

        public static readonly Schema METADATA_REQUEST_V0 = new Schema(new Field("topics", new KafkaArrayOf(KafkaTypesHelper.String),
                                                                          "An array of topics to fetch metadata for. If no topics are specified fetch metadata for all topics."));

        public static readonly Schema BROKER = new Schema(new Field("node_id", KafkaTypesHelper.Int32, "The broker id."),
                                                   new Field("host", KafkaTypesHelper.String, "The hostname of the broker."),
                                                   new Field("port", KafkaTypesHelper.Int32, "The port on which the broker accepts requests."));

        public static readonly Schema PARTITION_METADATA_V0 = new Schema(new Field("partition_error_code",
                                                                           KafkaTypesHelper.Int16,
                                                                           "The error code for the partition, if any."),
                                                                 new Field("partition_id",
                                                                           KafkaTypesHelper.Int32,
                                                                           "The id of the partition."),
                                                                 new Field("leader",
                                                                           KafkaTypesHelper.Int32,
                                                                           "The id of the broker acting as leader for this partition."),
                                                                 new Field("replicas",
                                                                           new KafkaArrayOf(KafkaTypesHelper.Int32),
                                                                           "The set of all nodes that host this partition."),
                                                                 new Field("isr",
                                                                           new KafkaArrayOf(KafkaTypesHelper.Int32),
                                                                           "The set of nodes that are in sync with the leader for this partition."));

        public static readonly Schema TOPIC_METADATA_V0 = new Schema(new Field("topic_error_code",
                                                                            KafkaTypesHelper.Int16,
                                                                            "The error code for the given topic."),
                                                                  new Field("topic", KafkaTypesHelper.String, "The name of the topic"),
                                                                  new Field("partition_metadata",
                                                                            new KafkaArrayOf(PARTITION_METADATA_V0),
                                                                            "Metadata for each partition of the topic."));

        public static readonly Schema METADATA_RESPONSE_V0 = new Schema(new Field("brokers",
                                                                               new KafkaArrayOf(BROKER),
                                                                               "Host and port information for all brokers."),
                                                                     new Field("topic_metadata",
                                                                               new KafkaArrayOf(TOPIC_METADATA_V0)));

        public static readonly Schema[] METADATA_REQUEST = new Schema[] { METADATA_REQUEST_V0 };
        public static readonly Schema[] METADATA_RESPONSE = new Schema[] { METADATA_RESPONSE_V0 };

        /* Produce api */

        public static readonly Schema TOPIC_PRODUCE_DATA_V0 = new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                      new Field("data", new KafkaArrayOf(new Schema(new Field("partition", KafkaTypesHelper.Int32),
                                                                                                         new Field("record_set", KafkaTypesHelper.Bytes)))));

        public static readonly Schema PRODUCE_REQUEST_V0 = new Schema(new Field("acks",
                                                                       KafkaTypesHelper.Int16,
                                                                       "The number of nodes that should replicate the produce before returning. -1 indicates the full ISR."),
                                                                   new Field("timeout", KafkaTypesHelper.Int32, "The time to await a response in ms."),
                                                                   new Field("topic_data", new KafkaArrayOf(TOPIC_PRODUCE_DATA_V0)));

        public static readonly Schema PRODUCE_RESPONSE_V0 = new Schema(new Field("responses",
                                                                        new KafkaArrayOf(new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                                               new Field("partition_responses",
                                                                                                         new KafkaArrayOf(new Schema(new Field("partition",
                                                                                                                                          KafkaTypesHelper.Int32),
                                                                                                                                new Field("error_code",
                                                                                                                                          KafkaTypesHelper.Int16),
                                                                                                                                new Field("base_offset",
                                                                                                                                          KafkaTypesHelper.Int64))))))));
        public static readonly Schema PRODUCE_REQUEST_V1 = PRODUCE_REQUEST_V0;

        public static readonly Schema PRODUCE_RESPONSE_V1 = new Schema(new Field("responses",
                                                                              new KafkaArrayOf(new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                                                     new Field("partition_responses",
                                                                                                               new KafkaArrayOf(new Schema(new Field("partition",
                                                                                                                                                KafkaTypesHelper.Int32),
                                                                                                                                      new Field("error_code",
                                                                                                                                                KafkaTypesHelper.Int16),
                                                                                                                                      new Field("base_offset",
                                                                                                                                                KafkaTypesHelper.Int64))))))),
                                                                    new Field("throttle_time_ms",
                                                                              KafkaTypesHelper.Int32,
                                                                              "Duration in milliseconds for which the request was throttled" +
                                                                                  " due to quota violation. (Zero if the request did not violate any quota.)",
                                                                              0));

        public static readonly Schema[] PRODUCE_REQUEST = new Schema[] { PRODUCE_REQUEST_V0, PRODUCE_REQUEST_V1 };
        public static readonly Schema[] PRODUCE_RESPONSE = new Schema[] { PRODUCE_RESPONSE_V0, PRODUCE_RESPONSE_V1 };

        /* Offset commit api */
        public static readonly Schema OFFSET_COMMIT_REQUEST_PARTITION_V0 = new Schema(new Field("partition",
                                                                                             KafkaTypesHelper.Int32,
                                                                                             "Topic partition id."),
                                                                                   new Field("offset",
                                                                                             KafkaTypesHelper.Int64,
                                                                                             "Message offset to be committed."),
                                                                                   new Field("metadata",
                                                                                             KafkaTypesHelper.String,
                                                                                             "Any associated metadata the client wants to keep."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_PARTITION_V1 = new Schema(new Field("partition",
                                                                                             KafkaTypesHelper.Int32,
                                                                                             "Topic partition id."),
                                                                                   new Field("offset",
                                                                                             KafkaTypesHelper.Int64,
                                                                                             "Message offset to be committed."),
                                                                                   new Field("timestamp",
                                                                                             KafkaTypesHelper.Int64,
                                                                                             "Timestamp of the commit"),
                                                                                   new Field("metadata",
                                                                                             KafkaTypesHelper.String,
                                                                                             "Any associated metadata the client wants to keep."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_PARTITION_V2 = new Schema(new Field("partition",
                                                                                             KafkaTypesHelper.Int32,
                                                                                             "Topic partition id."),
                                                                                   new Field("offset",
                                                                                             KafkaTypesHelper.Int64,
                                                                                             "Message offset to be committed."),
                                                                                   new Field("metadata",
                                                                                             KafkaTypesHelper.String,
                                                                                             "Any associated metadata the client wants to keep."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_TOPIC_V0 = new Schema(new Field("topic",
                                                                                         KafkaTypesHelper.String,
                                                                                         "Topic to commit."),
                                                                               new Field("partitions",
                                                                                         new KafkaArrayOf(OFFSET_COMMIT_REQUEST_PARTITION_V0),
                                                                                         "Partitions to commit offsets."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_TOPIC_V1 = new Schema(new Field("topic",
                                                                                         KafkaTypesHelper.String,
                                                                                         "Topic to commit."),
                                                                               new Field("partitions",
                                                                                         new KafkaArrayOf(OFFSET_COMMIT_REQUEST_PARTITION_V1),
                                                                                         "Partitions to commit offsets."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_TOPIC_V2 = new Schema(new Field("topic",
                                                                                         KafkaTypesHelper.String,
                                                                                         "Topic to commit."),
                                                                               new Field("partitions",
                                                                                         new KafkaArrayOf(OFFSET_COMMIT_REQUEST_PARTITION_V2),
                                                                                         "Partitions to commit offsets."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_V0 = new Schema(new Field("group_id",
                                                                                   KafkaTypesHelper.String,
                                                                                   "The group id."),
                                                                         new Field("topics",
                                                                                   new KafkaArrayOf(OFFSET_COMMIT_REQUEST_TOPIC_V0),
                                                                                   "Topics to commit offsets."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_V1 = new Schema(new Field("group_id",
                                                                                   KafkaTypesHelper.String,
                                                                                   "The group id."),
                                                                         new Field("group_generation_id",
                                                                                   KafkaTypesHelper.Int32,
                                                                                   "The generation of the group."),
                                                                         new Field("member_id",
                                                                                   KafkaTypesHelper.String,
                                                                                   "The member id assigned by the group coordinator."),
                                                                         new Field("topics",
                                                                                   new KafkaArrayOf(OFFSET_COMMIT_REQUEST_TOPIC_V1),
                                                                                   "Topics to commit offsets."));

        public static readonly Schema OFFSET_COMMIT_REQUEST_V2 = new Schema(new Field("group_id",
                                                                                   KafkaTypesHelper.String,
                                                                                   "The group id."),
                                                                         new Field("group_generation_id",
                                                                                   KafkaTypesHelper.Int32,
                                                                                   "The generation of the consumer group."),
                                                                         new Field("member_id",
                                                                                   KafkaTypesHelper.String,
                                                                                   "The consumer id assigned by the group coordinator."),
                                                                         new Field("retention_time",
                                                                                   KafkaTypesHelper.Int64,
                                                                                   "Time period in ms to retain the offset."),
                                                                         new Field("topics",
                                                                                   new KafkaArrayOf(OFFSET_COMMIT_REQUEST_TOPIC_V2),
                                                                                   "Topics to commit offsets."));

        public static readonly Schema OFFSET_COMMIT_RESPONSE_PARTITION_V0 = new Schema(new Field("partition",
                                                                                              KafkaTypesHelper.Int32,
                                                                                              "Topic partition id."),
                                                                                    new Field("error_code",
                                                                                              KafkaTypesHelper.Int16));

        public static readonly Schema OFFSET_COMMIT_RESPONSE_TOPIC_V0 = new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                                new Field("partition_responses",
                                                                                          new KafkaArrayOf(OFFSET_COMMIT_RESPONSE_PARTITION_V0)));

        public static readonly Schema OFFSET_COMMIT_RESPONSE_V0 = new Schema(new Field("responses",
                                                                                    new KafkaArrayOf(OFFSET_COMMIT_RESPONSE_TOPIC_V0)));

        public static readonly Schema[] OFFSET_COMMIT_REQUEST = new Schema[] { OFFSET_COMMIT_REQUEST_V0, OFFSET_COMMIT_REQUEST_V1, OFFSET_COMMIT_REQUEST_V2 };

        /* The response types for V0, V1 and V2 of OFFSET_COMMIT_REQUEST are the same. */
        public static readonly Schema OFFSET_COMMIT_RESPONSE_V1 = OFFSET_COMMIT_RESPONSE_V0;
        public static readonly Schema OFFSET_COMMIT_RESPONSE_V2 = OFFSET_COMMIT_RESPONSE_V0;

        public static readonly Schema[] OFFSET_COMMIT_RESPONSE = new Schema[] { OFFSET_COMMIT_RESPONSE_V0, OFFSET_COMMIT_RESPONSE_V1, OFFSET_COMMIT_RESPONSE_V2 };

        /* Offset fetch api */

        /*
         * Wire formats of version 0 and 1 are the same, but with different functionality.
         * Version 0 will read the offsets from ZK;
         * Version 1 will read the offsets from Kafka.
         */
        public static readonly Schema OFFSET_FETCH_REQUEST_PARTITION_V0 = new Schema(new Field("partition",
                                                                                            KafkaTypesHelper.Int32,
                                                                                            "Topic partition id."));

        public static readonly Schema OFFSET_FETCH_REQUEST_TOPIC_V0 = new Schema(new Field("topic",
                                                                                        KafkaTypesHelper.String,
                                                                                        "Topic to fetch offset."),
                                                                              new Field("partitions",
                                                                                        new KafkaArrayOf(OFFSET_FETCH_REQUEST_PARTITION_V0),
                                                                                        "Partitions to fetch offsets."));

        public static readonly Schema OFFSET_FETCH_REQUEST_V0 = new Schema(new Field("group_id",
                                                                                  KafkaTypesHelper.String,
                                                                                  "The consumer group id."),
                                                                        new Field("topics",
                                                                                  new KafkaArrayOf(OFFSET_FETCH_REQUEST_TOPIC_V0),
                                                                                  "Topics to fetch offsets."));

        public static readonly Schema OFFSET_FETCH_RESPONSE_PARTITION_V0 = new Schema(new Field("partition",
                                                                                             KafkaTypesHelper.Int32,
                                                                                             "Topic partition id."),
                                                                                   new Field("offset",
                                                                                             KafkaTypesHelper.Int64,
                                                                                             "Last committed message offset."),
                                                                                   new Field("metadata",
                                                                                             KafkaTypesHelper.String,
                                                                                             "Any associated metadata the client wants to keep."),
                                                                                   new Field("error_code", KafkaTypesHelper.Int16));

        public static readonly Schema OFFSET_FETCH_RESPONSE_TOPIC_V0 = new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                               new Field("partition_responses",
                                                                                         new KafkaArrayOf(OFFSET_FETCH_RESPONSE_PARTITION_V0)));

        public static readonly Schema OFFSET_FETCH_RESPONSE_V0 = new Schema(new Field("responses",
                                                                                   new KafkaArrayOf(OFFSET_FETCH_RESPONSE_TOPIC_V0)));

        public static readonly Schema OFFSET_FETCH_REQUEST_V1 = OFFSET_FETCH_REQUEST_V0;
        public static readonly Schema OFFSET_FETCH_RESPONSE_V1 = OFFSET_FETCH_RESPONSE_V0;

        public static readonly Schema[] OFFSET_FETCH_REQUEST = new Schema[] { OFFSET_FETCH_REQUEST_V0, OFFSET_FETCH_REQUEST_V1 };
        public static readonly Schema[] OFFSET_FETCH_RESPONSE = new Schema[] { OFFSET_FETCH_RESPONSE_V0, OFFSET_FETCH_RESPONSE_V1 };

        /* List offset api */
        public static readonly Schema LIST_OFFSET_REQUEST_PARTITION_V0 = new Schema(new Field("partition",
                                                                                           KafkaTypesHelper.Int32,
                                                                                           "Topic partition id."),
                                                                                 new Field("timestamp", KafkaTypesHelper.Int64, "Timestamp."),
                                                                                 new Field("max_num_offsets",
                                                                                           KafkaTypesHelper.Int32,
                                                                                           "Maximum offsets to return."));

        public static readonly Schema LIST_OFFSET_REQUEST_TOPIC_V0 = new Schema(new Field("topic",
                                                                                       KafkaTypesHelper.String,
                                                                                       "Topic to list offset."),
                                                                             new Field("partitions",
                                                                                       new KafkaArrayOf(LIST_OFFSET_REQUEST_PARTITION_V0),
                                                                                       "Partitions to list offset."));

        public static readonly Schema LIST_OFFSET_REQUEST_V0 = new Schema(new Field("replica_id",
                                                                                 KafkaTypesHelper.Int32,
                                                                                 "Broker id of the follower. For normal consumers, use -1."),
                                                                       new Field("topics",
                                                                                 new KafkaArrayOf(LIST_OFFSET_REQUEST_TOPIC_V0),
                                                                                 "Topics to list offsets."));

        public static readonly Schema LIST_OFFSET_RESPONSE_PARTITION_V0 = new Schema(new Field("partition",
                                                                                            KafkaTypesHelper.Int32,
                                                                                            "Topic partition id."),
                                                                                  new Field("error_code", KafkaTypesHelper.Int16),
                                                                                  new Field("offsets",
                                                                                            new KafkaArrayOf(KafkaTypesHelper.Int64),
                                                                                            "A list of offsets."));

        public static readonly Schema LIST_OFFSET_RESPONSE_TOPIC_V0 = new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                              new Field("partition_responses",
                                                                                        new KafkaArrayOf(LIST_OFFSET_RESPONSE_PARTITION_V0)));

        public static readonly Schema LIST_OFFSET_RESPONSE_V0 = new Schema(new Field("responses",
                                                                                  new KafkaArrayOf(LIST_OFFSET_RESPONSE_TOPIC_V0)));

        public static readonly Schema[] LIST_OFFSET_REQUEST = new Schema[] { LIST_OFFSET_REQUEST_V0 };
        public static readonly Schema[] LIST_OFFSET_RESPONSE = new Schema[] { LIST_OFFSET_RESPONSE_V0 };

        /* Fetch api */
        public static readonly Schema FETCH_REQUEST_PARTITION_V0 = new Schema(new Field("partition",
                                                                                     KafkaTypesHelper.Int32,
                                                                                     "Topic partition id."),
                                                                           new Field("fetch_offset",
                                                                                     KafkaTypesHelper.Int64,
                                                                                     "Message offset."),
                                                                           new Field("max_KafkaTypesHelper.Bytes",
                                                                                     KafkaTypesHelper.Int32,
                                                                                     "Maximum KafkaTypesHelper.Bytes to fetch."));

        public static readonly Schema FETCH_REQUEST_TOPIC_V0 = new Schema(new Field("topic", KafkaTypesHelper.String, "Topic to fetch."),
                                                                       new Field("partitions",
                                                                                 new KafkaArrayOf(FETCH_REQUEST_PARTITION_V0),
                                                                                 "Partitions to fetch."));

        public static readonly Schema FETCH_REQUEST_V0 = new Schema(new Field("replica_id",
                                                                           KafkaTypesHelper.Int32,
                                                                           "Broker id of the follower. For normal consumers, use -1."),
                                                                 new Field("max_wait_time",
                                                                           KafkaTypesHelper.Int32,
                                                                           "Maximum time in ms to wait for the response."),
                                                                 new Field("min_KafkaTypesHelper.Bytes",
                                                                           KafkaTypesHelper.Int32,
                                                                           "Minimum KafkaTypesHelper.Bytes to accumulate in the response."),
                                                                 new Field("topics",
                                                                           new KafkaArrayOf(FETCH_REQUEST_TOPIC_V0),
                                                                           "Topics to fetch."));

        // The V1 Fetch Request body is the same as V0.
        // Only the version number is incremented to indicate a newer client
        public static readonly Schema FETCH_REQUEST_V1 = FETCH_REQUEST_V0;
        public static readonly Schema FETCH_RESPONSE_PARTITION_V0 = new Schema(new Field("partition",
                                                                                      KafkaTypesHelper.Int32,
                                                                                      "Topic partition id."),
                                                                            new Field("error_code", KafkaTypesHelper.Int16),
                                                                            new Field("high_watermark",
                                                                                      KafkaTypesHelper.Int64,
                                                                                      "Last committed offset."),
                                                                            new Field("record_set", KafkaTypesHelper.Bytes));

        public static readonly Schema FETCH_RESPONSE_TOPIC_V0 = new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                        new Field("partition_responses",
                                                                                  new KafkaArrayOf(FETCH_RESPONSE_PARTITION_V0)));

        public static readonly Schema FETCH_RESPONSE_V0 = new Schema(new Field("responses",
                                                                            new KafkaArrayOf(FETCH_RESPONSE_TOPIC_V0)));
        public static readonly Schema FETCH_RESPONSE_V1 = new Schema(new Field("throttle_time_ms",
                                                                            KafkaTypesHelper.Int32,
                                                                            "Duration in milliseconds for which the request was throttled" +
                                                                                " due to quota violation. (Zero if the request did not violate any quota.)",
                                                                            0),
                                                                  new Field("responses",
                                                                          new KafkaArrayOf(FETCH_RESPONSE_TOPIC_V0)));

        public static readonly Schema[] FETCH_REQUEST = new Schema[] { FETCH_REQUEST_V0, FETCH_REQUEST_V1 };
        public static readonly Schema[] FETCH_RESPONSE = new Schema[] { FETCH_RESPONSE_V0, FETCH_RESPONSE_V1 };

        /* List groups api */
        public static readonly Schema LIST_GROUPS_REQUEST_V0 = new Schema();

        public static readonly Schema LIST_GROUPS_RESPONSE_GROUP_V0 = new Schema(new Field("group_id", KafkaTypesHelper.String),
                                                                              new Field("protocol_type", KafkaTypesHelper.String));
        public static readonly Schema LIST_GROUPS_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16),
                                                                        new Field("groups", new KafkaArrayOf(LIST_GROUPS_RESPONSE_GROUP_V0)));

        public static readonly Schema[] LIST_GROUPS_REQUEST = new Schema[] { LIST_GROUPS_REQUEST_V0 };
        public static readonly Schema[] LIST_GROUPS_RESPONSE = new Schema[] { LIST_GROUPS_RESPONSE_V0 };

        /* Describe group api */
        public static readonly Schema DESCRIBE_GROUPS_REQUEST_V0 = new Schema(new Field("group_ids",
                                                                                     new KafkaArrayOf(KafkaTypesHelper.String),
                                                                                     "List of groupIds to request metadata for (an empty groupId array will return empty group metadata)."));

        public static readonly Schema DESCRIBE_GROUPS_RESPONSE_MEMBER_V0 = new Schema(new Field("member_id",
                                                                                             KafkaTypesHelper.String,
                                                                                             "The memberId assigned by the coordinator"),
                                                                                   new Field("client_id",
                                                                                             KafkaTypesHelper.String,
                                                                                             "The client id used in the member's latest join group request"),
                                                                                   new Field("client_host",
                                                                                             KafkaTypesHelper.String,
                                                                                             "The client host used in the request session corresponding to the member's join group."),
                                                                                   new Field("member_metadata",
                                                                                             KafkaTypesHelper.Bytes,
                                                                                             "The metadata corresponding to the current group protocol in use (will only be present if the group is stable)."),
                                                                                   new Field("member_assignment",
                                                                                             KafkaTypesHelper.Bytes,
                                                                                             "The current assignment provided by the group leader (will only be present if the group is stable)."));

        public static readonly Schema DESCRIBE_GROUPS_RESPONSE_GROUP_METADATA_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16),
                                                                                           new Field("group_id",
                                                                                                     KafkaTypesHelper.String),
                                                                                           new Field("state",
                                                                                                     KafkaTypesHelper.String,
                                                                                                     "The current state of the group (one of: Dead, Stable, AwaitingSync, or PreparingRebalance, or empty if there is no active group)"),
                                                                                           new Field("protocol_type",
                                                                                                     KafkaTypesHelper.String,
                                                                                                     "The current group protocol type (will be empty if the there is no active group)"),
                                                                                           new Field("protocol",
                                                                                                     KafkaTypesHelper.String,
                                                                                                     "The current group protocol (only provided if the group is Stable)"),
                                                                                           new Field("members",
                                                                                                     new KafkaArrayOf(DESCRIBE_GROUPS_RESPONSE_MEMBER_V0),
                                                                                                     "Current group members (only provided if the group is not Dead)"));

        public static readonly Schema DESCRIBE_GROUPS_RESPONSE_V0 = new Schema(new Field("groups", new KafkaArrayOf(DESCRIBE_GROUPS_RESPONSE_GROUP_METADATA_V0)));

        public static readonly Schema[] DESCRIBE_GROUPS_REQUEST = new Schema[] { DESCRIBE_GROUPS_REQUEST_V0 };
        public static readonly Schema[] DESCRIBE_GROUPS_RESPONSE = new Schema[] { DESCRIBE_GROUPS_RESPONSE_V0 };

        /* Group coordinator api */
        public static readonly Schema GROUP_COORDINATOR_REQUEST_V0 = new Schema(new Field("group_id",
                                                                                       KafkaTypesHelper.String,
                                                                                       "The unique group id."));

        public static readonly Schema GROUP_COORDINATOR_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16),
                                                                              new Field("coordinator",
                                                                                        BROKER,
                                                                                        "Host and port information for the coordinator for a consumer group."));

        public static readonly Schema[] GROUP_COORDINATOR_REQUEST = new Schema[] { GROUP_COORDINATOR_REQUEST_V0 };
        public static readonly Schema[] GROUP_COORDINATOR_RESPONSE = new Schema[] { GROUP_COORDINATOR_RESPONSE_V0 };

        /* Controlled shutdown api */
        public static readonly Schema CONTROLLED_SHUTDOWN_REQUEST_V1 = new Schema(new Field("broker_id",
                                                                                         KafkaTypesHelper.Int32,
                                                                                         "The id of the broker for which controlled shutdown has been requested."));

        public static readonly Schema CONTROLLED_SHUTDOWN_PARTITION_V1 = new Schema(new Field("topic", KafkaTypesHelper.String),
                                                                                 new Field("partition",
                                                                                           KafkaTypesHelper.Int32,
                                                                                           "Topic partition id."));

        public static readonly Schema CONTROLLED_SHUTDOWN_RESPONSE_V1 = new Schema(new Field("error_code", KafkaTypesHelper.Int16),
                                                                                new Field("partitions_remaining",
                                                                                          new KafkaArrayOf(CONTROLLED_SHUTDOWN_PARTITION_V1),
                                                                                          "The partitions that the broker still leads."));

        /* V0 is not supported as it would require changes to the request header not to include `clientId` */
        public static readonly Schema[] CONTROLLED_SHUTDOWN_REQUEST = new Schema[] { null, CONTROLLED_SHUTDOWN_REQUEST_V1 };
        public static readonly Schema[] CONTROLLED_SHUTDOWN_RESPONSE = new Schema[] { null, CONTROLLED_SHUTDOWN_RESPONSE_V1 };

        /* Join group api */
        public static readonly Schema JOIN_GROUP_REQUEST_PROTOCOL_V0 = new Schema(new Field("protocol_name", KafkaTypesHelper.String),
                                                                               new Field("protocol_metadata", KafkaTypesHelper.Bytes));

        public static readonly Schema JOIN_GROUP_REQUEST_V0 = new Schema(new Field("group_id",
                                                                                KafkaTypesHelper.String,
                                                                                "The group id."),
                                                                      new Field("session_timeout",
                                                                                KafkaTypesHelper.Int32,
                                                                                "The coordinator considers the consumer dead if it receives no heartbeat after this timeout in ms."),
                                                                      new Field("member_id",
                                                                                KafkaTypesHelper.String,
                                                                                "The assigned consumer id or an empty KafkaTypesHelper.String for a new consumer."),
                                                                      new Field("protocol_type",
                                                                                KafkaTypesHelper.String,
                                                                                "Unique name for class of protocols implemented by group"),
                                                                      new Field("group_protocols",
                                                                                new KafkaArrayOf(JOIN_GROUP_REQUEST_PROTOCOL_V0),
                                                                                "List of protocols that the member supports"));


        public static readonly Schema JOIN_GROUP_RESPONSE_MEMBER_V0 = new Schema(new Field("member_id", KafkaTypesHelper.String),
                                                                              new Field("member_metadata", KafkaTypesHelper.Bytes));
        public static readonly Schema JOIN_GROUP_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16),
                                                                       new Field("generation_id",
                                                                                 KafkaTypesHelper.Int32,
                                                                                 "The generation of the consumer group."),
                                                                       new Field("group_protocol",
                                                                                 KafkaTypesHelper.String,
                                                                                 "The group protocol selected by the coordinator"),
                                                                       new Field("leader_id",
                                                                                 KafkaTypesHelper.String,
                                                                                 "The leader of the group"),
                                                                       new Field("member_id",
                                                                                 KafkaTypesHelper.String,
                                                                                 "The consumer id assigned by the group coordinator."),
                                                                       new Field("members",
                                                                                 new KafkaArrayOf(JOIN_GROUP_RESPONSE_MEMBER_V0)));

        public static readonly Schema[] JOIN_GROUP_REQUEST = new Schema[] { JOIN_GROUP_REQUEST_V0 };
        public static readonly Schema[] JOIN_GROUP_RESPONSE = new Schema[] { JOIN_GROUP_RESPONSE_V0 };

        /* SyncGroup api */
        public static readonly Schema SYNC_GROUP_REQUEST_MEMBER_V0 = new Schema(new Field("member_id", KafkaTypesHelper.String),
                                                                             new Field("member_assignment", KafkaTypesHelper.Bytes));
        public static readonly Schema SYNC_GROUP_REQUEST_V0 = new Schema(new Field("group_id", KafkaTypesHelper.String),
                                                                      new Field("generation_id", KafkaTypesHelper.Int32),
                                                                      new Field("member_id", KafkaTypesHelper.String),
                                                                      new Field("group_assignment", new KafkaArrayOf(SYNC_GROUP_REQUEST_MEMBER_V0)));
        public static readonly Schema SYNC_GROUP_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16),
                                                                       new Field("member_assignment", KafkaTypesHelper.Bytes));
        public static readonly Schema[] SYNC_GROUP_REQUEST = new Schema[] { SYNC_GROUP_REQUEST_V0 };
        public static readonly Schema[] SYNC_GROUP_RESPONSE = new Schema[] { SYNC_GROUP_RESPONSE_V0 };

        /* Heartbeat api */
        public static readonly Schema HEARTBEAT_REQUEST_V0 = new Schema(new Field("group_id", KafkaTypesHelper.String, "The group id."),
                                                                     new Field("group_generation_id",
                                                                               KafkaTypesHelper.Int32,
                                                                               "The generation of the group."),
                                                                     new Field("member_id",
                                                                               KafkaTypesHelper.String,
                                                                               "The member id assigned by the group coordinator."));

        public static readonly Schema HEARTBEAT_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16));

        public static readonly Schema[] HEARTBEAT_REQUEST = new Schema[] { HEARTBEAT_REQUEST_V0 };
        public static readonly Schema[] HEARTBEAT_RESPONSE = new Schema[] { HEARTBEAT_RESPONSE_V0 };

        /* Leave group api */
        public static readonly Schema LEAVE_GROUP_REQUEST_V0 = new Schema(new Field("group_id", KafkaTypesHelper.String, "The group id."),
                                                                       new Field("member_id",
                                                                                 KafkaTypesHelper.String,
                                                                                 "The member id assigned by the group coordinator."));

        public static readonly Schema LEAVE_GROUP_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16));

        public static readonly Schema[] LEAVE_GROUP_REQUEST = new Schema[] { LEAVE_GROUP_REQUEST_V0 };
        public static readonly Schema[] LEAVE_GROUP_RESPONSE = new Schema[] { LEAVE_GROUP_RESPONSE_V0 };

        /* Leader and ISR api */
        public static readonly Schema LEADER_AND_ISR_REQUEST_PARTITION_STATE_V0 =
                new Schema(new Field("topic", KafkaTypesHelper.String, "Topic name."),
                           new Field("partition", KafkaTypesHelper.Int32, "Topic partition id."),
                           new Field("controller_epoch", KafkaTypesHelper.Int32, "The controller epoch."),
                           new Field("leader", KafkaTypesHelper.Int32, "The broker id for the leader."),
                           new Field("leader_epoch", KafkaTypesHelper.Int32, "The leader epoch."),
                           new Field("isr", new KafkaArrayOf(KafkaTypesHelper.Int32), "The in sync replica ids."),
                           new Field("zk_version", KafkaTypesHelper.Int32, "The ZK version."),
                           new Field("replicas", new KafkaArrayOf(KafkaTypesHelper.Int32), "The replica ids."));

        public static readonly Schema LEADER_AND_ISR_REQUEST_LIVE_LEADER_V0 =
                new Schema(new Field("id", KafkaTypesHelper.Int32, "The broker id."),
                           new Field("host", KafkaTypesHelper.String, "The hostname of the broker."),
                           new Field("port", KafkaTypesHelper.Int32, "The port on which the broker accepts requests."));

        public static readonly Schema LEADER_AND_ISR_REQUEST_V0 = new Schema(new Field("controller_id", KafkaTypesHelper.Int32, "The controller id."),
                                                                          new Field("controller_epoch", KafkaTypesHelper.Int32, "The controller epoch."),
                                                                          new Field("partition_states",
                                                                                    new KafkaArrayOf(LEADER_AND_ISR_REQUEST_PARTITION_STATE_V0)),
                                                                          new Field("live_leaders", new KafkaArrayOf(LEADER_AND_ISR_REQUEST_LIVE_LEADER_V0)));

        public static readonly Schema LEADER_AND_ISR_RESPONSE_PARTITION_V0 = new Schema(new Field("topic", KafkaTypesHelper.String, "Topic name."),
                                                                                     new Field("partition", KafkaTypesHelper.Int32, "Topic partition id."),
                                                                                     new Field("error_code", KafkaTypesHelper.Int16, "Error code."));

        public static readonly Schema LEADER_AND_ISR_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16, "Error code."),
                                                                           new Field("partitions",
                                                                                     new KafkaArrayOf(LEADER_AND_ISR_RESPONSE_PARTITION_V0)));

        public static readonly Schema[] LEADER_AND_ISR_REQUEST = new Schema[] { LEADER_AND_ISR_REQUEST_V0 };
        public static readonly Schema[] LEADER_AND_ISR_RESPONSE = new Schema[] { LEADER_AND_ISR_RESPONSE_V0 };

        /* Replica api */
        public static readonly Schema STOP_REPLICA_REQUEST_PARTITION_V0 = new Schema(new Field("topic", KafkaTypesHelper.String, "Topic name."),
                                                                                  new Field("partition", KafkaTypesHelper.Int32, "Topic partition id."));

        public static readonly Schema STOP_REPLICA_REQUEST_V0 = new Schema(new Field("controller_id", KafkaTypesHelper.Int32, "The controller id."),
                                                                        new Field("controller_epoch", KafkaTypesHelper.Int32, "The controller epoch."),
                                                                        new Field("delete_partitions",
                                                                                  KafkaTypesHelper.Int8,
                                                                                  "Boolean which indicates if replica's partitions must be deleted."),
                                                                        new Field("partitions",
                                                                                  new KafkaArrayOf(STOP_REPLICA_REQUEST_PARTITION_V0)));

        public static readonly Schema STOP_REPLICA_RESPONSE_PARTITION_V0 = new Schema(new Field("topic", KafkaTypesHelper.String, "Topic name."),
                                                                                   new Field("partition", KafkaTypesHelper.Int32, "Topic partition id."),
                                                                                   new Field("error_code", KafkaTypesHelper.Int16, "Error code."));

        public static readonly Schema STOP_REPLICA_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16, "Error code."),
                                                                         new Field("partitions",
                                                                                   new KafkaArrayOf(STOP_REPLICA_RESPONSE_PARTITION_V0)));

        public static readonly Schema[] STOP_REPLICA_REQUEST = new Schema[] { STOP_REPLICA_REQUEST_V0 };
        public static readonly Schema[] STOP_REPLICA_RESPONSE = new Schema[] { STOP_REPLICA_RESPONSE_V0 };

        /* Update metadata api */

        public static readonly Schema UPDATE_METADATA_REQUEST_PARTITION_STATE_V0 = LEADER_AND_ISR_REQUEST_PARTITION_STATE_V0;

        public static readonly Schema UPDATE_METADATA_REQUEST_BROKER_V0 =
                new Schema(new Field("id", KafkaTypesHelper.Int32, "The broker id."),
                           new Field("host", KafkaTypesHelper.String, "The hostname of the broker."),
                           new Field("port", KafkaTypesHelper.Int32, "The port on which the broker accepts requests."));

        public static readonly Schema UPDATE_METADATA_REQUEST_V0 = new Schema(new Field("controller_id", KafkaTypesHelper.Int32, "The controller id."),
                                                                           new Field("controller_epoch", KafkaTypesHelper.Int32, "The controller epoch."),
                                                                           new Field("partition_states",
                                                                                     new KafkaArrayOf(UPDATE_METADATA_REQUEST_PARTITION_STATE_V0)),
                                                                           new Field("live_brokers",
                                                                                     new KafkaArrayOf(UPDATE_METADATA_REQUEST_BROKER_V0)));

        public static readonly Schema UPDATE_METADATA_RESPONSE_V0 = new Schema(new Field("error_code", KafkaTypesHelper.Int16, "Error code."));

        public static readonly Schema UPDATE_METADATA_REQUEST_PARTITION_STATE_V1 = UPDATE_METADATA_REQUEST_PARTITION_STATE_V0;

        public static readonly Schema UPDATE_METADATA_REQUEST_END_POINT_V1 =
                // for some reason, V1 sends `port` before `host` while V0 sends `host` before `port
                new Schema(new Field("port", KafkaTypesHelper.Int32, "The port on which the broker accepts requests."),
                           new Field("host", KafkaTypesHelper.String, "The hostname of the broker."),
                           new Field("security_protocol_type", KafkaTypesHelper.Int16, "The security protocol type."));

        public static readonly Schema UPDATE_METADATA_REQUEST_BROKER_V1 =
                new Schema(new Field("id", KafkaTypesHelper.Int32, "The broker id."),
                           new Field("end_points", new KafkaArrayOf(UPDATE_METADATA_REQUEST_END_POINT_V1)));

        public static readonly Schema UPDATE_METADATA_REQUEST_V1 = new Schema(new Field("controller_id", KafkaTypesHelper.Int32, "The controller id."),
                                                                           new Field("controller_epoch", KafkaTypesHelper.Int32, "The controller epoch."),
                                                                           new Field("partition_states",
                                                                                     new KafkaArrayOf(UPDATE_METADATA_REQUEST_PARTITION_STATE_V1)),
                                                                           new Field("live_brokers",
                                                                                     new KafkaArrayOf(UPDATE_METADATA_REQUEST_BROKER_V1)));

        public static readonly Schema UPDATE_METADATA_RESPONSE_V1 = UPDATE_METADATA_RESPONSE_V0;

        public static readonly Schema[] UPDATE_METADATA_REQUEST = new Schema[] { UPDATE_METADATA_REQUEST_V0, UPDATE_METADATA_REQUEST_V1 };
        public static readonly Schema[] UPDATE_METADATA_RESPONSE = new Schema[] { UPDATE_METADATA_RESPONSE_V0, UPDATE_METADATA_RESPONSE_V1 };

        /* an array of all requests and responses with all schema versions; a null value in the inner array means that the
         * particular version is not supported */
        public static readonly Schema[][] REQUESTS = new Schema[ApiKeysHelper.MaxApiKey + 1][];
        public static readonly Schema[][] RESPONSES = new Schema[ApiKeysHelper.MaxApiKey + 1][];

        /* the latest version of each api */
        public static readonly short[] CURR_VERSION = new short[ApiKeysHelper.MaxApiKey + 1];

        static Protocol()
        {
            REQUESTS[(int)ApiKeys.Produce] = PRODUCE_REQUEST;
            REQUESTS[(int)ApiKeys.Fetch] = FETCH_REQUEST;
            REQUESTS[(int)ApiKeys.ListOffsets] = LIST_OFFSET_REQUEST;
            REQUESTS[(int)ApiKeys.Metadata] = METADATA_REQUEST;
            REQUESTS[(int)ApiKeys.LeaderAndIsr] = LEADER_AND_ISR_REQUEST;
            REQUESTS[(int)ApiKeys.StopReplica] = STOP_REPLICA_REQUEST;
            REQUESTS[(int)ApiKeys.UpdateMetadata] = UPDATE_METADATA_REQUEST;
            REQUESTS[(int)ApiKeys.ControlledShutdown] = CONTROLLED_SHUTDOWN_REQUEST;
            REQUESTS[(int)ApiKeys.OffsetCommit] = OFFSET_COMMIT_REQUEST;
            REQUESTS[(int)ApiKeys.OffsetFetch] = OFFSET_FETCH_REQUEST;
            REQUESTS[(int)ApiKeys.GroupCoordinator] = GROUP_COORDINATOR_REQUEST;
            REQUESTS[(int)ApiKeys.JoinGroup] = JOIN_GROUP_REQUEST;
            REQUESTS[(int)ApiKeys.Heartbeat] = HEARTBEAT_REQUEST;
            REQUESTS[(int)ApiKeys.LeaveGroup] = LEAVE_GROUP_REQUEST;
            REQUESTS[(int)ApiKeys.SyncGroup] = SYNC_GROUP_REQUEST;
            REQUESTS[(int)ApiKeys.DescribeGroups] = DESCRIBE_GROUPS_REQUEST;
            REQUESTS[(int)ApiKeys.ListGroups] = LIST_GROUPS_REQUEST;

            RESPONSES[(int)ApiKeys.Produce] = PRODUCE_RESPONSE;
            RESPONSES[(int)ApiKeys.Fetch] = FETCH_RESPONSE;
            RESPONSES[(int)ApiKeys.ListOffsets] = LIST_OFFSET_RESPONSE;
            RESPONSES[(int)ApiKeys.Metadata] = METADATA_RESPONSE;
            RESPONSES[(int)ApiKeys.LeaderAndIsr] = LEADER_AND_ISR_RESPONSE;
            RESPONSES[(int)ApiKeys.StopReplica] = STOP_REPLICA_RESPONSE;
            RESPONSES[(int)ApiKeys.UpdateMetadata] = UPDATE_METADATA_RESPONSE;
            RESPONSES[(int)ApiKeys.ControlledShutdown] = CONTROLLED_SHUTDOWN_RESPONSE;
            RESPONSES[(int)ApiKeys.OffsetCommit] = OFFSET_COMMIT_RESPONSE;
            RESPONSES[(int)ApiKeys.OffsetFetch] = OFFSET_FETCH_RESPONSE;
            RESPONSES[(int)ApiKeys.GroupCoordinator] = GROUP_COORDINATOR_RESPONSE;
            RESPONSES[(int)ApiKeys.JoinGroup] = JOIN_GROUP_RESPONSE;
            RESPONSES[(int)ApiKeys.Heartbeat] = HEARTBEAT_RESPONSE;
            RESPONSES[(int)ApiKeys.LeaveGroup] = LEAVE_GROUP_RESPONSE;
            RESPONSES[(int)ApiKeys.SyncGroup] = SYNC_GROUP_RESPONSE;
            RESPONSES[(int)ApiKeys.DescribeGroups] = DESCRIBE_GROUPS_RESPONSE;
            RESPONSES[(int)ApiKeys.ListGroups] = LIST_GROUPS_RESPONSE;


            /* set the maximum version of each api */
            foreach (ApiKeys api in Enum.GetValues(typeof(ApiKeys)).Cast<ApiKeys>())
                CURR_VERSION[(int)api] = (short)(REQUESTS[(int)api].Length - 1);

            /* sanity check that we have the same number of request and response versions for each api */
            foreach (ApiKeys api in Enum.GetValues(typeof(ApiKeys)).Cast<ApiKeys>())
                if (REQUESTS[(int)api].Length != RESPONSES[(int)api].Length)
                    throw new KafkaException(REQUESTS[(int)api].Length + " request versions for api " + api.ToString()
                            + " but " + RESPONSES[(int)api].Length + " response versions.");
        }
    }
}
