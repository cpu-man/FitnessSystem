using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FitnessProgram;

public class Fitness
{
    private readonly List<Member> memberList = new List<Member>(); //liste hvor de forskellige medlemmer bliver tilføjet ind i

    public Fitness()
    {
        // Opretter medlemmer med givende id, navn og køn, bliver brugt til login
        memberList.Add(new Member(0, "admin", 'M', "Admin"));
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
    } //=> memberList;

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

    // SIDNEY MEMBER FILE

    public List<string> MemberFromFile()
    {
        string filePath = @"MemberList.txt";
        string[] members = File.ReadAllLines(filePath);
        return new List<string>(members);
    }

    public List<string> ActivityFromFile()
    {
        string filePath = @"ActivityList.txt";
        string[] activities = File.ReadAllLines(filePath);
        return new List<string>(activities);
    }

    // SIDNEY MEMBER FILE
}