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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace GymWPF
{
    /// <summary>
    /// Interaction logic for Profil.xaml
    /// </summary>
    public partial class Profil : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------//

        string ConnectedSalle, ConnectedSport, iduser, nom, prenom;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            da.SelectCommand.CommandText = "select u.Nom,u.Prenom,s.nom_Salle,t.nom_Type,u.Valide,s.IdSalle,t.IdType,u.IdUser from Utilisateur u join UtilisateurSportSalle us on u.IdUser = us.IdUser join Salle s on s.IdSalle = us.IdSalle join Type_Sport t on t.IdType = us.IdType where s.IdSalle = '" + ConnectedSalle + "' and t.IdType = '" + ConnectedSport + "' and u.IdUser = '" + iduser + "'";
            da.Fill(ds, "infos");


            if (ds.Tables["infos"].Rows.Count != 0 )
            {
                UserName.Text = ds.Tables["infos"].Rows[0][0].ToString() + " " + ds.Tables["infos"].Rows[0][1].ToString();
                SalleName.Text = ds.Tables["infos"].Rows[0][2].ToString();
                SportName.Text = ds.Tables["infos"].Rows[0][3].ToString();
                if (Convert.ToBoolean(ds.Tables["infos"].Rows[0][4]) == true)
                {
                    Acsess.Text = "Admin";
                    icon1.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    icon2.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    icon3.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    icon4.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    icon5.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));

                }
                else 
                {
                    Acsess.Text = "Editeur";
                    icon1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    icon2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    icon3.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    icon4.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    icon5.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                }
                
            }
            else
            {
                SalleName.Text = "Tout";
                SportName.Text = "Tout";
                da.SelectCommand.CommandText = "select * from Utilisateur where IdUser = '"+iduser+"'";
                da.Fill(ds, "users");
                Acsess.Text = "Admin Principale";
                UserName.Text = ds.Tables["users"].Rows[0][1].ToString() + " " + ds.Tables["users"].Rows[0][2].ToString();
                icon1.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                icon2.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                icon3.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                icon4.Foreground = new SolidColorBrush(Color.FromRgb(255, 155, 0));
                icon5.Foreground = new SolidColorBrush(Color.FromRgb(255, 155, 0));
            }


        }

        MainApp mv;
        public Profil(MainApp mv, string ConnectedSalle, string ConnectedSport, string iduser, string nom, string prenom)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;
            this.iduser = iduser;
            this.mv = mv;
            InitializeComponent();
        }
    }
}
