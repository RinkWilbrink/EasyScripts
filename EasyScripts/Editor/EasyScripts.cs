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

	public enum CreateScriptButtonState
	{
		Both = 0,
		createScript = 1,
		createScriptAddToGO = 2
	}

	// The Default Easy Scripts window
	[InitializeOnLoad, ExecuteInEditMode]
	public sealed class EasyScripts
	{
		private static readonly EasyScripts instance = new EasyScripts();
		private EasyScripts() { }

		#region EasyScripts window Distances

		// Project Path Label
		public int space_projectPath = 4;
		// Select Script Folder Button
		public int space_SelectScriptFolder = 0;
		// Sub Directory Drop Down menu
		public int space_SubDirectoryDropDown = 0;

		// ScriptPath Label
		public int space_ScriptPath = 16;
		// Class name Text field
		public int space_ClassName = 6;
		// Select template EnumDropDown menu
		public int space_SelectTemplate = 4;
		// Creating scripts from template buttons
		public int space_CreateScriptButtons = 4;

		// Make new Template Label
		public int space_NewTemplateLabel = 14;
		// Create new Template Button
		public int space_NewTemplatButton = 2;
		// Go to template folder Button
		public int space_GoToTemplateFolder = 4;

		// Select Script to make template from Label
		public int space_SelectTemplateLabel = 14;
		// Open new Template after it has been created CheckBox.
		public int space_OpenAfterCreatingCheckBox = -2;
		// Create Template from script Button
		public int space_NewTemplateFromScriptButton = 5;

		#endregion

		public static EasyScripts Instance { get { return instance; } }

		// Variables
		public ScriptMaker instance_ScriptMaker;
	}
}