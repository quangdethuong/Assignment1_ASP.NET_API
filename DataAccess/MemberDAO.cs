using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
       

        public static List<Member> GetMemberList()
        {
            var members = new List<Member>();
            try
            {
                using var context = new MyDbContext();
                members = context.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

     


        public static Member GetMemberById(int MemberId)
        {
            Member member = null;
            try
            {
                using var context = new MyDbContext();
                member = context.Members.SingleOrDefault(c => c.MemberId == MemberId);
                if (member != null)
                {
                    var e = context.Entry(member);
                    e.Collection(c => c.orders).Load();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public static Member MemberLogin(string Email, string Password)
        {
            Member m = new Member();
            try
            {
                using (var context = new MyDbContext())
                {
                    m = context.Members.SingleOrDefault(x => x.Email == Email && x.Password == Password);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return m;
        }

        public static void AddNew(Member m)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Members.Add(m);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(Member member)
        {
            try
            {
                Member _member = GetMemberById(member.MemberId);
                if (_member != null)
                {
                    using var context = new MyDbContext();
                    context.Members.Update(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The member does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteMember(Member m)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var m1 = context.Members.SingleOrDefault(c => c.MemberId == m.MemberId);
                    context.Members.Remove(m1);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Boolean RegisterOrNot(string Email)
        {
            Member member = null;
            try
            {
                using var context = new MyDbContext();
                member = context.Members.SingleOrDefault(c => c.Email == Email);
                if (member != null)
                {
                    var e = context.Entry(member);
                    e.Collection(c => c.orders).Load();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
