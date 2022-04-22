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
using System.IO;
using QuizGenerator_programowanie_IV.Modules;

namespace QuizGenerator_programowanie_IV
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ZMIENNE

        FileHandling fileHandle;
        CezarEncryption encryptionHanlde;

        string previousLocation;
        string quizOldName;

        List<Quiz> quizzes;
        List<Answer> answers;
        List<Question> questions;
        int questionIndex;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            fileHandle = new FileHandling();
            encryptionHanlde = new CezarEncryption();

            previousLocation = "";
            quizOldName = "";

            quizzes = new List<Quiz>();
            answers = new List<Answer>();
            questions = new List<Question>();
            questionIndex = 0;

            UpdateQuizListBox();
        }

        #region NAWIGACJA
        // 1. Menu View
        private void AddQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuView.Visibility = Visibility.Collapsed;
            AddView.Visibility = Visibility.Visible;
            ModifyView.Visibility = Visibility.Collapsed;

            ClearView();

            questions.Clear();
            questionIndex = 0;

            previousLocation = ((Button)sender).CommandParameter?.ToString();

            if (previousLocation == "Modify")
            {
                quizOldName = quizzes[QuizListBox.SelectedIndex].QuizName;
                if (quizzes[QuizListBox.SelectedIndex].Questions.Count <= 0)
                {
                    QuizName.Text = quizOldName;
                }
                else
                {
                    foreach (Question question in quizzes[QuizListBox.SelectedIndex].Questions)
                    {
                        questions.Add(question);
                    }

                    QuizName.Text = quizOldName;
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
            }
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

            if (questions.Any(q => q.QuestionNumber == questionIndex))
            {
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
            } else
            {
                ClearView();
            }
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

        private void DeleteQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            string[] fileToDelete = Directory.GetFiles("C:\\Users\\2000g\\OneDrive\\Pulpit\\QUIZY\\", $"{quizzes[QuizListBox.SelectedIndex].QuizName}.txt");
            if (fileToDelete.Length > 1 && fileToDelete.Length < 0)
            {
                MessageBox.Show("We are having troubles...");
            }
            File.Delete(fileToDelete[0]);

            UpdateQuizListBox();
        }

        #endregion

        #region ZAPIS QUIZU DO PLIKU
        private void SaveQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            SaveQuestion();

            if (previousLocation == "Modify")
            {
                File.Move(quizOldName, QuizName.Text);
            }

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

        public void ClearView()
        {
            if (previousLocation == "Menu")
            {
                QuizName.Text = "Nazwa quizu";
            }
            QuestionText.Text = "Treść pytania";
            AnswerA.Text = "Odpowiedź A";
            AnswerB.Text = "Odpowiedź B";
            AnswerC.Text = "Odpowiedź C";
            AnswerD.Text = "Odpowiedź D";
            IsRightA.IsChecked = false;
            IsRightB.IsChecked = false;
            IsRightC.IsChecked = false;
            IsRightD.IsChecked = false;
            QuestionTime.Text = "10";
        }
        
        public void UpdateQuizListBox()
        {
            QuizListBox.Items.Clear();
            quizzes.Clear();

            quizzes = fileHandle.ReadFromFile();

            if (quizzes.Count <= 0)
            {
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            } else
            {
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }

            foreach (Quiz quiz in quizzes)
            {
                QuizListBox.Items.Add(quiz.QuizName);
            }
        }

        #endregion
    }
}
