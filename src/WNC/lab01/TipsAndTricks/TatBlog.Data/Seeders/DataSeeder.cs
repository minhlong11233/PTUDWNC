﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;
        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Posts.Any()) return;
            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jason Mouth",
                    UrlSlug="jason-mouth",
                    Email="json@gmail.com",
                    JoinedDate=new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug="jessica-wonder",
                    Email="jessica665@gmail.com",
                    JoinedDate=new DateTime(2020, 4, 19)
                },

                new()
                {
                    FullName = "Nguyen Minh Thien",
                    UrlSlug="nguyen-minh-thien",
                    Email="thiennm@gmail.com",
                    JoinedDate=new DateTime(2016, 2, 28)
                },

                new()
                {
                    FullName = "Do Huynh Phuong",
                    UrlSlug="do-huynh-phuong",
                    Email="phuongdo@gmail.com",
                    JoinedDate=new DateTime(2002, 2, 15)
                }

            };

            _dbContext.Author.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;

        }
        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new() {Name= ".NET Core", Description=".NET Core",UrlSlug="dotnet-core"},
                new() { Name = "Architecture", Description = "Architecture", UrlSlug = "Architecture" },
                new() {Name= "Messaging", Description="messaging",UrlSlug="Messaging"},
                new() {Name= "OOP", Description="OOP",UrlSlug="OOP"},
                new() {Name= "Design Patterns", Description="Design Patterns",UrlSlug="Design-patterns"},
                new() {Name="Visual", Description="Visual",UrlSlug="Visual"}

        };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }
        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                new() {Name="   ",Description="Google", UrlSlug="Google"},
                new() {Name="ASP .Net MVC",Description="ASP .Net MVC", UrlSlug="ASP .Net MVC"},
                new() {Name="Razor Page",Description="Razor Page", UrlSlug="Razor-page"},
                new() {Name="Blazor",Description="Blazor", UrlSlug="Blazor"},
                new() {Name="Deep Learning",Description="Deep Learning", UrlSlug="Deep-learning"},
                new() {Name="Neural Network",Description="Neural Network", UrlSlug="Neural network"},
                new() {Name="YTB", Description="YTB", UrlSlug="YTB"}
            };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();
            return tags;
        }

        private IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories,
            IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title="ASP.Net Core Diagnostic Scenarios",
                    ShortDescription="David and friends has great reposibility",
                    Description="Here's a few great DON'T and DO examples",
                    Meta="David and friends has great reposibility filler",
                    UrlSlug="aspdotnet-core-diagnostic-scenarios",
                    Published=true,
                    PostedDate=new DateTime(2021,9,30,10,20,0),
                    ModifiedDate=null,
                    ViewCount=10,
                    Author=authors[0],
                    Category=categories[0],
                    Tags=new List<Tag>()
                    {
                        tags[0]
                    }


                }
            };
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }

    }
}