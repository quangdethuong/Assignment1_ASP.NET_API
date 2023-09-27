using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Members
{
    public class MemberRepository : IMemberRepository
    {
        public void DeleteMember(Member m) => MemberDAO.DeleteMember(m);
 
        public Member GetMemberById(int memberId) => MemberDAO.GetMemberById(memberId);

        public List<Member> GetMembers() => MemberDAO.GetMemberList();

        public void InsertMember(Member member) => MemberDAO.AddNew(member);

        public Member Login(string Email, string Password) => MemberDAO.MemberLogin(Email, Password);

        public bool RegisterOrNot(string Email) => MemberDAO.RegisterOrNot(Email);

        public void UpdateMember(Member member) => MemberDAO.Update(member);
    }
}
