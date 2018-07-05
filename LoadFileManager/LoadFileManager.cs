using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LoadFileManager
{
    public partial class LoadFileManager : Form
    {
        Batch activeBatch;
        List<Batch> batches;
        Type definitionListType;
        IDefinitionList activeDefinitionList;
        List<Tuple<IDefinitionList, Type>> definitionLists;

        public LoadFileManager()
        {
            InitializeComponent();

            batches = new List<Batch>();
            definitionLists = new List<Tuple<IDefinitionList, Type>>();
        }

        private void RefreshBatchList()
        {
            batchListBox.Items.Clear();
            batchListBox.Items.AddRange(batches.ToArray());

            while ((batchListBox.SelectedItem as Batch) != activeBatch)
            {
                batchListBox.SelectedIndex++;
            }
        }

        private async void NewBatch(BatchType type)
        {
            activeBatch = new Batch() { Type = type };
            batches.Add(activeBatch);
            RefreshBatchList();
        }

        private async void OpenBatchFile(string fileName, BatchType type)
        {

        }

        private async void SaveBatchFile(string fileName, BatchType type)
        {

        }

        private async void NewDefinitionFile(string fileName, BatchType type)
        {

        }

        public async void OpenDefinitionFile(string fileName, BatchType type)
        {

        }

        public async void SaveDefinitionFile(string fileName, BatchType type)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
