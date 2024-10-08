using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkFlowCoreWithMongoDB.Workflows.Steps
{
    public class StepB : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Executing Step B...");
            return ExecutionResult.Next();
        }
    }
}
