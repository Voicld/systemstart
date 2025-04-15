using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Control Panel ===");

        int interval = 5000;

        while (true)
        {
            Console.WriteLine($"\nUpdating process list every {interval / 1000} seconds...");
            ShowProcesses();

            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1 - Change the interval");
            Console.WriteLine("2 - Show the details of all processes");
            Console.WriteLine("3 - End a process");
            Console.WriteLine("4 - Run a program");
            Console.WriteLine("5 - Exit");

            Console.Write("Enter a number here: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter an interval in seconds: ");
                    if (int.TryParse(Console.ReadLine(), out int sec))
                        interval = sec * 1000;
                    break;

                case "2":
                    ShowProcessDetails();
                    break;

                case "3":
                    KillProcess();
                    break;

                case "4":
                    LaunchApplication();
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }

            Thread.Sleep(interval);
        }
    }

    static void ShowProcesses()
    {
        var processes = Process.GetProcesses().OrderBy(p => p.ProcessName).ToList();

        Console.WriteLine("\nActive processes:");
        for (int i = 0; i < processes.Count; i++)
        {
            try
            {
                Console.WriteLine($"{i}. {processes[i].ProcessName} (ID: {processes[i].Id})");
            }
            catch { }
        }
    }

    static void ShowProcessDetails()
    {
        Console.Write("Enter an ID of a process: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                var process = Process.GetProcessById(id);
                var count = Process.GetProcessesByName(process.ProcessName).Length;

                Console.WriteLine($"\nDetails of the process:");
                Console.WriteLine($"Name: {process.ProcessName}");
                Console.WriteLine($"ID: {process.Id}");
                Console.WriteLine($"Start time: {process.StartTime}");
                Console.WriteLine($"CPU Time: {process.TotalProcessorTime}");
                Console.WriteLine($"Amount of threads: {process.Threads.Count}");
                Console.WriteLine($"Copies of processess: {count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    static void KillProcess()
    {
        Console.Write("Enter an ID to kill a process: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                Process.GetProcessById(id).Kill();
                Console.WriteLine("The process has been killed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldnt end a process: " + ex.Message);
            }
        }
    }

    static void LaunchApplication()
    {
        Console.WriteLine("Choose an application to run:");
        Console.WriteLine("1 - Notepad");
        Console.WriteLine("2 - Calculator");
        Console.WriteLine("3 - Paint");
        Console.WriteLine("4 - Custom");

        Console.Write("Ваш вибір: ");
        string input = Console.ReadLine();

        string path = input switch
        {
            "1" => "notepad",
            "2" => "calc",
            "3" => "mspaint",
            "4" => PromptPath(),
            _ => null
        };

        if (path != null)
        {
            try
            {
                Process.Start(path);
                Console.WriteLine("The application is running.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldnt start an application: " + ex.Message);
            }
        }
    }

    static string PromptPath()
    {
        Console.Write("Enter a full path to an application: ");
        return Console.ReadLine();
    }
}
