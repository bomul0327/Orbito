using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandDispatcher
{
    /// <summary>
    /// 커맨드를 큐에 넣어서 순차적으로 실행
    /// </summary>
    private static Queue<ICommand> commandQueue = new Queue<ICommand>();

    public static void PushCommand(ICommand command)
    {
        commandQueue.Enqueue(command);
    }

    public static void PopCommand()
    {
        if (commandQueue.Count != 0)
            commandQueue.Dequeue().Execute();
    }
}
