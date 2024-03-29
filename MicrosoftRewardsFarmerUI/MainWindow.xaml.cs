﻿using MicrosoftRewardsFarmerUI.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MicrosoftRewardsFarmerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private string pathToData = Environment.ExpandEnvironmentVariables(@"%appdata%\MicrosoftRewardsFarmerUI\data");
        private Process InstallProcess;
        private Process FarmerProcess;
        private List<Account> Accounts;
        private List<Telegram> Telegrams;
        private List<Discord> Discords;

        public MainWindow() {
            InitializeComponent();
            InitContentFiles();
            RefreshAccounts();
            RefreshTelegramAccounts();
            RefreshDiscordAccounts();
        }

        private void InitContentFiles() {
            // Check if content files exist, if not create them
            if (!Directory.Exists(pathToData)) {
                Directory.CreateDirectory(pathToData);
            }
            if (!File.Exists(pathToData + @"\allAccounts.json")) {
                File.WriteAllText(pathToData + @"\allAccounts.json", "[]");
            }
            if (!File.Exists(pathToData + @"\accounts.json")) {
                File.WriteAllText(pathToData + @"\accounts.json", "[]");
            }
            if (!File.Exists(pathToData + @"\telegrams.json")) {
                File.WriteAllText(pathToData + @"\telegrams.json", "[]");
            }
            if (!File.Exists(pathToData + @"\discords.json")) {
                File.WriteAllText(pathToData + @"\discords.json", "[]");
            }
        }

        private void RefreshAccounts() {
            Accounts = JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(pathToData + @"\allAccounts.json"));
            AccountComboBox.Items.Clear();
            AccountComboBox.Items.Add("New Account...");

            foreach (var account in Accounts) {
                AccountComboBox.Items.Add(account.username);
            }
            AccountComboBox.SelectedIndex = AccountComboBox.Items.Count - 1;

            //Write new account to allAccounts.json
            string json = JsonConvert.SerializeObject(Accounts, Formatting.Indented);
            File.WriteAllText(pathToData + @"\allAccounts.json", json);
        }

        private void NumericOnly(object sender, TextChangedEventArgs e) {
            string res = string.Empty;

            foreach (char c in ((TextBox)sender).Text) {
                if (char.IsDigit(c)) {
                    res += c;
                }
            }
            ((TextBox)sender).Text = res;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            //Get selected account
            Account account = null;
            if (AccountComboBox.SelectedIndex != 0) {
                account = Accounts[AccountComboBox.SelectedIndex - 1];

                //Write account to accounts.json
                string json = JsonConvert.SerializeObject(new List<Account> { account }, Formatting.Indented);
                File.WriteAllText(pathToData + @"\accounts.json", json);
            } else {
                MessageBox.Show("Please select an account");
                return;
            }

            //Check if we need to notify telegram
            Telegram telegram = null;
            
            if (TelegramCheckBox.IsChecked == true) {
                if (TelegramComboBox.SelectedIndex != 0) {
                    telegram = Telegrams[TelegramComboBox.SelectedIndex - 1];
                } else {
                    MessageBox.Show("Please select a telegram account");
                    return;
                }
            }

            //Check if we need to notify discord
            Discord discord = null;

            if (DiscordCheckBox.IsChecked == true) {
                if (DiscordComboBox.SelectedIndex != 0) {
                    discord = Discords[DiscordComboBox.SelectedIndex - 1];
                } else {
                    MessageBox.Show("Please select a discord account");
                    return;
                }
            }

            //Running Farmer
            FarmerProcess = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "python.exe",
                    Arguments = string.Format("-u \"{0}\"{1}{2}{3}{4}{5}{6}", 
                                                @".\data\main.py", (bool)HeadCheckBox.IsChecked ? " -v" : "", LanguageTextBox.Text.Length > 0 ? " -l " + LanguageTextBox.Text : "", 
                                                GeolocationTextBox.Text.Length > 0 ? " -g " + GeolocationTextBox.Text : "", ProxyTextBox.Text.Length > 0 ? " -p " + ProxyTextBox.Text : "",
                                                (bool)TelegramCheckBox.IsChecked ? " -t " + telegram.token + " " + telegram.chatId : "", (bool)DiscordCheckBox.IsChecked ? " -d " + discord.webhook : ""),
                    UseShellExecute = false, // Do not use OS shell
                    CreateNoWindow = true, // We don't need new window
                    RedirectStandardOutput = true, // Any output, generated by application will be redirected back
                    RedirectStandardError = true // Any error in standard output will be redirected back (for example exceptions)
                }
            };
            FarmerProcess.OutputDataReceived += DataReceivedEventHandler;
            FarmerProcess.ErrorDataReceived += DataReceivedEventHandler;

            FarmerProcess.Start();
            FarmerProcess.BeginOutputReadLine();
            FarmerProcess.BeginErrorReadLine();

            //Wait for python script to finish async and send output to OutputBlock
            FarmerProcess.WaitForExitAsync().ContinueWith(task => {
                Dispatcher.Invoke(() => {
                    OutputBlock.Text += "\n---------------------------------------\n         Farmer Finished\n---------------------------------------\n";
                });
            });
        }

        private void DataReceivedEventHandler(object sender, DataReceivedEventArgs e) {
            Dispatcher.Invoke(() => {
                OutputBlock.Text += e.Data + Environment.NewLine; 
                if ((bool)ScrollCheckBox.IsChecked) {
                    OutputScroll.ScrollToBottom();
                }
            });
        }

        private void KillButton_Click(object sender, RoutedEventArgs e) {
            InstallProcess?.Kill(true);
            FarmerProcess?.Kill(true);
            OutputBlock.Text += "\n---------------------------------------\n         Farmer Process Killed\n---------------------------------------\n";
        }

        private void AccountComboBox_DropDownOpened(object sender, EventArgs e) {
            RefreshAccounts();
        }

        private void AccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (AccountComboBox.SelectedIndex == 0) {
                EmailTextBox.Text = "";
                PasswordTextBox.Password = "";
            } else {
                try {
                    EmailTextBox.Text = Accounts[AccountComboBox.SelectedIndex - 1].username;
                    PasswordTextBox.Password = Accounts[AccountComboBox.SelectedIndex - 1].password;
                } catch (Exception) {
                    // Do nothing
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            //Save Account to allAccounts.json
            if (AccountComboBox.SelectedIndex == 0) {
                // New Account
                Accounts.Add(new Account(EmailTextBox.Text, PasswordTextBox.Password));
            } else {
                // Existing Account
                Accounts[AccountComboBox.SelectedIndex - 1].username = EmailTextBox.Text;
                Accounts[AccountComboBox.SelectedIndex - 1].password = PasswordTextBox.Password;
            }
            string json = JsonConvert.SerializeObject(Accounts, Formatting.Indented);
            File.WriteAllText(pathToData + @"\allAccounts.json", json);
            RefreshAccounts();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            //Delete Account from allAccounts.json
            if (AccountComboBox.SelectedIndex != 0) {
                Accounts.RemoveAt(AccountComboBox.SelectedIndex - 1);
                string json = JsonConvert.SerializeObject(Accounts, Formatting.Indented);
                File.WriteAllText(pathToData + @"\allAccounts.json", json);
                RefreshAccounts();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e) {
            OutputBlock.Text = string.Empty;
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e) {
            //Installing requirements
            InstallProcess = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "pip.exe",
                    Arguments = string.Format("install -r {0}", ".\\data\\requirements.txt"),
                    UseShellExecute = false, // Do not use OS shell
                    CreateNoWindow = true, // We don't need new window
                    RedirectStandardOutput = true, // Any output, generated by application will be redirected back
                    RedirectStandardError = true // Any error in standard output will be redirected back (for example exceptions)
                }
            };
            InstallProcess.OutputDataReceived += DataReceivedEventHandler;
            InstallProcess.ErrorDataReceived += DataReceivedEventHandler;

            InstallProcess.Start();
            InstallProcess.BeginOutputReadLine();
            InstallProcess.BeginErrorReadLine();

            //Wait for pip install to finish async and send output to OutputBlock
            InstallProcess.WaitForExitAsync().ContinueWith(task => {
                Dispatcher.Invoke(() => {
                    OutputBlock.Text += "\n---------------------------------------\n         Install Finished\n---------------------------------------\n";
                });
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            InstallProcess?.Kill();
            FarmerProcess?.Kill();
        }

        private void RefreshTelegramAccounts() {
            Telegrams = JsonConvert.DeserializeObject<List<Telegram>>(File.ReadAllText(pathToData + @"\telegrams.json"));
            TelegramComboBox.Items.Clear();
            TelegramComboBox.Items.Add("New Telegram...");

            foreach (var telegram in Telegrams) {
                TelegramComboBox.Items.Add(telegram.chatId);
            }
            TelegramComboBox.SelectedIndex = TelegramComboBox.Items.Count - 1;

            //Write new account to telegram.json
            string json = JsonConvert.SerializeObject(Telegrams, Formatting.Indented);
            File.WriteAllText(pathToData + @"\telegrams.json", json);
        }

        private void TelegramSaveButton_Click(object sender, RoutedEventArgs e) {
            //Save Telegram to telegrams.json
            if (TelegramComboBox.SelectedIndex == 0) {
                // New Telegram
                Telegrams.Add(new Telegram(TokenTextBox.Text, ChatIDTextBox.Text));
            } else {
                // Existing Telegram
                Telegrams[TelegramComboBox.SelectedIndex - 1].token = TokenTextBox.Text;
                Telegrams[TelegramComboBox.SelectedIndex - 1].chatId = ChatIDTextBox.Text;
            }
            string json = JsonConvert.SerializeObject(Telegrams, Formatting.Indented);
            File.WriteAllText(pathToData + @"\telegrams.json", json);
            RefreshTelegramAccounts();
        }

        private void TelegramDeleteButton_Click(object sender, RoutedEventArgs e) {
            //Delete Telegram from telegrams.json
            if (TelegramComboBox.SelectedIndex != 0) {
                Telegrams.RemoveAt(TelegramComboBox.SelectedIndex - 1);
                string json = JsonConvert.SerializeObject(Telegrams, Formatting.Indented);
                File.WriteAllText(pathToData + @"\telegrams.json", json);
                RefreshTelegramAccounts();
            }
        }

        private void TelegramComboBox_DropDownOpened(object sender, EventArgs e) {
            RefreshTelegramAccounts();
        }

        private void TelegramComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TelegramComboBox.SelectedIndex == 0) {
                TokenTextBox.Text = "";
                ChatIDTextBox.Text = "";
            } else {
                try {
                    TokenTextBox.Text = Telegrams[TelegramComboBox.SelectedIndex - 1].token;
                    ChatIDTextBox.Text = Telegrams[TelegramComboBox.SelectedIndex - 1].chatId;
                } catch (Exception) {
                    // Do nothing
                }
            }
        }

        private void RefreshDiscordAccounts() {
            Discords = JsonConvert.DeserializeObject<List<Discord>>(File.ReadAllText(pathToData + @"\discords.json"));
            DiscordComboBox.Items.Clear();
            DiscordComboBox.Items.Add("New Discord...");

            foreach (var discord in Discords) {
                DiscordComboBox.Items.Add(discord.webhook);
            }
            DiscordComboBox.SelectedIndex = DiscordComboBox.Items.Count - 1;

            //Write new account to discords.json
            string json = JsonConvert.SerializeObject(Discords, Formatting.Indented);
            File.WriteAllText(pathToData + @"\discords.json", json);
        }

        private void DiscordSaveButton_Click(object sender, RoutedEventArgs e) {
            //Save Discord to discords.json
            if (DiscordComboBox.SelectedIndex == 0) {
                // New Discord
                Discords.Add(new Discord(WebhookTextBox.Text));
            } else {
                // Existing Discord
                Discords[DiscordComboBox.SelectedIndex - 1].webhook = WebhookTextBox.Text;
            }
            string json = JsonConvert.SerializeObject(Discords, Formatting.Indented);
            File.WriteAllText(pathToData + @"\discords.json", json);
            RefreshDiscordAccounts();
        }

        private void DiscordDeleteButton_Click(object sender, RoutedEventArgs e) {
            //Delete Discord from discords.json
            if (DiscordComboBox.SelectedIndex != 0) {
                Discords.RemoveAt(DiscordComboBox.SelectedIndex - 1);
                string json = JsonConvert.SerializeObject(Discords, Formatting.Indented);
                File.WriteAllText(pathToData + @"\discords.json", json);
                RefreshDiscordAccounts();
            }
        }

        private void DiscordComboBox_DropDownOpened(object sender, EventArgs e) {
            RefreshDiscordAccounts();
        }

        private void DiscordComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DiscordComboBox.SelectedIndex == 0) {
                WebhookTextBox.Text = "";
            } else {
                try {
                    WebhookTextBox.Text = Discords[DiscordComboBox.SelectedIndex - 1].webhook;
                } catch (Exception) {
                    // Do nothing
                }
            }
        }
    }
}
