using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Text; // Nødvendig for StringBuilder

namespace FitnessProgram
{
    public partial class ActivityWindow : Window
    {
        private readonly Fitness fitness; // Shared fitness system
        private readonly Member member;   // Logged in user

        // --- Constructor: MUST match Option 1 ---
        public ActivityWindow(Fitness fitness, Member member)
        {
            InitializeComponent();
            // Tilføjer null-checks for robusthed (baseret på tidligere korrektioner)
            this.fitness = fitness ?? throw new ArgumentNullException(nameof(fitness));
            this.member = member ?? throw new ArgumentNullException(nameof(member));

            ShowActivity();
            ApplyRoleRestrictions(); // Hide admin controls if not admin
            ApplyRoleRestrictions1(); //Member leave or join activity
            UpdateAllCapacities(); //updater så man kan se hvor mange er på en aktivitet

            // ** NYT OPKALD: Sørger for at fjerne ID/Køn for almindelige medlemmer, hvis data allerede er indlæst **
            FormatTextForNonAdmin();
        }


        // Hide admin-only controls
        private void ApplyRoleRestrictions()
        {
            if (member.role.ToLower() != "admin")
            {
                // Bruger null-check logik for at undgå fejl hvis elementer mangler i XAML
                if (DeleteMemberButton != null) DeleteMemberButton.Visibility = Visibility.Collapsed;
                if (CreateActivity != null) CreateActivity.Visibility = Visibility.Collapsed;
                if (EnterActivity != null) EnterActivity.Visibility = Visibility.Collapsed;
                if (EnterMember != null) EnterMember.Visibility = Visibility.Collapsed;
            }
        }


        public void ShowActivity() //Oprettelse af aktiviteter
        {
            List<string> localMembers = fitness.MemberFromFile(); //Kopi af listen over medlemmer
            List<string> localActivities = fitness.ActivityFromFile(); //Kopi af listen over aktiviteter

            if (Yoga != null) // Sikkerhedscheck for TextBlock
            {
                Yoga.Text = localActivities[0].ToUpper() + Environment.NewLine +
                            localMembers[1] + Environment.NewLine +
                            localMembers[3] + Environment.NewLine +
                            localMembers[8] + Environment.NewLine +
                            localMembers[11] + Environment.NewLine +
                            localMembers[13]; //TextBlock hvor der er manuelt er lagt visse medlemmer ind ved brug af indexer fra listen
            }

            if (Boxing != null)
            {
                Boxing.Text = localActivities[1].ToUpper() + Environment.NewLine +
                              localMembers[1] + Environment.NewLine +
                              localMembers[4] + Environment.NewLine +
                              localMembers[7];
            }

            if (Spinning != null)
            {
                Spinning.Text = localActivities[2].ToUpper() + Environment.NewLine +
                                localMembers[0] + Environment.NewLine +
                                localMembers[2] + Environment.NewLine +
                                localMembers[9] + Environment.NewLine +
                                localMembers[10];
            }

            if (Pilates != null)
            {
                Pilates.Text = localActivities[3].ToUpper();
            }

            if (Crossfit != null)
            {
                Crossfit.Text = localActivities[4].ToUpper() + Environment.NewLine +
                    localMembers[3] + Environment.NewLine +
                    localMembers[8] + Environment.NewLine +
                    localMembers[9] + Environment.NewLine +
                    localMembers[13];
            }
        }

