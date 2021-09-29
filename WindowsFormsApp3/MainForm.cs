using System;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp3
{
    public partial class XmlTreeForm : Form
    {
        XmlDocument doc = new XmlDocument();

        public XmlTreeForm()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (FileUrlTextBox.Text.Length == 0)
            {
                MessageBox.Show("Вы не ввели URL загружаемого файла.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning) ;
                return;
            }
            String URLString = FileUrlTextBox.Text;

            try
            {
               var root = ReadXmlFile(URLString);
               PrintElement(root);
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить данный файл", "Ошибка",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private XmlElement ReadXmlFile(string filename)
        {            
            var doc = new XmlDocument();  
            
            doc.Load(filename);            
            var root = doc.DocumentElement;
            
            return root;
        }
            
        private void PrintElement(XmlElement element, int nodeLevel = 0, int prevNodeLevel = 0)
        {
            TreeNode treeNode = new TreeNode(element.Name);

            if (nodeLevel > prevNodeLevel)
            {
                foreach (XmlAttribute attr in element.Attributes)
                {                                        
                    treeNode.Text = treeNode.Text + " " + "[" +attr.Name + "]"
                        + " = " + "\"" + attr.InnerText + "\"";
                }

                XmlTreeView.SelectedNode.Nodes.Add(treeNode);
            }

            if (nodeLevel == prevNodeLevel)
            {
                XmlTreeView.SelectedNode = treeNode;

                foreach (XmlAttribute attr in element.Attributes)
                {
                    //treeNode.Name = treeNode.Name + attr.OuterXml;                    
                    treeNode.Text = treeNode.Text + " " + "[" + attr.Name + "]"
                        + " = " + "\"" + attr.InnerText + "\"";
                }
                XmlTreeView.Nodes.Add(treeNode);
            }

            // Если у элемента есть зависимые элементы, то выводим.
            foreach (var child in element.ChildNodes)
            {
                if (child is XmlElement node)
                {
                    XmlTreeView.SelectedNode = treeNode;
                                        
                    PrintElement(node, nodeLevel + 1, nodeLevel);
                }

                if (child is XmlText text)
                {                   
                    treeNode.Text = treeNode.Text + " - " + text.InnerText;
                }
            }
        }       

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            XmlTreeView.Nodes.Clear();
            FileUrlTextBox.Clear();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteButton_Click(sender, e);
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadButton_Click(sender, e);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoForm info = new InfoForm();
            info.Show();
        }
    }
}
