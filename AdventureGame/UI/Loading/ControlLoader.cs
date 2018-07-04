using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NinjaGame.Common;
using NinjaGame.Common.Extensions;
using NinjaGame.Graphics2D.Managers;
using NinjaGame.UI.Controls;
using NinjaGame.Common.Loading;

namespace NinjaGame.UI.Loading
{
    class ControlLoader : IControlLoader
    {
        protected IGraphics2DManager _graphicsManager;
        protected Dictionary<string, string> _nameIdDict;
        protected Dictionary<string, Definition<ControlType>> _definitions;

        public ControlLoader(IGraphics2DManager graphicsManager)
        {
            _graphicsManager = graphicsManager;
            _nameIdDict = new Dictionary<string, string>();
            _definitions = new Dictionary<string, Definition<ControlType>>();
        }

        public IControl LoadControl(string filePath, string id)
        {
            var definition = LoadDefinition(filePath, id);

            if (definition is null)
                return null;

            var control = LoadControl(definition);
            return control;
        }

        public IControl LoadControl(IDefinition<ControlType> stagedControl)
        {
            IControl control = null;
            switch (stagedControl.Type)
            {
                case (ControlType.Button):
                    control = ParseButton(stagedControl.FilePath, stagedControl.Id);
                    break;
            }
            return control;
        }

        public ILoadBatch<IControl> LoadControlBatch(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphicbatch") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            var work = definition.Split(';');

            var name = "";
            var fileIdDict = new Dictionary<string, List<string>>();
            for (int i = 0; i < work.Length; i++)
            {
                if (work[i].Contains('='))
                {
                    var pair = work[i].Split('=');
                    if (pair[0].Trim().ToLower() == "name")
                        name = pair[1].Trim().ToLower();
                }
                else if (work[i].Contains(':'))
                {
                    var pair = work[i].Split(':');
                    var ids = pair[1].Trim()
                                   .Trim('{', '}')
                                   .Split(',')
                                   .Select(l => l.Trim())
                                   .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }                
            }

            var batch = new LoadBatch<IControl>(id, name);
            batch.FileIdDict = fileIdDict;
            return batch;
        }

        public IDefinition<ControlType> LoadDefinition(string filePath, string id)
        {
            _definitions.TryGetValue(id, out var def);
            if (!(def is null))
                return def;

            var line = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphic") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            if (line.Length == 0)
                return null;

            var work = line.Split(';');

            var name = "";
            ControlType type = ControlType.None;
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("control"):
                        type = ParseType(pair[1].Trim().ToLower());
                        break;
                    case ("id"):
                        name = pair[1].Trim().ToLower();
                        break;
                }
            }

            if (type == ControlType.None)
                return null;

            var definition = new Definition<ControlType>(id, name, filePath, type);

            if (!(definition is null) && !_definitions.ContainsKey(id))
                _definitions.Add(id, definition);

            if (!_nameIdDict.ContainsKey(definition.Name))
                _nameIdDict.Add(definition.Name, definition.Id);

            return definition;
        }

        public IControl LoadControlByName(string filePath, string name)
        {
            var definition = LoadDefinitionByName(filePath, name);

            if (definition is null)
                return null;

            var control = LoadControl(definition);
            return control;
        }

        public ILoadBatch<IControl> LoadControlBatchByName(string filePath, string name)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphicbatch") && l.Contains($"name={name}"))
                                .FirstOrDefault();

            var work = definition.Split(';');

            var id = "";
            var fileIdDict = new Dictionary<string, List<string>>();
            for (int i = 0; i < work.Length; i++)
            {
                if (work[i].Contains('='))
                {
                    var pair = work[i].Split('=');
                    if (pair[0].Trim().ToLower() == "id")
                        id = pair[1].Trim();
                }
                else if (work[i].Contains(':'))
                {
                    var pair = work[i].Split(':');
                    var ids = pair[1].Trim()
                                   .Trim('{', '}')
                                   .Split(',')
                                   .Select(l => l.Trim())
                                   .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }                
            }

            var batch = new LoadBatch<IControl>(id, name);
            batch.FileIdDict = fileIdDict;
            return batch;
        }

        public IDefinition<ControlType> LoadDefinitionByName(string filePath, string name)
        {
            _nameIdDict.TryGetValue(name, out var tmpId);
            if (!(tmpId is null))
            {
                _definitions.TryGetValue(tmpId, out var def);
                if (!(def is null))
                    return def;
            }

            var line = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphic") && l.Contains($"name={name}"))
                                .FirstOrDefault();

            if (line.Length == 0)
                return null;

            var work = line.Split(';');

            var id = "";
            ControlType type = ControlType.None;
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("control"):
                        type = ParseType(pair[1].Trim().ToLower());
                        break;
                    case ("id"):
                        id = pair[1].Trim();
                        break;
                }
            }

            if (type == ControlType.None)
                return null;

            var definition = new Definition<ControlType>(id, name, filePath, type);  
            
            if (!(definition is null) && !_definitions.ContainsKey(id))
                _definitions.Add(id, definition);

            if (!_nameIdDict.ContainsKey(definition.Name))
                _nameIdDict.Add(definition.Name, definition.Id);

            return definition;
        }

        private ControlType ParseType(string typeString)
        {
            var type = ControlType.None;
            switch (typeString.Trim().ToLower())
            {
                case ("button"):
                    type = ControlType.Button;
                    break;
            }
            return type;
        }

        private Button ParseButton(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("control") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            var parameters = definition.Split(';')
                                .Where(p => p.Contains("="))
                                .ToList();

            var position = Vector2.Zero;
            var dimensions = Vector2.Zero;
            var centered = false;
            var blurredImageId = "";
            var focusedImageId = "";
            var clickedImageId = "";
            var textId = "";

            foreach (var p in parameters)
            {
                var pair = p.Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("position"):
                        position = pair[1].ToVector2();
                        break;
                    case ("dimensions"):
                        dimensions = pair[1].ToVector2();
                        break;
                    case ("centered"):
                        centered = pair[1].ToBool();
                        break;
                    case ("blurredimage"):
                        blurredImageId = pair[1].Trim();
                        break;
                    case ("focusedimage"):
                        focusedImageId = pair[1].Trim();
                        break;
                    case ("clickedimage"):
                        clickedImageId = pair[1].Trim();
                        break;
                    case ("text"):
                        textId = pair[1].Trim();
                        break;
                }
            }

            var blurredImage = _graphicsManager.GetImage(blurredImageId);
            var focusedImage = _graphicsManager.GetImage(focusedImageId);
            var clickedImage = _graphicsManager.GetImage(clickedImageId);
            var text = _graphicsManager.GetText(textId);

            return new Button(position, dimensions, null, blurredImage, focusedImage, clickedImage, text, centered:centered);
        }
    }
}
