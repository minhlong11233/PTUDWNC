using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

var context = new BlogDbContext();


IBlogRepository blogRepo = new BlogRepository(context);
Console.WriteLine("{0,-5}{1,-50}{2,10}",
    "id", "name", "count");



