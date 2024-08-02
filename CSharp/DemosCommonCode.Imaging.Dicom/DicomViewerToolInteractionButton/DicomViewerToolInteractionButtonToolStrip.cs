using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using DemosCommonCode;

using Vintasoft.Imaging.Dicom.UI.VisualTools;

namespace DicomViewerDemo
{
    /// <summary>
    /// A toolbar for <see cref="DicomViewerTool"/>.
    /// </summary>
    public partial class DicomViewerToolInteractionButtonToolStrip : ToolStrip
    {

        #region Fields

        /// <summary>
        /// Dictionary: the DICOM viewer tool interaction mode => menu button.
        /// </summary>
        Dictionary<DicomViewerToolInteractionMode, ToolStripItem> _interactionModeToMenuButton =
            new Dictionary<DicomViewerToolInteractionMode, ToolStripItem>();

        /// <summary>
        /// Dictionary: menu button => the DICOM viewer tool interaction mode.
        /// </summary>
        Dictionary<ToolStripItem, DicomViewerToolInteractionMode> _menuButtonToInteractionMode =
            new Dictionary<ToolStripItem, DicomViewerToolInteractionMode>();

        /// <summary>
        /// Dictionary: the DICOM viewer tool interaction mode => icon name format for menu button.
        /// </summary>
        Dictionary<DicomViewerToolInteractionMode, string> _interactionModeToIconNameFormat =
            new Dictionary<DicomViewerToolInteractionMode, string>();

        /// <summary>
        /// The icon name of mouse wheel button.
        /// </summary>
        readonly string MOUSE_WHEEL_BUTTON_ICON_NAME;

        /// <summary>
        /// The available mouse buttons.
        /// </summary>
        MouseButtons[] _availableMouseButtons = new MouseButtons[] {
            MouseButtons.Left, MouseButtons.Middle, MouseButtons.Right
        };

        #endregion



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DicomViewerToolInteractionButtonToolStrip"/> class.
        /// </summary>
        public DicomViewerToolInteractionButtonToolStrip()
            :base()
        {
            InitializeComponent();

            // initialize interaction mode of DicomViewerTool
            _supportedInteractionModes = new DicomViewerToolInteractionMode[] {
                        DicomViewerToolInteractionMode.Browse,
                        DicomViewerToolInteractionMode.Pan,
                        DicomViewerToolInteractionMode.Zoom,
                        DicomViewerToolInteractionMode.WindowLevel};

            // initilize name of icons

            string iconsDir = "DemosCommonCode.Imaging.Dicom.DicomViewerToolInteractionButton.Icons.";

            MOUSE_WHEEL_BUTTON_ICON_NAME = iconsDir + "MouseWheel.png";

            _interactionModeToIconNameFormat.Add(DicomViewerToolInteractionMode.Browse,
                iconsDir + "Browse_{0}{1}{2}.png");
            _interactionModeToIconNameFormat.Add(DicomViewerToolInteractionMode.Pan,
                iconsDir + "Pan_{0}{1}{2}.png");
            _interactionModeToIconNameFormat.Add(DicomViewerToolInteractionMode.WindowLevel,
                iconsDir + "WindowLevel_{0}{1}{2}.png");
            _interactionModeToIconNameFormat.Add(DicomViewerToolInteractionMode.Zoom,
                iconsDir + "Zoom_{0}{1}{2}.png");

            // initialize buttons
            InitButtons();
        }

        #endregion



        #region Properties

        DicomViewerTool _tool;
        /// <summary>
        /// Gets or sets the visual tool.
        /// </summary>
        /// <value>
        /// Default value is <b>null</b>.
        /// </value>
        public DicomViewerTool Tool
        {
            get
            {
                return _tool;
            }
            set
            {
                if (Tool != value)
                {
                    if (_tool != null)
                        UnsubscribeFromDicomViewerToolEvents(_tool);

                    _tool = value;

                    if (value != null)
                        SubscribeToDicomViewerToolEvents(value);

                    ResetUnsupportedInteractionModes();
                    UpdateInteractionButtonIcons();
                }
            }
        }

        DicomViewerToolInteractionMode[] _supportedInteractionModes;
        /// <summary>
        /// Gets or sets the supported interaction modes of toolbar.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <i>value</i> is <b>null</b>.</exception>
        public DicomViewerToolInteractionMode[] SupportedInteractionModes
        {
            get
            {
                return _supportedInteractionModes;
            }
            set
            {
                if (_supportedInteractionModes != value)
                {
                    if (value == null)
                        throw new ArgumentNullException();

                    _supportedInteractionModes = value;

                    InitButtons();

                    ResetUnsupportedInteractionModes();
                }
            }
        }

