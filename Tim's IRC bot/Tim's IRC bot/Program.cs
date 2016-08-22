using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ChatSharp;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using Microsoft.Win32;
using IWshRuntimeLibrary;





namespace Tim_s_IRC_bot
{
    class Program
    {

       

        static void Main(string[] args) 
        {


            {


                
                
                if (System.IO.File.Exists(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemDrive"), "/TimsBot/settings.ini")))
                {
                   

                }
                else
                {
                    Console.WriteLine("Welcome to Tims IRC bot, press enter to continue");
                    Console.ReadLine();
                    Console.WriteLine("Which server do you want the bot to join?");
                    string Server = Console.ReadLine();
                    Console.WriteLine("Which channel do you want the bot to join?");
                    string Channel = Console.ReadLine();
                    Console.WriteLine("Which nickname do you want to have?");

                    string Nickname = Console.ReadLine();
                    Console.WriteLine("Which username do you have?(NEEDED!)");
                    string username = Console.ReadLine();
                    
                    Console.WriteLine("Which password do you have for the account?(NEEDED!)");
                    string password = Console.ReadLine();
                    


                    System.IO.Directory.CreateDirectory("/TimsBot/");
                    System.IO.Directory.CreateDirectory("/TimsBot/data");
                    System.IO.Directory.CreateDirectory("/TimsBot/data/logs/");
                    System.IO.Directory.CreateDirectory("/TimsBot/data/systems/tokensystem/");
                    System.IO.Directory.CreateDirectory("/TimsBot/data/systems/warnsystem/warning/");
                    System.IO.Directory.CreateDirectory("/TimsBot/data/systems/warnsystem/offtopic/");
                    System.IO.Directory.CreateDirectory("/TimsBot/data/access/");
                    System.IO.Directory.CreateDirectory("/TimsBot/data/access/commands/");
                    System.IO.Directory.CreateDirectory("/TimsBot/data/access/commands/users/");
                    System.IO.File.WriteAllText(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemDrive"), "/TimsBot/settings.ini"),"Server="+ Server + Environment.NewLine + "Channel=" + Channel + Environment.NewLine + "Botname=" + Nickname + Environment.NewLine + "username=" + username + Environment.NewLine + "password=" + password);
                    System.IO.File.Copy(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, "/TimsBot/TimsBot.exe");
                    System.IO.File.Copy(System.IO.Path.Combine(Environment.CurrentDirectory , "ChatSharp.dll"), "/TimsBot/ChatSharp.dll");
                    System.IO.File.WriteAllText("/TimsBot/data/logs/log.txt", "This is the log File" + Environment.NewLine);
                    object shDesktop = (object)"Desktop";
                    WshShell shell = new WshShell();
                    string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + "/TimsBot.lnk";
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                    shortcut.Description = "Tim's IRC Bot";
                    
                    shortcut.TargetPath = System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemDrive"), "TimsBot/TimsBot.exe");
                    shortcut.Save();
                    Console.Clear();
                    Console.WriteLine("You can open the ControlPanel by right clicking on the new system tray icon, when you run the program.");
                    Console.ReadLine();
                    
                    Console.WriteLine("Install successfull, you can open the program by going to the desktop and click on the shortcut or navigate to /TimsBot/ and run TimsBot.exe.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                
                
              

                var handle = GetConsoleWindow();

                ShowWindow(handle, SW_HIDE);
               
                
                string readvalue = System.IO.File.ReadAllText("/TimsBot/settings.ini");
           
                string modify1 = readvalue.Replace("Server=","").Replace("Channel=", "").Replace("Botname=", "").Replace("username=", "").Replace("password=", "");
                
                string[] splitvalue = modify1.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
              
                
            

               
                var client = new IrcClient(splitvalue[0], new IrcUser(splitvalue[2], splitvalue[3], splitvalue[4]));
                client.ConnectionComplete += (s, e) => client.JoinChannel(splitvalue[1]);
                Console.Clear();


                
                NotifyIcon trayIcon = new NotifyIcon();
                trayIcon.Text = "TimsIRCBot";
                trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

                ContextMenu trayMenu = new ContextMenu();
                trayMenu.MenuItems.Add("Control Panel", new EventHandler(item2_Click));
                trayMenu.MenuItems.Add("exit", new EventHandler(item1_Click));



                trayIcon.ContextMenu = trayMenu;
                trayIcon.Visible = true;
              




                client.ChannelMessageRecieved += (s, e) =>
                {

                    var channel = client.Channels[e.PrivateMessage.Source];

                    if (e.PrivateMessage.Message == "!list")
                        channel.SendMessage(string.Join(", ", channel.Users.Select(u => u.Nick)));
                    else if (e.PrivateMessage.Message.StartsWith("!ban "))

                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            var target = e.PrivateMessage.Message.Substring(5);
                            client.WhoIs(target, whois => channel.ChangeMode("+b *!*@" + whois.User.Hostname));
                            channel.Kick(e.PrivateMessage.Message.Substring(5));
                            Console.WriteLine(e.PrivateMessage.Message.Substring(5) + " " + "is banned");
                            client.SendMessage("You got banned from " + splitvalue[1], e.PrivateMessage.Message.Substring(5));
                        }



                    }
                    else if (e.PrivateMessage.Message.StartsWith("!unban "))
                    {

                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {

                            var target = e.PrivateMessage.Message.Substring(7);
                            client.WhoIs(target, whois => channel.ChangeMode("-b *!*@" + whois.User.Hostname));
                            Console.WriteLine(e.PrivateMessage.Message.Substring(7) + " " + "is unbanned");
                            client.SendMessage("You got unbanned from " + splitvalue[1], e.PrivateMessage.Message.Substring(7));

                        }

                    }
                    else if (e.PrivateMessage.Message == "!leave")
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            channel.Part();
                            Console.WriteLine("Bot left channel.");
                            Environment.Exit(0);
                        }
                    }
                    else if (e.PrivateMessage.Message == "!cycle")
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            channel.Part();
                            client.JoinChannel(splitvalue[1]);
                            Console.WriteLine("Bot cycled channel.");

                        }
                    }
                    else if (e.PrivateMessage.Message.StartsWith("!kick "))
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {


                            channel.Kick(e.PrivateMessage.Message.Substring(6));
                            Console.WriteLine(e.PrivateMessage.Message.Substring(6) + " " + "is kicked");
                            client.SendMessage("You got kicked from " + channel, e.PrivateMessage.Message.Substring(6));
                        }


                    }
                    else if (e.PrivateMessage.Message == "!commands")
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            channel.SendMessage("Commands: !ban, !unban, !kick, !join, !tokens, !give, !tokens, !warn, !warn_offtopic, !op, !deop");
                        }
                        else
                        {
                            channel.SendMessage("Commands: !tokens, !give.");
                        }

