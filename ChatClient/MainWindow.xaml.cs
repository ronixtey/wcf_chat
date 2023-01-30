﻿using System;
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

using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChatClient client;
        int Id;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client.SendMsg(tbMessage.Text, Id);
                    tbMessage.Text = String.Empty;
                }
            }
        }

        void ConnectUser()
        {
            if (isConnected) return;

            client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));

            Id = client.Connect(tbUserName.Text); 
            tbUserName.IsEnabled = false;                   
            btConnDisconn.Content = "Disconnect";
            isConnected = true;
        }

        void DisconnectUser() 
        {
            if (!isConnected) return;

            client.Disconnect(Id);
            tbUserName.IsEnabled = true;
            btConnDisconn.Content = "Connect";
            isConnected = false;

            client = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
                DisconnectUser();
            else
                ConnectUser();
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }
    }
}
