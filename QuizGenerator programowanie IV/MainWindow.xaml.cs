using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuizGenerator_programowanie_IV.Modules;

namespace QuizGenerator_programowanie_IV
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ZMIENNE
        FileHandling fileHandle = new FileHandling();
        List<Quiz> quizzes = new List<Quiz>();
        List<Answer> answers = new List<Answer>();
        List<Question> questions = new List<Question>();
        int questionIndex = 0;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            UpdateQuizListBox();
        }

        #region NAWIGACJA
        // 1. Menu View
        private void AddQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuView.Visibility = Visibility.Collapsed;
            AddView.Visibility = Visibility.Visible;
            ModifyView.Visibility = Visibility.Collapsed;
        }

        private void ModifyQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuView.Visibility = Visibility.Collapsed;
            AddView.Visibility = Visibility.Collapsed;
            ModifyView.Visibility = Visibility.Visible;
        }

        private void Exit_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // 2. Add View and Modify View
        private void GoToMenu_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuView.Visibility = Visibility.Visible;
            AddView.Visibility = Visibility.Collapsed;
            ModifyView.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region DODAWANIE I EDYCJA PYTAŃ

        private void NextQuestion_ButtonClick(object sender, RoutedEventArgs e)
        {
            // funckja zapisująca wprowadzone dane pytanie przed "zmieniemiem strony"

            SaveQuestion();

            // ustawienie pól na wartości bierzącego pytania lub domyślne

            questionIndex += 1;
            bool ifCurrentQuestionExists = questions.Any(q => q.QuestionNumber == questionIndex);

            QuestionText.Text = ifCurrentQuestionExists ? questions[questionIndex].QuestionText : "Treść pytania";
            AnswerA.Text = ifCurrentQuestionExists ? questions[questionIndex].Answers[0].AnswerText : "Odpowiedź A";
            AnswerB.Text = ifCurrentQuestionExists ? questions[questionIndex].Answers[1].AnswerText : "Odpowiedź B";
            AnswerC.Text = ifCurrentQuestionExists ? questions[questionIndex].Answers[2].AnswerText : "Odpowiedź C";
            AnswerD.Text = ifCurrentQuestionExists ? questions[questionIndex].Answers[3].AnswerText : "Odpowiedź D";
            IsRightA.IsChecked = ifCurrentQuestionExists ? questions[questionIndex].Answers[0].IsCorrect : false;
            IsRightB.IsChecked = ifCurrentQuestionExists ? questions[questionIndex].Answers[1].IsCorrect : false;
            IsRightC.IsChecked = ifCurrentQuestionExists ? questions[questionIndex].Answers[2].IsCorrect : false;
            IsRightD.IsChecked = ifCurrentQuestionExists ? questions[questionIndex].Answers[3].IsCorrect : false;
            QuestionTime.Text = ifCurrentQuestionExists ? questions[questionIndex].Time.ToString() : "10";
        }

        private void PreviousQuestion_ButtonClick(object sender, RoutedEventArgs e)
        {
            // funckja zapisująca wprowadzone dane pytanie przed "zmieniemiem strony"

            SaveQuestion();

            // przesuwanie indeksu pytania o jeden wcześniej jeśli nie jesteśmy na pierwszym pytaniu
            if (questionIndex == 0)
            {
                MessageBox.Show("Nie ma wcześniejszego pytania");
                return;
            }
            else
            {
                questionIndex -= 1;
            }

            // ustawianie pól na wartości zapisane w odpowiednim pytaniu

            QuestionText.Text = questions[questionIndex].QuestionText;
            AnswerA.Text = questions[questionIndex].Answers[0].AnswerText;
            AnswerB.Text = questions[questionIndex].Answers[1].AnswerText;
            AnswerC.Text = questions[questionIndex].Answers[2].AnswerText;
            AnswerD.Text = questions[questionIndex].Answers[3].AnswerText;
            IsRightA.IsChecked = questions[questionIndex].Answers[0].IsCorrect;
            IsRightB.IsChecked = questions[questionIndex].Answers[1].IsCorrect;
            IsRightC.IsChecked = questions[questionIndex].Answers[2].IsCorrect;
            IsRightD.IsChecked = questions[questionIndex].Answers[3].IsCorrect;
            QuestionTime.Text = questions[questionIndex].Time.ToString();
        }

        #endregion

        #region ZAPIS QUIZU DO PLIKU
        private void SaveQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            fileHandle.SaveToFile(new Quiz(QuizName.Text, questions));
            UpdateQuizListBox();
        }

        #endregion

        #region FUNCKJE DODATKOWE

        public void SaveQuestion()
        {
            // czyszczenie tablicy odpowiedzi i dodanie nowych

            answers.Clear();

            answers.Add(new Answer(AnswerA.Text, IsRightA.IsChecked));
            answers.Add(new Answer(AnswerB.Text, IsRightB.IsChecked));
            answers.Add(new Answer(AnswerC.Text, IsRightC.IsChecked));
            answers.Add(new Answer(AnswerD.Text, IsRightD.IsChecked));

            // sprawdzanie czy rekord o podanym indexie juz istnieje w liście pytań, jeśli tak -> aktualizuj, jeśli nie -> dodaj

            bool ifPreviousQuestionExists = questions.Any(q => q.QuestionNumber == questionIndex);

            if (ifPreviousQuestionExists)
            {
                questions[questionIndex] = new Question(questionIndex, QuestionText.Text, Convert.ToInt32(QuestionTime.Text), answers);
            }
            else
            {
                questions.Add(new Question(questionIndex, QuestionText.Text, Convert.ToInt32(QuestionTime.Text), answers));
            }
        }
        
        public void UpdateQuizListBox()
        {
            QuizListBox.Items.Clear();
            quizzes.Clear();
            quizzes = fileHandle.ReadFromFile();
            foreach (Quiz quiz in quizzes)
            {
                QuizListBox.Items.Add(quiz.QuizName);
            }
        }

        #endregion
    }
}
