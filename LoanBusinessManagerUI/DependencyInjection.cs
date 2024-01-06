using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace LoanBusinessManagerUI
{
    public static class DependencyInjection
    {
        public static void InjectDependency(this IServiceCollection services, IS3Config s3Config, ISqliteConfig sqliteConfig)
        {
            services.AddScoped<LBMContext>();
            services.AddScoped<ILBMUoW, LBMUoW>();

            services.AddScoped<PersonBusiness>();
            services.AddScoped<PhoneBusiness>();
            services.AddScoped<LoanBusiness>();
            services.AddScoped<PaymentBusiness>();
            services.AddScoped<DebtBusiness>();
            services.AddScoped<DescriptionHistoryBusiness>();

            services.AddScoped<PersonService>();
            services.AddScoped<PhoneService>();
            services.AddScoped<LoanService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<DebtService>();
            services.AddScoped<DescriptionHistoryService>();

            services.AddScoped<MainPageViewModel>();
            services.AddScoped<LoanViewModel>();
            services.AddScoped<PaymentViewModel>();
            services.AddScoped<AppShellViewModel>();
            services.AddScoped<PersonViewModel>();
            services.AddScoped<OptionsViewModel>();

            services.AddSingleton<MainPage>();
            services.AddSingleton<LoanPage>();
            services.AddSingleton<PersonPage>();
            services.AddSingleton<OptionsPage>();
            services.AddSingleton<CreatePersonPage>();
            services.AddSingleton<EditLoanPage>();
            services.AddSingleton<EditPaymentPage>();
            services.AddSingleton<EditPersonPage>();
            services.AddSingleton<LoansListPage>();
            services.AddSingleton<PaymentsListPage>();

#if ANDROID
            services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);
#endif

            services.AddSingleton(s3Config);
            services.AddSingleton(sqliteConfig);
        }
    }
}
