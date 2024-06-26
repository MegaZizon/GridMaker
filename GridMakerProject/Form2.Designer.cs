namespace GridMakerProject
{
    partial class Form2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2_1 = new System.Windows.Forms.Button();
            this.groupBox2_3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown2_2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2_1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.button2_7 = new System.Windows.Forms.Button();
            this.groupBox2_2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown2_4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2_3 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog2_1 = new System.Windows.Forms.ColorDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox2_3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_1)).BeginInit();
            this.groupBox2_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button2_1);
            this.groupBox1.Controls.Add(this.groupBox2_3);
            this.groupBox1.Controls.Add(this.groupBox2_2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 329);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 239);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 38);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(212, 16);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "모든 우클릭에 대해 같은 설정 적용";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button2_1
            // 
            this.button2_1.AutoSize = true;
            this.button2_1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button2_1.Location = new System.Drawing.Point(3, 283);
            this.button2_1.Name = "button2_1";
            this.button2_1.Size = new System.Drawing.Size(231, 43);
            this.button2_1.TabIndex = 0;
            this.button2_1.Text = "적용하기";
            this.button2_1.UseVisualStyleBackColor = true;
            this.button2_1.Click += new System.EventHandler(this.button2_1_Click);
            // 
            // groupBox2_3
            // 
            this.groupBox2_3.Controls.Add(this.label7);
            this.groupBox2_3.Controls.Add(this.label6);
            this.groupBox2_3.Controls.Add(this.numericUpDown2_2);
            this.groupBox2_3.Controls.Add(this.numericUpDown2_1);
            this.groupBox2_3.Controls.Add(this.label5);
            this.groupBox2_3.Controls.Add(this.button2_7);
            this.groupBox2_3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2_3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2_3.Location = new System.Drawing.Point(3, 125);
            this.groupBox2_3.Name = "groupBox2_3";
            this.groupBox2_3.Size = new System.Drawing.Size(231, 114);
            this.groupBox2_3.TabIndex = 16;
            this.groupBox2_3.TabStop = false;
            this.groupBox2_3.Text = "세부 선 설정";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(53, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "색상";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(53, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "투명도";
            // 
            // numericUpDown2_2
            // 
            this.numericUpDown2_2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2_2.Location = new System.Drawing.Point(114, 50);
            this.numericUpDown2_2.Name = "numericUpDown2_2";
            this.numericUpDown2_2.Size = new System.Drawing.Size(64, 21);
            this.numericUpDown2_2.TabIndex = 19;
            this.numericUpDown2_2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericUpDown2_1
            // 
            this.numericUpDown2_1.Location = new System.Drawing.Point(114, 23);
            this.numericUpDown2_1.Name = "numericUpDown2_1";
            this.numericUpDown2_1.Size = new System.Drawing.Size(64, 21);
            this.numericUpDown2_1.TabIndex = 18;
            this.numericUpDown2_1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(53, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "굵기";
            // 
            // button2_7
            // 
            this.button2_7.BackColor = System.Drawing.Color.Black;
            this.button2_7.Location = new System.Drawing.Point(114, 77);
            this.button2_7.Name = "button2_7";
            this.button2_7.Size = new System.Drawing.Size(65, 22);
            this.button2_7.TabIndex = 12;
            this.button2_7.UseVisualStyleBackColor = false;
            this.button2_7.Click += new System.EventHandler(this.button2_7_Click);
            // 
            // groupBox2_2
            // 
            this.groupBox2_2.Controls.Add(this.numericUpDown2_4);
            this.groupBox2_2.Controls.Add(this.numericUpDown2_3);
            this.groupBox2_2.Controls.Add(this.label4);
            this.groupBox2_2.Controls.Add(this.label3);
            this.groupBox2_2.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox2_2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2_2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2_2.Location = new System.Drawing.Point(3, 17);
            this.groupBox2_2.Name = "groupBox2_2";
            this.groupBox2_2.Size = new System.Drawing.Size(231, 108);
            this.groupBox2_2.TabIndex = 15;
            this.groupBox2_2.TabStop = false;
            this.groupBox2_2.Text = "세부 행렬 설정";
            // 
            // numericUpDown2_4
            // 
            this.numericUpDown2_4.Location = new System.Drawing.Point(115, 57);
            this.numericUpDown2_4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2_4.Name = "numericUpDown2_4";
            this.numericUpDown2_4.Size = new System.Drawing.Size(64, 21);
            this.numericUpDown2_4.TabIndex = 26;
            this.numericUpDown2_4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2_4.ValueChanged += new System.EventHandler(this.numericUpDown2_4_ValueChanged);
            // 
            // numericUpDown2_3
            // 
            this.numericUpDown2_3.Location = new System.Drawing.Point(115, 30);
            this.numericUpDown2_3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2_3.Name = "numericUpDown2_3";
            this.numericUpDown2_3.Size = new System.Drawing.Size(64, 21);
            this.numericUpDown2_3.TabIndex = 25;
            this.numericUpDown2_3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2_3.ValueChanged += new System.EventHandler(this.numericUpDown2_3_ValueChanged);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(54, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "열";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(54, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "행";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 329);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "그리드 세부 설정";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox2_3.ResumeLayout(false);
            this.groupBox2_3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_1)).EndInit();
            this.groupBox2_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2_3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2_3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown2_2;
        private System.Windows.Forms.NumericUpDown numericUpDown2_1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2_7;
        private System.Windows.Forms.GroupBox groupBox2_2;
        private System.Windows.Forms.NumericUpDown numericUpDown2_4;
        private System.Windows.Forms.NumericUpDown numericUpDown2_3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2_1;
        private System.Windows.Forms.ColorDialog colorDialog2_1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox checkBox1;
    }
}