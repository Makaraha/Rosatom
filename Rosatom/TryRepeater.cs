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
                try
                {
                    var task = Task.Run(function);

                    if (task.Wait(TimeSpan.FromSeconds(Timeout)))
                        return await task;
                    else
                        throw new Exception("Time out");
                }
                catch (Exception)
                {
                    await Task.Delay(Delay);
                }
            }
        }

        internal async Task<TResult> RepeatForAsync<TResult>(Func<TResult> function, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                try
                {
                    var task = Task.Run(function);

                    if (task.Wait(TimeSpan.FromSeconds(Timeout)))
                        return await task;
                    else
                        throw new Exception("Time out");
                }
                catch (Exception)
                {
                    await Task.Delay(Delay);
                }
            }

            throw new Exception("Не удалось выполнить операцию");
        }

        internal async Task RepeatForAsync(Action action, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                try
                {
                    var task = Task.Run(action);

                    if (task.Wait(TimeSpan.FromSeconds(Timeout)))
                        return;
                    else
                        throw new Exception("Time out");
                }
                catch (Exception)
                {
                    await Task.Delay(Delay);
                }
            }

            throw new Exception("Не удалось выполнить операцию");
        }
    }
}
