using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorkFlowCoreWithMongoDB.Models
{
    public class WorkflowInstanceModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string WorkflowId { get; set; }
        public string ActionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }       // e.g., Running, StepACompleted, StepBCompleted, etc.
        public string CurrentStepId { get; set; } // Optional: Track current step
        public string ActionId { get; set; }
    }
}
