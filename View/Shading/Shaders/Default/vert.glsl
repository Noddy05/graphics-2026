
#version 420 core

layout (location = 0) in vec3 iPosition;

uniform mat4 transformationMatrix = mat4(1);

layout (std140, binding = 1) uniform Camera
{
    mat4 view;
    mat4 projection;
} camera;  

void main(){
    vec3 worldPosition = (transformationMatrix * vec4(iPosition, 1)).xyz;
    
    gl_Position = camera.projection * camera.view * vec4(worldPosition, 1);
}