using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace Tietokanta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();

            //Yhdistä tietokanta
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Tietokantaa.mdb";
            BindGrid();
        }

        //Näytä tietueet
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from tbOpi";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        //Lisää tietueita
        private void BtnLisätä_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtOpiId.Text != "")
            {
                if (txtOpiId.IsEnabled == true)
                {
                    if (ddlSukupuoli.Text != "Valitse Sukupuoli")
                    {
                        cmd.CommandText = "insert into tbOpi(OpiId,OpiNimi,Sukupuoli,Kontakti,Osoite) Values(" + txtOpiId.Text + ",'" + txtOpiNimi.Text + "','" + ddlSukupuoli.Text + "'," + txtKontakti.Text + ",'" + txtOsoite.Text + "')";
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Opiskelija lisätty onnistuneesti...");
                        ClearAll();

                    }
                    else
                    {
                        MessageBox.Show("Valitse Sukupuoli....");
                    }
                }
                else
                {
                    cmd.CommandText = "update tbOpi set OpiNimi='" + txtOpiNimi.Text + "',Sukupuoli='" + ddlSukupuoli.Text + "',Kontakti=" + txtKontakti.Text + ",Osoite='" + txtOsoite.Text + "' where OpiId=" + txtOpiId.Text;
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Opiskelijan tiedot päivitetty onnistuneesti...");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Lisää opiskelijan tunnus.......");
            }
            
        }

        //Tyhjennä tietueet
        private void BtnPeruuttaa_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtOpiId.Text = "";
            txtOpiNimi.Text = "";
            ddlSukupuoli.SelectedIndex = 0;
            txtKontakti.Text = "";
            txtOsoite.Text = "";
            BtnLisätä.Content = "Lisätä";
            txtOpiId.IsEnabled = true;
        }

        //Muokata tietueet
        private void BtnMuokata_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtOpiId.Text = row["OpiId"].ToString();
                txtOpiNimi.Text = row["OpiNimi"].ToString();
                ddlSukupuoli.Text = row["Sukupuoli"].ToString();
                txtKontakti.Text = row["Kontakti"].ToString();
                txtOsoite.Text = row["Osoite"].ToString();
                txtOpiId.IsEnabled = false;
                BtnLisätä.Content = "Päivitä";
            }
            else
            {
                MessageBox.Show("Valitse opiskelija luettelosta...");
            }
        }

        //Poistaa tietueet
        private void BtnPoistaa_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from tbOpi where OpiId=" + row["OpiId"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Opiskelija poistettu onnistuneesti...");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Valitse opiskelija luettelosta...");
            }
        }

        //Poistu sovelluksesta
        private void BtnPoistua_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}