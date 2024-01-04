using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HowTo.UseHostInWPF;

public partial class App : Application
{
    [STAThread]
    static void Main(string[] args)
    {
        // 创建主机
        using IHost host = CreateHostBuilder(args).Build();

        App app = new();
        app.InitializeComponent();
        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                // 读取用户机密
                builder.AddUserSecrets(typeof(App).Assembly);
                // 读取配置文件
                builder.AddConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .Build());
            })
            // 注册服务
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton(sp => new MainWindow
                {
                    // 从配置中读取窗口大小
                    Width = Convert.ToDouble(context.Configuration["ux:windowSize:width"]),
                    Height = Convert.ToDouble(context.Configuration["ux:windowSize:height"]),
                    DataContext = sp.GetRequiredService<MainViewModel>()
                });
                // 注册消息中心
                services.AddSingleton<IMessenger, WeakReferenceMessenger>();
                // 注册主线程调度器
                services.AddSingleton(_ => Current.Dispatcher);
            });
    }
}