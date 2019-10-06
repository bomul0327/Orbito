using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Command를 관리하고, 실행하는 클래스
/// 모든 Command들은 여기를 무조건 통하게 해야 한다.
/// </summary>
public static class CommandDispatcher
{
    /// <summary>
    /// 커맨드를 큐에 넣어서 순차적으로 실행
    /// </summary>
    private static Queue<ICommand> commandQueue = new Queue<ICommand>();

    /// <summary>
    /// 커맨드를 Dispatcher에 올려놓습니다.
    /// </summary>
    public static void Publish(ICommand command)
    {
        commandQueue.Enqueue(command);
    }

    /// <summary>
    /// 올려놓은 커맨드들을 모두 실행
    /// </summary>
    public static void Handle()
    {
        for(int i = 0; i < commandQueue.Count; ++i)
        {
            commandQueue.Dequeue().Execute();
        }
    }
}
