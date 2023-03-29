using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
	public static class AuthorEndpoints
	{
		/*public static WebApplication MapAuthorEndpoints(
			this WebApplication app)
		{
			return app;
		}*/

		public static async Task<IResult> GetAuthors(
			[AsParameters] AuthorFilterModel model,
			IAuthorRepository authorRepository)
		{
			var authorsList = await authorRepository
				.GetPagedAuthorsAsync(model, model.Name);
			var paginationResult =
				new PaginationResult<AuthorItem>(authorsList);
			return Results.Ok(paginationResult);
		}

		public static WebApplication MapAuthorEndpoints(
			this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/author");

			routerGroupBuilder.MapGet("/", GetAuthors)
				.WithName("GetAuthors")
				.Produces<PaginationResult<AuthorItem>>();

			return app;
		}
	}
}
