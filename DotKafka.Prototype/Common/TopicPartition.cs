namespace DotKafka.Prototype.Common
{
    public class TopicPartition
    {
        private int Hash { get; set; }
        public int Partition { get; private set; }
        public string Topic { get; private set; }

        public TopicPartition(string topic, int partition)
        {
            Topic = topic;
            Partition = partition;
            Hash = CalculateHashCode(topic, partition);
        }

        private int CalculateHashCode(string topic, int partition)
        {
            int prime = 31;
            int result = 1;
            result = prime * result + partition;
            result = prime * result + ((topic == null) ? 0 : topic.GetHashCode());
            return result;
        }

        public override int GetHashCode()
        {
            return Hash;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TopicPartition;

            if (other == null)
                return false;

            if (Topic != other.Topic)
                return false;

            if (Partition != other.Partition)
                return false;

            return true;
        }

        public override string ToString()
        {
            return Topic + "-" + Partition;
        }
    }
}
