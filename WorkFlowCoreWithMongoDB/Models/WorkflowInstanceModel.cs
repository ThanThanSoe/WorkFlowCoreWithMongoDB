using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WorkFlowCoreWithMongoDB.Models
{
    public class WorkflowInstanceModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string WorkflowId { get; set; }
        public string ActionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } // Track status like "Running", "Completed", "Failed"
        public string CurrentStepId { get; set; } // Add this to track the current step
    }
}
