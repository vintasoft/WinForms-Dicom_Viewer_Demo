using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Vintasoft.Imaging;
#if !REMOVE_ANNOTATION_PLUGIN
using Vintasoft.Imaging.Annotation;
using Vintasoft.Imaging.Annotation.Dicom;
using Vintasoft.Imaging.Annotation.Dicom.UI.VisualTools;
using Vintasoft.Imaging.Annotation.Formatters;
using Vintasoft.Imaging.Annotation.UI;
using Vintasoft.Imaging.Annotation.UI.VisualTools;
#endif
using Vintasoft.Imaging.Codecs;
using Vintasoft.Imaging.Codecs.Decoders;
using Vintasoft.Imaging.Codecs.Encoders;
using Vintasoft.Imaging.Codecs.ImageFiles.Dicom;
using Vintasoft.Imaging.Dicom.UI;
using Vintasoft.Imaging.Dicom.UI.VisualTools;
using Vintasoft.Imaging.ImageColors;
using Vintasoft.Imaging.Metadata;
using Vintasoft.Imaging.UI;
using Vintasoft.Imaging.UI.VisualTools;
using Vintasoft.Imaging.UI.VisualTools.UserInteraction;
using Vintasoft.Imaging.UIActions;
using Vintasoft.Primitives;

using DemosCommonCode;
using DemosCommonCode.Imaging;
using DemosCommonCode.Imaging.Codecs;
using DemosCommonCode.Imaging.Codecs.Dialogs;

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
        /// DICOM viewer tool.
        /// </summary>
        DicomViewerTool _dicomViewerTool;

#if !REMOVE_ANNOTATION_PLUGIN
        /// <summary>
        /// DICOM annotated viewer tool.
        /// </summary>
        DicomAnnotatedViewerTool _dicomAnnotatedViewerTool;

        /// <summary>
        /// The previous interaction mode in DICOM viewer tool.
        /// </summary>
        DicomAnnotatedViewerToolInteractionMode _previousDicomViewerToolInteractionMode;

        /// <summary>
        /// The previous interaction mode in DICOM annotation tool.
        /// </summary>
        AnnotationInteractionMode _previousDicomAnnotationToolInteractionMode;

        /// <summary>
        /// Manager of interaction areas.
        /// </summary>
        InteractionAreaAppearanceManager _interactionAreaAppearanceManager;
#endif

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
        /// A value indicating whether image coolection must be disposed after save.
        /// </summary>
        bool _disposeImageCollectionAfterSave = false;


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

        /// <summary>
        /// Decoding setting of DICOM frame.
        /// </summary>
        DicomDecodingSettings _dicomFrameDecodingSettings = new DicomDecodingSettings(false);


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


        #region Annotations

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

            MeasurementVisualToolActionFactory.CreateActions(dicomAnnotatedViewerToolStrip1);
            dicomAnnotatedViewerToolStrip1.Items.Remove(voiLutsToolStripSplitButton);
            dicomAnnotatedViewerToolStrip1.Items.Add(voiLutsToolStripSplitButton);
            voiLutsToolStripSplitButton.Click += voiLutsToolStripSplitButton_ButtonClick;

            NoneAction noneAction = dicomAnnotatedViewerToolStrip1.FindAction<NoneAction>();
            noneAction.Activated += new EventHandler(noneAction_Activated);
            noneAction.Deactivated += new EventHandler(noneAction_Deactivated);

#if !REMOVE_ANNOTATION_PLUGIN
            ImageMeasureToolAction imageMeasureToolAction =
                dicomAnnotatedViewerToolStrip1.FindAction<ImageMeasureToolAction>();
            imageMeasureToolAction.Activated += new EventHandler(imageMeasureToolAction_Activated);
#endif

            MagnifierTool magnifierTool = new MagnifierTool();
            magnifierTool.ShowVisualTools = false;
            // create action, which allows to magnify of image region in image viewer
            MagnifierToolAction magnifierToolAction = new MagnifierToolAction(
                magnifierTool,
                DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_MAGNIFIER_TOOL,
                DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_MAGNIFIER,
                DemosResourcesManager.GetResourceAsBitmap("DemosCommonCode.Imaging.VisualToolsToolStrip.VisualTools.ZoomVisualTools.Resources.MagnifierTool.png"));

            _dicomViewerTool = new DicomViewerTool();
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool = new DicomAnnotatedViewerTool(
                   _dicomViewerTool,
                   new DicomAnnotationTool(),
                   (Vintasoft.Imaging.Annotation.Measurements.ImageMeasureTool)imageMeasureToolAction.VisualTool);
            _dicomAnnotatedViewerTool.InteractionMode = DicomAnnotatedViewerToolInteractionMode.None;

            _interactionAreaAppearanceManager = new AnnotationInteractionAreaAppearanceManager();
            _interactionAreaAppearanceManager.VisualTool = _dicomAnnotatedViewerTool.DicomAnnotationTool;
#endif

            // add visual tools to tool strip
#if REMOVE_ANNOTATION_PLUGIN
            dicomAnnotatedViewerToolStrip1.DicomAnnotatedViewerTool = _dicomViewerTool;
#else
            dicomAnnotatedViewerToolStrip1.DicomAnnotatedViewerTool = _dicomAnnotatedViewerTool;
#endif
            dicomAnnotatedViewerToolStrip1.AddVisualToolAction(magnifierToolAction);
#if REMOVE_ANNOTATION_PLUGIN
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool = _dicomViewerTool;
#else
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool = _dicomAnnotatedViewerTool;
#endif

            magnifierToolAction.Activated += new EventHandler(magnifierToolAction_Activated);

            _dicomViewerTool.NavigateBySeries = true;
            _dicomViewerTool.ScrollProperties.IsVisible = true;
            _dicomViewerTool.ScrollProperties.Anchor = AnchorType.Left;

            DemosTools.SetTestFilesFolder(openDicomFileDialog);

