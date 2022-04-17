using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using StaplePuck.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace StaplePuck.Hockey.NHLStatService
{
    public class LambdaEntryPoint
    {
        public LambdaEntryPoint()
        {
        }

        public async Task ProcessMessage(DateRequest request, ILambdaContext context)
        {
            if (string.IsNullOrEmpty(request.GameDateId))
            {
                request.GameDateId = StaplePuck.Core.DateExtensions.TodaysDateId();
            }
            var updater = Updater.Init();
            await updater.UpdateRequest(request);
        }

        //public async Task HandleSQSEvent(SQSEvent evnt, ILambdaContext context)
        //{
        //    foreach (var message in evnt.Records)
        //    {
        //        await ProcessMessageAsync(message, context);
        //    }
        //}

        //private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        //{
        //    context.Logger.LogLine($"Processed message {message.Body}");

        //    // TODO: Do interesting work based on the new message
        //    await Task.CompletedTask;
        //}
    }
}
