using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using DemosCommonCode.Imaging;

#if !REMOVE_ANNOTATION_PLUGIN
using Vintasoft.Imaging.Annotation.Dicom;
using Vintasoft.Imaging.Annotation.UI;
using Vintasoft.Imaging.Annotation.UI.VisualTools; 
#endif
using Vintasoft.Imaging.UI.VisualTools;
using Vintasoft.Imaging.UI;


namespace DicomViewerDemo
{
    /// <summary>
    /// Toolstrip with DICOM annotations.
    /// </summary>
    public partial class AnnotationsToolStrip : ToolStrip
    {

        #region Fields

        ToolStripButton _buildingAnnotationButton;

        const string SEPARATOR = "SEPARATOR";
        const string PointButtonName = "Point";
        const string CircleButtonName = "Circle";
        const string PolylineButtonName = "Polyline";
        const string InterpolatedButtonName = "Interpolated";

        const string EllipseButtonName = "Ellipse";
        const string MultilineButtonName = "Multiline";
        const string RangelineButtonName = "Rangeline";
        const string InifinitelineButtonName = "Infiniteline";
        const string CutlineButtonName = "Cutline";
        const string ArrowButtonName = "Arrow";
        const string RectangleButtonName = "Rectangle";
        const string AxisButtonName = "Axis";
        const string RulerButtonName = "Ruler";
        const string CrosshairButtonName = "Crosshair";

        const string TextButtonName = "Text";


        string[] AnnotationNames = { 
            PointButtonName,
            CircleButtonName,
            PolylineButtonName,
            InterpolatedButtonName,
            SEPARATOR,
            
            RectangleButtonName,
            EllipseButtonName,
            MultilineButtonName,
            RangelineButtonName,
            InifinitelineButtonName,
            CutlineButtonName,
            ArrowButtonName,
            AxisButtonName,
            RulerButtonName,
            CrosshairButtonName,
            SEPARATOR,

            TextButtonName,
        };

#if !REMOVE_ANNOTATION_PLUGIN
        AnnotationVisualTool _annotationTool; 
#endif

        #endregion



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationsToolStrip"/> class.
        /// </summary>
        public AnnotationsToolStrip()
            : base()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AnnotationsToolStrip));

            for (int i = 0; i < AnnotationNames.Length; i++)
            {
                string name = AnnotationNames[i];
                if (name == SEPARATOR)
                {
                    Items.Add(new ToolStripSeparator());
                }
                else
                {
                    ToolStripButton button = new ToolStripButton(name);
                    button.ImageTransparentColor = Color.Magenta;
                    button.ToolTipText = name;
                    button.Click += new EventHandler(buildAnnotationButton_Click);
                    button.Image = (Image)(resources.GetObject(name));
                    button.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    button.ImageScaling = ToolStripItemImageScaling.None;
                    Items.Add(button);
                }
            }

            Viewer = null;
        }

        #endregion



        #region Properties

        ImageViewer _viewer = null;
        /// <summary>
        /// Gets or sets the <see cref="AnnotationViewer"/> associated with
        /// this <see cref="AnnotationsToolStrip"/>.
        /// </summary>        
        public ImageViewer Viewer
        {
            get
            {
                return _viewer;
            }
            set
            {
#if !REMOVE_ANNOTATION_PLUGIN
                if (_annotationTool != null)
                {
                    _annotationTool.AnnotationBuildingFinished -= viewer_AnnotationBuildingFinished;
                    _annotationTool.AnnotationBuildingCanceled -= viewer_AnnotationBuildingCanceled;
                }

                _annotationTool = null;
                _viewer = value;
                if (_viewer != null)
                    _annotationTool = GetAnnotationVisualTool(_viewer.VisualTool);

                if (_annotationTool != null)
                {
                    _annotationTool.AnnotationBuildingFinished += new EventHandler<AnnotationViewEventArgs>(viewer_AnnotationBuildingFinished);
                    _annotationTool.AnnotationBuildingCanceled += new EventHandler<AnnotationViewEventArgs>(viewer_AnnotationBuildingCanceled);
                } 
#endif
            }
        }

        #endregion



        #region Methods

