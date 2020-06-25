using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class TextFieldInputController : BaseViewController
    {
        #region Constants, Enums, and Variables

        public enum Caller
        {
            EventNameController = 1,
            OtherEventInfo = 2
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Caller ControllerCaller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CharLimit { get; set; } = 255;

        /// <summary>
        /// 
        /// </summary>
        public string _TextviewValue { get; set; }

        #endregion

        #region Constructors

        public TextFieldInputController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void SetValue()
        {
            switch (this.ControllerCaller)
            {
                case Caller.EventNameController:
                    EventNameDescController.RequiresRefresh = true;
                    EventNameDescController.InputValue = _Textview.Text;
                    break;
                case Caller.OtherEventInfo:
                    OtherEventInfoController.RequiresRefresh = true;
                    OtherEventInfoController.InputValue = _Textview.Text;
                    break;
            }
            this.NavigationController.PopViewController(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _Textview.BecomeFirstResponder();
            _Textview.Text = string.IsNullOrEmpty(_TextviewValue) ? string.Empty : _TextviewValue;

            this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) => {
                SetValue();
            }), true);

            _Textview.ShouldChangeText = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= this.CharLimit;
            };

        }

        #endregion

    }
}