using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.Models
{
    public class PickerViewModel : UIPickerViewModel
    {

        #region Properties

        /// <summary>
        /// ToDo
        /// </summary>
        public Dictionary<int, string> PickerComponents { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        public string SelectedItem { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        public int SelectedItemRow { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [item selected].
        /// </summary>
        public event EventHandler ItemSelected;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerViewModel"/> class.
        /// </summary>
        /// <param name="pickerComponents">The picker components.</param>
        public PickerViewModel(Dictionary<int, string> pickerComponents)
        {
            this.PickerComponents = pickerComponents;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return this.PickerComponents.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="row"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return PickerComponents.ElementAt((int)row).Value;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnItemSelected()
        {
            if (ItemSelected != null)
            {
                ItemSelected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickerView"></param>
        /// <param name="row"></param>
        /// <param name="component"></param>
        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            this.SelectedItemRow = 0;
            SelectedItem = PickerComponents.ElementAt((int)row).Value;
            this.SelectedItemRow = (int)row;
            OnItemSelected();
        }

        #endregion
    }
}