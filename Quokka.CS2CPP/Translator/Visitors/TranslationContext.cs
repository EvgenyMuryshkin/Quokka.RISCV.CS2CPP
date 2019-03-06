using Microsoft.CodeAnalysis;
using Quokka.CS2CPP.CodeModels.CPP;
using Quokka.CS2CPP.Translator.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace Quokka.CS2CPP.Translator.Visitors
{
    public class TranslationContext
    {
        public ComponentsLibrary Library;
        public SyntaxTree Root;
        public SemanticModel SemanticModel => Library.SemanticModels[Root];

        public List<CPPModel> Models = new List<CPPModel>();

        public Stack<List<string>> UsingsStack = new Stack<List<string>>();
        public List<string> Usings => UsingsStack.Peek();

        public Stack<string> Namespaces = new Stack<string>();
        public Stack<IMembersContainerCPPModel> MembersContainers = new Stack<IMembersContainerCPPModel>();
        public IMembersContainerCPPModel MembersContainer => MembersContainers.Peek();

        public IDisposable WithUsings()
        {
            UsingsStack.Push(new List<string>());

            return Disposable.Create(() => UsingsStack.Pop());
        }

        public IDisposable WithCodeContainer<T>(T container)
            where T : CPPModel, IMembersContainerCPPModel
        {
            // check for root model
            if (!MembersContainers.Any())
                Models.Add(container);
            else
                MembersContainer.Members.Add(container);

            MembersContainers.Push(container);

            return Disposable.Create(() => MembersContainers.Pop());
        }
    }
}
