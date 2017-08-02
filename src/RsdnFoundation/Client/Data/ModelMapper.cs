namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Community;
    using Community.Interaction;
    using Storage;

    internal static class ModelMapper
    {
        public static ThreadModel CreateThreadModel(this DbPost post, DbThread thread, IEnumerable<DbRating> ratings,
            int userId, DbPost parent)
        {
            var threadModel =
                post.UserId == userId ? new PostActivityModel() :
                parent != null && parent.UserId == userId ? new ReplyActivityModel() :
                new ThreadModel();

            MapPost(threadModel, post);
            MapThread(threadModel, thread);
            MapRatings(threadModel, ratings);

            return threadModel;
        }

        private static void MapPost(ThreadModel threadModel, DbPost post)
        {
            // note that we don't map thread id here
            threadModel.Title = post.Title;
            threadModel.Excerpt = post.Message;
            threadModel.UserId = post.UserId;
            threadModel.Username = post.Username;
            threadModel.Updated = post.Updated ?? post.Posted;
        }

        private static void MapRatings(ThreadModel threadModel, IEnumerable<DbRating> ratings)
        {
            threadModel.InterestingCount = ratings.Count(r => r.Value == (int)VoteValue.Interesting);
            threadModel.ThanksCount = ratings.Count(r => r.Value == (int)VoteValue.Thanks);
            threadModel.ExcellentCount = ratings.Count(r => r.Value == (int)VoteValue.Excellent);
            threadModel.AgreedCount = ratings.Count(r => r.Value == (int)VoteValue.Agreed);
            threadModel.DisagreedCount = ratings.Count(r => r.Value == (int)VoteValue.Disagreed);
            threadModel.Plus1Count = ratings.Count(r => r.Value == (int)VoteValue.Plus1);
            threadModel.FunnyCount = ratings.Count(r => r.Value == (int)VoteValue.Funny);
        }

        private static void MapThread(ThreadModel threadModel, DbThread thread)
        {
            threadModel.Id = thread.ThreadId;
            threadModel.Viewed = thread.Viewed;
            threadModel.NewPostCount = thread.NewPostCount;
            threadModel.PostCount = thread.PostCount;
        }
    }
}