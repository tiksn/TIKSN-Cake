using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TIKSN.Cake.Core.Services
{
    public class LocalizationKeysGenerator : ILocalizationKeysGenerator
    {
        public void GenerateLocalizationKeys(string @namespace, string @class, string outputFolder, string[] resxFiles)
        {
            var dataElements = resxFiles
                .Select(XDocument.Load)
                .SelectMany(doc => doc.Element("root").Elements("data"));

            var workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
            var keys = dataElements.Select(dataElement => dataElement.Attribute("name").Value).Select(int.Parse).ToArray();

            var keyFields = keys.Select(key => generator.FieldDeclaration($"Key{key}",
                generator.TypeExpression(SpecialType.System_Int32),
                accessibility: Accessibility.Public,
                modifiers: DeclarationModifiers.Const))
                .ToArray();

            var classDefinition = generator.ClassDeclaration(
                @class,
                accessibility: Accessibility.Public,
                modifiers: DeclarationModifiers.Static,
                members: keyFields);

            var namespaceDeclaration = generator.NamespaceDeclaration(@namespace, classDefinition);

            var newNode = generator.CompilationUnit(namespaceDeclaration).NormalizeWhitespace();

            var codeFilePath = Path.Combine(outputFolder, $"{@class}.cs");
            File.WriteAllText(codeFilePath, newNode.GetText().ToString());
        }
    }
}