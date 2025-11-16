using library_mangment.library.domain.entities;

namespace library_mangment.library.domain.@interface
{
    public interface IMemberRepo
    {
        public bool UpdateMember(member updatedMember);
        ICollection<member> Getmembers();
        public member? GetMemberByID(int id);
        bool CreatMember(member member);
        public member? GetMemberByEmail(string email);
    }
}
