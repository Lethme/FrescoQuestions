using System;
using System.Windows.Forms;
using Tree;

namespace FrescoQuestions
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UI.Initialize(this);

            UI.StartTest(
                Question.Create("Вопрос 1", "a1", "a2", 5, AnswerType.Left),
                Question.Create("Вопрос 2", "b1", "b2", 3, AnswerType.Right),
                Question.Create("Вопрос 3", "c1", "c2", 7, AnswerType.Left),
                Question.Create("Вопрос 4", "d1", "d2", 2, AnswerType.Left),
                Question.Create("Вопрос 5", "e1", "e2", 4, AnswerType.Left),
                Question.Create("Вопрос 6", "f1", "f2", 8, AnswerType.Left),
                Question.Create("Вопрос 7", "g1", "g2", 5, AnswerType.Left)
            );
        }
    }
}