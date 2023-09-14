using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using NewsLetter.Authorization;
using NewsLetter.Models;
using NewsLetter.Models.Context;


namespace NewsLetter.Services
{
    public class TopicService
    {
        private readonly DataContext _db = new DataContext();
        public string authorizationId  { get; set; }
       
        public TopicService()
        {
            //this.authorizationId = authorizationId;
        }

        public User getCurUser(){
            return _db.Users.FirstOrDefault(sh => sh.AuthToken == RoleAccessAttribute.authorizationId);
        }

        public void CreateTopic(Topic topic, String userown)
        {
            
            User own = _db.Users.FirstOrDefault(sh => sh.Username == userown);
            Console.WriteLine(userown);
            topic.Status = TopicStatus.CREATED.ToString();
            topic.CreationDate = DateTime.Now; //apothikeusi trexousas imerominias
            topic.Owner = getCurUser();

            Topic ptopic = _db.Topic.FirstOrDefault(sh => sh.Name == topic.ParentTopic.Name);
            topic.ParentTopic = ptopic;
            _db.Add(topic);
            _db.SaveChanges();
        }

        public Topic GetTopic(long id)
        {
            Topic topic = _db.Topic
                .Include(sh => sh.ParentTopic)
                .FirstOrDefault(sh => sh.Id == id);
            if (topic is null) throw new IOException("Topic Not found");
            return topic;
        }

        public void updateTopic(Topic topic_for_change)
        {
            _db.Topic.Update(topic_for_change);
            _db.SaveChanges();
        }

        public void SetParentTopic(long parentId, long childrenId)
        {
            Topic parentTopic = this.GetTopic(parentId);
            Topic childrenTopic = this.GetTopic(childrenId);
            childrenTopic.ParentTopic = parentTopic;
            _db.Update(childrenTopic);
            _db.SaveChanges();
        }

        public void RemoveTopic(long id)
        {
            Topic toDelete = GetTopic(id);
            _db.Remove(toDelete);
            _db.SaveChanges();
        }

        public void SetTopicStatus(long id, string status)
        {
            Topic topic = GetTopic(id);
            if (!Enum.IsDefined(typeof(TopicStatus), status.ToUpper()))
                throw new IOException("Theme status not avaliable");

            // If we accept a post just set it as accepted
            if (String.Equals(status.ToUpper(), TopicStatus.ACCEPTED.ToString()))
                topic.Status = status.ToUpper();

            // If we reject a post Delete it
            if (String.Equals(status.ToUpper(), TopicStatus.REJECTED.ToString()))
                RemoveTopic(id);

            _db.SaveChanges();
        }


        public List<TopicMod> GetTopics()
        {

            var topics = _db.Topic
                .Include(sh => sh.ParentTopic)
                .Include(sh => sh.Owner)
                .OrderByDescending(sh => sh.Status)
                .ThenByDescending(sh => sh.Name)
                .Select(t=>new TopicMod{Id=t.Id,Name=t.Name,CreationDate=t.CreationDate, ParentTopic=t.ParentTopic,Status=t.Status,Owner=t.Owner.Username})
                .ToList();

            return topics;
        }

    }
}