using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Members
{
    public interface IMemberRepository
    {
        List<Member> GetMembers();
        Member GetMemberById(int memberId);
        void InsertMember(Member member);
        void DeleteMember(Member m);
        void UpdateMember(Member member);
        Boolean RegisterOrNot(string Email);
        Member Login(string Email, string Password);
    }
}