#if REMOVE_ANNOTATION_PLUGIN
            CompositeVisualTool compositeTool = new CompositeVisualTool(_dicomViewerTool, magnifierTool);
            compositeTool.ActiveTool = _dicomViewerTool;
            imageViewer1.VisualTool = compositeTool;
#else
            CompositeVisualTool compositeTool = new CompositeVisualTool(_dicomAnnotatedViewerTool, magnifierTool);
            compositeTool.ActiveTool = _dicomAnnotatedViewerTool;
            imageViewer1.VisualTool = compositeTool;
#endif
            annotationsToolStrip1.Viewer = imageViewer1;
            imageViewer1.IsFastScrollingEnabled = false;
            imageViewer1.ImageDecodingSettings = (DecodingSettings)_dicomFrameDecodingSettings.Clone();

            DicomSrRenderingSettings dicomSrRenderingSettings = new DicomSrRenderingSettings();
            dicomSrRenderingSettings.BackgroundColor = VintasoftColor.Black;
            dicomSrRenderingSettings.ReportHeaderTextColor = VintasoftColor.White;
            dicomSrRenderingSettings.ItemTextColor = VintasoftColor.White;
            imageViewer1.ImageRenderingSettings = dicomSrRenderingSettings;

            dicomViewerToolInteractionButtonToolStrip1.Tool = _dicomViewerTool;

            // init DICOM annotation tool
            InitDicomAnnotationTool();

            _dicomViewerTool.DicomImageVoiLutChanged +=
                new EventHandler<VoiLutChangedEventArgs>(dicomViewerTool_DicomImageVoiLutChanged);

            SubscribeToImageCollectionEvents(imageViewer1.Images);

            // init unit of measures for rulers
            InitUnitOfMeasuresForRulers();

            _defaultVoiLutToolStripMenuItem = new ToolStripMenuItem(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DEFAULT_VOI_LUT);
            _defaultVoiLutToolStripMenuItem.Click += new EventHandler(voiLutMenuItem_Click);

            this.Text = "VintaSoft DICOM Viewer Demo v" + ImagingGlobalSettings.ProductVersion;

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
                    return DicomFile.GetFileAssociatedWithImage(image);

                return null;
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
            CloseDicomFiles();
        }

        #endregion


        #region 'File' menu

        /// <summary>
        /// Handles the Click event of addFilesToolStripMenuItem object.
        /// </summary>
        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDicomFiles();
        }

        /// <summary>
        /// Handles the Click event of openDirectoryToolStripMenuItem object.
        /// </summary>
        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDirectory();
        }

        /// <summary>
        /// Handles the Click event of saveImagesAsToolStripMenuItem object.
        /// </summary>
        private void saveImagesAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageCollection images = GetSeriesImages();
            SubscribeToImageCollectionEvents(images);
            _disposeImageCollectionAfterSave = false;
            bool useMultipageEncoderOnly = images.Count > 1;

            CodecsFileFilters.SetSaveFileDialogFilter(saveFileDialog1, useMultipageEncoderOnly, true);
            // if file is selected in "Save file" dialog
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
#if !REMOVE_ANNOTATION_PLUGIN
                DicomAnnotationTool annotationTool = _dicomAnnotatedViewerTool.DicomAnnotationTool;
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
                        _disposeImageCollectionAfterSave = true;
                    }
                }
