using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Mapsters;
using WebApp.Extensions;


var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureMvc()
		.ConfigureServices()
		.ConfigureMapster();
}

var app = builder.Build();
{
	app.UseRequestPipeline();
	app.UseBlogRoutes();
	app.UseDataSeeder();
}

app.Run();