        DicomViewerToolInteractionMode[] _disabledInteractionModes = null;
        /// <summary>
        /// Gets or sets the disabled interaction modes of toolbar.
        /// </summary>
        /// <value>
        /// Default value is <b>null</b>.
        /// </value>
        public DicomViewerToolInteractionMode[] DisabledInteractionModes
        {
            get
            {
                return _disabledInteractionModes;
            }
            set
            {
                // if value is changed
                if (_disabledInteractionModes != value)
                {
                    // save new value
                    _disabledInteractionModes = value;

                    // for each interaction mode
                    foreach (DicomViewerToolInteractionMode interactionMode in _interactionModeToMenuButton.Keys)
                        // enable button for interaction mode
                        _interactionModeToMenuButton[interactionMode].Enabled = true;

                    // if disabled interaction modes are specified
                    if (_disabledInteractionModes != null)
                    {
                        // for each interaction mode
                        foreach (DicomViewerToolInteractionMode interactionMode in _disabledInteractionModes)
                        {
                            // the menu button of interaction mode
                            ToolStripItem menuButton = null;
                            // if button is enabled
                            if (_interactionModeToMenuButton.TryGetValue(interactionMode, out menuButton))
                                // disable the button for interaction mode
                                menuButton.Enabled = false;
                        }
                    }
                }
            }
        }

        #endregion



        #region Methods

        /// <summary>
        /// Initializes the buttons.
        /// </summary>
        private void InitButtons()
        {
            // remove old buttons
            Items.Clear();

            InitMouseWheelButtons();

            if (Items.Count > 0)
                Items.Add(new ToolStripSeparator());

            InitInteractionModeMenuButtons();
        }

        /// <summary>
        /// Initializes the mouse wheel buttons.
        /// </summary>
        private void InitMouseWheelButtons()
        {
            // the button name
            string name = DicomViewerDemo.Localization.Strings.DICOMVIEWERDEMO_MOUSE_WHEEL;
            // create the "Mouse Wheel" button
            ToolStripDropDownButton mouseWheelMenuButton = new ToolStripDropDownButton(name);
            mouseWheelMenuButton.ToolTipText = name;

            // set the button icon
            SetToolStripButtonIcon(mouseWheelMenuButton, MOUSE_WHEEL_BUTTON_ICON_NAME);

            // available interaction modes of mouse wheel
            DicomViewerToolMouseWheelInteractionMode[] mouseWheelInteractionMode =
                new DicomViewerToolMouseWheelInteractionMode[] {
                     DicomViewerToolMouseWheelInteractionMode.None,
                     DicomViewerToolMouseWheelInteractionMode.Slide,
                     DicomViewerToolMouseWheelInteractionMode.Zoom };
            // for each interaction mode
            foreach (DicomViewerToolMouseWheelInteractionMode interactionMode in mouseWheelInteractionMode)
            {
                // create button
                ToolStripMenuItem menuButton = new ToolStripMenuItem(interactionMode.ToString());

                // if interaction mode is "Browse"
                if (interactionMode == DicomViewerToolMouseWheelInteractionMode.Slide)
                {
                    // mark button as checked
                    menuButton.Checked = true;
                }

                // save information about interaction mode in button
                menuButton.Tag = interactionMode;
                // subscribe to the button click event
                menuButton.Click += new EventHandler(mouseWheelInteractionModeButton_Click);

                // add button
                mouseWheelMenuButton.DropDownItems.Add(menuButton);
            }

            // add button to ToolStrip
            Items.Add(mouseWheelMenuButton);
        }

        /// <summary>
        /// Initializes the interaction mode menu buttons.
        /// </summary>
        private void InitInteractionModeMenuButtons()
        {
            // clear dictionaries
            _interactionModeToMenuButton.Clear();
            _menuButtonToInteractionMode.Clear();

            // for each suported interaction mode
            foreach (DicomViewerToolInteractionMode interactionMode in _supportedInteractionModes)
            {
                // create button

                // get button name
                string name = interactionMode.ToString();

                ToolStripItem menuButton = new ToolStripButton(name);

                menuButton.ToolTipText = name;

                // set the button icon
                SetToolStripButtonIcon(menuButton, interactionMode, MouseButtons.None);

                // add button to the dictionaries
                _interactionModeToMenuButton.Add(interactionMode, menuButton);
                _menuButtonToInteractionMode.Add(menuButton, interactionMode);

                // if button must be disabled
                if (_disabledInteractionModes != null &&
                    Array.IndexOf(_disabledInteractionModes, interactionMode) >= 0)
                    // disable the button
                    menuButton.Enabled = false;

                menuButton.MouseDown += new MouseEventHandler(interactionModeButton_MouseDown);

                // add button to the ToolStrip
                Items.Add(menuButton);
            }
        }


        #region Interaction Mode

