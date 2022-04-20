using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGenerator_programowanie_IV.Modules
{
    class Question
    {
        public int QuistionNumber { get; set; }
        public string QuestionText { get; set; }
        public int Time { get; set; }
        public List<Answer> Answers { get; set; }

        public Question(int questionNumber, string questionText, int time, List<Answer> answers)
        {
            this.QuistionNumber = questionNumber;
            this.QuestionText = questionText;
            this.Time = time;
            this.Answers = answers;
        }
    }
}
