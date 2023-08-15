namespace BooksLibrary.Web.Utils
{
    public class WebAlertsUtil
    {
        public void ShowAlert(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowAlertError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }
    }
}
