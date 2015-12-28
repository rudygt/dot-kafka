using System.Linq;
using System.Text;

namespace DotKafka.Prototype.Common
{
    public class PartitionInfo
    {
        public string Topic { get; private set; }
        public int Partition { get; private set; }
        public Node Leader { get; private set; }
        public Node[] Replicas { get; private set; }
        public Node[] InSyncReplicas { get; private set; }

        public PartitionInfo(string topic, int partition, Node leader, Node[] replicas, Node[] inSyncReplicas)
        {
            Topic = topic;
            Partition = partition;
            Leader = leader;
            Replicas = replicas;
            InSyncReplicas = inSyncReplicas;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Partition(topic = {0}, partition = {1}, leader = {2}", Topic, Partition, Leader == null ? "none" : Leader.Id.ToString());

            builder.Append(", replicas = [" + string.Join(",", Replicas.Select(item => item.Id)) + "]");
            builder.Append(", inSyncReplicas = [" + string.Join(",", InSyncReplicas.Select(item => item.Id)) + "])");            
            return builder.ToString();
        }
    }
}
