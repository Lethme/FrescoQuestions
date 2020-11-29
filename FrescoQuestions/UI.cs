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
        private static QuestionPath QuestionPath { get; set; }
        private static bool Initialized { get; set; } = false;
        public static bool IsTestFinished { get; private set; } = true;
        public static void Initialize<TForm>(TForm formHandler) where TForm : Form
        {
            if (formHandler == null) throw new NullReferenceException();

            if (!Initialized)
            {
                FormHandler = formHandler;

                /* Form properties */
                FormHandler.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                FormHandler.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                FormHandler.MinimumSize = new System.Drawing.Size(800, 550);
                FormHandler.ClientSize = FormHandler.MinimumSize;
                FormHandler.Name = "Form1";
                FormHandler.Text = Properties.Resources.AppName;
                FormHandler.ResumeLayout(false);
                FormHandler.Icon = Properties.Resources.Icon;

                /* Components events */
                Components.ExitMenuItem.Click += (s, e) => 
                {
                    if (Confirmation("Вы действительно хотите выйти в окно?", "Выйти в окно"))
                    {
                        Application.Exit();
                    }
                };
                Components.AboutMenuItem.Click += (s, e) => { (new AboutForm()).ShowDialog(); };
                Components.StartTestMenuItem.Click += (s, e) => { UI.StartTest(Questions); };
                Components.PathFromLeafMenuItem.Click += (s, e) => { (new SolutionPathForm(QuestionPath)).ShowDialog(); };

                /* Initial rendering */
                Renderer.Menu.Render();
                StartTest(Questions);
                Renderer.Test.CreateButtonsEvents();

                Initialized = true;
            }
        }
        public static void StartTest(QuestionPath questionPath)
        {
            if (IsTestFinished)
            {
                if (QuestionPath == null) QuestionPath = questionPath;
                else QuestionPath.Reset();
                Renderer.PostScreen.Release();
                Renderer.Test.Render(QuestionPath.CurrentQuestion);

                IsTestFinished = false;
            }
        }
        public static void StartTest(IEnumerable<Question> questions)
        {
            if (IsTestFinished)
            {
                if (QuestionPath == null) QuestionPath = QuestionPath.Create(questions);
                else QuestionPath.Reset();
                Renderer.PostScreen.Release();
                Renderer.Test.Render(QuestionPath.CurrentQuestion);

                IsTestFinished = false;
            }
        }
        public static void StartTest(params Question[] questions)
        {
            if (IsTestFinished)
            {
                if (QuestionPath == null) QuestionPath = QuestionPath.Create(questions);
                else QuestionPath.Reset();
                Renderer.PostScreen.Release();
                Renderer.Test.Render(QuestionPath.CurrentQuestion);

                IsTestFinished = false;
            }
        }
        public static bool Confirmation(string ConfirmationText, string ConfirmationTitle, MessageBoxDefaultButton DefaultButton = MessageBoxDefaultButton.Button1)
        {
            if (ConfirmationText == String.Empty || ConfirmationTitle == String.Empty)
                throw new ArgumentNullException();

            var ConfirmationResult = MessageBox.Show
            (
                ConfirmationText,
                ConfirmationTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                DefaultButton
            );

            if (ConfirmationResult == DialogResult.Yes) return true;
            return false;
        }
        public static class Components
        {
            public static ToolStripMenuItem TestMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem HelpMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem StartTestMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem ExitMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem AboutMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem TreeAlgorithmMenuItem { get; private set; } = new ToolStripMenuItem();
            public static ToolStripMenuItem PathFromLeafMenuItem { get; private set; } = new ToolStripMenuItem();
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
                    QuestionLabel.Text = $"Ваш счёт: {QuestionPath.TotalScore}\n{question.QuestionText}";
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

                    if (!ControlCollection.Contains(LeftButton)) ControlCollection.Add(LeftButton);
                    if (!ControlCollection.Contains(RightButton)) ControlCollection.Add(RightButton);
                    if (!ControlCollection.Contains(QuestionLabel)) ControlCollection.Add(QuestionLabel);
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
                            Test.Release();
                            PostScreen.Render();
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
                            Test.Release();
                            PostScreen.Render();
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
                public static void Release()
                {
                    foreach (var control in ControlCollection)
                    {
                        Controls.Remove((Control)control);
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
                    Components.HelpMenuItem,
                    Components.TreeAlgorithmMenuItem});
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

                    Components.PathFromLeafMenuItem.Name = "PathFromLeafMenuItem";
                    Components.PathFromLeafMenuItem.Text = "Путь от заданного листа до решения";

                    Components.TreeAlgorithmMenuItem.Name = "TreeAlgorithmMenuItem";
                    Components.TreeAlgorithmMenuItem.Text = "Алгоритмы дерева";
                    Components.TreeAlgorithmMenuItem.DropDownItems.AddRange(new ToolStripItem[]
                    {
                        Components.PathFromLeafMenuItem
                    });

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
            public static class PostScreen
            {
                public static Label ResultLabel { get; private set; } = new Label();
                public static Control.ControlCollection ControlCollection { get; private set; } = new Control.ControlCollection(new Control());
                public static void InitializeComponents()
                {
                    ResultLabel.Dock = DockStyle.Fill;
                    ResultLabel.Font = new System.Drawing.Font("Calibri Light", 10);
                    ResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                    var pathList = QuestionPath.QuestionPathList.Select(item => $"{item.QuestionText}\n{(item.ChosenAnswer == item.RightAnswer ? $":: {item.ChosenAnswerText} ::" : item.ChosenAnswerText)}");
                    ResultLabel.Text = $"Ваш счёт: {QuestionPath.TotalScore}\n" +
                        $"{(QuestionPath.TotalScore == 7 ? $"Вы уже настолько преисполнились..." : $"Вы – {QuestionPath.WhoAmI()}")}\n\n" +
                        $"Ваши ответы: \n\n" +
                        $"{pathList.AsIndentedString("\n\n")}";

                    if (!ControlCollection.Contains(ResultLabel)) ControlCollection.Add(ResultLabel);
                }
                public static void Render()
                {
                    InitializeComponents();
                    foreach (var item in ControlCollection)
                    {
                        if (!Controls.Contains((Control)item)) Controls.Add((Control)item);
                    }
                }
                public static void Release()
                {
                    foreach (var control in ControlCollection)
                    {
                        Controls.Remove((Control)control);
                    }
                }
            }
        }
    }
}
