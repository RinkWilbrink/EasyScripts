/* Easy Scripts | Template to Script Tool. By Rink Wilbrink
 * EULA

 * In any case of the use of this Tool or it's contents, you agree to the terms and conditions mentioned below.
 * In any case of breach of these terms and conditions or the below mentioned clauses, 
 * appropriate action may be taken against the user (Read more information in the Breach of Terms and Conditions and Mentioned Clauses section).

 * ------------------------------------------------------------------------------------------------------------------------

 * Use of this Tool

 * Rink Wilbrink grants you a revocable, non-exclusive, non-transferable, limited license to download, 
 * install and use the Tool for your personal, commercial and non-commercial purposes in accordance
 * with the terms of this Agreement.

 * Use of any code from this Tool for any type of commercial use is strictly prohibited unless a discrete notice is given.

 * ------------------------------------------------------------------------------------------------------------------------

 * You agree not to, and you will not permit others to:

 * a) license, sell, rent, lease, assign, distribute, transmit, host, outsource, disclose or otherwise 
 *    commercially exploit the Tool or make the Tool available to any third party.

 * b) modify, copy this Tool or any of it's content, inlcuding the code included with the tool,
 *    and sell, redistribute or otherwise commercially exploit to any third party.

 * ------------------------------------------------------------------------------------------------------------------------

 * Modifications to this Tool or Code.

 * Rink Wilbrink reserves the right to modify, suspend or discontinue, temporarily or permanently, 
 * the Tool or any service to which it connects, with or without notice and without liability to you.

 * ------------------------------------------------------------------------------------------------------------------------

 * Severability

 * If any provision of this Agreement is held to be unenforceable or invalid, such provision will be
 * changed and interpreted to accomplish the objectives of such provision to the greatest extent possible 
 * under applicable law and the remaining provisions will continue in full force and effect.

 * ------------------------------------------------------------------------------------------------------------------------

 * Breach of Terms and Conditions and Mentioned Clauses.
 * 
 * In any case of breach of the above mentioned terms,
 * Appropriate action may be taken including but not limited to,
 * Legal Action.

 * ------------------------------------------------------------------------------------------------------------------------
*/
namespace EasyScripts
{
    using UnityEngine;
    using UnityEditor;
    // System namespaces
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    [InitializeOnLoad, ExecuteInEditMode]
    public class ScriptMaker : EditorWindow
    {
        // Other Paths
        private string project_Path;
        private string settings_Path;

        // Scripts
        private string script_ClassName;
        private string scripts_Path;
        private string scripts_DisplayPath;

        // templates variables
        /// <summary>The Path of the script that the user wants to make a template from.</summary>
        private string template_SelectedTemplatePath;
        private string templates_Path;
        private string[] template_Selected;
        private bool template_ShowScriptToTemplateAfterCreation;
        private int template_NumberSelected;
        private List<System.IO.FileInfo> template_ScriptTemplateList;

        // Component
        private bool component_HasAdded = false;
        private GameObject component_ObjectSelected;

        // EasyScripts Reference
        private EasyScripts easyScripts_Instance;

        // Sub Directories
        private List<string> subDirectory_DirectoriesList = new List<string>();
        private bool subDirectory_DropDownMenu = false;
        // New folder name
        private bool subDirectory_IsTypingNewFolderName = false;
        private string subDirectory_newFolderName;
        private int subDirectory_NewButtonHeight;
        private int subDirectory_ConfirmDeletionButtonIndex;
        private bool subDirectory_hasSubDirectoryFiles = false;

        // Space
        private int space_WindowMiddle;
        private int _newScriptHeight;

        #region pixelSpaces between GUI Items

        /// <summary>The distance where the GUI item starts from the left</summary>
        private const byte gui_Start = 4;
        private const byte gui_ItemHeight = 18;
        private const byte gui_subDirectoryStart = gui_Start + 12;

        #region GUI Item distances
        // Script Creation Label / Settings Window Button
        private const byte space_Def = 4;

        // The distance that will be added when the SubDirectory is opened
        private int gui_SubDirectoryDistanceAdd;

        // Project Path Label
        private int space_projectPath;
        // Select Script Folder Button
        private int space_SelectScriptFolder;
        // Sub Directory Drop Down menu
        private int space_SubDirectoryDropDown;

