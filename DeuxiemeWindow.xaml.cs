

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Windows.Shapes;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shell;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Security.Policy;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Windows.Documents;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EDT_TSE_2.Class;









namespace moodle2
{
    public partial class DeuxiemeWindow : INotifyPropertyChanged
    {
        private string filePath;
    
        private string username1;
        private string password1;
        private string username2;
        private string password2;
        public event EventHandler<EventArgs> AuthenticationCompletedl;
        public event PropertyChangedEventHandler PropertyChanged;
        EDT_Manager edt_manager;
        Date_Manager date_manager;
        private List<Note2> notes = new List<Note2>();

        public DeuxiemeWindow(string filePath,string filePath2, string username1, string password1,string username2,string password2)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.filePath = filePath2;
            this.username1 = username1;
            this.password1 = password1;
            this.username2 = username2;
            this.password2 = password2;
            ScrapCoursesFromFile1(filePath);
            ScrapCoursesFromFile2(filePath2);
            DataContext = this;
            date_manager = new Date_Manager(ListeSemaines);
            edt_manager = new EDT_Manager(grid2, date_manager.current_week_number);
            Semaine_Correspondance.Text = edt_manager.Set_Semaine(date_manager.current_week_number);

        }
        public DeuxiemeWindow()
        {
            InitializeComponent();
           
        }


        private void ScrapCoursesFromFile2(string filePath)
        {
            try
            {
                // Charger le contenu du fichier HTML
                string htmlContent = File.ReadAllText(filePath);

                // Charger le contenu HTML dans HtmlAgilityPack
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                
                   var courseBoxes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'coursebox')]");

