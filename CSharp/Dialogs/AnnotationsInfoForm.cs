﻿using System.Windows.Forms;

#if !REMOVE_ANNOTATION_PLUGIN
using Vintasoft.Imaging.Annotation;
using Vintasoft.Imaging.Annotation.Dicom; 
#endif


namespace DicomViewerDemo
{
    /// <summary>
    /// A form that allows to show information about annotations of DICOM file.
    /// </summary>
    public partial class AnnotationsInfoForm : Form
    {

#if !REMOVE_ANNOTATION_PLUGIN
        public AnnotationsInfoForm(DicomAnnotationDataController annotations)
        {
            InitializeComponent();

            for (int i = 0; i < annotations.Images.Count; i++)
            {
                ListViewGroup group = annoInfoListView.Groups.Add("pageNumber", DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_PAGE + (i + 1));
                for (int j = 0; j < annotations[i].Count; j++)
                {
                    AnnotationData annot = annotations[i][j];
                    ListViewItem item = annoInfoListView.Items.Add(annot.GetType().ToString());
                    item.Group = group;
                    item.SubItems.Add(annot.Location.ToString());
                }
            }
        } 
#endif

    }
}
