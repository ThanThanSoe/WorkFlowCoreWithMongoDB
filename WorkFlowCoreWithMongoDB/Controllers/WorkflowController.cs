using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WorkflowCore.Interface;
using WorkFlowCoreWithMongoDB.Models;

namespace WorkFlowCoreWithMongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowHost _workflowHost;
        private readonly IMongoCollection<WorkflowInstanceModel> _workflowCollection;

        public WorkflowController(IWorkflowHost workflowHost, IMongoClient mongoClient)
        {
            _workflowHost = workflowHost;
            var database = mongoClient.GetDatabase("YourDatabaseName");
            _workflowCollection = database.GetCollection<WorkflowInstanceModel>("workflows");
        }

        // Start the workflow and store the workflow ID in MongoDB
        [HttpPost("start-workflow")]
        public async Task<IActionResult> StartWorkflow([FromBody] WorkflowRequest request)
        {
            // Start the workflow
            var workflowId = await _workflowHost.StartWorkflow("DynamicWorkflow", request);

            // Save the workflow instance in MongoDB
            var workflowInstance = new WorkflowInstanceModel
            {
                WorkflowId = workflowId,
                ActionType = request.ActionType,
                CreatedAt = DateTime.UtcNow,
                Status = "Running" // Initial status when workflow is started
            };
            await _workflowCollection.InsertOneAsync(workflowInstance);

            return Ok(new { WorkflowId = workflowId });
        }

        // Check the workflow status using the workflow ID
        [HttpGet("check-status/{workflowId}")]
        public async Task<IActionResult> CheckWorkflowStatus(string workflowId)
        {
            // Retrieve the workflow instance from MongoDB by workflow ID
            var workflowInstance = await _workflowCollection.Find(w => w.WorkflowId == workflowId).FirstOrDefaultAsync();

            if (workflowInstance == null)
                return NotFound(new { Message = "Workflow not found." });

            // Here, you may need to update the status based on the execution progress
            // For example, you can check if the workflow is completed, failed, etc.

            // Get the current step in the workflow (you may need to adapt this part based on your implementation)
            var currentStepId = workflowInstance.CurrentStepId; // Update your model accordingly

            return Ok(new
            {
                WorkflowId = workflowId,
                CurrentStepId = currentStepId,
                CurrentStepStatus = workflowInstance.Status,
            });
        }

        // Retrieve workflow information from MongoDB
        [HttpGet("get-workflow/{workflowId}")]
        public async Task<IActionResult> GetWorkflowFromMongoDb(string workflowId)
        {
            var workflow = await _workflowCollection.Find(w => w.WorkflowId == workflowId).FirstOrDefaultAsync();

            if (workflow == null)
                return NotFound(new { Message = "Workflow not found in MongoDB." });

            return Ok(workflow);
        }
    }
}
