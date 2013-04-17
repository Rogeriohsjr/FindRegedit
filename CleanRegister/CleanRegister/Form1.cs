using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CleanRegister
{
    public partial class Form1 : Form
    {
        List<string> sListFolderToDelete = new List<string>();

        public Form1()
        {
            InitializeComponent();
            txtAdress.Text = "SOFTWARE\\WOW6432Node\\Microsoft\\Classes\\CLSID";
            txtKeyWord.Text = "C:\\Program Files\\Processware\\O2P";
                 
        }

        private string[] GetFolder(string sAddress)
        {
            RegistryKey root = Registry.LocalMachine.OpenSubKey(sAddress, true);
            return root.GetSubKeyNames();
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            bool bFound = false;
            string sAddressN0 = txtAdress.Text;
            
            string[] sFolderCLSID = GetFolder(sAddressN0);

            foreach (string sFolder in sFolderCLSID)
            {
                string sAddressN1 = sAddressN0 + "\\" + sFolder;
                string[] sNivel1 = GetFolder(sAddressN1);
                
                //001
                foreach (string sFolderN1 in sNivel1)
                {
                    string sAddressN2 = sAddressN1 + "\\" + sFolderN1;

                    RegistryKey N2 = Registry.LocalMachine.OpenSubKey(sAddressN2, true);
                    string[] sF1 = N2.GetValueNames();

                    foreach (string sFile1 in sF1)
                    {
                        if (N2.GetValue(sFile1).ToString().Replace("\\", "") == txtKeyWord.Text.Replace("\\", ""))
                        {
                            bFound = true;
                            break;
                        }
                    }

                    if (bFound)
                    {
                        sListFolderToDelete.Add(sAddressN1);
                       bFound = false;
                    }
                }
            }

            lstListKeys.DataSource = sListFolderToDelete;
            lstListKeys.Update();
            lblTotal.Text = "Total: " + sListFolderToDelete.Count;

        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            foreach (string sItem in sListFolderToDelete)
            {
                Registry.LocalMachine.DeleteSubKeyTree(sItem);
            }

            sListFolderToDelete.Clear();
            lstListKeys.DataSource = sListFolderToDelete;
            lstListKeys.Update();
            lblTotal.Text = "Total: " + sListFolderToDelete.Count;
        }
    }
}
