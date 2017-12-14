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
    const string FILENAME = "database.json";

    public static List<RootObject> LoadDataFromJson()
    {
        using (StreamReader r = new StreamReader(FILENAME))
        {
            string json = r.ReadToEnd();

            data = JsonConvert.DeserializeObject<List<RootObject>>(json);

        }

        return data;
    }


    public static List<string> GetQuestionTypes()
    {
        return data.Select(question => question.TestType).Distinct().ToList();

    }

    public static List<string> GetQuestionThemes()
    {
        return data.Select(question => question.Theme).Distinct().ToList();
    }

    public static int GetMaxQuestionId()
    {
        return data.Select(question => question.ID).Max();
    }

    public static List<string> GetThemesInType(string selectedType)
    {
        return data.Where(question => question.TestType == selectedType).Select(question => question.Theme).Distinct().ToList();
    }

    public static List<string> GetQuestionsInTheme(string selectedTheme, string selectedType)
    {
        return data.Where(question => (question.Theme == selectedTheme) && (question.TestType == selectedType)).Select(question => question.Question).Distinct().ToList();

    }

    public static int GetQuestionId(string selectedQuestion)
    {
        return data.Where(question => question.Question == selectedQuestion).Select(question => question.ID).First();
    }

    public static RootObject GetQuestionByText(string questionText)
    {
        return data.Where(question => question.Question == questionText).First();
    }

    public static RootObject GetQuestionById(int id)
    {
        return data.Where(question => question.ID == id).First();
    }

    public static bool CheckUniqueQuestion(string questionText, int id)
    {

        if (data.Where(q => q.Question == questionText).Where(q => q.ID != id).Any())
        {
            return false;
        }

        return true;
    }

    public static bool CheckUniqueQuestion(string questionText)
    {
        if (data.Where(question => question.Question == questionText).Any())
        {
            return false;
        }

        return true;
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

