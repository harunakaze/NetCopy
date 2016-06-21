using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NetCopy {
    class ClipboardData {
        private const string EOF_KEY = "<!@#!@#)?>";
        private string currentClipboardText;

        private bool needToBackup = true;
        private Stream audioStream = null;
        private StringCollection stringCollection = null;
        private Image imageData = null;
        private String textData = null;
        
        //TODO: Backup other clipboard data

        public void BackupData() {
            if (!needToBackup)
                return;

            if (Clipboard.ContainsAudio())
                audioStream = Clipboard.GetAudioStream();

            if (Clipboard.ContainsFileDropList())
                stringCollection = Clipboard.GetFileDropList();

            if (Clipboard.ContainsImage())
                imageData = Clipboard.GetImage();

            if (Clipboard.ContainsText())
                textData = Clipboard.GetText();

        }

        public void RestoreData() {
            if (!needToBackup)
                return;

            if (audioStream != null)
                Clipboard.SetAudio(audioStream);

            if (stringCollection != null)
                Clipboard.SetFileDropList(stringCollection);

            if (imageData != null)
                Clipboard.SetImage(imageData);

            if (textData != null)
                Clipboard.SetText(textData);

            needToBackup = false;
        }

        public void SetClipboardText() {
            currentClipboardText = Clipboard.GetText();
            needToBackup = true;
        }

        public string GetSendedText() {
            return currentClipboardText + EOF_KEY;
        }

        public static bool IsValidString(string data) {
            return data.IndexOf(EOF_KEY) > -1;
        }

        private string GetClearData(string data) {
            int eofIndex = data.IndexOf(EOF_KEY);
            string clearText = data.Substring(0, eofIndex);

            return clearText;
        }
    }
}
