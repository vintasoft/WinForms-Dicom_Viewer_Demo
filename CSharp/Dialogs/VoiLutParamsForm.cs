using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Vintasoft.Imaging;
using Vintasoft.Imaging.Codecs.Decoders;
using Vintasoft.Imaging.Codecs.ImageFiles.Dicom;
using Vintasoft.Imaging.Dicom.UI.VisualTools;
using Vintasoft.Imaging.ImageProcessing;
using Vintasoft.Imaging.Metadata;


namespace DicomViewerDemo
{
    /// <summary>
    /// A form that allows to select a predefine VOI LUT (value of interest lookup table)
    /// or specify VOI LUT using window center and windows width.
    /// </summary>
    public partial class VoiLutParamsForm : Form
    {

        #region Fields

        /// <summary>
        /// Visual tool of image viewer.
        /// </summary>
        DicomViewerTool _visualTool = null;

        /// <summary>
        /// An array of default VOI LUTs.
        /// </summary>
        List<DicomImageVoiLookupTable> _defaultVoiLuts = null;

        /// <summary>
        /// Determines that DICOM image VOI LUT is changing.
        /// </summary>
        bool _isVoiLutChanging = false;

        /// <summary>
        /// The dictionary: VOI LUT search mode => VOI LUT.
        /// </summary>
        Dictionary<VoiLutSearchMode, DicomImageVoiLookupTable> _searchModeToVoiLut =
            new Dictionary<VoiLutSearchMode, DicomImageVoiLookupTable>();

        #endregion



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoiLutParamsForm"/> class.
        /// </summary>
        /// <param name="owner">Owner of this form.</param>
        /// <param name="visualTool">Visual tool.</param>
        public VoiLutParamsForm(
            Form owner,
            DicomViewerTool visualTool)
        {
            InitializeComponent();

            Owner = owner;

            _visualTool = visualTool;

            visualTool.DicomImageVoiLutChanged += new EventHandler<VoiLutChangedEventArgs>(visualTool_DicomImageVoiLutChanged);


            windowWidthNumericUpDown.Maximum = int.MaxValue;

            windowCenterNumericUpDown.Minimum = int.MinValue;
            windowCenterNumericUpDown.Maximum = int.MaxValue;


            foreach (VoiLutSearchMode searchMode in Enum.GetValues(typeof(VoiLutSearchMode)))
                voiLutSearchMethodComboBox.Items.Add(searchMode);
            voiLutSearchMethodComboBox.SelectedItem = VoiLutSearchMode.Simple;

            UpdateUI();
        }

        #endregion



        #region Properties

        VintasoftImage _dicomFrame = null;
        /// <summary>
        /// Gets or sets the DICOM frame.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal VintasoftImage DicomFrame
        {
            get
            {
                return _dicomFrame;
            }
            set
            {
                if (!Enabled)
                    return;

                _dicomFrame = value;

                // update UI
                voiLutPanel.Enabled = _dicomFrame != null;

                _searchModeToVoiLut.Clear();

                if (_dicomFrame == null)
                {
                    voiLutsComboBox.SelectedIndex = -1;
                }
                else
                {
                    DicomFrameMetadata metadata = _dicomFrame.Metadata.MetadataTree as DicomFrameMetadata;
                    if (metadata != null)
                    {
                        // get information about VOI LUTs from DICOM frame
                        _defaultVoiLuts = new List<DicomImageVoiLookupTable>(metadata.AvailableVoiLuts);

                        for (int i = _defaultVoiLuts.Count - 1; i >= 0; i--)
                        {
                            // if VOI LUT is empty
                            if (_defaultVoiLuts[i].WindowCenter == 0 &&
                                _defaultVoiLuts[i].WindowWidth == 0)
                                _defaultVoiLuts.RemoveAt(i);
                        }

                        UpdateVoiLutsComboBox();

                        DicomImageVoiLookupTable voiLut = _visualTool.DicomImageVoiLut;

                        if (!double.IsNaN(voiLut.WindowCenter) && !double.IsNaN(voiLut.WindowWidth))
                        {
                            if (voiLut.WindowCenter != 0 || voiLut.WindowWidth != 0)
                                voiLutsComboBox.SelectedIndex = GetVoiLut(voiLut.WindowCenter, voiLut.WindowWidth);

                            windowCenterNumericUpDown.Value = (decimal)voiLut.WindowCenter;
                            windowWidthNumericUpDown.Value = (decimal)voiLut.WindowWidth;
                        }
                    }
                }

                UpdateUI();
            }
        }

