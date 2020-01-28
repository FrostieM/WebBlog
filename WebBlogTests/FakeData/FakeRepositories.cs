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
                new Blog{Id = 1, User = users[0], Posts = new []
                {
                    new Post{Id = 1, Title = "title1", Description = "description1", Type = "testType", Likes = new List<PostLike>()},
                    new Post{Id = 4, Title = "title4", Description = "description4", Type = "testType", Likes = new List<PostLike>()},
                    new Post{Id = 5, Title = "title5", Description = "description5", Type = "testType", Likes = new List<PostLike>()},
                    new Post{Id = 6, Title = "title6", Description = "description6", Type = "testType2", Likes = new List<PostLike>()}
                }},
                new Blog{Id = 2, User = users[1], Posts = new []
                {
                    new Post{Id = 2, Title = "title2", Description = "description2", Type = "testType", Likes = new List<PostLike>()},
                    new Post{Id = 3, Title = "title3", Description = "description3", Type = "testType", Likes = new List<PostLike>()}
                }}
            };
        }

        public static IEnumerable<Post> GetFakePosts(IList<Blog> blogs)
        {
            
            return new[]
            {
                new Post
                {
                    Id = 1, Blog = blogs[0], Title = "title2", Description = "description2", Type = "testType", Likes = new []
                    { new PostLike { User = blogs[0].User} }
                },
                new Post{
                    Id = 2, Blog = blogs[1], Title = "title3", Description = "description3", Type = "testType", Likes = new []
                        { new PostLike { User = blogs[1].User} }
                },
                new Post{
                    Id = 3, Blog = blogs[1], Title = "title1", Description = "description1", Type = "testType", Likes = new []
                        { new PostLike { User = blogs[1].User} }
                },
                new Post{
                    Id = 4, Blog = blogs[0], Title = "title4", Description = "description4", Type = "testType", Likes = new []
                        { new PostLike { User = blogs[0].User} }
                },
                new Post{
                    Id = 5, Blog = blogs[0], Title = "title5", Description = "description5", Type = "testType", Likes = new []
                        { new PostLike { User = blogs[0].User} }
                
                },
                new Post{
                    Id = 6, Blog = blogs[0], Title = "title6", Description = "description6", Type = "testType2", Likes = new []
                        { new PostLike { User = blogs[0].User} }
                }
                
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

        public static IEnumerable<Comment> GetFakeComments(IList<Post> posts, IList<User> users)
        {
            var parentComment = new Comment { Id = 1, Content = "testComment0", Post = posts[0], User = users[0]};
            return new[]
            {
                parentComment,
                new Comment {Id = 2, Content = "testComment1", Post = posts[0], User = users[0], ParentComment = parentComment},
                new Comment {Id = 3, Content = "testComment2", Post = posts[1], User = users[0]},
                new Comment {Id = 4, Content = "testComment3", Post = posts[0], User = users[1], ParentComment = parentComment},
                new Comment {Id = 5, Content = "testComment4", Post = posts[0], User = users[0]},
                new Comment {Id = 6, Content = "testComment5", Post = posts[0], User = users[0]}
            };
        }
    }
}