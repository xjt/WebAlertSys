namespace WebAlertSys
{
    /// <summary>
    /// 参数设置窗口
    /// </summary>
    partial class Form_Set
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
            this.textBox_code = new System.Windows.Forms.TextBox();
            this.numericUpDown_min = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_max = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_Validate = new System.Windows.Forms.CheckBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.radioButton_sz = new System.Windows.Forms.RadioButton();
            this.radioButton_sh = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_AutoRun = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_max)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_code
            // 
            this.textBox_code.Location = new System.Drawing.Point(14, 41);
            this.textBox_code.Name = "textBox_code";
            this.textBox_code.Size = new System.Drawing.Size(83, 21);
            this.textBox_code.TabIndex = 0;
            // 
            // numericUpDown_min
            // 
            this.numericUpDown_min.DecimalPlaces = 2;
            this.numericUpDown_min.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_min.Location = new System.Drawing.Point(14, 93);
            this.numericUpDown_min.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown_min.Name = "numericUpDown_min";
            this.numericUpDown_min.Size = new System.Drawing.Size(75, 21);
            this.numericUpDown_min.TabIndex = 2;
            this.numericUpDown_min.Tag = "止损价";
            // 
            // numericUpDown_max
            // 
            this.numericUpDown_max.DecimalPlaces = 2;
            this.numericUpDown_max.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_max.Location = new System.Drawing.Point(120, 93);
            this.numericUpDown_max.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown_max.Name = "numericUpDown_max";
            this.numericUpDown_max.Size = new System.Drawing.Size(83, 21);
            this.numericUpDown_max.TabIndex = 3;
            this.numericUpDown_max.Tag = "止盈价";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "代码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "监控区间：";
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(120, 20);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 9;
            this.button_OK.Text = "Set";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBox_Validate);
            this.groupBox1.Controls.Add(this.button_Add);
            this.groupBox1.Controls.Add(this.textBox_code);
            this.groupBox1.Controls.Add(this.radioButton_sz);
            this.groupBox1.Controls.Add(this.radioButton_sh);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown_min);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDown_max);
            this.groupBox1.Location = new System.Drawing.Point(28, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 172);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "stock";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "-";
            // 
            // checkBox_Validate
            // 
            this.checkBox_Validate.AutoSize = true;
            this.checkBox_Validate.Location = new System.Drawing.Point(17, 133);
            this.checkBox_Validate.Name = "checkBox_Validate";
            this.checkBox_Validate.Size = new System.Drawing.Size(60, 16);
            this.checkBox_Validate.TabIndex = 12;
            this.checkBox_Validate.Text = "有效性";
            this.checkBox_Validate.UseVisualStyleBackColor = true;
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(121, 129);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Add.TabIndex = 10;
            this.button_Add.Text = "OK";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // radioButton_sz
            // 
            this.radioButton_sz.AutoSize = true;
            this.radioButton_sz.Location = new System.Drawing.Point(161, 43);
            this.radioButton_sz.Name = "radioButton_sz";
            this.radioButton_sz.Size = new System.Drawing.Size(35, 16);
            this.radioButton_sz.TabIndex = 9;
            this.radioButton_sz.TabStop = true;
            this.radioButton_sz.Text = "sz";
            this.radioButton_sz.UseVisualStyleBackColor = true;
            // 
            // radioButton_sh
            // 
            this.radioButton_sh.AutoSize = true;
            this.radioButton_sh.Location = new System.Drawing.Point(120, 43);
            this.radioButton_sh.Name = "radioButton_sh";
            this.radioButton_sh.Size = new System.Drawing.Size(35, 16);
            this.radioButton_sh.TabIndex = 8;
            this.radioButton_sh.TabStop = true;
            this.radioButton_sh.Text = "sh";
            this.radioButton_sh.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox_AutoRun);
            this.groupBox3.Controls.Add(this.button_OK);
            this.groupBox3.Location = new System.Drawing.Point(28, 195);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(216, 61);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Others";
            // 
            // checkBox_AutoRun
            // 
            this.checkBox_AutoRun.AutoSize = true;
            this.checkBox_AutoRun.Location = new System.Drawing.Point(14, 27);
            this.checkBox_AutoRun.Name = "checkBox_AutoRun";
            this.checkBox_AutoRun.Size = new System.Drawing.Size(72, 16);
            this.checkBox_AutoRun.TabIndex = 0;
            this.checkBox_AutoRun.Text = "Auto Run";
            this.checkBox_AutoRun.UseVisualStyleBackColor = true;
            // 
            // Form_Set
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 282);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Set";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parameter Control";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_max)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_code;
        private System.Windows.Forms.NumericUpDown numericUpDown_min;
        private System.Windows.Forms.NumericUpDown numericUpDown_max;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_AutoRun;
        private System.Windows.Forms.RadioButton radioButton_sh;
        private System.Windows.Forms.RadioButton radioButton_sz;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_Validate;
    }
}