                        Console.WriteLine("!commands requested");
                    }
                    else if (e.PrivateMessage.Message.StartsWith("!say "))
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            channel.SendMessage(e.PrivateMessage.Message.Substring(5));
                        }


                    }

                    else if (e.PrivateMessage.Message.StartsWith("!join "))
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            client.JoinChannel(e.PrivateMessage.Message.Substring(5));
                        }


                    }
                    else if (e.PrivateMessage.Message.StartsWith("!warn_offtopic "))
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            if (System.IO.Directory.Exists("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15)))
                            {
                                if (System.IO.File.Exists("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15) + "/" + "4"))
                                {
                                    var target = e.PrivateMessage.Message.Substring(15);
                                    client.WhoIs(target, whois => channel.ChangeMode("+b *!*@" + whois.User.Hostname));

                                    Console.WriteLine(e.PrivateMessage.Message.Substring(15) + " " + "is banned");
                                    client.SendMessage("You got banned from " + splitvalue[1] + " because you keep going off topic", e.PrivateMessage.Message.Substring(15));
                                    channel.Kick(e.PrivateMessage.Message.Substring(15));
                                }
                                else
                                {
                                    if (System.IO.File.Exists("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15) + "/" + "3"))
                                    {
                                        System.IO.File.WriteAllText("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15) + "/" + "4", "Warning number 4");
                                        channel.Kick(e.PrivateMessage.Message.Substring(15));
                                        Console.WriteLine(e.PrivateMessage.Message.Substring(15) + " " + "is kicked");
                                        client.SendMessage("You got kicked from " + splitvalue[1] + " because you keep going off topic", e.PrivateMessage.Message.Substring(6));

                                    }
                                    else
                                    {
                                        if (System.IO.File.Exists("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15) + "/" + "2"))
                                        {
                                            System.IO.File.WriteAllText("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15) + "/" + "3", "Warning number 3");
                                            client.SendMessage("This is your last warning, please stop being off topic, you will be kicked if you go on in " + splitvalue[1], e.PrivateMessage.Message.Substring(15));
                                        }
                                        else
                                        {
                                            System.IO.File.WriteAllText("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15) + "/" + "2", "Warning number 2");
                                            client.SendMessage("Please remember, this is a chat for chat with related topics in " + splitvalue[1], e.PrivateMessage.Message.Substring(15));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                System.IO.Directory.CreateDirectory("/TimsBot/data/systems/warnsystem/offtopic/" + e.PrivateMessage.Message.Substring(15));
                                client.SendMessage("Please try and stay on topic in " + splitvalue[1], e.PrivateMessage.Message.Substring(15));
                            }

                        }

                        else

                        {

                        }



                    }
                    else if (e.PrivateMessage.Message.StartsWith("!warn "))
                    {
                        if (System.IO.File.Exists("/TimsBot/data/access/commands/users/" + e.PrivateMessage.User.Nick))
                        {
                            if (System.IO.Directory.Exists("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6)))
                            {
                                if (System.IO.File.Exists("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6) + "/" + "4"))
                                {
                                    var target = e.PrivateMessage.Message.Substring(6);
                                    client.WhoIs(target, whois => channel.ChangeMode("+b *!*@" + whois.User.Hostname));

                                    Console.WriteLine(e.PrivateMessage.Message.Substring(6) + " " + "is banned");
                                    client.SendMessage("You got banned from " + splitvalue[1] + " because you have 5 warnings", e.PrivateMessage.Message.Substring(6));
                                    channel.Kick(e.PrivateMessage.Message.Substring(6));
                                }
                                else
                                {
                                    if (System.IO.File.Exists("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6) + "/" + "3"))
                                    {
                                        System.IO.File.WriteAllText("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6) + "/" + "4", "Warning number 4");
                                        channel.Kick(e.PrivateMessage.Message.Substring(6));
                                        Console.WriteLine(e.PrivateMessage.Message.Substring(6) + " " + "is kicked");
                                        client.SendMessage("You got kicked from " + splitvalue[1] + " because you have 4 warnings, if you get one more warning you will get banned.", e.PrivateMessage.Message.Substring(6));

                                    }
                                    else
                                    {
                                        if (System.IO.File.Exists("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6) + "/" + "2"))
                                        {
                                            System.IO.File.WriteAllText("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6) + "/" + "3", "Warning number 3");
                                            client.SendMessage("Please remember, this is your 3th warning in in " + splitvalue[1], e.PrivateMessage.Message.Substring(6));
                                        }
                                        else
                                        {
                                            System.IO.File.WriteAllText("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6) + "/" + "2", "Warning number 2");
                                            client.SendMessage("Please remember, this is your second warning in " + splitvalue[1], e.PrivateMessage.Message.Substring(6));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                System.IO.Directory.CreateDirectory("/TimsBot/data/systems/warnsystem/warning/" + e.PrivateMessage.Message.Substring(6));
                                client.SendMessage("Please remember, this is your first warning in " + splitvalue[1], e.PrivateMessage.Message.Substring(6));
                            }

                        }
                    }

                    else if (e.PrivateMessage.Message.StartsWith("!give "))
                    {
                        Again2:
                        if (System.IO.File.Exists("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick))
                        {
                            goto Next;

                        }
                        else
                        {
                            System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick, "10000");
                            goto Next;
                        }


                        Next:

                        string value9 = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick);
                        string input8 = e.PrivateMessage.Message;
                        string[] words2 = input8.Split(' ');
                        if (System.IO.File.Exists("/TimsBot/data/systems/tokensystem/" + words2[1]))

                        {


                            string value = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick);
                            string input2 = e.PrivateMessage.Message;
                            string[] words = input2.Split(' ');
                            string value2 = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + words[1]);
                            string value3 = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick);
                            int n = Int32.Parse(value3);
                            if (n == 0)
                            {
                                channel.SendMessage(e.PrivateMessage.User.Nick + " You have 0 Tokens!");

                            }
                            else
                            {

                                



                                int x = Int32.Parse(value);
                                int y = Int32.Parse(words[2]);
                                int u = x - y;
                                int a = Int32.Parse(value2);
                                int b = Int32.Parse(words[2]);
                                int i = a + b;
                                if (words[1] == e.PrivateMessage.User.Nick)

                                {
                                    channel.SendMessage(e.PrivateMessage.User.Nick + ", you can't give tokens to yourself!");
                                }


                                else

                                {


                                    System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick, "" + u);
                                    System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + words[1], "" + i);
                                    string value5 = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick);
                                    if (value5.StartsWith("-"))

                                    {

                                        channel.SendMessage(e.PrivateMessage.User.Nick + " You don't have enough tokens!");
                                        int h = x + y;
                                        int p = a - b;
                                        System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick, "" + h);
                                        System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + words[1], "" + p);
                                    }
                                    else
                                    {

                                        channel.SendMessage("Transferred " + words[2] + " tokens from " + e.PrivateMessage.User.Nick + " to " + words[1] + "!");
                                        string value4 = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick);
                                        channel.SendMessage(e.PrivateMessage.User.Nick + " You have " + value4 + " Tokens left!");
                                    }





                                }
                            }
                            /// else
                            {
                                //channel.SendMessage(e.PrivateMessage.User.Nick + " , NickName is invalid.");
                                //}
                            }
                        }
                        else
                        {
                            string value = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick);
                            string input2 = e.PrivateMessage.Message;
                            string[] words = input2.Split(' ');

                            System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + words[1], "10000");
                            goto Again2;
                        }


                    }
                    else if (e.PrivateMessage.Message.StartsWith("!tokens"))
                    {

                        Again:
                        if (System.IO.File.Exists("/TimsBot/data/systems/tokensystem/" + (e.PrivateMessage.User.Nick)))
                        {
                            string tokenvalue = System.IO.File.ReadAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick);
                            channel.SendMessage(e.PrivateMessage.User.Nick + "," + " You have " + tokenvalue + " Tokens!");
                        }
                        else
                        {
                            System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + e.PrivateMessage.User.Nick, "10000");
                            goto Again;
                        }



                    }
                    else if (e.PrivateMessage.Message == "!op")
                    {
                        if (System.IO.File.Exists("/TimsBot/data/systems/tokensystem//" + e.PrivateMessage.User.Nick))
                        {
                            var target = e.PrivateMessage.User.Nick;
                            client.WhoIs(target, whois => channel.ChangeMode("+o  "+ e.PrivateMessage.User.Nick));
                        }
                    }
                    else if (e.PrivateMessage.Message == "!deop")
                    {
                        if (System.IO.File.Exists("/TimsBot/data/systems/tokensystem//" + e.PrivateMessage.User.Nick))
                        {
                            var target = e.PrivateMessage.User.Nick;
                            client.WhoIs(target, whois => channel.ChangeMode("-o " + e.PrivateMessage.User.Nick));
                        }
                    }

                    client.UserJoinedChannel += (f, g) =>
                    {
                        if (System.IO.File.Exists("/TimsBot/data/systems/tokensystem//" + g.User.Nick))
                        {
                           
                        }
                        else {
                            System.IO.File.WriteAllText("/TimsBot/data/systems/tokensystem/" + g.User.Nick , "10000");

                        }

                    };


                };

                client.ConnectAsync();

                Application.Run();
                while (true) ;

            }

        }


        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void item1_Click(object sender, EventArgs e )
        {
            NotifyIcon trayIcon = new NotifyIcon();
            trayIcon.Icon = null;
            Environment.Exit(0);

        }
        public static void item2_Click(object sender, EventArgs e)
        {
            BotControlPanel frm = new BotControlPanel();
            frm.Show();

        }
    }
      

    }







