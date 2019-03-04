using Quokka.CS2C.CodeModels.C;
using Quokka.CS2C.Translator.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace Quokka.CS2C.Translator.Visitors
{
    public class TranslationContext
    {
        public ComponentsLibrary Library;
        public List<CModel> Models = new List<CModel>();

        public Stack<List<string>> UsingsStack = new Stack<List<string>>();
        public List<string> Usings => UsingsStack.Peek();

        public Stack<string> Namespaces = new Stack<string>();
        public Stack<IMembersContainerCModel> MembersContainers = new Stack<IMembersContainerCModel>();
        public IMembersContainerCModel MembersContainer => MembersContainers.Peek();

        public IDisposable WithUsings()
        {
            UsingsStack.Push(new List<string>());

            return Disposable.Create(() => UsingsStack.Pop());
        }

        public IDisposable WithCodeContainer<T>(T container)
            where T : CModel, IMembersContainerCModel
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
