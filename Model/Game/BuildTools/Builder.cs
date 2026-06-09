using Graphics2026.Controller;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.BuildTools
{
    internal class Builder : ActiveRoleBehaviour
    {
        public List<BuildTool> buildTools = new();
        private int toolIndex = -1;

        public static InFrontShader HIGHLIGHT_SHADER = new InFrontShader(new Color4(0, 255, 0, 100));
        public static InFrontShader HIGHLIGHT_ERROR_SHADER = new InFrontShader(new Color4(255, 0, 0, 100));

        public Builder() { 
        
        }

        protected override void Update(float deltaTime)
        {
            for(int i = 0; i < buildTools.Count; i++)
            {
                if (Input.GetNumberKeyDown(i + 1))
                    ActiveTool(i);
            }

            if (Input.GetNumberKeyDown(0))
                DeactiveTools();
        }

        public Builder AddTool(BuildTool tool)
        {
            buildTools.Add(tool);
            return this;
        }

        public void ActiveTool(int index)
        {
            if (index == toolIndex)
            {
                DeactiveTools();
                return;
            }

            if (index >= buildTools.Count || index < 0)
            {
                Console.WriteLine("Tool index outside bounds!");
                return;
            }

            if (toolIndex != -1)
                buildTools[toolIndex].DeactivateTool();

            buildTools[index].ActivateTool();

            toolIndex = index;
        }
        public void DeactiveTools()
        {
            if (toolIndex != -1)
                buildTools[toolIndex].DeactivateTool();

            toolIndex = -1;
        }
    }
}
