//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="WEIR">
//    © 2019 WEIR All Rights Reserved
// </copyright>
//-----------------------------------------------------------------------
namespace Msireceiver
{
    using System;
    using System.Diagnostics;   
    using System.IO;
    using Msireceiver.UDP;

    /// <summary>
    /// Program class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">array of arguments</param>
        public static void Main(string[] args)
        {
            // create a new server
            using (var server = new UdpListener())
            {
                // start listening for messages and copy the messages back to the client
                // execute only when message received 
                while (true)
                {
                    var received = server.Receive().Result;
                    Console.WriteLine("Sender:" + received.Sender + " " + "Message:" + received.Message);
                    string file = Path.Combine(Constants.VolumePath, received.Message);
                    if (IsExecuteCompleted(file))
                    {
                        server.Reply("Success", received.Sender);
                    }
                    else
                    {
                        server.Reply("Failure", received.Sender);
                    }
                    
                    Console.WriteLine("Application installed successfully!");
                }
            }
        }

        /// <summary>
        /// Execute *.msi file using "cmd.exe" in passive mode, which is received from docker
        /// </summary>
        /// <param name="file">path of the file</param>
        private static bool IsExecuteCompleted(string file)
        {
            bool status = false;
            Console.WriteLine("Starting to install application");
            using (var p = new Process())
            {
                p.StartInfo = new ProcessStartInfo(Constants.Executor, "/c" + file + " /passive")
                {
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    Verb = Constants.RunAs,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                p.Start();
                p.Exited += new EventHandler(ProcessStatus);
                p.EnableRaisingEvents = true;
                p.WaitForExit();

                status = p.ExitCode == 0 ? true: false;
            }
            return status;
        }

        /// <summary>
        /// Get the status of the process
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private static void ProcessStatus(object sender, EventArgs e)
        {
            Console.WriteLine("Process exited");
        }
    }
}
