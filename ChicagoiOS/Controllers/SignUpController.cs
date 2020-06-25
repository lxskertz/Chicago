using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Users;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class SignUpController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// Gets or sets SignUpDataSource
        /// </summary>
        private DataSource.SignUpDataSource SignUpDataSource { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public UITextField Firstname { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public UITextField Lastname { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField Email { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField Username { get; set; }
        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public UITextField Password { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField ReEnterPAssword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISwitch Individual { get; set; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public UISwitch Business { get; set; }

        #endregion

        #region Constructors

        public SignUpController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load Data
        /// </summary>
        private void LoadData()
        {
            try
            {
                CreateAcctTable.EstimatedRowHeight = 44f;
                CreateAcctTable.RowHeight = UITableView.AutomaticDimension;
                SignUpDataSource = new DataSource.SignUpDataSource(this);
                CreateAcctTable.Source = SignUpDataSource;
                CreateAcctTable.TableFooterView = new UIView();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Continue", UIBarButtonItemStyle.Plain, async (sender, args) =>
                {
                    await Next();
                }), true);

                UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                {
                    CreateAcctTable.EndEditing(true);
                });

                tapGesture.CancelsTouchesInView = false;
                CreateAcctTable.AddGestureRecognizer(tapGesture);
                BindView();
            }
            catch (Exception)
            {
            }
        }

        private void BindView()
        {
            TermsOfUseLbl.AttributedText = new NSAttributedString("Terms of Use", underlineStyle: NSUnderlineStyle.Single);
            TermsOfUseLbl.UserInteractionEnabled = true;
            UITapGestureRecognizer r = new UITapGestureRecognizer(async () => {
                TermsOfUseLbl.Highlighted = true;
                TermsOfUseController c = this.Storyboard.InstantiateViewController("TermsOfUseController") as TermsOfUseController;
                this.NavigationController.PushViewController(c, true);
                await Task.Delay(1000);
                TermsOfUseLbl.Highlighted = false;
            });
            TermsOfUseLbl.AddGestureRecognizer(r);

            PrivacyPolicyLbl.UserInteractionEnabled = true;
            PrivacyPolicyLbl.AttributedText = new NSAttributedString("Privacy Policy", underlineStyle: NSUnderlineStyle.Single);
            UITapGestureRecognizer p = new UITapGestureRecognizer(async () => {
                PrivacyPolicyLbl.Highlighted = true;
                PrivacyPolicyController pc = this.Storyboard.InstantiateViewController("PrivacyPolicyController") as PrivacyPolicyController;
                this.NavigationController.PushViewController(pc, true);
                await Task.Delay(1000);
                PrivacyPolicyLbl.Highlighted = false;
            });
            PrivacyPolicyLbl.AddGestureRecognizer(p);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);


                //this.Individual.ValueChanged += delegate
                //{
                //    Business.On = !Individual.On; //SetState(false, true);
                //};

                //this.Business.ValueChanged += delegate
                //{
                //    Individual.On = !Business.On;
                //};
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                LoadData();

                //AppDelegate.Track("Sign Up", "View");

            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Next()
        {
            try
            {
                if (string.IsNullOrEmpty(Firstname.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyFirstname, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Lastname.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyLastname, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Email.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyEmail, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (!IsValidEmail(Email.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NotAValidEmail, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Username.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyUsername, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Password.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyPassword, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (Password.Text.Length <= 8)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.PasswordLength, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (!string.Equals(Password.Text, ReEnterPAssword.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.UnequalPassword, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (!Individual.On && !Business.On)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyIsBusinessorIndividual, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    await Register();
                }

                //AppDelegate.Track("Sign Up", "Sign up");

            }
            catch (Exception)
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task Register()
        {
            if (AppDelegate.IsOfflineMode())
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                return;
            }
            Users user = null;
            BTProgressHUD.Show(ToastMessage.PleaseWait, -1f, ProgressHUD.MaskType.Black);
            try
            {

                var emailExist = await AppDelegate.UsersFactory.EmailExist(Email.Text.Trim());
                var usernameExist = await AppDelegate.UsersFactory.UsernameExist(Username.Text.Trim());

                if (emailExist)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.AccountExist, Helpers.ToastTime.ErrorTime);
                }
                else if (usernameExist)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.AccountUsernameExist, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    var newUser = new Users
                    {
                        FirstName = Firstname.Text.Trim(),
                        LastName = Lastname.Text.Trim(),
                        Email = Email.Text.Trim(),
                        PasswordHash = Password.Text.Trim(),
                        Username = Username.Text.Trim(),
                        IsBusiness = Business.On,
                        IsIndividual = Individual.On
                    };

                    await AppDelegate.UsersFactory.AddNewUser(newUser);
                    user = await AppDelegate.UsersFactory.GetUser(Email.Text.Trim());

                    if (user != null)
                    {
                        AppDelegate.DeleteCurrentUser();
                        user.PasswordHash = string.Empty;
                        AppDelegate.SaveCurrentUser(user);
                        AppDelegate.SaveEnvironment(Shared.MyEnvironment.Environment); //TODO: set right url
                        AppDelegate.CurrentUser = user;
                        await CreateAccount(user);
                        OpenHomeScreen(user);
                        AppDelegate.AddToAzureNotificationHub(null, null, true);
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
                    }

                    //VerificationCode verificationCode = new VerificationCode();
                    //verificationCode.Email = Email.Text.Trim();
                    //verificationCode.Code = KazopiMobile.Helpers.PasswordHash.GenerateCodeNumber();
                    //verificationCode.UserId = user.UserId;
                    //await AppDelegate.VerificationCodeManager.AddVerificationCode(verificationCode);

                    //VerifyAccountController verifiyAcct = this.Storyboard.InstantiateViewController("VerifyAccountController") as VerifyAccountController;
                    //verifiyAcct.Email = Email.Text.Trim();
                    //this.NavigationController.PushViewController(verifiyAcct, true);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
            BTProgressHUD.Dismiss();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        private void OpenHomeScreen(Users newUser)
        {
            if (newUser.IsIndividual)
            {
                UIViewController individualController = this.Storyboard.InstantiateViewController("IndividualHomeController") as IndividualHomeController;
                this.NavigationController.SetViewControllers(new UIViewController[] { individualController }, true);
            }
            else if (newUser.IsBusiness)
            {
                BusinessRegistrationController c = this.Storyboard.InstantiateViewController("BusinessRegistrationController") as BusinessRegistrationController;
                c.UserInfo = newUser;
                this.NavigationController.PushViewController(c, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<bool> CreateAccount(Users user)
        {
            try
            {
                if (user.IsBusiness)
                {
                    //Business biz = new Business();
                    //biz.UserId = user.UserId;
                    //await AppDelegate.BusinessFactory.CreateBusiness(biz);
                }
                else
                {
                    Individual ind = new Individual();
                    ind.UserId = user.UserId;
                    await AppDelegate.IndividualFactory.CreateIndividual(ind);

                    Shared.Models.Points.Point point = new Shared.Models.Points.Point();
                    point.UserId = user.UserId;
                    point.PointStatus = Shared.Models.Points.Point.ToasterPointStatus.Earned;
                    point.EarnedDate = DateTime.Now;
                    point.RedeemedDate = null;
                    point.PointAmount = (int)Shared.Models.Points.Point.PointAmountScale.SignUp;
                    await AppDelegate.ToasterPointsFactory.NewDailyPoint(point);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}