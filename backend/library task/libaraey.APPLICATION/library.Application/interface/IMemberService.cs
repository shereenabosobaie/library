using library_mangment.library.Application.dto;
using library_mangment.library.domain.entities;

namespace library_mangment.library.Application.@interface
{
    public interface IMemberService
    {
        ICollection<MemberDto> GetAllMembers();
        bool Register(MemberDto dto);
        string? Login(logindto dto);
        public member? GetById(int id);
        public bool UpdateMemberInfo(MemberDto dto);
    }
}