#endif

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
        /// Handles the Click event of saveDisplayedImageToolStripMenuItem object.
        /// </summary>
        private void saveDisplayedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodecsFileFilters.SetSaveFileDialogFilter(saveFileDialog1, false, false);
            // if file is selected in "Save file" dialog
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string saveFilename = Path.GetFullPath(saveFileDialog1.FileName);

                using (EncoderBase imageEncoder = GetEncoder(saveFilename))
                {
                    if (imageEncoder != null)
                    {
                        VintasoftImage image = _dicomViewerTool.GetDisplayedImage();
                        if (image == null)
                            image = imageViewer1.Image;

                        IsFileSaving = true;
                        try
                        {
                            // save images to a file
                            image.Save(saveFilename, imageEncoder);
                        }
                        catch (Exception ex)
                        {
                            DemosTools.ShowErrorMessage(ex);
                        }
                        finally
                        {
                            IsFileSaving = false;

                            if (image != imageViewer1.Image)
                                image.Dispose();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of burnAndSaveToDICOMFileToolStripMenuItem object.
        /// </summary>
        private void burnAndSaveToDICOMFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            saveFileDialog1.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DICOM_FILESDCM;

            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // destination file path
                    string destFilePath = Path.GetFullPath(saveFileDialog1.FileName);

                    bool needUpdateFocusedSeries = false;

                    // get file path for focused image
                    string focusedImageFilePath = Path.GetFullPath(imageViewer1.Image.SourceInfo.Filename);
                    // if modified DICOM image must be saved to the source DICOM file
                    if (focusedImageFilePath == destFilePath)
                    {
                        // specify that focused series must be updated
                        needUpdateFocusedSeries = true;
                    }
                    // if modified DICOM image must be saved to a new DICOM file
                    else
                    {
                        // for each DICOM image in image viewer
                        foreach (VintasoftImage image in imageViewer1.Images)
                        {
                            // get file path for current image
                            string currentImageFilePath = Path.GetFullPath(image.SourceInfo.Filename);
                            // if DICOM image must be saved to the source DICOM file
                            if (currentImageFilePath == destFilePath)
                            {
                                throw new InvalidOperationException(
                                    DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_DICOM_IMAGES_CAN_CAN_BE_SAVED_TO_THE_SOURCE_FILE_IF_SOURCE_FILE_IS_FOCUSED_IN_VIEWER_OR_TO_A_NEW_FILE);
                            }
                        }
                    }

                    // burn annotations and measurements on DICOM images and save DICOM images to a file
                    _dicomAnnotatedViewerTool.BurnAndSaveToDicomFile(destFilePath);

                    // if focused series must be updated
                    if (needUpdateFocusedSeries)
                    {
                        // get identifier of focused image
                        string focusedImageId = dicomSeriesManagerControl1.SeriesManager.GetImageIdentifierByImage(imageViewer1.Image);
                        // get series identifier for focused image
                        string focusedImageSeriesId = dicomSeriesManagerControl1.SeriesManager.GetSeriesIdentifierByImage(imageViewer1.Image);
                        // get series images by series identifier
                        VintasoftImage[] seriesImages = dicomSeriesManagerControl1.SeriesManager.GetSeriesImages(focusedImageSeriesId);

                        // remove series images from image viewer
                        imageViewer1.Images.RemoveRange(seriesImages);
                        // for each series image
                        foreach (VintasoftImage imageForDispose in seriesImages)
                            // dispose image
                            imageForDispose.Dispose();

                        // load series images from file (we saved series images to the file in code above)
                        imageViewer1.Images.Add(destFilePath);

                        // get focused image by image identifier
                        VintasoftImage focusedImage = dicomSeriesManagerControl1.SeriesManager.GetImage(focusedImageId);
                        // if focused image is found
                        if (focusedImage != null)
                        {
                            // find image index in image viewer
                            int index = imageViewer1.Images.IndexOf(focusedImage);
                            // if index is found
                            if (index != -1)
                            {
                                // set focused image in image viewer
                                imageViewer1.FocusedIndex = index;
                            }
                        }

                        // update UI
                        UpdateUI();
                        UpdateUIWithInformationAboutDicomFile();
                    }
                }
            }
            catch (Exception ex)
            {
                DemosTools.ShowErrorMessage(ex);
            }
#endif
        }

        /// <summary>
        /// Handles the Click event of saveViewerScreenshotToolStripMenuItem object.
        /// </summary>
        private void saveViewerScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get image of image viewer
            using (VintasoftImage image = imageViewer1.RenderViewerImage())
            {
                // save image to a file
                SaveImageFileForm.SaveImageToFile(image, ImagingEncoderFactory.Default);
            }
        }

        /// <summary>
        /// Handles the Click event of closeFilesToolStripMenuItem object.
        /// </summary>
        private void closeFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // close DICOM file
            CloseDicomFiles();
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
                toolStripPanel1.Visible = false;
                menuStrip1.Visible = false;
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
                toolStripPanel1.Visible = true;
                menuStrip1.Visible = true;
                statusStrip1.Visible = true;

                TopMost = false;
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                if (WindowState != _windowState)
                    WindowState = _windowState;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of showViewerScrollbarsToolStripMenuItem object.
        /// </summary>
        private void showViewerScrollbarsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            // show/hide scrollbars in image viewer
            imageViewer1.AutoScroll = showViewerScrollbarsToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of showBrowseScrollbarToolStripMenuItem object.
        /// </summary>
        private void showBrowseScrollbarToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_dicomViewerTool != null)
                _dicomViewerTool.ScrollProperties.IsVisible = showBrowseScrollbarToolStripMenuItem.Checked;
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
            imageViewer1.ImageDecodingSettings = _dicomFrameDecodingSettings;
            _dicomViewerTool.Refresh();
            dicomSeriesManagerControl1.ClearThumbnailsCache();
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

                // invalidates images and visual tool
                imageViewer1.ImageDecodingSettings = _dicomFrameDecodingSettings;
                _dicomViewerTool.Refresh();
                dicomSeriesManagerControl1.ClearThumbnailsCache();
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
            _dicomViewerTool.IsTextOverlayVisible = showMetadataInViewerToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the Click event of textOverlaySettingsToolStripMenuItem object.
        /// </summary>
        private void textOverlaySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (DicomOverlaySettingEditorForm dlg = new DicomOverlaySettingEditorForm(OVERLAY_OWNER_NAME, _dicomViewerTool))
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                // show dialog
                dlg.ShowDialog(this);

                // set text overlay for DICOM viewer tool
                DicomOverlaySettingEditorForm.SetTextOverlay(OVERLAY_OWNER_NAME, _dicomViewerTool);
                // refresh the DICOM viewer tool
                _dicomViewerTool.Refresh();
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
            _dicomViewerTool.ShowRulers = showRulersInViewerToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the Click event of rulersColorToolStripMenuItem object.
        /// </summary>
        private void rulersColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // init dialog
            colorDialog1.Color = _dicomViewerTool.VerticalImageRuler.RulerPen.Color;
            // show dialog
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // update rulers
                _dicomViewerTool.VerticalImageRuler.RulerPen.Color = colorDialog1.Color;
                _dicomViewerTool.HorizontalImageRuler.RulerPen.Color = colorDialog1.Color;

                // refresh DICOM viewer tool
                _dicomViewerTool.Refresh();
            }
        }

        /// <summary>
        /// Handles the Click event of rulersUnitOfMeasureToolStripMenuItem object.
        /// </summary>
        private void rulersUnitOfMeasureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentRulersUnitOfMeasureMenuItem.Checked = false;
            _currentRulersUnitOfMeasureMenuItem = (ToolStripMenuItem)sender;
            _dicomViewerTool.RulersUnitOfMeasure =
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
                DicomFrameMetadata metadata = GetFocusedFrameMetadata();

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
            _dicomViewerTool.IsImageNegative = negativeImageToolStripMenuItem.Checked;
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
                _dicomViewerTool.DefaultDicomImageVoiLut;
            // if the default VOI LUT is equal to new VOI LUT
            if (defaultVoiLut.WindowCenter == e.WindowCenter &&
                defaultVoiLut.WindowWidth == e.WindowWidth)
            {
                // specify that DICOM viewer tool must use VOI LUT from DICOM image metadata for DICOM image
                _dicomViewerTool.AlwaysLoadVoiLutFromMetadataOfDicomFrame = true;
            }
            else
            {
                // specify that DICOM viewer tool must use the same VOI LUT for all DICOM images
                _dicomViewerTool.AlwaysLoadVoiLutFromMetadataOfDicomFrame = false;
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

            _dicomViewerTool.DicomImageVoiLutCenterDirection = DicomInteractionDirection.BottomToTop;
            _dicomViewerTool.DicomImageVoiLutWidthDirection = DicomInteractionDirection.LeftToRight;
        }

        /// <summary>
        /// Handles the Click event of widthHorizontalCenterVerticalToolStripMenuItem object.
        /// </summary>
        private void widthHorizontalCenterVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            widthHorizontalInvertedCenterVerticalToolStripMenuItem.Checked = false;
            widthHorizontalCenterVerticalToolStripMenuItem.Checked = true;
            widthVerticalCenterHorizontalToolStripMenuItem.Checked = false;

            _dicomViewerTool.DicomImageVoiLutCenterDirection = DicomInteractionDirection.BottomToTop;
            _dicomViewerTool.DicomImageVoiLutWidthDirection = DicomInteractionDirection.RightToLeft;
        }

        /// <summary>
        /// Handles the Click event of widthVerticalCenterHorizontalToolStripMenuItem object.
        /// </summary>
        private void widthVerticalCenterHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            widthHorizontalInvertedCenterVerticalToolStripMenuItem.Checked = false;
            widthHorizontalCenterVerticalToolStripMenuItem.Checked = false;
            widthVerticalCenterHorizontalToolStripMenuItem.Checked = true;

            _dicomViewerTool.DicomImageVoiLutCenterDirection = DicomInteractionDirection.RightToLeft;
            _dicomViewerTool.DicomImageVoiLutWidthDirection = DicomInteractionDirection.BottomToTop;
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


        #region Interaction Points

        /// <summary>
        /// Handles the Click event of interactionPointsAppearanceToolStripMenuItem object.
        /// </summary>
        private void interactionPointsAppearanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            // create  interaction area appearance manager form
            using (InteractionAreaAppearanceManagerForm dialog = new InteractionAreaAppearanceManagerForm())
            {
                dialog.InteractionAreaSettings = _interactionAreaAppearanceManager;
                dialog.ShowDialog();
            }
