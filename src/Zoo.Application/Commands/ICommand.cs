namespace Zoo.Application.Commands;

public interface ICommand
{
}

public interface ICommand<TResult> : ICommand
{
}
