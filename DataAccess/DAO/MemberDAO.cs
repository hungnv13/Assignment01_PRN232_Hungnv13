using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class MemberDAO
    {
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();

        private MemberDAO() { }

        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new MemberDAO();
                    return instance;
                }
            }
        }

        public List<Member> GetMemberList()
        {
            using var context = new ClothesOrderDbContext();
            return context.Members.ToList();
        }

        public Member GetMemberById(int id)
        {
            using var context = new ClothesOrderDbContext();
            return context.Members.FirstOrDefault(m => m.MemberId == id);
        }

        public void AddMember(Member m)
        {
            using var context = new ClothesOrderDbContext();
            int maxId = context.Members.Any()
                ? context.Members.Max(x => x.MemberId) : 0;

            m.MemberId = maxId + 1;
            context.Members.Add(m);
            context.SaveChanges();
        }

        public void UpdateMember(Member m)
        {
            using var context = new ClothesOrderDbContext();
            context.Members.Update(m);
            context.SaveChanges();
        }

        public void DeleteMember(int id)
        {
            using var context = new ClothesOrderDbContext();
            var member = context.Members.Find(id);
            if (member != null)
            {
                context.Members.Remove(member);
                context.SaveChanges();
            }
        }

        public Member Login(string email, string password)
        {
            using var context = new ClothesOrderDbContext();
            return context.Members.FirstOrDefault(m => m.Email == email && m.Password == password);
        }
    }
}
