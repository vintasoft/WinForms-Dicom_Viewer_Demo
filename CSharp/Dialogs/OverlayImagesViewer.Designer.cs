namespace DicomViewerDemo
{
    partial class OverlayImagesViewer
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
            this.imageViewer = new Vintasoft.Imaging.UI.ImageViewer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveAsImageButton = new System.Windows.Forms.Button();
            this.overlayImagesComboBox = new System.Windows.Forms.ComboBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageViewerBackColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageViewer
            // 
            this.imageViewer.AutoScroll = true;
            this.imageViewer.BackColor = System.Drawing.Color.Black;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(0, 0);
            this.imageViewer.MaxThreadsForRendering = 2;
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.ShortcutCopy = System.Windows.Forms.Shortcut.None;
            this.imageViewer.ShortcutCut = System.Windows.Forms.Shortcut.None;
            this.imageViewer.ShortcutDelete = System.Windows.Forms.Shortcut.None;
            this.imageViewer.ShortcutInsert = System.Windows.Forms.Shortcut.None;
            this.imageViewer.ShortcutSelectAll = System.Windows.Forms.Shortcut.None;
            this.imageViewer.Size = new System.Drawing.Size(439, 482);
            this.imageViewer.SizeMode = Vintasoft.Imaging.UI.ImageSizeMode.BestFit;
            this.imageViewer.TabIndex = 0;
            this.imageViewer.Text = "ImageViewer";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1MinSize = 160;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imageViewer);
            this.splitContainer1.Size = new System.Drawing.Size(636, 482);
            this.splitContainer1.SplitterDistance = 193;
            this.splitContainer1.TabIndex = 1;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 16);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(159, 357);
            this.propertyGrid.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.saveAsImageButton);
            this.groupBox1.Controls.Add(this.overlayImagesComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 76);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Overlay Image";
            // 
            // saveAsImageButton
            // 
            this.saveAsImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveAsImageButton.Location = new System.Drawing.Point(47, 46);
            this.saveAsImageButton.Name = "saveAsImageButton";
            this.saveAsImageButton.Size = new System.Drawing.Size(110, 23);
            this.saveAsImageButton.TabIndex = 1;
            this.saveAsImageButton.Text = "Save as Image ...";
            this.saveAsImageButton.UseVisualStyleBackColor = true;
            this.saveAsImageButton.Click += new System.EventHandler(this.saveAsImageButton_Click);
            // 
            // overlayImagesComboBox
            // 
            this.overlayImagesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.overlayImagesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.overlayImagesComboBox.FormattingEnabled = true;
            this.overlayImagesComboBox.Location = new System.Drawing.Point(6, 19);
            this.overlayImagesComboBox.Name = "overlayImagesComboBox";
            this.overlayImagesComboBox.Size = new System.Drawing.Size(151, 21);
            this.overlayImagesComboBox.TabIndex = 0;
            this.overlayImagesComboBox.SelectedIndexChanged += new System.EventHandler(this.overlayImagesComboBox_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(636, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageViewerBackColorToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // imageViewerBackColorToolStripMenuItem
            // 
            this.imageViewerBackColorToolStripMenuItem.Name = "imageViewerBackColorToolStripMenuItem";
            this.imageViewerBackColorToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.imageViewerBackColorToolStripMenuItem.Text = "Image Viewer Back Color...";
            this.imageViewerBackColorToolStripMenuItem.Click += new System.EventHandler(this.imageViewerBackColorToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.propertyGrid);
            this.groupBox2.Location = new System.Drawing.Point(12, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 376);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DicomOverlayImage";
            // 
            // OverlayImagesViewer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 506);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "OverlayImagesViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OverlayImagesViewer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Vintasoft.Imaging.UI.ImageViewer imageViewer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveAsImageButton;
        private System.Windows.Forms.ComboBox overlayImagesComboBox;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageViewerBackColorToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}