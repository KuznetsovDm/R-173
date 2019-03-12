using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace R_173.BE
{
    public class JsonBinder<T>
    {
        public T BindFromAssemblyResources(string path)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(path))
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    var obj = reader.ReadToEnd();
                    return Bind(obj);
                }
            }
            catch (Exception)
            {
                return default(T);
            }
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
        public ControlDescription ConnectionEasy { get; set; }
        public ControlDescription ConnectionHard { get; set; }
		public ControlDescription EndSuccesseful { get; set; }
        public ControlDescription EndFail { get; set; }
    }
}