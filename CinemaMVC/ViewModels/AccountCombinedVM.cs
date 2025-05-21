namespace CinemaMVC.ViewModels
{
    public class AccountCombinedVM
    {
        public RegisterVM RegisterVM { get; set; } = new();
        public LoginVM LoginVM { get; set; } = new();
    }
}
