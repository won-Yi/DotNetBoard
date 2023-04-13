using Board;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TestingControllersSample
{
    public class Program
    {
        // Main 메서드: 애플리케이션의 진입점
        public static void Main(string[] args)
        {

            try {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            // CreateHostBuilder 메서드를 호출하여 호스트를 생성하고, Build 메서드로 빌드한 뒤 Run 메서드로 애플리케이션 실행
            
        }

        // CreateHostBuilder 메서드: 호스트 빌더를 생성하는 메서드
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    // 기본 호스트 빌더를 생성하여 사용
        //    Host.CreateDefaultBuilder(args)
        //        // 웹 호스팅을 구성하는 메서드
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            // Startup 클래스를 사용하여 웹 호스팅 구성
        //            webBuilder.UseStartup<Startup>();
        //        });
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args) //CreateDefaultBuilder를 호출하여 호스트 설정 시작
          .UseStartup<Startup>();
    }
}