        //NY METODE: Sikrer at almindelige brugere kun ser navne i de viste TextBlocks -- Philip
        private void FormatTextForNonAdmin()
        {
            // Kun nødvendigt at køre for almindelige medlemmer
            if (member.role.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return; // Admin skal se de fulde detaljer
            }

            // TextBlock references skal have null-check, da de er XAML-elementer
            TextBlock?[] activityBlocks = { Yoga, Boxing, Spinning, Pilates, Crossfit };

            foreach (var block in activityBlocks)
            {
                if (block == null) continue;

                List<string> lines = block.Text
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                if (lines.Count > 0)
                {
                    string activityName = lines[0]; // Behold altid aktivitetens navn

                    // Brug StringBuilder for effektiv linjesammenføjning
                    var sb = new StringBuilder();
                    sb.AppendLine(activityName);

                    for (int i = 1; i < lines.Count; i++)
                    {
                        string line = lines[i];
                        string memberName = line;

                        // Hvis linjen indeholder "ID:" eller "Køn:" betyder det, at den blev skrevet af en Admin 
                        if (line.Contains("Navn:", StringComparison.OrdinalIgnoreCase))
                        {
                            // Dette er det detaljerede ADMIN format. Trækker kun navnet ud.
                            int nameStart = line.IndexOf("Navn:", StringComparison.OrdinalIgnoreCase);
                            int nameEnd = line.IndexOf("Køn:", StringComparison.OrdinalIgnoreCase);

                            if (nameStart != -1)
                            {
                                int start = nameStart + "Navn:".Length;

                                if (nameEnd != -1 && nameEnd > start)
                                {
                                    // Substring fra "Navn:" til "Køn:"
                                    memberName = line.Substring(start, nameEnd - start).Trim();
                                }
                                else
                                {
                                    // Substring fra "Navn:" til slutningen af linjen
                                    memberName = line.Substring(start).Trim();
                                }
                            }
                        }
                        // Hvis linjen ikke indeholder ID/Køn, er det allerede kun navnet

                        if (!string.IsNullOrWhiteSpace(memberName))
                        {
                            sb.AppendLine(memberName);
                        }
                    }

                    block.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                }
            }
        }

        // Remove a member from an activity - Sidney
        private void RemoveMemberFromActivity()
        {
            if (!int.TryParse(EnterActivity?.Text, out int activityIndex))
            {
                MessageBox.Show("Indtast aktivitet 1-5");
                return;
            }
            
            if (!int.TryParse(EnterMember?.Text, out int memberId))
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
                // Tjek om linjen indeholder navnet i det korte format
                if (lines[i].Contains(memberName, StringComparison.OrdinalIgnoreCase))
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
                // Admin knapper
                if (DeleteMemberButton != null) DeleteMemberButton.Visibility = Visibility.Visible;
                if (CreateActivity != null) CreateActivity.Visibility = Visibility.Visible;
                if (EnterActivity != null) EnterActivity.Visibility = Visibility.Visible;
                if (EnterMember != null) EnterMember.Visibility = Visibility.Visible;

                // Hide user buttons FOR ADMIN
                if (JoinButton != null) JoinButton.Visibility = Visibility.Collapsed;
                if (LeaveButton != null) LeaveButton.Visibility = Visibility.Collapsed;
                if (TypeActivityIn != null) TypeActivityIn.Visibility = Visibility.Collapsed;
                if (MeldDigTilHold != null) MeldDigTilHold.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide admin-only controls FOR USER
                if (DeleteMemberButton != null) DeleteMemberButton.Visibility = Visibility.Collapsed;
                if (CreateActivity != null) CreateActivity.Visibility = Visibility.Collapsed;
                if (EnterActivity != null) EnterActivity.Visibility = Visibility.Collapsed;
                if (EnterMember != null) EnterMember.Visibility = Visibility.Collapsed;
                if (NewActivity != null) NewActivity.Visibility = Visibility.Collapsed;
                if (IndtastMedlemsID != null) IndtastMedlemsID.Visibility = Visibility.Collapsed;
                if (IndtastNummeretPåHoldet != null) IndtastNummeretPåHoldet.Visibility = Visibility.Collapsed;
                if (IndtastNavnetOpret != null) IndtastNavnetOpret.Visibility = Visibility.Collapsed;


                // Show user buttons
                if (JoinButton != null) JoinButton.Visibility = Visibility.Visible;
                if (LeaveButton != null) LeaveButton.Visibility = Visibility.Visible;
            }
        }

