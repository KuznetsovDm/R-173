using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public ControllDescription Begin { get; set; }
        public ControllDescription End { get; set; }
    }

    public class ControllDescription
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string[] Buttons { get; set; }
    }

    public class Tasks
    {
        public ControllDescription Begin { get; set; }
        public ControllDescription PreparationToWork { get; set; }
        public ControllDescription HealthCheck { get; set; }
        public ControllDescription WorkingFrequencyPreparation { get; set; }
        public ControllDescription EndSuccesseful { get; set; }
        public ControllDescription EndFail { get; set; }
    }
}
