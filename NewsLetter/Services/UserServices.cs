using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NewsLetter.Models;
using NewsLetter.Models.Context;
using RandomNameGeneratorLibrary;

namespace NewsLetter.Services
{
    public class UserServices
    {
        private readonly DataContext _db;
        public String authorizationId { get; set; }


        public UserServices()
        {
            _db = new DataContext();
        }
        public UserServices(string authorizationVal)
        {
            this.authorizationId = authorizationVal;
        }

        public User GetUser(String authToken)
        {
            return _db.Users.FirstOrDefault(sh => sh.AuthToken == authToken);
        }

        public List<User> GetAllUsers(){
            var users = _db.Users.ToList();
            return users;
        }

        public void RegisterUser(User user)
        {
            // First we check if another user exists with the same username
            var users = _db.Users.ToList();
            if (users.Find(x => x.Username == user.Username) != null)
                throw new IOException("username already exists");

            // Then we check is password satisfies password policies
            if (!PasswordPoliciesSatisfied(user.Password))
                throw new IOException("Password Policies not satisfied");

            user.Status = "INACTIVE";
            user.Password = EncryptPassword(user.Password);

            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void SetUserStatus(User user, string status){
             user.Status = status;
            _db.SaveChanges();
        }

        public void SetUserActive(int id){
            User user_from_id = _db.Users.FirstOrDefault(sh => sh.Id == id);

            if (user_from_id is null)
                throw new IOException("User ID not found");
            SetUserStatus(user_from_id, "ACTIVE");
        }

        public void SetUserRole(User user, string role){
             user.Role = role;
            _db.SaveChanges();
        }

        public void changePassword(String oldp, String newp){
            if (oldp == null || newp == null){
                throw new IOException("Empty passwords");
            }
            else if (oldp == newp){
                throw new IOException("New password should not be the same with old one");
            }
            else {
                 User currentUser = _db.Users.FirstOrDefault(sh => sh.AuthToken == authorizationId);

                 if (currentUser == null){
                    throw new IOException("Wrong auth user");
                 }
                 else if (currentUser.Password != EncryptPassword(oldp)){
                    throw new IOException("Wrong old password");
                 }
                 else{
                    currentUser.Password = EncryptPassword(newp);
                    _db.Users.Update(currentUser);
                }
            }
        }

        public void setRole(int id, String role){
            User user_from_id = _db.Users.FirstOrDefault(sh => sh.Id == id);
            if (user_from_id is null)
                throw new IOException("User ID not found");
            
            SetUserRole(user_from_id, role);
        }

        public void SetUserInActive(int id){
            User user_from_id = _db.Users.FirstOrDefault(sh => sh.Id == id);

            if (user_from_id is null)
                throw new IOException("User ID not found");
            SetUserStatus(user_from_id, "INACTIVE");
        }

        public String MakeAdmin(int id){
            User user_from_id = _db.Users.FirstOrDefault(sh => sh.Id == id);
            if (user_from_id is null)
                return "false";
            
            user_from_id.Role = "admin";
             _db.SaveChanges();
            return "OK";

        }

        public String LoginUser(User user)
        {
            // First we find the user with the exact username and password
            User toLogIn = _db.Users.FirstOrDefault(sh =>
                sh.Username == user.Username &&
                sh.Password == EncryptPassword(user.Password)
            );

            if (toLogIn is null)
                throw new IOException("Username or password mismatch");

            // We need also to check if user is inactive during login
            if (toLogIn.Status == "INACTIVE")
                throw new IOException("User is inactive");

            // Create an authentication token
            toLogIn.AuthToken = Guid.NewGuid().ToString();
            toLogIn.TokenExpire = DateTime.Now.AddDays(1);

            _db.SaveChanges();

            return toLogIn.AuthToken;
        }

        private string EncryptPassword(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte t in result)
            {
                stringBuilder.Append(t.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        private bool PasswordPoliciesSatisfied(string userPassword)
        {
            if (userPassword.Length < 8)
                return false;

            if (!userPassword.Any(char.IsUpper))
                return false;

            return hasSpecialChar(userPassword);
        }

        private static bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
                if (input.Contains(item)) return true;

            return false;
        }

        public void UpdateUser(User user)
        {
            // Check if the user is the same that updates the request
            User currentUser = _db.Users.FirstOrDefault(sh => sh.AuthToken == authorizationId);

            // Find the user
            User toUpdate = _db.Users.FirstOrDefault(sh => sh.Id == user.Id);

            if (currentUser.AuthToken != toUpdate.AuthToken)
                throw new Exception("Could not update user");

            if (toUpdate is null)
                throw new IOException("User not found");

            toUpdate.Username = user.Username;
            toUpdate.Password = EncryptPassword(user.Password);
            _db.Users.Update(toUpdate);

        }




    }
}