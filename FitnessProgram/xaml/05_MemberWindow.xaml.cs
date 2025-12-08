using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FitnessProgram
{
    public partial class MemberWindow : Window
    {
        //private readonly Fitness _fitness; // shared Fitness system
        private readonly Fitness fitness; // Shared fitness system
        private readonly Member member;   // Logged in user
        public List<string> _localList;
        public MemberWindow(Fitness fitness, Member member)
        {
            InitializeComponent();
            //_fitness = fitness;
            this.fitness = fitness;
            this.member = member;
            this._localList = fitness.MemberFromFile().ToList();
            ShowMembers(); // Kalder ShowMembers for at vise medlemmerne
        }

        // --- Show all members in TextBlock ---
        public void ShowMembers()
        {
            /*List<Member> localList = _fitness.GetAllMembers();
            StringBuilder allMembers = new StringBuilder();

            for (int i = 0; i < localList.Count; i++)
            {
                Member member = localList[i];
                allMembers.AppendLine($"ID: {member.id} Navn: {member.name} Køn: {member.gender}");
            }

            MemberBlock.Text = allMembers.ToString();*/
            StringBuilder allMembers = new StringBuilder();

            for (int i = 0; i < _localList.Count; i++)
            {
                allMembers.AppendLine(_localList[i]);  // viser 1, 2, 3... i stedet for 0, 1, 2...
            }
            MemberBlock.Text = allMembers.ToString();
        }

        // --- Remove member by ID ---
        public void RemoveMember()
        {
            if (int.TryParse(EnterMember.Text, out int memberID))
            {
                int memberIndex = memberID - 1;
                if (memberIndex >= 0 && memberIndex < _localList.Count)
                {
                    _localList.RemoveAt(memberIndex);
                    //File.WriteAllLines(@"MemberList.txt", localList);
                    ShowMembers();
                    MessageBox.Show($"{memberIndex} er blevet slettet!");
                }
                else
                {
                    MessageBox.Show($"{memberIndex} findes ikke, prøv igen");
                }
            }
            else
            {
                MessageBox.Show("Indtast venligst et tal");
            }


        }



        // --- Back button ---
        // BACK BUTTON HANDLER
        private void GoToNextWindow_Click(object sender, RoutedEventArgs e)
        {
            NextWindow next = new NextWindow(member, fitness);
            next.Show();
            this.Close();
        }

        // --- Delete member button ---
        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveMember();
        }
    }
}