#endif
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
            ImageCollection images = GetSeriesImages();
            SubscribeToImageCollectionEvents(images);
            _disposeImageCollectionAfterSave = false;


            // if file is selected in "Save file" dialog
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
#if !REMOVE_ANNOTATION_PLUGIN
                DicomAnnotationTool annotationTool = _dicomAnnotatedViewerTool.DicomAnnotationTool;
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
                        _disposeImageCollectionAfterSave = true;
                    }
                }
#endif

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

        #endregion


        #region 'Annotation' menu

        /// <summary>
        /// Handles the Click event of infoToolStripMenuItem object.
        /// </summary>
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            using (AnnotationsInfoForm dialog = new AnnotationsInfoForm(_dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController))
            {
                dialog.Owner = this;
                dialog.ShowDialog();
            }
#endif
        }

        /// <summary>
        /// Handles the Click event of noneToolStripMenuItem object.
        /// </summary>
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;
#endif
        }

        /// <summary>
        /// Handles the Click event of viewToolStripMenuItem object.
        /// </summary>
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.View;
#endif
        }

        /// <summary>
        /// Handles the Click event of authorToolStripMenuItem object.
        /// </summary>
        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.Author;
#endif
        }

        /// <summary>
        /// Handles the annotationEraserToolStripMenuItem_Click event of interactionMode object.
        /// </summary>
        private void interactionMode_annotationEraserToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.AnnotationEraser;
#endif
        }

        /// <summary>
        /// Handles the Click event of loadToolStripMenuItem object.
        /// </summary>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
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
                        _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController.AddAnnotationDataSet(presentationStateFile.Annotations);
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
#endif
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
#if !REMOVE_ANNOTATION_PLUGIN
            if (_isAnnotationsLoadedForCurrentFrame)
            {
                DicomAnnotationCodec codec = new DicomAnnotationCodec();
                DicomAnnotationDataCollection collection = (DicomAnnotationDataCollection)
                    _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController.GetAnnotations(imageViewer1.Image);
                codec.Encode(PresentationStateFile.Annotations, collection);
                PresentationStateFile.SaveChanges();
            }
            else
            {
                _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController.UpdateAnnotationDataSets();
                PresentationStateFile.SaveChanges();
            }
            MessageBox.Show(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESENTATION_STATE_FILE_IS_SAVED);
