using System;

public interface ICommand : IDisposable
{
    /// <summary>
    /// 커맨드 실행
    /// </summary>
    void Execute ();
}
