using Microsoft.EntityFrameworkCore;
using NewsLetter.Authorization;
using NewsLetter.Models;
using NewsLetter.Models.Context;
using NewsLetter.Models.DataTransferObjects;


namespace NewsLetter.Services
{
    public class NewsService
    {
        public String authorizationId { get; set; }
        private readonly DataContext _db = new();
        private readonly NewsService newsService;
        private readonly UserServices userServices = new();


        public NewsService()
        {
        }

        public User getCurUser(){
            return _db.Users.FirstOrDefault(sh => sh.AuthToken == RoleAccessAttribute.authorizationId);
        }

        public void CreateNews(News news)
        {
            news.Status = NewsStatus.CREATED.ToString();
            news.CreatationDate = DateTime.Now;
            news.Comments = new List<Comment>();

       
            if (news.Topic is not null)
            {
                Console.WriteLine("Heee");
                List<Topic> topics = new List<Topic>();
                foreach (var topicst in news.Topic){
                    Topic ptopic = _db.Topic.FirstOrDefault(sh => sh.Name == topicst.Name);
                    Console.WriteLine(topicst.Name);
                    Console.WriteLine(ptopic.Name);
                    topics.Add(ptopic); 
                    
                }
                news.Topic = topics;
            }


            // We are getting the creator from each authorization system
            news.Owner = getCurUser();

            _db.News.Add(news);
            _db.SaveChanges();
        }

        public List<NewsMod> GetAllNews(){
             var news = _db.News
                .Include(sh => sh.Owner)
                .Include(sh => sh.Topic)
                .Select(t=>new NewsMod{Id=t.Id,Title=t.Title,Content=t.Content,Status=t.Status.ToString(), CreatationDate=t.CreatationDate,PublicationDate=t.PublicationDate,Comments=t.Comments.Select(obj => obj.Content).ToList(), Topic=t.Topic.Select(obj=>obj.Name).ToList(), Owner=t.Owner.Username})
                .ToList();

            return news;
        }

        public void UpdateNews(int id, News News)
        {
            var currentNews = _db.News.FirstOrDefault(sh => sh.Id == id);
            if (currentNews is null)
            {
                throw new IOException("News not found");
            }
            if (currentNews.Status != NewsStatus.CREATED.ToString())
                throw new IOException("Cannot update " + currentNews.Status + " News");

            currentNews.Title = News.Title;
            currentNews.Content = News.Content;
            if (News.Topic is not null)
            {
                Console.WriteLine("Heee");
                List<Topic> topics = new List<Topic>();
                foreach (var topicst in News.Topic){
                    Topic ptopic = _db.Topic.FirstOrDefault(sh => sh.Name == topicst.Name);
                    Console.WriteLine(topicst.Name);
                    Console.WriteLine(ptopic.Name);
                    topics.Add(ptopic); 
                    
                }
                currentNews.Topic = topics;
            }
            _db.News.Update(currentNews);
            _db.SaveChanges();
        }

        public List<string> getNewsDescription(string keyword)
        {
            var filteredNews = from c in _db.News
                where EF.Functions.Like(c.Content, "[" + keyword + "]%")
                select c;

            return filteredNews.ToList().Select(s => s.Content).ToList();
        }

        public bool submitNews(int id){
            var currentNews = _db.News.FirstOrDefault(sh => sh.Id == id);
            if (currentNews is null) throw new IOException("News does not exist");
            string status = currentNews.Status;
            // We are checking if the user gave a valid Status 
            if (!Enum.IsDefined(typeof(NewsStatus), status.ToUpper())) throw new IOException("Invalid status");

            if (status.Equals(NewsStatus.CREATED.ToString())){
                currentNews.Status = NewsStatus.APPLIED.ToString();
                _db.News.Update(currentNews);
                _db.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool approveNews(int id){
            var currentNews = _db.News.FirstOrDefault(sh => sh.Id == id);
            if (currentNews is null) throw new IOException("News does not exist");
            string status = currentNews.Status;
            // We are checking if the user gave a valid Status 
            if (!Enum.IsDefined(typeof(NewsStatus), status.ToUpper())) throw new IOException("Invalid status");

            if (status.Equals(NewsStatus.APPLIED.ToString())){
                currentNews.Status = NewsStatus.ACCEPTED.ToString();
                _db.News.Update(currentNews);
                _db.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool publishNews(int id){
            var currentNews = _db.News.FirstOrDefault(sh => sh.Id == id);
            if (currentNews is null) throw new IOException("News does not exist");
            string status = currentNews.Status;
            // We are checking if the user gave a valid Status 
            if (!Enum.IsDefined(typeof(NewsStatus), status.ToUpper())) throw new IOException("Invalid status");

            if (status.Equals(NewsStatus.ACCEPTED.ToString())){
                currentNews.Status = NewsStatus.PUBLISHED.ToString();
                _db.News.Update(currentNews);
                _db.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool rejectNews(int id){
            var currentNews = _db.News.FirstOrDefault(sh => sh.Id == id);
            if (currentNews is null) throw new IOException("News does not exist");
            string status = currentNews.Status;
            
            if (status.Equals(NewsStatus.APPLIED.ToString())){
                currentNews.Status = NewsStatus.CREATED.ToString();
                _db.News.Update(currentNews);
                _db.SaveChanges();
                return true;
            }
            else return false;
        }

        public List<News> SearchNews(string[] searchw){
            
           var results = _db.News.Where(i => i.Title.ToLower().Contains(searchw[0])
                  || i.Content.ToLower().Contains(searchw[0]));

            if(searchw.Length > 1) {
                for(int x = 1; x < searchw.Length; x++) {
                    string s = searchw[x];
                    results = results.Where(i => i.Title.ToLower().Contains(s)
                            || i.Content.ToLower().Contains(s));
                }
            }
            return results.ToList();
        }
          public News GetNews(long id)
        {
            News news = _db.News
                .FirstOrDefault(sh => sh.Id == id);
            if (news is null) throw new IOException("News Not found");
            return news;
        }

    
    }
}