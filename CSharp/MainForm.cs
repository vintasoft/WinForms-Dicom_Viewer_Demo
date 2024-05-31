using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;

using Vintasoft.Imaging;
using Vintasoft.Imaging.Annotation;
using Vintasoft.Imaging.Annotation.Dicom;
using Vintasoft.Imaging.Annotation.Dicom.UI.VisualTools;
using Vintasoft.Imaging.Annotation.Formatters;
using Vintasoft.Imaging.Annotation.UI;
using Vintasoft.Imaging.Annotation.UI.VisualTools;
using Vintasoft.Imaging.Codecs;
using Vintasoft.Imaging.Codecs.Decoders;
using Vintasoft.Imaging.Codecs.Encoders;
using Vintasoft.Imaging.Codecs.ImageFiles.Dicom;
using Vintasoft.Imaging.Dicom.UI.VisualTools;
using Vintasoft.Imaging.ImageColors;
using Vintasoft.Imaging.Metadata;
using Vintasoft.Imaging.UI;
using Vintasoft.Imaging.UI.VisualTools;
using Vintasoft.Imaging.UIActions;

using DemosCommonCode;
using DemosCommonCode.Imaging;
using DemosCommonCode.Imaging.Codecs;
using DemosCommonCode.Imaging.Codecs.Dialogs;
using DemosCommonCode.Spelling;
using DemosCommonCode.Dicom;

namespace DicomViewerDemo
{
    /// <summary>
    /// Main form of DICOM viewer demo.
    /// </summary>
    public partial class MainForm : Form
    {

        #region Constants

        /// <summary>
        /// The name of text overlay collection owner.
        /// </summary>
        const string OVERLAY_OWNER_NAME = "Dicom Viewer";

        #endregion



        #region Fields

        /// <summary>
        /// Template of the application title.
        /// </summary>
        string _titlePrefix = "VintaSoft DICOM Viewer Demo v" + ImagingGlobalSettings.ProductVersion + " - {0}";

        /// <summary>
        /// DICOM annotated viewer tool.
        /// </summary>
        DicomAnnotatedViewerTool _dicomViewerTool;

        /// <summary>
        /// The previous interaction mode in DICOM viewer tool.
        /// </summary>
        DicomAnnotatedViewerToolInteractionMode _previousDicomViewerToolInteractionMode;

        /// <summary>
        /// The previous interaction mode in DICOM annotation tool.
        /// </summary>
        AnnotationInteractionMode _previousDicomAnnotationToolInteractionMode;

        /// <summary>
        /// Current rulers unit menu item.
        /// </summary>
        ToolStripMenuItem _currentRulersUnitOfMeasureMenuItem = null;

        /// <summary>
        /// A value indicating whether the application form is closing.
        /// </summary> 
        bool _isFormClosing = false;

        /// <summary>
        /// The image encoder for saving of images.
        /// </summary>
        EncoderBase _imageEncoder;


        /// <summary>
        /// Dictionary: the tool strip menu item => rulers units of measure.
        /// </summary>
        Dictionary<ToolStripMenuItem, UnitOfMeasure> _toolStripMenuItemToRulersUnitOfMeasure =
            new Dictionary<ToolStripMenuItem, UnitOfMeasure>();

        /// <summary>
        /// Dictionary: the tool strip item => VOI LUT.
        /// </summary>
        Dictionary<ToolStripItem, DicomImageVoiLookupTable> _toolStripItemToVoiLut =
            new Dictionary<ToolStripItem, DicomImageVoiLookupTable>();

        /// <summary>
        /// A value indicating whether the visual tool of <see cref="ImageViewerToolStrip"/> is changing.
        /// </summary>
        bool _isVisualToolChanging = false;

        /// <summary>
        /// Current application window state.
        /// </summary>
        FormWindowState _windowState;


        #region DICOM file

        /// <summary>
        /// Controller of files in current DICOM series.
        /// </summary>
        DicomSeriesController _dicomSeriesController = new DicomSeriesController();

        /// <summary>
        /// DICOM file without images.
        /// </summary>
        DicomFile _dicomFileWithoutImages = null;

        /// <summary>
        /// Decoding setting of DICOM frame.
        /// </summary>
        DicomDecodingSettings _dicomFrameDecodingSettings = new DicomDecodingSettings(false);

        #endregion


        #region VOI LUT

        /// <summary>
        /// Default VOI LUT menu item.
        /// </summary>
        ToolStripMenuItem _defaultVoiLutToolStripMenuItem = null;

        /// <summary>
        /// Current VOI LUT menu item.
        /// </summary>
        ToolStripMenuItem _currentVoiLutMenuItem = null;

        /// <summary>
        /// A form that allows to specify VOI LUT with custom parameters.
        /// </summary>
        VoiLutParamsForm _voiLutParamsForm = null;

        #endregion


        #region Animation

        /// <summary>
        /// A value indicating whether the animation is cycled.
        /// </summary>
        bool _isAnimationCycled = true;

        /// <summary>
        /// Animation delay in milliseconds.
        /// </summary>
        int _animationDelay = 100;

        /// <summary>
        /// Animation thread.
        /// </summary>
        Thread _animationThread = null;

        /// <summary>
        /// Index of current animated frame.
        /// </summary>
        int _currentAnimatedFrameIndex = 0;

        /// <summary>
        /// A value indicating whether the focused index is changing.
        /// </summary>
        bool _isFocusedIndexChanging = false;

        #endregion


        #region Presentation State Files

        /// <summary>
        /// The extensions of the DICOM presentation state files.
        /// </summary>
        string[] _presentationStateFileExtensions = null;

        #endregion


        #region Annotations

        /// <summary>
        /// A value indicating whether transforming of annotation is started.
        /// </summary>
        bool _isAnnotationTransforming = false;

        /// <summary>
        /// A value indicating whether the annotation property is changing.
        /// </summary>
        bool _isAnnotationPropertyChanging = false;

        /// <summary>
        /// A value indicating whether the annotations are loaded for the current frame.
        /// </summary>
        bool _isAnnotationsLoadedForCurrentFrame = false;

        #endregion

        #endregion



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // register the evaluation license for VintaSoft Imaging .NET SDK
            Vintasoft.Imaging.ImagingGlobalSettings.Register("REG_USER", "REG_EMAIL", "EXPIRATION_DATE", "REG_CODE");

            InitializeComponent();

            MoveDicomCodecToFirstPosition();

            Jpeg2000AssemblyLoader.Load();

            AnnotationTypeEditorRegistrator.Register();

            _presentationStateFileExtensions = new string[] { ".DCM", ".DIC", ".ACR", ".PRE", "" };

            // init ImageViewerToolStrip
            InitImageViewerToolStrip();

            MeasurementVisualToolActionFactory.CreateActions(dicomAnnotatedViewerToolStrip1);
            dicomAnnotatedViewerToolStrip1.Items.Remove(voiLutsToolStripSplitButton);
            dicomAnnotatedViewerToolStrip1.Items.Add(voiLutsToolStripSplitButton);

            NoneAction noneAction = dicomAnnotatedViewerToolStrip1.FindAction<NoneAction>();
            noneAction.Activated += new EventHandler(noneAction_Activated);
            noneAction.Deactivated += new EventHandler(noneAction_Deactivated);

            ImageMeasureToolAction imageMeasureToolAction =
                dicomAnnotatedViewerToolStrip1.FindAction<ImageMeasureToolAction>();
            imageMeasureToolAction.Activated += new EventHandler(imageMeasureToolAction_Activated);

            MagnifierTool magnifierTool = new MagnifierTool();
            magnifierTool.ShowVisualTools = false;
            // create action, which allows to magnify of image region in image viewer
            MagnifierToolAction magnifierToolAction = new MagnifierToolAction(
                magnifierTool,
                DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_MAGNIFIER_TOOL,
                DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_MAGNIFIER,
                DemosResourcesManager.GetResourceAsBitmap("DemosCommonCode.Imaging.VisualToolsToolStrip.VisualTools.ZoomVisualTools.Resources.MagnifierTool.png"));

            // create action, which allows to pan an image in image viewer
            VisualToolAction panToolAction = new VisualToolAction(
                new PanTool(),
                DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PAN_TOOL,
                DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PAN,
                DemosResourcesManager.GetResourceAsBitmap(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DEMOSCOMMONCODEIMAGINGVISUALTOOLSTOOLSTRIPVISUALTOOLSZOOMVISUALTOOLSRESOURCESPANTOOLPNG));


            _dicomViewerTool = new DicomAnnotatedViewerTool(
                new DicomViewerTool(),
                new DicomAnnotationTool(),
                (Vintasoft.Imaging.Annotation.Measurements.ImageMeasureTool)imageMeasureToolAction.VisualTool);

            // add visual tools to tool strip
            dicomAnnotatedViewerToolStrip1.DicomAnnotatedViewerTool = _dicomViewerTool;
            dicomAnnotatedViewerToolStrip1.AddVisualToolAction(magnifierToolAction);
            dicomAnnotatedViewerToolStrip1.AddVisualToolAction(panToolAction);
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool = _dicomViewerTool;

            magnifierToolAction.Activated += new EventHandler(magnifierToolAction_Activated);
            panToolAction.Activated += PanToolAction_Activated;

            _dicomViewerTool.DicomViewerTool.TextOverlay.Add(
                new CompressionInfoTextOverlay(AnchorType.Top | AnchorType.Left));

            DemosTools.SetTestFilesFolder(openDicomFileDialog);

            CompositeVisualTool compositeTool = new CompositeVisualTool(_dicomViewerTool, magnifierTool);
            compositeTool.ActiveTool = _dicomViewerTool;
            imageViewer1.VisualTool = compositeTool;
            annotationsToolStrip1.Viewer = imageViewer1;

            // init DICOM annotation tool
            InitDicomAnnotationTool();

            _dicomViewerTool.DicomViewerTool.DicomImageVoiLutChanged +=
                new EventHandler<VoiLutChangedEventArgs>(dicomViewerTool_DicomImageVoiLutChanged);

            thumbnailViewer1.ImageDecodingSettings = _dicomFrameDecodingSettings;

            SubscribeToImageCollectionEvents(imageViewer1.Images);

            // init unit of measures for rulers
            InitUnitOfMeasuresForRulers();

            _defaultVoiLutToolStripMenuItem = new ToolStripMenuItem(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DEFAULT_VOI_LUT);
            _defaultVoiLutToolStripMenuItem.Click += new EventHandler(voiLutMenuItem_Click);

            this.Text = string.Format(_titlePrefix, DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_UNTITLED);

            // update the UI
            UpdateUI();
        }

        #endregion



        #region Properties

        bool _isDicomFileOpening = false;
        /// <summary>
        /// Gets or sets a value indicating whether the DICOM file is opening.
        /// </summary>    
        bool IsDicomFileOpening
        {
            get
            {
                return _isDicomFileOpening;
            }
            set
            {
                _isDicomFileOpening = value;

                if (InvokeRequired)
                    InvokeUpdateUI();
                else
                    UpdateUI();
            }
        }

        bool _isFileSaving = false;
        /// <summary>
        /// Gets or sets a value indicating whether file is saving.
        /// </summary>
        bool IsFileSaving
        {
            get
            {
                return _isFileSaving;
            }
            set
            {
                _isFileSaving = value;

                if (InvokeRequired)
                    InvokeUpdateUI();
                else
                    UpdateUI();
            }
        }

        bool _isAnimationStarted = false;
        /// <summary>
        /// Gets or sets a value indicating whether the animation is started.
        /// </summary>
        bool IsAnimationStarted
        {
            get
            {
                return _isAnimationStarted;
            }
            set
            {
                if (IsAnimationStarted == value)
                    return;

                if (value)
                    StartAnimation();
                else
                    StopAnimation();

                imageViewerToolStrip1.IsNavigateEnabled = !value;
                UpdateUI();
            }
        }

        /// <summary>
        /// Gets the DICOM file of focused image.
        /// </summary>    
        DicomFile DicomFile
        {
            get
            {
                VintasoftImage image = imageViewer1.Image;
                if (image != null)
                    return _dicomSeriesController.GetDicomFile(image);

                return _dicomFileWithoutImages;
            }
        }

        /// <summary>
        /// Gets the DICOM frame of focused image.
        /// </summary>
        DicomFrame DicomFrame
        {
            get
            {
                VintasoftImage image = imageViewer1.Image;

                return DicomFrame.GetFrameAssociatedWithImage(image);
            }
        }

        /// <summary>
        /// Gets the DICOM presentation state file of <paramref name="DicomFile"/>.
        /// </summary>
        DicomFile PresentationStateFile
        {
            get
            {
                if (DicomFile == null)
                    return null;

                return PresentationStateFileController.GetPresentationStateFile(DicomFile);
            }
        }

        #endregion



        #region Methods

        #region PROTECTED