#endif
        }

        /// <summary>
        /// Handles the Click event of presentationStatesSaveToToolStripMenuItem object.
        /// </summary>
        private void presentationStatesSaveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            string dicomFilePath = imageViewer1.Image.SourceInfo.Filename;
            saveDicomAnnotationsFileDialog.FileName = Path.GetFileNameWithoutExtension(dicomFilePath) + ".pre";
            saveDicomAnnotationsFileDialog.Filter = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PRESENTATION_STATE_FILEPREPRE;
            saveDicomAnnotationsFileDialog.FilterIndex = 1;
            // show save dialog
            if (saveDicomAnnotationsFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _dicomAnnotatedViewerTool.DicomAnnotationTool.CancelAnnotationBuilding();

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
#endif
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
#if !REMOVE_ANNOTATION_PLUGIN
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
                        AnnotationDataCollection annotations = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController.GetAnnotations(imageViewer1.Image);
                        //
                        annotationFormatter.Serialize(fs, annotations);
                    }
                }
                catch (Exception ex)
                {
                    DemosTools.ShowErrorMessage(ex);
                }
            }
#endif
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
#if !REMOVE_ANNOTATION_PLUGIN
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
                        AnnotationDataCollection annotations = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController.GetAnnotations(
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
#endif
        }

        /// <summary>
        /// Handles the Click event of addToolStripMenuItem object.
        /// </summary>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (_dicomAnnotatedViewerTool.DicomAnnotationTool.FocusedAnnotationView != null &&
                _dicomAnnotatedViewerTool.DicomAnnotationTool.FocusedAnnotationView.InteractionController ==
                _dicomAnnotatedViewerTool.DicomAnnotationTool.FocusedAnnotationView.Builder)
                _dicomAnnotatedViewerTool.DicomAnnotationTool.CancelAnnotationBuilding();
            annotationsToolStrip1.BuildAnnotation(item.Text);
#endif
        }

        /// <summary>
        /// Handles the Click event of propertiesToolStripMenuItem object.
        /// </summary>
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            using (PropertyGridForm form = new PropertyGridForm(
                _dicomAnnotatedViewerTool.DicomAnnotationTool.FocusedAnnotationView,
                DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_ANNOTATION_PROPERTIES))
            {
                form.ShowDialog();
            }
#endif
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
            }

            UpdateUIWithInformationAboutDicomFile();

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
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.InteractionMode = _previousDicomViewerToolInteractionMode;
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode = _previousDicomAnnotationToolInteractionMode;
#endif
        }

        /// <summary>
        /// Handles the Deactivated event of noneAction object.
        /// </summary>
        private void noneAction_Deactivated(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            // save the DICOM viewer tool state

            _previousDicomViewerToolInteractionMode = _dicomAnnotatedViewerTool.InteractionMode;
            _previousDicomAnnotationToolInteractionMode = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode;
#endif
        }

        /// <summary>
        /// Handles the Activated event of imageMeasureToolAction object.
        /// </summary>
        private void imageMeasureToolAction_Activated(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            _isVisualToolChanging = true;
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool = dicomAnnotatedViewerToolStrip1.DicomAnnotatedViewerTool;
            _dicomAnnotatedViewerTool.ActiveTool = null;
            _isVisualToolChanging = false;
#endif
        }

        /// <summary>
        /// Handles the Activated event of magnifierToolAction object.
        /// </summary>
        private void magnifierToolAction_Activated(object sender, EventArgs e)
        {
            _isVisualToolChanging = true;
            dicomAnnotatedViewerToolStrip1.MainVisualTool.ActiveTool =
                dicomAnnotatedViewerToolStrip1.MainVisualTool.FindVisualTool<MagnifierTool>();
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;
#endif
            _isVisualToolChanging = false;
        }

        #endregion


        #region DICOM Series Manager Control

        /// <summary>
        /// Handles the AddedFileCountChanged event of dicomSeriesManagerControl1 object.
        /// </summary>
        private void dicomSeriesManagerControl1_AddedFileCountChanged(object sender, EventArgs e)
        {
            DicomSeriesManagerControl control = (DicomSeriesManagerControl)sender;

            // if DICOM files loaded
            if (control.AddedFileCount == control.AddingFileCount)
            {
                // hide action label and progress bar
                progressBar1.Visible = false;
                progressBar1.Maximum = 0;

                if (!_isFormClosing)
                {
                    // update the UI
                    IsDicomFileOpening = false;
                }
            }
            else
            {
                // if DICOM files loading started
                if (control.AddingFileCount != progressBar1.Maximum)
                {
                    progressBar1.Visible = true;
                    progressBar1.Maximum = control.AddingFileCount;
                    // update the UI
                    IsDicomFileOpening = true;
                }

                progressBar1.Value = control.AddedFileCount;
            }
        }

        /// <summary>
        /// Handles the AddFilesException event of dicomSeriesManagerControl1 object.
        /// </summary>
        private void dicomSeriesManagerControl1_AddFilesException(object sender, ImageSourceExceptionEventArgs e)
        {
            if (e.Exception.Message != "Image file does not contain pages (Dicom).")
                DemosTools.ShowErrorMessage(e.SourceFilename + ":" + Environment.NewLine + e.Exception.Message);
        }

        #endregion


        #region Annotations UI

        /// <summary>
        /// Handles the SelectedIndexChanged event of annotationInteractionModeToolStripComboBox object.
        /// </summary>
        private void annotationInteractionModeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode =
            (AnnotationInteractionMode)annotationInteractionModeToolStripComboBox.SelectedItem;
#endif
        }

#if !REMOVE_ANNOTATION_PLUGIN
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
            interactionMode_annotationEraserToolStripMenuItem.Checked = false;

            AnnotationInteractionMode annotationInteractionMode = e.NewValue;
            switch (annotationInteractionMode)
            {
                case AnnotationInteractionMode.None:
                    interactionMode_noneToolStripMenuItem.Checked = true;
                    break;

                case AnnotationInteractionMode.View:
                    interactionMode_viewToolStripMenuItem.Checked = true;
                    break;

                case AnnotationInteractionMode.Author:
                    interactionMode_authorToolStripMenuItem.Checked = true;
                    break;

                case AnnotationInteractionMode.AnnotationEraser:
                    interactionMode_annotationEraserToolStripMenuItem.Checked = true;
                    break;
            }

            annotationInteractionModeToolStripComboBox.SelectedItem = annotationInteractionMode;


            // update the UI
            UpdateUI();
        }
