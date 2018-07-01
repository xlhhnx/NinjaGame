using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaGame.Common.Extensions
{
    public static class TaskExtensions
    {
        public static IEnumerable<Task<T>> InCompletionOrder<T>(this IEnumerable<Task<T>> source)
        {
            var inputs = source.ToList();
            var boxes = inputs.Select(i => new TaskCompletionSource<T>())
                              .ToList();

            var currentIndex = -1;
            foreach (var task in inputs)
            {
                task.ContinueWith(c =>
                    {
                        var nextBox = boxes[Interlocked.Increment(ref currentIndex)];
                        PropogateResult(c, nextBox);
                    }, 
                    TaskContinuationOptions.ExecuteSynchronously);
            }
            return boxes.Select(b => b.Task);
        }

        private static void PropogateResult<T>(Task<T> completedTask, TaskCompletionSource<T> completionSource)
        {
            switch (completedTask.Status)
            {
                case (TaskStatus.Canceled):
                    completionSource.TrySetCanceled();
                    break;
                case (TaskStatus.Faulted):
                    completionSource.TrySetException(completedTask.Exception.InnerExceptions);
                    break;
                case (TaskStatus.RanToCompletion):
                    completionSource.TrySetResult(completedTask.Result);
                    break;
            }
        }
    }
}
