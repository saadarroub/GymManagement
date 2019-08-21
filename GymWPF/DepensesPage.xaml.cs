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
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour DepensesPage.xaml
    /// </summary>
    public partial class DepensesPage : Page
    {
        string  ConnectedSalle, ConnectedSport, iduser;     

        MainApp dade;


        //declaration --------------------------------------
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;
        //------------------------------------------------------

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DateTimePicker.Language = System.Windows.Markup.XmlLanguage.GetLanguage("fr");
            
            if (ConnectedSalle.ToString() == "" && ConnectedSport.ToString() == "")
            {
                BtnAjouter.IsEnabled = false;
                BtnModifier.IsEnabled = false;
                DepensesTextBox.IsEnabled = false;
                DateTimePicker.IsEnabled = false;
                PrixTextBox.IsEnabled = false;
                BtnAjouter.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                BtnModifier.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "select d.IdDep as IdDep, d.Depense as Depense,convert(varchar, d.date_dep, 103) as date_dep,d.prix as prix,u.UserName as UserName,s.nom_Salle as nom_Salle,t.nom_Type as nom_Type,d.IdSalle,d.IdType,d.IdUser from Depenses d join Utilisateur u on d.IdUser=u.IdUser join Salle s on d.IdSalle=s.IdSalle join Type_Sport t on d.IdType=t.IdType ";
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                ListViewUtilisateurs.DataContext = dt;
                cn.Close();
            }
            else
            {
                loaded();

            }
        }
        private void loaded()
        {
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select d.IdDep as IdDep, d.Depense as Depense,convert(varchar, d.date_dep, 103) as date_dep,d.prix as prix,u.UserName as UserName,s.nom_Salle as nom_Salle,t.nom_Type as nom_Type,d.IdSalle,d.IdType,d.IdUser from Depenses d join Utilisateur u on d.IdUser=u.IdUser join Salle s on d.IdSalle=s.IdSalle join Type_Sport t on d.IdType=t.IdType  where d.IdSalle='" + ConnectedSalle+"' and d.IdType='"+ConnectedSport+"' and d.IdUser='"+iduser+"'";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            ListViewUtilisateurs.DataContext = dt;
            cn.Close();
        }

        private void ListViewUtilisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewUtilisateurs.SelectedIndex != -1)
            {
                BtnAjouter.Content = "Nouveau";

                int index = ListViewUtilisateurs.SelectedIndex;
                DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
                DepensesTextBox.Text = row.Row[1].ToString();
                DateTimePicker.Text = row.Row[2].ToString();
                PrixTextBox.Text = row.Row[3].ToString();
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewUtilisateurs.SelectedIndex == -1)
            {
                messageContent.Text = "veuillez selectioner une ligne";
                animateBorder(borderMessage);
            }
            else
            {
             if (DepensesTextBox.Text == "" || DateTimePicker.Text == "" || PrixTextBox.Text == "")
            {
                messageContent.Text = "Merci de remplire tout les champs";
                animateBorder(borderMessage);
            }
            else
            {
                try
                {
                    int index = ListViewUtilisateurs.SelectedIndex;
                    DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
                    int id = int.Parse(row.Row[0].ToString());

                    cn.Open();
                    cmd.Connection = cn;
                        cmd.Parameters.Clear();
                    cmd.CommandText = "update Depenses set  Depense ='" + DepensesTextBox.Text + "', date_dep = @a, prix = @b where IdDep = '" + id + "'";
                        cmd.Parameters.AddWithValue("@a", DateTime.Parse(DateTimePicker.Text.ToString(), new System.Globalization.CultureInfo("fr")));
                        cmd.Parameters.AddWithValue("@b",double.Parse(PrixTextBox.Text));
                        cmd.ExecuteNonQuery();

                    messageContent.Text = "Bien modifiée";
                    animateBorder(borderMessage);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
                finally
                {
                    cn.Close();
                    BtnAjouter.Content = "Ajouter";
                    DepensesTextBox.Text = null;
                    DateTimePicker.Text = null;
                    PrixTextBox.Text = null;
                    ListViewUtilisateurs.UnselectAll();

                    loaded();
                }
            }
            }

           
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewUtilisateurs.Items.IndexOf(item);
            DataRowView row = ListViewUtilisateurs.Items.GetItemAt(index) as DataRowView;
            
               

            ConfirmForm c = new ConfirmForm("voulez vous vraiment supprimer ?");
            c.Owner = dade;
            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            if ((bool)c.ShowDialog())
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "delete from Depenses where IdDep = '" + row.Row[0].ToString() + "'";
                cmd.ExecuteNonQuery();
                cn.Close();

                messageContent.Text = "Bien supprimée";
                animateBorder(borderMessage);

                BtnAjouter.Content = "Ajouter";
                DepensesTextBox.Text = null;
                DateTimePicker.Text = null;
                PrixTextBox.Text = null;
                ListViewUtilisateurs.UnselectAll();
                loaded();

            }
            dade.Opacity = 1;
            dade.Effect = null;
        }

        public DepensesPage(MainApp d, string ConnectedSalle, string ConnectedSport, string iduser)
        {
            this.ConnectedSalle = ConnectedSalle;
            this.ConnectedSport = ConnectedSport;
            this.iduser = iduser;
            InitializeComponent();
            this.dade = d;
        }

        private void PrixTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex(@"\D");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAjouter.Content.ToString() == "Nouveau")
            {
                BtnAjouter.Content = "Ajouter";
                DepensesTextBox.Text = null;
                DateTimePicker.Text = null;
                PrixTextBox.Text = null;
                ListViewUtilisateurs.UnselectAll();
            }
            else if (BtnAjouter.Content.ToString() == "Ajouter")

            {
                if (DepensesTextBox.Text == "" || DateTimePicker.Text == "" || PrixTextBox.Text == "")
                {
                    messageContent.Text = "Merci de remplire tout les champs";
                    animateBorder(borderMessage);
                }
                else
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.Parameters.Clear();
                        cmd.CommandText = "insert into Depenses values ('" + DepensesTextBox.Text + "', @a ,'" + double.Parse(PrixTextBox.Text) + "','" + ConnectedSalle + "','" + ConnectedSport + "','" + iduser + "')";

                        cmd.Parameters.AddWithValue("@a", DateTime.Parse(DateTimePicker.Text.ToString(), new System.Globalization.CultureInfo("fr")));
                        cmd.ExecuteNonQuery();
                        messageContent.Text = "Bien ajoutée";
                        animateBorder(borderMessage);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        MessageForm m = new MessageForm(msg);
                        m.ShowDialog();
                    }
                    finally
                    {
                        cn.Close();
                        DepensesTextBox.Text = null;
                        DateTimePicker.Text = null;
                        PrixTextBox.Text = null;
                        loaded();
                    }
                }

            }

        }

        public void animateBorder(Border c)
        {
            ((Storyboard)GridGlobal.Resources["animate"]).Begin(c);
        }
    }
}
