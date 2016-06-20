using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace NetCopy {
    class ClipboardData {
        private Stream audioStream = null;
        private StringCollection stringCollection = null;
        private Image imageData = null;
        private String textData = null;

        public void BackupData() {
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
            if (audioStream != null)
                Clipboard.SetAudio(audioStream);

            if (stringCollection != null)
                Clipboard.SetFileDropList(stringCollection);

            if (imageData != null)
                Clipboard.SetImage(imageData);

            if (textData != null)
                Clipboard.SetText(textData);
        }
    }
}
