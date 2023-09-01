namespace DicomViewerDemo
{
    partial class VoiLutParamsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VoiLutParamsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.voiLutsComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.windowCenterNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.windowWidthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.voiLutPanel = new System.Windows.Forms.Panel();
            this.voiLutSearchMethodComboBox = new System.Windows.Forms.ComboBox();
            this.calculateVoiLutButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.windowCenterNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowWidthNumericUpDown)).BeginInit();
            this.voiLutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "VOI LUT";
            // 
            // voiLutsComboBox
            // 
            this.voiLutsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voiLutsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.voiLutsComboBox.FormattingEnabled = true;
            this.voiLutsComboBox.Location = new System.Drawing.Point(95, 0);
            this.voiLutsComboBox.Name = "voiLutsComboBox";
            this.voiLutsComboBox.Size = new System.Drawing.Size(242, 23);
            this.voiLutsComboBox.TabIndex = 1;
            this.voiLutsComboBox.SelectedIndexChanged += new System.EventHandler(this.voiLutsComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 2;
            resources.ApplyResources(this.label2, "label2");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 15);
            this.label3.TabIndex = 3;
            resources.ApplyResources(this.label3, "label3");
            // 
            // windowCenterNumericUpDown
            // 
            this.windowCenterNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windowCenterNumericUpDown.DecimalPlaces = 1;
            this.windowCenterNumericUpDown.Location = new System.Drawing.Point(95, 55);
            this.windowCenterNumericUpDown.Name = "windowCenterNumericUpDown";
            this.windowCenterNumericUpDown.Size = new System.Drawing.Size(242, 23);
            this.windowCenterNumericUpDown.TabIndex = 4;
            this.windowCenterNumericUpDown.ValueChanged += new System.EventHandler(this.windowCenterNumericUpDown_ValueChanged);
            // 
            // windowWidthNumericUpDown
            // 
            this.windowWidthNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windowWidthNumericUpDown.DecimalPlaces = 1;
            this.windowWidthNumericUpDown.Location = new System.Drawing.Point(95, 81);
            this.windowWidthNumericUpDown.Name = "windowWidthNumericUpDown";
            this.windowWidthNumericUpDown.Size = new System.Drawing.Size(242, 23);
            this.windowWidthNumericUpDown.TabIndex = 5;
            this.windowWidthNumericUpDown.ValueChanged += new System.EventHandler(this.windowWidthNumericUpDown_ValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(5, 121);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.Size = new System.Drawing.Size(340, 43);
            this.textBox1.TabIndex = 6;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Press left mouse button on image for changing VOI LUT.";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // voiLutPanel
            // 
            this.voiLutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voiLutPanel.Controls.Add(this.voiLutSearchMethodComboBox);
            this.voiLutPanel.Controls.Add(this.calculateVoiLutButton);
            this.voiLutPanel.Controls.Add(this.voiLutsComboBox);
            this.voiLutPanel.Controls.Add(this.label1);
            this.voiLutPanel.Controls.Add(this.windowWidthNumericUpDown);
            this.voiLutPanel.Controls.Add(this.label2);
            this.voiLutPanel.Controls.Add(this.windowCenterNumericUpDown);
            this.voiLutPanel.Controls.Add(this.label3);
            this.voiLutPanel.Location = new System.Drawing.Point(5, 12);
            this.voiLutPanel.Name = "voiLutPanel";
            this.voiLutPanel.Size = new System.Drawing.Size(340, 103);
            this.voiLutPanel.TabIndex = 7;
            // 
            // voiLutSearchMethodComboBox
            // 
            this.voiLutSearchMethodComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voiLutSearchMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.voiLutSearchMethodComboBox.FormattingEnabled = true;
            this.voiLutSearchMethodComboBox.Location = new System.Drawing.Point(95, 28);
            this.voiLutSearchMethodComboBox.Name = "voiLutSearchMethodComboBox";
            this.voiLutSearchMethodComboBox.Size = new System.Drawing.Size(174, 23);
            this.voiLutSearchMethodComboBox.TabIndex = 7;
            this.voiLutSearchMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.voiLutSearchMethodComboBox_SelectedIndexChanged);
            // 
            // calculateVoiLutButton
            // 
            this.calculateVoiLutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.calculateVoiLutButton.Location = new System.Drawing.Point(275, 27);
            this.calculateVoiLutButton.Name = "calculateVoiLutButton";
            this.calculateVoiLutButton.Size = new System.Drawing.Size(62, 22);
            this.calculateVoiLutButton.TabIndex = 6;
            this.calculateVoiLutButton.Text = "Calculate";
            this.calculateVoiLutButton.UseVisualStyleBackColor = true;
            this.calculateVoiLutButton.Click += new System.EventHandler(this.calculateVoiLutButton_Click);
            // 
            // VoiLutParamsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(357, 161);
            this.Controls.Add(this.voiLutPanel);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VoiLutParamsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.windowCenterNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowWidthNumericUpDown)).EndInit();
            this.voiLutPanel.ResumeLayout(false);
            this.voiLutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox voiLutsComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown windowCenterNumericUpDown;
        private System.Windows.Forms.NumericUpDown windowWidthNumericUpDown;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel voiLutPanel;
        private System.Windows.Forms.Button calculateVoiLutButton;
        private System.Windows.Forms.ComboBox voiLutSearchMethodComboBox;
    }
}