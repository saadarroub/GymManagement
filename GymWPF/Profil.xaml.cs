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
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Animation;

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

        string ConnectedSalle, ConnectedSport, iduser, nom, prenom ,ToolTip1, ToolTip2 , ToolTip3, ToolTip4, ToolTip5;


        TimeSpan ts;

        int cpt1, cpt2, cpt3;
        

        string soon, end, mcha;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            

            da.SelectCommand.CommandText = "select u.Nom,u.Prenom,s.nom_Salle,t.nom_Type,u.Valide,s.IdSalle,t.IdType,u.IdUser from Utilisateur u join UtilisateurSportSalle us on u.IdUser = us.IdUser join Salle s on s.IdSalle = us.IdSalle join Type_Sport t on t.IdType = us.IdType where s.IdSalle = '" + ConnectedSalle + "' and t.IdType = '" + ConnectedSport + "' and u.IdUser = '" + iduser + "'";
            da.Fill(ds, "infos");

            da.SelectCommand.CommandText = "select * from Clients c join SportClients s on c.IdClient = s.IdClient where IdSalle = '"+ConnectedSalle+"' and IdType = '"+ConnectedSport+"'";
            da.Fill(ds, "Clients");

            if (ds.Tables["infos"].Rows.Count != 0 )
            {
                UserName.Text =  ds.Tables["infos"].Rows[0][1].ToString().ToUpper();
                SalleName.Text = ds.Tables["infos"].Rows[0][2].ToString();
                SportName.Text = ds.Tables["infos"].Rows[0][3].ToString();
                if (Convert.ToBoolean(ds.Tables["infos"].Rows[0][4]) == true)
                {
                    Acsess.Text = "Admin";
                    icon1.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                    icon2.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                    icon3.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                    icon4.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                    icon5.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                    ToolTip1 = "Vous avez l'access a Ajouter, Modifier et Supprimer Une Salle";
                    ToolTip2 = "Vous avez l'access a Ajouter, Modifier et Supprimer Un Sport. Tu peut affecter Un Sport a tout les Salles";
                    ToolTip3 = "Vous avez l'access pour ajouter un nouveau Utilisateur, determiner son Rolle. Supprimer et Modifier tout les Utilisateur";
                    ToolTip4 = "vous avez l'access a Ajouter ,Modifier et Supprimer un Clients a partir de votre Salle et Sport";
                    ToolTip5 = "Vous avez l'access de gestioner votre depenses a partir de votre Salle et Sport";

                }
                else 
                {
                    Acsess.Text = "Editeur";
                    icon1.Foreground = new SolidColorBrush(Color.FromRgb(255, 52, 73));
                    icon2.Foreground = new SolidColorBrush(Color.FromRgb(255, 52, 73));
                    icon3.Foreground = new SolidColorBrush(Color.FromRgb(255, 52, 73));
                    icon4.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                    icon5.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                    ToolTip1 = "Pas d'access";
                    ToolTip2 = "Pas d'access";
                    ToolTip3 = "Pas d'access";
                    ToolTip4 = "vous avez l'access a Ajouter ,Modifier et Supprimer un Clients a partir de votre Salle et Sport";
                    ToolTip5 = "Vous avez l'access de gestioner votre depenses a partir de votre Salle et Sport";
                }
                
            }
            else
            {
                SalleName.Text = "Tout";
                SportName.Text = "Tout";
                da.SelectCommand.CommandText = "select * from Utilisateur where IdUser = '"+iduser+"'";
                da.Fill(ds, "users");
                Acsess.Text = "Admin Principale";
                UserName.Text = ds.Tables["users"].Rows[0][2].ToString().ToUpper();
                icon1.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                icon2.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                icon3.Foreground = new SolidColorBrush(Color.FromRgb(52, 255, 72));
                icon4.Foreground = new SolidColorBrush(Color.FromRgb(255, 174, 52));
                icon5.Foreground = new SolidColorBrush(Color.FromRgb(255, 174, 52));
                ToolTip1 = "Vous avez l'access a Ajouter, Modifier et Supprimer Une Salle";
                ToolTip2 = "Vous avez l'access a Ajouter, Modifier et Supprimer Un Sport. Tu peut affecter Un Sport a tout les Salles";
                ToolTip3 = "Vous avez l'access pour ajouter un nouveau Utilisateur, determiner son Rolle. Supprimer et Modifier tout les Utilisateur";
                ToolTip4 = "vous avez l'access a Consulter la liste des Clients de tout les Salles et Sports";
                ToolTip5 = "Vous avez l'access a Consulter les depenses de tout les salles";
            }

            

            for (int i = 0; i < ds.Tables["Clients"].Rows.Count ; i++)
            {
                if (ds.Tables["Clients"].Rows[i][6].ToString() != "")
                {
                      ts = DateTime.Now - DateTime.Parse(ds.Tables["Clients"].Rows[i][6].ToString());
                        int count = int.Parse(ts.Days.ToString());
                        if (count >= 28 && count == 30)
                        {
                            cpt1++;
                            soon = ds.Tables["Clients"].Rows[i][6].ToString() + "soon";
                        }
                        if (count > 30 && count <= 40)
                        {
                            cpt2++;
                            end = ds.Tables["Clients"].Rows[i][6].ToString() + "end";
                        }
                        if (count > 40)
                        {
                            cpt3++;
                            mcha = ds.Tables["Clients"].Rows[i][6].ToString() + "mcha";
                        }
                        nots.Text = (cpt1 + cpt2 + cpt3).ToString();
                    }      
            }

            da.SelectCommand.CommandText = "select * from Utilisateur";
            da.Fill(ds, "users");

            DataTable dataTable = ds.Tables["users"];

            foreach (DataRow row in dataTable.Rows)
            {

                if (row[0].ToString() == iduser)
                {
                    if (row[7].ToString() != "")
                    {
                        byte[] blob = (byte[])row[7];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        back.Source = bi;
                    }
                    else
                    {
                        back.Source = new BitmapImage(new Uri("/Resource/profil.jpg", UriKind.Relative));
                    }


                }
            }

        }
        

        private void Icon1_MouseEnter(object sender, MouseEventArgs e)
        {
            animateBorderIn(BorderToolTip);
            ToolTipTextBlock.Text = ToolTip1;
        }

        string strName, imageName;
        private void AddBackImageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fl = new OpenFileDialog();
                fl.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                if (fl.ShowDialog() == true)
                {
                    imageName = fl.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    back.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                    if (imageName != null)
                    {
                        FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                        byte[] imgByte = new byte[fs.Length];
                        fs.Read(imgByte, 0, Convert.ToInt32(fs.Length));
                        fs.Close();


                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "update Utilisateur set imaBack = @img where IdUser = '" + iduser + "'";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("img", imgByte);
                        cmd.ExecuteNonQuery();
                        cn.Close();

                    }
                    
                }
               



            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                MessageForm m = new MessageForm(msg);
                m.ShowDialog();
            }
        }

        private void Icon1_MouseLeave(object sender, MouseEventArgs e)
        {
            animateBorderOut(BorderToolTip);
        }

        private void Icon2_MouseEnter(object sender, MouseEventArgs e)
        {
            animateBorderIn(BorderToolTip);
            ToolTipTextBlock.Text = ToolTip2;
        }

        private void Icon2_MouseLeave(object sender, MouseEventArgs e)
        {
            animateBorderOut(BorderToolTip);
        }

        private void Icon3_MouseEnter(object sender, MouseEventArgs e)
        {
            animateBorderIn(BorderToolTip);
            ToolTipTextBlock.Text = ToolTip3;
        }

        private void Icon3_MouseLeave(object sender, MouseEventArgs e)
        {
            animateBorderOut(BorderToolTip);
        }

        private void Icon4_MouseEnter(object sender, MouseEventArgs e)
        {
            animateBorderIn(BorderToolTip);
            ToolTipTextBlock.Text = ToolTip4;
        }

        private void Icon4_MouseLeave(object sender, MouseEventArgs e)
        {
            animateBorderOut(BorderToolTip);
        }

        private void Icon5_MouseEnter(object sender, MouseEventArgs e)
        {
            animateBorderIn(BorderToolTip);
            ToolTipTextBlock.Text = ToolTip5;
        }

        private void Icon5_MouseLeave(object sender, MouseEventArgs e)
        {
            animateBorderOut(BorderToolTip);
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
        public void animateBorderIn(Border c)
        {
            ((Storyboard)App.Current.Resources["ToolTipAnimationGoIn"]).Begin(c);
        }
        public void animateBorderOut(Border c)
        {
            ((Storyboard)App.Current.Resources["ToolTipAnimationGoOut"]).Begin(c);
        }

    }
}
