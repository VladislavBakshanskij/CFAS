using System;
using System.Collections.Generic;

namespace testGUI.FileWork {
    public abstract class FileWorker {
        public List<string> CellSourceName = new List<string>() {

        };

        protected string text;
        protected string filePath;
        protected List<string> dataForFile = new List<string>();

        public string Text {
            get => text;
            set {
                if (value is string) {
                    text = value;
                    dataForFile.Add(text);
                }
            }
        }

        public string FilePath {
            get => filePath;
            set {
                if (value is string) {
                    filePath = value;
                }
            }
        }

        protected FileWorker() {
            text = "";
        }
    }
}
