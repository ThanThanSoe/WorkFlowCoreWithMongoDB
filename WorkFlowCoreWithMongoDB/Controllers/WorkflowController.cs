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
        private readonly ILogger<WorkflowController> _logger;

        public WorkflowController(ILogger<WorkflowController> logger, IWorkflowHost workflowHost, IMongoClient mongoClient)
        {
            _logger = logger;
            _workflowHost = workflowHost;
            var database = mongoClient.GetDatabase("YourDatabaseName"); // Make sure to replace with the actual database name
            _workflowCollection = database.GetCollection<WorkflowInstanceModel>("workflows");
        }

        // Endpoint to start a workflow
        [HttpPost("start-workflow")]
        public async Task<IActionResult> StartWorkflow([FromBody] WorkflowRequest request)
        {
            // Check if the workflow instance already exists
            var workflowInstance = await _workflowCollection.Find(w => w.WorkflowId == request.WorkflowId).FirstOrDefaultAsync();
           
            var actionInstance = await _workflowCollection.Find(w => w.ActionId == request.ActionId).SortByDescending(w => w.CreatedAt).FirstOrDefaultAsync();

           // var actionType = actionInstance.ActionType;
            if(actionInstance != null && workflowInstance == null)
            {
                if(actionInstance.ActionId == request.ActionId && actionInstance.ActionType == request.ActionType)
                {
                    return BadRequest(new { Message = $"Cannot start {request.ActionType}. Request steps must be existed." });
                }
            }

            if (workflowInstance != null && actionInstance != null)
            {
                //if(workflowInstance.ActionType == request.ActionType)
                if((actionInstance.ActionType == request.ActionType) && (workflowInstance.ActionId == request.ActionId))
                {
                    return BadRequest(new { Message = $"Cannot start {request.ActionType}. Request steps must be existed." });
                }
                // Check if the requested step can be executed
                if (!await IsStepValid(workflowInstance.WorkflowId, request.ActionType))
                {
                    return BadRequest(new { Message = $"Cannot start {request.ActionType}. Previous steps must be completed." });
                }
            }
            else
            {
                // If no workflow instance exists, generate a new one
                request.WorkflowId = Guid.NewGuid().ToString();
            }

            // Start the workflow
            var workflowId = await _workflowHost.StartWorkflow("DynamicWorkflow", request);
            var newWorkflowInstance = new WorkflowInstanceModel();
            if (request.ActionType == "StepA")
            {
                newWorkflowInstance.WorkflowId= workflowId;
                newWorkflowInstance.ActionType = request.ActionType;
                newWorkflowInstance.CreatedAt = DateTime.UtcNow;
                newWorkflowInstance.Status = "StepACompleted";
                newWorkflowInstance.ActionId = request.ActionId;
                await _workflowCollection.InsertOneAsync(newWorkflowInstance); // Insert a new workflow instance in MongoDB
            }
            else if(request.ActionType == "StepB")
            {
                newWorkflowInstance.WorkflowId = workflowId;
                newWorkflowInstance.ActionType = request.ActionType;
                newWorkflowInstance.CreatedAt = DateTime.UtcNow;
                newWorkflowInstance.Status = "StepBCompleted"; // Initial status when workflow starts
                newWorkflowInstance.ActionId = request.ActionId;
                await _workflowCollection.InsertOneAsync(newWorkflowInstance); // Insert a new workflow instance in MongoDB
            }
            else if(request.ActionType == "StepC")
            {

                newWorkflowInstance.WorkflowId = workflowId;
                newWorkflowInstance.ActionType = request.ActionType;
                newWorkflowInstance.CreatedAt = DateTime.UtcNow;
                newWorkflowInstance.Status = "StepCCompleted"; // Initial status when workflow starts
                newWorkflowInstance.ActionId = request.ActionId;
                await _workflowCollection.InsertOneAsync(newWorkflowInstance); // Insert a new workflow instance in MongoDB
            }
            else if(request.ActionType == "StepD")
            {
                newWorkflowInstance.WorkflowId = workflowId;
                newWorkflowInstance.ActionType = request.ActionType;
                newWorkflowInstance.CreatedAt = DateTime.UtcNow;
                newWorkflowInstance.Status = "Complete"; // Initial status when workflow starts
                newWorkflowInstance.ActionId = request.ActionId;
                await _workflowCollection.InsertOneAsync(newWorkflowInstance); // Insert a new workflow instance in MongoDB
            }
            else
            {
                return BadRequest(new { Message = $"Cannot start {request.ActionType}. ActionType is wrong." });
            }
           
            return Ok(new { WorkflowId = workflowId });
        }

        // Endpoint to check the current status of a workflow
        [HttpGet("check-status/{workflowId}")]
        public async Task<IActionResult> CheckWorkflowStatus(string workflowId)
        {
            var workflowInstance = await _workflowCollection.Find(w => w.WorkflowId == workflowId).FirstOrDefaultAsync();

            if (workflowInstance == null)
                return NotFound(new { Message = "Workflow not found." });

            return Ok(new
            {
                WorkflowId = workflowId,
                CurrentStepId = workflowInstance.CurrentStepId,
                CurrentStepStatus = workflowInstance.Status,
            });
        }

        // Endpoint to retrieve the workflow from MongoDB
        [HttpGet("get-workflow/{workflowId}")]
        public async Task<IActionResult> GetWorkflowFromMongoDb(string workflowId)
        {
            var workflow = await _workflowCollection.Find(w => w.WorkflowId == workflowId).FirstOrDefaultAsync();

            if (workflow == null)
                return NotFound(new { Message = "Workflow not found in MongoDB." });

            return Ok(workflow);
        }

        // Helper function to validate step execution based on the current workflow status
        private async Task<bool> IsStepValid(string workflowId, string actionType)
        {
            var workflowInstance = await _workflowCollection.Find(w => w.WorkflowId == workflowId).FirstOrDefaultAsync();

            if (workflowInstance == null)
            {
                // Workflow instance not found
                return false;
            }
            
            // Validate steps based on workflow state
            switch (actionType)
            {
                case "StepA":
                    return workflowInstance.Status == "Pending" || workflowInstance.Status == "Completed";

                case "StepB":
                    return workflowInstance.Status == "StepACompleted";

                case "StepC":
                    return workflowInstance.Status == "StepBCompleted";

                case "StepD":
                    return workflowInstance.Status == "StepCCompleted";

                default:
                    return false;
            }
        }
    }
}
