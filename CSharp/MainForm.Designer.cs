namespace DicomViewerDemo
{
    partial class MainForm
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
            Vintasoft.Imaging.Utils.WinFormsSystemClipboard winFormsSystemClipboard1 = new Vintasoft.Imaging.Utils.WinFormsSystemClipboard();
            Vintasoft.Imaging.Codecs.Decoders.RenderingSettings renderingSettings1 = new Vintasoft.Imaging.Codecs.Decoders.RenderingSettings();
            Vintasoft.Imaging.UI.ThumbnailAppearance thumbnailAppearance1 = new Vintasoft.Imaging.UI.ThumbnailAppearance();
            Vintasoft.Imaging.UI.ThumbnailAppearance thumbnailAppearance2 = new Vintasoft.Imaging.UI.ThumbnailAppearance();
            Vintasoft.Imaging.UI.ThumbnailAppearance thumbnailAppearance3 = new Vintasoft.Imaging.UI.ThumbnailAppearance();
            Vintasoft.Imaging.UI.ThumbnailAppearance thumbnailAppearance4 = new Vintasoft.Imaging.UI.ThumbnailAppearance();
            Vintasoft.Imaging.UI.ThumbnailAppearance thumbnailAppearance5 = new Vintasoft.Imaging.UI.ThumbnailAppearance();
            Vintasoft.Imaging.UI.ThumbnailCaption thumbnailCaption1 = new Vintasoft.Imaging.UI.ThumbnailCaption();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.imageViewer1 = new Vintasoft.Imaging.UI.ImageViewer();
            this.thumbnailViewer1 = new Vintasoft.Imaging.UI.ThumbnailViewer();
            this.annotationComboBox = new System.Windows.Forms.ComboBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDicomFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDicomFileToImageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeDicomFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thumbnailViewerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.imageViewerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clockwiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.counterclockwiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.showOverlayImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overlayColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.showMetadataInViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textOverlaySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.showRulersInViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rulersColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rulersUnitOfMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.voiLutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.negativeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voiLutMouseMoveDirectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.widthHorizontalCenterVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.widthVerticalCenterHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.magnifierSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overlayImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animationDelayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animationDelay_valueToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.animationRepeatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsGifFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.annotationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.interactionModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interactionMode_noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interactionMode_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interactionMode_authorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.presentationStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presentationStateLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presentationStateInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presentationStateSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presentationStateSaveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.presentationStateLoadAutomaticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryFormatLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryFormatSaveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmpFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmpFormatLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmpFormatSaveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polylineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interpolatedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.rectangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ellipseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multilineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rangelineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infinitelineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arrowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rulerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crosshairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.actionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageInfoToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.openDicomFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openDicomAnnotationsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.annotationInteractionModeToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.annotationInteractionModeToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripPanel1 = new System.Windows.Forms.ToolStripPanel();
            this.imageViewerToolStrip1 = new DemosCommonCode.Imaging.ImageViewerToolStrip();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.dicomAnnotatedViewerToolStrip1 = new DemosCommonCode.Imaging.DicomAnnotatedViewerToolStrip();
            this.voiLutsToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.annotationsToolStrip1 = new DicomViewerDemo.AnnotationsToolStrip();
            this.saveDicomAnnotationsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.annotationInteractionModeToolStrip.SuspendLayout();
            this.toolStripPanel1.SuspendLayout();
            this.imageViewerToolStrip1.SuspendLayout();
            this.dicomAnnotatedViewerToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.imageViewer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.annotationComboBox);
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(810, 399);
            this.splitContainer1.SplitterDistance = 571;
            this.splitContainer1.TabIndex = 3;
            // 
            // imageViewer1
            // 
            this.imageViewer1.BackColor = System.Drawing.Color.Black;
            this.imageViewer1.Clipboard = winFormsSystemClipboard1;
            this.imageViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer1.FocusPointAnchor = Vintasoft.Imaging.AnchorType.None;
            this.imageViewer1.ImageRenderingSettings = renderingSettings1;
            this.imageViewer1.ImageRotationAngle = 0;
            this.imageViewer1.IsFocusPointFixed = false;
            this.imageViewer1.IsKeyboardNavigationEnabled = true;
            this.imageViewer1.Location = new System.Drawing.Point(0, 0);
            this.imageViewer1.MasterViewer = this.thumbnailViewer1;
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.ShortcutCopy = System.Windows.Forms.Shortcut.None;
            this.imageViewer1.ShortcutCut = System.Windows.Forms.Shortcut.None;
            this.imageViewer1.ShortcutDelete = System.Windows.Forms.Shortcut.None;
            this.imageViewer1.ShortcutInsert = System.Windows.Forms.Shortcut.None;
            this.imageViewer1.ShortcutSelectAll = System.Windows.Forms.Shortcut.None;
            this.imageViewer1.Size = new System.Drawing.Size(571, 399);
            this.imageViewer1.SizeMode = Vintasoft.Imaging.UI.ImageSizeMode.BestFit;
            this.imageViewer1.TabIndex = 3;
            this.imageViewer1.Text = "imageViewer1";
            this.imageViewer1.ImageLoadingProgress += new System.EventHandler<Vintasoft.Imaging.ProgressEventArgs>(this.imageViewer1_ImageLoadingProgress);
            this.imageViewer1.FocusedIndexChanged += new System.EventHandler<Vintasoft.Imaging.UI.FocusedIndexChangedEventArgs>(this.imageViewer1_FocusedIndexChanged);
            this.imageViewer1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imageViewer1_KeyDown);
            // 
            // thumbnailViewer1
            // 
            this.thumbnailViewer1.AllowDrag = false;
            this.thumbnailViewer1.AllowDrop = true;
            this.thumbnailViewer1.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.thumbnailViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thumbnailViewer1.Clipboard = winFormsSystemClipboard1;
            this.thumbnailViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            thumbnailAppearance1.BackColor = System.Drawing.Color.Transparent;
            thumbnailAppearance1.BorderColor = System.Drawing.Color.Gray;
            thumbnailAppearance1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Dotted;
            thumbnailAppearance1.BorderWidth = 1;
            this.thumbnailViewer1.FocusedThumbnailAppearance = thumbnailAppearance1;
            this.thumbnailViewer1.GenerateOnlyVisibleThumbnails = true;
            thumbnailAppearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(235)))));
            thumbnailAppearance2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(235)))));
            thumbnailAppearance2.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            thumbnailAppearance2.BorderWidth = 2;
            this.thumbnailViewer1.HoveredThumbnailAppearance = thumbnailAppearance2;
            this.thumbnailViewer1.ImageRotationAngle = 0;
            this.thumbnailViewer1.Location = new System.Drawing.Point(0, 0);
            this.thumbnailViewer1.MultiSelect = false;
            this.thumbnailViewer1.Name = "thumbnailViewer1";
            thumbnailAppearance3.BackColor = System.Drawing.Color.Black;
            thumbnailAppearance3.BorderColor = System.Drawing.Color.Black;
            thumbnailAppearance3.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            thumbnailAppearance3.BorderWidth = 0;
            this.thumbnailViewer1.NotReadyThumbnailAppearance = thumbnailAppearance3;
            thumbnailAppearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(238)))), ((int)(((byte)(253)))));
            thumbnailAppearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(222)))), ((int)(((byte)(253)))));
            thumbnailAppearance4.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            thumbnailAppearance4.BorderWidth = 1;
            this.thumbnailViewer1.SelectedThumbnailAppearance = thumbnailAppearance4;
            this.thumbnailViewer1.ShortcutCopy = System.Windows.Forms.Shortcut.None;
            this.thumbnailViewer1.ShortcutCut = System.Windows.Forms.Shortcut.None;
            this.thumbnailViewer1.ShortcutDelete = System.Windows.Forms.Shortcut.None;
            this.thumbnailViewer1.ShortcutInsert = System.Windows.Forms.Shortcut.None;
            this.thumbnailViewer1.ShortcutSelectAll = System.Windows.Forms.Shortcut.None;
            this.thumbnailViewer1.Size = new System.Drawing.Size(810, 126);
            this.thumbnailViewer1.TabIndex = 0;
            this.thumbnailViewer1.Text = "thumbnailViewer1";
            thumbnailAppearance5.BackColor = System.Drawing.Color.Transparent;
            thumbnailAppearance5.BorderColor = System.Drawing.Color.Transparent;
            thumbnailAppearance5.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            thumbnailAppearance5.BorderWidth = 1;
            this.thumbnailViewer1.ThumbnailAppearance = thumbnailAppearance5;
            thumbnailCaption1.Padding = new Vintasoft.Imaging.PaddingF(0F, 0F, 0F, 0F);
            thumbnailCaption1.TextColor = System.Drawing.Color.Black;
            this.thumbnailViewer1.ThumbnailCaption = thumbnailCaption1;
            this.thumbnailViewer1.ThumbnailFlowStyle = Vintasoft.Imaging.UI.ThumbnailFlowStyle.SingleRow;
            this.thumbnailViewer1.ThumbnailImagePadding = new Vintasoft.Imaging.PaddingF(0F, 0F, 0F, 0F);
            this.thumbnailViewer1.ThumbnailMargin = new System.Windows.Forms.Padding(3);
            this.thumbnailViewer1.ThumbnailSize = new System.Drawing.Size(100, 100);
            // 
            // annotationComboBox
            // 
            this.annotationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.annotationComboBox.FormattingEnabled = true;
            this.annotationComboBox.Location = new System.Drawing.Point(3, 3);
            this.annotationComboBox.Name = "annotationComboBox";
            this.annotationComboBox.Size = new System.Drawing.Size(229, 21);
            this.annotationComboBox.TabIndex = 1;
            this.annotationComboBox.DropDown += new System.EventHandler(this.annotationComboBox_DropDown);
            this.annotationComboBox.SelectedIndexChanged += new System.EventHandler(this.annotationComboBox_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(3, 30);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(229, 366);
            this.propertyGrid1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.metadataToolStripMenuItem,
            this.pageToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.annotationsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(810, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDicomFilesToolStripMenuItem,
            this.saveDicomFileToImageFileToolStripMenuItem,
            this.closeDicomFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openDicomFilesToolStripMenuItem
            // 
            this.openDicomFilesToolStripMenuItem.Name = "openDicomFilesToolStripMenuItem";
            this.openDicomFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openDicomFilesToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.openDicomFilesToolStripMenuItem.Text = "Open DICOM File...";
            this.openDicomFilesToolStripMenuItem.Click += new System.EventHandler(this.openDicomFilesToolStripMenuItem_Click);
            // 
            // saveDicomFileToImageFileToolStripMenuItem
            // 
            this.saveDicomFileToImageFileToolStripMenuItem.Name = "saveDicomFileToImageFileToolStripMenuItem";
            this.saveDicomFileToImageFileToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.saveDicomFileToImageFileToolStripMenuItem.Text = "Save DICOM File To Image File...";
            this.saveDicomFileToImageFileToolStripMenuItem.Click += new System.EventHandler(this.saveDicomFileToImageFileToolStripMenuItem_Click);
            // 
            // closeDicomFileToolStripMenuItem
            // 
            this.closeDicomFileToolStripMenuItem.Name = "closeDicomFileToolStripMenuItem";
            this.closeDicomFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeDicomFileToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.closeDicomFileToolStripMenuItem.Text = "Close DICOM File";
            this.closeDicomFileToolStripMenuItem.Click += new System.EventHandler(this.closeDicomFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(239, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.deleteAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.DropDownClosed += new System.EventHandler(this.editToolStripMenuItem_DropDownClosed);
            this.editToolStripMenuItem.DropDownOpened += new System.EventHandler(this.editToolStripMenuItem_DropDownOpened);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeyDisplayString = "Del";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Delete)));
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete All";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thumbnailViewerSettingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.imageViewerSettingsToolStripMenuItem,
            this.rotateViewToolStripMenuItem,
            this.toolStripSeparator4,
            this.showOverlayImagesToolStripMenuItem,
            this.overlayColorToolStripMenuItem,
            this.toolStripSeparator6,
            this.showMetadataInViewerToolStripMenuItem,
            this.textOverlaySettingsToolStripMenuItem,
            this.toolStripSeparator8,
            this.showRulersInViewerToolStripMenuItem,
            this.rulersColorToolStripMenuItem,
            this.rulersUnitOfMeasureToolStripMenuItem,
            this.toolStripSeparator7,
            this.voiLutToolStripMenuItem,
            this.negativeImageToolStripMenuItem,
            this.voiLutMouseMoveDirectionToolStripMenuItem,
            this.toolStripSeparator23,
            this.magnifierSettingsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // thumbnailViewerSettingsToolStripMenuItem
            // 
            this.thumbnailViewerSettingsToolStripMenuItem.Name = "thumbnailViewerSettingsToolStripMenuItem";
            this.thumbnailViewerSettingsToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.thumbnailViewerSettingsToolStripMenuItem.Text = "Thumbnail Viewer Settings...";
            this.thumbnailViewerSettingsToolStripMenuItem.Click += new System.EventHandler(this.thumbnailViewerSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(236, 6);
            // 
            // imageViewerSettingsToolStripMenuItem
            // 
            this.imageViewerSettingsToolStripMenuItem.Name = "imageViewerSettingsToolStripMenuItem";
            this.imageViewerSettingsToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.imageViewerSettingsToolStripMenuItem.Text = "Image Viewer Settings...";
            this.imageViewerSettingsToolStripMenuItem.Click += new System.EventHandler(this.imageViewerSettingsToolStripMenuItem_Click);
            // 
            // rotateViewToolStripMenuItem
            // 
            this.rotateViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockwiseToolStripMenuItem,
            this.counterclockwiseToolStripMenuItem});
            this.rotateViewToolStripMenuItem.Name = "rotateViewToolStripMenuItem";
            this.rotateViewToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.rotateViewToolStripMenuItem.Text = "Rotate View";
            // 
            // clockwiseToolStripMenuItem
            // 
            this.clockwiseToolStripMenuItem.Name = "clockwiseToolStripMenuItem";
            this.clockwiseToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Ctrl+Plus";
            this.clockwiseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Oemplus)));
            this.clockwiseToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.clockwiseToolStripMenuItem.Text = "Clockwise";
            this.clockwiseToolStripMenuItem.Click += new System.EventHandler(this.clockwiseToolStripMenuItem_Click);
            // 
            // counterclockwiseToolStripMenuItem
            // 
            this.counterclockwiseToolStripMenuItem.Name = "counterclockwiseToolStripMenuItem";
            this.counterclockwiseToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Ctrl+Minus";
            this.counterclockwiseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.OemMinus)));
            this.counterclockwiseToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.counterclockwiseToolStripMenuItem.Text = "Counterclockwise";
            this.counterclockwiseToolStripMenuItem.Click += new System.EventHandler(this.counterclockwiseToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(236, 6);
            // 
            // showOverlayImagesToolStripMenuItem
            // 
            this.showOverlayImagesToolStripMenuItem.CheckOnClick = true;
            this.showOverlayImagesToolStripMenuItem.Name = "showOverlayImagesToolStripMenuItem";
            this.showOverlayImagesToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.showOverlayImagesToolStripMenuItem.Text = "Show Overlay Images";
            this.showOverlayImagesToolStripMenuItem.Click += new System.EventHandler(this.showOverlayImagesToolStripMenuItem_Click);
            // 
            // overlayColorToolStripMenuItem
            // 
            this.overlayColorToolStripMenuItem.Name = "overlayColorToolStripMenuItem";
            this.overlayColorToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.overlayColorToolStripMenuItem.Text = "Overlay Color...";
            this.overlayColorToolStripMenuItem.Click += new System.EventHandler(this.overlayColorToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(236, 6);
            // 
            // showMetadataInViewerToolStripMenuItem
            // 
            this.showMetadataInViewerToolStripMenuItem.Checked = true;
            this.showMetadataInViewerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMetadataInViewerToolStripMenuItem.Name = "showMetadataInViewerToolStripMenuItem";
            this.showMetadataInViewerToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.showMetadataInViewerToolStripMenuItem.Text = "Show Metadata In Viewer";
            this.showMetadataInViewerToolStripMenuItem.Click += new System.EventHandler(this.showMetadataOnViewerToolStripMenuItem_Click);
            // 
            // textOverlaySettingsToolStripMenuItem
            // 
            this.textOverlaySettingsToolStripMenuItem.Name = "textOverlaySettingsToolStripMenuItem";
            this.textOverlaySettingsToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.textOverlaySettingsToolStripMenuItem.Text = "Text Overlay Settings...";
            this.textOverlaySettingsToolStripMenuItem.Click += new System.EventHandler(this.textOverlaySettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(236, 6);
            // 
            // showRulersInViewerToolStripMenuItem
            // 
            this.showRulersInViewerToolStripMenuItem.Checked = true;
            this.showRulersInViewerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showRulersInViewerToolStripMenuItem.Name = "showRulersInViewerToolStripMenuItem";
            this.showRulersInViewerToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.showRulersInViewerToolStripMenuItem.Text = "Show Rulers In Viewer";
            this.showRulersInViewerToolStripMenuItem.Click += new System.EventHandler(this.showRulersOnViewerToolStripMenuItem_Click);
            // 
            // rulersColorToolStripMenuItem
            // 
            this.rulersColorToolStripMenuItem.Name = "rulersColorToolStripMenuItem";
            this.rulersColorToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.rulersColorToolStripMenuItem.Text = "Rulers Color...";
            this.rulersColorToolStripMenuItem.Click += new System.EventHandler(this.rulersColorToolStripMenuItem_Click);
            // 
            // rulersUnitOfMeasureToolStripMenuItem
            // 
            this.rulersUnitOfMeasureToolStripMenuItem.Name = "rulersUnitOfMeasureToolStripMenuItem";
            this.rulersUnitOfMeasureToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.rulersUnitOfMeasureToolStripMenuItem.Text = "Rulers Unit Of Measure";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(236, 6);
            // 
            // voiLutToolStripMenuItem
            // 
            this.voiLutToolStripMenuItem.Name = "voiLutToolStripMenuItem";
            this.voiLutToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.voiLutToolStripMenuItem.Text = "VOI LUT...";
            this.voiLutToolStripMenuItem.Click += new System.EventHandler(this.voiLutToolStripMenuItem_Click);
            // 
            // negativeImageToolStripMenuItem
            // 
            this.negativeImageToolStripMenuItem.Name = "negativeImageToolStripMenuItem";
            this.negativeImageToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.negativeImageToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.negativeImageToolStripMenuItem.Text = "Is Negative";
            this.negativeImageToolStripMenuItem.Click += new System.EventHandler(this.negativeImageToolStripMenuItem_Click);
            // 
            // voiLutMouseMoveDirectionToolStripMenuItem
            // 
            this.voiLutMouseMoveDirectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.widthHorizontalCenterVerticalToolStripMenuItem,
            this.widthVerticalCenterHorizontalToolStripMenuItem});
            this.voiLutMouseMoveDirectionToolStripMenuItem.Name = "voiLutMouseMoveDirectionToolStripMenuItem";
            this.voiLutMouseMoveDirectionToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.voiLutMouseMoveDirectionToolStripMenuItem.Text = "VOI LUT Mouse Move Direction";
            // 
            // widthHorizontalCenterVerticalToolStripMenuItem
            // 
            this.widthHorizontalCenterVerticalToolStripMenuItem.Checked = true;
            this.widthHorizontalCenterVerticalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.widthHorizontalCenterVerticalToolStripMenuItem.Name = "widthHorizontalCenterVerticalToolStripMenuItem";
            this.widthHorizontalCenterVerticalToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.widthHorizontalCenterVerticalToolStripMenuItem.Text = "Width Horizontal, Center Vertical";
            this.widthHorizontalCenterVerticalToolStripMenuItem.Click += new System.EventHandler(this.voiLutMouseMoveDirectionMenuItem_Click);
            // 
            // widthVerticalCenterHorizontalToolStripMenuItem
            // 
            this.widthVerticalCenterHorizontalToolStripMenuItem.Name = "widthVerticalCenterHorizontalToolStripMenuItem";
            this.widthVerticalCenterHorizontalToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.widthVerticalCenterHorizontalToolStripMenuItem.Text = "Width Vertical, Center Horizontal";
            this.widthVerticalCenterHorizontalToolStripMenuItem.Click += new System.EventHandler(this.voiLutMouseMoveDirectionMenuItem_Click);
            // 
            // toolStripSeparator23
            // 
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            this.toolStripSeparator23.Size = new System.Drawing.Size(236, 6);
            // 
            // magnifierSettingsToolStripMenuItem
            // 
            this.magnifierSettingsToolStripMenuItem.Name = "magnifierSettingsToolStripMenuItem";
            this.magnifierSettingsToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.magnifierSettingsToolStripMenuItem.Text = "Magnifier Settings...";
            this.magnifierSettingsToolStripMenuItem.Click += new System.EventHandler(this.magnifierSettingsToolStripMenuItem_Click);
            // 
            // metadataToolStripMenuItem
            // 
            this.metadataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMetadataToolStripMenuItem});
            this.metadataToolStripMenuItem.Name = "metadataToolStripMenuItem";
            this.metadataToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.metadataToolStripMenuItem.Text = "Metadata";
            // 
            // fileMetadataToolStripMenuItem
            // 
            this.fileMetadataToolStripMenuItem.Name = "fileMetadataToolStripMenuItem";
            this.fileMetadataToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.fileMetadataToolStripMenuItem.Text = "File Metadata...";
            this.fileMetadataToolStripMenuItem.Click += new System.EventHandler(this.metadata_fileMetadataToolStripMenuItem_Click);
            // 
            // pageToolStripMenuItem
            // 
            this.pageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.overlayImagesToolStripMenuItem});
            this.pageToolStripMenuItem.Name = "pageToolStripMenuItem";
            this.pageToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.pageToolStripMenuItem.Text = "Page";
            // 
            // overlayImagesToolStripMenuItem
            // 
            this.overlayImagesToolStripMenuItem.Name = "overlayImagesToolStripMenuItem";
            this.overlayImagesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.overlayImagesToolStripMenuItem.Text = "Overlay Images...";
            this.overlayImagesToolStripMenuItem.Click += new System.EventHandler(this.overlayImagesToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAnimationToolStripMenuItem,
            this.animationDelayToolStripMenuItem,
            this.animationRepeatToolStripMenuItem,
            this.saveAsGifFileToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.toolsToolStripMenuItem.Text = "Animation";
            // 
            // showAnimationToolStripMenuItem
            // 
            this.showAnimationToolStripMenuItem.CheckOnClick = true;
            this.showAnimationToolStripMenuItem.Name = "showAnimationToolStripMenuItem";
            this.showAnimationToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.showAnimationToolStripMenuItem.Text = "Show Animation";
            this.showAnimationToolStripMenuItem.Click += new System.EventHandler(this.showAnimationToolStripMenuItem_Click);
            // 
            // animationDelayToolStripMenuItem
            // 
            this.animationDelayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.animationDelay_valueToolStripComboBox});
            this.animationDelayToolStripMenuItem.Name = "animationDelayToolStripMenuItem";
            this.animationDelayToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.animationDelayToolStripMenuItem.Text = "Animation Delay";
            // 
            // animationDelay_valueToolStripComboBox
            // 
            this.animationDelay_valueToolStripComboBox.Items.AddRange(new object[] {
            "10",
            "100",
            "1000",
            "2000"});
            this.animationDelay_valueToolStripComboBox.Name = "animationDelay_valueToolStripComboBox";
            this.animationDelay_valueToolStripComboBox.Size = new System.Drawing.Size(121, 23);
            this.animationDelay_valueToolStripComboBox.Text = "100";
            this.animationDelay_valueToolStripComboBox.TextChanged += new System.EventHandler(this.animationDelayToolStripComboBox_TextChanged);
            // 
            // animationRepeatToolStripMenuItem
            // 
            this.animationRepeatToolStripMenuItem.Checked = true;
            this.animationRepeatToolStripMenuItem.CheckOnClick = true;
            this.animationRepeatToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.animationRepeatToolStripMenuItem.Name = "animationRepeatToolStripMenuItem";
            this.animationRepeatToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.animationRepeatToolStripMenuItem.Text = "Animation Repeat";
            this.animationRepeatToolStripMenuItem.Click += new System.EventHandler(this.animationRepeatToolStripMenuItem_Click);
            // 
            // saveAsGifFileToolStripMenuItem
            // 
            this.saveAsGifFileToolStripMenuItem.Name = "saveAsGifFileToolStripMenuItem";
            this.saveAsGifFileToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.saveAsGifFileToolStripMenuItem.Text = "Save as GIF file...";
            this.saveAsGifFileToolStripMenuItem.Click += new System.EventHandler(this.saveAsGifFileToolStripMenuItem_Click);
            // 
            // annotationsToolStripMenuItem
            // 
            this.annotationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.toolStripSeparator9,
            this.interactionModeToolStripMenuItem,
            this.toolStripSeparator10,
            this.presentationStateToolStripMenuItem,
            this.binaryFormatToolStripMenuItem,
            this.xmpFormatToolStripMenuItem,
            this.toolStripSeparator11,
            this.addToolStripMenuItem});
            this.annotationsToolStripMenuItem.Name = "annotationsToolStripMenuItem";
            this.annotationsToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.annotationsToolStripMenuItem.Text = "Annotations";
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.infoToolStripMenuItem.Text = "Info...";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(166, 6);
            // 
            // interactionModeToolStripMenuItem
            // 
            this.interactionModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interactionMode_noneToolStripMenuItem,
            this.interactionMode_viewToolStripMenuItem,
            this.interactionMode_authorToolStripMenuItem});
            this.interactionModeToolStripMenuItem.Name = "interactionModeToolStripMenuItem";
            this.interactionModeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.interactionModeToolStripMenuItem.Text = "Interaction Mode";
            // 
            // interactionMode_noneToolStripMenuItem
            // 
            this.interactionMode_noneToolStripMenuItem.Name = "interactionMode_noneToolStripMenuItem";
            this.interactionMode_noneToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.interactionMode_noneToolStripMenuItem.Text = "None";
            this.interactionMode_noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // interactionMode_viewToolStripMenuItem
            // 
            this.interactionMode_viewToolStripMenuItem.Name = "interactionMode_viewToolStripMenuItem";
            this.interactionMode_viewToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.interactionMode_viewToolStripMenuItem.Text = "View";
            this.interactionMode_viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // interactionMode_authorToolStripMenuItem
            // 
            this.interactionMode_authorToolStripMenuItem.Checked = true;
            this.interactionMode_authorToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.interactionMode_authorToolStripMenuItem.Name = "interactionMode_authorToolStripMenuItem";
            this.interactionMode_authorToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.interactionMode_authorToolStripMenuItem.Text = "Author";
            this.interactionMode_authorToolStripMenuItem.Click += new System.EventHandler(this.authorToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(166, 6);
            // 
            // presentationStateToolStripMenuItem
            // 
            this.presentationStateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.presentationStateLoadToolStripMenuItem,
            this.presentationStateInfoToolStripMenuItem,
            this.presentationStateSaveToolStripMenuItem,
            this.presentationStateSaveToToolStripMenuItem,
            this.toolStripSeparator17,
            this.presentationStateLoadAutomaticallyToolStripMenuItem});
            this.presentationStateToolStripMenuItem.Name = "presentationStateToolStripMenuItem";
            this.presentationStateToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.presentationStateToolStripMenuItem.Text = "Presentation State";
            // 
            // presentationStateLoadToolStripMenuItem
            // 
            this.presentationStateLoadToolStripMenuItem.Name = "presentationStateLoadToolStripMenuItem";
            this.presentationStateLoadToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.presentationStateLoadToolStripMenuItem.Text = "Load...";
            this.presentationStateLoadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // presentationStateInfoToolStripMenuItem
            // 
            this.presentationStateInfoToolStripMenuItem.Name = "presentationStateInfoToolStripMenuItem";
            this.presentationStateInfoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.presentationStateInfoToolStripMenuItem.Text = "Info...";
            this.presentationStateInfoToolStripMenuItem.Click += new System.EventHandler(this.presentationStateInfoToolStripMenuItem_Click);
            // 
            // presentationStateSaveToolStripMenuItem
            // 
            this.presentationStateSaveToolStripMenuItem.Name = "presentationStateSaveToolStripMenuItem";
            this.presentationStateSaveToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.presentationStateSaveToolStripMenuItem.Text = "Save";
            this.presentationStateSaveToolStripMenuItem.Click += new System.EventHandler(this.presentationStateSaveToolStripMenuItem_Click);
            // 
            // presentationStateSaveToToolStripMenuItem
            // 
            this.presentationStateSaveToToolStripMenuItem.Name = "presentationStateSaveToToolStripMenuItem";
            this.presentationStateSaveToToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.presentationStateSaveToToolStripMenuItem.Text = "Save To...";
            this.presentationStateSaveToToolStripMenuItem.Click += new System.EventHandler(this.presentationStatesSaveToToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(174, 6);
            // 
            // presentationStateLoadAutomaticallyToolStripMenuItem
            // 
            this.presentationStateLoadAutomaticallyToolStripMenuItem.CheckOnClick = true;
            this.presentationStateLoadAutomaticallyToolStripMenuItem.Name = "presentationStateLoadAutomaticallyToolStripMenuItem";
            this.presentationStateLoadAutomaticallyToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.presentationStateLoadAutomaticallyToolStripMenuItem.Text = "Load Automatically";
            // 
            // binaryFormatToolStripMenuItem
            // 
            this.binaryFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.binaryFormatLoadToolStripMenuItem,
            this.binaryFormatSaveToToolStripMenuItem});
            this.binaryFormatToolStripMenuItem.Name = "binaryFormatToolStripMenuItem";
            this.binaryFormatToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.binaryFormatToolStripMenuItem.Text = "Binary Format";
            // 
            // binaryFormatLoadToolStripMenuItem
            // 
            this.binaryFormatLoadToolStripMenuItem.Name = "binaryFormatLoadToolStripMenuItem";
            this.binaryFormatLoadToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.binaryFormatLoadToolStripMenuItem.Text = "Load...";
            this.binaryFormatLoadToolStripMenuItem.Click += new System.EventHandler(this.binaryFormatLoadToolStripMenuItem_Click);
            // 
            // binaryFormatSaveToToolStripMenuItem
            // 
            this.binaryFormatSaveToToolStripMenuItem.Name = "binaryFormatSaveToToolStripMenuItem";
            this.binaryFormatSaveToToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.binaryFormatSaveToToolStripMenuItem.Text = "Save To...";
            this.binaryFormatSaveToToolStripMenuItem.Click += new System.EventHandler(this.binaryFormatSaveToToolStripMenuItem_Click);
            // 
            // xmpFormatToolStripMenuItem
            // 
            this.xmpFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xmpFormatLoadToolStripMenuItem,
            this.xmpFormatSaveToToolStripMenuItem});
            this.xmpFormatToolStripMenuItem.Name = "xmpFormatToolStripMenuItem";
            this.xmpFormatToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.xmpFormatToolStripMenuItem.Text = "XMP Format";
            // 
            // xmpFormatLoadToolStripMenuItem
            // 
            this.xmpFormatLoadToolStripMenuItem.Name = "xmpFormatLoadToolStripMenuItem";
            this.xmpFormatLoadToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.xmpFormatLoadToolStripMenuItem.Text = "Load...";
            this.xmpFormatLoadToolStripMenuItem.Click += new System.EventHandler(this.xmpFormatLoadToolStripMenuItem_Click);
            // 
            // xmpFormatSaveToToolStripMenuItem
            // 
            this.xmpFormatSaveToToolStripMenuItem.Name = "xmpFormatSaveToToolStripMenuItem";
            this.xmpFormatSaveToToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.xmpFormatSaveToToolStripMenuItem.Text = "Save To...";
            this.xmpFormatSaveToToolStripMenuItem.Click += new System.EventHandler(this.xmpFormatSaveToToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(166, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointToolStripMenuItem,
            this.circleToolStripMenuItem,
            this.polylineToolStripMenuItem,
            this.interpolatedToolStripMenuItem,
            this.toolStripSeparator15,
            this.rectangleToolStripMenuItem,
            this.ellipseToolStripMenuItem,
            this.multilineToolStripMenuItem,
            this.rangelineToolStripMenuItem,
            this.infinitelineToolStripMenuItem,
            this.cutlineToolStripMenuItem,
            this.arrowToolStripMenuItem,
            this.axisToolStripMenuItem,
            this.rulerToolStripMenuItem,
            this.crosshairToolStripMenuItem,
            this.toolStripSeparator16,
            this.textToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // pointToolStripMenuItem
            // 
            this.pointToolStripMenuItem.Name = "pointToolStripMenuItem";
            this.pointToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.pointToolStripMenuItem.Text = "Point";
            this.pointToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // circleToolStripMenuItem
            // 
            this.circleToolStripMenuItem.Name = "circleToolStripMenuItem";
            this.circleToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.circleToolStripMenuItem.Text = "Circle";
            this.circleToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // polylineToolStripMenuItem
            // 
            this.polylineToolStripMenuItem.Name = "polylineToolStripMenuItem";
            this.polylineToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.polylineToolStripMenuItem.Text = "Polyline";
            this.polylineToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // interpolatedToolStripMenuItem
            // 
            this.interpolatedToolStripMenuItem.Name = "interpolatedToolStripMenuItem";
            this.interpolatedToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.interpolatedToolStripMenuItem.Text = "Interpolated";
            this.interpolatedToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(135, 6);
            // 
            // rectangleToolStripMenuItem
            // 
            this.rectangleToolStripMenuItem.Name = "rectangleToolStripMenuItem";
            this.rectangleToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.rectangleToolStripMenuItem.Text = "Rectangle";
            this.rectangleToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // ellipseToolStripMenuItem
            // 
            this.ellipseToolStripMenuItem.Name = "ellipseToolStripMenuItem";
            this.ellipseToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.ellipseToolStripMenuItem.Text = "Ellipse";
            this.ellipseToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // multilineToolStripMenuItem
            // 
            this.multilineToolStripMenuItem.Name = "multilineToolStripMenuItem";
            this.multilineToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.multilineToolStripMenuItem.Text = "Multiline";
            this.multilineToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // rangelineToolStripMenuItem
            // 
            this.rangelineToolStripMenuItem.Name = "rangelineToolStripMenuItem";
            this.rangelineToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.rangelineToolStripMenuItem.Text = "Rangeline";
            this.rangelineToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // infinitelineToolStripMenuItem
            // 
            this.infinitelineToolStripMenuItem.Name = "infinitelineToolStripMenuItem";
            this.infinitelineToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.infinitelineToolStripMenuItem.Text = "Infiniteline";
            this.infinitelineToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // cutlineToolStripMenuItem
            // 
            this.cutlineToolStripMenuItem.Name = "cutlineToolStripMenuItem";
            this.cutlineToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.cutlineToolStripMenuItem.Text = "Cutline";
            this.cutlineToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // arrowToolStripMenuItem
            // 
            this.arrowToolStripMenuItem.Name = "arrowToolStripMenuItem";
            this.arrowToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.arrowToolStripMenuItem.Text = "Arrow";
            this.arrowToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // axisToolStripMenuItem
            // 
            this.axisToolStripMenuItem.Name = "axisToolStripMenuItem";
            this.axisToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.axisToolStripMenuItem.Text = "Axis";
            this.axisToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // rulerToolStripMenuItem
            // 
            this.rulerToolStripMenuItem.Name = "rulerToolStripMenuItem";
            this.rulerToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.rulerToolStripMenuItem.Text = "Ruler";
            this.rulerToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // crosshairToolStripMenuItem
            // 
            this.crosshairToolStripMenuItem.Name = "crosshairToolStripMenuItem";
            this.crosshairToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.crosshairToolStripMenuItem.Text = "Crosshair";
            this.crosshairToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(135, 6);
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.textToolStripMenuItem.Text = "Text";
            this.textToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar1,
            this.actionLabel,
            this.toolStripStatusLabel1,
            this.imageInfoToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 609);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(810, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar1
            // 
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 16);
            this.progressBar1.Visible = false;
            // 
            // actionLabel
            // 
            this.actionLabel.Name = "actionLabel";
            this.actionLabel.Size = new System.Drawing.Size(73, 17);
            this.actionLabel.Text = "Action Label";
            this.actionLabel.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(795, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // imageInfoToolStripStatusLabel
            // 
            this.imageInfoToolStripStatusLabel.Name = "imageInfoToolStripStatusLabel";
            this.imageInfoToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // openDicomFileDialog
            // 
            this.openDicomFileDialog.Filter = "DICOM files|*.dcm;*.dic;*.acr|All files|*.*";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.Location = new System.Drawing.Point(0, 77);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.thumbnailViewer1);
            this.splitContainerMain.Panel2MinSize = 50;
            this.splitContainerMain.Size = new System.Drawing.Size(810, 529);
            this.splitContainerMain.SplitterDistance = 399;
            this.splitContainerMain.TabIndex = 4;
            // 
            // openDicomAnnotationsFileDialog
            // 
            this.openDicomAnnotationsFileDialog.Filter = "Presentation State File(*.pre)|*.pre|Binary Annotations(*.vsab)|*.vsab|XMP Annota" +
    "tions(*.xmp)|*.xmp|All Formats(*.pre;*.vsab;*.xmp)|*.pre;*.vsab;*.xmp";
            this.openDicomAnnotationsFileDialog.FilterIndex = 4;
            // 
            // annotationInteractionModeToolStrip
            // 
            this.annotationInteractionModeToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.annotationInteractionModeToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.annotationInteractionModeToolStripComboBox});
            this.annotationInteractionModeToolStrip.Location = new System.Drawing.Point(375, 25);
            this.annotationInteractionModeToolStrip.Name = "annotationInteractionModeToolStrip";
            this.annotationInteractionModeToolStrip.Size = new System.Drawing.Size(296, 25);
            this.annotationInteractionModeToolStrip.TabIndex = 6;
            this.annotationInteractionModeToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(161, 22);
            this.toolStripLabel1.Text = "Annotation Interaction Mode";
            // 
            // annotationInteractionModeToolStripComboBox
            // 
            this.annotationInteractionModeToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.annotationInteractionModeToolStripComboBox.Name = "annotationInteractionModeToolStripComboBox";
            this.annotationInteractionModeToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.annotationInteractionModeToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.annotationInteractionModeToolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripPanel1
            // 
            this.toolStripPanel1.Controls.Add(this.imageViewerToolStrip1);
            this.toolStripPanel1.Controls.Add(this.dicomAnnotatedViewerToolStrip1);
            this.toolStripPanel1.Controls.Add(this.annotationsToolStrip1);
            this.toolStripPanel1.Controls.Add(this.annotationInteractionModeToolStrip);
            this.toolStripPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripPanel1.Location = new System.Drawing.Point(0, 24);
            this.toolStripPanel1.Name = "toolStripPanel1";
            this.toolStripPanel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.toolStripPanel1.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripPanel1.Size = new System.Drawing.Size(810, 50);
            // 
            // imageViewerToolStrip1
            // 
            this.imageViewerToolStrip1.AssociatedZoomTrackBar = null;
            this.imageViewerToolStrip1.CanPrint = false;
            this.imageViewerToolStrip1.CanSaveFile = false;
            this.imageViewerToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.imageViewerToolStrip1.ImageViewer = null;
            this.imageViewerToolStrip1.ScanButtonEnabled = true;
            this.imageViewerToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator12});
            this.imageViewerToolStrip1.Location = new System.Drawing.Point(3, 0);
            this.imageViewerToolStrip1.Name = "imageViewerToolStrip1";
            this.imageViewerToolStrip1.PageCount = 0;
            this.imageViewerToolStrip1.PrintButtonEnabled = true;
            this.imageViewerToolStrip1.SaveButtonEnabled = true;
            this.imageViewerToolStrip1.Size = new System.Drawing.Size(316, 25);
            this.imageViewerToolStrip1.TabIndex = 2;
            this.imageViewerToolStrip1.Text = "imageViewerToolStrip1";
            this.imageViewerToolStrip1.UseImageViewerImages = true;
            this.imageViewerToolStrip1.OpenFile += new System.EventHandler(this.imageViewerToolStrip1_OpenFile);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // dicomAnnotatedViewerToolStrip1
            // 
            this.dicomAnnotatedViewerToolStrip1.DicomAnnotatedViewerTool = null;
            this.dicomAnnotatedViewerToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.dicomAnnotatedViewerToolStrip1.Enabled = false;
            this.dicomAnnotatedViewerToolStrip1.ImageViewer = this.imageViewer1;
            this.dicomAnnotatedViewerToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.voiLutsToolStripSplitButton});
            this.dicomAnnotatedViewerToolStrip1.Location = new System.Drawing.Point(319, 0);
            this.dicomAnnotatedViewerToolStrip1.MandatoryVisualTool = null;
            this.dicomAnnotatedViewerToolStrip1.Name = "dicomAnnotatedViewerToolStrip1";
            this.dicomAnnotatedViewerToolStrip1.Size = new System.Drawing.Size(67, 25);
            this.dicomAnnotatedViewerToolStrip1.TabIndex = 7;
            this.dicomAnnotatedViewerToolStrip1.VisualToolsMenuItem = null;
            // 
            // voiLutsToolStripSplitButton
            // 
            this.voiLutsToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.voiLutsToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("voiLutsToolStripSplitButton.Image")));
            this.voiLutsToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.voiLutsToolStripSplitButton.Name = "voiLutsToolStripSplitButton";
            this.voiLutsToolStripSplitButton.Size = new System.Drawing.Size(32, 22);
            this.voiLutsToolStripSplitButton.Text = "Value of interest lookup tables";
            // 
            // annotationsToolStrip1
            // 
            this.annotationsToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.annotationsToolStrip1.Location = new System.Drawing.Point(3, 25);
            this.annotationsToolStrip1.Name = "annotationsToolStrip1";
            this.annotationsToolStrip1.Size = new System.Drawing.Size(369, 25);
            this.annotationsToolStrip1.TabIndex = 5;
            this.annotationsToolStrip1.Text = "annotationsToolStrip1";
            this.annotationsToolStrip1.Viewer = null;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "mpg";
            this.saveFileDialog1.Filter = "Mpeg files|*.mpg";
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.DefaultExt = "gif";
            this.saveFileDialog2.Filter = "GIF files|*.gif";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(810, 631);
            this.Controls.Add(this.toolStripPanel1);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(375, 330);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VintaSoft Dicom Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.annotationInteractionModeToolStrip.ResumeLayout(false);
            this.annotationInteractionModeToolStrip.PerformLayout();
            this.toolStripPanel1.ResumeLayout(false);
            this.toolStripPanel1.PerformLayout();
            this.imageViewerToolStrip1.ResumeLayout(false);
            this.imageViewerToolStrip1.PerformLayout();
            this.dicomAnnotatedViewerToolStrip1.ResumeLayout(false);
            this.dicomAnnotatedViewerToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private Vintasoft.Imaging.UI.ThumbnailViewer thumbnailViewer1;
        private System.Windows.Forms.ToolStripMenuItem closeDicomFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private DemosCommonCode.Imaging.ImageViewerToolStrip imageViewerToolStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAnimationToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.OpenFileDialog openDicomFileDialog;
        private System.Windows.Forms.ToolStripMenuItem animationRepeatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thumbnailViewerSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem imageViewerSettingsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel imageInfoToolStripStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem showOverlayImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem overlayImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem overlayColorToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStripMenuItem voiLutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem animationDelayToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox animationDelay_valueToolStripComboBox;
        private System.Windows.Forms.ToolStripMenuItem showMetadataInViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem showRulersInViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rulersUnitOfMeasureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMetadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textOverlaySettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem rulersColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem annotationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interactionModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interactionMode_noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interactionMode_viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interactionMode_authorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem pointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polylineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interpolatedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem ellipseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multilineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rangelineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arrowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectangleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        private AnnotationsToolStrip annotationsToolStrip1;
        private System.Windows.Forms.OpenFileDialog openDicomAnnotationsFileDialog;
        private System.Windows.Forms.ToolStrip annotationInteractionModeToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox annotationInteractionModeToolStripComboBox;
        private System.Windows.Forms.ToolStripPanel toolStripPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox annotationComboBox;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private Vintasoft.Imaging.UI.ImageViewer imageViewer1;
        private System.Windows.Forms.SaveFileDialog saveDicomAnnotationsFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel actionLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem presentationStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem presentationStateLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem presentationStateSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem presentationStateSaveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem presentationStateLoadAutomaticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmpFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryFormatLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryFormatSaveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmpFormatLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmpFormatSaveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem presentationStateInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDicomFileToImageFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutlineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infinitelineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rulerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crosshairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem voiLutMouseMoveDirectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem widthHorizontalCenterVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem widthVerticalCenterHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDicomFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem negativeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private DemosCommonCode.Imaging.DicomAnnotatedViewerToolStrip dicomAnnotatedViewerToolStrip1;
        private System.Windows.Forms.ToolStripSplitButton voiLutsToolStripSplitButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripMenuItem magnifierSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clockwiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem counterclockwiseToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem saveAsGifFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
    }
}