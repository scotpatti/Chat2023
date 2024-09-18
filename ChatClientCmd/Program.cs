using System.ComponentModel;
using ChatLib;

namespace ChatClientCmd;

class Program
{
    private static string? Name { get; set; } = string.Empty;
    private static MainModel? Model { get; set; }

    public static void Main(string[] args)
    {
        var prog = new Program();
        prog.MainMethod(args);
    }

    public async void MainMethod(string[] args)
    {
        //Part 1: Get the user's name
        Name = GetInput("Enter your name: ");

        //Part 2: Connect to the server
        Model = new MainModel();
        //Part 3: Listen for messages
        //Be aware that the model implements INotifyPropertyChanged which allows
        //us to just listen for update to the MessageBoard property and print it. 
        Model.PropertyChanged += PropertyChangedListener;
        Model.Username = Name;
        Model.Connect();
        Console.WriteLine("Connected to the server!");
        Console.Write("Enter your message: ");


        //Part 1.1 Get Non-blocking input
        await Task.Run(() => GetInputNonBlocking());

        //Part 4: Indefinite chat
        while (true)
        {
            //Model.CurrentMessage = GetInput("Enter your message: ");
            //Model.Send();
            await Task.Delay(100);
        }
    }


    /// <summary>
    /// We call this twice, that enough to justify a method
    /// </summary>
    /// <param name="prompt">The prompt to show a user</param>
    /// <returns></returns>
    private static string GetInput(string prompt)
    {
        string? input = string.Empty;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(input));
        return input;
    }

    private static void GetInputNonBlocking()
    {
        string tempMessage = string.Empty;
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Model!.CurrentMessage = tempMessage;
                    Model!.Send();
                    tempMessage = string.Empty;
                    Console.Write("\r\nEnter your message: ");
                }
                else
                {
                    tempMessage += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Task.Delay(100).Wait();
        }
    }

    /// <summary>
    /// This method listens for changes to the model and prints the message board if the MessageBoard property changes
    /// </summary>
    /// <param name="sender">This is the model object that we are using</param>
    /// <param name="e">Information about the event</param>
    private static void PropertyChangedListener(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is MainModel model)
        {
            if (e.PropertyName == "MessageBoard" && !string.IsNullOrEmpty(model.MessageBoard))
            {
                Console.Clear();
                Console.WriteLine($"\r\n{model.MessageBoard}");
                Console.Write("\r\nEnter your message: ");
            }
        }
    }
}
