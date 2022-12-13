using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using schoolsite.Models;
using System.Text;
using System.Collections.Generic;
using System.IO.Compression;

namespace schoolsite.Controllers;

public class TaskController : Controller
{
    private readonly ILogger<TaskController> _logger;

    public TaskController(ILogger<TaskController> logger)
    {
        _logger = logger;
    }

    public IActionResult Result(string surname)
    {
        ViewBag.Surname = surname;
        return View();
    }

    public FileResult Download(string surname)
    {
        string[] files = {"anketa", "glossary", "index", "software"};
        foreach (string f in files)
        {   
            string path = $"./wwwroot/task/site/{f}.html";
            string data = System.IO.File.ReadAllText($"./wwwroot/assets/task1/{f}.html").Replace("%name%", surname);

            using (FileStream fs = System.IO.File.Create(path)) {
                fs.Write(Encoding.UTF8.GetBytes(data));
            }
        }

        using (FileStream fs = System.IO.File.Create("./wwwroot/task/site/computer.png")) {
            fs.Write(System.IO.File.ReadAllBytes("./wwwroot/assets/task1/computer.png"));
        }

        bool exists = System.IO.File.Exists($"./wwwroot/task/{surname}.zip");
        if(exists)
            System.IO.File.Delete($"./wwwroot/task/{surname}.zip");

        ZipFile.CreateFromDirectory("./wwwroot/task/site", $"./wwwroot/task/{surname}.zip");

        return File(System.IO.File.ReadAllBytes($"./wwwroot/task/{surname}.zip"), System.Net.Mime.MediaTypeNames.Application.Octet, $"{surname}.zip");
    }
}
