using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkFlowCoreWithMongoDB.Models;

namespace WorkFlowCoreWithMongoDB.Workflows.Steps
{
    public class StepB : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // Logic for Step B
            Console.WriteLine("Executing Step B...");
            var data = context.Workflow.Data as WorkflowRequest;
            data.Status = "StepBCompleted"; // Update status after completion
            return ExecutionResult.Next();  // Proceed to the next step
        }
    }
}
