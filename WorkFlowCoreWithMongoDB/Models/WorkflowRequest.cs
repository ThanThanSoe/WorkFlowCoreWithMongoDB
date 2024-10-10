namespace WorkFlowCoreWithMongoDB.Models
{
    public class WorkflowRequest
    {
        public string ActionType { get; set; } // The action type to determine which step to start
        public string Status { get; set; }     // To track the workflow's current status

        public string WorkflowId { get; set; }

        public string ActionId { get; set; }
    }
}
