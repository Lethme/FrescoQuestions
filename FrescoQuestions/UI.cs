using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tree;
using Newtonsoft.Json;

namespace FrescoQuestions
{
    public static class UI
    {
        public static Form1 FormHandler { get; private set; }
        public static int FormWidth => FormHandler.Width;
        public static int FormHeight => FormHandler.Height;
        public static Control.ControlCollection Controls => FormHandler.Controls;
        public static List<Question> Questions => JsonConvert.DeserializeObject<List<Question>>(Encoding.UTF8.GetString(Properties.Resources.Questions));
        public static QuestionPath QuestionPath { get; private set; }
        private static bool Initialized { get; set; } = false;
        public static bool IsTestFinished { get; private set; } = false;
        public static void Initialize(Form1 formHandler)
        {
            if (formHandler == null) throw new NullReferenceException();
            
            if (!Initialized)
            {
                FormHandler = formHandler;
                FormHandler.Text = Properties.Resources.AppName;
                FormHandler.exitMenuItem.Click += (s, e) => { Application.Exit(); };
                FormHandler.aboutMenuItem.Click += (s, e) => { (new AboutForm()).ShowDialog(); };

                Initialized = true;
            }
        }
        private static void CreateButtonsEvents()
        {
            Renderer.Test.LeftButton.Click += (s, e) =>
            {
                QuestionPath.SelectAnswer(AnswerType.Left);

                if (QuestionPath.NextQuestion())
                {
                    Renderer.Test.Render(QuestionPath.CurrentQuestion);
                }
                else
                {
                    IsTestFinished = true;
                    MessageBox.Show($"Your score: {QuestionPath.TotalCount}");
                }
            };
            Renderer.Test.RightButton.Click += (s, e) =>
            {
                QuestionPath.SelectAnswer(AnswerType.Right);

                if (QuestionPath.NextQuestion())
                {
                    Renderer.Test.Render(QuestionPath.CurrentQuestion);
                }
                else
                {
                    IsTestFinished = true;
                    MessageBox.Show($"Your score: {QuestionPath.TotalCount}");
                }
            };
        }
        public static void StartTest(QuestionPath questionPath)
        {
            QuestionPath = questionPath;
            Renderer.Test.Render(QuestionPath.CurrentQuestion);
            CreateButtonsEvents();
        }
        public static void StartTest(IEnumerable<Question> questions)
        {
            QuestionPath = QuestionPath.Create(questions);
            Renderer.Test.Render(QuestionPath.CurrentQuestion);
            CreateButtonsEvents();
        }
        public static void StartTest(params Question[] questions)
        {
            QuestionPath = QuestionPath.Create(questions);
            Renderer.Test.Render(QuestionPath.CurrentQuestion);
            CreateButtonsEvents();
        }
        public static class Renderer
        {
            public static class Test
            {
                public static Button LeftButton { get; private set; } = new Button();
                public static Button RightButton { get; private set; } = new Button();
                public static Label QuestionLabel { get; private set; } = new Label();
                public static void InitializeComponents(Question question)
                {
                    /* Question label properties */
                    QuestionLabel.Text = question.QuestionText;
                    QuestionLabel.Dock = DockStyle.Fill;
                    QuestionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    QuestionLabel.Font = new System.Drawing.Font("Calibri Light", 24); ;

                    /* No button properties */
                    RightButton.Height = 30;
                    RightButton.Dock = DockStyle.Bottom;
                    RightButton.Text = question.QuestionAnswers.Item2;
                    RightButton.Font = new System.Drawing.Font("Calibri Light", 12); ;

                    /* Yes button properties */
                    LeftButton.Height = 30;
                    LeftButton.Dock = DockStyle.Bottom;
                    LeftButton.Text = question.QuestionAnswers.Item1;
                    LeftButton.Font = new System.Drawing.Font("Calibri Light", 12); ;
                }
                public static void Render(Question question)
                {
                    InitializeComponents(question);
                    Controls.Add(LeftButton);
                    Controls.Add(RightButton);
                    Controls.Add(QuestionLabel);
                }
            }
        }
    }
}
