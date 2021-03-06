﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebBlog.Controllers;
using WebBlog.Model;
using WebBlog.Model.Forms;
using WebBlog.Model.Interfaces.Repositories;
using WebBlog.Model.ViewData;
using WebBlogTests.FakeData;

using Xunit;

namespace WebBlogTests
{
    public class PostsControllerTests
    {
        private readonly Mock<IPostRepository> _postRepository = new Mock<IPostRepository>(); 
        private readonly Mock<IPostLikeRepository> _postLikeRepository = new Mock<IPostLikeRepository>();
        private readonly Mock<IBlogRepository> _blogRepository = new Mock<IBlogRepository>();
        
        public PostsControllerTests()
        {
            var users = FakeRepositories.FakeUsers.ToList();
            var blogs = FakeRepositories.GetFakeBlogs(users).ToList();
            var posts = FakeRepositories.GetFakePosts(blogs).ToList();
            var postsLikes = FakeRepositories.GetFakePostLikes(posts, users);
            
            _blogRepository.Setup(c => c.Blogs).Returns(blogs.AsQueryable);
            
            _postRepository.Setup(c => c.Posts).Returns(posts.AsQueryable);

            _postLikeRepository.Setup(l => l.PostLikes).Returns(postsLikes.AsQueryable);
        }

        [Fact]
        public void CannotGetPosts()
        {
            var controller = new PostsController(
                _postRepository.Object,
                _postLikeRepository.Object, 
                _blogRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.GetPosts(new PostsInfo
            {
                Type = "testType",
                Username = "test0",
                CurrentPage = 1,
            }) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CanGetPosts()
        {
            var controller = new PostsController(
                _postRepository.Object,
                _postLikeRepository.Object, 
                _blogRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };
            
            var result = controller.GetPosts(new PostsInfo
            {
                Type = "testType",
                Username = "test1",
                CurrentPage = 1,
            }) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            var posts = result.Value as UserPostsViewData;
            Assert.IsType<UserPostsViewData>(posts);
            Assert.Equal(3, posts.Posts.Count());
        }

        [Fact]
        public void CanPaginatePosts()
        {
            var controller = new PostsController(
                _postRepository.Object,
                _postLikeRepository.Object, 
                _blogRepository.Object)
            {
                ItemsPerPage = 2,
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };

            var result = controller.GetPosts(new PostsInfo
            {
                Type = "testType",
                Username = "test1",
                CurrentPage = 1,
            }) as OkObjectResult;
            
            Assert.NotNull(result);

            var posts = result.Value as UserPostsViewData;
            Assert.NotNull(posts);
            Assert.Equal(1, posts.PagingInfo.CurrentPage);
            Assert.Equal(3, posts.PagingInfo.TotalItems);
            Assert.Equal(2, posts.PagingInfo.ItemsPerPage);
            Assert.Equal(2, posts.PagingInfo.TotalPages);

            var postsList = posts.Posts.ToList();
            Assert.Equal(2, postsList.Count);
            Assert.Equal(1, postsList[0].Item.Id);
            Assert.Equal(4, postsList[1].Item.Id);
        }

        [Fact]
        public void CannotSavePost()
        {
            var controller = new PostsController(
                _postRepository.Object,
                _postLikeRepository.Object, 
                _blogRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("testNotFound", "User")
            };

            var result = controller.SavePost(new PostForm
            {
                Description = "test description",
                Title = "test title",
                Type = "test type"
            }) as ObjectResult;
            
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CanSavePost()
        {
            var controller = new PostsController(
                _postRepository.Object,
                _postLikeRepository.Object, 
                _blogRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };

            var result = controller.SavePost(new PostForm
            {
                Description = "test description",
                Title = "test title",
                Type = "test type",
                Tags = "test1 test2"
            }) as ObjectResult;
            
            _postRepository.Verify(m =>
                m.SavePost(It.IsAny<Post>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("saved", result.Value);
        }

        [Fact]
        public void CannotDeletePost_UserNotFound()
        {
            var controller = new PostsController(
                _postRepository.Object, 
                _postLikeRepository.Object,
                _blogRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test0", "User")
            };

            var result = controller.DeletePost(1) as ObjectResult;
            
            _postRepository.Verify(m =>
                m.DeletePost(It.IsAny<Post>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("User not found", result.Value);
        }
        
        [Fact]
        public void CannotDeletePost_PostNotFound()
        {
            var controller = new PostsController(
                _postRepository.Object, 
                _postLikeRepository.Object,
                _blogRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };

            var result = controller.DeletePost(0) as ObjectResult;
            
            _postRepository.Verify(m =>
                m.DeletePost(It.IsAny<Post>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Post not found", result.Value);
        }
        
        [Fact]
        public void CanDeletePost()
        {
            var controller = new PostsController(
                _postRepository.Object, 
                _postLikeRepository.Object,
                _blogRepository.Object)
            {
                ControllerContext = FakeController.GetContextWithIdentity("test1", "User")
            };

            var result = controller.DeletePost(1) as ObjectResult;
            
            _postRepository.Verify(m =>
                m.DeletePost(It.IsAny<Post>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("deleted", result.Value);
        }
    }
}