#endif

        #endregion


        #region Annotation visual tool

#if !REMOVE_ANNOTATION_PLUGIN
        /// <summary>
        /// Handles the FocusedAnnotationViewChanged event of annotationTool object.
        /// </summary>
        private void annotationTool_FocusedAnnotationViewChanged(object sender, AnnotationViewChangedEventArgs e)
        {
            if (e.OldValue != null)
                e.OldValue.Data.PropertyChanging -= new EventHandler<ObjectPropertyChangingEventArgs>(AnnotationdData_PropertyChanging);
            if (e.NewValue != null)
                e.NewValue.Data.PropertyChanging += new EventHandler<ObjectPropertyChangingEventArgs>(AnnotationdData_PropertyChanging);

            // update the UI
            UpdateUI();
        }

        /// <summary>
        /// Handles the Changed event of SelectedAnnotations object.
        /// </summary>
        private void SelectedAnnotations_Changed(object sender, EventArgs e)
        {
            // update the UI
            UpdateUI();
        }
#endif

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
                _dicomViewerTool.DicomImageVoiLut = _toolStripItemToVoiLut[_currentVoiLutMenuItem];
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

            if (_disposeImageCollectionAfterSave)
            {
                images.ClearAndDisposeItems();
                _disposeImageCollectionAfterSave = false;
            }

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

#if REMOVE_ANNOTATION_PLUGIN
            if (_dicomViewerTool == null)
                return;
#else
            if (_dicomAnnotatedViewerTool == null)
                return;
#endif

            bool hasImages = imageViewer1.Images.Count > 0;
            bool isDicomFileLoaded = hasImages || DicomFile != null;
            bool isDicomFileOpening = _isDicomFileOpening;
            bool isAnnotationsFileLoaded = PresentationStateFile != null;
            bool isFileSaving = _isFileSaving;
            ImageCollection seriesImages = GetSeriesImages();
            bool isMultipageFile = seriesImages.Count > 1;
            bool isAnimationStarted = IsAnimationStarted;
            bool isImageSelected = imageViewer1.Image != null;
            bool isAnnotationEmpty = true;
            bool isImageNegative = _dicomViewerTool.IsImageNegative;
#if !REMOVE_ANNOTATION_PLUGIN
            if (isImageSelected)
            {
                isAnnotationEmpty = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController[imageViewer1.FocusedIndex].Count <= 0;
            }
#endif
            bool isAnnotationDataControllerEmpty = true;
#if !REMOVE_ANNOTATION_PLUGIN
            if (_dicomAnnotatedViewerTool.DicomAnnotationTool.ImageViewer != null)
            {
                DicomAnnotationDataController dataController = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController;
                foreach (VintasoftImage image in seriesImages)
                {
                    if (dataController.GetAnnotations(image).Count > 0)
                    {
                        isAnnotationDataControllerEmpty = false;
                        break;
                    }
                }
            }
#endif
            bool isInteractionModeAuthor = false;
            bool isAnnotationFocused = false;
#if !REMOVE_ANNOTATION_PLUGIN
            isInteractionModeAuthor = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode == AnnotationInteractionMode.Author;
            isAnnotationFocused = _dicomAnnotatedViewerTool.DicomAnnotationTool.FocusedAnnotationData != null;
#endif

            bool hasOverlayImages = false;
            bool isMonochromeImage = false;

            DicomFrameMetadata metadata = GetFocusedFrameMetadata();
            if (metadata != null)
            {
                hasOverlayImages = metadata.OverlayImages.Length > 0;
                isMonochromeImage = metadata.ColorSpace == DicomImageColorSpaceType.Monochrome1 ||
                                    metadata.ColorSpace == DicomImageColorSpaceType.Monochrome2;
            }

            // 'File' menu
            //
            addFilesToolStripMenuItem.Enabled = !isDicomFileOpening && !isFileSaving;
            saveImagesAsToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && hasImages;
            saveImageAsCurrentVOILUTToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && hasImages;
            burnAndSaveToDICOMFileToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && hasImages;
            saveViewerScreenshotToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && !isFileSaving && hasImages;
            closeFilesToolStripMenuItem.Enabled = isDicomFileLoaded && !isFileSaving;

            // 'View' menu
            //
            showOverlayImagesToolStripMenuItem.Enabled = isDicomFileLoaded && !isDicomFileOpening && hasOverlayImages && !isFileSaving;
            overlayColorToolStripMenuItem.Enabled = showOverlayImagesToolStripMenuItem.Enabled;
            showMetadataInViewerToolStripMenuItem.Enabled = !isAnimationStarted;
            showRulersInViewerToolStripMenuItem.Enabled = !isAnimationStarted;
            rulersUnitOfMeasureToolStripMenuItem.Enabled = !isAnimationStarted;
            voiLutToolStripMenuItem.Enabled = !isAnimationStarted && isMonochromeImage;
            negativeImageToolStripMenuItem.Checked = isImageNegative;

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
            dicomAnnotatedViewerToolStrip1.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;
            annotationInteractionModeToolStrip.Enabled = !isDicomFileOpening && !isFileSaving && isDicomFileLoaded;

            propertiesToolStripMenuItem.Enabled = isInteractionModeAuthor && isAnnotationFocused;
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
                _voiLutParamsForm = new VoiLutParamsForm(this, _dicomViewerTool);
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
        /// Adds a DICOM files.
        /// </summary>
        private void AddDicomFiles()
        {
            if (openDicomFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openDicomFileDialog.FileNames.Length > 0)
                {
                    // add DICOM files to the DICOM series
                    AddDicomFiles(openDicomFileDialog.FileNames);
                }
            }
        }

        /// <summary>
        /// Adds the DICOM files.
        /// </summary>
        /// <param name="filesPath">Files path.</param>
        private void AddDicomFiles(params string[] filesPath)
        {
            dicomSeriesManagerControl1.AddFiles(filesPath, false);
        }

        /// <summary>
        /// Opens a directory.
        /// </summary>
        private void OpenDirectory()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                AddDicomFilesFromDirectory(folderBrowserDialog1.SelectedPath);
            }
        }

        /// <summary>
        /// Adds the DICOM files from directory.
        /// </summary>
        /// <param name="filesPath">Files path.</param>
        private void AddDicomFilesFromDirectory(string filesPath)
        {
            dicomSeriesManagerControl1.AddDirectory(filesPath, true, false);
        }

        /// <summary>
        /// Closes series of DICOM frames.
        /// </summary>
        private void CloseDicomFiles()
        {
            if (imageViewer1.Images.Count != 0)
                CloseAllPresentationStateFiles();

            // if animation is enabled
            if (IsAnimationStarted)
            {
                // stop animation
                IsAnimationStarted = false;
            }

            // clear image collection of image viewer and dispose all images
            dicomSeriesManagerControl1.CloseAllSeries();

            // update the UI
            UpdateUI();
            UpdateUIWithInformationAboutDicomFile();
        }

        #endregion


        #region Annotations

