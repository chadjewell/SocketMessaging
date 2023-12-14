using SocketMessaging;

// See https://aka.ms/new-console-template for more information

// Prompt for IP address
//Console.WriteLine("Enter IP Address:");
//string ipAddress = Console.ReadLine();
string ipAddress = "10.0.0.147";

// Prompt for Port
//Console.WriteLine("Enter Port #");
//int port = int.Parse(Console.ReadLine());
int port = 4545;

// Acknowledge Connect
Console.WriteLine("Press Enter to Connect");
Console.ReadKey();

SocketClient inSight = new SocketClient(ipAddress, port);
//Console.WriteLine(inSight.Connect().ToString());

List<string> response = inSight.Connect();
foreach (string line in response)
{
    Console.WriteLine(line);
}

// Prompt for Username
string msg = Console.ReadLine();
response = inSight.Send(msg);
foreach (string line in response)
{
    Console.WriteLine(line);
}

// Prompt for Password
msg = Console.ReadLine();
response = inSight.Send(msg);
foreach (string line in response)
{
    Console.WriteLine(line);
}

// Prompt for Command
msg = Console.ReadLine();
response = inSight.Send(msg);
foreach (string line in response)
{
    Console.WriteLine(line);
}

// Prompt for disconnect
Console.WriteLine("Press Enter to Disconnect");
Console.ReadKey();

inSight.Disconnect();
