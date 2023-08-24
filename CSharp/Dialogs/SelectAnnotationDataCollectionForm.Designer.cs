namespace DicomViewerDemo
{
    partial class SelectAnnotationDataCollectionForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sopClassLabel = new System.Windows.Forms.Label();
            this.sopInstanceLabel = new System.Windows.Forms.Label();
            this.frameNumberLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.annoInfoListView = new System.Windows.Forms.ListView();
            this.type = new System.Windows.Forms.ColumnHeader();
            this.locationColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.selectedAnnotationDataCollectionComboBox = new System.Windows.Forms.ComboBox();
            this.cancelButton1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(319, 313);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.sopClassLabel);
            this.groupBox1.Controls.Add(this.sopInstanceLabel);
            this.groupBox1.Controls.Add(this.frameNumberLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 80);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Collection Info";
            // 
            // sopClassLabel
            // 
            this.sopClassLabel.AutoSize = true;
            this.sopClassLabel.Location = new System.Drawing.Point(91, 16);
            this.sopClassLabel.Name = "sopClassLabel";
            this.sopClassLabel.Size = new System.Drawing.Size(0, 13);
            this.sopClassLabel.TabIndex = 5;
            // 
            // sopInstanceLabel
            // 
            this.sopInstanceLabel.AutoSize = true;
            this.sopInstanceLabel.Location = new System.Drawing.Point(91, 37);
            this.sopInstanceLabel.Name = "sopInstanceLabel";
            this.sopInstanceLabel.Size = new System.Drawing.Size(0, 13);
            this.sopInstanceLabel.TabIndex = 4;
            // 
            // frameNumberLabel
            // 
            this.frameNumberLabel.AutoSize = true;
            this.frameNumberLabel.Location = new System.Drawing.Point(91, 58);
            this.frameNumberLabel.Name = "frameNumberLabel";
            this.frameNumberLabel.Size = new System.Drawing.Size(0, 13);
            this.frameNumberLabel.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Frame Number:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Sop Instance:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sop Class:";
            // 
            // annoInfoListView
            // 
            this.annoInfoListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.annoInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.type,
            this.locationColumnHeader});
            this.annoInfoListView.Location = new System.Drawing.Point(12, 124);
            this.annoInfoListView.Name = "annoInfoListView";
            this.annoInfoListView.Size = new System.Drawing.Size(463, 183);
            this.annoInfoListView.TabIndex = 8;
            this.annoInfoListView.UseCompatibleStateImageBehavior = false;
            this.annoInfoListView.View = System.Windows.Forms.View.Details;
            // 
            // type
            // 
            this.type.Text = "Annotation type";
            this.type.Width = 273;
            // 
            // locationColumnHeader
            // 
            this.locationColumnHeader.Text = "Location";
            this.locationColumnHeader.Width = 183;
            // 
            // selectedAnnotationDataCollectionComboBox
            // 
            this.selectedAnnotationDataCollectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectedAnnotationDataCollectionComboBox.FormattingEnabled = true;
            this.selectedAnnotationDataCollectionComboBox.Location = new System.Drawing.Point(12, 11);
            this.selectedAnnotationDataCollectionComboBox.Name = "selectedAnnotationDataCollectionComboBox";
            this.selectedAnnotationDataCollectionComboBox.Size = new System.Drawing.Size(223, 21);
            this.selectedAnnotationDataCollectionComboBox.TabIndex = 9;
            this.selectedAnnotationDataCollectionComboBox.SelectedIndexChanged += new System.EventHandler(this.selectedAnnotationDataCollectionComboBox_SelectedIndexChanged);
            // 
            // cancelButton
            // 
            this.cancelButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton1.Location = new System.Drawing.Point(400, 313);
            this.cancelButton1.Name = "cancelButton1";
            this.cancelButton1.Size = new System.Drawing.Size(75, 23);
            this.cancelButton1.TabIndex = 10;
            this.cancelButton1.Text = "Cancel";
            this.cancelButton1.UseVisualStyleBackColor = true;
            // 
            // SelectAnnotationDataCollectionForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton1;
            this.ClientSize = new System.Drawing.Size(487, 348);
            this.Controls.Add(this.cancelButton1);
            this.Controls.Add(this.selectedAnnotationDataCollectionComboBox);
            this.Controls.Add(this.annoInfoListView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.MinimumSize = new System.Drawing.Size(503, 292);
            this.Name = "SelectAnnotationDataCollectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SelectAnnotationDataCollection";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label sopClassLabel;
        private System.Windows.Forms.Label sopInstanceLabel;
        private System.Windows.Forms.Label frameNumberLabel;
        private System.Windows.Forms.ListView annoInfoListView;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ColumnHeader locationColumnHeader;
        private System.Windows.Forms.ComboBox selectedAnnotationDataCollectionComboBox;
        private System.Windows.Forms.Button cancelButton1;
    }
}