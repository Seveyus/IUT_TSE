
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Support.UI;
//using System.Diagnostics;
//using System.IO;
//using System.Numerics;
//namespace Application_moodle
//{
//    public class Authentification
//    {
//        public static string filePath = "C:/Users/yoyof/OneDrive/Bureau/C#/Application_moodle/bin/Debug/net8.0-windows/page1.html";
//        public static string filePath2 = "C:/Users/yoyof/OneDrive/Bureau/C#/Application_moodle/bin/Debug/net8.0-windows/page2.html";
//        public delegate void AuthenticationCompletedEventHandler(object sender, DeuxiemeWindow deuxiemeWindow);
//        public static event AuthenticationCompletedEventHandler AuthenticationCompleted;
//        private readonly IWebDriver driver;

//        public static bool Authentifier(IWebDriver driver, string url, string username, string password)
//        {
//            driver.Navigate().GoToUrl(url);

//            // Récupérer les éléments pour le nom d'utilisateur et le mot de passe
//            IWebElement usernameField = driver.FindElement(By.Name("username"));
//            IWebElement passwordField = driver.FindElement(By.Name("password"));

//            // Saisir les identifiants
//            usernameField.SendKeys(username);
//            passwordField.SendKeys(password);

//            // Soumettre le formulaire
//            passwordField.Submit();

//            // Attendre jusqu'à ce que l'URL après l'authentification soit celle attendue
//            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//            return wait.Until(driver =>
//                driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/") ||
//                driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php")
//            );
//        }
//        public static bool AuthentifierSurSite(string url, string username, string password)
//        {

//            //string chromeDriverPath = @"C:\Users\yoyof\OneDrive\Bureau\C#\Application_moodle\chromedriver.exe";

//            ChromeOptions options = new ChromeOptions();
//            //options.AddArgument("--headless"); // Exécuter Chrome en mode headless
//            //options.AddArgument("--disable-gpu"); // Désactiver l'accélération matérielle en mode headless
//            //options.AddArgument("--silent"); // Exécuter le navigateur en mode silencieux
//            //options.AddArgument("--log-level=3"); // Définir le niveau de journalisation minimal

//            //// Rediriger la sortie standard et d'erreur vers un flux nul
//            //options.AddArgument("--disable-logging");
//            //options.AddArgument("--disable-logging-redirect");



//            using (IWebDriver driver = new ChromeDriver(options))
//            {
//                try
//                {
//                    driver.Navigate().GoToUrl(url);

//                    IWebElement usernameField = driver.FindElement(By.Name("username"));
//                    IWebElement passwordField = driver.FindElement(By.Name("password"));

//                    usernameField.SendKeys(username);
//                    passwordField.SendKeys(password);

//                    passwordField.Submit();

//                    // Attente explicite pour vérifier si l'URL après l'authentification est celle attendue
//                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

//                    // Vérifier si l'URL commence par l'une des deux valeurs attendues
//                    wait.Until(driver =>
//                        driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/") ||
//                        driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php")
//                    );

//                    if (driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/"))
//                    {

//                        driver.Navigate().GoToUrl("https://mood.univ-st-etienne.fr/course/view.php?id=1540");
//                        string pageSource = driver.PageSource;


//                        File.WriteAllText(filePath, pageSource);



//                    }
//                    if (driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/index.php"))
//                    {

//                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/");
//                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=65");
//                        driver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=157");
//                        string pageSource2 = driver.PageSource;
//                        File.WriteAllText(filePath2, pageSource2);
//                    }

//                    // Si l'attente réussit, l'authentification est considérée comme réussie
//                    return true;
//                }
//                catch (WebDriverTimeoutException)
//                {
//                    // L'attente a expiré, l'URL n'est pas celle attendue, l'authentification a échoué
//                    return false;
//                }
//                finally
//                {
//                    // Fermez proprement le navigateur
//                    driver.Quit();
//                }


//            }
//        }
//    }
//}





using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Security.Policy;
using OpenQA.Selenium.Interactions;

