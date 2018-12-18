using Newtonsoft.Json;
using System;
using System.IO;

namespace R_173.BE
{
    public class JsonBinder<T>
    {
        public T BindFromFile(string path)
        {
            try
            {
                var text = File.ReadAllText(path);
                return Bind(text);
            }
            catch (Exception ex)
            {
                SimpleLogger.Log(ex);
            }
            return default(T);
        }

        public T Bind(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

    }

    public class ActionDescriptionOption
    {
        public ActionDescriptions PreparationToWork { get; set; }
        public ActionDescriptions HealthCheck { get; set; }
        public ActionDescriptions WorkingFrequencyPreparation { get; set; }
        public ActionDescriptions RadioCommunicationManagment { get; set; }
        public Tasks Tasks { get; set; }
    }

    public class ActionDescriptions
    {
        public ControlDescription Begin { get; set; }
        public ControlDescription End { get; set; }
    }

    public class ControlDescription
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string[] Buttons { get; set; }
    }

    public class Tasks
    {
        public ControlDescription Begin { get; set; }
        public ControlDescription PreparationToWork { get; set; }
        public ControlDescription HealthCheck { get; set; }
        public ControlDescription WorkingFrequencyPreparation { get; set; }
        public ControlDescription EndSuccesseful { get; set; }
        public ControlDescription EndFail { get; set; }
    }
}
