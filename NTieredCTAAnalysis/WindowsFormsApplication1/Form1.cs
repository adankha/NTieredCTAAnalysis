using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data.SqlClient;
using System.Diagnostics;
using System.Numerics;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
            }
            catch
            {
                // Ignore any exception, only trying to start-up the DB when we run the program.
            }
        }

        private void ClearRidership()
        {
            // Clear Ridership UI
            txtTotalRidership.Clear();
            txtTotalRidership.Refresh();
            txtAvgRidership.Clear();
            txtAvgRidership.Refresh();
            txtPercentRidership.Clear();
            txtPercentRidership.Refresh();
        }

        private void ClearDays()
        {
            // Clear the Days UI
            txtWeekday.Clear();
            txtWeekday.Refresh();
            txtSaturday.Clear();
            txtSaturday.Refresh();
            txtSunHoliday.Clear();
            txtSunHoliday.Refresh();
        }

        private void ClearStationInformation()
        {
            // Clear Station Info. UI
            txtHandicap.Clear();
            txtHandicap.Refresh();
            txtDirectionTravel.Clear();
            txtDirectionTravel.Refresh();
            txtLocation.Clear();
            txtLocation.Refresh();
            txtLines.Clear();
            txtLines.Refresh();
        }

        private void ClearStops()
        {
            listBox2.Items.Clear();
            listBox2.Refresh();
        }
        private void ClearStationListBox()
        {
            listBox1.Items.Clear();
            listBox1.Refresh();
        }
        private void ClearTopTen()
        {

            listView1.Clear();
            listView1.Refresh();
        }

        private void ClearAll()
        {
            ClearStationListBox();
            ClearStationInformation();
            ClearRidership();
            ClearDays();
            ClearTopTen();
            ClearStops();

        }

        private void buttonTopTen_Click(object sender, EventArgs e)
        {
            ClearAll();

            string filename;
            try
            {
                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                int nStations;
                if (!int.TryParse(this.txtTopN.Text, out nStations))
                    throw new Exception("You did not enter an integer to the box below the button.");

                Convert.ToInt32(this.txtTopN.Text);
                Console.WriteLine(nStations);
                var topStations = bizTier.GetTopStations(nStations);

                listView1.View = View.Details;
                listView1.Columns.Add("Station Name", -2, HorizontalAlignment.Left);
                listView1.Columns.Add("DailyTotal", -2, HorizontalAlignment.Right);

                foreach (var station in topStations)
                {
                    this.listBox1.Items.Add(station.Name);
                    listView1.Items.Add(new ListViewItem(new[] { station.Name, string.Format("{0:n0}", station.DailyTotal) }));
                }


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }

        }

        int stationID = -999;

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            ClearStationInformation();
            ClearRidership();
            ClearDays();
            ClearStops();
            string userHighlight = null;
            string filename;

            try
            {
                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier = new BusinessTier.Business(filename);
                var stations = bizTier.GetStations();
                // Find the station that the user clicked on
                foreach (Object obj in listBox1.SelectedItems)
                    userHighlight = obj.ToString();
                // If nothing selected, throw an exception
                if (userHighlight == null)
                    throw new Exception("Error: Nothing selected.");
                // Go through the station database to find the name user clicked on



                foreach (var station in stations)
                {
                    // If we found a match, get the ID
                    if (userHighlight == station.Name)
                    {
                        // Grab the stops based off of the ID
                        stationID = station.ID;
                        var stops = bizTier.GetStops(station.ID);
                        // Add each stop to listBox2
                        foreach (var stop in stops)
                            listBox2.Items.Add(stop.Name);
                        break;
                    }
                }

                var stationData = bizTier.GetStationData(stationID);
                foreach (var data in stationData)
                {
                    txtTotalRidership.AppendText(string.Format("{0:n0}", data.TotalRidership));
                    txtAvgRidership.AppendText(string.Format("{0:n0}", data.AvgRidership));
                    txtPercentRidership.AppendText(String.Format("{0:n}", data.PercentRidership) + "%");

                    txtWeekday.AppendText(string.Format("{0:n0}", data.Weekdays));
                    txtSaturday.AppendText(string.Format("{0:n0}", data.Saturdays));
                    txtSunHoliday.AppendText(string.Format("{0:n0}", data.Sunday_Holidays));
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }

        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            ClearStationInformation();


            try
            {
                string filename, userHighlight = null;

                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier = new BusinessTier.Business(filename);
                var stations = bizTier.GetStations();
                // Find the station that the user clicked on
                foreach (Object obj in listBox2.SelectedItems)
                    userHighlight = obj.ToString();
                // If nothing selected, throw an exception
                if (userHighlight == null)
                    throw new Exception("Error: Nothing selected.");
                // Go through the station database to find the name user clicked on



                var stops = bizTier.GetStops(stationID);
                // Add each stop to listBox2
                foreach (var stop in stops)
                {
                    if (userHighlight == stop.Name)
                    {
                        string ada = Convert.ToString(stop.ADA);
                        string direction = Convert.ToString(stop.Direction);
                        string latitude = Convert.ToString(stop.Latitude);
                        string longitude = Convert.ToString(stop.Longitude);


                        txtHandicap.AppendText(ada);
                        txtDirectionTravel.AppendText(direction);
                        txtLocation.AppendText("(" + latitude + ", " + longitude + ")");

                        string stopName = stop.Name.Replace("'", "''");
                        var Colors = bizTier.GetStopColorInfo(stopName);
                        foreach (var color in Colors)
                        {
                            txtLines.AppendText(color.ColorDetail + "\n");
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }


        }

        private void byTotalRidershipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();

            string filename;
            try
            {
                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                int nStations;
                if (!int.TryParse(this.txtTopN.Text, out nStations))
                    throw new Exception("You did not enter an integer to the box left of button.");

                Convert.ToInt32(this.txtTopN.Text);
                Console.WriteLine(nStations);
                var topStations = bizTier.GetTopStations(nStations);

                listView1.View = View.Details;
                listView1.Columns.Add("Station Name", -2, HorizontalAlignment.Left);
                listView1.Columns.Add("DailyTotal", -2, HorizontalAlignment.Right);

                foreach (var station in topStations)
                {
                    this.listBox1.Items.Add(station.Name);
                    listView1.Items.Add(new ListViewItem(new[] { station.Name, string.Format("{0:n0}", station.DailyTotal) }));
                }


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }
        }



        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();


            string filename;
            try
            {
                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);

                var stations = bizTier.GetStations();

                foreach (var station in stations)
                {
                    this.listBox1.Items.Add(station.Name);
                }


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();
            txtlikeName.Clear();
            txtTopN.Clear();
        }

        private void likeNameBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            string filename;

            try
            {
                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);

                //var stations = bizTier.GetStations();
                string likeName = this.txtlikeName.Text.Replace("'", "''");

                var stations = bizTier.GetStations(likeName);
                foreach (var station in stations)
                {
                    this.listBox1.Items.Add(station.Name);
                }


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }
        }

        private void topTenByDay(string day = "", int N = 10 )
        {
            ClearAll();
            string filename;
            try
            {
                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);

                var topStations = bizTier.GetTopStations(N, day);

                listView1.View = View.Details;
                listView1.Columns.Add("Station Name", -2, HorizontalAlignment.Left);
                listView1.Columns.Add("DailyTotal", -2, HorizontalAlignment.Right);

                foreach (var station in topStations)
                {
                    this.listBox1.Items.Add(station.Name);
                    listView1.Items.Add(new ListViewItem(new[] { station.Name, string.Format("{0:n0}", station.DailyTotal) }));
                }


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }

        }

        private void byWeekdayToolStripMenuItem_Click(object sender, EventArgs e)
        {

            topTenByDay("W");

        }

        private void bySaturdayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            topTenByDay("A");
        }

        private void bySundayHolidayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            topTenByDay("U");
        }

        private void bttnUpdateADA_Click(object sender, EventArgs e)
        {

            string filename;

            try
            {
                filename = this.txtDatabaseFname.Text;       // File name of the Database to load in
                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                string newADA;


                if (txtHandicap.Text.Equals("False"))
                {
                    txtHandicap.Clear();
                    txtHandicap.Refresh();
                    newADA = "True";
                }
                else
                {
                    txtHandicap.Clear();
                    txtHandicap.Refresh();
                    newADA = "False";
                }


                //var stations = bizTier.GetStations();
                string stopName = this.listBox2.SelectedItem.ToString();
                stopName = stopName.Replace("'", "''");

                bizTier.UpdateStopADA(stopName);

                txtHandicap.AppendText(newADA);


            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
            finally
            {

            }
        }
    }
}
