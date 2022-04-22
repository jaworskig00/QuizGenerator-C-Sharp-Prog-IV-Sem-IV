using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;

namespace QuizGenerator_programowanie_IV.Modules
{
    class FileHandling
    {
        public List<Quiz> quizzes = new List<Quiz>();
        public void SaveToFile(Quiz quiz)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(quiz);

            // tutaj będzie szyfrowanie

            // można tutaj dodać jak nam się będzie nudziło żeby program tworzył nowy folder w "Dokumentach" użytkownika i tam zapisywał quiz'y
            File.WriteAllText($"C:\\Users\\Kiepson\\Desktop\\QUIZY\\{quiz.QuizName}.txt", json);
        }
       
        public List<Quiz> ReadFromFile()
        {
            
            quizzes.Clear();
            string[] files = Directory.GetFiles("C:\\Users\\Kiepson\\Desktop\\QUIZY\\");
            
            foreach (string file in files)
            {
                string text = File.ReadAllText(file);
                Quiz stuff = JsonConvert.DeserializeObject<Quiz>(text);

                quizzes.Add(stuff);
            }

            return quizzes;
        }
    }
}
