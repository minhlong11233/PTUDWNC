using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureCors()
		.ConfigureNlog()
		.ConfigureServices()
		.ConfigureMapster()
		.ConfigureFluentValidation()
		.ConfigureSwaggerOpenApi();
}
var app = builder.Build();
{
	app.SetupRequestPiqeline();
	app.MapAuthorEndpoints();
	app.Run();
}
