namespace TemplateProductName.Domain.Services
{
    public interface IPasswordService
    {
        HashedPassword HashPassword(string plaintextPassword);
        bool VerifyPassword(HashedPassword expected, string plaintextPassword);
    }
}