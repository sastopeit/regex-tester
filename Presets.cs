using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Sastopeit.RegexTester
{
    public class Presets
    {
        public List<StringCollection> PresetList { get; set; } = new List<StringCollection>();  // Generische Liste, die sog. StringCollections enthält


        public Presets()
        {
            PresetList.Add(new StringCollection {
                "4-stellige Zahl, 1. Stelle ungleich \"0\"",
                "7344",
                "^[1-9][0-9]{3}$"
            });
            PresetList.Add(new StringCollection {
                "Wort mit 4 Buchstaben",
                "Haus",
                "^[A-zäöüÄÖÜ][a-zäöüß]{3}$"
            });
            PresetList.Add(new StringCollection {
                "Wort mit Großbuchstaben zu Beginn, das mit \"en\" endet",
                "Tannen",
                "^[A-ZÄÜÖ][a-zäöüß]+en$"
            });
            PresetList.Add(new StringCollection {
                "\"Wort\", das nur aus Kleinbuchstaben besteht und keinen Selbstlaut enthält",
                "dkfs",
                "^([b-d]|[f-h]|[j-n]|[p-t]|[v-z]){2,}$"
            });
            PresetList.Add(new StringCollection {
                "Zahl (1. Stelle ungleich \"0\") mit Einheit (\" Stück\")",
                "60 Stück",
                "^[1-9][0-9]* Stück$"
            });
            PresetList.Add(new StringCollection {
                "Ziffer Strich Ziffer Strich Ziffer Strich",
                "1-7-6",
                "^([0-9]-){2}[0-9]$"
            });
            PresetList.Add(new StringCollection {
                "Nur Buchstabe(n) \"a\" und/oder \"b\" enthalten",
                "abbababbaaa",
                "^[ab]+$"
            });
            PresetList.Add(new StringCollection {
                "Binärzahlen (längenmäßig unbeschränkt)",
                "00100101",
                "^[01]+$"
            });
            PresetList.Add(new StringCollection {
                "Dreistellige Zahl von -750 bis 750 (wenn Zahl positiv, dann kein Vorzeichen)",
                "747",
                "^-?(7[0-4][0-9]|750|[1-6][0-9]{2})$"
            });
            PresetList.Add(new StringCollection {
                "4-stellig; Buchstaben \"A\" an 1. Stelle gefolgt von Zahl [300;399] oder Buchstabe \"B\" zu Beginn gefolgt von Zahl [707;717;...;797] (Zahl: 1. Stelle \"7\", 2. Stelle beliebig, 3. Stelle \"7\")",
                "A380",
                "^A[300-399]|B7[0-9]7$"
            });
            PresetList.Add(new StringCollection {
                "Zulässige E-Mail-Adresse",
                "bill_gates@googlemail.com",
                "^[A-z0-9_\\.!#$%&'*+/=?^`{|}~-]{1,64}@[A-z0-9_\\.-]+[a-z]{2,}$"
            });
            PresetList.Add(new StringCollection {
                "Wort mit mind. 2 gleichen aufeinanderfolgenden Zeichen bzw. Buchstaben",
                "Schiff",
                "(.)\\1"
            });
            PresetList.Add(new StringCollection {
                "Uhrzeit im Format 00:00:00",
                "16:41:33",
                "^[01][0-9]|2[0-3](:[0-5][0-9]){2}$"
            });
            PresetList.Add(new StringCollection {
                "Betrag in € (optional 2 Kommastellen), mit 1.000er-Trennzeichen",
                "10.000,00 €",
                "^-?[1-9][0-9]?[0-9]?(\\.[0-9]{3})*(,[0-9]{2})? ?€$"
            });
            PresetList.Add(new StringCollection {
                "Smiley (1. Zeichen \":\" oder \"\"); 2. Zeichen \"-\" 3. Zeichen \"(\", \")\" oder \"|\")",
                ";-)",
                "^[:;]-[()|]$"
            });
        }
        


        //private void B10_Preset_Click(object sender, RoutedEventArgs e)
        //{
        //    TBX_Intention.Text = "4-stellig; Buchstaben \"A\" an 1. Stelle gefolgt von Zahl [300;399] oder Buchstabe \"B\" zu Beginn gefolgt von Zahl [707;717;...;797] (Zahl: 1. Stelle \"7\", 2. Stelle beliebig, 3. Stelle \"7\")";
        //    TBX_String.Text = "A380";
        //    TBX_RegEx.Text = "^A[300-399]|B7[0-9]7$";
        //    // Alternative mit Quantifizierer: ^A3[0-9]{2}|B7[0-9]7$
        //    // Alternative ohne Quantifizierer: ^A3[0-9][0-9]|B7[0-9]7$
        //}
        //private void B11_Preset_Click(object sender, RoutedEventArgs e)
        //{
        //    TBX_Intention.Text = "Zulässige E-Mail-Adresse";
        //    TBX_String.Text = "bill_gates@googlemail.com";
        //    // Regeln: https://en.wikipedia.org/wiki/Email_address
        //    // Annäherung (ein paar Regeln sind noch nicht umgesetzt und in den Zeichenauswahlen sind Dinge wie z.B. das _ überflüssig (siehe ASCII-Tabelle)):
        //    TBX_RegEx.Text = "^[A-z0-9_\\.!#$%&'*+/=?^`{|}~-]{1,64}@[A-z0-9_\\.-]+[a-z]{2,}$";
        //}
        //private void B12_Preset_Click(object sender, RoutedEventArgs e)
        //{
        //    TBX_Intention.Text = "Wort mit mind. 2 gleichen aufeinanderfolgenden Zeichen bzw. Buchstaben";
        //    TBX_String.Text = "Schiff";
        //    // Was ist z.B. mit "Aal", also Groß- und Kleinbuchstabe "vom selben Typ"???
        //    // ... Lösung dafür vielleicht per "Unicode Categories" Lu (uppercase) und Ll (lowercase)
        //    //
        //    // Abkürzungen mit ausschließlich Großbuchstaben wie "OOP"???
        //    //
        //    // Krude Lösung mit ALLEN Zeichen (nicht nur Buchstaben):
        //    TBX_RegEx.Text = "(.)\\1";
        //}
        //private void B13_Preset_Click(object sender, RoutedEventArgs e)
        //{
        //    TBX_Intention.Text = "Uhrzeit im Format 00:00:00";
        //    TBX_String.Text = "16:41:33";
        //    TBX_RegEx.Text = "^[01][0-9]|2[0-3]:[0-5][0-9]:[0-5][0-9]$";
        //    // Alternative mit Quantifizierer: ^[01][0-9]|2[0-3](:[0-5][0-9]){2}$
        //}

    }
}