        /// <summary>
        /// Selects the interaction mode button.
        /// </summary>
        private void dicomViewerTool_InteractionModeChanged(object sender, DicomViewerToolInteractionModeChangedEventArgs e)
        {
            UpdateInteractionMode(e.Button, e.InteractionMode);
        }

        /// <summary>
        /// Selects the interaction mode of <see cref="DicomViewerTool"/>.
        /// </summary>
        private void interactionModeButton_MouseDown(object sender, MouseEventArgs e)
        {
            ToolStripSplitButton splitMenuButton = sender as ToolStripSplitButton;
            if (splitMenuButton != null && !splitMenuButton.ButtonPressed)
                return;

            ToolStripItem menuButton = (ToolStripItem)sender;
            DicomViewerToolInteractionMode interactionMode = _menuButtonToInteractionMode[menuButton];

            UpdateInteractionMode(e.Button, interactionMode);
        }

        /// <summary>
        /// Updates the interaction mode in <see cref="DicomViewerTool"/>.
        /// </summary>
        /// <param name="mouseButton">Mouse button.</param>
        /// <param name="interactionMode">Interaction mode.</param>
        private void UpdateInteractionMode(MouseButtons mouseButton, DicomViewerToolInteractionMode interactionMode)
        {
            // if interaction mode is NOT supported
            if (Array.IndexOf(SupportedInteractionModes, interactionMode) == -1)
                interactionMode = DicomViewerToolInteractionMode.None;

            // if mouse button is NOT supported
            if (Array.IndexOf(_availableMouseButtons, mouseButton) == -1)
                interactionMode = DicomViewerToolInteractionMode.None;

            // set the interaction mode for DICOM viewer tool
            Tool.SetInteractionMode(mouseButton, interactionMode);

            // update icons of interaction buttons
            UpdateInteractionButtonIcons();
        }

        #endregion


        #region Buttons Icon

        /// <summary>
        /// Updates the icon of interaction buttons.
        /// </summary>
        private void UpdateInteractionButtonIcons()
        {
            // for each interaction mode
            foreach (DicomViewerToolInteractionMode interactionMode in _interactionModeToMenuButton.Keys)
            {
                // get the menu button of interaction mode
                ToolStripItem menuButton = _interactionModeToMenuButton[interactionMode];

                // get mouse buttons of interaction mode
                MouseButtons mouseButtons = GetMouseButtonsForInteractionMode(interactionMode);

                // update icon for menu button
                SetToolStripButtonIcon(menuButton, interactionMode, mouseButtons);
            }
        }

        /// <summary>
        /// Returns the icon name of specified interaction mode and buttons.
        /// </summary>
        /// <param name="interactionMode">The interaction mode.</param>
        /// <param name="mouseButtons">The mouse buttons of interaction mode.</param>
        /// <returns>
        /// The icon name.
        /// </returns>
        private string GetInteractionModeIconName(DicomViewerToolInteractionMode interactionMode, MouseButtons mouseButtons)
        {
            // indices of action buttons (left, middle, right)
            byte[] indexes = new byte[] { 0, 0, 0 };

            // if mouse buttons are not empty
            if (mouseButtons != MouseButtons.None)
            {
                // if left mouse button is active
                if ((mouseButtons & MouseButtons.Left) != 0)
                    indexes[0] = 1;
                // if middle mouse button is active
                if ((mouseButtons & MouseButtons.Middle) != 0)
                    indexes[1] = 1;
                // if right mouse button is active
                if ((mouseButtons & MouseButtons.Right) != 0)
                    indexes[2] = 1;
            }

            // get the icon name format
            string iconNameFormat = _interactionModeToIconNameFormat[interactionMode];

            // return the icon name
            return string.Format(iconNameFormat, indexes[0], indexes[1], indexes[2]);
        }

        /// <summary>
        /// Sets the icon for the tool strip button.
        /// </summary>
        /// <param name="menuButton">The menu button.</param>
        /// <param name="interactionMode"></param>
        /// <param name="mouseButtons"></param>
        private void SetToolStripButtonIcon(
            ToolStripItem menuButton,
            DicomViewerToolInteractionMode interactionMode,
            MouseButtons mouseButtons)
        {
            // get icon name for interaction mode
            string iconName = GetInteractionModeIconName(interactionMode, mouseButtons);

            // set the icon for button
            SetToolStripButtonIcon(menuButton, iconName);
        }

