using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Autofac;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Interfaces;
using Tabs.Mobile.Shared.Managers;
using Tabs.Mobile.Shared.WebServices;

namespace Tabs.Mobile.Shared
{
    public class AppStart
    {

        #region Properties

        /// <summary>
        /// Gets or set the AutoFac Container
        /// </summary>
        public static IContainer AutoFacContainer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public static void Init()
        {
            try
            {

                var builder = new ContainerBuilder();

                builder.RegisterInstance(new UsersService()).As<Interfaces.Users.IUserFactory>();
                builder.RegisterType<Managers.Users.UsersFactory>();

                builder.RegisterInstance(new BusinessService()).As<Interfaces.Businesses.IBusinessFactory>();
                builder.RegisterType<Managers.Businesses.BusinessFactory>();

                builder.RegisterInstance(new IndividualsService()).As<Interfaces.Individuals.IIndividualFactory>();
                builder.RegisterType<Managers.Individuals.IndividualFactory>();

                builder.RegisterInstance(new BusinessAccountsService()).As<Interfaces.Accounts.IBusinessAccountFactory>();
                builder.RegisterType<Managers.Accounts.BusinessAccountFactory>();

                builder.RegisterInstance(new VerificationCodeService()).As<IVerificationCode>();
                builder.RegisterType<VerificationCodeFactory>();

                builder.RegisterInstance(new ToastersService()).As<Interfaces.Individuals.IToastersFactory>();
                builder.RegisterType<Managers.Individuals.ToastersFactory>();

                builder.RegisterInstance(new AddressService()).As<Interfaces.Businesses.IAddressFactory>();
                builder.RegisterType<Managers.Businesses.AddressFactory>();

                builder.RegisterInstance(new BusinessTypesService()).As<Interfaces.Businesses.IBusinessTypesFactory>();
                builder.RegisterType<Managers.Businesses.BusinessTypesFactory>();

                builder.RegisterInstance(new BusinessEventsService()).As<Interfaces.Events.IBusinessEventsFactory>();
                builder.RegisterType<Managers.Events.BusinessEventsFactory>();

                builder.RegisterInstance(new RsvpService()).As<Interfaces.Events.IRsvpFactory>();
                builder.RegisterType<Managers.Events.RsvpFactory>();

                builder.RegisterInstance(new EventLikesService()).As<Interfaces.Events.IEventLikesFactory>();
                builder.RegisterType<Managers.Events.EventLikesFactory>();

                builder.RegisterInstance(new CheckInService()).As<Interfaces.CheckIns.ICheckInFactory>();
                builder.RegisterType<Managers.CheckIns.CheckInFactory>();

                builder.RegisterInstance(new CheckInLikesService()).As<Interfaces.CheckIns.ICheckInLikesFactory>();
                builder.RegisterType<Managers.CheckIns.CheckInLikesFactory>();

                builder.RegisterInstance(new BusinessDrinkService()).As<Interfaces.Drinks.IBusinessDrinkFactory>();
                builder.RegisterType<Managers.Drinks.BusinessDrinkFactory>();

                builder.RegisterInstance(new StripeCustomerInfoService()).As<Interfaces.Payments.IStripeCustomerInfoFactory>();
                builder.RegisterType<Managers.Payments.StripeCustomerInfoFactory>();

                //builder.RegisterType<Interfaces.Payments.ICustomerPaymentInfoFactory, Managers.Payments.CustomerPaymentInfoFactory>(new contain);
                builder.RegisterType<Managers.Payments.CustomerPaymentInfoFactory>().As<Interfaces.Payments.ICustomerPaymentInfoFactory>();

                builder.RegisterInstance(new ToasterOrderService()).As<Interfaces.Orders.IToasterOrderFactory>();
                builder.RegisterType<Managers.Orders.ToasterOrderFactory>();

                builder.RegisterInstance(new BusinessPhotoService()).As<Interfaces.Businesses.IBusinessPhotoFactory>();
                builder.RegisterType<Managers.Businesses.BusinessPhotoFactory>();

                builder.RegisterInstance(new ToasterPhotoService()).As<Interfaces.Individuals.IToasterPhotoFactory>();
                builder.RegisterType<Managers.Individuals.ToasterPhotoFactory>();

                builder.RegisterInstance(new ToasterPointsService()).As<Interfaces.Points.IToasterPointsFactory>();
                builder.RegisterType<Managers.Points.ToasterPointsFactory>();

                builder.RegisterInstance(new SMSMessageService()).As<Interfaces.Individuals.ISMSMessageFactory>();
                builder.RegisterType<Managers.Individuals.SMSMessageFactory>();

                builder.RegisterInstance(new NotificationRegisterService()).As<INotificationRegisterFactory>();
                builder.RegisterType<NotificationRegisterFactory>();

                builder.RegisterInstance(new ReportedSpamCheckInService()).As<Interfaces.Reports.Spams.IReportedSpamCheckInFactory>();
                builder.RegisterType<Managers.Reports.Spams.ReportedSpamCheckInFactory>();

                builder.RegisterInstance(new InappropriateReportCheckInService()).As<Interfaces.Reports.InappropriateReports.IInappropriateReportCheckInFactory>();
                builder.RegisterType<Managers.Reports.InappropriateReports.InappropriateReportCheckInFactory>();

                builder.RegisterInstance(new ReportedUserService()).As<Interfaces.Reports.Users.IReportedUserFactory>();
                builder.RegisterType<Managers.Reports.Users.ReportedUserFactory>();

                AutoFacContainer = builder.Build();

            } catch(Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}
