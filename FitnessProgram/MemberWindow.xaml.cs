using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitnessProgram
{
    /// <summary>
    /// Interaction logic for MemberWindow.xaml
    /// </summary>
    public partial class MemberWindow : Window
    {
        Fitness fitness = new Fitness();

        public MemberWindow()
        {
            InitializeComponent();

            ShowMembers(); //Kalder på ShowMembers funktionen som printer medlemmerne ud
            
        }
        //Hello
        public void ShowMembers()
        {
            List<Member> localList = fitness.GetAllMembers();
            StringBuilder allMembers = new StringBuilder(); //Opretter en StringBuilder som samler alt fra listen på en hukommelseseffektiv måde
            //string allMembers = "";
            for (int i = 0; i < localList.Count; i++) //For løkke der printer alle medlemmer ud
            {
                Member member = localList[i];
                allMembers.AppendLine($"ID: {member.id} Navn: {member.name} Køn: {member.gender}"); //printer hvert variabel i hver sin linje, da StringBuilder gør det til én stor string
                
            }
            MemberBlock.Text = allMembers.ToString(); //Laver vores StringBuilder om til den endelige string og ligger det ind i vores TextBlock
        }

        public void RemoveMember()
        {
            if (int.TryParse(EnterMember.Text, out int memberID)) // Brugers input bliver konverteret til en int vi kalder for memberID
            {
                Member member = fitness.GetAllMembers().FirstOrDefault(member => member.id == memberID); //Finder et matchende ID; hvis der ikke bliver fundet noget får vi 'null'
                if (member != null) //Hvis den får et matchene id, kører denne del
                {
                    fitness.GetAllMembers().Remove(member); //Fjerner medlemmen
                    ShowMembers(); //Genindlæser listen
                    MessageBox.Show($"{memberID} slettet!"); //Besked til brugeren
                }
                else
                {
                    MessageBox.Show($"{memberID} ikke fundet, prøv igen"); //Hvis ID'et ikke matcher vises denne besked
                }
            }
            else
            {
                MessageBox.Show("Indtast venligst et tal"); //Hvis der ikke bliver tastet et tal ind, vises denne besked
            }
            
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NextWindow next = new NextWindow();
            next.Show();
            this.Close();
        }

        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveMember();
        }
    }
}
