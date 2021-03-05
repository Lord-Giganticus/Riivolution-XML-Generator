using System;

namespace Riivolution_XML_Generator_Command_Line
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string test = args[5];
            } catch (Exception err)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(err);
                return;
            }
            Riivolution_XML_Generator.Classes.XML_Generator.Generate(args[0],args[1],args[2],args[3],args[4],args[5]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Complete. Press enter to exit.");
            Console.ReadKey();
        }
    }
}
