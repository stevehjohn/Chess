using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Helpers
{
    public static class TaskHelpers
    {
        public static void WaitForAllToComplete(IEnumerable<Task> tasks)
        {
            var taskArray = tasks.ToArray();

            Task.WaitAll(taskArray.ToArray(), -1);

            var incomplete = taskArray.Where(t => t.Status != TaskStatus.RanToCompletion).ToList();

            while (incomplete.Any())
            {
                Task.WaitAll(incomplete.ToArray());

                incomplete = incomplete.Where(t => t.Status != TaskStatus.RanToCompletion).ToList();
            }
        }
    }
}