        // ScriptPath Label
        private int space_ScriptPath;
        // Class name Text field
        private int space_ClassName;
        // Select template EnumDropDown menu
        private int space_SelectTemplate;
        // Creating scripts from template buttons
        private int space_CreateScriptButtons;

        // Make new Template Label
        private int space_NewTemplateLabel;
        // Create new Template Button
        private int space_NewTemplatButton;
        // Go to template folder Button
        private int space_GoToTemplateFolder;

        // Select Script to make template from Label
        private int space_SelectTemplateLabel;
        // Open new Template after it has been created CheckBox.
        private int space_OpenAfterCreatingCheckBox;
        // Create Template from script Button
        private int space_NewTemplateFromScriptButton;

        #endregion

        private void setGuiItemDistances()
        {
            // Project Path Label
            space_projectPath = space_Def + gui_ItemHeight + 4;
            // Select Script Folder Button
            space_SelectScriptFolder = (space_projectPath + gui_ItemHeight);
            // Sub Directory Drop Down menu
            space_SubDirectoryDropDown = (space_SelectScriptFolder + gui_ItemHeight);

            // Creating Scripts
            space_ScriptPath = (space_SubDirectoryDropDown + gui_ItemHeight + easyScripts_Instance.space_SubDirectoryDropDown + _newScriptHeight);
            // Class name Text field
            space_ClassName = (space_ScriptPath + gui_ItemHeight + easyScripts_Instance.space_ScriptPath);
            // Select template EnumDropDown menu
            space_SelectTemplate = (space_ClassName + gui_ItemHeight + easyScripts_Instance.space_ClassName);
            // Creating scripts from template buttons
            space_CreateScriptButtons = (space_SelectTemplate + gui_ItemHeight + easyScripts_Instance.space_SelectTemplate);

            // Make new Template Label
            space_NewTemplateLabel = (space_CreateScriptButtons + gui_ItemHeight + easyScripts_Instance.space_NewTemplateLabel);
            // Create new Template Button
            space_NewTemplatButton = (space_NewTemplateLabel + gui_ItemHeight + easyScripts_Instance.space_NewTemplatButton);
            // Go to template folder Button
            space_GoToTemplateFolder = (space_NewTemplatButton + gui_ItemHeight + easyScripts_Instance.space_GoToTemplateFolder);

            // Select Script to make template from Label
            space_SelectTemplateLabel = (space_GoToTemplateFolder + gui_ItemHeight + easyScripts_Instance.space_SelectTemplateLabel);
            // Open new Template after it has been created CheckBox
            space_OpenAfterCreatingCheckBox = (space_SelectTemplateLabel + gui_ItemHeight + easyScripts_Instance.space_OpenAfterCreatingCheckBox);
            // Create Template from script Button
            space_NewTemplateFromScriptButton = (space_OpenAfterCreatingCheckBox + gui_ItemHeight + easyScripts_Instance.space_NewTemplateFromScriptButton);
        }

        #endregion

        [MenuItem("Assets/Create/New Easy Script", priority = 80)]
        public static void CreateNewScript()
        {
            try
            {
                getEasyScriptsInstance().instance_ScriptMaker.Init();
            }
            catch
            {
                getEasyScriptsWindow();
                getEasyScriptsInstance().instance_ScriptMaker.Init();
            }

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(string.Format("{0}/{1}", getEasyScriptsInstance().instance_ScriptMaker.templates_Path,
                getEasyScriptsInstance().instance_ScriptMaker.template_Selected[getEasyScriptsInstance().instance_ScriptMaker.template_NumberSelected]), "NewScript.cs");
        }

        [MenuItem("Window/Easy Scripts Window")]
        public static void getEasyScriptsWindow()
        {
            getEasyScriptsInstance().instance_ScriptMaker = GetWindow<ScriptMaker>("Easy Scripts");
            // Set the EasyScripts Singleton class
            getEasyScriptsInstance().instance_ScriptMaker.easyScripts_Instance = getEasyScriptsInstance();
        }

