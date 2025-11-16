using library_mangment.library.domain.entities;

namespace library_mangment.library.Application.@interface
{
    public interface IauthService
    {
        string createtoken(member member);
        public int? ValidateToken(string token);
    }
}