        #endregion



        #region Methods

        #region PROTECTED

        /// <summary>
        /// Form is closing.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            _visualTool.DicomImageVoiLutChanged -= visualTool_DicomImageVoiLutChanged;

            base.OnClosed(e);
        }

        #endregion


        #region PRIVATE

        /// <summary>
        /// Updates the user interface of this form.
        /// </summary>
        private void UpdateUI()
        {
            // get the VOI LUT search mode
            VoiLutSearchMode voiLutSearchMode = (VoiLutSearchMode)voiLutSearchMethodComboBox.SelectedItem;

            // if VOI LUT is calculated already for VOI LUT search mode
            if (_searchModeToVoiLut.ContainsKey(voiLutSearchMode))
                // disable the "Calculate" button
                calculateVoiLutButton.Enabled = false;
            // if VOI LUT is NOT calculated for VOI LUT search mode
            else
                // enable the "Calculate" button
                calculateVoiLutButton.Enabled = true;
        }

        /// <summary>
        /// Sets VOI LUT in DICOM viewer tool.
        /// </summary>
        /// <param name="windowCenter">Window center of DICOM frame.</param>
        /// <param name="windowWidth">Window width of DICOM frame.</param>
        private void SetVoiLutInDicomViewerTool(
            double windowCenter,
            double windowWidth)
        {
            _visualTool.DicomImageVoiLut = new DicomImageVoiLookupTable(windowCenter, windowWidth);

            // change an index in a list of VOI LUTS
            voiLutsComboBox.SelectedIndex = GetVoiLut(windowCenter, windowWidth);
        }

        /// <summary>
        /// Returns index of VOI LUT if possible.
        /// </summary>
        private int GetVoiLut(double windowCenter, double windowWidth)
        {
            for (int i = 0; i < _defaultVoiLuts.Count; i++)
            {
                if (windowCenter == _defaultVoiLuts[i].WindowCenter &&
                    windowWidth == _defaultVoiLuts[i].WindowWidth)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Selected VOI LUT is changed.
        /// </summary>
        private void voiLutsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isVoiLutChanging)
                return;

            int index = voiLutsComboBox.SelectedIndex;
            if (index == -1)
                return;

            // get selected VOI LUT
            DicomImageVoiLookupTable voiLut = _defaultVoiLuts[index];

            // set new VOI LUT in DICOM viewer tool
            SetVoiLutInDicomViewerTool(voiLut.WindowCenter, voiLut.WindowWidth);
            windowCenterNumericUpDown.Value = (decimal)voiLut.WindowCenter;
            windowWidthNumericUpDown.Value = (decimal)voiLut.WindowWidth;
        }

