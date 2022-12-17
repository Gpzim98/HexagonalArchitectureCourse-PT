namespace Domain.Entities
{
    public class User
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
