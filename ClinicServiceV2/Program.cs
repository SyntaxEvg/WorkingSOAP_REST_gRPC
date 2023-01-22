using ClinicService.Data.DbContexts;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Net;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        #region ConfigGrpc

      
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5100, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
            options.Listen(IPAddress.Any, 5101, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1;
            }) ;
        });
        #endregion
        builder.Services.AddGrpc()
                        .AddJsonTranscoding(); //нужен для 

        #region Configure EF DB Context Service (ClinicService Database)

        builder.Services.AddDbContext<ClinicServiceDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
        });

        #endregion

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddGrpcSwagger();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Grpc Info", Version = "V1" });
            var f = Path.Combine(System.AppContext.BaseDirectory, "ClinicServiceV2.xml");
            x.IncludeXmlComments(f);
            x.IncludeGrpcXmlComments(f,includeControllerXmlComments:true);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x=> x.SwaggerEndpoint("/swagger/v1/swagger.json","MY API V1"));
        }
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.UseRouting(); //передача запросов к методам действия
        app.UseGrpcWeb(new GrpcWebOptions() { DefaultEnabled =true});
        app.MapGrpcService<ClinicService.Services.Impl.ClinicService>().EnableGrpcWeb();
        app.MapGet("/", () => "Hi");
     

        app.Run();
    }
}