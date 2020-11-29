using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tree;

namespace FrescoQuestions
{
    public partial class SolutionPathForm : Form
    {
        public SolutionPathForm(QuestionPath questionPath)
        {
            InitializeComponent();
            Text = "Выберите вопрос и интересующий результат";

            cumboBox1.Items.AddRange(questionPath.QuestionList.Select(item => item.QuestionText).ToArray());
            cumboBox1.SelectedIndex = 0;
            cumboBox2.Items.AddRange(QuestionPath.TestRank.ToArray());
            cumboBox2.SelectedIndex = 0;

            button1.Click += (s, e) =>
            {
                var question = questionPath.GetQuestion((String)cumboBox1.SelectedItem);
                var path = questionPath.PathToSolution(question, (String)cumboBox2.SelectedItem);
                var rank = QuestionPath.TestRank.IndexesWhere(x => x == (String)cumboBox2.SelectedItem).First();
                var editedPath = path.Select((item, index) => $"{(index < rank ? $"{item.QuestionText}\n{item.RightAnswerText}" : $"{item.QuestionText}\n{item.WrongAnswetText}")}");
                
                MessageBox.Show
                (
                    $"Путь к решению '{(String)cumboBox2.SelectedItem}'\n\n" +
                    editedPath.AsIndentedString("\n\n")
                );
            };

            button2.Click += (s, e) =>
            {
                var question = questionPath.GetQuestion((String)cumboBox1.SelectedItem);
                var path = questionPath.PathFromRoot(question).Select(item => $"{item.QuestionText}\n{item.ChosenAnswerText}");

                MessageBox.Show
                (
                    $"Путь от корня до выбранного вопроса\n\n" + 
                    path.AsIndentedString("\n\n")
                );
            };
        }
    }
}