        /// <summary>
        /// Processes a command key.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />,
        /// passed by reference, that represents the window message to process.</param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values
        /// that represents the key to process.</param>
        /// <returns>
        /// <b>true</b> if the character was processed by the control; otherwise, <b>false</b>.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Shift | Keys.Control | Keys.Add))
            {
                RotateViewClockwise();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.Control | Keys.Subtract))
            {
                RotateViewCounterClockwise();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion


        #region PRIVATE

        #region UI

        #region Main Form

        /// <summary>
        /// Handles the FormClosing event of MainForm object.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsAnimationStarted = false;
            _isFormClosing = true;

            // close the previously opened DICOM files
            ClosePreviouslyOpenedFile();

            DicomAnnotationTool annotationTool = _dicomViewerTool.DicomAnnotationTool;
            if (annotationTool.SpellChecker != null)
            {
                SpellCheckTools.DisposeSpellCheckManagerAndEngines(annotationTool.SpellChecker);
                annotationTool.SpellChecker = null;
            }
        }

        #endregion


        #region 'File' menu

        /// <summary>
        /// Handles the Click event of openDicomFilesToolStripMenuItem object.
        /// </summary>
        private void openDicomFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDicomFile();
        }

        /// <summary>
        /// Handles the Click event of saveDicomFileToImageFileToolStripMenuItem object.
        /// </summary>
        private void saveDicomFileToImageFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageCollection images = imageViewer1.Images;
            bool useMultipageEncoderOnly = images.Count > 1;

            CodecsFileFilters.SetSaveFileDialogFilter(saveFileDialog1, useMultipageEncoderOnly, true);
            // if file is selected in "Save file" dialog
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DicomAnnotationTool annotationTool = _dicomViewerTool.DicomAnnotationTool;
                // if there are annotation on images
                if (AreThereAnnotationsOnImages(images, annotationTool))
                {
                    DialogResult dialogResult = MessageBox.Show(
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DICOM_ANNOTATIONS_CANNOT_BE_CONVERTED_INTO_VINTASOFT_ANNOTATIONS_BUT_ANNOTATIONS_CAN_BE_BURNED_ON_IMAGERN +
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_BURN_ANNOTATIONS_ON_IMAGESRN +
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESS_YES_IF_YOU_WANT_SAVE_IMAGES_WITH_BURNED_ANNOTATIONSRN +
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESS_NO_IF_YOU_WANT_SAVE_IMAGES_WITHOUT_ANNOTATIONSRN +
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESS_CANCEL_TO_CANCEL_SAVING,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_ANNOTATIONS,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    if (dialogResult == DialogResult.Yes)
                    {
                        // get images with burned annotations
                        images = GetImagesWithBurnedAnnotations(images, annotationTool);
                        // subscribe to the events of image collection
                        SubscribeToImageCollectionEvents(images);
                    }
                }

                _imageEncoder = null;
                try
                {
                    IsFileSaving = true;

                    string saveFilename = Path.GetFullPath(saveFileDialog1.FileName);
                    if (useMultipageEncoderOnly)
                        _imageEncoder = GetMultipageEncoder(saveFilename);
                    else
                        _imageEncoder = GetEncoder(saveFilename);

                    // if encoder exists
                    if (_imageEncoder != null)
                    {
                        progressBar1.Maximum = 100;
                        progressBar1.Minimum = 0;
                        progressBar1.Value = 0;
                        progressBar1.Visible = true;

                        // save images to a file
                        images.SaveAsync(saveFilename, _imageEncoder);
                    }
                }
                catch (Exception ex)
                {
                    DemosTools.ShowErrorMessage(ex);
                    progressBar1.Visible = false;
                    _imageEncoder.Dispose();

                    IsFileSaving = false;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of closeDicomFileToolStripMenuItem object.
        /// </summary>
        private void closeDicomFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // close DICOM file
            CloseDicomSeries();
        }

        /// <summary>
        /// Handles the Click event of exitToolStripMenuItem object.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion


        #region 'Edit' menu

        /// <summary>
        /// Handles the DropDownOpened event of editToolStripMenuItem object.
        /// </summary>
        private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            UpdateEditMenuItems();
        }

        /// <summary>
        /// Handles the DropDownClosed event of editToolStripMenuItem object.
        /// </summary>
        private void editToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            EnableEditMenuItems();
        }

        /// <summary>
        /// Handles the Click event of cutToolStripMenuItem object.
        /// </summary>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteUiAction<CutItemUIAction>();
        }

        /// <summary>
        /// Handles the Click event of copyToolStripMenuItem object.
        /// </summary>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteUiAction<CopyItemUIAction>();
        }

        /// <summary>
        /// Handles the Click event of pasteToolStripMenuItem object.
        /// </summary>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteUiAction<PasteItemUIAction>();
        }

        /// <summary>
        /// Handles the Click event of deleteToolStripMenuItem object.
        /// </summary>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteUiAction<DeleteItemUIAction>();
        }

        /// <summary>
        /// Handles the Click event of deleteAllToolStripMenuItem object.
        /// </summary>
        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteUiAction<DeleteAllItemsUIAction>();
        }

        #endregion


        #region 'View' menu

        #region Thumbnail viewer settings

        /// <summary>
        /// Handles the Click event of thumbnailViewerSettingsToolStripMenuItem object.
        /// </summary>
        private void thumbnailViewerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ThumbnailViewerSettingsForm dlg = new ThumbnailViewerSettingsForm(thumbnailViewer1))
            {
                dlg.ShowDialog();
            }
        }

        #endregion


        #region Image viewer settings

        /// <summary>
        /// Handles the Click event of imageViewerSettingsToolStripMenuItem object.
        /// </summary>
        private void imageViewerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ImageViewerSettingsForm dlg = new ImageViewerSettingsForm(imageViewer1))
            {
                dlg.CanEditMultipageSettings = false;
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of clockwiseToolStripMenuItem object.
        /// </summary>
        private void clockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateViewClockwise();
        }

        /// <summary>
        /// Handles the Click event of counterclockwiseToolStripMenuItem object.
        /// </summary>
        private void counterclockwiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateViewCounterClockwise();
        }

        /// <summary>
        /// Handles the CheckedChanged event of fullScreenToolStripMenuItem object.
        /// </summary>
        private void fullScreenToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (fullScreenToolStripMenuItem.Checked)
            {
                // enable full screen mode
                splitContainer1.Panel2Collapsed = true;
                toolStripPanel1.Visible = false;
                menuStrip1.Visible = false;
                thumbnailViewer1.BackColor = Color.Black;
                statusStrip1.Visible = false;

                TopMost = true;
                _windowState = WindowState;
                FormBorderStyle = FormBorderStyle.None;
                if (WindowState == FormWindowState.Maximized)
                    WindowState = FormWindowState.Normal;

                WindowState = FormWindowState.Maximized;
            }
            else
            {
                // disable full screen mode
                splitContainer1.Panel2Collapsed = false;
                toolStripPanel1.Visible = true;
                menuStrip1.Visible = true;
                thumbnailViewer1.BackColor = SystemColors.Control;
                statusStrip1.Visible = true;

                TopMost = false;
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                if (WindowState != _windowState)
                    WindowState = _windowState;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of showScrollbarsToolStripMenuItem object.
        /// </summary>
        private void showScrollbarsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            // show/hide scrollbars in image viewer
            imageViewer1.AutoScroll = showScrollbarsToolStripMenuItem.Checked;
        }

        #endregion


        #region Overlay images

        /// <summary>
        /// Handles the Click event of showOverlayImagesToolStripMenuItem object.
        /// </summary>
        private void showOverlayImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change decoding settings
            _dicomFrameDecodingSettings.ShowOverlayImages = showOverlayImagesToolStripMenuItem.Checked;

            // invalidates images and visual tool
            _dicomViewerTool.DicomViewerTool.Refresh();
        }

        /// <summary>
        /// Handles the Click event of overlayColorToolStripMenuItem object.
        /// </summary>
        private void overlayColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get filling color from decoding settings
            Rgb24Color rgb24Color = _dicomFrameDecodingSettings.OverlayColor;
            // init dialog
            colorDialog1.Color = Color.FromArgb(rgb24Color.Red, rgb24Color.Green, rgb24Color.Blue);
            // show dialog
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // update filling color in decoding settings
                _dicomFrameDecodingSettings.OverlayColor = new Rgb24Color(colorDialog1.Color);

                _dicomViewerTool.DicomViewerTool.Refresh();
            }
        }

        #endregion


        #region Metadata

        /// <summary>
        /// Handles the Click event of showMetadataOnViewerToolStripMenuItem object.
        /// </summary>
        private void showMetadataOnViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showMetadataInViewerToolStripMenuItem.Checked ^= true;
            _dicomViewerTool.DicomViewerTool.IsTextOverlayVisible = showMetadataInViewerToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the Click event of textOverlaySettingsToolStripMenuItem object.
        /// </summary>
        private void textOverlaySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (DicomOverlaySettingEditorForm dlg = new DicomOverlaySettingEditorForm(OVERLAY_OWNER_NAME, _dicomViewerTool.DicomViewerTool))
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                // show dialog
                dlg.ShowDialog(this);

                // set text overlay for DICOM viewer tool
                DicomOverlaySettingEditorForm.SetTextOverlay(OVERLAY_OWNER_NAME, _dicomViewerTool.DicomViewerTool);
                // refresh the DICOM viewer tool
                _dicomViewerTool.DicomViewerTool.Refresh();
            }
        }

        #endregion


        #region Rulers

        /// <summary>
        /// Handles the Click event of showRulersOnViewerToolStripMenuItem object.
        /// </summary>
        private void showRulersOnViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showRulersInViewerToolStripMenuItem.Checked ^= true;
            _dicomViewerTool.DicomViewerTool.ShowRulers = showRulersInViewerToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the Click event of rulersColorToolStripMenuItem object.
        /// </summary>
        private void rulersColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // init dialog
            colorDialog1.Color = _dicomViewerTool.DicomViewerTool.VerticalImageRuler.RulerPen.Color;
            // show dialog
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // update rulers
                _dicomViewerTool.DicomViewerTool.VerticalImageRuler.RulerPen.Color = colorDialog1.Color;
                _dicomViewerTool.DicomViewerTool.HorizontalImageRuler.RulerPen.Color = colorDialog1.Color;

                // refresh DICOM viewer tool
                _dicomViewerTool.DicomViewerTool.Refresh();
            }
        }

        /// <summary>
        /// Handles the Click event of rulersUnitOfMeasureToolStripMenuItem object.
        /// </summary>
        private void rulersUnitOfMeasureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentRulersUnitOfMeasureMenuItem.Checked = false;
            _currentRulersUnitOfMeasureMenuItem = (ToolStripMenuItem)sender;
            _dicomViewerTool.DicomViewerTool.RulersUnitOfMeasure =
                _toolStripMenuItemToRulersUnitOfMeasure[_currentRulersUnitOfMeasureMenuItem];
            _currentRulersUnitOfMeasureMenuItem.Checked = true;
        }

        #endregion


        #region VOI LUT

        /// <summary>
        /// Handles the Click event of voiLutToolStripMenuItem object.
        /// </summary>
        private void voiLutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCustomVoiLutForm();
        }

        /// <summary>
        /// Handles the FormClosing event of voiLutParamsForm object.
        /// </summary>
        private void voiLutParamsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if application is not closing
            if (!_isFormClosing)
            {
                DicomFrameMetadata metadata = GetFocusedImageMetadata();

                // if image viewer contains image 
                if (metadata != null)
                {
                    if (metadata.ColorSpace == DicomImageColorSpaceType.Monochrome1 ||
                        metadata.ColorSpace == DicomImageColorSpaceType.Monochrome2)
                    {
                        voiLutsToolStripSplitButton.Visible = true;
                    }
                    else
                    {
                        voiLutsToolStripSplitButton.Visible = false;
                    }
                }

                voiLutToolStripMenuItem.Checked = false;
                _voiLutParamsForm = null;
            }
        }

        /// <summary>
        /// Handles the Click event of negativeImageToolStripMenuItem object.
        /// </summary>
        private void negativeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            negativeImageToolStripMenuItem.Checked ^= true;
            _dicomViewerTool.DicomViewerTool.IsImageNegative = negativeImageToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the DicomImageVoiLutChanged event of dicomViewerTool object.
        /// </summary>
        private void dicomViewerTool_DicomImageVoiLutChanged(object sender, VoiLutChangedEventArgs e)
        {
            // if there is selected VOI LUT menu item
            if (_currentVoiLutMenuItem != null)
            {
                // clear the selected VOI LUT menu item

                _currentVoiLutMenuItem.Checked = false;
                _currentVoiLutMenuItem = null;
            }

            ToolStripItemCollection items = voiLutsToolStripSplitButton.DropDownItems;
            // for each VOI LUT menu item
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripMenuItem)
                {
                    // VOI LUT menu item
                    ToolStripMenuItem toolStripItem = (ToolStripMenuItem)item;

                    if (_toolStripItemToVoiLut.ContainsKey(toolStripItem))
                    {
                        // VOI LUT table, which is associated with VOI LUT menu item
                        DicomImageVoiLookupTable window = _toolStripItemToVoiLut[toolStripItem];

                        // if VOI LUT menu item parameters are equal to the parameters of new VOI LUT
                        if (window.WindowCenter == e.WindowCenter &&
                            window.WindowWidth == e.WindowWidth)
                        {
                            // set the VOI LUT menu item as selected

                            _currentVoiLutMenuItem = toolStripItem;
                            _currentVoiLutMenuItem.Checked = true;
                            break;
                        }
                    }
                }
            }

            // the default VOI LUT
            DicomImageVoiLookupTable defaultVoiLut =
                _dicomViewerTool.DicomViewerTool.DefaultDicomImageVoiLut;
            // if the default VOI LUT is equal to new VOI LUT
            if (defaultVoiLut.WindowCenter == e.WindowCenter &&
                defaultVoiLut.WindowWidth == e.WindowWidth)
            {
                // specify that DICOM viewer tool must use VOI LUT from DICOM image metadata for DICOM image
                _dicomViewerTool.DicomViewerTool.AlwaysLoadVoiLutFromMetadataOfDicomFrame = true;
            }
            else
            {
                // specify that DICOM viewer tool must use the same VOI LUT for all DICOM images
                _dicomViewerTool.DicomViewerTool.AlwaysLoadVoiLutFromMetadataOfDicomFrame = false;
            }
        }

        /// <summary>
        /// Handles the Click event of widthHorizontalInvertedCenterVerticalToolStripMenuItem object.
        /// </summary>
        private void widthHorizontalInvertedCenterVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            widthHorizontalInvertedCenterVerticalToolStripMenuItem.Checked = true;
            widthHorizontalCenterVerticalToolStripMenuItem.Checked = false;
            widthVerticalCenterHorizontalToolStripMenuItem.Checked = false;

            _dicomViewerTool.DicomViewerTool.DicomImageVoiLutCenterDirection = DicomInteractionDirection.BottomToTop;
            _dicomViewerTool.DicomViewerTool.DicomImageVoiLutWidthDirection = DicomInteractionDirection.LeftToRight;
        }

        /// <summary>
        /// Handles the Click event of widthHorizontalCenterVerticalToolStripMenuItem object.
        /// </summary>
        private void widthHorizontalCenterVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            widthHorizontalInvertedCenterVerticalToolStripMenuItem.Checked = false;
            widthHorizontalCenterVerticalToolStripMenuItem.Checked = true;
            widthVerticalCenterHorizontalToolStripMenuItem.Checked = false;

            _dicomViewerTool.DicomViewerTool.DicomImageVoiLutCenterDirection = DicomInteractionDirection.BottomToTop;
            _dicomViewerTool.DicomViewerTool.DicomImageVoiLutWidthDirection = DicomInteractionDirection.RightToLeft;
        }

        /// <summary>
        /// Handles the Click event of widthVerticalCenterHorizontalToolStripMenuItem object.
        /// </summary>
        private void widthVerticalCenterHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            widthHorizontalInvertedCenterVerticalToolStripMenuItem.Checked = false;
            widthHorizontalCenterVerticalToolStripMenuItem.Checked = false;
            widthVerticalCenterHorizontalToolStripMenuItem.Checked = true;

            _dicomViewerTool.DicomViewerTool.DicomImageVoiLutCenterDirection = DicomInteractionDirection.RightToLeft;
            _dicomViewerTool.DicomViewerTool.DicomImageVoiLutWidthDirection = DicomInteractionDirection.BottomToTop;
        }

        #endregion


        #region Magnifier

        /// <summary>
        /// Handles the Click event of magnifierSettingsToolStripMenuItem object.
        /// </summary>
        private void magnifierSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MagnifierToolAction magnifierToolAction = dicomAnnotatedViewerToolStrip1.FindAction<MagnifierToolAction>();

            if (magnifierToolAction != null)
                magnifierToolAction.ShowVisualToolSettings();
        }

        #endregion

        #endregion


        #region 'Metadata' menu

        /// <summary>
        /// Handles the fileMetadataToolStripMenuItem_Click event of metadata object.
        /// </summary>
        private void metadata_fileMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCurrentFileMetadata();
        }

        #endregion


        #region 'Page' menu

        /// <summary>
        /// Handles the Click event of overlayImagesToolStripMenuItem object.
        /// </summary>
        private void overlayImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageViewer1.Image != null)
            {
                using (OverlayImagesViewer dialog = new OverlayImagesViewer(imageViewer1.Image))
                {
                    dialog.ShowDialog();
                }
            }
        }

        #endregion


        #region 'Animation' menu

        /// <summary>
        /// Handles the Click event of showAnimationToolStripMenuItem object.
        /// </summary>
        private void showAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsAnimationStarted = showAnimationToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the Click event of animationRepeatToolStripMenuItem object.
        /// </summary>
        private void animationRepeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isAnimationCycled = animationRepeatToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the TextChanged event of animationDelayToolStripComboBox object.
        /// </summary>
        private void animationDelayToolStripComboBox_TextChanged(object sender, EventArgs e)
        {
            int delay;
            if (int.TryParse(animationDelay_valueToolStripComboBox.Text, out delay))
                _animationDelay = Math.Max(1, delay);
            else
                animationDelay_valueToolStripComboBox.Text = _animationDelay.ToString();
        }

        /// <summary>
        /// Handles the Click event of saveAsGifFileToolStripMenuItem object.
        /// </summary>
        private void saveAsGifFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageCollection images = imageViewer1.Images;
            try
            {
                // if file is selected in "Save file" dialog
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    DicomAnnotationTool annotationTool = _dicomViewerTool.DicomAnnotationTool;
                    // if there are annotation on images
                    if (AreThereAnnotationsOnImages(images, annotationTool))
                    {
                        DialogResult dialogResult = MessageBox.Show(
                            DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DICOM_ANNOTATIONS_CANNOT_BE_CONVERTED_INTO_VINTASOFT_ANNOTATIONS_BUT_ANNOTATIONS_CAN_BE_BURNED_ON_IMAGERN_ALT1 +
                            DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_BURN_ANNOTATIONS_ON_IMAGESRN_ALT1 +
                            DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESS_YES_IF_YOU_WANT_SAVE_IMAGES_WITH_BURNED_ANNOTATIONSRN_ALT1 +
                            DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESS_NO_IF_YOU_WANT_SAVE_IMAGES_WITHOUT_ANNOTATIONSRN_ALT1 +
                            DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESS_CANCEL_TO_CANCEL_SAVING_ALT1,
                            DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_ANNOTATIONS_ALT1,
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Warning);

                        if (dialogResult == DialogResult.Cancel)
                        {
                            return;
                        }
                        if (dialogResult == DialogResult.Yes)
                        {
                            // get images with burned annotations
                            images = GetImagesWithBurnedAnnotations(images, annotationTool);
                            // subscribe to the events of image collection
                            SubscribeToImageCollectionEvents(images);
                        }
                    }

                    try
                    {
                        // specify that image saving is started
                        IsFileSaving = true;

                        // get filename from Save dialog
                        string saveFilename = saveFileDialog2.FileName;
                        // if filename does not have ".GIF" extension
                        if (Path.GetExtension(saveFilename).ToUpperInvariant() != ".GIF")
                        {
                            // change file extension to ".gif"
                            saveFilename = Path.Combine(Path.GetDirectoryName(saveFilename), Path.GetFileNameWithoutExtension(saveFilename) + ".gif");
                        }

                        // create GIF encoder
                        using (GifEncoder gifEncoder = new GifEncoder())
                        {
                            // get the animation delay
                            int animationDelay = int.Parse(animationDelay_valueToolStripComboBox.Text);
                            // set animation delay in GIF encoder
                            gifEncoder.Settings.AnimationDelay = Math.Max(1, animationDelay / 10);
                            // set infinite animation flag in GIF encoder
                            gifEncoder.Settings.InfiniteAnimation = animationRepeatToolStripMenuItem.Checked;

                            progressBar1.Maximum = 100;
                            progressBar1.Minimum = 0;
                            progressBar1.Value = 0;
                            progressBar1.Visible = true;

                            // save images to a GIF file
                            images.SaveAsync(saveFilename, gifEncoder);
                        }
                    }
                    catch (Exception ex)
                    {
                        DemosTools.ShowErrorMessage(ex);
                        progressBar1.Visible = false;

                        // specify that image saving is finished
                        IsFileSaving = false;
                    }
                }
            }
            finally
            {
                // if there is images with burned annotations
                if (images != null && images != imageViewer1.Images)
                {
                    UnsubscribeFromImageCollectionEvents(images);
                    // clear and dispose images with burned annotations
                    images.ClearAndDisposeItems();
                }
            }
        }

        #endregion


        #region 'Annotation' menu

        /// <summary>
        /// Handles the Click event of infoToolStripMenuItem object.
        /// </summary>
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AnnotationsInfoForm dialog = new AnnotationsInfoForm(_dicomViewerTool.DicomAnnotationTool.AnnotationDataController))
            {
                dialog.Owner = this;
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of noneToolStripMenuItem object.
        /// </summary>
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;
        }

        /// <summary>
        /// Handles the Click event of viewToolStripMenuItem object.
        /// </summary>
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.View;
        }

        /// <summary>
        /// Handles the Click event of authorToolStripMenuItem object.
        /// </summary>
        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.Author;
        }

        /// <summary>
        /// Handles the Click event of loadToolStripMenuItem object.
        /// </summary>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDicomAnnotationsFileDialog.FileName = null;
            openDicomAnnotationsFileDialog.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESENTATION_STATE_FILEPREPREALL_FORMATS;
            openDicomAnnotationsFileDialog.FilterIndex = 1;

            if (openDicomAnnotationsFileDialog.ShowDialog() == DialogResult.OK)
            {
                DicomFile presentationStateFile = null;
                try
                {
                    CloseCurrentPresentationStateFile();

                    string fileName = openDicomAnnotationsFileDialog.FileName;
                    presentationStateFile = new DicomFile(fileName, false);
                    if (presentationStateFile.IsReferencedTo(DicomFile) &&
                        presentationStateFile.Annotations != null)
                    {
                        _isAnnotationsLoadedForCurrentFrame = false;
                        _dicomViewerTool.DicomAnnotationTool.AnnotationDataController.AddAnnotationDataSet(presentationStateFile.Annotations);
                        PresentationStateFileController.UpdatePresentationStateFile(DicomFile, presentationStateFile);
                    }
                    else
                    {
                        presentationStateFile.Dispose();
                        presentationStateFile = null;
                    }

                    UpdateUI();
                }
                catch (Exception ex)
                {
                    if (presentationStateFile != null)
                        presentationStateFile.Dispose();

                    DemosTools.ShowErrorMessage(ex);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of presentationStateInfoToolStripMenuItem object.
        /// </summary>
        private void presentationStateInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PresentationStateInfoForm dialog = new PresentationStateInfoForm(PresentationStateFile))
            {
                dialog.Owner = this;
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of presentationStateSaveToolStripMenuItem object.
        /// </summary>
        private void presentationStateSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isAnnotationsLoadedForCurrentFrame)
            {
                DicomAnnotationCodec codec = new DicomAnnotationCodec();
                DicomAnnotationDataCollection collection = (DicomAnnotationDataCollection)
                    _dicomViewerTool.DicomAnnotationTool.AnnotationDataController.GetAnnotations(imageViewer1.Image);
                codec.Encode(PresentationStateFile.Annotations, collection);
                PresentationStateFile.SaveChanges();
            }
            else
            {
                _dicomViewerTool.DicomAnnotationTool.AnnotationDataController.UpdateAnnotationDataSets();
                PresentationStateFile.SaveChanges();
            }
            MessageBox.Show(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESENTATION_STATE_FILE_IS_SAVED);
        }

        /// <summary>
        /// Handles the Click event of presentationStatesSaveToToolStripMenuItem object.
        /// </summary>
        private void presentationStatesSaveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dicomFilePath = _dicomSeriesController.GetDicomFilePath(DicomFile);
            saveDicomAnnotationsFileDialog.FileName = Path.GetFileNameWithoutExtension(dicomFilePath) + ".pre";
            saveDicomAnnotationsFileDialog.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESENTATION_STATE_FILEPREPRE;
            saveDicomAnnotationsFileDialog.FilterIndex = 1;
            // show save dialog
            if (saveDicomAnnotationsFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _dicomViewerTool.DicomAnnotationTool.CancelAnnotationBuilding();

                    // get annotations of DICOM file
                    DicomAnnotationDataCollection[] annotations = GetAnnotationsAssociatedWithDicomFileImages(DicomFile);

                    // create presentation state file
                    using (DicomFile presentationStateFile = CreatePresentationStateFile(DicomFile, annotations))
                    {
                        // get file name of presentation state file
                        string fileName = saveDicomAnnotationsFileDialog.FileName;
                        // if file exists
                        if (File.Exists(fileName))
                            // remove file
                            File.Delete(fileName);

                        // save presentation state file
                        presentationStateFile.Save(fileName);
                    }
                }
                catch (Exception ex)
                {
                    DemosTools.ShowErrorMessage(ex);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of binaryFormatLoadToolStripMenuItem object.
        /// </summary>
        private void binaryFormatLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadAnnotationFromBinaryOrXmpFormat(true);
        }

        /// <summary>
        /// Handles the Click event of binaryFormatSaveToToolStripMenuItem object.
        /// </summary>
        private void binaryFormatSaveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDicomAnnotationsFileDialog.FileName = null;
            saveDicomAnnotationsFileDialog.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_BINARY_ANNOTATIONSVSABVSAB;
            saveDicomAnnotationsFileDialog.FilterIndex = 1;

            if (saveDicomAnnotationsFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(saveDicomAnnotationsFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        AnnotationVintasoftBinaryFormatter annotationFormatter = new AnnotationVintasoftBinaryFormatter();
                        //
                        AnnotationDataCollection annotations = _dicomViewerTool.DicomAnnotationTool.AnnotationDataController.GetAnnotations(imageViewer1.Image);
                        //
                        annotationFormatter.Serialize(fs, annotations);
                    }
                }
                catch (Exception ex)
                {
                    DemosTools.ShowErrorMessage(ex);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of xmpFormatLoadToolStripMenuItem object.
        /// </summary>
        private void xmpFormatLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadAnnotationFromBinaryOrXmpFormat(false);
        }

        /// <summary>
        /// Handles the Click event of xmpFormatSaveToToolStripMenuItem object.
        /// </summary>
        private void xmpFormatSaveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDicomAnnotationsFileDialog.FileName = null;
            saveDicomAnnotationsFileDialog.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_XMP_ANNOTATIONSXMPXMP;
            saveDicomAnnotationsFileDialog.FilterIndex = 1;

            if (saveDicomAnnotationsFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(saveDicomAnnotationsFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        AnnotationVintasoftXmpFormatter annotationFormatter = new AnnotationVintasoftXmpFormatter();

                        //
                        AnnotationDataCollection annotations = _dicomViewerTool.DicomAnnotationTool.AnnotationDataController.GetAnnotations(
                            imageViewer1.Image);
                        //
                        annotationFormatter.Serialize(fs, annotations);
                    }
                }
                catch (Exception ex)
                {
                    DemosTools.ShowErrorMessage(ex);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of addToolStripMenuItem object.
        /// </summary>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (_dicomViewerTool.DicomAnnotationTool.FocusedAnnotationView != null &&
                _dicomViewerTool.DicomAnnotationTool.FocusedAnnotationView.InteractionController ==
                _dicomViewerTool.DicomAnnotationTool.FocusedAnnotationView.Builder)
                _dicomViewerTool.DicomAnnotationTool.CancelAnnotationBuilding();
            annotationsToolStrip1.BuildAnnotation(item.Text);
        }

        #endregion


        #region 'Help' menu

        /// <summary>
        /// Handles the Click event of aboutToolStripMenuItem object.
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBoxForm dlg = new AboutBoxForm())
            {
                dlg.ShowDialog();
            }
        }

        #endregion


        #region File manipulation

        /// <summary>
        /// Handles the OpenFile event of imageViewerToolStrip1 object.
        /// </summary>
        private void imageViewerToolStrip1_OpenFile(object sender, EventArgs e)
        {
            OpenDicomFile();
        }

        #endregion


        #region Image Viewer

        /// <summary>
        /// Handles the KeyDown event of imageViewer1 object.
        /// </summary>
        private void imageViewer1_KeyDown(object sender, KeyEventArgs e)
        {
            // if "Delete" key is pressed
            if (deleteToolStripMenuItem.Enabled &&
                e.KeyCode == Keys.Delete &&
                e.Modifiers == Keys.None)
            {
                ExecuteUiAction<DeleteItemUIAction>();

                // update the UI
                UpdateUI();
            }
        }

        /// <summary>
        /// Handles the ImageLoadingProgress event of imageViewer1 object.
        /// </summary>
        private void imageViewer1_ImageLoadingProgress(object sender, ProgressEventArgs e)
        {
            if (_isFormClosing)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Handles the FocusedIndexChanged event of imageViewer1 object.
        /// </summary>
        private void imageViewer1_FocusedIndexChanged(object sender, FocusedIndexChangedEventArgs e)
        {
            UpdateUI();

            // get the focused image
            VintasoftImage focusedImage = imageViewer1.Image;
            string imageInfo = string.Empty;
            // if image viewer has focused image
            if (focusedImage != null)
            {
                // create information about focused image
                imageInfo = string.Format(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_SIZEARG0XARG1_PIXELFORMATARG2_RESOLUTIONARG3,
                   focusedImage.Width, focusedImage.Height, focusedImage.PixelFormat, focusedImage.Resolution.ToString());

                // get DICOM frame, which is associated with focused image
                DicomFrame dicomFrame = DicomFrame.GetFrameAssociatedWithImage(focusedImage);
                // if DICOM frame exists
                if (dicomFrame != null)
                {
                    // save information about VOI LUT of DICOM frame
                    _toolStripItemToVoiLut[_defaultVoiLutToolStripMenuItem] = dicomFrame.VoiLut;
                }
            }

            // update image info
            imageInfoToolStripStatusLabel.Text = imageInfo;

            if (!_isFocusedIndexChanging)
            {
                _isFocusedIndexChanging = true;
                try
                {
                    // if animation is executed
                    if (IsAnimationStarted)
                    {
                        // disable animation
                        IsAnimationStarted = false;
                        // uncheck the "Show Animation" menu
                        showAnimationToolStripMenuItem.Checked = false;
                    }
                    else
                    {

                        if (_voiLutParamsForm != null)
                            _voiLutParamsForm.DicomFrame = imageViewer1.Image;
                    }

                    imageViewerToolStrip1.SelectedPageIndex = e.FocusedIndex;
                }
                finally
                {
                    _isFocusedIndexChanging = false;
                }
            }
        }

        /// <summary>
        /// Handles the Activated event of noneAction object.
        /// </summary>
        private void noneAction_Activated(object sender, EventArgs e)
        {
            // restore the DICOM viewer tool state
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool = dicomAnnotatedViewerToolStrip1.DicomAnnotatedViewerTool;
            _dicomViewerTool.InteractionMode = _previousDicomViewerToolInteractionMode;
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = _previousDicomAnnotationToolInteractionMode;
        }

        /// <summary>
        /// Handles the Deactivated event of noneAction object.
        /// </summary>
        private void noneAction_Deactivated(object sender, EventArgs e)
        {
            // save the DICOM viewer tool state

            _previousDicomViewerToolInteractionMode = _dicomViewerTool.InteractionMode;
            _previousDicomAnnotationToolInteractionMode = _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode;
        }

        /// <summary>
        /// Handles the Activated event of imageMeasureToolAction object.
        /// </summary>
        private void imageMeasureToolAction_Activated(object sender, EventArgs e)
        {
            _isVisualToolChanging = true;
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool = dicomAnnotatedViewerToolStrip1.DicomAnnotatedViewerTool;
            _dicomViewerTool.InteractionMode = DicomAnnotatedViewerToolInteractionMode.Measuring;
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;
            _isVisualToolChanging = false;
        }

        /// <summary>
        /// Handles the Activated event of magnifierToolAction object.
        /// </summary>
        private void magnifierToolAction_Activated(object sender, EventArgs e)
        {
            _isVisualToolChanging = true;
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool =
                dicomAnnotatedViewerToolStrip1.MainVisualTool.FindVisualTool<MagnifierTool>();
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;
            _isVisualToolChanging = false;
        }

        /// <summary>
        /// Handles the Activated event of PanToolAction object.
        /// </summary>
        private void PanToolAction_Activated(object sender, EventArgs e)
        {
            _isVisualToolChanging = true;
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool =
                dicomAnnotatedViewerToolStrip1.MainVisualTool.FindVisualTool<PanTool>();
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;
            _isVisualToolChanging = false;
        }

        /// <summary>
        /// Handles the PageIndexChanged event of imageViewerToolStrip1 object.
        /// </summary>
        private void imageViewerToolStrip1_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            if (!IsAnimationStarted)
            {
                imageViewer1.FocusedIndex = e.SelectedPageIndex;
            }
        }

        #endregion


        #region Annotations UI

        /// <summary>
        /// Handles the DropDown event of annotationComboBox object.
        /// </summary>
        private void annotationComboBox_DropDown(object sender, EventArgs e)
        {
            FillAnnotationComboBox();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of annotationComboBox object.
        /// </summary>
        private void annotationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageViewer1.FocusedIndex != -1 && annotationComboBox.SelectedIndex != -1)
            {
                _dicomViewerTool.DicomAnnotationTool.FocusedAnnotationData =
                    _dicomViewerTool.DicomAnnotationTool.AnnotationDataCollection[annotationComboBox.SelectedIndex];
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of annotationInteractionModeToolStripComboBox object.
        /// </summary>
        private void annotationInteractionModeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode =
                (AnnotationInteractionMode)annotationInteractionModeToolStripComboBox.SelectedItem;
        }

        /// <summary>
        /// Handles the AnnotationInteractionModeChanged event of annotationTool object.
        /// </summary>
        private void annotationTool_AnnotationInteractionModeChanged(object sender, AnnotationInteractionModeChangedEventArgs e)
        {
            if (!_isVisualToolChanging)
                dicomAnnotatedViewerToolStrip1.Reset();

            interactionMode_noneToolStripMenuItem.Checked = false;
            interactionMode_viewToolStripMenuItem.Checked = false;
            interactionMode_authorToolStripMenuItem.Checked = false;

            DicomAnnotatedViewerToolInteractionMode visualToolInteractionMode =
                 _dicomViewerTool.InteractionMode;

            AnnotationInteractionMode annotationInteractionMode = e.NewValue;
            switch (annotationInteractionMode)
            {
                case AnnotationInteractionMode.None:
                    interactionMode_noneToolStripMenuItem.Checked = true;
                    visualToolInteractionMode = DicomAnnotatedViewerToolInteractionMode.Dicom;
                    break;

                case AnnotationInteractionMode.View:
                    interactionMode_viewToolStripMenuItem.Checked = true;
                    visualToolInteractionMode = DicomAnnotatedViewerToolInteractionMode.Dicom;
                    break;

                case AnnotationInteractionMode.Author:
                    interactionMode_authorToolStripMenuItem.Checked = true;
                    visualToolInteractionMode = DicomAnnotatedViewerToolInteractionMode.Annotation;
                    break;
            }

            if (!_isVisualToolChanging)
                _dicomViewerTool.InteractionMode = visualToolInteractionMode;

            annotationInteractionModeToolStripComboBox.SelectedItem = annotationInteractionMode;


            // update the UI
            UpdateUI();
        }

        #endregion


        #region Annotation visual tool

        /// <summary>
        /// Handles the FocusedAnnotationViewChanged event of annotationTool object.
        /// </summary>
        private void annotationTool_FocusedAnnotationViewChanged(object sender, AnnotationViewChangedEventArgs e)
        {
            if (e.OldValue != null)
                e.OldValue.Data.PropertyChanging -= new EventHandler<ObjectPropertyChangingEventArgs>(AnnotationdData_PropertyChanging);
            if (e.NewValue != null)
                e.NewValue.Data.PropertyChanging += new EventHandler<ObjectPropertyChangingEventArgs>(AnnotationdData_PropertyChanging);

            FillAnnotationComboBox();
            ShowAnnotationProperties(_dicomViewerTool.DicomAnnotationTool.FocusedAnnotationView);

            // update the UI
            UpdateUI();
        }

        /// <summary>
        /// Handles the AnnotationBuildingFinished event of annotationTool object.
        /// </summary>
        private void annotationTool_AnnotationBuildingFinished(object sender, AnnotationViewEventArgs e)
        {
            ShowAnnotationProperties(_dicomViewerTool.DicomAnnotationTool.FocusedAnnotationView);
        }

        /// <summary>
        /// Handles the AnnotationTransformingStarted event of annotationTool object.
        /// </summary>
        private void annotationTool_AnnotationTransformingStarted(object sender, AnnotationViewEventArgs e)
        {
            _isAnnotationTransforming = true;
        }

        /// <summary>
        /// Handles the AnnotationTransformingFinished event of annotationTool object.
        /// </summary>
        private void annotationTool_AnnotationTransformingFinished(object sender, AnnotationViewEventArgs e)
        {
            _isAnnotationTransforming = false;
            propertyGrid1.Refresh();
        }

        /// <summary>
        /// Handles the Changed event of SelectedAnnotations object.
        /// </summary>
        private void SelectedAnnotations_Changed(object sender, EventArgs e)
        {
            // update the UI
            UpdateUI();
        }

        #endregion


        #region VOI LUT

        /// <summary>
        /// Handles the Click event of voiLutMenuItem object.
        /// </summary>
        private void voiLutMenuItem_Click(object sender, EventArgs e)
        {
            if (imageViewer1.Image != null && sender is ToolStripMenuItem)
            {
                if (_currentVoiLutMenuItem != null)
                    _currentVoiLutMenuItem.Checked = false;

                _currentVoiLutMenuItem = (ToolStripMenuItem)sender;
                _currentVoiLutMenuItem.Checked = true;
                _dicomViewerTool.DicomViewerTool.DicomImageVoiLut = _toolStripItemToVoiLut[_currentVoiLutMenuItem];
            }
        }

        /// <summary>
        /// Handles the ButtonClick event of voiLutsToolStripSplitButton object.
        /// </summary>
        private void voiLutsToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            voiLutsToolStripSplitButton.ShowDropDown();
        }

        /// <summary>
        /// Handles the Click event of customVoiLutMenuItem object.
        /// </summary>
        private void customVoiLutMenuItem_Click(object sender, EventArgs e)
        {
            ShowCustomVoiLutForm();
        }

        #endregion


        #region Save image(s)

        /// <summary>
        /// Handles the ImageCollectionSavingProgress event of Images object.
        /// </summary>
        private void Images_ImageCollectionSavingProgress(object sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new SavingProgressDelegate(Images_ImageCollectionSavingProgress), sender, e);
            }
            else
            {
                progressBar1.Value = e.Progress;
            }
        }

        /// <summary>
        /// Handles the ImageCollectionSavingFinished event of Images object.
        /// </summary>
        private void Images_ImageCollectionSavingFinished(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new SavingFinishedDelegate(Images_ImageCollectionSavingFinished), sender, e);
            }
            else
                progressBar1.Visible = false;

            IsFileSaving = false;

            ImageCollection images = (ImageCollection)sender;

            if (images != imageViewer1.Images)
                images.ClearAndDisposeItems();

            if (_imageEncoder != null)
            {
                _imageEncoder.Dispose();
                _imageEncoder = null;
            }
        }

        #endregion

        #endregion


        #region UI state

        /// <summary>
        /// Updates UI safely.
        /// </summary>
        private void InvokeUpdateUI()
        {
            if (InvokeRequired)
                Invoke(new UpdateUIDelegate(UpdateUI));
            else
                UpdateUI();
        }

        /// <summary>
        /// Updates the user interface of this form.
        /// </summary>
        private void UpdateUI()
        {
            // if application is closing
            if (_isFormClosing)
                // exit
                return;

            if (_dicomViewerTool == null)
                return;

            bool hasImages = imageViewer1.Images.Count > 0;
            bool isDicomFileLoaded = hasImages || DicomFile != null;
            bool isDicomFileOpening = _isDicomFileOpening;
            bool isAnnotationsFileLoaded = PresentationStateFile != null;
            bool isFileSaving = _isFileSaving;
            bool isMultipageFile = imageViewer1.Images.Count > 1;
            bool isAnimationStarted = IsAnimationStarted;
            bool isImageSelected = imageViewer1.Image != null;
            bool isAnnotationEmpty = true;
            if (isImageSelected)
            {
                isAnnotationEmpty = _dicomViewerTool.DicomAnnotationTool.AnnotationDataController[imageViewer1.FocusedIndex].Count <= 0;
            }
            bool isAnnotationDataControllerEmpty = true;
            if (_dicomViewerTool.DicomAnnotationTool.ImageViewer != null)
            {
                DicomAnnotationDataController dataController = _dicomViewerTool.DicomAnnotationTool.AnnotationDataController;
                foreach (VintasoftImage image in imageViewer1.Images)
                {
                    if (dataController.GetAnnotations(image).Count > 0)
                    {
                        isAnnotationDataControllerEmpty = false;
                        break;
                    }
                }
            }
            bool isInteractionModeAuthor = _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode == AnnotationInteractionMode.Author;

            bool hasOverlayImages = false;
            bool isMonochromeImage = false;

            DicomFrameMetadata metadata = GetFocusedImageMetadata();
            if (metadata != null)
            {
                hasOverlayImages = metadata.OverlayImages.Length > 0;
                isMonochromeImage = metadata.ColorSpace == DicomImageColorSpaceType.Monochrome1 ||
                                    metadata.ColorSpace == DicomImageColorSpaceType.Monochrome2;
            }

            // 'File' menu
            //
            openDicomFilesToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving;
            saveDicomFileToImageFileToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && hasImages;
            closeDicomFileToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving;
            imageViewerToolStrip1.Enabled = !isDicomFileOpening && !isFileSaving;

            // 'View' menu
            //
            showOverlayImagesToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && hasOverlayImages && !isFileSaving;
            overlayColorToolStripMenuItem.Enabled = showOverlayImagesToolStripMenuItem.Enabled;
            showMetadataInViewerToolStripMenuItem.Enabled = !isAnimationStarted;
            showRulersInViewerToolStripMenuItem.Enabled = !isAnimationStarted;
            rulersUnitOfMeasureToolStripMenuItem.Enabled = !isAnimationStarted;
            voiLutToolStripMenuItem.Enabled = !isAnimationStarted && isMonochromeImage;

            // 'Metadata' menu
            //
            fileMetadataToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving;

            // 'Page' menu
            //
            overlayImagesToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && hasOverlayImages;

            // 'Animation' menu
            //
            showAnimationToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && isMultipageFile;
            animationRepeatToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && isMultipageFile;
            saveAsGifFileToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && isMultipageFile;
            animationDelayToolStripMenuItem.Enabled = animationRepeatToolStripMenuItem.Enabled;

            thumbnailViewer1.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving;

            voiLutsToolStripSplitButton.Visible = isMonochromeImage && _voiLutParamsForm == null;
            voiLutsToolStripSplitButton.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && !isAnimationStarted;


            // "Annotations" menu
            //
            infoToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;
            //
            interactionModeToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;
            //
            presentationStateLoadToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;
            binaryFormatLoadToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;
            xmpFormatLoadToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;
            //
            presentationStateSaveToolStripMenuItem.Enabled = isAnnotationsFileLoaded;
            presentationStateSaveToToolStripMenuItem.Enabled = !isAnnotationDataControllerEmpty;
            presentationStateInfoToolStripMenuItem.Enabled = isAnnotationsFileLoaded;
            binaryFormatSaveToToolStripMenuItem.Enabled = !isAnnotationEmpty;
            xmpFormatSaveToToolStripMenuItem.Enabled = !isAnnotationEmpty;

            //
            addToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded && isInteractionModeAuthor;

            // annotation tool strip 
            annotationsToolStrip1.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;

            annotationComboBox.Enabled = isInteractionModeAuthor;
            propertyGrid1.Enabled = isInteractionModeAuthor;
        }

        /// <summary>
        /// Updates UI with information about DICOM file.
        /// </summary>
        private void UpdateUIWithInformationAboutDicomFile()
        {
            if (DicomFrame != null)
            {
                UpdateWindowLevelToolStripSplitButton(DicomFrame.SourceFile.Modality);
            }
        }

        #endregion


        #region 'Edit' menu

        /// <summary>
        /// Executes the specified type UI action of visual tool.
        /// </summary>
        /// <typeparam name="T">The UI action type.</typeparam>
        private void ExecuteUiAction<T>() where T : UIAction
        {
            // get the UI action
            T uiAction = DemosTools.GetUIAction<T>(imageViewer1.VisualTool);

            if (uiAction != null)
            {
                uiAction.Execute();

                UpdateUI();
            }
        }

        /// <summary>
        /// Enables the "Edit" menu items.
        /// </summary>
        private void EnableEditMenuItems()
        {
            cutToolStripMenuItem.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            deleteAllToolStripMenuItem.Enabled = true;
            deleteAllToolStripMenuItem.Visible = true;
        }

        /// <summary>
        /// Updates the "Edit" menu items.
        /// </summary>
        private void UpdateEditMenuItems()
        {
            VisualTool visualTool = imageViewer1.VisualTool;

            UpdateEditMenuItem(cutToolStripMenuItem, DemosTools.GetUIAction<CutItemUIAction>(visualTool), DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CUT);
            UpdateEditMenuItem(copyToolStripMenuItem, DemosTools.GetUIAction<CopyItemUIAction>(visualTool), DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_COPY);
            UpdateEditMenuItem(pasteToolStripMenuItem, DemosTools.GetUIAction<PasteItemUIAction>(visualTool), DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PASTE);
            UpdateEditMenuItem(deleteToolStripMenuItem, DemosTools.GetUIAction<DeleteItemUIAction>(visualTool), DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DELETE);

            UIAction deleteAllItemsUiAction = DemosTools.GetUIAction<DeleteAllItemsUIAction>(visualTool);
            UpdateEditMenuItem(deleteAllToolStripMenuItem, deleteAllItemsUiAction, DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DELETE_ALL);
            if (deleteAllItemsUiAction == null)
                deleteAllToolStripMenuItem.Visible = false;
            else
                deleteAllToolStripMenuItem.Visible = true;
        }

        /// <summary>
        /// Updates the "Edit" menu item.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <param name="action">The action.</param>
        /// <param name="defaultText">The default text of the menu item.</param>
        private void UpdateEditMenuItem(ToolStripMenuItem menuItem, UIAction action, string defaultText)
        {
            if (action != null && action.IsEnabled)
            {
                menuItem.Enabled = true;
                menuItem.Text = action.Name;
            }
            else
            {
                menuItem.Enabled = false;
                menuItem.Text = defaultText;
            }
        }

        #endregion


        #region 'View' menu

        #region VOI LUT

        /// <summary>
        /// Shows the dialog that allows to change VOI LUT of DICOM frame.
        /// </summary>
        private void ShowCustomVoiLutForm()
        {
            voiLutToolStripMenuItem.Checked ^= true;

            if (voiLutToolStripMenuItem.Checked)
            {
                // create form
                _voiLutParamsForm = new VoiLutParamsForm(this, _dicomViewerTool.DicomViewerTool);
                // set current DICOM frame
                _voiLutParamsForm.DicomFrame = imageViewer1.Image;
                _voiLutParamsForm.FormClosing += new FormClosingEventHandler(voiLutParamsForm_FormClosing);
                // hide VOI LUT info
                voiLutsToolStripSplitButton.Visible = false;
                // show form
                _voiLutParamsForm.Show();
            }
            else
            {
                // close the form
                _voiLutParamsForm.Close();
                _voiLutParamsForm = null;
            }
        }

        #endregion

        #endregion


        #region File manipulation

        /// <summary>
        /// Opens a DICOM file.
        /// </summary>
        private void OpenDicomFile()
        {
            if (openDicomFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openDicomFileDialog.FileNames.Length > 0)
                {
                    // close the previously opened DICOM files
                    ClosePreviouslyOpenedFile();

                    // add DICOM files to the DICOM series
                    AddDicomFilesToSeries(openDicomFileDialog.FileNames);
                    _dicomViewerTool.DicomViewerTool.DicomImageVoiLut =
                        _dicomViewerTool.DicomViewerTool.DefaultDicomImageVoiLut;
                }
            }
        }

        /// <summary>
        /// Adds the DICOM files to the series.
        /// </summary>
        /// <param name="filesPath">Files path.</param>
        private void AddDicomFilesToSeries(params string[] filesPath)
        {
            try
            {
                List<DicomFile> filesForLoadPresentationState = new List<DicomFile>();
                string dirPath = null;

                // show action label and progress bar
                actionLabel.Visible = true;
                progressBar1.Visible = true;
                progressBar1.Maximum = filesPath.Length;
                progressBar1.Value = 0;

                bool skipCorruptedFiles = false;

                foreach (string filePath in filesPath)
                {
                    if (dirPath == null)
                        dirPath = Path.GetDirectoryName(filePath);

                    // set action info
                    actionLabel.Text = string.Format(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_LOADING_ARG0, Path.GetFileName(filePath));
                    // update progress bar
                    progressBar1.Value++;
                    statusStrip1.Update();
                    imageViewer1.Update();

                    DicomFile dicomFile = null;
                    try
                    {
                        // if the series already contains the specified DICOM file
                        if (_dicomSeriesController.Contains(filePath))
                        {
                            DemosTools.ShowInfoMessage(string.Format(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_THE_SERIES_ALREADY_CONTAINS_DICOM_FILE_ARG0, Path.GetFileName(filePath)));
                            return;
                        }

                        // instance number of new DICOM file
                        int newDicomFileInstanceNumber = 0;
                        // add DICOM file to the current series of DICOM images and get the DICOM images of new DICOM file
                        ImageCollection newDicomImages =
                            _dicomSeriesController.AddDicomFileToSeries(filePath, out dicomFile, out newDicomFileInstanceNumber);

                        // if DICOM file represents the DICOM directory
                        if (IsDicomDirectory(dicomFile))
                        {
                            // close the DICOM file
                            _dicomSeriesController.CloseDicomFile(dicomFile);
                            // show the error message
                            DemosTools.ShowInfoMessage(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_THE_DICOM_DIRECTORY_CANNOT_BE_ADDED_TO_THE_SERIES_OF_DICOM_IMAGES);
                            return;
                        }

                        IsDicomFileOpening = true;

                        // if DICOM file does not contain images
                        if (dicomFile.Pages.Count == 0)
                        {
                            // if image viewer contains images
                            if (imageViewer1.Images.Count > 0)
                            {
                                DemosTools.ShowInfoMessage(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_THE_DICOM_FILE_CANNOT_BE_ADDED_TO_THE_SERIES_OF_DICOM_IMAGES_BECAUSE_THE_DICOM_FILE_DOES_NOT_CONTAIN_IMAGE);
                            }
                            else
                            {
                                // save reference to the DICOM file
                                _dicomFileWithoutImages = dicomFile;

                                // show message for user
                                DemosTools.ShowInfoMessage(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DICOM_FILE_DOES_NOT_CONTAIN_IMAGE);
                                // show metadata of DICOM file
                                ShowCurrentFileMetadata();
                            }
                        }
                        else
                        {
                            // update frame count in series
                            imageViewerToolStrip1.PageCount = imageViewer1.Images.Count + dicomFile.Pages.Count;

                            // get image index in image collection of current DICOM file
                            int imageIndex = GetImageIndexInImageCollectionForNewImage(newDicomFileInstanceNumber);

                            try
                            {
                                // insert images to the specified index
                                imageViewer1.Images.InsertRange(imageIndex, newDicomImages.ToArray());
                            }
                            catch
                            {
                                // remove new DICOM images from image collection of image viewer
                                foreach (VintasoftImage newDicomImage in newDicomImages)
                                    imageViewer1.Images.Remove(newDicomImage);

                                // close new DICOM file
                                _dicomSeriesController.CloseDicomFile(dicomFile);
                                dicomFile = null;

                                // update frame count in series
                                imageViewerToolStrip1.PageCount = imageViewer1.Images.Count;

                                throw;
                            }

                            // if DICOM presentation state file must be loaded automatically
                            if (presentationStateLoadAutomaticallyToolStripMenuItem.Checked)
                            {
                                filesForLoadPresentationState.Add(dicomFile);
                            }

                            // if image viewer shows the first image in series
                            if (imageViewerToolStrip1.PageCount == dicomFile.Pages.Count)
                                // update UI of DICOM file
                                UpdateUIWithInformationAboutDicomFile();
                        }

                        // update header of form
                        this.Text = string.Format(_titlePrefix, Path.GetFileName(filePath));
                    }
                    catch (Exception ex)
                    {
                        // close file
                        if (dicomFile != null)
                            _dicomSeriesController.CloseDicomFile(dicomFile);

                        if (!skipCorruptedFiles)
                        {
                            if (filesPath.Length == 1)
                            {
                                DemosTools.ShowErrorMessage(ex);

                                dirPath = null;
                                CloseDicomSeries();
                            }
                            else
                            {
                                string exceptionMessage = string.Format(
                                    DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_THE_FILE_ARG0_CAN_NOT_BE_OPENEDRNARG1RNDO_YOU_WANT_TO_CONTINUE_ANYWAY,
                                    Path.GetFileName(filePath), DemosTools.GetFullExceptionMessage(ex).Trim());
                                if (MessageBox.Show(
                                    exceptionMessage,
                                    DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_ERROR_ALT1,
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Error) == DialogResult.No)
                                {
                                    dirPath = null;
                                    CloseDicomSeries();
                                    break;
                                }
                            }

                            skipCorruptedFiles = true;
                        }
                    }
                }

                // hide action label and progress bar
                actionLabel.Text = string.Empty;
                actionLabel.Visible = false;
                progressBar1.Visible = false;

                if (!string.IsNullOrEmpty(dirPath))
                {
                    // if DICOM presentation files must be loaded automatically
                    if (presentationStateLoadAutomaticallyToolStripMenuItem.Checked)
                        // load presentation state file of DICOM file
                        LoadAnnotationsFromPresentationStateFiles(dirPath, filesForLoadPresentationState.ToArray());
                }

                // update UI
                UpdateUI();
            }
            finally
            {
                // hide action label and progress bar
                actionLabel.Text = string.Empty;
                actionLabel.Visible = false;
                progressBar1.Visible = false;

                if (!_isFormClosing)
                {
                    // update the UI
                    IsDicomFileOpening = false;
                }
            }
        }

        /// <summary>
        /// Returns the index, in image collection, where the new DICOM image must be inserted.
        /// </summary>
        /// <param name="dicomFileInstanceNumber">The DICOM file instance number of new image.</param>
        /// <returns>
        /// The image index of image collection.
        /// </returns>
        private int GetImageIndexInImageCollectionForNewImage(int newImageDicomFileInstanceNumber)
        {
            int imageIndex = imageViewer1.Images.Count;
            while (imageIndex > 0)
            {
                // get DICOM file instance number for the image from image collection
                int imageDicomFileInstanceNumber =
                    _dicomSeriesController.GetDicomFileInstanceNumber(imageViewer1.Images[imageIndex - 1]);

                // if new image must be inserted after the image from image collection
                if (newImageDicomFileInstanceNumber > imageDicomFileInstanceNumber)
                    break;

                imageIndex--;
            }
            return imageIndex;
        }

        /// <summary>
        /// Determines whether the specified DICOM file contains DICOM directory metadata.
        /// </summary>
        /// <param name="dicomFile">The DICOM file.</param>
        /// <returns>
        /// <returns><b>true</b> if the DICOM file contains DICOM directory metadata; 
        /// otherwise, <b>false</b>.</returns>
        /// </returns>
        private bool IsDicomDirectory(DicomFile dicomFile)
        {
            if (dicomFile.DataSet.DataElements.Contains(DicomDataElementId.DirectoryRecordSequence))
                return true;

            return false;
        }

        /// <summary>
        /// Closes the previously opened DICOM file.
        /// </summary>
        private void ClosePreviouslyOpenedFile()
        {
            // if DICOM file without images is opened
            if (_dicomFileWithoutImages != null)
            {
                // close the DICOM file without images
                _dicomFileWithoutImages.Dispose();
                _dicomFileWithoutImages = null;
            }
            // if DICOM series has files
            if (_dicomSeriesController.FileCount > 0)
            {
                // close series of DICOM frames
                CloseDicomSeries();
            }
        }

        /// <summary>
        /// Closes series of DICOM frames.
        /// </summary>
        private void CloseDicomSeries()
        {
            if (imageViewer1.Images.Count != 0)
                CloseAllPresentationStateFiles();

            // if animation is enabled
            if (IsAnimationStarted)
            {
                // stop animation
                IsAnimationStarted = false;
            }

            imageViewerToolStrip1.SelectedPageIndex = -1;
            imageViewerToolStrip1.PageCount = 0;

            // clear image collection of image viewer and dispose all images
            imageViewer1.Images.ClearAndDisposeItems();
            thumbnailViewer1.Images.ClearAndDisposeItems();

            _dicomSeriesController.CloseSeries();
            _dicomFileWithoutImages = null;

            this.Text = string.Format(_titlePrefix, DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_UNTITLED_ALT1);

            // update the UI
            UpdateUI();
            UpdateUIWithInformationAboutDicomFile();
        }

        #endregion


        #region Annotations

        /// <summary>
        /// Returns an array of annotations, which are associated with images from DICOM file.
        /// </summary>
        /// <param name="dicomFile">The DICOM file.</param>
        /// <returns>
        /// The annotation data.
        /// </returns>
        private DicomAnnotationDataCollection[] GetAnnotationsAssociatedWithDicomFileImages(DicomFile dicomFile)
        {
            // create result list
            List<DicomAnnotationDataCollection> result = new List<DicomAnnotationDataCollection>();

            // get data controller of annotation tool
            DicomAnnotationDataController controller = _dicomViewerTool.DicomAnnotationTool.AnnotationDataController;
            // get images of DICOM file
            VintasoftImage[] dicomFileImages = _dicomSeriesController.GetImages(dicomFile);
            // for each image
            foreach (VintasoftImage image in dicomFileImages)
            {
                // get annotations of image
                DicomAnnotationDataCollection annotations =
                    (DicomAnnotationDataCollection)controller.GetAnnotations(image);

                // if annotation collection is not empty
                if (annotations.Count > 0)
                    // add annotations
                    result.Add(annotations);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Loads the annotation from binary or XMP packet.
        /// </summary>
        private void LoadAnnotationFromBinaryOrXmpFormat(bool binaryFormat)
        {
            openDicomAnnotationsFileDialog.FileName = null;
            if (binaryFormat)
                openDicomAnnotationsFileDialog.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_BINARY_ANNOTATIONSVSABVSAB_ALT1;
            else
                openDicomAnnotationsFileDialog.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_XMP_ANNOTATIONSXMPXMP_ALT1;
            openDicomAnnotationsFileDialog.FilterIndex = 1;

            if (openDicomAnnotationsFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(openDicomAnnotationsFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        // get the annotation collection
                        AnnotationDataCollection annotations = _dicomViewerTool.DicomAnnotationTool.AnnotationDataCollection;
                        // clear the annotation collection
                        annotations.ClearAndDisposeItems();
                        // add annotations from stream to the annotation collection
                        annotations.AddFromStream(fs, imageViewer1.Image.Resolution);
                    }
                }
                catch (Exception ex)
                {
                    DemosTools.ShowErrorMessage(ex);
                }
            }
        }

        #endregion


        #region Annotations UI

        /// <summary>
        /// Fills combobox with information about annotations of image.
        /// </summary>
        private void FillAnnotationComboBox()
        {
            annotationComboBox.Items.Clear();

            if (imageViewer1.FocusedIndex >= 0)
            {
                DicomAnnotationDataController annotationDataController = _dicomViewerTool.DicomAnnotationTool.AnnotationDataController;
                AnnotationData focusedAnnotation = _dicomViewerTool.DicomAnnotationTool.FocusedAnnotationData;
                AnnotationDataCollection annotations = annotationDataController[imageViewer1.FocusedIndex];
                for (int i = 0; i < annotations.Count; i++)
                {
                    annotationComboBox.Items.Add(string.Format("[{0}] {1}", i, annotations[i].GetType().Name));
                    if (focusedAnnotation == annotations[i])
                        annotationComboBox.SelectedIndex = i;
                }
            }
        }

        /// <summary>
        /// Shows information about annotation in property grid.
        /// </summary>
        private void ShowAnnotationProperties(AnnotationView annotation)
        {
            if (propertyGrid1.SelectedObject != annotation)
                propertyGrid1.SelectedObject = annotation;
            else if (!_isAnnotationTransforming)
                propertyGrid1.Refresh();
        }

        #endregion


        #region Annotation visual tool

        /// <summary>
        /// The annotation property is changing.
        /// </summary>
        private void AnnotationdData_PropertyChanging(object sender, ObjectPropertyChangingEventArgs e)
        {
            if (e.PropertyName == "UnitOfMeasure")
            {
                if (_isAnnotationPropertyChanging)
                    return;

                _isAnnotationPropertyChanging = true;
                DicomAnnotationData data = (DicomAnnotationData)sender;

                data.ChangeUnitOfMeasure((DicomUnitOfMeasure)e.NewValue, imageViewer1.Image);
                _isAnnotationPropertyChanging = false;
            }
        }

        #endregion


        #region Presentation state file

        /// <summary>
        /// Loads the annotations from DICOM presentation state files.
        /// </summary>
        /// <param name="presentationStateFileDirectoryPath">A path to a directory,
        /// where DICOM presentation files must be searched.</param>
        /// <param name="sourceDicomFiles">The source DICOM files.</param>
        private void LoadAnnotationsFromPresentationStateFiles(
           string presentationStateFileDirectoryPath,
           params DicomFile[] sourceDicomFiles)
        {
            // if directory does NOT exist
            if (!Directory.Exists(presentationStateFileDirectoryPath))
                // exit
                return;

            // get paths to the files in directory
            string[] filePaths = Directory.GetFiles(presentationStateFileDirectoryPath);

            // show action label and progress bar
            actionLabel.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Maximum = filePaths.Length;
            progressBar1.Value = 0;

            try
            {
                // dictionary: DICOM file => path to the DICOM presentation state files, which are referenced to the DICOM file
                Dictionary<DicomFile, List<string>> dicomFileToPresentationStateFilePaths =
                    new Dictionary<DicomFile, List<string>>();

                List<string> dicomFilePaths = new List<string>();
                foreach (DicomFile dicomFile in sourceDicomFiles)
                    dicomFilePaths.Add(_dicomSeriesController.GetDicomFilePath(dicomFile));

                // for each file path in directory
                foreach (string filePath in filePaths)
                {
                    // if file path is NOT a path to a DICOM file
                    if (IsDicomFilePath(filePath, dicomFilePaths))
                        // skip the file
                        continue;

                    // if file path is NOT a path to a DICOM presentation state file
                    if (!IsDicomPresentationStateFilePath(filePath))
                        // skip the file
                        continue;

                    // set action info
                    actionLabel.Text = string.Format(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_SCANNING_ARG0, Path.GetFileName(filePath));
                    // update progress bar
                    progressBar1.Value++;
                    statusStrip1.Update();
                    imageViewer1.Update();

                    DicomFile dicomFile = null;
                    try
                    {
                        // open new DICOM file in read-only mode
                        dicomFile = new DicomFile(filePath, true);
                        // if DICOM file has annotations
                        if (dicomFile.Annotations != null)
                        {
                            // for each source DICOM file
                            foreach (DicomFile sourceDicomFile in sourceDicomFiles)
                            {
                                // if DICOM file references to the source DICOM file
                                if (dicomFile.IsReferencedTo(sourceDicomFile))
                                {
                                    // if presentation state file paths for DICOM file are NOT found
                                    if (!dicomFileToPresentationStateFilePaths.ContainsKey(sourceDicomFile))
                                        // create an empty list
                                        dicomFileToPresentationStateFilePaths.Add(sourceDicomFile, new List<string>());
                                    // add file path to a list of presentation state file paths
                                    dicomFileToPresentationStateFilePaths[sourceDicomFile].Add(Path.GetFullPath(filePath));

                                    break;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (dicomFile != null)
                            dicomFile.Dispose();
                    }
                }

                // hide action label and progress bar
                actionLabel.Text = string.Empty;
                actionLabel.Visible = false;
                progressBar1.Visible = false;

                // if presentation state files is searched
                if (dicomFileToPresentationStateFilePaths.Count > 0)
                {
                    foreach (DicomFile sourceDicomFile in dicomFileToPresentationStateFilePaths.Keys)
                    {
                        // load DICOM annotations from DICOM presentation state file
                        SelectDicomPresentationStateFileAndLoadAnnotations(
                            sourceDicomFile,
                            dicomFileToPresentationStateFilePaths[sourceDicomFile].ToArray(),
                            _dicomViewerTool.DicomAnnotationTool.AnnotationDataController);
                    }
                }
            }
            finally
            {
                // hide action label and progress bar
                actionLabel.Text = string.Empty;
                actionLabel.Visible = false;
                progressBar1.Visible = false;
            }
        }

        /// <summary>
        /// Selects the DICOM presentation state file and loads annotations from the selected file.
        /// </summary>
        /// <param name="dicomFile">DICOM file.</param>
        /// <param name="presentationStateFilePaths">An array with paths to the DICOM presentation files,
        /// which are associated with <i>dicomFile</i>.</param>
        /// <param name="annotationDataController">The annotation data controller, where information
        /// about annotations must be added.</param>
        private void SelectDicomPresentationStateFileAndLoadAnnotations(
            DicomFile dicomFile,
            string[] presentationStateFilePaths,
            DicomAnnotationDataController annotationDataController)
        {
            // create dialog
            using (SelectPresentationStateFile dlg =
                new SelectPresentationStateFile(presentationStateFilePaths))
            {
                string dicomFilePath = _dicomSeriesController.GetDicomFilePath(dicomFile);
                dlg.Text += ": " + Path.GetFileName(dicomFilePath);
                dlg.Owner = this;

                string selectedPresentationStateFileName = null;
                if (presentationStateFilePaths.Length > 0)
                {
                    string selectedPresentationStateFilePath = presentationStateFilePaths[presentationStateFilePaths.Length - 1];
                    selectedPresentationStateFileName = Path.GetFileNameWithoutExtension(selectedPresentationStateFilePath);
                }
                if (selectedPresentationStateFileName != null)
                    dlg.SelectedPresentationStateFilename = selectedPresentationStateFileName;

                // show dialog
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // get name of selected presentation state file
                    string presentationStateFilename = dlg.SelectedPresentationStateFilename;
                    // if name is not empty
                    if (!string.IsNullOrEmpty(presentationStateFilename))
                    {
                        // create DICOM presentation state file
                        DicomFile presentationStateFile =
                            PresentationStateFileController.LoadPresentationStateFile(
                            dicomFile, presentationStateFilename);
                        // add annotations from DICOM presentation state file to the annotation data controller
                        annotationDataController.AddAnnotationDataSet(presentationStateFile.Annotations);
                    }
                }
            }
        }

        /// <summary>
        /// Determines that file path is a path to a DICOM file.
        /// </summary>
        /// <param name="filePath">A file path.</param>
        /// <param name="dicomFilePaths">A list with paths to the DICOM files.</param>
        private bool IsDicomFilePath(string filePath, List<string> dicomFilePaths)
        {
            // for each DICOM file path
            foreach (string dicomFilePath in dicomFilePaths)
            {
                // if file path and DICOM file path are equals
                if (filePath.ToUpperInvariant() == dicomFilePath.ToUpperInvariant())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines that file path is a path to a DICOM presentation state file.
        /// </summary>
        /// <param name="filePath">A file path.</param>
        private bool IsDicomPresentationStateFilePath(string filePath)
        {
            // get file extension
            string fileExtension = Path.GetExtension(filePath);
            // for each supported presentation file extension
            for (int i = 0; i < _presentationStateFileExtensions.Length; i++)
            {
                // if file has presentation file extension
                if (string.Equals(fileExtension, _presentationStateFileExtensions[i], StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Creates the DICOM presentation state file.
        /// </summary>
        /// <param name="dicomFile">The source DICOM file.</param>
        /// <param name="annotations">The annotations of DICOM presentation state file.</param>
        /// <returns>
        /// The presentation state file.
        /// </returns>
        private DicomFile CreatePresentationStateFile(
            DicomFile dicomFile,
            params DicomAnnotationDataCollection[] annotations)
        {
            // create DICOM presentation state file for DICOM image file
            DicomFile presentationStateFile = DicomFile.CreatePresentationState(dicomFile);

            // create annotaiton codec
            DicomAnnotationCodec annoCodec = new DicomAnnotationCodec();
            if (presentationStateFile.Annotations == null)
                presentationStateFile.Annotations = new DicomAnnotationTreeNode(presentationStateFile);

            // encode annotation to the DICOM presentation state file
            annoCodec.Encode(presentationStateFile.Annotations, annotations);

            return presentationStateFile;
        }

        /// <summary>
        /// Closes the DICOM presentation state file of focused DICOM file.
        /// </summary>
        private void CloseCurrentPresentationStateFile()
        {
            ClosePresentationStateFileOfFile(DicomFile);
        }

        /// <summary>
        /// Closes all DICOM presentation state files.
        /// </summary>
        private void CloseAllPresentationStateFiles()
        {
            DicomFile[] dicomFiles = _dicomSeriesController.GetFilesOfSeries();

            foreach (DicomFile dicomFile in dicomFiles)
                ClosePresentationStateFileOfFile(dicomFile);
        }

        /// <summary>
        /// Closes the DICOM presentation state file of specified DICOM file.
        /// </summary>
        /// <param name="dicomFile">The DICOM file.</param>
        private void ClosePresentationStateFileOfFile(DicomFile dicomFile)
        {
            // get the presentation state file of source DICOM file
            DicomFile presentationStateFile = PresentationStateFileController.GetPresentationStateFile(dicomFile);

            if (presentationStateFile == null)
                return;

            // get controller of DicomAnnotationTool
            DicomAnnotationDataController controller = _dicomViewerTool.DicomAnnotationTool.AnnotationDataController;

            // remove annotations from controller
            controller.RemoveAnnotationDataSet(presentationStateFile.Annotations);

            // close the presentation state file of source DICOM file
            PresentationStateFileController.ClosePresentationStateFile(dicomFile);
        }

        #endregion


        #region VOI LUT

        /// <summary>
        /// Updates menu items with VOI LUT information.
        /// </summary>
        /// <param name="modality">Modality of DICOM file.</param>
        private void UpdateWindowLevelToolStripSplitButton(DicomFileModality modality)
        {
            ToolStripItemCollection items = voiLutsToolStripSplitButton.DropDownItems;
            // clear buttons
            items.Clear();

            // is current frame is empty
            if (imageViewer1.Image == null)
                return;

            // add default window level button
            _defaultVoiLutToolStripMenuItem.Checked = true;
            _currentVoiLutMenuItem = _defaultVoiLutToolStripMenuItem;
            items.Add(_defaultVoiLutToolStripMenuItem);

            _toolStripItemToVoiLut.Clear();

            ToolStripMenuItem menuItem = null;

            DicomFrameMetadata metadata = GetFocusedImageMetadata();
            DicomImageVoiLookupTable defaultVoiLut = metadata.VoiLut;
            if (defaultVoiLut.IsEmpty)
                defaultVoiLut = _dicomViewerTool.DicomViewerTool.DicomImageVoiLut;
            _toolStripItemToVoiLut.Add(_defaultVoiLutToolStripMenuItem, defaultVoiLut);


            if (metadata == null)
                return;

            // get the available VOI LUTs
            DicomImageVoiLookupTable[] voiLuts = metadata.AvailableVoiLuts;
            // if DICOM frame has VOI LUTs
            if (voiLuts.Length > 0)
            {
                bool addSeparator = true;
                // for each VOI LUT
                for (int i = 0; i < voiLuts.Length; i++)
                {
                    // if VOI LUT is equal to the default VOI LUT
                    if (metadata.VoiLut.WindowCenter == voiLuts[i].WindowCenter &&
                        metadata.VoiLut.WindowWidth == voiLuts[i].WindowWidth)
                        continue;

                    if (addSeparator)
                    {
                        items.Add(new ToolStripSeparator());
                        addSeparator = false;
                    }

                    string explanation = voiLuts[i].Explanation;
                    if (explanation == string.Empty)
                        explanation = string.Format("VOI LUT {0}", i + 1);

                    menuItem = new ToolStripMenuItem(explanation);
                    _toolStripItemToVoiLut.Add(menuItem, voiLuts[i]);
                    menuItem.Click += new EventHandler(voiLutMenuItem_Click);
                    items.Add(menuItem);
                }
            }

            string[] windowExplanation = null;
            voiLuts = null;

            // add standard VOI LUT for specific modalities

            switch (modality)
            {
                case DicomFileModality.CT:
                    windowExplanation = new string[] {
                        "Abdomen",
                        "Angio",
                        "Bone",
                        "Brain",
                        "Chest",
                        "Lungs" };

                    voiLuts = new DicomImageVoiLookupTable[] {
                        new DicomImageVoiLookupTable(60,400),
                        new DicomImageVoiLookupTable(300,600),
                        new DicomImageVoiLookupTable(300,1500),
                        new DicomImageVoiLookupTable(40,80),
                        new DicomImageVoiLookupTable(40,400),
                        new DicomImageVoiLookupTable(-400,1500) };
                    break;

                case DicomFileModality.CR:
                case DicomFileModality.DX:
                case DicomFileModality.MR:
                case DicomFileModality.NM:
                case DicomFileModality.XA:
                    windowExplanation = new string[] {
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_20_WIDTH_40,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_40_WIDTH_80,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_80_WIDTH_160,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_600_WIDTH_1280,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_1280_WIDTH_2560,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_2560_WIDTH_5120};

                    voiLuts = new DicomImageVoiLookupTable[] {
                        new DicomImageVoiLookupTable(20,40),
                        new DicomImageVoiLookupTable(40,80),
                        new DicomImageVoiLookupTable(80,160),
                        new DicomImageVoiLookupTable(600,1280),
                        new DicomImageVoiLookupTable(1280,2560),
                        new DicomImageVoiLookupTable(2560,5120) };
                    break;

                case DicomFileModality.MG:
                case DicomFileModality.PT:
                    windowExplanation = new string[] {
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_30_WIDTH_60,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_125_WIDTH_250,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_500_WIDTH_1000,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_1875_WIDTH_3750,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_3750_WIDTH_7500,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_7500_WIDTH_15000,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_15000_WIDTH_30000,
                        DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CENTER_30000_WIDTH_60000};

                    voiLuts = new DicomImageVoiLookupTable[] {
                        new DicomImageVoiLookupTable(30,60),
                        new DicomImageVoiLookupTable(125,250),
                        new DicomImageVoiLookupTable(500,1000),
                        new DicomImageVoiLookupTable(1875,3750),
                        new DicomImageVoiLookupTable(3750,7500),
                        new DicomImageVoiLookupTable(7500,15000),
                        new DicomImageVoiLookupTable(15000,30000),
                        new DicomImageVoiLookupTable(30000,60000) };
                    break;
            }

            if (voiLuts != null)
            {
                items.Add(new ToolStripSeparator());
                for (int i = 0; i < voiLuts.Length; i++)
                {
                    menuItem = new ToolStripMenuItem(windowExplanation[i]);
                    _toolStripItemToVoiLut.Add(menuItem, voiLuts[i]);
                    menuItem.Click += new EventHandler(voiLutMenuItem_Click);
                    items.Add(menuItem);
                }
            }

            items.Add(new ToolStripSeparator());
            menuItem = new ToolStripMenuItem(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CUSTOM_VOI_LUT);
            menuItem.Click += customVoiLutMenuItem_Click;
            items.Add(menuItem);
        }

        #endregion


        #region Metadata

        /// <summary>
        /// Shows a form that allows to browse the DICOM file metadata.
        /// </summary>
        private void ShowCurrentFileMetadata()
        {
            // create a metadata editor form
            using (DicomMetadataEditorForm dlg = new DicomMetadataEditorForm())
            {
                dlg.CanEdit = false;
                dlg.StartPosition = FormStartPosition.CenterScreen;

                // get metadata of DICOM image
                DicomFrameMetadata metadata = GetFocusedImageMetadata();
                // if DICOM image does not have metadata
                if (metadata == null)
                    // get metadata of DICOM file
                    metadata = new DicomFrameMetadata(DicomFile);
                dlg.RootMetadataNode = metadata;

                // show the dialog
                dlg.ShowDialog();

                // if image viewer has image
                if (imageViewer1.Image != null)
                {
                    // update the UI with information about DICOM file
                    UpdateUIWithInformationAboutDicomFile();
                    // refresh the DICOM viewer tool
                    _dicomViewerTool.DicomViewerTool.Refresh();
                }

                UpdateUI();
            }
        }

        /// <summary>
        /// Returns the metadata of focused image.
        /// </summary>
        private DicomFrameMetadata GetFocusedImageMetadata()
        {
            if (imageViewer1.Image == null)
                return null;

            DicomFrameMetadata metadata = imageViewer1.Image.Metadata.MetadataTree as DicomFrameMetadata;

            return metadata;
        }

        #endregion


        #region Frame animation

        /// <summary>
        /// Starts animation.
        /// </summary>
        private void StartAnimation()
        {
            StopAnimation();

            _animationThread = new Thread(AnimationMethod);
            _animationThread.IsBackground = true;

            // disable visual tool
            _dicomViewerTool.Enabled = false;
            _dicomViewerTool.DicomViewerTool.IsTextOverlayVisible = false;
            _dicomViewerTool.DicomViewerTool.ShowRulers = false;
            // disable tool strip
            imageViewerToolStrip1.Enabled = false;
            annotationsToolStrip1.Enabled = false;
            annotationInteractionModeToolStrip.Enabled = false;
            dicomAnnotatedViewerToolStrip1.Enabled = false;
            dicomAnnotatedViewerToolStrip1.FindAction<MagnifierToolAction>().VisualTool.Enabled = false;

            if (_voiLutParamsForm != null)
            {
                _voiLutParamsForm.Enabled = false;
                _voiLutParamsForm.Hide();
            }

            _animationThread.Start();
            _isAnimationStarted = true;
        }

        /// <summary>
        /// Stops animation.
        /// </summary>
        private void StopAnimation()
        {
            _isAnimationStarted = false;

            if (_animationThread == null)
                return;

            showAnimationToolStripMenuItem.Checked = false;

            _animationThread = null;

            _isFocusedIndexChanging = false;

            if (_voiLutParamsForm != null)
            {
                _voiLutParamsForm.Show();
                _voiLutParamsForm.Enabled = true;
            }

            // if the focused index in thumbnail viewer was NOT changed during animation
            if (!_isFocusedIndexChanging)
            {
                // if thumbnail viewer and image viewer have different focused images
                if (imageViewer1.FocusedIndex != _currentAnimatedFrameIndex)
                {
                    // change focus in the thumbnail viewer and make it the same as focus in image viewer
                    imageViewer1.FocusedIndex = _currentAnimatedFrameIndex;
                }

                if (imageViewerToolStrip1.SelectedPageIndex != _currentAnimatedFrameIndex)
                {
                    imageViewerToolStrip1.SelectedPageIndex = _currentAnimatedFrameIndex;
                }
            }

            // enable tool strip
            imageViewerToolStrip1.Enabled = true;
            annotationsToolStrip1.Enabled = true;
            annotationInteractionModeToolStrip.Enabled = true;
            dicomAnnotatedViewerToolStrip1.Enabled = true;
            dicomAnnotatedViewerToolStrip1.FindAction<MagnifierToolAction>().VisualTool.Enabled = true;
            // enable visual tool
            _dicomViewerTool.DicomViewerTool.IsTextOverlayVisible = showMetadataInViewerToolStripMenuItem.Checked;
            _dicomViewerTool.DicomViewerTool.ShowRulers = showRulersInViewerToolStripMenuItem.Checked;
            _dicomViewerTool.Enabled = true;
        }

        /// <summary>
        /// Animation thread.
        /// </summary>
        private void AnimationMethod()
        {
            Thread currentThread = Thread.CurrentThread;
            _currentAnimatedFrameIndex = imageViewer1.FocusedIndex;
            int count = imageViewer1.Images.Count;
            for (; _currentAnimatedFrameIndex < count || _isAnimationCycled;)
            {
                if (_animationThread != currentThread)
                    break;

                _isFocusedIndexChanging = true;
                // change focused image in image viewer
                imageViewer1.SetFocusedIndexSync(_currentAnimatedFrameIndex);
                _isFocusedIndexChanging = false;
                Thread.Sleep(_animationDelay);

                _currentAnimatedFrameIndex++;
                if (_isAnimationCycled && _currentAnimatedFrameIndex >= count)
                    _currentAnimatedFrameIndex = 0;
            }

            _currentAnimatedFrameIndex--;
            BeginInvoke(new ThreadStart(StopAnimation));
        }

        #endregion


        #region Save image(s)

        /// <summary>
        /// Returns the encoder for saving of single image.
        /// </summary>
        private EncoderBase GetEncoder(string filename)
        {
            MultipageEncoderBase multipageEncoder = GetMultipageEncoder(filename);
            if (multipageEncoder != null)
                return multipageEncoder;

            switch (Path.GetExtension(filename).ToUpperInvariant())
            {
                case ".JPG":
                case ".JPEG":
                    JpegEncoder jpegEncoder = new JpegEncoder();
                    jpegEncoder.Settings.AnnotationsFormat = AnnotationsFormat.VintasoftBinary;

                    JpegEncoderSettingsForm jpegEncoderSettingsDlg = new JpegEncoderSettingsForm();
                    jpegEncoderSettingsDlg.EditAnnotationSettings = true;
                    jpegEncoderSettingsDlg.EncoderSettings = jpegEncoder.Settings;
                    if (jpegEncoderSettingsDlg.ShowDialog() != DialogResult.OK)
                        return null;

                    return jpegEncoder;

                case ".PNG":
                    PngEncoder pngEncoder = new PngEncoder();
                    pngEncoder.Settings.AnnotationsFormat = AnnotationsFormat.VintasoftBinary;

                    PngEncoderSettingsForm pngEncoderSettingsDlg = new PngEncoderSettingsForm();
                    pngEncoderSettingsDlg.EditAnnotationSettings = true;
                    pngEncoderSettingsDlg.EncoderSettings = pngEncoder.Settings;
                    if (pngEncoderSettingsDlg.ShowDialog() != DialogResult.OK)
                        return null;

                    return pngEncoder;

                default:
                    return AvailableEncoders.CreateEncoder(filename);
            }
        }

        /// <summary>
        /// Returns the encoder for saving of image collection.
        /// </summary>
        private MultipageEncoderBase GetMultipageEncoder(string filename)
        {
            bool isFileExist = File.Exists(filename);
            switch (Path.GetExtension(filename).ToUpperInvariant())
            {
                case ".TIF":
                case ".TIFF":
                    TiffEncoder tiffEncoder = new TiffEncoder();
                    tiffEncoder.Settings.AnnotationsFormat = AnnotationsFormat.VintasoftBinary;

                    TiffEncoderSettingsForm tiffEncoderSettingsDlg = new TiffEncoderSettingsForm();
                    tiffEncoderSettingsDlg.CanAddImagesToExistingFile = isFileExist;
                    tiffEncoderSettingsDlg.EditAnnotationSettings = true;
                    tiffEncoderSettingsDlg.EncoderSettings = tiffEncoder.Settings;
                    if (tiffEncoderSettingsDlg.ShowDialog() != DialogResult.OK)
                        return null;

                    return tiffEncoder;
            }

            return null;
        }

        /// <summary>
        /// Returns a value indicating whether there are annotations on images.
        /// </summary>
        /// <param name="images">Image collection.</param>
        /// <param name="annotationTool">DICOM annotation tool.</param>
        /// <returns>A value indicating whether there are annotations on images.</returns>
        private bool AreThereAnnotationsOnImages(ImageCollection images, DicomAnnotationTool annotationTool)
        {
            DicomAnnotationDataController annotationDataController = annotationTool.AnnotationDataController;
            for (int i = 0; i < images.Count; i++)
            {
                if (annotationDataController[i].Count > 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns images with burned annotations.
        /// </summary>
        /// <param name="images">Image collection.</param>
        /// <param name="annotationTool">DICOM annotation tool.</param>
        /// <returns>Images with burned annotations.</returns>
        private ImageCollection GetImagesWithBurnedAnnotations(ImageCollection images, DicomAnnotationTool annotationTool)
        {
            ImageCollection imagesWithBurnedAnnotations = new ImageCollection();

            using (AnnotationViewController viewController = new AnnotationViewController(annotationTool.AnnotationDataController))
            {
                for (int i = 0; i < images.Count; i++)
                {
                    imagesWithBurnedAnnotations.Add(viewController.GetImageWithAnnotations(i));
                }
            }

            return imagesWithBurnedAnnotations;
        }

        #endregion


        #region View Rotation

        /// <summary>
        /// Rotates images in both annotation viewer and thumbnail viewer by 90 degrees clockwise.
        /// </summary>
        private void RotateViewClockwise()
        {
            if (imageViewer1.ImageRotationAngle != 270)
            {
                imageViewer1.ImageRotationAngle += 90;
                thumbnailViewer1.ImageRotationAngle += 90;
            }
            else
            {
                imageViewer1.ImageRotationAngle = 0;
                thumbnailViewer1.ImageRotationAngle = 0;
            }
        }

        /// <summary>
        /// Rotates images in both annotation viewer and thumbnail viewer by 90 degrees counterclockwise.
        /// </summary>
        private void RotateViewCounterClockwise()
        {
            if (imageViewer1.ImageRotationAngle != 0)
            {
                imageViewer1.ImageRotationAngle -= 90;
                thumbnailViewer1.ImageRotationAngle -= 90;
            }
            else
            {
                imageViewer1.ImageRotationAngle = 270;
                thumbnailViewer1.ImageRotationAngle = 270;
            }
        }

        #endregion


        #region Init

        /// <summary>
        /// Initializes the image viewer tool strip.
        /// </summary>
        private void InitImageViewerToolStrip()
        {
            imageViewerToolStrip1.ImageViewer = imageViewer1;
            imageViewerToolStrip1.SelectedPageIndex = -1;
            imageViewerToolStrip1.UseImageViewerImages = false;
            imageViewerToolStrip1.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(imageViewerToolStrip1_PageIndexChanged);
        }

        /// <summary>
        /// Initializes the DICOM annotation tool.
        /// </summary>
        private void InitDicomAnnotationTool()
        {
            _dicomViewerTool.DicomAnnotationTool.MultiSelect = false;
            _dicomViewerTool.DicomAnnotationTool.FocusedAnnotationViewChanged +=
                new EventHandler<AnnotationViewChangedEventArgs>(annotationTool_FocusedAnnotationViewChanged);
            _dicomViewerTool.DicomAnnotationTool.SelectedAnnotations.Changed +=
                new EventHandler(SelectedAnnotations_Changed);
            _dicomViewerTool.DicomAnnotationTool.AnnotationBuildingFinished +=
                new EventHandler<AnnotationViewEventArgs>(annotationTool_AnnotationBuildingFinished);
            _dicomViewerTool.DicomAnnotationTool.AnnotationTransformingStarted +=
                new EventHandler<AnnotationViewEventArgs>(annotationTool_AnnotationTransformingStarted);
            _dicomViewerTool.DicomAnnotationTool.AnnotationTransformingFinished +=
                new EventHandler<AnnotationViewEventArgs>(annotationTool_AnnotationTransformingFinished);
            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionModeChanged +=
                new EventHandler<AnnotationInteractionModeChangedEventArgs>(annotationTool_AnnotationInteractionModeChanged);

            _dicomViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;

            _dicomViewerTool.DicomAnnotationTool.SpellChecker = SpellCheckTools.CreateSpellCheckManager();

            annotationInteractionModeToolStripComboBox.Items.Add(AnnotationInteractionMode.None);
            annotationInteractionModeToolStripComboBox.Items.Add(AnnotationInteractionMode.View);
            annotationInteractionModeToolStripComboBox.Items.Add(AnnotationInteractionMode.Author);
            // set interaction mode to the View 
            annotationInteractionModeToolStripComboBox.SelectedItem = AnnotationInteractionMode.None;
        }

        /// <summary>
        /// Initializes the unit of measures for rulers.
        /// </summary>
        private void InitUnitOfMeasuresForRulers()
        {
            UnitOfMeasure[] unitsOfMeasure = new UnitOfMeasure[] {
                UnitOfMeasure.Centimeters,
                UnitOfMeasure.Inches,
                UnitOfMeasure.Millimeters,
                UnitOfMeasure.Pixels
            };

            _toolStripMenuItemToRulersUnitOfMeasure.Clear();

            foreach (UnitOfMeasure unit in unitsOfMeasure)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(unit.ToString());
                _toolStripMenuItemToRulersUnitOfMeasure.Add(menuItem, unit);
                menuItem.Click += new EventHandler(rulersUnitOfMeasureToolStripMenuItem_Click);
                if (unit == _dicomViewerTool.DicomViewerTool.RulersUnitOfMeasure)
                {
                    menuItem.Checked = true;
                    _currentRulersUnitOfMeasureMenuItem = menuItem;
                }
                rulersUnitOfMeasureToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        #endregion

        
        #region Drag&Drop
      
        /// <summary>
        /// Handles the Dragging event of imageViewer1 object.
        /// </summary>
        private void imageViewer1_Dragging(object sender, DragEventArgs e)
        {
            // if image files are dragging
            if (e.Data.GetDataPresent("FileNameW"))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Handles the DragDrop event of imageViewer1 object.
        /// </summary>
        private void imageViewer1_DragDrop(object sender, DragEventArgs e)
        {
            // if image viewer allows to drop image files and image files are dropped
            if (e.Data.GetDataPresent("FileDrop"))
            {
                // get image file names
                string[] filenames = (string[])e.Data.GetData("FileDrop");

                // close the previously opened DICOM files
                ClosePreviouslyOpenedFile();

                // if is single directory
                if (filenames.Length == 1 && Directory.Exists(filenames[0]))
                {
                    // get files from directory
                    filenames = Directory.GetFiles(filenames[0]);
                }

                // add DICOM files to the DICOM series
                AddDicomFilesToSeries(filenames);
                _dicomViewerTool.DicomViewerTool.DicomImageVoiLut = _dicomViewerTool.DicomViewerTool.DefaultDicomImageVoiLut;
            }
        }
     
        #endregion

        /// <summary>
        /// Subscribes to the event of image collection.
        /// </summary>
        /// <param name="images">Image collection.</param>
        private void SubscribeToImageCollectionEvents(ImageCollection images)
        {
            images.ImageCollectionSavingProgress += new EventHandler<ProgressEventArgs>(Images_ImageCollectionSavingProgress);
            images.ImageCollectionSavingFinished += new EventHandler(Images_ImageCollectionSavingFinished);
        }

        /// <summary>
        /// Unsubscribes from the event of image collection.
        /// </summary>
        /// <param name="images">Image collection.</param>
        private void UnsubscribeFromImageCollectionEvents(ImageCollection images)
        {
            images.ImageCollectionSavingProgress -= new EventHandler<ProgressEventArgs>(Images_ImageCollectionSavingProgress);
            images.ImageCollectionSavingFinished -= new EventHandler(Images_ImageCollectionSavingFinished);
        }

        /// <summary>
        /// Moves the DICOM codec to the first position in <see cref="AvailableCodecs"/>.
        /// </summary>
        private static void MoveDicomCodecToFirstPosition()
        {
            ReadOnlyCollection<Codec> codecs = AvailableCodecs.Codecs;

            for (int i = codecs.Count - 1; i >= 0; i--)
            {
                Codec codec = codecs[i];

                if (codec.Name.Equals("DICOM", StringComparison.InvariantCultureIgnoreCase))
                {
                    AvailableCodecs.RemoveCodec(codec);
                    AvailableCodecs.InsertCodec(0, codec);
                    break;
                }
            }
        }



        #endregion

        #endregion



        #region Delegates

        /// <summary>
        /// Represents the <see cref="UpdateUI"/> method.
        /// </summary>
        delegate void UpdateUIDelegate();

        /// <summary>
        /// Represents the method that handles the <see cref="ImageCollection.ImageCollectionSavingProgress"/> event.
        /// </summary>
        delegate void SavingProgressDelegate(object sender, ProgressEventArgs e);

        /// <summary>
        /// Represents the method that handles the <see cref="ImageCollection.ImageCollectionSavingFinished"/> event.
        /// </summary>
        delegate void SavingFinishedDelegate(object sender, EventArgs e);

        #endregion

    }
}
