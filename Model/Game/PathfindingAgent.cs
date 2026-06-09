
using Graphics2026.Model.Actors;
using Graphics2026.Model.Game.Shopping;
using Graphics2026.Model.MeshGeneration;
using Graphics2026.Model.SceneManagement;
using Graphics2026.Model.VertexData;
using Graphics2026.View;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game
{
    internal class PathfindingAgent : ActiveRoleBehaviour
    {
        public readonly Actor actor;
        public Vector2i coordinate;
        private NavigationGrid grid;
        private POI? target;
        private float moveSpeed = 5f;

        private bool hasReachedTarget = false;
        private float timeSinceLastMove = 0;
        private Vector2i direction;

        private bool drawWireframeInstead = false;
        private static PhysicalShader? shader;
        private static Mesh<Vertex>? mesh;

        public PathfindingAgent(NavigationGrid grid)
        {
            actor = new Actor();

            if (shader == null)
                shader = new DefaultLit();
            if (mesh == null)
                mesh = MeshImport.OBJ(Program.LOCAL + "Assets/hotdog_character.obj");

            actor.mesh = mesh;

            actor.shader = shader;
            SceneManager.CurrentScene().renderables.Add(actor);
            this.grid = grid;
        }

        public void SetTarget(POI? target) {
            hasReachedTarget = false;
            this.target = target;
        }

        public bool HasReachedTarget() => hasReachedTarget;


        public void SetSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;
        }

        protected override void Update(float deltaTime)
        {
            if (drawWireframeInstead)
            {
                WireRenderer.SetColor(Color4.Pink);
                WireRenderer.DrawSphere(actor.transform.localPosition, 0.25f);
                WireRenderer.DrawCircle(grid.GridSpaceToWorldSpace(coordinate), 0.4f);
                actor.SetRenderStatus(false);
            }
            else
            {
                actor.SetRenderStatus(true);
                if (direction != Vector2i.Zero)
                {
                    actor.transform.localRotation.Y = MathF.Atan2(direction.X, direction.Y) / MathF.PI * 180;
                }
            }

            //grid.GridSpaceToWorldSpace is slow
            actor.transform.localPosition = grid.GridSpaceToWorldSpace(coordinate) 
                - (1 - timeSinceLastMove) * new Vector3(direction.X, 0, direction.Y);


            if (target == null)
            {
                hasReachedTarget = true;
                return;
            }

            timeSinceLastMove += deltaTime * moveSpeed / direction.EuclideanLength;
            if(timeSinceLastMove > 1)
            {
                direction = target.GetRecommendedWithRandomnessDirection(coordinate);
                coordinate += direction;
                hasReachedTarget = direction == Vector2.Zero;
                timeSinceLastMove = 0;
            }
        }
    }
}
