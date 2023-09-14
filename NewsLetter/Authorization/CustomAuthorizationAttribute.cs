using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using NewsLetter.Models;
using NewsLetter.Services;

namespace NewsLetter.Authorization
{
    public class RoleAccessAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserServices userServices = new();
        public static string authorizationId {get; set;}
        // private readonly TopicService topicService = new();
        // private readonly NewsService newsService = new();

        string[] authRole = null;

        public RoleAccessAttribute(params string[] authRole)
        {
            this.authRole = authRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Pairnoume to aitima kai kathe kefalida
            IHeaderDictionary headerDictionary = context.HttpContext.Request.Headers;

            // elegxoume tin exousiodotisi me to token
            StringValues authorizationId = headerDictionary["exousiodotisi"];
            
           

            // an o xristis den einai exousiodotimenos tote h diadikasia den ekteleitai
            if (String.IsNullOrEmpty(authorizationId)) throw new IOException("o xristis den einai exousiodotimenos");

            RoleAccessAttribute.authorizationId = authorizationId;
            userServices.authorizationId = authorizationId;
            //topicService.authorizationId = authorizationId;
            //newsService.authorizationId = authorizationId;

            // lambanoume ton xristi me vasi to token exousiodotisis pou katexei 
            User currentUser = userServices.GetUser(authorizationId);
            


            if (currentUser is null)
                throw new IOException("O xristis den vrethike");

            // elegxos gia to ean eleixe h exousiodotisi
            if (currentUser.TokenExpire < DateTime.Now)
                throw new IOException("Token expired , prepei na xanasindetheite");


            //elegxos an epitrepete h prosvasi gia autoy tou idous exousiodotimenou xristi
            if (!authRole.Contains(currentUser.Role.ToLower()))
                throw new IOException("O xristis den exei prosvasi se autin tin diadromi");
        }
    }
}