using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Xml;
using System.Windows.Threading;
using Microsoft.Win32;
using LoggingManager;
using Compiler;

namespace TabbedInterface {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			// Load our custom highlighting definition
			IHighlightingDefinition customHighlighting;
			using (Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("TabbedInterface.CustomHighlighting.xshd")) {
				if (s == null)
					throw new InvalidOperationException("Could not find embedded resource");
				using (XmlReader reader = new XmlTextReader(s)) {
					customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
						HighlightingLoader.Load(reader, HighlightingManager.Instance);
				}
			}
			// and register it in the HighlightingManager
			HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", new string[] { ".cool" }, customHighlighting);

			InitializeComponent();
			propertyGridComboBox.SelectedIndex = 2;

			//textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
			//textEditor.SyntaxHighlighting = customHighlighting;
			// initial highlighting now set by XAML

			//This happens immediately before text is entered
			textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
			//This happens immediately after text is entered
			textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

			DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
			foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
			foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
			foldingUpdateTimer.Start();

		}
		string currentFileName;

		void openFileClick(object sender, RoutedEventArgs e) {
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			if (dlg.ShowDialog() ?? false) {
				currentFileName = dlg.FileName;
				textEditor.Load(currentFileName);
				textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(System.IO.Path.GetExtension(currentFileName));
			}
		}

		void saveFileClick(object sender, EventArgs e) {
			if (currentFileName == null) {
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.DefaultExt = ".txt";
				if (dlg.ShowDialog() ?? false) {
					currentFileName = dlg.FileName;
				} else {
					return;
				}
			}
			textEditor.Save(currentFileName);
		}

		void propertyGridComboBoxSelectionChanged(object sender, RoutedEventArgs e) {
			if (propertyGrid == null)
				return;
			switch (propertyGridComboBox.SelectedIndex) {
				case 0:
					propertyGrid.SelectedObject = textEditor;
					break;
				case 1:
					propertyGrid.SelectedObject = textEditor.TextArea;
					break;
				case 2:
					propertyGrid.SelectedObject = textEditor.Options;
					break;
			}
		}

		CompletionWindow completionWindow;

		TextualContent textualContent = new TextualContent();
		
		//Executed prior to key press
		void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e) {
			if (e.Text == "\n") {
				textualContent.SetCurrentLine(
					textEditor.Document.GetText(
						textEditor.Document.GetOffset(textEditor.TextArea.Caret.Line, 1), 
						textEditor.Document.Lines[textEditor.TextArea.Caret.Line - 1].Length)
							.ToString().Trim());
			}
			if (e.Text.Length > 0 && completionWindow != null) {
				if (!char.IsLetterOrDigit(e.Text[0])) {
					// Whenever a non-letter is typed while the completion window is open,
					// insert the currently selected element.
					completionWindow.CompletionList.RequestInsertion(e);
				}
			}
			// do not set e.Handled=true - we still want to insert the character that was typed
		}

		//Executed after key press
		void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e) {
			if (e.Text == ".") {
				// open code completion after the user has pressed dot:
				completionWindow = new CompletionWindow(textEditor.TextArea);
				// provide AvalonEdit with the data:
				IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
				data.Add(new MyCompletionData("Item1"));
				data.Add(new MyCompletionData("Item2"));
				data.Add(new MyCompletionData("Item3"));
				data.Add(new MyCompletionData("Another item"));
				completionWindow.Show();
				completionWindow.Closed += delegate {
					completionWindow = null;
				};
			}

			if (e.Text == "\n") {
				int offset = textEditor.Document.GetOffset(textEditor.TextArea.Caret.Line, 1);
				textEditor.Document.Insert(offset, textualContent.GetOutputString());
				textEditor.Document.Text += "\n";
			}
		}

		#region Folding
		FoldingManager foldingManager;
		AbstractFoldingStrategy foldingStrategy;

		void HighlightingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (textEditor.SyntaxHighlighting == null) {
				foldingStrategy = null;
			} else {
				switch (textEditor.SyntaxHighlighting.Name) {
					case "XML":
						foldingStrategy = new XmlFoldingStrategy();
						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						break;
					case "C#":
					case "C++":
					case "PHP":
					case "Java":
						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(textEditor.Options);
						foldingStrategy = new BraceFoldingStrategy();
						break;
					default:
						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						foldingStrategy = null;
						break;
				}
			}
			if (foldingStrategy != null) {
				if (foldingManager == null)
					foldingManager = FoldingManager.Install(textEditor.TextArea);
				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
			} else {
				if (foldingManager != null) {
					FoldingManager.Uninstall(foldingManager);
					foldingManager = null;
				}
			}
		}

		void foldingUpdateTimer_Tick(object sender, EventArgs e) {
			if (foldingStrategy != null) {
				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
			}
		}
		#endregion

	}
}