#if !REMOVE_ANNOTATION_PLUGIN
        /// <summary>
        /// Returns an annotation object by the annotation type name.
        /// </summary>
        protected virtual AnnotationView GetAnnotation(string annotationName)
        {
            DicomAnnotationData data = null;

            switch (annotationName)
            {
                case PointButtonName:
                    data = new DicomPointAnnotationData();
                    break;

                case CircleButtonName:
                    data = new DicomCircleAnnotationData();
                    break;

                case PolylineButtonName:
                    data = new DicomPolylineAnnotationData();
                    break;

                case InterpolatedButtonName:
                    data = new DicomPolylineAnnotationData();
                    ((DicomPolylineAnnotationData)data).UseInterpolation = true;
                    break;

                case EllipseButtonName:
                    data = new DicomEllipseAnnotationData();
                    break;

                case MultilineButtonName:
                    data = new DicomMultilineAnnotationData();
                    break;

                case RangelineButtonName:
                    data = new DicomRangeLineAnnotationData();
                    break;

                case InifinitelineButtonName:
                    data = new DicomInfiniteLineAnnotationData();
                    break;

                case CutlineButtonName:
                    data = new DicomCutLineAnnotationData();
                    break;

                case ArrowButtonName:
                    data = new DicomArrowAnnotationData();
                    break;

                case RectangleButtonName:
                    data = new DicomRectangleAnnotationData();
                    break;

                case AxisButtonName:
                    data = new DicomAxisAnnotationData();
                    break;

                case RulerButtonName:
                    data = new DicomRulerAnnotationData();
                    break;

                case CrosshairButtonName:
                    data = new DicomCrosshairAnnotationData();
                    break;

                case TextButtonName:
                    data = new DicomTextAnnotationData();
                    ((DicomTextAnnotationData)data).UnformattedTextValue = "Text";
                    break;
            }

            return AnnotationViewFactory.CreateView(data);
        } 
#endif

        /// <summary>
        /// The annotation building is started.
        /// </summary>
        private void buildAnnotationButton_Click(object sender, EventArgs e)
        {
#if !REMOVE_ANNOTATION_PLUGIN
            ToolStripButton annotationButton = (ToolStripButton)sender;
            annotationButton.Checked = true;

            if (_annotationTool.FocusedAnnotationView != null &&
                _annotationTool.FocusedAnnotationView.InteractionController ==
                _annotationTool.FocusedAnnotationView.Builder)
                _annotationTool.CancelAnnotationBuilding();

            if (_buildingAnnotationButton != null)
                _buildingAnnotationButton.Checked = false;

            if (annotationButton == _buildingAnnotationButton)
            {
                _buildingAnnotationButton = null;
            }
            else
            {
                if (_annotationTool.AnnotationInteractionMode != AnnotationInteractionMode.Author)
                    _annotationTool.AnnotationInteractionMode = AnnotationInteractionMode.Author;

                AnnotationView annotationView = BuildAnnotation(annotationButton.ToolTipText);

                if (annotationView != null)
                    _buildingAnnotationButton = annotationButton;
                else
                    _buildingAnnotationButton = null;
            } 
#endif
        }

#if !REMOVE_ANNOTATION_PLUGIN
        /// <summary>
        /// Adds an annotation to an image and starts building of annotation.
        /// </summary>
        public AnnotationView BuildAnnotation(string annotationName)
        {
            if (Viewer == null || Viewer.Image == null)
                return null;

            AnnotationView annotationView = null;
            try
            {
                annotationView = GetAnnotation(annotationName);

                if (annotationView != null)
                {
                    //
                    _annotationTool.AddAndBuildAnnotation(annotationView);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_BUILDING_ANNOTATION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return annotationView;
        }

        /// <summary>
        /// Annotation building is finished.
        /// </summary>
        private void viewer_AnnotationBuildingFinished(object sender, AnnotationViewEventArgs e)
        {
            ToolStripButton buildingAnnotationButton = _buildingAnnotationButton;
            if (buildingAnnotationButton != null)
            {
                _buildingAnnotationButton = null;
                buildingAnnotationButton.Checked = false;
            }
        }

        /// <summary>
        /// Annotation building is canceled.
        /// </summary>
        private void viewer_AnnotationBuildingCanceled(object sender, AnnotationViewEventArgs e)
        {
            ToolStripButton buildingAnnotationButton = _buildingAnnotationButton;
            if (buildingAnnotationButton != null)
            {
                _buildingAnnotationButton = null;
                buildingAnnotationButton.Checked = false;
            }
        }

        private AnnotationVisualTool GetAnnotationVisualTool(VisualTool visualTool)
        {
            if (visualTool is CompositeVisualTool)
            {
                CompositeVisualTool compositeVisualTool = (CompositeVisualTool)visualTool;
                foreach (VisualTool tool in compositeVisualTool)
                {
                    AnnotationVisualTool result = GetAnnotationVisualTool(tool);
                    if (result != null)
                        return result;
                }

                return null;
            }

            return visualTool as AnnotationVisualTool;
        } 
#endif

        #endregion

    }
}
