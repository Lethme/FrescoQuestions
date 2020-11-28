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
        private static Form FormHandler { get; set; }
        private static int FormWidth => FormHandler.Width;
        private static int FormHeight => FormHandler.Height;
        private static Control.ControlCollection Controls => FormHandler.Controls;
        public static List<Question> Questions => JsonConvert.DeserializeObject<List<Question>>(Encoding.UTF8.GetString(Properties.Resources.Questions));
        public static QuestionPath QuestionPath { get; private set; }
        private static bool Initialized { get; set; } = false;
        public static bool IsTestFinished { get; private set; } = true;
        public static void Initialize<T>(T formHandler) where T : Form
        {
            if (formHandler == null) throw new NullReferenceException();
            
            if (!Initialized)
            {
                FormHandler = formHandler;
                FormHandler.Text = Properties.Resources.AppName;
                Components.ExitMenuItem.Click += (s, e) => { Application.Exit(); };
                Components.AboutMenuItem.Click += (s, e) => { (new AboutForm()).ShowDialog(); };
                Renderer.Menu.Render();

                Initialized = true;
            }
        }
        public static void StartTest(QuestionPath questionPath)
        {
            if (IsTestFinished)
            {
                QuestionPath = questionPath;
                Renderer.Test.Render(QuestionPath.CurrentQuestion);
                Renderer.Test.CreateButtonsEvents();

                IsTestFinished = false;
            }
        }
        public static void StartTest(IEnumerable<Question> questions)
        {
            if (IsTestFinished)
            {
                QuestionPath = QuestionPath.Create(questions);
                Renderer.Test.Render(QuestionPath.CurrentQuestion);
                Renderer.Test.CreateButtonsEvents();

                IsTestFinished = false;
            }
        }
        public static void StartTest(params Question[] questions)
        {
            if (IsTestFinished)
            {
                QuestionPath = QuestionPath.Create(questions);
                Renderer.Test.Render(QuestionPath.CurrentQuestion);
                Renderer.Test.CreateButtonsEvents();

                IsTestFinished = false;
            }
        }
        public static class Components
        {
            public static ToolStripMenuItem TestMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem HelpMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem StartTestMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem ExitMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem AboutMenuItem { get; private set; } = new ToolStripMenuItem();
            public static MenuStrip MainMenu { get; private set; } = new MenuStrip();
        }
        private static class Renderer
        {
            public static class Test
            {
                public static Button LeftButton { get; private set; } = new Button();
                public static Button RightButton { get; private set; } = new Button();
                public static Label QuestionLabel { get; private set; } = new Label();
                public static Control.ControlCollection ControlCollection { get; private set; } = new Control.ControlCollection(new Control());
                public static void InitializeComponents(Question question)
                {
                    /* Question label properties */
                    QuestionLabel.Text = question.QuestionText;
                    QuestionLabel.Dock = DockStyle.Fill;
                    QuestionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    QuestionLabel.Font = new System.Drawing.Font("Calibri Light", 24);

                    /* No button properties */
                    RightButton.Height = 30;
                    RightButton.Dock = DockStyle.Bottom;
                    RightButton.Text = question.QuestionAnswers.Item2;
                    RightButton.Font = new System.Drawing.Font("Calibri Light", 12);

                    /* Yes button properties */
                    LeftButton.Height = 30;
                    LeftButton.Dock = DockStyle.Bottom;
                    LeftButton.Text = question.QuestionAnswers.Item1;
                    LeftButton.Font = new System.Drawing.Font("Calibri Light", 12);

                    ControlCollection.Add(LeftButton);
                    ControlCollection.Add(RightButton);
                    ControlCollection.Add(QuestionLabel);
                }
                public static void CreateButtonsEvents()
                {
                    LeftButton.Click += (s, e) =>
                    {
                        QuestionPath.SelectAnswer(AnswerType.Left);

                        if (QuestionPath.NextQuestion())
                        {
                            Render(QuestionPath.CurrentQuestion);
                        }
                        else
                        {
                            IsTestFinished = true;
                            MessageBox.Show($"Your score: {QuestionPath.TotalCount}");
                        }
                    };
                    RightButton.Click += (s, e) =>
                    {
                        QuestionPath.SelectAnswer(AnswerType.Right);

                        if (QuestionPath.NextQuestion())
                        {
                            Render(QuestionPath.CurrentQuestion);
                        }
                        else
                        {
                            IsTestFinished = true;
                            MessageBox.Show($"Your score: {QuestionPath.TotalCount}");
                        }
                    };
                }
                public static void Render(Question question)
                {
                    InitializeComponents(question);
                    foreach (var control in ControlCollection)
                    {
                        if (!Controls.Contains((Control)control)) Controls.Add((Control)control);
                    }
                }
            }
            public static class Menu
            {
                public static Control.ControlCollection ControlCollection { get; private set; } = new Control.ControlCollection(new Control());
                public static void InitializeComponents()
                {
                    /* Main menu properties */
                    Components.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    Components.TestMenuItem,
                    Components.HelpMenuItem});
                    Components.MainMenu.Location = new System.Drawing.Point(0, 0);
                    Components.MainMenu.Name = "MainMenu";
                    Components.MainMenu.Size = new System.Drawing.Size(FormWidth, 24);
                    Components.MainMenu.TabIndex = 0;
                    Components.MainMenu.Text = "menuStrip1";

                    /* Test menu properties */
                    Components.TestMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    Components.StartTestMenuItem,
                    Components.ExitMenuItem});
                    Components.TestMenuItem.Name = "TestMenuItem";
                    Components.TestMenuItem.Size = new System.Drawing.Size(42, 20);
                    Components.TestMenuItem.Text = "Тест";

                    /* Help menu properties */
                    Components.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    Components.AboutMenuItem});
                    Components.HelpMenuItem.Name = "HelpMenuItem";
                    Components.HelpMenuItem.Size = new System.Drawing.Size(68, 20);
                    Components.HelpMenuItem.Text = "Помощь";

                    /* Start test menu item */
                    Components.StartTestMenuItem.Name = "StartTestMenuItem";
                    Components.StartTestMenuItem.Size = new System.Drawing.Size(180, 22);
                    Components.StartTestMenuItem.Text = "Начать";

                    /* Exit menu item */
                    Components.ExitMenuItem.Name = "ExitMenuItem";
                    Components.ExitMenuItem.Size = new System.Drawing.Size(180, 22);
                    Components.ExitMenuItem.Text = "Выйти в окно";

                    /* About menu item */
                    Components.AboutMenuItem.Name = "AboutMenuItem";
                    Components.AboutMenuItem.Size = new System.Drawing.Size(149, 22);
                    Components.AboutMenuItem.Text = "О программе";

                    ControlCollection.Add(Components.MainMenu);
                }
                public static void Render()
                {
                    InitializeComponents();
                    foreach (var control in ControlCollection)
                    {
                        if (!Controls.Contains((Control)control)) Controls.Add((Control)control);
                    }
                }
            }
        }
    }
}
