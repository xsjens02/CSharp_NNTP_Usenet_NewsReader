namespace UsenetProgram.Models
{
    public class Group
    {
        public string Name { get; set; }
        public bool Postable { get; set; }

        public Group(string name, bool postable)
        {
            this.Name = name;
            this.Postable = postable;
        }
    }
}