                if (courseBoxes != null)
                {
                    // Parcourir chaque élément de la classe "coursebox" pour extraire les informations
                    foreach (var courseBox in courseBoxes)
                    {
                        // Récupérer le nom du cours et le lien associé
                        var courseNameNode = courseBox.SelectSingleNode(".//h3[@class='coursename']/a");
                        var courseName = courseNameNode?.InnerText.Trim();
                        var courseLink = courseNameNode?.GetAttributeValue("href", "");
                        
                        Button courseButton = new Button();
                        courseButton.Content = courseName;
                        courseButton.Click += async (sender, e) => await OpenCoursePage2(courseLink);
                        CourseStackPanel.Children.Add(courseButton);

                    }
                }
                else
                {
                    MessageBox.Show("Aucun cours trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }
        private void ScrapCoursesFromFile1(string filePath)
        {
            try
            {
                // Charger le contenu du fichier HTML
                string htmlContent = File.ReadAllText(filePath);

                // Charger le contenu HTML dans HtmlAgilityPack
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // Sélectionner tous les éléments span avec la classe media-body
                var nodes = doc.DocumentNode.SelectNodes("//a[@class='list-group-item list-group-item-action  ']");

                // Vérifier si des éléments ont été trouvés
                if (nodes != null)
                {
                    // Parcourir chaque élément trouvé
                    foreach (var node in nodes)
                    {
                        var spanNode = node.SelectSingleNode(".//span[@class='media-body ']");
                        if (spanNode != null)
                        {
                            // Récupérer le nom du cours
                            string courseName = spanNode.InnerText.Trim();

                            // Récupérer le lien du cours
                            string courseLink = node.GetAttributeValue("href", "");

                            // Créer un bouton pour chaque cours
                            Button courseButton = new Button();
                            courseButton.Content = courseName;
                            courseButton.Click += async (sender, e) => await OpenCoursePage1(courseLink);
                            



                            // Ajouter le bouton à la fenêtre
                            CourseStackPanel.Children.Add(courseButton);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Aucun cours trouvé.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }

        private async Task OpenCoursePage1(string courseLink)
        {
            ChromeOptions options = new ChromeOptions();

         
              courseLink = courseLink.Replace("&amp;", "&");
              PagecontentWindow pagecontentWindow = new PagecontentWindow(courseLink,username1,password1);

            
              pagecontentWindow.Show();
              



        }
        private async Task OpenCoursePage2(string courseLink)
        {

            courseLink = courseLink.Replace("&amp;", "&");
            PagecontentWindow pagecontentWindow = new PagecontentWindow(courseLink, username2, password2);


           pagecontentWindow.Show();


        }
        public List<Note2> ExtractNotesIUTFromHtml(string html)
        {
            List<Note2> notes = new List<Note2>();

            // Charger le document HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Sélectionner tous les éléments de tableau (balise <tr>)
            var rows = doc.DocumentNode.SelectNodes("//tr");

            //initialiser module et ue compteur
            string currentModule = null;
            int ueCount = 0;

            // Parcourir chaque ligne
            foreach (var row in rows)
            {
                // Check if the row has class 'ue'
                if (row.GetClasses().Contains("ue"))
                {
                    // If it's a module row, extract module data
                    ueCount++;
                }
                // Check if the row has class 'module'
                else if (row.GetClasses().Contains("module"))
                {
                    // If it's a module row, extract module data
                    currentModule = row.InnerText.Trim();
                }

                // Check if the row has class 'evaluation'
                else if (ueCount == 1 && row.GetClasses().Contains("evaluation"))
                {
                    // Initialize variables to store evaluation data
                    string name = null;
                    string min = null;
                    string max = null;
                    string moy = null;
                    string note = null;
                    string coef = null;

                    // Sélectionner toutes les cellules dans la ligne
                    var cells = row.SelectNodes("td");
                    if (cells != null)
                    {
                        // Extraire les données pertinentes
                        name = cells[1].InnerText.Trim();
                        min = cells[2].InnerText.Trim();
                        max = cells[3].InnerText.Trim();
                        moy = cells[4].InnerText.Trim();
                        note = cells[5].InnerText.Trim();
                        coef = cells[6].InnerText.Trim();
                    }
                    // Ajouter la note à la liste
                    notes.Add(new Note2 { Module = currentModule, Name = name, Min = min, Max = max, Moy = moy, Note = note, Coefficient = coef });

                }
            }
            return notes;
        }


        public List<Note2> ExtractNotesTSEFromHtml(string html)
        {
            List<Note2> notes = new List<Note2>();

            // Charger le document HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            //Selectionner les éléments de tableau contenant les données des matières
            var tableRows = doc.DocumentNode.SelectNodes("//tr");


            // Parcourir chaque ligne du tableau
            foreach (var row in tableRows)
            {
                var cells = row.SelectNodes("td");
                // Vérifier si la ligne contient une note (ignorer les lignes vides)
                if (cells != null && cells.Count == 3 && cells[1].InnerText.Trim() != "-" && row.Attributes["class"].Value != "emptyrow")
                {
                    // Récupérer le lien et la note
                    var moduleLink = cells[0].SelectSingleNode("a").Attributes["href"].Value.Replace("amp;", "");
                    var moduleName = cells[0].InnerText.Trim();

                    //acceder au lien associé
                    Authentification.mydriver.Navigate().GoToUrl(moduleLink);

                    string htmlNoteDetails = Authentification.mydriver.PageSource;

                    //extraire les détails des notes
                    notes.AddRange(ExtractNoteDetailsFromHtml(htmlNoteDetails, moduleName));
                }
            }

            return notes;
        }

        private List<Note2> ExtractNoteDetailsFromHtml(string html, string module)
        {
            List<Note2> noteDetails = new List<Note2>();

            // Charger le document HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Sélectionner les éléments de tableau contenant les données des notes en détail
            var tableRows = doc.DocumentNode.SelectNodes("//tbody//tr");

            // Parcourir chaque ligne du tableau
            foreach (var row in tableRows)
            {
                var cells = row.SelectNodes("td | th");
                if (cells != null && cells.Count == 7 && cells[2].InnerText.Trim() != "-")
                {
                    string nomDevoir = cells[0].InnerText.Trim();
                    string coefficientText = cells[1].InnerText.Trim();
                    string noteText = cells[2].InnerText.Trim();

                    //ajouter la note
                    noteDetails.Add(new Note2 { Module = module, Name = nomDevoir, Coefficient = coefficientText, Note = noteText });
                }
            }
            return noteDetails;
        }
        public void ShowNotesWindow(string html_IUT, string html_TSE)
        {
            // Définir l'ItemsSource du listview sur la liste de notes
            
            notes.AddRange(ExtractNotesIUTFromHtml(html_IUT));
            notes.AddRange(ExtractNotesTSEFromHtml(html_TSE));

            listView.ItemsSource = notes;
            Show();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        private void ListeSemaines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int week_selected = int.Parse(e.AddedItems[0].ToString());


            date_manager.Update_Date(week_selected, ListeSemaines);
            edt_manager.peupler_edt(week_selected, grid2);
            Semaine_Correspondance.Text = edt_manager.Set_Semaine(week_selected);
            OnPropertyChanged();
        }

    }
}

