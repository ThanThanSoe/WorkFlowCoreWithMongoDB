using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkFlowCoreWithMongoDB.Models;
using WorkFlowCoreWithMongoDB.Workflows.Steps;

namespace WorkFlowCoreWithMongoDB.Workflows
{
    //public class DynamicWorkflow : IWorkflow<WorkflowRequest>
    //{
    //    public string Id => "DynamicWorkflow";
    //    public int Version => 1;

    //    public void Build(IWorkflowBuilder<WorkflowRequest> builder)
    //    {
    //        builder
    //            .StartWith(context => Console.WriteLine("Starting workflow..."))
    //            .If(data => data.ActionType == "A")
    //                .Do(then => then.StartWith<StepA>())
    //            .If(data => data.ActionType == "B")
    //                .Do(then => then.StartWith<StepB>())
    //            .Then(context => Console.WriteLine("Workflow complete."));
    //    }
    //}

    public class DynamicWorkflow : IWorkflow<WorkflowRequest>
    {
        public string Id => "DynamicWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<WorkflowRequest> builder)
        {
            builder
                .StartWith(context => Console.WriteLine("Starting workflow..."))
                .If(data => data.ActionType == "A")
                    .Do(then => then.StartWith<StepA>())
                .If(data => data.ActionType == "B")
                    .Do(then => then.StartWith<StepB>())
                .Then(context => Console.WriteLine("Workflow complete."));
        }
    }
}