#if !REMOVE_ANNOTATION_PLUGIN
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
            DicomAnnotationDataController controller = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController;
            // for each image
            foreach (VintasoftImage image in imageViewer1.Images)
            {
                DicomFile currentDicomFile = DicomFile.GetFileAssociatedWithImage(image);

                if (currentDicomFile == dicomFile)
                {
                    // get annotations of image
                    DicomAnnotationDataCollection annotations =
                        (DicomAnnotationDataCollection)controller.GetAnnotations(image);

                    // if annotation collection is not empty
                    if (annotations.Count > 0)
                        // add annotations
                        result.Add(annotations);
                }
            }

            return result.ToArray();
        }
#endif

        /// <summary>
        /// Loads the annotation from binary or XMP packet.
        /// </summary>
        private void LoadAnnotationFromBinaryOrXmpFormat(bool binaryFormat)
        {
#if !REMOVE_ANNOTATION_PLUGIN
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
                        AnnotationDataCollection annotations = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataCollection;
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
#endif
        }

        #endregion


        #region Annotation visual tool

        /// <summary>
        /// The annotation property is changing.
        /// </summary>
        private void AnnotationdData_PropertyChanging(object sender, ObjectPropertyChangingEventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            if (e.PropertyName == "UnitOfMeasure")
            {
                if (_isAnnotationPropertyChanging)
                    return;

                _isAnnotationPropertyChanging = true;
                DicomAnnotationData data = (DicomAnnotationData)sender;

                data.ChangeUnitOfMeasure((DicomUnitOfMeasure)e.NewValue, imageViewer1.Image);
                _isAnnotationPropertyChanging = false;
            }
#endif
        }

        #endregion


        #region Presentation state file               

#if !REMOVE_ANNOTATION_PLUGIN
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
#endif

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
            DicomFile[] dicomFiles = DicomFile.GetFilesAssociatedWithImages(imageViewer1.Images.ToArray());
            foreach (DicomFile dicomFile in dicomFiles)
                ClosePresentationStateFileOfFile(dicomFile);
        }

        /// <summary>
        /// Closes the DICOM presentation state file of specified DICOM file.
        /// </summary>
        /// <param name="dicomFile">The DICOM file.</param>
        private void ClosePresentationStateFileOfFile(DicomFile dicomFile)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            // get the presentation state file of source DICOM file
            DicomFile presentationStateFile = PresentationStateFileController.GetPresentationStateFile(dicomFile);

            if (presentationStateFile == null)
                return;

            // get controller of DicomAnnotationTool
            DicomAnnotationDataController controller = _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationDataController;

            // remove annotations from controller
            controller.RemoveAnnotationDataSet(presentationStateFile.Annotations);

            // close the presentation state file of source DICOM file
            PresentationStateFileController.ClosePresentationStateFile(dicomFile);