        /// <summary>
        /// Value of window center is changed.
        /// </summary>
        private void windowCenterNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!_isVoiLutChanging)
            {
                SetVoiLutInDicomViewerTool(
                    (double)windowCenterNumericUpDown.Value,
                    _visualTool.DicomImageVoiLut.WindowWidth);
            }

        }

        /// <summary>
        /// Value of window width is changed.
        /// </summary>
        private void windowWidthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!_isVoiLutChanging)
            {
                SetVoiLutInDicomViewerTool(
                    _visualTool.DicomImageVoiLut.WindowCenter,
                    (double)windowWidthNumericUpDown.Value);
            }
        }

        /// <summary>
        /// VOI LUT is changed.
        /// </summary>
        private void visualTool_DicomImageVoiLutChanged(object sender, VoiLutChangedEventArgs e)
        {
            if (double.IsNaN(e.WindowCenter) || double.IsNaN(e.WindowWidth))
                return;

            _isVoiLutChanging = true;
            windowCenterNumericUpDown.Value = (decimal)e.WindowCenter;
            windowCenterNumericUpDown.Update();
            windowWidthNumericUpDown.Value = (decimal)e.WindowWidth;
            windowWidthNumericUpDown.Update();
            voiLutsComboBox.SelectedIndex = GetVoiLut(e.WindowCenter, e.WindowWidth);
            _isVoiLutChanging = false;
        }

        /// <summary>
        /// Calculates the VOI LUT of DICOM frame.
        /// </summary>
        private void calculateVoiLutButton_Click(object sender, EventArgs e)
        {
            // get the decoder for DICOM frame
            DicomDecoder dicomDecoder = _dicomFrame.SourceInfo.Decoder as DicomDecoder;
            // if decoder is NOT DICOM decoder
            if (dicomDecoder == null)
                // exit
                return;

            DicomFrameMetadata metadata = _dicomFrame.Metadata.MetadataTree as DicomFrameMetadata;
            DicomDecodingSettings decodingSettings = new DicomDecodingSettings(false, false, DicomImagePixelFormat.Source);
            RenderingSettings renderingSettings = new RenderingSettings(ImagingEnvironment.ScreenResolution);
            // get DICOM frame image
            using (VintasoftImage rawImage = dicomDecoder.GetImage(_dicomFrame.SourceInfo.PageIndex, decodingSettings, renderingSettings))
            {
                // create command for getting VOI LUT
                GetDefaultVoiLutCommand command = new GetDefaultVoiLutCommand();
                // set the VOI LUT search mode
                command.VoiLutSearchMode = (VoiLutSearchMode)voiLutSearchMethodComboBox.SelectedItem;
                // execute the command
                command.ExecuteInPlace(rawImage);

                // get the VOI LUT calculated by the processing command
                DicomImageVoiLookupTable resultVoiLut = command.ResultVoiLut;

                // create new VOI LUT, which is based on calculated VOI LUT
                DicomImageVoiLookupTable voiLut = new DicomImageVoiLookupTable(
                    resultVoiLut.WindowCenter, resultVoiLut.WindowWidth,
                    resultVoiLut.FunctionType,
                    string.Format(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_CALCULATED_ARG0, command.VoiLutSearchMode));

                // add VOI LUT to the list of default VOI LUTs
                _defaultVoiLuts.Add(voiLut);
                // save information about VOI LUT for search mode
                _searchModeToVoiLut.Add(command.VoiLutSearchMode, voiLut);

                // update combobox with VOI LUTs
                UpdateVoiLutsComboBox();
                // set VOI LUT in DICOM viewer tool
                SetVoiLutInDicomViewerTool(voiLut.WindowCenter, voiLut.WindowWidth);
                // update UI
                UpdateUI();
            }
        }

        /// <summary>
        /// Changes the VOI LUT search method.
        /// </summary>
        private void voiLutSearchMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Updates the VOI LUT combobox.
        /// </summary>
        private void UpdateVoiLutsComboBox()
        {
            voiLutsComboBox.BeginUpdate();
            // clear the combo box with information about VOI LUTs
            voiLutsComboBox.Items.Clear();

            // for each VOI LUT
            for (int i = 0; i < _defaultVoiLuts.Count; i++)
            {
                // get information about VOI LUT
                string explanation = _defaultVoiLuts[i].Explanation;
                // if information about VOI LUT is empty
                if (explanation == string.Empty)
                    // create standard information about VOI LUT
                    explanation = "VOI LUT " + (i + 1).ToString();
                // add information about VOI LUT to a combo box
                voiLutsComboBox.Items.Add(explanation);
            }

            voiLutsComboBox.EndUpdate();
        }

        #endregion

        #endregion
    }
}
