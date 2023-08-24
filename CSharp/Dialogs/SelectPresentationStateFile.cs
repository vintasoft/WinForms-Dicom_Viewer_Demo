using System;
using System.IO;
using System.Windows.Forms;


namespace DicomViewerDemo
{
    /// <summary>
    /// A form that allows to select DICOM presentation state file.
    /// </summary>
    public partial class SelectPresentationStateFile : Form
    {

        #region Fields

        /// <summary>
        /// Names of presentation state files.
        /// </summary>
        string[] _presentationStateFilenames = null;

        #endregion



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectPresentationStateFile"/> class.
        /// </summary>
        /// <param name="presentationStateFilenames">Names of presentation state files.</param>
        public SelectPresentationStateFile(string[] presentationStateFilenames)
        {
            InitializeComponent();

            _presentationStateFilenames = presentationStateFilenames;

            foreach (string filename in presentationStateFilenames)
            {
                filenamesComboBox.Items.Add(Path.GetFileName(filename));
            }

            if (_presentationStateFilenames.Length > 0)
                filenamesComboBox.SelectedIndex = 0;
        }

        #endregion



        #region Properties

        /// <summary>
        /// Gets the name of selected presentation state file.
        /// </summary>
        public string SelectedPresentationStateFilename
        {
            get
            {
                if (filenamesComboBox.SelectedIndex == -1)
                    return string.Empty;

                return _presentationStateFilenames[filenamesComboBox.SelectedIndex];
            }
            set
            {
                int index = Array.IndexOf(_presentationStateFilenames, value);

                filenamesComboBox.SelectedIndex = index;
            }
        }

        #endregion

    }
}
