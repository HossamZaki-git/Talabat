namespace Admin_Panel.Models.UserVMs
{
    public class UsersVM
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<string> Roles { get; set; }
    }
}