        //Medlemer tilmelder sig aktivitet -- Philip
        private void JoinActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TypeActivityIn?.Text, out int activityIndex))
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

            // ** FORMATERING BASERET PÅ ROLLE **
            string displayMember;
            if (member.role.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin ser det fulde format (som det var før)
                displayMember = $"ID: {member.id} Navn: {member.name} Køn: {member.gender}";
            }
            else
            {
                // Almindeligt medlem ser KUN navnet (som ønsket)
                displayMember = member.name;
            }
            // **********************************************

            // Split lines
            List<string> lines = target.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // Første linje er altid aktivitetens navn
            int currentCount = lines.Count - 1;

            // Tjek om brugeren allerede står på holdet
            if (lines.Any(line => line.Contains(member.name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Du er allerede tilmeldt dette hold.");
                return;
            }

            // Fuldt hold på 5 tjek
            if (currentCount >= 5)
            {
                MessageBox.Show("Dette hold er fyldt. (5/5)");
                return;
            }

            // Tilføj medlem
            target.Text += Environment.NewLine + displayMember;
            // Skiftet fra CustomMessageBox til MessageBox.Show
            MessageBox.Show($"Du er nu tilmeldt {target.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).First()}."); // Brug den faktiske aktivitet

            UpdateAllCapacities();
        }

        //Medlemer melder sig af aktivitet -- Philip
        private void LeaveActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TypeActivityIn?.Text, out int activityIndex))
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

            // Denne linje er kun til identifikation i metoden, men den fulde streng er ikke nødvendig for fjernelse takket være 'Contains'
            // Bemærk: memberLineToRemove bruges ikke længere direkte, men koden er beholdt som i dit originale udkast.
            string memberLineToRemove;
            if (member.role.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                memberLineToRemove = $"ID: {member.id} Navn: {member.name} Køn: {member.gender}";
            }
            else
            {
                memberLineToRemove = member.name;
            }


            List<string> lines = target.Text
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

            // Find og fjern den linje, der matcher navnet/den fulde linje
            bool removed = false;
            for (int i = 1; i < lines.Count; i++)
            {
                // Tjek for en match. Vi bruger member.name, da den enten matcher det fulde admin-format eller det simple navn.
                if (lines[i].Contains(member.name, StringComparison.OrdinalIgnoreCase))
                {
                    lines.RemoveAt(i);
                    removed = true;
                    break;
                }
            }


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
            // Disse TextBlock referencer (YogaCount, BoxingCount, etc.) antages at eksistere i XAML
            // og er nødvendige for at køre.
            if (YogaCount != null) UpdateCapacity(Yoga, YogaCount);
            if (BoxingCount != null) UpdateCapacity(Boxing, BoxingCount);
            if (SpinningCount != null) UpdateCapacity(Spinning, SpinningCount);
            if (PilatesCount != null) UpdateCapacity(Pilates, PilatesCount);
            if (CrossfitCount != null) UpdateCapacity(Crossfit, CrossfitCount);
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
        private void CreateActivity_Click(object sender, RoutedEventArgs e) 
        {
            string newActName = NewActivity?.Text.Trim() ?? string.Empty; //Brugerens input bliver sat i ind string der bruges senere

            if (string.IsNullOrEmpty(newActName)) //if statement der kører så længe brugeren ikke skriver noget i TextBoxen
            {
                MessageBox.Show("Indtast venligst et navn til aktivitetet");
                return; //Stopper her og springer resten af koden nedenunder over
            }

            // Sidney 
            TextBlock block = new TextBlock(); //Opretter ny TextBlock med properties
            if (ActivityGrid != null)
            {
                Grid.SetRow(block, 1);
                block.Foreground = Brushes.White;
                block.FontWeight = FontWeights.Bold;
                block.HorizontalAlignment = HorizontalAlignment.Left;
                block.VerticalAlignment = VerticalAlignment.Top;
                block.Margin = new Thickness(1030, 130, 0, 0);
                block.TextWrapping = TextWrapping.Wrap;

                block.Text = newActName.ToUpper(); 
                ActivityGrid.Children.Add(block);
                MessageBox.Show($"Aktivitet {newActName} oprettet");
                UpdateAllCapacities();
            }
        }
    }
}