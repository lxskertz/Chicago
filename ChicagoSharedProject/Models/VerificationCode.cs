namespace Tabs.Mobile.Shared.Models
{
    public class VerificationCode : BaseModel
    {

        #region Properties

        /// <summary>
        /// Gets or sets the code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets if code should be sent
        /// </summary>
        public bool SendCode { get; set; }

        #endregion

    }
}
