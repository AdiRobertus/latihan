using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_04
{
    public partial class CoreForm : Form
    {
        protected PC_04Entities db = new PC_04Entities();
        public CoreForm()
        {
            InitializeComponent();
        }
    }

    public static class Extension
    {
        private static Random random = new Random();
        private static readonly string characters = "QWERTYUIOPASDFGHJKLZXCVBNM123456789";

        public static bool IsAnyEmpty(this Control.ControlCollection controls, params Control[] exceptions)
        {
            foreach (Control control in controls)
            {
                if (!exceptions.Contains(control) && control is TextBox && control.Text.Trim() == "")
                {
                    return true;
                }
            }

            return false;
        }

        public static void ClearFields(this Control.ControlCollection controls, params Control[] exceptions)
        {
            foreach (Control control in controls)
            {
                if (!exceptions.Contains(control) && control is TextBox)
                {
                    control.Text = "";
                }
            }
        }
        public static void ChangeState(this Control.ControlCollection controls, bool state,params Control[] exceptions)
        {
            foreach (Control control in controls)
            {
                if (!exceptions.Contains(control) && (control is TextBox || control is NumericUpDown || control is ComboBox || control is DateTimePicker || control is RadioButton))
                {
                    control.Enabled = state;
                }
            }
        }


        public static string ToSha256(this string text)
        {
            SHA256 sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static bool IsNumber(this string text)
        {
            if (text.All(char.IsNumber) && text.Trim() != "")
            {
                return true;
            }

            return false;
        }

        public static bool IsPositifNumber(this string text)
        {
            if (text.IsNumber())
            {
                int number = int.Parse(text);
                if (number > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsEmail(this string text)
        {
            var email = new EmailAddressAttribute();
            if (email.IsValid(text))
            {
                return true;
            }

            return false;
        }

        public static string GenerateRandomText(int length)
        {
            var result = new StringBuilder();
            for (int i = 0; 0 < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }
    }
}
