using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FitnessProgram
{
    /// <summary>
    /// Interaction logic for ActivityWindow.xaml
    /// </summary>
    public partial class ActivityWindow : Window
    {
        private readonly Fitness fitness; // Shared fitness system
        private readonly Member member;   // Logged in user

        // --- Constructor: MUST match Option 1 ---
        public ActivityWindow(Fitness fitness, Member member)
        {
            InitializeComponent();
            this.fitness = fitness;
            this.member = member;

            ShowActivity();
            ApplyRoleRestrictions(); // Hide admin controls if not admin
            ApplyRoleRestrictions1(); //Member leave or join activity
            UpdateAllCapacities(); //updater så man kan se hvor mange er på en aktivitet
        }

        // Hide admin-only controls
        private void ApplyRoleRestrictions()
        {
            if (member.role.ToLower() != "admin")
            {
                DeleteMemberButton.Visibility = Visibility.Collapsed;
                AddMemberButton.Visibility = Visibility.Collapsed;
                CreateActivity.Visibility = Visibility.Collapsed;
                EnterActivity.Visibility = Visibility.Collapsed;
                EnterMember.Visibility = Visibility.Collapsed;
            }
        }

      
        public void ShowActivity() //Oprettelse af aktiviteter
        {
            List<string> localMembers = fitness.MemberFromFile(); //Kopi af listen over medlemmer
            List<string> localActivities = fitness.ActivityFromFile(); //Kopi af listen over aktiviteter

            Yoga.Text = localActivities[0].ToUpper() + Environment.NewLine +
                        localMembers[1] + Environment.NewLine +
                        localMembers[3] + Environment.NewLine +
                        localMembers[8] + Environment.NewLine +
                        localMembers[11] + Environment.NewLine +
                        localMembers[13]; //TextBlock hvor der er manuelt er lagt visse medlemmer ind ved brug af indexer fra listen

            Boxing.Text = localActivities[1].ToUpper() + Environment.NewLine +
                          localMembers[1] + Environment.NewLine +
                          localMembers[4] + Environment.NewLine +
                          localMembers[7];

            Spinning.Text = localActivities[2].ToUpper() + Environment.NewLine +
                            localMembers[0] + Environment.NewLine +
                            localMembers[2] + Environment.NewLine +
                            localMembers[9] + Environment.NewLine +
                            localMembers[10];

            Pilates.Text = localActivities[3].ToUpper();

            Crossfit.Text = localActivities[4].ToUpper() + Environment.NewLine + 
                localMembers[3] + Environment.NewLine + 
                localMembers[8] + Environment.NewLine + 
                localMembers[9] + Environment.NewLine + 
                localMembers[13];
        }


        // Remove a member from an activity - Sidney
        private void RemoveMemberFromActivity()
        {
            if (!int.TryParse(EnterActivity.Text, out int activityIndex))
            {
                MessageBox.Show("Indtast aktivitet 1-5");
                return;
            }

            if (!int.TryParse(EnterMember.Text, out int memberId))
            {
                MessageBox.Show("Indtast gyldigt medlem ID");
                return;
            }

            int memberIndex = memberId - 1;

            List<string> localMembers = fitness.MemberFromFile();

            if (memberIndex < 0 || memberIndex >= localMembers.Count)
            {
                MessageBox.Show("Medlem findes ikke!");
                return;
            }

            string memberName = localMembers[memberIndex];

            TextBlock? target = activityIndex switch
            {
                1 => Yoga,
                2 => Boxing,
                3 => Spinning,
                4 => Pilates,
                5 => Crossfit,
                _ => null
            };

            if (target == null) return;

            List<string> lines = target.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            bool removed = false;

            for (int i = 1; i < lines.Count; i++)
            {
                if (lines[i] == memberName)
                {
                    lines.RemoveAt(i);
                    removed = true;
                    break;
                }
            }

            if (!removed)
            {
                MessageBox.Show("Medlem er ikke i denne aktivitet.");
                return;
            }

            target.Text = string.Join(Environment.NewLine, lines);
            MessageBox.Show($"Fjernede {memberName} fra aktiviteten.");

            UpdateAllCapacities();
        }

        //Hide Bruger login fra Admin -- philip
        private void ApplyRoleRestrictions1()
        {
            if (member.role.ToLower() == "admin")
            {
                DeleteMemberButton.Visibility = Visibility.Visible;
                AddMemberButton.Visibility = Visibility.Visible;
                CreateActivity.Visibility = Visibility.Visible;
                EnterActivity.Visibility = Visibility.Visible;
                EnterMember.Visibility = Visibility.Visible;

                // Hide user buttons
                JoinButton.Visibility = Visibility.Collapsed;
                LeaveButton.Visibility = Visibility.Collapsed;
                TypeActivityIn.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide admin-only controls
                DeleteMemberButton.Visibility = Visibility.Collapsed;
                AddMemberButton.Visibility = Visibility.Collapsed;
                CreateActivity.Visibility = Visibility.Collapsed;
                EnterActivity.Visibility = Visibility.Collapsed;
                EnterMember.Visibility = Visibility.Collapsed;
                NewActivity.Visibility = Visibility.Collapsed;

                // Show user buttons
                JoinButton.Visibility = Visibility.Visible;
                LeaveButton.Visibility = Visibility.Visible;
            }
        }

        //Medlemer tilmelder sig aktivitet -- Philip
        private void JoinActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TypeActivityIn.Text, out int activityIndex))
            {
                MessageBox.Show("Indtast et gyldigt aktivitetsnummer 1-5.");
                return;
            }

            TextBlock? target = activityIndex switch
            {
                1 => Yoga,
                2 => Boxing,
                3 => Spinning,
                4 => Pilates,
                5 => Crossfit,
                _ => null
            };

            if (target == null) return;

            // Format: "ID: 1 Navn: Mathias Køn: M"
            string displayMember =
                $"ID: {member.id} Navn: {member.name} Køn: {member.gender}";

            // Split lines
            List<string> lines = target.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // Første linje er altid aktivitetens navn
            int currentCount = lines.Count - 1;

            // Tjek om brugeren allerede står på holdet
            if (target.Text.Contains(member.name))
            {
                MessageBox.Show("Du er allerede tilmeldt dette hold.");
                return;
            }
            // Tilføj medlem
            target.Text += Environment.NewLine + displayMember;
            MessageBox.Show($"Du er nu tilmeldt {activityIndex}.");

            UpdateAllCapacities();
        }

        //Medlemer melder sig af aktivitet -- Philip
        private void LeaveActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TypeActivityIn.Text, out int activityIndex))
            {
                MessageBox.Show("Indtast et gyldigt aktivitetsnummer 1-5.");
                return;
            }

            TextBlock? target = activityIndex switch
            {
                1 => Yoga,
                2 => Boxing,
                3 => Spinning,
                4 => Pilates,
                5 => Crossfit,
                _ => null
            };

            if (target == null) return;

            string displayMember =
            $"ID: {member.id} Navn: {member.name} Køn: {member.gender}";

            List<string> lines = target.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            bool removed = lines.Remove(displayMember);

            if (!removed)
            {
                MessageBox.Show("Du er ikke tilmeldt dette hold.");
                return;
            }

            target.Text = string.Join(Environment.NewLine, lines);
            MessageBox.Show("Du er nu frameldt.");

            UpdateAllCapacities();
        }

        //Vis antal meldemer tilmeldt aktivitet -- Philip 
        private void UpdateCapacity(TextBlock activityText, TextBlock countText)
        {
            int maxCapacity = 5;

            var lines = activityText.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            int count = lines.Length - 1; // minus activity name

            countText.Text = $"{count}/{maxCapacity} tilmeldt";
        }
        private void UpdateAllCapacities()
        {
            UpdateCapacity(Yoga, YogaCount);
            UpdateCapacity(Boxing, BoxingCount);
            UpdateCapacity(Spinning, SpinningCount);
            UpdateCapacity(Pilates, PilatesCount);
            UpdateCapacity(Crossfit, CrossfitCount);
        }


        // DELETE BUTTON HANDLER
        private void DeleteActivityButton_Click(object sender, RoutedEventArgs e) //Knap der kører RemoveMemberFromActivity funktionen
        {
            RemoveMemberFromActivity();
        }

        // BACK BUTTON HANDLER
        private void GoToNextWindow_Click(object sender, RoutedEventArgs e) //Knap der vender tilbage til hovedmenuen
        {
            NextWindow next = new NextWindow(member, fitness);
            next.Show();
            this.Close();
        }

        // Sidney
        private void CreateActivity_Click(object sender, RoutedEventArgs e) //Knap der opretter ny aktivitet
        {
            string newActName = NewActivity.Text.Trim(); //Brugerens input bliver sat i ind string der bruges senere
            TextBlock block = new TextBlock(); //Opretter ny TextBlock med properties
            Grid.SetRow(block, 1);
            block.Foreground = Brushes.White;
            block.FontWeight = FontWeights.Bold;
            block.HorizontalAlignment = HorizontalAlignment.Left;
            block.VerticalAlignment = VerticalAlignment.Top;
            block.Margin = new Thickness(1100, 130, 0, 0);
            block.TextWrapping = TextWrapping.Wrap;

            block.Text = newActName.ToUpper(); //Brugerens input bliver lagt ind i TextBlocken
            ActivityGrid.Children.Add(block);
            MessageBox.Show($"Aktivitet {newActName} oprettet");

            UpdateAllCapacities();
        }
    }
}