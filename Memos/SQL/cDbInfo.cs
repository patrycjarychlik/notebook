﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    public class cDbInfo
    {
        #region Zmienne

        private string m_login;

        private string m_password;

        private string m_database;

        private int m_port;

        private string m_ip;

        #endregion

        #region Konstruktory

        public cDbInfo(string dbPath, string password)
        {
            m_ip = dbPath;
            m_password = password;
        }

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <param name="database">Jedna z możliwych baz danych</param>
        /// <param name="strona">Strona po której używany jest adapter</param>
        public cDbInfo(string ip, int port, string database, string login, string password)
        {
            m_login = login;
            m_password = password;
            m_database = database;
            m_ip = ip;
            m_port = port;
        }

        #endregion

        #region Właściwości

        /// <summary>
        /// Ustawia, lub zwraca login użytkownika.
        /// </summary>
        public string Login
        {
            [System.Diagnostics.DebuggerStepThrough()]
            get
            {
                return m_login;
            }
        }

        /// <summary>
        /// Ustawia, lub zwraca jawne hasło użytkownika.
        /// </summary>
        public string Password
        {
            [System.Diagnostics.DebuggerStepThrough()]
            get
            {
                return m_password;
            }
        }

        /// <summary>
        /// Ustawia, lub zwraca rodzaj bazy danych.
        /// </summary>
        public string Database
        {
            [System.Diagnostics.DebuggerStepThrough()]
            get
            {
                return m_database;
            }
        }

        public string IP
        {

            [System.Diagnostics.DebuggerStepThrough()]
            get
            {
                return m_ip;
            }
        }

        public int Port
        {
            [System.Diagnostics.DebuggerStepThrough()]
            get
            {
                return m_port;
            }
        }

        #endregion
    }
}
