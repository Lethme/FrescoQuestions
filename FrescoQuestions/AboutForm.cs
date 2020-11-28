using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrescoQuestions
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            label1.Text = Properties.Resources.AppName;
            MaximumSize = new Size(Width, Height);
            MinimumSize = new Size(Width, Height);
            Icon = Properties.Resources.Icon;
        }
    }
}
