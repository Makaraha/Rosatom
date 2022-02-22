namespace Rosatom
{
    internal class TryRepeater
    {
        internal int Timeout { get; private set; } = 60;
        internal int Delay { get; private set; } = 2000;

        internal async Task<TResult> RepeatUntilSuccessAsync<TResult>(Func<TResult> function)
        {
            while (true)
            {
                var task = Task.Run(function);

                if (task.Wait(TimeSpan.FromSeconds(Timeout)))
                    return await task;
                else
                    Thread.Sleep(Delay);
            }
        }

        internal async Task<TResult> RepeatForAsync<TResult>(Func<TResult> function, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var task = Task.Run(function);

                if (task.Wait(TimeSpan.FromSeconds(Timeout)))
                    return await task;
                else
                    Thread.Sleep(Delay);
            }

            throw new Exception("Не удалось выполнить операцию");
        }
    }
}
