using WorkflowCore.Interface;
using WorkFlowCoreWithMongoDB.Models;
using WorkFlowCoreWithMongoDB.Workflows.Steps;

namespace WorkFlowCoreWithMongoDB.Workflows
{
    public class DynamicWorkflow : IWorkflow<WorkflowRequest>
    {
        public string Id => "DynamicWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<WorkflowRequest> builder)
        {
            builder
                .StartWith<StepA>()  // Use StepA, StepB, StepC, StepD
                .Then<StepB>()
                .Then<StepC>()
                .Then<StepD>()
                .Then(context => Console.WriteLine("Workflow complete."));
        }
    }
}
