﻿namespace Blog.Test.Controllers
{
    using System.Collections.Generic;
    using Blog.Controllers;
    using Data;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using Services.Models;
    using Xunit;

    public class HomeControllerTest
    {        
        [Theory]
        [InlineData(2, true, 2)]
        [InlineData(4, true, 3)]
        [InlineData(4, false, 0)]
        public void IndexShouldReturnDefaultViewWithCorrectModel(int total, bool arePublic, int expected)
            => MyController<HomeController>
                .Instance(instance => instance
                    .WithData(ArticleTestData.GetArticles(total, arePublic)))
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<ArticleListingServiceModel>>()
                    .Passing(articles => articles
                        .Should()
                        .HaveCount(expected)));

        [Fact]
        public void PrivacyShouldReturnDefaultView()
            => MyController<HomeController>
                .Calling(c => c.Privacy())
                .ShouldReturn()
                .View();
    }
}