#endif
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

            _toolStripItemToVoiLut.Add(
                _defaultVoiLutToolStripMenuItem,
                new DicomImageVoiLookupTable(double.NaN, double.NaN));

            DicomFrameMetadata metadata = GetFocusedFrameMetadata();
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
                DicomPageMetadata metadata = GetFocusedPageMetadata();
                // if DICOM image does not have metadata
                if (metadata == null)
                    // get metadata of DICOM file
                    metadata = new DicomPageMetadata(DicomFile);
                dlg.RootMetadataNode = metadata;

                // show the dialog
                dlg.ShowDialog();

                // if image viewer has image
                if (imageViewer1.Image != null)
                {
                    // update the UI with information about DICOM file
                    UpdateUIWithInformationAboutDicomFile();
                    // refresh the DICOM viewer tool
                    _dicomViewerTool.Refresh();
                }

                UpdateUI();
            }
        }

        /// <summary>
        /// Returns the metadata of focused image.
        /// </summary>
        private DicomPageMetadata GetFocusedPageMetadata()
        {
            if (imageViewer1.Image == null)
                return null;

            DicomPageMetadata metadata = imageViewer1.Image.Metadata.MetadataTree as DicomPageMetadata;

            return metadata;
        }

        /// <summary>
        /// Returns the metadata of focused image.
        /// </summary>
        private DicomFrameMetadata GetFocusedFrameMetadata()
        {
            return GetFocusedPageMetadata() as DicomFrameMetadata;
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
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.Enabled = false;
#endif
            _dicomViewerTool.IsTextOverlayVisible = false;
            _dicomViewerTool.ShowRulers = false;
            // disable tool strip
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
            }

            // enable tool strip
            annotationsToolStrip1.Enabled = true;
            annotationInteractionModeToolStrip.Enabled = true;
            dicomAnnotatedViewerToolStrip1.Enabled = true;
            dicomAnnotatedViewerToolStrip1.FindAction<MagnifierToolAction>().VisualTool.Enabled = true;
            // enable visual tool
            _dicomViewerTool.IsTextOverlayVisible = showMetadataInViewerToolStripMenuItem.Checked;
            _dicomViewerTool.ShowRulers = showRulersInViewerToolStripMenuItem.Checked;
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.Enabled = true;
#endif
        }

        /// <summary>
        /// Animation thread.
        /// </summary>
        private void AnimationMethod()
        {
            Thread currentThread = Thread.CurrentThread;
            _currentAnimatedFrameIndex = imageViewer1.FocusedIndex;

            VintasoftImage[] seriesImages = dicomSeriesManagerControl1.SeriesManager.GetSeriesImages(
                dicomSeriesManagerControl1.SeriesManager.GetSeriesIdentifierByImage(imageViewer1.Image));
            int index = Array.IndexOf(seriesImages, imageViewer1.Image);
            int count = seriesImages.Length;

            for (; index < count || _isAnimationCycled;)
            {
                if (_animationThread != currentThread)
                    break;

                _isFocusedIndexChanging = true;
                _currentAnimatedFrameIndex = imageViewer1.Images.IndexOf(seriesImages[index]);
                // change focused image in image viewer
                imageViewer1.SetFocusedIndexSync(_currentAnimatedFrameIndex);
                _isFocusedIndexChanging = false;
                Thread.Sleep(_animationDelay);

                index++;
                if (_isAnimationCycled && index >= count)
                    index = 0;
            }

            if (index == 0)
                _currentAnimatedFrameIndex = 0;
            else
                _currentAnimatedFrameIndex = imageViewer1.Images.IndexOf(seriesImages[index - 1]);
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

#if !REMOVE_ANNOTATION_PLUGIN
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
#endif

        #endregion


        #region View Rotation

        /// <summary>
        /// Rotates images in both annotation viewer and thumbnail viewer by 90 degrees clockwise.
        /// </summary>
        private void RotateViewClockwise()
        {
            imageViewer1.RotateViewClockwise();
        }

        /// <summary>
        /// Rotates images in both annotation viewer and thumbnail viewer by 90 degrees counterclockwise.
        /// </summary>
        private void RotateViewCounterClockwise()
        {
            imageViewer1.RotateViewCounterClockwise();
        }

        #endregion


        #region Init

        /// <summary>
        /// Initializes the DICOM annotation tool.
        /// </summary>
        private void InitDicomAnnotationTool()
        {
#if !REMOVE_ANNOTATION_PLUGIN
            _dicomAnnotatedViewerTool.DicomAnnotationTool.MultiSelect = false;
            _dicomAnnotatedViewerTool.DicomAnnotationTool.FocusedAnnotationViewChanged +=
                new EventHandler<AnnotationViewChangedEventArgs>(annotationTool_FocusedAnnotationViewChanged);
            _dicomAnnotatedViewerTool.DicomAnnotationTool.SelectedAnnotations.Changed +=
                new EventHandler(SelectedAnnotations_Changed);
            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionModeChanged +=
                new EventHandler<AnnotationInteractionModeChangedEventArgs>(annotationTool_AnnotationInteractionModeChanged);

            _dicomAnnotatedViewerTool.DicomAnnotationTool.AnnotationInteractionMode = AnnotationInteractionMode.None;

            annotationInteractionModeToolStripComboBox.Items.Add(AnnotationInteractionMode.None);
            annotationInteractionModeToolStripComboBox.Items.Add(AnnotationInteractionMode.View);
            annotationInteractionModeToolStripComboBox.Items.Add(AnnotationInteractionMode.Author);
            annotationInteractionModeToolStripComboBox.Items.Add(AnnotationInteractionMode.AnnotationEraser);
            // set interaction mode to the View 
            annotationInteractionModeToolStripComboBox.SelectedItem = AnnotationInteractionMode.None;
#endif
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
                if (unit == _dicomViewerTool.RulersUnitOfMeasure)
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

                if (sender is ImageViewer)
                    // close the previously opened DICOM files
                    CloseDicomFiles();

                foreach (string filename in filenames)
                {
                    // if is directory
                    if (Directory.Exists(filename))
                        // add files from directory
                        AddDicomFilesFromDirectory(filename);
                    else
                        // add DICOM files to the DICOM series
                        AddDicomFiles(filename);
                }
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

        /// <summary>
        /// Returns images for DICOM series that contains focused image.
        /// </summary>
        /// <returns>
        /// The collection of DICOM images.
        /// </returns>
        private ImageCollection GetSeriesImages()
        {
            string seriesIdentifier = dicomSeriesManagerControl1.SeriesManager.GetSeriesIdentifierByImage(imageViewer1.Image);
            VintasoftImage[] seriesImages = dicomSeriesManagerControl1.SeriesManager.GetSeriesImages(seriesIdentifier);
            ImageCollection images = new ImageCollection();
            images.AddRange(seriesImages);
            return images;
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
