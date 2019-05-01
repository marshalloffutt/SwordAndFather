namespace SwordAndFather.Models
{
    public class Assassin
    {
        public Assassin(string codeName, string catchphrase, string preferredWeapon)
        {
            CodeName = codeName;
            Catchphrase = catchphrase;
            PreferredWeapon = preferredWeapon;
        }

        public int Id { get; set; }
        public string CodeName { get; set; }
        public string Catchphrase { get; set; }
        public string PreferredWeapon { get; set; }
        public int Rating { get; set; }
        public decimal StandardFee { get; set; }
    }
}