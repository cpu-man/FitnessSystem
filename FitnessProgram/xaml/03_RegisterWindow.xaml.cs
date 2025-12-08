using FitnessProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitnessProgram 
{ 
    //Philip Kode 
    public partial class RegisterWindow : Window
    {
        private Fitness _fitness;

        public RegisterWindow(Fitness fitness)
        {
            InitializeComponent();
            _fitness = fitness;
        }


        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            string name = NameInput.Text;
            char gender = GenderInput.Text.Length > 0 ? GenderInput.Text[0] : 'M';
            if (!int.TryParse(AgeInput.Text, out int age))
            {
                MessageBox.Show("Alder skal være et tal.");
                return;
            }

            Member newMember = _fitness.Register(name, gender, age);
            SaveMemberToFile(newMember); //Kalder på metoden
            MessageBox.Show($"Bruger oprettet! Dit ID og Adgangskode er {newMember.id}");
            this.Close();
        }

        private void SaveMemberToFile(Member member) //Metode der gemmer ny medlem i text filen, tager den nye medlem som input -- Sidney
        {
            string filePath = @"MemberList.txt"; //Gemmer stien til textfilen
            string m = $"ID: {member.id}, Navn: {member.name}, Køn: {member.gender}"; //Opretter ny string med medlemmets infomation
            File.AppendAllText(filePath, Environment.NewLine + m + Environment.NewLine); //Bliver gemt i filen

        }
    }
} 