
namespace FrescoQuestions
{
    partial class SolutionPathForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cumboBox1 = new System.Windows.Forms.ComboBox();
            this.cumboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cumboBox1
            // 
            this.cumboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cumboBox1.FormattingEnabled = true;
            this.cumboBox1.Location = new System.Drawing.Point(12, 12);
            this.cumboBox1.Name = "cumboBox1";
            this.cumboBox1.Size = new System.Drawing.Size(726, 21);
            this.cumboBox1.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.cumboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cumboBox2.FormattingEnabled = true;
            this.cumboBox2.Location = new System.Drawing.Point(12, 39);
            this.cumboBox2.Name = "comboBox2";
            this.cumboBox2.Size = new System.Drawing.Size(726, 21);
            this.cumboBox2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(726, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Найти путь от вопроса к решению";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 95);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(726, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Найти путь от корня до вопроса";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SolutionPathForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 128);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cumboBox2);
            this.Controls.Add(this.cumboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SolutionPathForm";
            this.Text = "SolutionPathForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cumboBox1;
        private System.Windows.Forms.ComboBox cumboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}