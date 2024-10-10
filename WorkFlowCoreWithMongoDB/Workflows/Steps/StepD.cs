using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkFlowCoreWithMongoDB.Models;

namespace WorkFlowCoreWithMongoDB.Workflows.Steps
{
    public class StepD : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // Logic for Step D
            Console.WriteLine("Executing Step D...");
            var data = context.Workflow.Data as WorkflowRequest;
            data.Status = "StepDCompleted"; // Update status after completion
            return ExecutionResult.Next();  // Proceed to the next step
        }
    }
}
