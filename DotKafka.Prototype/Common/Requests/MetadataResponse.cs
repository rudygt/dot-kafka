using DotKafka.Prototype.Common.Protocol;
using DotKafka.Prototype.Common.Protocol.Types;
using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Requests
{
    public class MetadataResponse : AbstractRequestResponse
    {
        private static Schema CurrentSchema = Protocol.ProtoUtils.currentResponseSchema((int)ApiKeys.Metadata);
        private static string BrokersKeyName = "brokers";
        private static string TopicMetadataKeyName = "topic_metadata";

        private static string NodeIdKeyName = "node_id";
        private static string HostKeyName = "host";
        private static string PortKeyName = "port";

        private static string TopicErrorCodeKeyName = "topic_error_code";

        private static string TopicKeyName = "topic";
        private static string PartitionMetadataKeyName = "partition_metadata";

        private static string PartitionErrorCodeKeyName = "partition_error_code";

        private static string PartitionKeyName = "partition_id";
        private static string LeaderKeyName = "leader";
        private static string ReplicasKeyName = "replicas";
        private static string ISRKeyName = "isr";

        private Cluster cluster;
        private Dictionary<string, Error> errors;

        public MetadataResponse(Cluster cluster, Dictionary<string, Error> errors) : base(new Struct(CurrentSchema))
        {
            var brokerArray = new List<Struct>();

            foreach (var node in cluster.Nodes)
            {
                var broker = _struct.Instance(BrokersKeyName);
                broker.Set(NodeIdKeyName, node.Id);
                broker.Set(HostKeyName, node.Host);
                broker.Set(PortKeyName, node.Port);
                brokerArray.Add(broker);
            }

            _struct.Set(BrokersKeyName, brokerArray.ToArray());

            var topicArray = new List<Struct>();

            foreach (var errorEntry in errors)
            {
                var topicData = _struct.Instance(TopicMetadataKeyName);
                topicData.Set(TopicKeyName, errorEntry.Key);
                topicData.Set(TopicErrorCodeKeyName, (int)errorEntry.Value);
                topicData.Set(PartitionMetadataKeyName, new Struct[0]);
                topicArray.Add(topicData);
            }

            foreach (var topic in cluster.GetTopics())
            {
                if (errors.ContainsKey(topic))
                    continue;

                var topicData = _struct.Instance(TopicMetadataKeyName);
                topicData.Set(TopicKeyName, topic);
                topicData.Set(TopicErrorCodeKeyName, (int)Error.None);

                var partitionArray = new List<Struct>();

                foreach (var fetchPartitionData in cluster.GetPartitionsForTopic(topic))
                {
                    var partitionData = topicData.Instance(PartitionMetadataKeyName);
                    partitionData.Set(PartitionErrorCodeKeyName, (int)Error.None);
                    partitionData.Set(PartitionKeyName, fetchPartitionData.Partition);
                    partitionData.Set(LeaderKeyName, fetchPartitionData.Leader.Id);

                    var replicas = new List<int>();

                    foreach (var node in fetchPartitionData.Replicas)
                    {
                        replicas.Add(node.Id);
                    }

                    partitionData.Set(ReplicasKeyName, replicas.ToArray());

                    var isr = new List<int>();

                    foreach (var node in fetchPartitionData.InSyncReplicas)
                    {
                        isr.Add(node.Id);
                    }

                    partitionData.Set(ISRKeyName, isr.ToArray());

                    partitionArray.Add(partitionData);
                }

                topicData.Set(PartitionMetadataKeyName, partitionArray.ToArray());

                topicArray.Add(topicData);
            }

            _struct.Set(TopicMetadataKeyName, topicArray.ToArray());

            this.cluster = cluster;
            this.errors = errors;
        }

        public MetadataResponse(Struct theStruct) : base(theStruct)
        {
            var errors = new Dictionary<string, Error>();
            var brokers = new Dictionary<int, Node>();

            var brokerStructs = (object[])_struct.Get(BrokersKeyName);

            for (int i = 0; i < brokerStructs.Length; i++)
            {
                var broker = (Struct)brokerStructs[i];
                var nodeId = broker.GetInt(NodeIdKeyName);
                var host = broker.GetString(HostKeyName);
                var port = broker.GetInt(PortKeyName);
                brokers[nodeId] = new Node(nodeId, host, port);
            }

            var partitions = new List<PartitionInfo>();

            var topicInfos = (object[])_struct.Get(TopicMetadataKeyName);

            for (int i = 0; i < topicInfos.Length; i++)
            {
                var topicInfo = (Struct)topicInfos[i];

                var topicError = topicInfo.GetShort(TopicErrorCodeKeyName);

                var topic = topicInfo.GetString(TopicKeyName);

                if (topicError == (short)Error.None)
                {
                    var partitionInfos = (object[])topicInfo.Get(PartitionMetadataKeyName);
                    for (int j = 0; j < partitionInfos.Length; j++)
                    {
                        var partitionInfo = (Struct)partitionInfos[j];

                        var partition = partitionInfo.GetInt(PartitionKeyName);
                        var leader = partitionInfo.GetInt(LeaderKeyName);

                        Node leaderNode = leader == -1 ? null : brokers[leader];

                        var replicas = (object[])partitionInfo.Get(ReplicasKeyName);

                        var replicaNodes = new Node[replicas.Length];

                        for (int k = 0; k < replicas.Length; k++)
                        {
                            replicaNodes[k] = brokers[(int)replicas[k]];
                        }

                        var isr = (object[])partitionInfo.Get(ISRKeyName);

                        var isrNodes = new Node[isr.Length];

                        for (int k = 0; k < isr.Length; k++)
                        {
                            isrNodes[k] = brokers[(int)isr[k]];
                        }

                        partitions.Add(new PartitionInfo(topic, partition, leaderNode, replicaNodes, isrNodes));
                    }
                }
                else
                {
                    errors[topic] = (Error)topicError;
                }

            }

            this.errors = errors;
            this.cluster = new Cluster(brokers.Values, partitions, GetUnauthorizedTopics(errors));
        }


        private HashSet<string> GetUnauthorizedTopics(Dictionary<string, Error> topicErrors)
        {

            HashSet<string> unauthorizedTopics = new HashSet<string>();

            foreach (var error in topicErrors)
            {
                if (error.Value == Error.TopicAuthorizationFailed)
                {
                    unauthorizedTopics.Add(error.Key);
                }
            }

            return unauthorizedTopics;
        }

    }
}
