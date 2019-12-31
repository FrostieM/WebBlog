using System.Collections.Generic;
using WebBlog.Model;

namespace WebBlogTests.FakeData
{
    public static class FakeRepositories
    {
        public static IEnumerable<User> FakeUsers => new[]
        {
            new User
            {
                Id = "test1", UserName = "test1", Password = "testPassword1", Email = "testEmail1",
                FirstName = "testName1", LastName = "testLastName1"
            },
            new User
            {
                Id = "test2", UserName = "test2", Password = "testPassword2", Email = "testEmail2",
                FirstName = "testName2", LastName = "testLastName2"
            }
        };

        public static IEnumerable<Blog> GetFakeBlogs(IList<User> users)
        {
            return new[]
            {
                new Blog{Id = 1, User = users[0]},
                new Blog{Id = 2, User = users[1]}
            };
        }

        public static IEnumerable<Post> GetFakePosts(IList<Blog> blogs)
        {
            return new[]
            {
                new Post{Id = 1, Blog = blogs[0], Title = "title1", Description = "description1", Type = "testType"},
                new Post{Id = 2, Blog = blogs[1], Title = "title2", Description = "description2", Type = "testType"},
                new Post{Id = 3, Blog = blogs[1], Title = "title3", Description = "description3", Type = "testType"},
                new Post{Id = 4, Blog = blogs[0], Title = "title4", Description = "description4", Type = "testType"},
                new Post{Id = 5, Blog = blogs[0], Title = "title5", Description = "description5", Type = "testType"},
                new Post{Id = 6, Blog = blogs[0], Title = "title6", Description = "description6", Type = "testType2"}
            };
        }

        public static IEnumerable<PostLike> GetFakePostLikes(IList<Post> posts, IList<User> users)
        {
            return new[]
            {
                new PostLike {Id = 1, Post = posts[0], User = users[0]},
                new PostLike {Id = 2, Post = posts[1], User = users[0]},
                new PostLike {Id = 3, Post = posts[0], User = users[1]},
                new PostLike {Id = 4, Post = posts[1], User = users[1]},
                new PostLike {Id = 5, Post = posts[2], User = users[0]}
            };
        }
    }
}