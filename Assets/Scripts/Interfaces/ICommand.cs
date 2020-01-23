using System;

public interface ICommand : IDisposable
{
    /// <summary>
    /// 커맨드 실행
    /// </summary>
    void Execute ();

    /// <summary>
    /// 커맨드의 속성들을 초기화
    /// </summary>
    /// <param name="values">초기화시킬 때 필요한 파라미터들</param>
    void SetData(params object[] values);
}
