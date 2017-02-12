﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Customer
{
    public partial class CustomerProfile : Form
    {
        DataHandler dh = new DataHandler("localhost", "boombroom", "root", "root");

        int customerID;
        //bool isAdmin; //PIVOTAL PARAMETER!
        public CustomerProfile(int customerID)
        {
            this.customerID = customerID;
            InitializeComponent();
        }

        private void CustomerProfile_Load(object sender, EventArgs e)
        {
            //dataGridView1.DataSource;
            DataTable temp = dh.getAllAccountsByCustomerDataTable(customerID);
            lblAccount.Text = "Number of Active Accounts: " + dh.numberOfActiveAccounts(customerID);
            
            DataTable dt = new DataTable();
            dt.Columns.Add("Account ID");
            dt.Columns.Add("Money Lent");
            dt.Columns.Add("Entry Date");
            dt.Columns.Add("Status");


            Console.WriteLine(temp.Rows[0][3].GetType());
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = temp.Rows[i][0];
                dr[1] = temp.Rows[i][2];
                dr[2] = ((DateTime)temp.Rows[i][3]).ToString("MM-dd-yyyy");
                dr[3] = temp.Rows[i][4];

                dt.Rows.Add(dr);
            }

            dgvAccounts.DataSource = dt;
            dgvAccounts.Columns[0].Visible = false; //Hide accountID
            dgvAccounts.Sort(dgvAccounts.Columns[3], ListSortDirection.Ascending);

        }

        private void dgvAccounts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int accountID = int.Parse(dgvAccounts.Rows[e.RowIndex].Cells[0].Value.ToString());
            Console.WriteLine(accountID);
            refreshAccountProfile(accountID);
            
        }

        private void refreshAccountProfile(int accountID)
        {
            
            string[] accountDetails = dh.getAccountDetails(accountID);
            if (accountDetails[4] == "Paid")
                panelBreakdown.Visible = false;
            else
                panelBreakdown.Visible = true;

            lblEntryDate.Text = "Entry Date: " + dh.getEntryDate(accountID).ToString("MM-dd-yyyy");
            lblInterestRate.Text = "Interest Rate: " + accountDetails[5];
            lblMoneyLent.Text = "Money Lent: " + accountDetails[2];
            lblStatus.Text = "Status: " + accountDetails[4];
            lblInterest.Text = "Interest: " + dh.getInterestAmount(accountID);
            lblTotalLoan.Text = "Total Loan: " + (dh.getInterestAmount(accountID) + double.Parse(accountDetails[2]));
            lblAmountPaid.Text = "Amount Paid: " + dh.getTotalPayment(accountID);
            lblAmountRemaining.Text = "Amount Remaining: " + (dh.getInterestAmount(accountID) + double.Parse(accountDetails[2]) - dh.getTotalPayment(accountID));


        }

        private void lblRemainingLoan_Click(object sender, EventArgs e)
        {
            lblInterest.Text = "Remaining Loan: "+ dh.getInterestAmount(11);
        }
    }
}
