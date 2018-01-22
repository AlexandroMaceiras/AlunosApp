using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AlunosApp.Classes
{
    public class Utilities
    {
        public static bool Rengex { get; private set; }

        public static bool ValidarEmail(string email)
        {
            return Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success;
        }
    }
}
