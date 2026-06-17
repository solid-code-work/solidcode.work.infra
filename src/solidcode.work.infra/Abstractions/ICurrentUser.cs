namespace Solidcode.Work.Infra.Abstractions;

public interface ICurrentUser
{
    string UserId { get; }
    string UserName { get; }
}