using DotKafka.Prototype.Common.Errors;
using DotKafka.Prototype.Common.Protocol;
using DotKafka.Prototype.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotKafka.Prototype.Common.Requests
{

    public abstract class AbstractRequestResponse
    {
        protected readonly Struct _struct;


        public AbstractRequestResponse(Struct _struct)
        {
            this._struct = _struct;
        }

        public Struct ToStruct()
        {
            return _struct;
        }

        /**
         * Get the serialized size of this object
         */
        public int SizeOf()
        {
            return _struct.SizeOf();
        }

        /**
         * Write this object to a buffer
         */
        public void WriteTo(MemoryStream buffer)
        {
            _struct.WriteTo(buffer);
        }


        public new string ToString()
        {
            return _struct.ToString();
        }


        public new int GetHashCode()
        {
            return _struct.GetHashCode();
        }

        public new bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            AbstractRequestResponse other = obj as AbstractRequestResponse;
            if (other == null)
                return false;
            return _struct.Equals(other._struct);
        }
    }

    public abstract class AbstractRequest : AbstractRequestResponse
    {
        public AbstractRequest(Struct _struct) : base(_struct)
        {

        }

        public abstract AbstractRequestResponse GetErrorResponse(int versionId, Exception e);

        public static AbstractRequest GetRequest(int requestId, int versionId, MemoryStream buffer)
        {
            switch (ApiKeysHelper.ForId(requestId))
            {
                //case ApiKeys.Produce: return ProduceRequest.parse(buffer, versionId);
                //case ApiKeys.Fetch: return FetchRequest.parse(buffer, versionId);
                //case ApiKeys.ListOffsets: return ListOffsetRequest.parse(buffer, versionId);
                case ApiKeys.Metadata: return MetadataRequest.Parse(buffer, versionId);
                //case ApiKeys.OffsetCommit: return OffsetCommitRequest.parse(buffer, versionId);
                //case ApiKeys.OffsetFetch: return OffsetFetchRequest.parse(buffer, versionId);
                //case ApiKeys.GroupCoordinator: return GroupCoordinatorRequest.parse(buffer, versionId);
                /*case ApiKeys.JoinGroup: return JoinGroupRequest.parse(buffer, versionId);
                case ApiKeys.Heartbeat: return HeartbeatRequest.parse(buffer, versionId);
                case ApiKeys.LeaveGroup: return LeaveGroupRequest.parse(buffer, versionId);
                case ApiKeys.SyncGroup: return SyncGroupRequest.parse(buffer, versionId);
                case ApiKeys.StopReplica: return StopReplicaRequest.parse(buffer, versionId);
                case ApiKeys.ControlledShutdown: return ControlledShutdownRequest.parse(buffer, versionId);
                case ApiKeys.UpdateMetadata: return UpdateMetadataRequest.parse(buffer, versionId);
                case ApiKeys.LeaderAndIsr: return LeaderAndIsrRequest.parse(buffer, versionId);
                case ApiKeys.DescribeGroups: return DescribeGroupsRequest.parse(buffer, versionId);
                case ApiKeys.ListGroups: return ListGroupsRequest.parse(buffer, versionId);*/
                default:
                    return null;
            }
        }
    }

    public class MetadataRequest : AbstractRequest
    {

        private static readonly Schema CURRENT_SCHEMA = ProtoUtils.currentRequestSchema((int)ApiKeys.Metadata);
        private static readonly string TOPICS_KEY_NAME = "topics";

        private readonly List<string> topics;

        public MetadataRequest(List<string> topics) : base(new Struct(CURRENT_SCHEMA))
        {

            _struct.Set(TOPICS_KEY_NAME, topics);
            this.topics = topics;
        }

        public MetadataRequest(Struct s) : base(s)
        {

            object[] topicArray = s.GetArray(TOPICS_KEY_NAME);
            topics = new List<string>();
            foreach (object topicObj in topicArray)
            {
                topics.Add((string)topicObj);
            }
        }

        public override AbstractRequestResponse GetErrorResponse(int versionId, Exception e)
        {
            Dictionary<string, Error> topicErrors = new Dictionary<string, Error>();
            foreach (string topic in topics)
            {
                topicErrors[topic] = e.FromException() ;
            }

            Cluster cluster = new Cluster(new List<Node>(), new List<PartitionInfo>(), new HashSet<string>());

            switch (versionId)
            {
                case 0:
                    return new MetadataResponse(cluster, topicErrors);
                default:
                    throw new KafkaException(String.Format("Version {0} is not valid. Valid versions for {1} are 0 to {2}",
                            versionId, this.GetType().Name, ProtoUtils.latestVersion((int)ApiKeys.Metadata)));
            }
        }

        public List<String> Topics()
        {
            return topics;
        }

        public static MetadataRequest Parse(MemoryStream buffer, int versionId)
        {
            return new MetadataRequest(ProtoUtils.parseRequest((int)ApiKeys.Metadata, versionId, buffer));
        }

        public static MetadataRequest parse(MemoryStream buffer)
        {
            return new MetadataRequest((Struct)CURRENT_SCHEMA.Read(buffer));
        }

    }
}
