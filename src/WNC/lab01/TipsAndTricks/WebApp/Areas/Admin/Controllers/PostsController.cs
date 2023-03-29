using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class PostsController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IBlogRepository _blogRepository;
		private readonly IMediaManager _mediaManager;
		private readonly ILogger<PostsController> _logger;
		private readonly IAuthorRepository _authorRepository;


		public PostsController(
			IBlogRepository blogRepository,
			IMediaManager mediaManager,
			IAuthorRepository authorRepository,
			IMapper mapper,
			ILogger<PostsController> logger)
		{
			_blogRepository = blogRepository;
			_mediaManager = mediaManager;
			_authorRepository = authorRepository;
			_mapper = mapper;
			_logger = logger;
		}

		private async Task PopulatePostFilterModelAsync( PostFilterModel model, IBlogRepository _blogRepository)
		{
			var authors = await _authorRepository.GetAuthorsAsync();
			var categories = await _blogRepository.GetCategoriesAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});
		}

		public async Task<IActionResult> Index(PostFilterModel model)
		{

			/*var postQuery = new PostQuery()
			{
				Keyword = model.Keyword,
				CategoryId = model.CategoryId,
				AuthorId = model.AuthorId,
				YearPost = model.Year,
				MonthPost = model.Month
			};*/

			_logger.LogInformation("tạo điều kiện truy vấn");
			var postQuery = _mapper.Map<PostQuery>(model);

			_logger.LogInformation("Lấy danh sách bài viết từ CSDL");

			ViewBag.PostsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, 1, 10);

			_logger.LogInformation("chuẩn bị dữ liệu cho ViewModel");

			await PopulatePostFilterModelAsync(model);

			return View(model);
		}

		private Task PopulatePostFilterModelAsync(PostFilterModel model)
		{
			throw new NotImplementedException();
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			var post = id > 0
				? await _blogRepository.GetPostByIdAsync(id, true)
				: null;

			var model = post == null
				? new PostEditModel()
				: _mapper.Map<PostEditModel>(post);

			await PopulatePostEditModelAsync(model);

			return View(model);
		}

		private Task PopulatePostEditModelAsync(PostEditModel model)
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(
			IValidator<PostEditModel> postValidator,
			PostEditModel model)
		{
			var validationResult = await postValidator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}
			if (!ModelState.IsValid)
			{
				await PopulatePostEditModelAsync(model);
				return View(model);
			}

			var post = model.Id > 0
				? await _blogRepository.GetPostByIdAsync(model.Id)
				: null;

			if (post == null)
			{
				post.Id = 0;
				post.PostedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, post);

				post.Category = null;
				post.ModifiedDate = DateTime.Now;
			}

			if (model.ImageFile?.Length > 0)
			{
				var newImagePath = await _mediaManager.SaveFileAsync(
					model.ImageFile.OpenReadStream(),
					model.ImageFile.FileName,
					model.ImageFile.ContentType);
				if (string.IsNullOrWhiteSpace(newImagePath))
				{
					await _mediaManager.DeleteFileAsync(post.ImageUrl);
					post.ImageUrl = newImagePath;
				}
			}

			await _blogRepository.CreateOrUpdatePostAsync(
				post, model.GetSelectedTags());

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyPostSlug(
			int id, string urlSlug)
		{
			var slugExisted = await _blogRepository
				.IsPostSlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"Slug '{urlSlug}' đã được sử dụng")
				: Json(true);
		}

		
	}
}