        private void Init()
        {
            // Set Easy Scripts Instance if it doesn't exist yet.
            if (easyScripts_Instance == null)
            {
                easyScripts_Instance = getEasyScriptsInstance();
            }

            // Check if the ProjectPath is null, if so then set the project path to its correct path on the drive.
            project_Path = Application.dataPath;

            // Check if the template path exists in the user's documents folder, if not create the directory
            CheckFilePathExistance(string.Format("{0}/EasyScripts Templates", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)));

            // Set TemplatePath to the directory in the documents folder with all the templates
            templates_Path = string.Format("{0}/EasyScripts Templates", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));

            // Check if SettingsPath is null, if so then set it to the correct path.
            if (settings_Path == null)
            {
                settings_Path = string.Format("{0}/EasyScripts/Settings", project_Path);
            }

            if (scripts_Path == null)
            {
                // get the settings in the shit
                try
                {
                    GetSettings();
                }
                catch { }
            }

            // Check if ScriptTemplateList is null, if so then initialize a new List.
            if (template_ScriptTemplateList == null)
            {
                template_ScriptTemplateList = new List<System.IO.FileInfo>();
            }

            // Check for the files in the directory of TemplatePath
            GetFilesInDirectory(templates_Path);

            // Set window width variables
            space_WindowMiddle = (int)((Screen.width - 4) * 0.5f);

            // Add pixel distance when the SubDirectory has been opened
            gui_SubDirectoryDistanceAdd = space_SubDirectoryDropDown;

