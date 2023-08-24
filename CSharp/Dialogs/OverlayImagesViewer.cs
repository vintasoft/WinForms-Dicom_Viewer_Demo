using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Vintasoft.Imaging;
using Vintasoft.Imaging.Codecs.Encoders;
using Vintasoft.Imaging.Codecs.ImageFiles.Dicom;
using Vintasoft.Imaging.Metadata;

using DemosCommonCode;


namespace DicomViewerDemo
{
    /// <summary>
    /// A form that allows to preview the overlay images of DICOM file.
    /// </summary>
    public partial class OverlayImagesViewer : Form
    {

        #region Fields

        /// <summary>
        /// Dictionary with information about overlay images.
        /// </summary>
        Dictionary<string, DicomOverlayImage> overlayImagesInfo = new Dictionary<string, DicomOverlayImage>();

        /// <summary>
        /// Dictionary with overlay images.
        /// </summary>
        Dictionary<string, VintasoftImage> overlayImages = new Dictionary<string, VintasoftImage>();

        #endregion



        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayImagesViewer"/> class.
        /// </summary>
        /// <param name="image">Source image.</param>
        public OverlayImagesViewer(VintasoftImage image)
        {
            InitializeComponent();

            DicomFrameMetadata metadata = image.Metadata.MetadataTree as DicomFrameMetadata;

            if (metadata == null)
            {
                MessageBox.Show("Current image is not DICOM frame.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            // load overlay images
            LoadOverlayImages(metadata);
        }

        #endregion



        #region Methods

        /// <summary> 
        /// Loads overlay images of DICOM file.
        /// </summary>
        /// <param name="file">Source image.</param>
        private void LoadOverlayImages(DicomFrameMetadata metadata)
        {
            // for each overlay image of DICOM page
            for (int overlayImageIndex = 0; overlayImageIndex < metadata.OverlayImages.Length; overlayImageIndex++)
            {
                // get the image name
                string imageName = string.Format("OverlayImage: {0}", overlayImageIndex + 1);

                bool error = false;
                // get the information about overlay image
                DicomOverlayImage imageInfo = metadata.OverlayImages[overlayImageIndex];
                VintasoftImage overlayImage = null;
                try
                {
                    // get the overlay image
                    overlayImage = imageInfo.GetOverlayImage();
                }
                catch (DicomFileException)
                {
                    error = true;
                }
                if (error)
                    continue;

                overlayImagesInfo.Add(imageName, imageInfo);
                overlayImages.Add(imageName, overlayImage);
                overlayImagesComboBox.Items.Add(imageName);
            }

            if (overlayImages.Count == 0)
                DemosTools.ShowErrorMessage("Overlay images are damaged.");

            if (overlayImagesComboBox.Items.Count > 0)
                overlayImagesComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Selected overlay image is changed in combo box.
        /// </summary>
        private void overlayImagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (overlayImagesComboBox.SelectedIndex >= 0)
            {
                string key = (string)overlayImagesComboBox.Items[overlayImagesComboBox.SelectedIndex];
                // load overlay image info
                propertyGrid.SelectedObject = overlayImagesInfo[key];
                // load overlay image
                imageViewer.Image = overlayImages[key];
            }
            else
            {
                propertyGrid.SelectedObject = null;
                imageViewer.Image = null;
            }
        }

        /// <summary>
        /// Saves the overlay image to an image file.
        /// </summary>
        private void saveAsImageButton_Click(object sender, EventArgs e)
        {
            if (overlayImagesComboBox.SelectedIndex >= 0)
            {
                string key = (string)overlayImagesComboBox.Items[overlayImagesComboBox.SelectedIndex];
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "TIFF Files|*.tif;*.tiff|JPEG Files|*.jpg;*.jpeg|PNG Files|.png";
                    saveFileDialog.DefaultExt = ".tiff";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string saveFilename = Path.GetFullPath(saveFileDialog.FileName);
                            EncoderBase encoder = null;
                            switch (Path.GetExtension(saveFilename).ToUpperInvariant())
                            {
                                case ".TIF":
                                case ".TIFF":
                                    encoder = new TiffEncoder();
                                    break;

                                case ".JPG":
                                case ".JPEG":
                                    encoder = new JpegEncoder();
                                    break;

                                case ".PNG":
                                    encoder = new PngEncoder();
                                    break;
                            }

                            overlayImages[key].Save(saveFilename, encoder);
                        }
                        catch (Exception ex)
                        {
                            DemosTools.ShowErrorMessage(ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of ImageViewerBackColorToolStripMenuItem object.
        /// </summary>
        private void imageViewerBackColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = imageViewer.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                imageViewer.BackColor = colorDialog1.Color;
        }

        #endregion

    }
}
