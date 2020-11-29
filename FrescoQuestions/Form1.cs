using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Tree;
using System.Linq;

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