            // Set the distance between all GUI Items, these are variables so it can be changed from the settings window.
            try
            {
                setGuiItemDistances();
            }
            catch
            {
                getEasyScriptsWindow();
            }
        }

        private void OnGUI()
        {
            if (component_HasAdded == true)
            {
                if (!EditorApplication.isCompiling)
                {
                    if (component_ObjectSelected != null)
                    {
                        // Add the Component to the object
                        component_ObjectSelected.AddComponent(getClassType(script_ClassName));
                    }
                    // Stop the code from running again when not necesairy.
                    component_HasAdded = false;

                    // Cleanup memory
                    component_ObjectSelected = null;
                }
            }

            // Execute all the code that needs to be done before the Easy Scripts can properly function,
            // This is going to repeat getting executed to make sure everything is always up to date.
            Init();

            GUI.Label(new Rect(gui_Start, space_Def, Screen.width, gui_ItemHeight), "Script Creation.", EditorStyles.boldLabel);

            //--------------------------------------------------------------------------------------------------------

            #region Creating Scripts

            // Path where the file will be stored
            GUI.Label(new Rect(gui_Start, space_projectPath, Screen.width, gui_ItemHeight), new GUIContent(string.Format("{0}/", project_Path), "This is the project path up until the Assets folder from the project."));

            // Selection window for where the user wants to store their scripts.
            if (GUI.Button(new Rect(gui_Start, space_SelectScriptFolder, Screen.width - 10, gui_ItemHeight), "Select Script Folder"))
            {
                string temp = EditorUtility.SaveFolderPanel("Save scripts at", project_Path, "");
                if(temp.Length > 0)
                {
                    scripts_Path = temp;
                    SetSettings();
                }
                // Clean up local Variables
                temp = null;
            }

            // Drop Down menu
            subDirectory_DropDownMenu = EditorGUI.Foldout(new Rect(gui_Start, space_SubDirectoryDropDown, Screen.width - 10, gui_ItemHeight), subDirectory_DropDownMenu, "Select Sub Directory.");
            if (subDirectory_DropDownMenu)
            {
                foldoutSubDirectories();
            }
            else
            {
                _newScriptHeight = 0;
            }

            GUI.Label(new Rect(gui_Start, space_ScriptPath, Screen.width - 10, gui_ItemHeight + 10), new GUIContent(string.Format("ScriptPath: /{0}", scripts_DisplayPath), scripts_Path));

            // Set the name of the Class and the Filename. e.g. Player (Player.cs, class Player)
            script_ClassName = EditorGUI.TextField(new Rect(gui_Start, space_ClassName, Screen.width - 10, gui_ItemHeight), new GUIContent("Class/File name", "Put the name of the class and script here."), script_ClassName);

            template_Selected = null;
            template_Selected = template_ScriptTemplateList.Select(I => I.Name).ToArray();
            // Drop down menu.
            template_NumberSelected = EditorGUI.Popup(new Rect(gui_Start, space_SelectTemplate, Screen.width - 10, gui_ItemHeight), "Select Template", template_NumberSelected, template_Selected, EditorStyles.popup);

            // Button that creates the script.
            if (GUI.Button(new Rect(gui_Start, space_CreateScriptButtons, space_WindowMiddle - 2, gui_ItemHeight), new GUIContent("Create Script.", "Create a script from the selected Template with the name in the InputField above.")))
            {
                createNewScript();
            }

            if (GUI.Button(new Rect(space_WindowMiddle + 4, space_CreateScriptButtons, space_WindowMiddle - 3, gui_ItemHeight), new GUIContent("Add to Selected GameObject", "Create a script from the selected Template and add it to the selected GameObject in the Hierarchy.")))
            {
                createNewScript(true);
            }

            #endregion

            //--------------------------------------------------------------------------------------------------------

            #region Creating and Managing Templates

            GUI.Label(new Rect(gui_Start, space_NewTemplateLabel, Screen.width - 10, gui_ItemHeight), "Make new Templates", EditorStyles.boldLabel);

            if (GUI.Button(new Rect(gui_Start, space_NewTemplatButton, Screen.width - 10, gui_ItemHeight), new GUIContent("Create New Template", "Create a new Template in the Template folder.")))
            {
                // Create the new template with a (In most cases) proper name.
                CreateTemplateFile(templates_Path, string.Format("csScriptTemplate_{0}", template_ScriptTemplateList.ToArray().Length + 1));

                // Open the folder with the new template created.
                EditorUtility.RevealInFinder(string.Format("{0}/EasyScripts Templates/csScriptTemplate_{1}.txt",
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), template_ScriptTemplateList.ToArray().Length + 1));
            }

            if (GUI.Button(new Rect(gui_Start, space_GoToTemplateFolder, Screen.width - 10, gui_ItemHeight), new GUIContent("Go To Template Folder", "Open a new File Explorer window to the Template folder")))
            {
                EditorUtility.RevealInFinder(System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.MyDocuments) + "/EasyScripts Templates/" + template_Selected[0]);
            }

            GUI.Label(new Rect(gui_Start, space_SelectTemplateLabel, Screen.width - 10, gui_ItemHeight), "Select script to make a template from.", EditorStyles.boldLabel);

            // Show Template folder or not Toggle.
            template_ShowScriptToTemplateAfterCreation = GUI.Toggle(new Rect(gui_Start, space_OpenAfterCreatingCheckBox, Screen.width - 10, gui_ItemHeight), template_ShowScriptToTemplateAfterCreation, new GUIContent("Open new Template after it has been created.",
                "Check this if you want to open a File Explorer window after creating a new Template from the selected script."));

            // Make a script from the users selected template.
            if (GUI.Button(new Rect(gui_Start, space_NewTemplateFromScriptButton, Screen.width - 10, gui_ItemHeight), new GUIContent("Create Template of selected Script", "Select a Script and make a Template file from it's content. (Could be used to back up a script)")))
            {
                createNewTemplate();
            }

            #endregion

            GUILayout.Space(space_NewTemplateFromScriptButton + 10);
        }

        //--------------------------------------------------------------------------------------------------------

        #region settings

        /// <summary>Get the paths of the variables stored in the settings.txt file.</summary>
        private void GetSettings()
        {
            CheckFilePathExistance(settings_Path);
            string values = Regex.Replace(GetContentFromFileAtPath(string.Format("{0}/settings.txt", settings_Path)), @"\t|\n|\r", string.Empty);

            string[] ContentStrings = values.Split(';');

            // Cleanup values
            values = null;

            for (int i = 0; i < ContentStrings.Length; i++)
            {
                string[] CurrentValue = ContentStrings[i].Split('=');

                if (CurrentValue[0] == "ScriptPath")
                {
                    scripts_Path = CurrentValue[1];
                    scripts_DisplayPath = scripts_Path.Replace(project_Path, "Assets");
                }

                // set currentvalue to null (Clean up)
                CurrentValue = null;
            }

            // Set Contentstrings to null (Clean up)
            ContentStrings = null;
        }

        /// <summary>Set the paths of the values stored in the settings.txt file.</summary>
        private void SetSettings()
        {
            string settingsText = string.Format("ScriptPath={0}", scripts_Path);

            // Create or overwrite the settings file in the template folder.
            CreateAndWriteFile(string.Format("{0}/settings.txt", settings_Path), settingsText);

            scripts_DisplayPath = scripts_Path.Replace(project_Path, "Assets");

            // Cleanup local variable
            settingsText = null;
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------

        #region get data/files

        /// <summary>Get and return the text from the desired file</summary>
        /// <param name="FilePath">The path to the desired file.</param>
        /// <returns>The text from the desired file.</returns>
        private string GetContentFromFileAtPath(string FilePath)
        {
            return System.IO.File.ReadAllText(FilePath).TrimEnd('\n', '\t', '\r', ' ');
        }

        /// <summary>Take the text from the user defined template and replace the #SCRIPTNAME# with the class name the user has given.</summary>
        /// <param name="FilePath">The path where the script should be stored.</param>
        /// <param name="DesiredClassName">The class name the user has given.</param>
        /// <returns>The correct version of the template with the desired class name</returns>
        private string GetScriptTemplateSetClassName(string FilePath, string DesiredClassName)
        {
            return System.IO.File.ReadAllText(FilePath).Replace("#SCRIPTNAME#", DesiredClassName).TrimEnd();
        }

        /// <summary>Check all the files in the Template path, if there are no templates to choose from then make a default template.</summary>
        /// <param name="Path">File Path for the template folder</param>
        private void GetFilesInDirectory(string Path)
        {
            // Clear Script Template List, if this can't occur then nothing will happen.
            try
            {
                // Clear the List so there wont be duplicate templates in the appearing in the selection drop down menu.
                template_ScriptTemplateList.Clear();
            }
            catch { }

            // Go though all the *.txt files in the desired directory and add them to the List<FileInfo>.
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Path);
            System.IO.FileInfo[] ScriptTemplates = dir.GetFiles("*.txt");
            // Go through the array of templates and put the in the list.
            if (ScriptTemplates.Length > 0)
            {
                // Go through the Array and add the templates to the Global List in the correct order.
                for (int i = 0; i < ScriptTemplates.Length; i++)
                {
                    try
                    {
                        template_ScriptTemplateList.Add(ScriptTemplates[i]);
                    }
                    catch { }
                }
            }
            else
            {
                // Create a default template if no compatible templates exist.
                CreateTemplateFile(templates_Path, "csScriptTemplate_1");
            }

            // Cleanup local variables
            dir = null;
            ScriptTemplates = null;
        }

        /// <summary>Returns a Class Type based on a name given as a string parameter</summary>
        /// <param name="typeName">Name of the Class you want to return</param>
        /// <returns>Class Type by name (given by a string parameter</returns>
        public static System.Type getClassType(string typeName)
        {
            var type = System.Type.GetType(typeName);
            if (type != null) return type;
            // Get Class Type by string name
            foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }

        // get Subdirectories in given folder
        private void getSubDirectories(string FilePath)
        {
            string[] subDirectorieArray = System.IO.Directory.GetDirectories(FilePath);
            subDirectory_DirectoriesList.Clear();
            for (int i = 0; i < subDirectorieArray.Length; i++)
            {
                subDirectorieArray[i] = subDirectorieArray[i].Replace(scripts_Path, "");
                subDirectorieArray[i] = subDirectorieArray[i].Replace(@"\", "/");
                subDirectory_DirectoriesList.Add(subDirectorieArray[i]);
            }
        }


        /// <summary>Check if the Directory exist for the templates. If not then make the folder directory.</summary>
        /// <param name="FilePath">The Path of the file directory where all the templates will be stored</param>
        private void CheckFilePathExistance(string FilePath)
        {
            // Check if the folder exists.
            if (!System.IO.Directory.Exists(FilePath))
            {
                System.IO.Directory.CreateDirectory(FilePath); // create the folder.

                // Clean up local variable
                FilePath = null;
            }
        }

        private bool CheckIfFilePathExists(string FilePath)
        {
            if (System.IO.Directory.Exists(FilePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------

        #region set data/files

        /// <summary>Create a file and write all the text that needs to be in the file</summary>
        /// <param name="NewFilePath">The path of the file the user wants to write to</param>
        /// <param name="WriteText">The text the user wants inside of the file</param>
        private void CreateAndWriteFile(string NewFilePath, string WriteText)
        {
            using (var writer = new System.IO.StreamWriter(NewFilePath))
            {
                // Write the text given as parameter and put that text in to the newly created file.
                writer.WriteLine(WriteText.TrimEnd('\n', '\t', '\r', ' '));
            }

            // Cleanup local variables
            NewFilePath = null;
            WriteText = null;
        }

        /// <summary>Create a new template at the desired path.</summary>
        /// <param name="newFilePath">The path where the new file should be stored.</param>
        /// <param name="FileName">The name the new template should have (.txt is not needed).</param>
        private void CreateTemplateFile(string newFilePath, string FileName)
        {
            CreateAndWriteFile(string.Format("{0}/{1}.txt", newFilePath, FileName), defaultCsTemplate());

            // Cleanup local variables
            newFilePath = null;
            FileName = null;
        }

        /// <summary>Remove the folder at the path given</summary>
        private void removeDirectory(string FilePath, bool recursive = false)
        {
            System.IO.Directory.Delete(FilePath, recursive);
        }

        /// <summary>Return a default C# script layout for templates</summary>
        /// <returns>A default C# script layout</returns>
        private string defaultCsTemplate()
        {
            return "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\n\npublic class #SCRIPTNAME# : MonoBehaviour\n{    \n    void Start()\n    {\n        \n    }\n    \n    void Update()\n    {\n        \n    }\n}".TrimEnd();
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------

        #region GUI Functions

        private void foldoutSubDirectories()
        {
            subDirectory_NewButtonHeight = 0;
            subDirectory_NewButtonHeight += 20;

            if (!string.IsNullOrEmpty(scripts_Path) && !string.IsNullOrWhiteSpace(scripts_Path) && CheckIfFilePathExists(scripts_Path))
            {
                SubDirectoryButtons();
            }
            else
            {
                GUI.Label(new Rect(gui_subDirectoryStart + 5, space_SubDirectoryDropDown + 18, Screen.width - 162, gui_ItemHeight), new GUIContent("Script Path is Empty or Invalid!",
                    "Select a script path first by clicking on the 'Select Script Folder' button and then select a path where you want your scripts to get created."));

                // Button to reset the script path to the project path
                if (GUI.Button(new Rect(Screen.width - 140, space_SubDirectoryDropDown + 18, 130, gui_ItemHeight),
                    new GUIContent("Reset Script Path", "Reset the Script Path to the Project Path")))
                {
                    scripts_Path = project_Path;
                    SetSettings();
                }

                subDirectory_NewButtonHeight += 20;
            }

            _newScriptHeight = (subDirectory_NewButtonHeight - 14);
        }

        /// <summary>This Functions handles all the Sub directory buttons and selection.</summary>
        private void SubDirectoryButtons()
        {
            #region GoBackOneSubDirectory

            // Go Back 1 Subdirectory Button
            if (GUI.Button(new Rect(gui_subDirectoryStart, space_SubDirectoryDropDown + subDirectory_NewButtonHeight, Screen.width - 32, gui_ItemHeight),
                new GUIContent("../", "Go back One(1) Directory Folder.\n(Limited to stay withing the Project Assets folder).")))
            {
                try
                {
                    string[] newScriptPath = scripts_Path.Split('/');
                    // Check if you are not in the 'ProjectPath/Assets' folder, if so, the user can go back but if he is in the Assets folder the button doesn't do anything.
                    if (string.Format("{0}/Assets", newScriptPath[newScriptPath.Length - 2]) != string.Format("{0}/Assets", project_Path.Split('/')[project_Path.Split('/').Length - 2]))
                    {
                        // Set new Script Path
                        scripts_Path = scripts_Path.Replace("/" + newScriptPath[newScriptPath.Length - 1], "");
                    }

                    // Check if the Script path is shorter then the project path, if so, set it to Project path
                    if (scripts_Path.Length <= project_Path.Length)
                    {
                        scripts_Path = project_Path;
                    }

                    // Remove local variable for better RAM usage and optimisation
                    newScriptPath = null;

                    // Update the Script path in the settings file
                    SetSettings();

                    subDirectory_IsTypingNewFolderName = false;
                }
                catch { }
            }

            subDirectory_NewButtonHeight += 3;

            #endregion

            // ----------------------------------------------------------------------------------------------\\

            #region SubDirectoryFolderButtons

            // Sub Directory Buttons
            getSubDirectories(scripts_Path);
            for (int i = 0; i < subDirectory_DirectoriesList.Count; i++)
            {
                subDirectory_NewButtonHeight += gui_ItemHeight + 2;

                if (subDirectory_hasSubDirectoryFiles == false || subDirectory_ConfirmDeletionButtonIndex != i)
                {
                    // Sub Directory folder button
                    if (GUI.Button(new Rect(gui_subDirectoryStart + 5, space_SubDirectoryDropDown + subDirectory_NewButtonHeight, Screen.width - 56, gui_ItemHeight),
                    new GUIContent(subDirectory_DirectoriesList.ToArray()[i], subDirectory_DirectoriesList.ToArray()[i])))
                    {
                        scripts_Path += subDirectory_DirectoriesList[i];
                        subDirectory_IsTypingNewFolderName = false;

                        // Update the Script path in the settings file
                        SetSettings();
                    }

                    // Remove the folder
                    if (GUI.Button(new Rect(Screen.width - 34, space_SubDirectoryDropDown + subDirectory_NewButtonHeight, gui_ItemHeight, gui_ItemHeight), new GUIContent("X", "Delete this folder")))
                    {
                        try
                        {
                            removeDirectory(string.Format("{0}{1}", scripts_Path, subDirectory_DirectoriesList.ToArray()[i]));

                            // Refresh the asset data base to update the Project tab in the Unity editor.
                            subDirectory_newFolderName = "";
                            AssetDatabase.Refresh();
                        }
                        catch
                        {
                            subDirectory_hasSubDirectoryFiles = true;
                            subDirectory_ConfirmDeletionButtonIndex = i;
                        }
                    }
                }
                if (subDirectory_hasSubDirectoryFiles == true && subDirectory_ConfirmDeletionButtonIndex == i)
                {
                    // Button that creates the script.
                    if (GUI.Button(new Rect(gui_subDirectoryStart + 5, space_SubDirectoryDropDown + subDirectory_NewButtonHeight, space_WindowMiddle - (gui_subDirectoryStart + 5), gui_ItemHeight),
                    new GUIContent("Confirm Permanent Deletion", "Delete this folder and all the files and sub folders it contains")))
                    {

                        removeDirectory(string.Format("{0}{1}", scripts_Path, subDirectory_DirectoriesList.ToArray()[i]), true);
                        subDirectory_hasSubDirectoryFiles = false;
                        AssetDatabase.Refresh();
                    }

                    if (GUI.Button(new Rect(space_WindowMiddle + 4, space_SubDirectoryDropDown + subDirectory_NewButtonHeight, space_WindowMiddle - gui_subDirectoryStart, gui_ItemHeight),
                        new GUIContent("Deny Permanent Deletion", "Don't delete this folder and it's contents")))
                    {

                        subDirectory_hasSubDirectoryFiles = false;
                    }
                }
            }

            subDirectory_NewButtonHeight += gui_ItemHeight + 5;

            #endregion

            // ----------------------------------------------------------------------------------------------\\

            #region SubDirectoryCreateNewFolderButtons

            // New Folder button
            if (!subDirectory_IsTypingNewFolderName)
            {
                if (GUI.Button(new Rect(gui_subDirectoryStart, space_SubDirectoryDropDown + subDirectory_NewButtonHeight, Screen.width - 32, gui_ItemHeight), "New Folder"))
                {

                    subDirectory_IsTypingNewFolderName = true;
                }
            }

            if (subDirectory_IsTypingNewFolderName)
            {
                subDirectory_newFolderName = EditorGUI.TextField(new Rect(gui_subDirectoryStart, space_SubDirectoryDropDown + subDirectory_NewButtonHeight, Screen.width - 32, gui_ItemHeight), "New Folder Name", subDirectory_newFolderName);

                subDirectory_NewButtonHeight += gui_ItemHeight + 6;

                if (GUI.Button(new Rect(gui_subDirectoryStart, space_SubDirectoryDropDown + subDirectory_NewButtonHeight - 4, Screen.width - 50, gui_ItemHeight), "Create New Folder"))
                {
                    if (!string.IsNullOrEmpty(subDirectory_newFolderName) && !string.IsNullOrWhiteSpace(subDirectory_newFolderName))
                    {
                        CheckFilePathExistance(string.Format("{0}/{1}", scripts_Path, subDirectory_newFolderName));

                        // Reset newFolderName to being empty
                        subDirectory_newFolderName = "";

                        AssetDatabase.Refresh();

                        subDirectory_IsTypingNewFolderName = false;
                    }
                }
                // Exit button, Close the 'Create new folder' button and input field.
                if (GUI.Button(new Rect(Screen.width - 34, space_SubDirectoryDropDown + subDirectory_NewButtonHeight - 4, gui_ItemHeight, gui_ItemHeight), "X"))
                {
                    subDirectory_newFolderName = "";

                    subDirectory_IsTypingNewFolderName = false;
                }
            }
            subDirectory_NewButtonHeight += gui_ItemHeight + 2;

            #endregion
        }

        /// <summary>Create a new Script from a Template.</summary>
        /// <param name="addScriptToSelectedComponent">set to true if you want to add the new script as a component to the selected object</param>
        private void createNewScript(bool addScriptToSelectedComponent = false)
        {
            if (template_Selected != null)
            {
                if (!string.IsNullOrEmpty(scripts_Path) && !string.IsNullOrWhiteSpace(scripts_Path))
                {
                    if (!string.IsNullOrEmpty(script_ClassName) && !string.IsNullOrWhiteSpace(script_ClassName) && script_ClassName.Contains(" ") == false)
                    {
                        // Creating the .cs file.
                        string FullFilePathAndName = string.Format("{0}/{1}.cs", scripts_Path, script_ClassName);
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(FullFilePathAndName))
                        {
                            // Writing the text from the user's selected template into the new script.
                            writer.WriteLine(GetScriptTemplateSetClassName(string.Format("{0}/{1}", templates_Path,
                                template_Selected[template_NumberSelected]), script_ClassName));
                        }
                        // Refresh the Project assets so the newly created scripts will instantly pop up when created.
                        AssetDatabase.Refresh();

                        if (addScriptToSelectedComponent)
                        {
                            // Set selected Object
                            component_ObjectSelected = Selection.activeGameObject;
                            component_HasAdded = true;
                        }
                        // Clean up FullFilePathAndName
                        FullFilePathAndName = null;
                    }
                    else { Debug.LogErrorFormat("Error with ClassName: {0}", script_ClassName); }
                }
                else { Debug.LogErrorFormat("Error with Script Path: {0}", scripts_Path); }
            }
            else
            {
                GetFilesInDirectory(templates_Path);
                Debug.LogFormat("Completed Looking for Templates. Found {0}", template_ScriptTemplateList.ToArray().Length);
            }
        }

        /// <summary>Create a new Template from a script that the user selects.</summary>
        private void createNewTemplate()
        {
            template_SelectedTemplatePath = EditorUtility.OpenFilePanel("Script to Template", project_Path, "cs");

            // Create the script and open if the user wants to
            if (template_SelectedTemplatePath != null)
            {
                // Make an array of strings to seperate the path and get the script name.
                string[] NewFileName = template_SelectedTemplatePath.Split('/');

                string newTemplateName = NewFileName[NewFileName.Length - 1];

                // Create a new template from the users selected script
                CreateAndWriteFile(string.Format("{0}/{1}.txt", templates_Path, newTemplateName),
                    GetContentFromFileAtPath(template_SelectedTemplatePath));

                if (template_ShowScriptToTemplateAfterCreation)
                {
                    // Open the file explorer with the newly created template.
                    EditorUtility.RevealInFinder(string.Format("{0}/{1}.txt", templates_Path, newTemplateName));
                }

                // Cleanup local variables (Less Garbage Collection lag)
                NewFileName = null;
                newTemplateName = null;
            }
            else
            {
                Debug.LogError("SelectedTemplatePath is empty!");
            }
        }

        private static EasyScripts getEasyScriptsInstance()
        {
            return EasyScripts.Instance;
        }

        #endregion
    }
}