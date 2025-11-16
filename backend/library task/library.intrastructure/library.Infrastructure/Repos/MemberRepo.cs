using library_mangment.library.domain.entities;
using library_mangment.library.domain.@interface;
using library_mangment.library.Infrastructure.appdbcontext;
using library_mangment.library.Infrastructure.@interface;
using Microsoft.EntityFrameworkCore;

namespace library_mangment.library.Infrastructure.Repos
{
    public class MemberRepo : IMemberRepo
    {
        private readonly Dbcontext context;

        public MemberRepo(Dbcontext context)
        {
            this.context = context;
        }

        public bool CreatMember(member member)
        {
           context.members.Add(member);
            return context.SaveChanges() > 0 ? true : false;
        }
        public member? GetMemberByEmail(string email)
        {
            return context.members.FirstOrDefault(m => m.Email == email);
        }
        public member? GetMemberByID(int id)
        {
            return context.members.FirstOrDefault(m => m.Id == id);
        }
        public ICollection<member> Getmembers()
        {
            return context.members.OrderBy(x => x.Id).ToList();
        }


        public bool UpdateMember(member updatedMember)
        {
            var existingMember = context.members.FirstOrDefault(m => m.Id == updatedMember.Id);
            if (existingMember == null)
                return false;

            existingMember.Name = updatedMember.Name ?? existingMember.Name;
            existingMember.Email = updatedMember.Email ?? existingMember.Email;
            existingMember.passwordHas = updatedMember.passwordHas ?? existingMember.passwordHas;
            existingMember.ImageUrl = updatedMember.ImageUrl ?? existingMember.ImageUrl;
            existingMember.role = updatedMember.role ?? existingMember.role;

            context.members.Update(existingMember);
            return context.SaveChanges() > 0;
        }
    }
}