namespace moodle2
{
    public class Authentification
    {
        public static string filePath = "page1.html";
        public static string filePath2 = "page2.html";
        public static string filePath3 = "page3.html";
        public static string filePath4= "page4.html";

        public static IWebDriver mydriver;
        string htmlContent;




        public delegate void AuthenticationCompletedEventHandler(object sender, EventArgs e);
        public static event AuthenticationCompletedEventHandler AuthenticationCompleted;
      

        public static bool AuthentifierSurSite(string url, string username, string password)
        {
            try
            {
                // Initialiser le pilote ChromeDriver
                ChromeOptions options = new ChromeOptions();
                options.AddUserProfilePreference("download.default_directory", @"C:\Users\yoyof\OneDrive\Bureau\C#\moodle2\bin\Debug");
                //executer chrome en mode headless
                //options.AddArgument("--headless");
                //options.AddArgument("--disable-gpu");
                //options.AddArgument("--silent");
                //options.AddArgument("--log-level=3");
                ////ne pas afficher la fenetre de la console
                //options.AddArgument("--disable-logging");
                //options.AddArgument("--disable-logging-redirect");
               


                mydriver = new ChromeDriver(options);

                // Authentifier sur le site
                bool authentificationReussie = Authentifier(mydriver, url, username, password);

                // Sauvegarder la page source en fonction de l'URL
                if (authentificationReussie)
                {
                    
                    if (mydriver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/"))
                    {
                        
                        mydriver.Navigate().GoToUrl("https://mood.univ-st-etienne.fr/course/view.php?id=1540");

                        string pageSource = mydriver.PageSource;
                        File.WriteAllText(filePath, pageSource);


                        //LEO
                        mydriver.Navigate().GoToUrl("https://intraiut.univ-st-etienne.fr/scodoc/report/");
                        string htmlContent = mydriver.PageSource;
                        File.WriteAllText(filePath3, htmlContent);



                    }
                    
                    else if (mydriver.Url.StartsWith("https://mootse.telecom-st-etienne.fr/"))
                    {

                        mydriver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/");
                        mydriver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=65");
                        mydriver.Navigate().GoToUrl("https://mootse.telecom-st-etienne.fr/course/index.php?categoryid=157");
                        string pageSource2 = mydriver.PageSource;
                        File.WriteAllText(filePath2, pageSource2);


                        //LEO
                        mydriver.Navigate().GoToUrl("https://intraiut.univ-st-etienne.fr/scodoc/report/");
                        string htmlContent2 = mydriver.PageSource;
                        File.WriteAllText(filePath4, htmlContent2);

                        //Partie pour ICS coté IUT (récupération du nom et prénom également)
                        string icsUrl = ExtractICS(username, password).Result;
                        
                        mydriver.Navigate().GoToUrl(icsUrl);

                        //Partie pour ICS coté TSE

                        string icsUrl2 = "https://www.telecom-st-etienne.fr/intranet/icsbyuid.php?uid=" + username;
                        mydriver.Navigate().GoToUrl(icsUrl2);
                    }
                    

                    // Informer que l'authentification est terminée
                    AuthenticationCompleted?.Invoke(null, EventArgs.Empty);
                }

                return authentificationReussie;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'authentification : " + ex.Message);
                return false;
            }

        }

        private static bool Authentifier(IWebDriver driver, string url, string username, string password)
        {

            driver.Navigate().GoToUrl(url);

            // Récupérer les éléments pour le nom d'utilisateur et le mot de passe
            IWebElement usernameField = driver.FindElement(By.Name("username"));
            IWebElement passwordField = driver.FindElement(By.Name("password"));

            // Saisir les identifiants
            usernameField.SendKeys(username);
            passwordField.SendKeys(password);

            // Soumettre le formulaire
            passwordField.Submit();


            // Attendre jusqu'à ce que l'URL après l'authentification soit celle attendue
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            return wait.Until(mydriver =>
                driver.Url.StartsWith("https://mood.univ-st-etienne.fr/my/") ||
                driver.Url.StartsWith("https://mootse.telecom-st-etienne.fr/") ||
                driver.Url.StartsWith("https://www.telecom-st-etienne.fr/intranet/")
            );
        }

        private static async Task<string> ExtractICS(string username, string password)
        {
            string urlIntranet = "https://www.telecom-st-etienne.fr/intranet/login.php";
            bool connecterTSE = Authentifier(mydriver, urlIntranet, username, password);

            if (connecterTSE)
            {
                mydriver.Navigate().GoToUrl("https://www.telecom-st-etienne.fr/intranet/");

                // Charger le document HTML
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(mydriver.PageSource);

                HtmlNode bElement = doc.DocumentNode.SelectSingleNode("//footer//b");
                string iud = bElement.InnerText;
                string link = "https://www.telecom-st-etienne.fr/intranet/annuaire/annuaire_fiche.php?id=" + iud;
             

                mydriver.Navigate().GoToUrl(link);

                // Charger le document HTML
                doc.LoadHtml(mydriver.PageSource);

                // Sélectionner l'élément <a> avec la classe "btn btn-primary" et un texte spécifique
                var elementsA = doc.DocumentNode.SelectNodes("//a[@class='btn btn-primary']");

                HtmlNode troisiemeElementA = elementsA[2];
                string texteElementA = troisiemeElementA.InnerText.Trim();

                string firstHalf = texteElementA.Substring(0, 6); // Extraire la première moitié du mot
                string secondHalf = texteElementA.Substring(6); // Extraire la deuxième moitié du mot

                string result = firstHalf + " " + secondHalf;

                string resultFinal = result.Replace('_', ' ');

              

                string urlPlanning = "https://planning.univ-st-etienne.fr/direct/index.jsp?projectId=2&login=Etu&password=etudiant";

                mydriver.Navigate().GoToUrl(urlPlanning);

                // Définir le temps d'attente implicite à 10 secondes
                mydriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                // Sélectionner l'élément de recherche par son ID
                IWebElement searchInputElement = mydriver.FindElement(By.Id("x-auto-111-input"));

                searchInputElement.SendKeys(resultFinal);

                // Sélectionner le bouton de recherche par sa classe
                IWebElement searchButton = mydriver.FindElement(By.XPath("//button[@aria-describedby='x-auto-6']"));

                // Cliquer sur le bouton de recherche
                searchButton.Click();

                Thread.Sleep(5000);

                // Sélectionner le deuxième bouton en utilisant un XPath spécifique
                IWebElement secondButton = mydriver.FindElement(By.XPath("//button[@aria-describedby='x-auto-1']"));

                // Cliquer sur le deuxième bouton
                secondButton.Click();


                // Définir le temps d'attente implicite à 10 secondes
                mydriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                // Localiser l'élément "Generate URL" par son texte
                IWebElement generateUrlButton = mydriver.FindElement(By.XPath("//button[text()='Generate URL']"));

                // Cliquer sur le bouton "Generate URL"
                generateUrlButton.Click();

                // Sélectionner l'élément contenant le lien
                IWebElement linkElement = mydriver.FindElement(By.Id("logdetail"));

                // Obtenir le lien contenu dans l'élément
                string linkUrlPlanning = linkElement.FindElement(By.TagName("a")).GetAttribute("href");

                // Pattern pour capturer les dates
                string pattern2 = @"(firstDate=)(\d{4}-\d{2}-\d{2})(&lastDate=)(\d{4}-\d{2}-\d{2})";

                // Remplacement des dates
                string modifiedUrl = Regex.Replace(linkUrlPlanning, pattern2, match =>
                {
                    DateTime startDate = DateTime.Parse(match.Groups[2].Value);
                    DateTime endDate = DateTime.Parse(match.Groups[4].Value);

                    // Modification des dates
                    DateTime newStartDate = new DateTime(startDate.Year, 9, 1); // 1er septembre
                    DateTime newEndDate = new DateTime(endDate.Year, 6, 30);    // 30 juin de l'année suivante

                    return $"{match.Groups[1].Value}{newStartDate:yyyy-MM-dd}{match.Groups[3].Value}{newEndDate:yyyy-MM-dd}";
                });

                return modifiedUrl;
            }

            return "erreur";
        }


    }
}