        /// <summary>
        /// Sets the icon for the tool strip button.
        /// </summary>
        /// <param name="menuButton">The menu button.</param>
        /// <param name="iconName">The icon name.</param>
        private void SetToolStripButtonIcon(ToolStripItem menuButton, string iconName)
        {
            // if the icon name is NOT specified
            if (string.IsNullOrEmpty(iconName))
                return;

            // if menu button contains infomation about the button icon
            if (menuButton.Tag is string)
            {
                // get the icon name
                string currentIconName = menuButton.Tag.ToString();

                // if icon is not changed
                if (String.Equals(currentIconName, iconName, StringComparison.InvariantCultureIgnoreCase))
                    return;
            }

            menuButton.ImageTransparentColor = Color.Magenta;
            // load resource stream
            Stream stream = DemosResourcesManager.GetResourceAsStream(iconName);
            // if stream is loaded
            if (stream != null)
            {
                // if icon must be removed
                if (menuButton.Image != null)
                    menuButton.Image.Dispose();

                // load image
                menuButton.Image = Image.FromStream(stream);
                // save icon name
                menuButton.Tag = iconName;
                stream.Dispose();
            }

            menuButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            menuButton.ImageScaling = ToolStripItemImageScaling.None;
        }

        #endregion


        #region Common

        /// <summary>
        /// Returns the mouse buttons for interaction mode.
        /// </summary>
        /// <param name="interactionMode">The interaction mode.</param>
        /// <returns>
        /// The mouse buttons for interaction mode.
        /// </returns>
        private MouseButtons GetMouseButtonsForInteractionMode(DicomViewerToolInteractionMode interactionMode)
        {
            // the result mouse buttons
            MouseButtons resultMouseButton = MouseButtons.None;

            // if tool exists
            if (Tool != null)
            {
                // for each available mouse button
                foreach (MouseButtons button in _availableMouseButtons)
                {
                    // get an interaction mode for mouse button
                    DicomViewerToolInteractionMode mouseButtonInteractionMode = Tool.GetInteractionMode(button);
                    // if interaction mode for mouse button equals to the analyzing interaction mode
                    if (mouseButtonInteractionMode == interactionMode)
                        // add mouse button to the result
                        resultMouseButton |= button;
                }
            }

            return resultMouseButton;
        }

        /// <summary>
        /// Resets the unsupported interaction modes.
        /// </summary>
        private void ResetUnsupportedInteractionModes()
        {
            if (Tool == null)
                return;

            // for each mouse button
            foreach (MouseButtons mouseButton in _availableMouseButtons)
            {
                // get the interaction mode for mouse button
                DicomViewerToolInteractionMode interactionMode = Tool.GetInteractionMode(mouseButton);

                // if interaction mode is None
                if (interactionMode == DicomViewerToolInteractionMode.None)
                    continue;

                // is interaction mode is not supported
                if (Array.IndexOf(SupportedInteractionModes, interactionMode) == -1)
                    // reset the interaction mode for mouse button
                    Tool.SetInteractionMode(mouseButton, DicomViewerToolInteractionMode.None);
            }
        }

        /// <summary>
        /// The mouse wheel interaction mode is changed.
        /// </summary>
        private void mouseWheelInteractionModeButton_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem currentMenuButton = (ToolStripMenuItem)sender;
            // get the interaction mode
            DicomViewerToolMouseWheelInteractionMode interactionMode = (DicomViewerToolMouseWheelInteractionMode)currentMenuButton.Tag;

            // update the interaction mode for mouse wheel
            Tool.MouseWheelInteractionMode = interactionMode;


            // uncheck all buttons

            ToolStripDropDownButton parentMenuButton = currentMenuButton.OwnerItem as ToolStripDropDownButton;
            // if parent menu button exists
            if (parentMenuButton != null)
            {
                // for each item in parent menu item
                foreach (ToolStripItem item in parentMenuButton.DropDownItems)
                {
                    // if item is menu button
                    if (item is ToolStripMenuItem)
                        // uncheck the menu button
                        ((ToolStripMenuItem)item).Checked = false;
                }
            }

            // check the current menu button
            currentMenuButton.Checked ^= true;
        }

        /// <summary>
        /// Subscribes to the <see cref="DicomViewerTool"/> events.
        /// </summary>
        /// <param name="tool">The <see cref="DicomViewerTool"/>.</param>
        private void SubscribeToDicomViewerToolEvents(DicomViewerTool tool)
        {
            tool.ImageViewer.GotFocus += new EventHandler(imageViewer_GotFocus);
            tool.InteractionModeChanged +=
                new EventHandler<DicomViewerToolInteractionModeChangedEventArgs>(dicomViewerTool_InteractionModeChanged);
        }

        /// <summary>
        /// Unsubscribes from the <see cref="DicomViewerTool"/> events.
        /// </summary>
        /// <param name="tool">The <see cref="DicomViewerTool"/>.</param>
        private void UnsubscribeFromDicomViewerToolEvents(DicomViewerTool tool)
        {
            tool.InteractionModeChanged -= dicomViewerTool_InteractionModeChanged;
        }

        /// <summary>
        /// Updates the focused measurement annotations.
        /// </summary>
        private void imageViewer_GotFocus(object sender, EventArgs e)
        {
            UpdateInteractionButtonIcons();
        }

        #endregion

        #endregion

    }
}
