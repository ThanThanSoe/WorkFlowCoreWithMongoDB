﻿using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkFlowCoreWithMongoDB.Models;

namespace WorkFlowCoreWithMongoDB.Workflows.Steps
{
    public class StepA : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // Logic for Step A
            Console.WriteLine("Executing Step A...");
            var data = context.Workflow.Data as WorkflowRequest;
            data.Status = "StepACompleted"; // Update status after completion
            return ExecutionResult.Next();  // Proceed to the next step
        }
    }
}
