using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowCoreWithMongoDB.Workflows.Steps
{
    public class StepA : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Executing Step A...");
            return ExecutionResult.Next();
        }
    }
}
