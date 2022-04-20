using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace QuizGenerator_programowanie_IV.Modules
{
    class FileHandling
    {
        public void SaveToFile(Quiz quiz)
        {
            string json = JsonSerializer.Serialize(quiz);

            // tutaj będzie szyfrowanie

            // można tutaj dodać jak nam się będzie nudziło żeby program tworzył nowy folder w "Dokumentach" użytkownika i tam zapisywał quiz'y
            File.WriteAllText($"C:\\Users\\2000g\\OneDrive\\Pulpit\\{quiz.QuizName}.txt", json);
        }
    }
}
