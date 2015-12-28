namespace DotKafka.Prototype.Common
{
    public class Node
    {
        public int Id { get; private set; }
        public string IdString { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }

        public Node(int id, string host, int port)
        {
            Id = id;
            IdString = id.ToString();
            Host = host;
            Port = port;
        }

        public static Node NoNode()
        {
            return new Node(-1, "", -1);
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((Host == null) ? 0 : Host.GetHashCode());
            result = prime * result + Id;
            result = prime * result + Port;
            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Node;

            if (other == null)
                return false;

            if (Id != other.Id)
                return false;

            if (Host != other.Host)
                return false;

            if (Port != other.Port)
                return false;

            return true;
        }

        public override string ToString()
        {
            return "Node(" + Id + ", " + Host + ", " + Port + ")"; 
        }
    }
}
