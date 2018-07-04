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

namespace NinjaGame.UI.Loading
{
    class ControlLoader : IControlLoader
    {
        protected IGraphics2DManager _graphicsManager;

        public ControlLoader(IGraphics2DManager graphicsManager)
        {
            _graphicsManager = graphicsManager;
        }

        public IControl LoadControl(string filePath, string id)
        {
            var stagedControl = StageControl(filePath, id);

            if (stagedControl.FilePath == string.Empty || stagedControl.Id == string.Empty || stagedControl.Type == ControlType.None)
                return null;

            var control = LoadControl(stagedControl);
            return control;
        }

        public IControl LoadControl(StagedControl stagedControl)
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
                                .Where(l => l.ToLower().StartsWith("controlbatch") && l.Contains($"id>{id}"))
                                .FirstOrDefault();

            var work = definition.Split(';');

            var fileIdDict = new Dictionary<string, List<string>>();
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                if (pair.Length > 1 && pair[0].Trim().Length > 0)
                {
                    var ids = pair[1].Trim()
                                .Trim('{','}')
                                .Split(',')
                                .Select(l => l.Trim())
                                .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }
            }

            var batch = new LoadBatch<IControl>(id) { FileIdDict = fileIdDict };
            return batch;
        }

        public StagedControl StageControl(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("control") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            if (definition.Length == 0)
                return new StagedControl();

            var work = definition.Split(';');
            
            ControlType type = ControlType.None;
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                if (pair[0].Trim().ToLower() == "control")
                {
                    type = ParseType(pair[1].Trim().ToLower());
                    break;
                }
            }

            if (type == ControlType.None)
                return new StagedControl();

            var stagedGraphic = new StagedControl(id, filePath, type);
            return stagedGraphic;
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
