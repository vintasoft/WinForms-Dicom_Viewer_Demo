using System.Collections.Generic;

#if !REMOVE_ANNOTATION_PLUGIN
using Vintasoft.Imaging.Annotation.Dicom.UI.VisualTools; 
#endif
using Vintasoft.Imaging.UI.VisualTools;


namespace DemosCommonCode.Imaging
{
    /// <summary>
    /// A tool strip that shows buttons, which allow to enable/disable the <see cref="DicomAnnotatedViewerTool"/> in image viewer.
    /// </summary>
    public partial class DicomAnnotatedViewerToolStrip : VisualToolsToolStrip
    {

        #region Fields

        /// <summary>
        /// Additional visual tools.
        /// </summary>
        List<VisualTool> _additionalVisualTools = new List<VisualTool>();

        #endregion



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DicomAnnotatedViewerToolStrip"/> class.
        /// </summary>
        public DicomAnnotatedViewerToolStrip()
        {
        }

        #endregion



        #region Properties

        /// <summary>
        /// Gets or sets the mandatory visual tool.
        /// </summary>
        /// <value>
        /// Default value is <b>null</b>.
        /// </value>
        /// <remarks>
        /// The mandatory visual tool is always active because is always used in composition with selected visual tool.
        /// </remarks>
        public override VisualTool MandatoryVisualTool
        {
            get
            {
                return base.MandatoryVisualTool;
            }
            set
            {
            }
        }

        CompositeVisualTool _mainVisualTool = null;
        /// <summary>
        /// Gets or sets the main visual tool.
        /// </summary>
        /// <value>
        /// Default value is <b>null</b>.
        /// </value>
        public CompositeVisualTool MainVisualTool
        {
            get
            {
                return _mainVisualTool;
            }
        }

#if REMOVE_ANNOTATION_PLUGIN
        Vintasoft.Imaging.Dicom.UI.VisualTools.DicomViewerTool _dicomAnnotatedViewerTool;
        /// <summary>
        /// Gets or sets the <see cref="DicomViewerTool"/>.
        /// </summary>
        /// <value>
        /// Default value is <b>null</b>.
        /// </value>
        public Vintasoft.Imaging.Dicom.UI.VisualTools.DicomViewerTool DicomAnnotatedViewerTool
        {
            get
            {
                return _dicomAnnotatedViewerTool;
            }
            set
            {
                if (_dicomAnnotatedViewerTool != value)
                {
                    _dicomAnnotatedViewerTool = value;

                    if (_mainVisualTool == null)
                    {
                        _mainVisualTool = _dicomAnnotatedViewerTool;
                    }
                    else
                    {
                        // update main visual tool
                        List<VisualTool> tools = new List<VisualTool>(_additionalVisualTools);
                        tools.Add(_dicomAnnotatedViewerTool);
                        _mainVisualTool = new CompositeVisualTool(tools.ToArray());
                    }
                }
            }
        }
#else
        DicomAnnotatedViewerTool _dicomAnnotatedViewerTool = null;
        /// <summary>
        /// Gets or sets the <see cref="DicomAnnotatedViewerTool"/>.
        /// </summary>
        /// <value>
        /// Default value is <b>null</b>.
        /// </value>
        public DicomAnnotatedViewerTool DicomAnnotatedViewerTool
        {
            get
            {
                return _dicomAnnotatedViewerTool;
            }
            set
            {
                if (_dicomAnnotatedViewerTool != value)
                {
                    _dicomAnnotatedViewerTool = value;

                    if (_mainVisualTool == null)
                    {
                        _mainVisualTool = _dicomAnnotatedViewerTool;
                    }
                    else
                    {
                        // update main visual tool
                        List<VisualTool> tools = new List<VisualTool>(_additionalVisualTools);
                        tools.Add(_dicomAnnotatedViewerTool);
                        _mainVisualTool = new CompositeVisualTool(tools.ToArray());
                    }
                }
            }
        } 
#endif

        #endregion



        #region Methods

        /// <summary>
        /// Returns the visual tool for the specified visual tool action.
        /// </summary>
        /// <param name="visualToolAction">The visual tool action.</param>
        /// <returns>
        /// The visual tool.
        /// </returns>
        protected override VisualTool GetVisualTool(VisualToolAction visualToolAction)
        {
            return _mainVisualTool;
        }

        /// <summary>
        /// Adds the visual tool action to this tool strip 
        /// and adds visual tool to <see cref="MainVisualTool"/>.
        /// </summary>
        /// <param name="visualToolAction">Additional visual tool action.</param>
        /// <exception cref="System.Exception"> Thrown if <see cref="MainVisualTool"/> was <b>null</b>.</exception>
        public void AddVisualToolAction(VisualToolAction visualToolAction)
        {
            if (_mainVisualTool == null)
                throw new System.Exception(DicomViewerDemo.Localization.Strings.DEMOSCOMMONCODE_IMAGING_ADD_DICOMANNOTATEDVIEWERTOOL_FIRST);

            // add action to tool strip
            base.AddAction(visualToolAction);

            _additionalVisualTools.Add(visualToolAction.VisualTool);

            // update main visual tool
            List<VisualTool> tools = new List<VisualTool>(_additionalVisualTools);
            tools.Add(_dicomAnnotatedViewerTool);
            _mainVisualTool = new CompositeVisualTool(tools.ToArray());
        }

        #endregion

    }
}
