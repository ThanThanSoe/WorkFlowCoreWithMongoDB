using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkFlowCoreWithMongoDB.Models;

namespace WorkFlowCoreWithMongoDB.Workflows.Steps
{
    public class StepC : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // Logic for Step C
            Console.WriteLine("Executing Step C...");
            var data = context.Workflow.Data as WorkflowRequest;
            data.Status = "StepCCompleted"; // Update status after completion
            return ExecutionResult.Next();  // Proceed to the next step
        }
    }
}
