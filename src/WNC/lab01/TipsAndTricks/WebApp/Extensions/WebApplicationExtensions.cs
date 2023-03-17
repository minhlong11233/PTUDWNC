﻿using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Middlewares;

namespace WebApp.Extensions
{
	public static class WebApplicationExtensions
	{
		//Thêm các dịch vụ được yêu cầu bỏi MVC
		public static WebApplicationBuilder ConfigureMvc(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddControllersWithViews();
			builder.Services.AddResponseCompression();

			return builder;
		}

		//Đăng kí các dịch vụ với DI Container
		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<BlogDbContext>(options =>
				options.UseSqlServer(
					builder.Configuration
					.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();
			builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
			return builder;

		}

	
		public static WebApplication UseRequestPipeline(
			this WebApplication app)
		{
			
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Blog/Error");
				app.UseHsts();
			}

			app.UseResponseCompression();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();

			app.UseMiddleware<UserActivityMiddleware>();


			return app;

		}

		//Thêm dữ liêu mẫu vào CSDL
		public static IApplicationBuilder UseDataSeeder(
			this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();

			try
			{
				scope.ServiceProvider
					.GetRequiredService<IDataSeeder>()
					.Initialize();
			}
			catch (Exception ex)
			{
				scope.ServiceProvider
					.GetRequiredService<ILogger<Program>>()
					.LogError(ex, "Could not insert data into database");
			}

			

			return app;
		}
	}
}
