using System.Collections.Generic;

public class CommandInvoker 
{
    private static Stack<ICommand> commandHistory = new Stack<ICommand>();

    public static void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }


}
