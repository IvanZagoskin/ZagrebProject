using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class DataLoad
    {

        public static List<RootObject> data = null;
        const string FILENAME = "test.json";

    public static List<RootObject> LoadDataFromJson()
        {
            using (StreamReader r = new StreamReader(FILENAME))
            {
                string json = r.ReadToEnd();
     
                 data = JsonConvert.DeserializeObject<List<RootObject>>(json);

            }

        return data;
    }

    public static void SaveDataToJson(List<RootObject> updatedJson)
    {
        string json = JsonConvert.SerializeObject(updatedJson);
        File.WriteAllText(FILENAME, json);
    }

    public class Answer
    {
        public string Text { get; set; }
        public int isRight { get; set; }

        public Answer(string Text, int isRight)
        {
            this.Text = Text;
            this.isRight = isRight;
        }
    }

    public class RootObject
    {
        public string TestType { get; set; }
        public string Theme { get; set; }
        public int ID { get; set; }
        public string Question { get; set; }
        public string Complexity { get; set; }
        public List<Answer> Answers { get; set; }


        public RootObject(string TestType, string Question, string Theme, int ID, string Complexity, List<Answer> Answers)
        {
            this.TestType = TestType;
            this.Question = Question;
            this.Theme = Theme;
            this.ID = ID;
            this.Complexity = Complexity;
            this.Answers = Answers;
        }
    }


    }
    
