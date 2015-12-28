using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotKafka.Prototype.Common
{
    public class Cluster
    {
        public IList<Node> Nodes { get; private set; }
        public HashSet<string> UnauthorizedTopics { get; private set; }
        private Dictionary<TopicPartition, PartitionInfo> partitionsByTopicPartition;
        private Dictionary<string, IList<PartitionInfo>> partitionsByTopic;
        private Dictionary<string, IList<PartitionInfo>> availablePartitionsByTopic;
        private Dictionary<int, IList<PartitionInfo>> partitionsByNode;
        private Dictionary<int, Node> nodesById;

        public Cluster(IEnumerable<Node> nodes, IEnumerable<PartitionInfo> partitions, HashSet<string> unauthorizedTopics)
        {
            Nodes = new ReadOnlyCollection<Node>(nodes.ToList());

            nodesById = new Dictionary<int, Node>();

            foreach (var node in nodes)
            {
                nodesById[node.Id] = node;
            }

            partitionsByTopicPartition = new Dictionary<TopicPartition, PartitionInfo>();

            foreach (var partition in partitions)
            {
                partitionsByTopicPartition[new TopicPartition(partition.Topic, partition.Partition)] = partition;
            }

            Dictionary<string, IList<PartitionInfo>> partsForTopic = new Dictionary<string, IList<PartitionInfo>>();
            Dictionary<int, IList<PartitionInfo>> partsForNode = new Dictionary<int, IList<PartitionInfo>>();

            foreach (var node in nodes)
            {
                partsForNode[node.Id] = new List<PartitionInfo>();
            }

            foreach (var p in partitions)
            {
                if (!partsForTopic.ContainsKey(p.Topic))
                {
                    partsForTopic[p.Topic] = new List<PartitionInfo>();
                }

                var current = partsForTopic[p.Topic];
                current.Add(p);

                if (p.Leader != null)
                {
                    var psNode = Utils.Utils.NotNull(partsForNode[p.Leader.Id]);
                    psNode.Add(p);
                }
            }

            partitionsByTopic = new Dictionary<string, IList<PartitionInfo>>();

            availablePartitionsByTopic = new Dictionary<string, IList<PartitionInfo>>();

            foreach (var kvp in partsForTopic)
            {
                var topic = kvp.Key;

                var partitionList = kvp.Value;

                partitionsByTopic[topic] = new ReadOnlyCollection<PartitionInfo>(partitionList);

                var available = new List<PartitionInfo>();

                foreach (var part in partitionList)
                {
                    if (part.Leader != null)
                        available.Add(part);
                }

                availablePartitionsByTopic[topic] = available;
            }

            partitionsByNode = new Dictionary<int, IList<PartitionInfo>>();

            foreach (var kvp in partsForNode)
            {
                partitionsByNode[kvp.Key] = new ReadOnlyCollection<PartitionInfo>(kvp.Value);
            }

            UnauthorizedTopics = unauthorizedTopics;
        }

        public Node GetNodeById(int id)
        {
            return this.nodesById[id];
        }

        public Node GetLeaderFor(TopicPartition topicPartition)
        {
            PartitionInfo info = partitionsByTopicPartition[topicPartition];
            if (info == null)
                return null;
            else
                return info.Leader;
        }

        public PartitionInfo GetPartition(TopicPartition topicPartition)
        {
            return partitionsByTopicPartition[topicPartition];
        }

        public IList<PartitionInfo> GetPartitionsForTopic(string topic)
        {
            return partitionsByTopic[topic];
        }

        public IList<PartitionInfo> GetAvailablePartitionsForTopic(string topic)
        {
            return availablePartitionsByTopic[topic];
        }

        public IList<PartitionInfo> GetPartitionsForNode(int nodeId)
        {
            return partitionsByNode[nodeId];
        }

        public int GetNumberOfPartitionsForTopic(string topic)
        {
            var partitions = partitionsByTopic[topic];
            if (partitions == null)
                return -1;
            return partitions.Count;
        }

        public HashSet<string> GetTopics()
        {
            return new HashSet<string>(partitionsByTopic.Keys);
        }

        public static Cluster Empty()
        {
            return new Cluster(new List<Node>(), new List<PartitionInfo>(), new HashSet<string>());
        }

        public static Cluster Bootstrap(List<Uri> addresses)
        {
            var nodes = new List<Node>();

            int nodeId = -1;

            foreach (var address in addresses)
            {
                nodes.Add(new Node(nodeId--, address.Host, address.Port));
            }

            return new Cluster(nodes, new List<PartitionInfo>(), new HashSet<string>());
        }

        public override string ToString()
        {
            return "Cluster( nodes = " + Nodes.ToString() + ", partitions = " + this.partitionsByTopicPartition.Values.ToString() + ")";
        }
    }
}
