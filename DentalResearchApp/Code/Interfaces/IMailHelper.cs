namespace DentalResearchApp.Code.Interfaces
{
    public interface IMailHelper
    {
        void SendMail(string receiverAdress, string messageSubject, string messageBody);
    }
}
