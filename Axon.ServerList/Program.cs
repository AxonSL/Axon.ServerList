﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axon.ServerList;

public static class Program
{
    public static string ConfigPath { get; set; } = "";
    public static ServerList? ServerList { get; set; }

    public static void Main(string[] args)
    {
        var dir = Environment.GetEnvironmentVariable("CONFIGPATH") ?? Directory.GetCurrentDirectory();
        Console.WriteLine("Config Directory: " + dir);
        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        ConfigPath = Path.Combine(dir, "config.json");
        if (!File.Exists(ConfigPath))
            CreateConfig(ConfigPath);

        var content = File.ReadAllText(ConfigPath);
        var config = JsonConvert.DeserializeObject<ServerListConfiguration>(content);

        var url = Environment.GetEnvironmentVariable("URL");
        if (url != null)
        {
            config.Url = url;
        }
        ServerList = new ServerList(config, args);

        Task.Delay(-1).GetAwaiter().GetResult();
    }

    private static void CreateConfig(string path)
    {
        var config = new ServerListConfiguration
        {
            Url = "https://*",
            VerifiedServers = new VerifiedServers[0] { }
        };

        var json = JsonConvert.SerializeObject(config);
        File.Create(path).Close();
        File.WriteAllText(path, json);
    }
}
