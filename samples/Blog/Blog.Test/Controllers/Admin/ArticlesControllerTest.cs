﻿namespace Blog.Test.Controllers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blog.Controllers;
    using Blog.Data.Models;
    using Data;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using Services.Models;
    using Xunit;

    using ArticlesController = Web.Areas.Admin.Controllers.ArticlesController;

    public class ArticlesControllerTest
    {
        [Fact]
        public void ControllerShouldBeInAdminArea()
            => MyController<ArticlesController>
                .ShouldHave()
                .Attributes(attrs => attrs
                    .SpecifyingArea(ControllerConstants.AdministratorArea)
                    .RestrictingForAuthorizedRequests(ControllerConstants.AdministratorRole));

        [Fact]
        public void AllShouldReturnViewWithAllArticles()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(ArticleTestData.GetArticles(1, isPublic: false)))
                .Calling(c => c.All())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<ArticleNonPublicListingServiceModel>>()
                    .Passing(articles => articles
                        .Should()
                        .NotBeEmpty()));

        [Fact]
        public void ChangeVisibilityShouldChangeArticleAndRedirectToAll()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(ArticleTestData.GetArticles(1, isPublic: false)))
                .Calling(c => c.ChangeVisibility(1))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Article>(set => set
                        .FirstOrDefault(a => a.IsPublic)
                        .Should()
                        .NotBeNull()
                        .And
                        .Subject
                        .As<Article>()
                        .PublishedOn
                        .Should()
                        .NotBeNull()
                        .And
                        .Should()
                        .Be(new DateTime(1, 1, 1))))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c => c.All()));
    }
}
