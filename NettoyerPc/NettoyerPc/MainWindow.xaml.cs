using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NettoyerPc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
      
    {
        //variable de recuperation des donnee d
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;
        public MainWindow()
        {
            InitializeComponent();
            //chemin vers  le dossier temp
            winTemp = new DirectoryInfo(@"C:\Windows\Temp");
            //recuperation du chemin complet avec gettemppath();
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
            checkActu();
            saveDate();
            GetDate();

        }
        //verifications des actualié 
        /// <summary>
        /// 
        /// </summary>
       public void checkActu()
        {

            string url = "http://127.0.0.1/siteweb/info.txt";
            using(WebClient client = new WebClient())
            {
                string actu = client.DownloadString(url);
                if(actu != string.Empty)
                {
                    actutTxt.Content = actu;
                    actutTxt.Visibility = Visibility.Visible;
                    bandeauActu.Visibility = Visibility.Visible;
                }
            }
        }
        //calcul de la taille du dossier
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public long DirSize(DirectoryInfo dir)
        {
            //une fonction revoie toujour un resultat or une methode ne renvoie rien
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }
        //vider le contenu  d'un dossier
        public void ClearTempData(DirectoryInfo di)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    Console.WriteLine(file.FullName);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                    Console.WriteLine(dir.FullName);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
    
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_His_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ok ok");
        }

        private void Button_site_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Process.Start(new ProcessStartInfo("www.google.com")
                {
                    UseShellExecute = true

                });
            }catch(Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Maj_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("le logiciel a jour", "analyse pc", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Button_Analyse_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("nous y travaillons");
            AnalyseFolder();
        }
        //analyse des differents dossiers du system$

            /// <summary>
            /// analyse des dossier s
            /// </summary>
        public void AnalyseFolder()
        {
            int nombreAnalyse =0;
            Console.WriteLine("debut de lanalyse.....");
            long totalSize = 0;
            try
            { //calcul du poids des dossiers
                totalSize += DirSize(winTemp) / 1000000;
                titre.Content = "Analyse effectué !";
                
                // nbAnalyse.Content = nombreAnalyse;
                nombreAnalyse+=nombreAnalyse;
                File.WriteAllText("fichier_analyse.txt", nombreAnalyse.ToString());
                totalSize += DirSize(appTemp) / 1000000;
            }
            catch(Exception ex)
            {
                Console.WriteLine("imposssible d'analyser les dossiers :" + ex.Message);
            }
           
            espace.Content = totalSize + "Mb";
            date.Content = DateTime.Today;
        }
        /// <summary>
        /// foction de nettoyage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_nettoyer_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("ne pas arreté le pc svp ", "analyse", MessageBoxButton.OK, MessageBoxImage.Information);

            //methode de nottoyage 
            Console.WriteLine("nettoyage encours...");
            nettoyer.Content = "Nettoyage en cours ..";
            espace.Content = "0 Mb";
            Clipboard.Clear();
            
            try
            {
                ClearTempData(winTemp);
            }catch(Exception ex)
            {
                Console.WriteLine("erreur de nottoyage " + ex.Message);
            }
            try
            {
                ClearTempData(appTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("erreur de nottoyage " + ex.Message);
            }
            nettoyer.Content = "Nettoyage terminé";
            string donner = "";
            donner += donner + 1;
            File.WriteAllText("fiche.txt", donner);

            // string fichiernettoyer = File.WriteAllText("nombreNettoyage.txt" ,nombreNettoyage.ToString);
        }
        /// <summary>
        /// sauvegarder la date de l'analyse
        /// </summary>
        public void saveDate()
        {
        
            string date = DateTime.Today.ToString();
            File.WriteAllText("date.txt",date);
        }
        public void GetDate()
        {
            string dateFichier = File.ReadAllText("date.txt");
            if (dateFichier != string.Empty)
            {
                date.Content = dateFichier;
            }
        }

    }
}
