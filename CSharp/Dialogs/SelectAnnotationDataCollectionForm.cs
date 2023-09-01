using System;
using System.Windows.Forms;

using Vintasoft.Imaging.Annotation;
using Vintasoft.Imaging.Annotation.Dicom;
using Vintasoft.Imaging.Codecs.ImageFiles.Dicom;


namespace DicomViewerDemo
{
    /// <summary>
    /// A form that allows to select annotation collection from the list of annotation collections.
    /// </summary>
    public partial class SelectAnnotationDataCollectionForm : Form
    {

        #region Fields

        /// <summary>
        /// An array of annotation collections.
        /// </summary>
        DicomAnnotationDataCollection[] _collections = null;

        #endregion



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAnnotationDataCollectionForm"/> class.
        /// </summary>
        /// <param name="collections">An array of annotation collections.</param>
        public SelectAnnotationDataCollectionForm(params DicomAnnotationDataCollection[] collections)
        {
            InitializeComponent();

            _collections = collections;

            if (_collections.Length > 0)
            {
                for (int i = 0; i < _collections.Length; i++)
                {
                    string title = string.Format(DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_ANNOTATION_DATA_COLLECTION_NARG0, i + 1);

                    selectedAnnotationDataCollectionComboBox.Items.Add(title);
                }

                SelectedAnnotationDataCollection = _collections[0];
            }
        }

        #endregion



        #region Properties

        DicomAnnotationDataCollection _selectedAnnotationDataCollection = null;
        /// <summary>
        /// Gets or sets the selected annotation data collection.
        /// </summary>
        public DicomAnnotationDataCollection SelectedAnnotationDataCollection
        {
            get
            {
                return _selectedAnnotationDataCollection;
            }
            set
            {
                int collectionIndex = Array.IndexOf(_collections, value);
                if (collectionIndex == -1)
                    throw new ArgumentOutOfRangeException();

                if (_selectedAnnotationDataCollection != value)
                {
                    _selectedAnnotationDataCollection = value;

                    selectedAnnotationDataCollectionComboBox.SelectedIndex = collectionIndex;

                    DicomReferencedImage referencedImage = _selectedAnnotationDataCollection.ReferencedImage;
                    sopClassLabel.Text = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_UNKNOWN_ALT1;
                    if (referencedImage.SopClass != null)
                        sopClassLabel.Text = referencedImage.SopClass.Value;

                    sopInstanceLabel.Text = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_UNKNOWN_ALT2;
                    if (referencedImage.SopInstance != null)
                        sopInstanceLabel.Text = referencedImage.SopInstance.Value;

                    frameNumberLabel.Text = referencedImage.FrameNumber.ToString();

                    annoInfoListView.Items.Clear();
                    for (int i = 0; i < _selectedAnnotationDataCollection.Count; i++)
                    {
                        AnnotationData annotation = _selectedAnnotationDataCollection[i];
                        ListViewItem item = annoInfoListView.Items.Add(annotation.GetType().ToString());
                        item.SubItems.Add(annotation.Location.ToString());
                    }
                }
            }
        }

        #endregion



        #region Methods

        /// <summary>
        /// Focused annotation data collection is changed.
        /// </summary>
        private void selectedAnnotationDataCollectionComboBox_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            SelectedAnnotationDataCollection = _collections[selectedAnnotationDataCollectionComboBox.SelectedIndex];
        }

        #endregion

    }
}
