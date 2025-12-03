using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessProgram;

namespace FitnessProgram
{
    public class Fitness
    {
        List<Member> memberList = new List<Member>(); //liste hvor de forskellige medlemmer bliver tilføjet ind i
        public Dictionary<string, List<Member>> activityMembers = new Dictionary<string, List<Member>>();


        public Fitness()
        {

            //Opretter medlemmer med givende id, navn og køn
            memberList.Add(new Member(1, "Mathias", 'M'));
            memberList.Add(new Member(2, "Anders", 'M'));
            memberList.Add(new Member(3, "Sofie", 'F'));
            memberList.Add(new Member(4, "Caroline", 'F'));
            memberList.Add(new Member(5, "Rasmus", 'M'));
            memberList.Add(new Member(6, "Johan", 'M'));
            memberList.Add(new Member(7, "Ida", 'F'));
            memberList.Add(new Member(8, "Emma", 'F'));
            memberList.Add(new Member(9, "Victor", 'M'));
            memberList.Add(new Member(10, "Gertrud", 'F'));
            memberList.Add(new Member(11, "Richard", 'M'));
            memberList.Add(new Member(12, "Lasse", 'M'));
            memberList.Add(new Member(13, "Maya", 'F'));
            memberList.Add(new Member(14, "Victoria", 'F'));
            memberList.Add(new Member(15, "Magnus", 'M'));

        }

        public List<Member> GetAllMembers()
        {
            return memberList;
        }

        // Login - Philip 
        public Member Authenticate(string username, string password)
        {
            if (!int.TryParse(password, out int id))
                return null;

            return memberList.FirstOrDefault(m =>
            m.name.Equals(username, StringComparison.OrdinalIgnoreCase)
            && m.id == id);
        }


        // REGISTER (opret nyt medlem) - Philip
        public Member Register(string name, char gender, int age)
        {
            int newId = (memberList.Count > 0) ? memberList.Max(m => m.id) + 1 : 1;

            Member newMember = new Member(newId, name, gender);
            memberList.Add(newMember);

            return newMember;
        }

        public void CreateActivity()
        {
            ActivityWindow activity = new ActivityWindow();
            
        }


        public void ActivityMemberCount()
        {
            activityMembers["Yoga"] = new List<Member> { memberList[0], memberList[1] }; 
            activityMembers["Boxing"] = new List<Member> { memberList[0], memberList[3] };
        }
        public List<Member> GetMembersForActivity(string activityName)
        {
            return activityMembers.TryGetValue(activityName, out var list) ? list : new List<Member>();
        }
    }
}
