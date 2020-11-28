using System;
using System.Diagnostics;
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
        }
    }
}