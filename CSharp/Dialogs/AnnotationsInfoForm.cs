using System.Windows.Forms;

using Vintasoft.Imaging.Annotation;
using Vintasoft.Imaging.Annotation.Dicom;


namespace DicomViewerDemo
{
    /// <summary>
    /// A form that allows to show information about annotations of DICOM file.
    /// </summary>
    public partial class AnnotationsInfoForm : Form
    {

        public AnnotationsInfoForm(DicomAnnotationDataController annotations)
        {
            InitializeComponent();

            for (int i = 0; i < annotations.Images.Count; i++)
            {
                ListViewGroup group = annoInfoListView.Groups.Add("pageNumber", "Page " + (i + 1));
                for (int j = 0; j < annotations[i].Count; j++)
                {
                    AnnotationData annot = annotations[i][j];
                    ListViewItem item = annoInfoListView.Items.Add(annot.GetType().ToString());
                    item.Group = group;
                    item.SubItems.Add(annot.Location.ToString());
                }
            }
        }

    }
}
