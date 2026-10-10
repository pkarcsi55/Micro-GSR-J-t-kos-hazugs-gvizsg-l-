namespace BLE_test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            textBoxData = new TextBox();
            timerMeasure = new System.Windows.Forms.Timer(components);
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            buttonBaseline = new Button();
            comboMicrobit = new ComboBox();
            buttonConnect = new Button();
            labelPin0 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            buttonStopPlot = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(173, 3);
            button1.Name = "button1";
            button1.Size = new Size(68, 30);
            button1.TabIndex = 0;
            button1.Text = "Keresés";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBoxData
            // 
            textBoxData.Location = new Point(718, 3);
            textBoxData.Multiline = true;
            textBoxData.Name = "textBoxData";
            textBoxData.Size = new Size(325, 33);
            textBoxData.TabIndex = 1;
            // 
            // timerMeasure
            // 
            timerMeasure.Tick += timerMeasure_Tick;
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(52, 184);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(797, 291);
            formsPlot1.TabIndex = 2;
            // 
            // buttonBaseline
            // 
            buttonBaseline.Location = new Point(412, 3);
            buttonBaseline.Name = "buttonBaseline";
            buttonBaseline.Size = new Size(69, 30);
            buttonBaseline.TabIndex = 3;
            buttonBaseline.Text = "Alapvonal";
            buttonBaseline.UseVisualStyleBackColor = true;
            buttonBaseline.Click += buttonBaseline_Click;
            // 
            // comboMicrobit
            // 
            comboMicrobit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMicrobit.FormattingEnabled = true;
            comboMicrobit.Location = new Point(3, 3);
            comboMicrobit.Name = "comboMicrobit";
            comboMicrobit.Size = new Size(85, 23);
            comboMicrobit.TabIndex = 4;
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new Point(270, 3);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(83, 30);
            buttonConnect.TabIndex = 5;
            buttonConnect.Text = "Kapcsolódás";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // labelPin0
            // 
            labelPin0.AutoSize = true;
            labelPin0.Location = new Point(497, 0);
            labelPin0.Name = "labelPin0";
            labelPin0.Size = new Size(38, 15);
            labelPin0.TabIndex = 6;
            labelPin0.Text = "label1";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.7762241F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.5664339F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19.86014F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.8881121F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.6083918F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19.3007F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 337F));
            tableLayoutPanel1.Controls.Add(button1, 1, 0);
            tableLayoutPanel1.Controls.Add(buttonBaseline, 3, 0);
            tableLayoutPanel1.Controls.Add(textBoxData, 6, 0);
            tableLayoutPanel1.Controls.Add(buttonConnect, 2, 0);
            tableLayoutPanel1.Controls.Add(comboMicrobit, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonStopPlot, 5, 0);
            tableLayoutPanel1.Controls.Add(labelPin0, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1054, 39);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // buttonStopPlot
            // 
            buttonStopPlot.Location = new Point(580, 3);
            buttonStopPlot.Name = "buttonStopPlot";
            buttonStopPlot.Size = new Size(93, 23);
            buttonStopPlot.TabIndex = 7;
            buttonStopPlot.Text = "StopPlot";
            buttonStopPlot.UseVisualStyleBackColor = true;
            buttonStopPlot.Click += buttonStopPlot_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1054, 549);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(formsPlot1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBoxData;
        private System.Windows.Forms.Timer timerMeasure;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button buttonBaseline;
        private ComboBox comboMicrobit;
        private Button buttonConnect;
        private Label labelPin0;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonStopPlot;
    }
}
