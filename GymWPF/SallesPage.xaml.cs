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
using System.ComponentModel;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace GymWPF
{
    /// <summary>
    /// Logique d'interaction pour SallesPage.xaml
    /// </summary>
    public partial class SallesPage : Page
    {
        //Declaration -------------------------//
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter("", "Data Source=.;Initial Catalog=NSS_Salle_Application;Integrated Security=True");
        DataSet ds = new DataSet();
        SqlDataReader dr;       
        //------------------------------------//
        
        MainApp dade;
        public SallesPage(MainApp dade)
        {
            InitializeComponent();
            this.dade = dade;
            
        }
        public void LoadResoource()
        {           
            da.SelectCommand.CommandText = "select IdSalle,nom_Salle from Salle";
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(ds, "Salle");
            ListViewSalles.DataContext = ds.Tables["Salle"].DefaultView;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadResoource();              
        }

        private void ListViewSalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            int index = ListViewSalles.SelectedIndex;
            if(index >= 0)
            {
                BtnAjouter.Content = "Nouveau";
                DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
                SalleName.Text = row.Row[1].ToString();
            }
            else
            {
                SalleName.Text = "";
            }             
            
                
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
               
            if (BtnAjouter.Content.ToString() == "Nouveau")
            {
                BtnAjouter.Content = "Ajouter";
                SalleName.Text = null;
                ListViewSalles.UnselectAll();
            }
            else if (BtnAjouter.Content.ToString() == "Ajouter")
            {
                try
                {
                    if (SalleName.Text == "")
                    {
                        string msg = "Merci de remplire tout les champs";
                        MessageForm m = new MessageForm(msg);
                        m.ShowDialog();
                    }
                    
                    else 
                    {
                        DataRow r = ds.Tables["Salle"].NewRow();
                        r[1] = SalleName.Text;
                        ds.Tables["Salle"].Rows.Add(r);
                        da.SelectCommand.CommandText = "select IdSalle,nom_Salle from Salle";
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        da.Update(ds, "Salle");

                        messageContent.Text = "Bien ajoutée";
                        animateBorder(borderMessage);

                        ListViewSalles.DataContext = ds.Tables["Salle"].DefaultView;
                        ListViewSalles.UnselectAll();
                        SalleName.Text = null;
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
            }
                    
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSalles.SelectedIndex != -1)
            {
                int index = ListViewSalles.SelectedIndex;
            DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
            int id = int.Parse(row.Row[0].ToString());

                try
                {
                    if (SalleName.Text == "")
                    {
                        string msg = "Merci de remplire tout les champs";
                        MessageForm m = new MessageForm(msg);
                        m.ShowDialog();
                    }
                    else
                    {
                        DataRow r = ds.Tables["Salle"].Rows.Find(id.ToString());
                        r.BeginEdit();
                        r[1] = SalleName.Text;
                        r.EndEdit();
                        da.SelectCommand.CommandText = "select IdSalle,nom_Salle from Salle";
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        da.Update(ds, "Salle");

                        messageContent.Text = "Bien modifiée";
                        animateBorder(borderMessage);

                        ListViewSalles.DataContext = ds.Tables["Salle"].DefaultView;
                        BtnAjouter.Content = "Ajouter";
                        SalleName.Text = null;
                        ListViewSalles.UnselectAll();
                    }
                   
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    MessageForm m = new MessageForm(msg);
                    m.ShowDialog();
                }
            }          
            
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = ListViewSalles.Items.IndexOf(item);
            DataRowView row = ListViewSalles.Items.GetItemAt(index) as DataRowView;
            int id = int.Parse(row.Row[0].ToString());


          
            ConfirmForm c = new ConfirmForm("voulez vous vraiment supprimer ?");
            c.Owner = dade;
            dade.Opacity = 0.5;
            dade.Effect = new BlurEffect();
            if ((bool)c.ShowDialog())
            {
                DataRow r = ds.Tables["Salle"].Rows.Find(id);
                r.Delete();
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.Update(ds, "Salle");

                messageContent.Text = "Bien supprimée";
                animateBorder(borderMessage);

                BtnAjouter.Content = "Ajouter";
                SalleName.Text = null;
                ListViewSalles.UnselectAll();
                ListViewSalles.DataContext = ds.Tables["Salle"].DefaultView;


            }
            dade.Opacity = 1;
            dade.Effect = null;
        }
        public void animateBorder(Border c)
        {
            ((Storyboard)GridGlobal.Resources["animate"]).Begin(c);
        }
    }
}
